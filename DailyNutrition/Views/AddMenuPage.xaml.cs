using System;
using DailyNutrition.Database;
using DailyNutrition.Models;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Storage;

namespace DailyNutrition.Views;

public partial class AddMenuPage : ContentPage
{
    // ประกาศ Field newMenu ที่นี่
    private ClassMenu newMenu = new ClassMenu();
    private NutritionDatabase _database = new NutritionDatabase();

    public AddMenuPage()
    {
        InitializeComponent();
    }

    private async void AddMenuButton_Clicked(object sender, EventArgs e)
    {
        string name = MenuName.Text;

        // ตรวจสอบชื่อเมนู
        if (string.IsNullOrEmpty(name))
        {
            await DisplayAlert("ข้อผิดพลาด", "กรุณากรอกชื่อเมนู", "ตกลง");
            return;
        }

        // ตรวจสอบข้อมูลโภชนาการ
        if (!float.TryParse(Protein.Text, out float protein) ||
            !float.TryParse(Carbohydrates.Text, out float carbs) ||
            !float.TryParse(Fat.Text, out float fat))
        {
            await DisplayAlert("ข้อผิดพลาด", "กรุณากรอกข้อมูลโภชนาการที่ถูกต้อง", "ตกลง");
            return;
        }

        // สร้างเมนูใหม่
        var newMenu = new ClassMenu
        {
            Name = name,
            Protein = protein,
            Carbohydrates = carbs,
            Fat = fat,
            ImagePath = "" // ใส่เส้นทางรูปภาพหากต้องการ
        };

        // บันทึกลงฐานข้อมูล
        await _database.AddMenuAsync(newMenu);

        await DisplayAlert("สำเร็จ", "เมนูถูกบันทึกในฐานข้อมูลเรียบร้อยแล้ว!", "ตกลง");
    }

    // Event: อัปโหลดรูปภาพ
    private async void UploadImageButton_Clicked(object sender, EventArgs e)
    {
        try
        {
            var result = await FilePicker.PickAsync(new PickOptions
            {
                PickerTitle = "เลือกภาพอาหาร",
                FileTypes = FilePickerFileType.Images
            });

            if (result != null)
            {
                var stream = await result.OpenReadAsync();
                FoodImage.Source = ImageSource.FromStream(() => stream);

                // บันทึกเส้นทางไฟล์รูปใน newMenu
                newMenu.ImagePath = result.FullPath;
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("ข้อผิดพลาด", $"ไม่สามารถอัปโหลดรูปได้: {ex.Message}", "ตกลง");
        }
    }

    private void btnBack_Clicked(object sender, EventArgs e)
    {
        App.Current.MainPage = new TabSimplePage();
    }
}