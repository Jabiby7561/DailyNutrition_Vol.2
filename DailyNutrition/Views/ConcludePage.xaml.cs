using DailyNutrition.Models;
using System.Collections.ObjectModel;

namespace DailyNutrition.Views;

public partial class ConcludePage : ContentPage
{
    ObservableCollection<CalculationRecord> RecordInfo { get; set; }
    public ConcludePage()
	{
		InitializeComponent();
        RecordInfo = new ObservableCollection<CalculationRecord>();
        ViewsRecord.ItemsSource = RecordInfo;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        LoadMenu();
    }

    private async void LoadMenu()
    {
        RecordInfo = new ObservableCollection<CalculationRecord>(await App.UserDatabase.GetAllRecordAsync());
        ViewsRecord.ItemsSource = RecordInfo;
        OnPropertyChanged(nameof(CalculationRecord));
    }

    private void btnDeleteDatabase_Clicked(object sender, EventArgs e)
    {
        var dbPath = Path.Combine(FileSystem.AppDataDirectory, "NutritionApp.db3");
        if (File.Exists(dbPath))
            File.Delete(dbPath);
    }
}