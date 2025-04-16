namespace DailyNutrition.Views;

public partial class ConcludePage : ContentPage
{
	public ConcludePage()
	{
		InitializeComponent();
	}

    private void btnDeleteDatabase_Clicked(object sender, EventArgs e)
    {
        var dbPath = Path.Combine(FileSystem.AppDataDirectory, "NutritionApp.db3");
        if (File.Exists(dbPath))
            File.Delete(dbPath);
    }
}