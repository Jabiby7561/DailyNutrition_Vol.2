namespace DailyNutrition.Views;

public partial class TabSimplePage : TabbedPage
{
	public TabSimplePage()
	{
		InitializeComponent();
	}

    private void btnProfileSetting_Clicked(object sender, EventArgs e)
    {
        App.Current.MainPage = new NavigationPage(new DailyCalorieCalculator());
    }
}