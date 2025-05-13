using DailyNutrition.Models;
using Microsoft.Maui.Controls;
using System.Collections.ObjectModel;

namespace DailyNutrition.Views;

public partial class TodayMenuPage : ContentPage
{
    ObservableCollection<ClassMenu> FoodMenu { get; set; }
    public TodayMenuPage()
	{
		InitializeComponent();
        FoodMenu = new ObservableCollection<ClassMenu>();
        TodayMenu.ItemsSource = FoodMenu;
    }

    private async void UpdateTotalEnergy()
    {
        try 
        {
            double totalEnergy = 0;
            foreach (var menu in FoodMenu)
            {
                if (menu.Quantity > 0)
                {
                    totalEnergy += menu.Energy * menu.Quantity;
                }
            }
            TotalEnergyLabel.Text = $"พลังงานทั้งหมดที่ได้รับในวันนี้ : {totalEnergy} cal";
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

    private async void btnAddEnergy_Clicked(object sender, EventArgs e)
    {
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
        }
        catch (Exception ex)
        {
            await DisplayAlert("เกิดข้อผิดพลาด!", $"เกิดข้อผิดพลาดในการบันทึกข้อมูล: {ex.Message}", "ตกลง");
        }
    }

}