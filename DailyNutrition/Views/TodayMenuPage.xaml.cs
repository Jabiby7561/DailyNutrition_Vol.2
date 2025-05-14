using DailyNutrition.Models;
using Microsoft.Maui.Controls;
using System.Collections.ObjectModel;

namespace DailyNutrition.Views;

public partial class TodayMenuPage : ContentPage
{
    public double currentTDEE = 0;
    ObservableCollection<ClassMenu> FoodMenu { get; set; }
    public TodayMenuPage()
	{
		InitializeComponent();
        FoodMenu = new ObservableCollection<ClassMenu>();
        TodayMenu.ItemsSource = FoodMenu;
    }

    private async void UpdateTotalEnergy()
    {
        // หลังจากผู้ใช้กรอกข้อมูลแล้ว ให้ดึงค่า TDEE ล่าสุดมาอัปเดตใน Label
        var latestCalculation = await App.UserDatabase.GetLatestRecordAsync();
        if (latestCalculation != null)
        {
            currentTDEE = latestCalculation.TDEE;
            RequiredEnergyLabel.Text = $"พลังงานที่ร่างกายต้องการ : {latestCalculation.TDEE} cal";
        }

        try 
        {
            // คำนวณพลังงานจากเมนูทั้งหมดที่เลือก
            double totalEnergy = 0;
            foreach (var menu in FoodMenu)
            {
                if (menu.Quantity > 0)
                {
                    totalEnergy += menu.Energy * menu.Quantity;
                }
            }
            TotalEnergyLabel.Text = $"พลังงานทั้งหมดที่ได้รับในวันนี้ : {totalEnergy} cal";
            
            // คำนวณพลังงาน่ส่วนเกิน (ส่วนเกิน = received - required)
            double overEnergy = totalEnergy - (currentTDEE);
            if (overEnergy <= 0)
            {
                overEnergy = 0;
            }
            OverEnergyLabel.Text = $"พลังงานทั้งหมดที่เกินมา : {overEnergy:F2} cal";

            return;
        }
        catch (Exception ex)
        {
            // Handle exception
            await DisplayAlert("เกิดข้อผิดพลาด!", $"ไม่สามารถคำนวนแคลอรี่ได้: {ex.Message}", "ตกลง");
        }
    }

    private async void TodayMenu_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        // สำหรับรายการที่เพิ่งถูกเลือก ให้ตั้งค่า Quantity จาก 0 เป็น 1 
        foreach (var item in e.CurrentSelection)
        {
            if (item is ClassMenu menu)
            {
                menu.IsSelected = true;
                if (menu.Quantity == 0)
                {
                    menu.Quantity = 1;
                }
            }
        }

        // สำหรับรายการที่ถูก deselect ให้ตั้งค่า Quantity จาก 1 เป็น 0 
        var deselectedItems = e.PreviousSelection.Except(e.CurrentSelection);
        foreach (var item in deselectedItems)
        {
            if (item is ClassMenu menu)
            {
                menu.IsSelected = false;
                menu.Quantity = 0;
            }
        }
        UpdateTotalEnergy();
    }

    private void Stepper_ValueChanged(object sender, ValueChangedEventArgs e)
    {
        var stepper = sender as Stepper;
        if (stepper == null) return;

        // ดึงข้อมูลเมนู (ClassMenu) จาก BindingContext ของ Stepper
        var menuItem = stepper.BindingContext as ClassMenu;
        if (menuItem != null)
        {
            // เผื่อกรณี user ลักเลี่ยง event, ตรวจสอบให้แน่ใจว่ารายการถูกเลือกแล้ว
            if (menuItem.IsSelected)
            {
                menuItem.Quantity = (int)e.NewValue;
                UpdateTotalEnergy();
            }
        }
    }

    private async void btnAddEnergy_Clicked(object sender, EventArgs e)
    {
        // ดึงข้อมูล TDEE ล่าสุดจากฐานข้อมูล UserRecordDatabase
        var latestCalculation = await App.UserDatabase.GetLatestRecordAsync();

        // ถ้าไม่มี record หรือตัว TDEE เป็น 0 ให้แจ้งเตือนและนำผู้ใช้ไปกรอกข้อมูลในหน้า DailyCalorieCalculator
        if (latestCalculation == null || latestCalculation.TDEE == 0)
        {
            await DisplayAlert("ข้อมูลไม่ครบ!", "กรุณาไปบันทึกค่าคำนวณพลังงานที่ร่างกายต้องการในหน้าตั้งค่าก่อน", "ตกลง");
        }
        else
        {
            // ถ้ามีค่า TDEE แล้ว ให้อัปเดต Label ด้วยค่า TDEE ล่าสุด
            currentTDEE = latestCalculation.TDEE;
            RequiredEnergyLabel.Text = $"พลังงานที่ร่างกายต้องการ : {latestCalculation.TDEE} cal";
        }

        //---------- ดำเนินการบันทึกข้อมูลการรับประทานอาหารในวันนี้ ----------//

        // คัดกรองรายการเมนูที่ถูกเลือกจาก CollectionView
        var selectedMenus = TodayMenu.SelectedItems.Cast<ClassMenu>().ToList();

        // ตรวจสอบว่ามีเมนูถูกเลือกอยู่หรือไม่
        if (!selectedMenus.Any())
        {
            await DisplayAlert("เกิดข้อผิดพลาด!", "กรุณาเลือกอย่างน้อย 1 เมนู", "ตกลง");
            return;
        }

        // คำนวณพลังงานรวมจากรายการที่ถูกเลือก
        double totalEnergy = selectedMenus.Sum(menu => menu.Energy * menu.Quantity);

        // สร้าง DailyRecord สำหรับบันทึกในฐานข้อมูล
        DailyRecord dailyRecord = new DailyRecord
        {
            DailyEnergy = (float)totalEnergy,
            DateCreated = DateTime.Now
        };

        try
        {
            // บันทึกลงฐานข้อมูลด้วย SQLite
            await App.DailyDatabase.AddDateAsync(dailyRecord);
            await DisplayAlert("สำเร็จ", "บันทึกแคลอรี่ในวันนี้เรียบร้อยแล้ว", "ตกลง");
            TotalEnergyLabel.Text = "พลังงานทั้งหมดที่ได้รับในวันนี้ : 0 cal";
            OverEnergyLabel.Text = $"พลังงานทั้งหมดที่เกินมา : 0 cal";
        }
        catch (Exception ex)
        {
            await DisplayAlert("เกิดข้อผิดพลาด!", $"เกิดข้อผิดพลาดในการบันทึกข้อมูล: {ex.Message}", "ตกลง");
        }
    }

    private async void MenuSearchBar_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (string.IsNullOrWhiteSpace(e.NewTextValue))
        {
            TodayMenu.ItemsSource = new ObservableCollection<ClassMenu>(await App.MenuDatabase.GetAllMenuAsync());
        }
        else
        {
            var filteredMenu = await App.MenuDatabase.SearchMenuAsync(e.NewTextValue);
            TodayMenu.ItemsSource = new ObservableCollection<ClassMenu>(filteredMenu);
        }
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        LoadMenu();
    }

    private async void LoadMenu()
    {
        FoodMenu = new ObservableCollection<ClassMenu>(await App.MenuDatabase.GetAllMenuAsync());
        TodayMenu.ItemsSource = FoodMenu;
        OnPropertyChanged(nameof(ClassMenu));
    }

}