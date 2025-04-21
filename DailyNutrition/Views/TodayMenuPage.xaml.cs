using DailyNutrition.Models;
using System.Collections.ObjectModel;

namespace DailyNutrition.Views;

public partial class TodayMenuPage : ContentPage
{
    ObservableCollection<ClassMenu> FoodMenu { get; set; }
    public TodayMenuPage()
	{
		InitializeComponent();
        FoodMenu = new ObservableCollection<ClassMenu>();
        ViewsMenu.ItemsSource = FoodMenu;
    }

    private async void ViewsMenu_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection[0] != null)
        {
            ClassMenu menus = e.CurrentSelection[0] as ClassMenu;
            App.Current.MainPage = new NavigationPage(new EditMenuPage()
            //await Navigation.PushAsync(new EditMenuPage()
            {
                BindingContext = menus
            });
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
        ViewsMenu.ItemsSource = FoodMenu;
        OnPropertyChanged(nameof(ClassMenu));
    }
}