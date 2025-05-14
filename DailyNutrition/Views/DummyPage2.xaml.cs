using DailyNutrition.Models;
using System.Collections.ObjectModel;

namespace DailyNutrition.Views;

public partial class DummyPage2 : ContentPage
{
    ObservableCollection<DailyRecord> DailyInfo { get; set; }
    public DummyPage2()
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