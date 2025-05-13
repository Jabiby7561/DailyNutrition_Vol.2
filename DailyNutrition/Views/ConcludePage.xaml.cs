using DailyNutrition.Models;
using System.Collections.ObjectModel;

namespace DailyNutrition.Views;

public partial class ConcludePage : ContentPage
{
    ObservableCollection<DailyRecord> DailyInfo { get; set; }
    public ConcludePage()
	{
		InitializeComponent();
        DailyInfo = new ObservableCollection<DailyRecord>();
        ViewsRecord.ItemsSource = DailyInfo;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        LoadRecord();
    }

    private async void LoadRecord()
    {
        DailyInfo = new ObservableCollection<DailyRecord>(await App.DailyDatabase.GetAllDateAsync());
        ViewsRecord.ItemsSource = DailyInfo;
        OnPropertyChanged(nameof(DailyRecord));
    }
}