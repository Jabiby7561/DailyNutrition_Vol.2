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

    private async void TodayMenu_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        double totalEnergy = 0;
        try 
        {
            foreach (var item in TodayMenu.SelectedItems)
            {
                if (item is ClassMenu menu)
                {
                    totalEnergy += menu.Energy;
                }
            }

            TotalEnergyLabel.Text = $"Total Energy : {totalEnergy} cal";
            return;
        }
        catch (Exception ex)
        {
            // Handle exception
            await DisplayAlert("เกิดข้อผิดพลาด!", $"ไม่สามารถคำนวนแคลอรี่ได้: {ex.Message}", "ตกลง");
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