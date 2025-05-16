using DailyNutrition.Models;
using System.Collections.ObjectModel;
using System.Globalization;
using Syncfusion.Maui.Charts;

namespace DailyNutrition.Views;

public partial class ConcludePage : ContentPage
{
    ObservableCollection<DailyRecord> DailyInfo { get; set; } = new();
    public ConcludePage()
	{
		InitializeComponent();
        BindingContext = this;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        var viewData = new ViewData();
        await viewData.LoadDataAsync();
        BindingContext = viewData;
        LoadRecord();
    }

    private async void LoadRecord()
    {
        var allMenuHistory = await App.DailyDatabase.GetAllDateAsync();

        var today = DateTime.Today;
        // คำนวณจุดเริ่มต้นของสัปดาห์ (จันทร์-อาทิตย์)
        int offset = today.DayOfWeek == DayOfWeek.Sunday ? 6 : (int)today.DayOfWeek - 1;
        var startOfWeek = today.AddDays(-offset);
        var endOfWeek = startOfWeek.AddDays(6);

        var thisWeekMenus = allMenuHistory
            .Where(m => m.DateCreated.Date >= startOfWeek && m.DateCreated.Date <= endOfWeek)
            .ToList();

        // ตรวจสอบว่าครบ 7 วันหรือยัง
        if (thisWeekMenus.Count >= 7)
        {
            await DisplayAlert(
                "ครบสัปดาห์แล้ว",
                "บันทึกข้อมูลครบ 7 ครั้งแล้ว อย่าลืมอัปเดตค่าการบริโภคแคลอรี่ของร่างกายในหน้าตั้งค่าด้วยละ",
                "ตกลง");
        }

        float totalProtein = 0, totalCarb = 0, totalFat = 0, totalEnergy = 0;
        int totalQuantity = 0;

        foreach (var record in thisWeekMenus)
        {
            totalProtein += record.TotalProtein;
            totalCarb += record.TotalCarbohydrates;
            totalFat += record.TotalFat;
            totalEnergy += (float)record.DailyEnergy;
            totalQuantity += record.MenuAmuount;
        }

        // เฉลี่ย
        float avgProtein = totalQuantity > 0 ? totalProtein / totalQuantity : 0;
        float avgCarb = totalQuantity > 0 ? totalCarb / totalQuantity : 0;
        float avgFat = totalQuantity > 0 ? totalFat / totalQuantity : 0;
        float avgEnergy = totalQuantity > 0 ? totalEnergy / totalQuantity : 0;

        AvgProteinLabel.Text = $"ค่าเฉลี่ยโปรตีนที่ได้รับ : {avgProtein * 4:F2} cal";
        AvgCarbohydratesLabel.Text = $"ค่าเฉลี่ยคาร์โบไฮเดรตที่ได้รับ : {avgCarb * 4:F2} cal";
        AvgFatLabel.Text = $"ค่าเฉลี่ยไขมันที่ได้รับ : {avgFat * 9:F2} cal";
        AvgRequiredEnergyLabel.Text = $"ค่าเฉลี่ยพลังงานที่ร่างกายได้รับ : {avgEnergy:F2} cal";
    }

}