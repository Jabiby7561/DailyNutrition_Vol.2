using DailyNutrition.Views;
using Microsoft.Maui.Storage;
using System;

namespace DailyNutrition
{
    public partial class DailyCalorieCalculator : ContentPage
    {
        private string selectedGender;
        public DailyCalorieCalculator()
        {
            InitializeComponent();
            BindingContext = new UserRecord();
        }

        // ฟังก์ชันเมื่อกดปุ่มเลือกเพศ
        private void OnGenderSelected(object sender, EventArgs e)
        {
            var button = (Button)sender;

            // กำหนดสีปุ่มที่เลือกเป็นสีหลัก และเปลี่ยนสีปุ่มที่ไม่ได้เลือกเป็นสีพื้นฐาน
            if (button.Text == "ชาย")
            {
                selectedGender = "ชาย";
                MaleButton.BackgroundColor = Color.FromArgb("#007AFF"); // สีน้ำเงิน
                MaleButton.TextColor = Colors.White;
                FemaleButton.BackgroundColor = Colors.LightGray;
                FemaleButton.TextColor = Colors.Black;
            }
            else if (button.Text == "หญิง")
            {
                selectedGender = "หญิง";
                FemaleButton.BackgroundColor = Color.FromArgb("#007AFF"); // สีน้ำเงิน
                FemaleButton.TextColor = Colors.White;
                MaleButton.BackgroundColor = Colors.LightGray;
                MaleButton.TextColor = Colors.Black;
            }

            // คำนวณผลลัพธ์ใหม่
            CalculateBMRAndTDEE();
        }

        // ฟังก์ชันที่ถูกเรียกเมื่อกรอกข้อมูลเปลี่ยนแปลง
        private void OnInputChanged(object sender, EventArgs e)
        {
            CalculateBMRAndTDEE();
        }

        // ฟังก์ชันคำนวณ BMR และ TDEE
        private void CalculateBMRAndTDEE()
        {
            // ตรวจสอบข้อมูลว่าครบถ้วนหรือไม่
            if (!float.TryParse(WeightEntry.Text, out float weight) ||
                !float.TryParse(HeightEntry.Text, out float height) ||
                !float.TryParse(AgeEntry.Text, out float age) ||
                string.IsNullOrEmpty(selectedGender))
            {
                BmrResultLabel.Text = "BMR: .......................";
                TdeeResultLabel.Text = "TDEE: .......................";
                return;
            }

            // คำนวณค่า BMR ตามเพศ
            float bmr = selectedGender == "ชาย"
                ? 66 + (13.7f * weight) + (5f * height) - (6.8f * age)
                : 655 + (9.6f * weight) + (1.8f * height) - (4.7f * age);

            BmrResultLabel.Text = $"BMR: {bmr:F2} kcal/day";

            // ตรวจสอบระดับกิจกรรมสำหรับการคำนวณ TDEE
            if (ActivityPicker.SelectedItem == null)
            {
                TdeeResultLabel.Text = "TDEE: .......................";
                return;
            }

            // ดึงค่าของระดับกิจกรรม
            string activityLevel = ActivityPicker.SelectedItem.ToString();
            float tdeeMultiplier = activityLevel switch
            {
                "ไม่ออกกำลังกาย" => 1.2f,
                "ออกกำลังกายเบา ๆ (1-2 ครั้ง/สัปดาห์)" => 1.375f,
                "ออกกำลังกายปานกลาง (3-5 ครั้ง/สัปดาห์)" => 1.55f,
                "ออกกำลังกายหนัก (6-7 ครั้ง/สัปดาห์)" => 1.725f,
                "ออกกำลังกายหนักมาก (ทุกวัน, อาชีพแรงงาน)" => 1.9f,
                _ => 1.0f
            };

            // คำนวณค่า TDEE
            float tdee = bmr * tdeeMultiplier;
            TdeeResultLabel.Text = $"TDEE: {tdee:F2} kcal/day";
        }

        private async void btnSaveProfile_Clicked(object sender, EventArgs e)
        {
            // ตรวจสอบว่าข้อมูลครบถ้วนก่อนบันทึก
            if (!float.TryParse(WeightEntry.Text, out float weight) ||
                !float.TryParse(HeightEntry.Text, out float height) ||
                !float.TryParse(AgeEntry.Text, out float age) ||
                string.IsNullOrEmpty(selectedGender) ||
                ActivityPicker.SelectedItem == null)
            {
                await DisplayAlert("ข้อผิดพลาด", "กรุณากรอกข้อมูลให้ครบถ้วนก่อนบันทึก", "ตกลง");
                return;
            }

            // ตรวจสอบค่า BMR และ TDEE
            if (!float.TryParse(BmrResultLabel.Text.Replace("BMR: ", "").Replace(" kcal/day", ""), out float bmr) ||
                !float.TryParse(TdeeResultLabel.Text.Replace("TDEE: ", "").Replace(" kcal/day", ""), out float tdee))
            {
                await DisplayAlert("ข้อผิดพลาด", "ผลลัพธ์ BMR/TDEE ไม่ถูกต้อง", "ตกลง");
                return;
            }

            // สร้างรายการข้อมูลใหม่
            var calculation = new UserRecord
            {
                Gender = selectedGender,
                Weight = weight,
                Height = height,
                Age = age,
                BMR = bmr,
                TDEE = tdee,
                ActivityLevel = ActivityPicker.SelectedItem.ToString(),
                RecordCreated = DateTime.Now
            };

            // บันทึกลงฐานข้อมูล
            await App.UserDatabase.AddRecordAsync(calculation);
            // ลบข้อมูลในตาราง DailyRecord เพื่อล้างกราฟ
            await App.DailyDatabase.DeleteDateAsync();


            await DisplayAlert("สำเร็จ", "บันทึกข้อมูลสำเร็จ!", "ตกลง");

            App.Current.MainPage = new TabSimplePage();
        }

        private async void btnBack_Clicked(object sender, EventArgs e)
        {
            App.Current.MainPage = new TabSimplePage();
            //await Navigation.PopAsync();
        }

        private void btnDeleteDatabase_Clicked(object sender, EventArgs e)
        {
            var dbPath = Path.Combine(FileSystem.AppDataDirectory, "NutritionApp.db3");
            if (File.Exists(dbPath))
                File.Delete(dbPath);
        }
    }
}
