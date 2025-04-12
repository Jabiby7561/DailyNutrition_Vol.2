using DailyNutrition.Database;
using DailyNutrition.Models;

namespace DailyNutrition.Views;

public partial class EditMenuPage : ContentPage
{
    public EditMenuPage()
	{
		InitializeComponent();
        BindingContext = new ClassMenu();
    }
    private async void btnSaveMenu_Clicked(object sender, EventArgs e)
    {
        var saveMenu = (ClassMenu)BindingContext;

        // ตรวจสอบชื่อเมนู
        if (string.IsNullOrEmpty(saveMenu.Name))
        {
            await DisplayAlert("เกิดข้อผิดพลาด!", "กรุณากรอกชื่อเมนู", "ตกลง");
            return;
        }

        // ตรวจสอบข้อมูลโภชนาการ
        if (!float.TryParse(saveMenu.Protein.ToString(), out float protein) ||
            !float.TryParse(saveMenu.Carbohydrates.ToString(), out float carbs) ||
            !float.TryParse(saveMenu.Fat.ToString(), out float fat))
        {
            await DisplayAlert("เกิดข้อผิดพลาด!", "กรุณากรอกข้อมูลโภชนาการที่ถูกต้อง", "ตกลง");
            return;
        }

        // บันทึกลงฐานข้อมูล
        await App.MenuDatabase.AddMenuAsync(saveMenu);
        await DisplayAlert("สำเร็จ", "เมนูถูกบันทึกในฐานข้อมูลเรียบร้อยแล้ว!", "ตกลง");

        App.Current.MainPage = new TabSimplePage();
        //await Navigation.PopAsync();
    }

    private async void btnDeleteMenu_Clicked(object sender, EventArgs e)
    {
        var deleteMenu = (ClassMenu)BindingContext;
        await App.MenuDatabase.DeleteMenuAsync(deleteMenu);
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

    private async void btnBack_Clicked(object sender, EventArgs e)
    {
        App.Current.MainPage = new TabSimplePage();
        //await Navigation.PopAsync();
    }
}