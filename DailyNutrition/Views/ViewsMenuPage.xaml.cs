using DailyNutrition.Database;
using DailyNutrition.Models;
using System.Collections.ObjectModel;

namespace DailyNutrition.Views;

public partial class ViewsMenuPage : ContentPage
{   
    ObservableCollection<ClassMenu> FoodMenu { get; set; }
    public ViewsMenuPage()
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
            //App.Current.MainPage = new NavigationPage(new EditMenuPage()
            await Navigation.PushAsync(new EditMenuPage()
            {
                BindingContext = menus
            });
        }
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        LoadNotes();
    }

    private async void LoadNotes()
    {
        FoodMenu = new ObservableCollection<ClassMenu>(await App.MenuDatabase.GetAllMenuAsync());
        ViewsMenu.ItemsSource = FoodMenu;
        OnPropertyChanged(nameof(ClassMenu));
    }

    private async void btnAddMenuPage_Clicked(object sender, EventArgs e)
    {
        //App.Current.MainPage = new NavigationPage(new AddMenuPage());
        await Navigation.PushAsync(new AddMenuPage());
    }

    //private async void btnEditMenu_Clicked(object sender, EventArgs e)
    //{
    //    if (sender is Button button && button.CommandParameter is ClassMenu menus)
    //    {
    //        await Navigation.PushAsync(new EditMenuPage()
    //        {
    //            BindingContext = menus
    //        });
    //    }
    //}
}