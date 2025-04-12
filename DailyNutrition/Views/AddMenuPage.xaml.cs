using System;
using DailyNutrition.Database;
using DailyNutrition.Models;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Storage;

namespace DailyNutrition.Views;

public partial class AddMenuPage : ContentPage
{
    public AddMenuPage()
    {
        InitializeComponent();
        BindingContext = new ClassMenu();
    }

    // ปุ่มเพิ่มเมนู
    private async void btnAddMenu_Clicked(object sender, EventArgs e)
    {
        var newMenu = (ClassMenu)BindingContext;

        // ตรวจสอบชื่อเมนู
        if (string.IsNullOrEmpty(newMenu.Name))
        {
            await DisplayAlert("เกิดข้อผิดพลาด!", "กรุณากรอกชื่อเมนู", "ตกลง");
            return;
        }

        // ตรวจสอบข้อมูลโภชนาการ
        if (!float.TryParse(newMenu.Protein.ToString(), out float protein) ||
            !float.TryParse(newMenu.Carbohydrates.ToString(), out float carbs) ||
            !float.TryParse(newMenu.Fat.ToString(), out float fat))
        {
            await DisplayAlert("เกิดข้อผิดพลาด!", "กรุณากรอกข้อมูลโภชนาการที่ถูกต้อง", "ตกลง");
            return;
        }

        // บันทึกลงฐานข้อมูล
        await App.MenuDatabase.AddMenuAsync(newMenu);
        await DisplayAlert("สำเร็จ", "เมนูถูกบันทึกในฐานข้อมูลเรียบร้อยแล้ว!", "ตกลง");

        App.Current.MainPage = new TabSimplePage();
        //await Navigation.PopAsync();
    }

    // อัปโหลดรูปภาพ
    private async void UploadImageButton_Clicked(object sender, EventArgs e)
    {
        var picMenu = (ClassMenu)BindingContext;
        try
        {
            var result = await FilePicker.Default.PickAsync(new PickOptions
            {
                PickerTitle = "เลือกภาพอาหาร",
                FileTypes = FilePickerFileType.Images
            });

            if (result != null)
            {
                var stream = await result.OpenReadAsync();
                FoodImage.Source = ImageSource.FromStream(() => stream);

                // บันทึกเส้นทางไฟล์รูปใน picMenu
                picMenu.ImagePath = result.FullPath;
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("เกิดข้อผิดพลาด!", $"ไม่สามารถอัปโหลดรูปได้: {ex.Message}", "ตกลง");
        }
    }

    // ปุ่มย้อนกลับ
    private async void btnBack_Clicked(object sender, EventArgs e)
    {
        App.Current.MainPage = new TabSimplePage();
        //await Navigation.PopAsync();
    }
}