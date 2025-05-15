using DailyNutrition.Database;
using DailyNutrition.Models;
using System.Collections.ObjectModel;

namespace DailyNutrition.Views;

public partial class ViewsMenuPage : ContentPage
{   
    ObservableCollection<MenuRecord> FoodMenu { get; set; }
    public ViewsMenuPage()
	{
		InitializeComponent();
        FoodMenu = new ObservableCollection<MenuRecord>();
        ViewsMenu.ItemsSource = FoodMenu;
    }

    private async void ViewsMenu_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection[0] != null)
        {
            MenuRecord menus = e.CurrentSelection[0] as MenuRecord;
            App.Current.MainPage = new NavigationPage(new EditMenuPage()
            //await Navigation.PushAsync(new EditMenuPage()
            {
                BindingContext = menus
            });
        }
    }

    private async void MenuSearchBar_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (string.IsNullOrWhiteSpace(e.NewTextValue))
        {
            FoodMenu = new ObservableCollection<MenuRecord>(await App.MenuDatabase.GetAllMenuAsync());
        }
        else
        {
            var filteredMenu = await App.MenuDatabase.SearchMenuAsync(e.NewTextValue);
            FoodMenu = new ObservableCollection<MenuRecord>(filteredMenu);
        }
        ViewsMenu.ItemsSource = FoodMenu;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        LoadMenu();
    }

    private async void LoadMenu()
    {
        FoodMenu = new ObservableCollection<MenuRecord>(await App.MenuDatabase.GetAllMenuAsync());
        ViewsMenu.ItemsSource = FoodMenu;
        OnPropertyChanged(nameof(MenuRecord));
    }

    private async void btnAddMenuPage_Clicked(object sender, EventArgs e)
    {
        App.Current.MainPage = new NavigationPage(new AddMenuPage());
        //await Navigation.PushAsync(new AddMenuPage());
    }
}