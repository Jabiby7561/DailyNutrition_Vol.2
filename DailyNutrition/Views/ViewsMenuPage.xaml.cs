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
    }

    private void btnAddMenu_Clicked(object sender, EventArgs e)
    {
        App.Current.MainPage = new NavigationPage(new AddMenuPage());
    }
}