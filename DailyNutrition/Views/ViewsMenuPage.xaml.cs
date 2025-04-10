namespace DailyNutrition.Views;

public partial class ViewsMenuPage : ContentPage
{
	public ViewsMenuPage()
	{
		InitializeComponent();
	}

    private void btnAddMenu_Clicked(object sender, EventArgs e)
    {
        App.Current.MainPage = new NavigationPage(new AddMenuPage());
    }
}