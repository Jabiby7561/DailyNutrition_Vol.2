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
            await Navigation.PushAsync(new AddMenuPage
            {
                BindingContext = menus
            });
        }
    }
    private void btnAddMenu_Clicked(object sender, EventArgs e)
    {
        App.Current.MainPage = new NavigationPage(new AddMenuPage());
        //await Navigation.PushAsync(new AddMenuPage());
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        LoadNotes();
    }

    private async void LoadNotes()
    {
        FoodMenu = new ObservableCollection<ClassMenu>(); // เดิมทีใน()ข้างๆ ClassMenu มี await App.Database.GetNotesAsync(App.User.ID)
        ViewsMenu.ItemsSource = FoodMenu;
        OnPropertyChanged(nameof(FoodMenu));
    }
}