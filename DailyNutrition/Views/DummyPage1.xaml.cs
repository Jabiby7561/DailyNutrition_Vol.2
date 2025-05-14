using System.Collections.ObjectModel;

namespace DailyNutrition.Views;

public partial class DummyPage1 : ContentPage
{
    ObservableCollection<CalculationRecord> RecordInfo { get; set; }
    public DummyPage1()
	{
		InitializeComponent();
        RecordInfo = new ObservableCollection<CalculationRecord>();
        ViewsRecord.ItemsSource = RecordInfo;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        LoadRecord();
    }

    private async void LoadRecord()
    {
        RecordInfo = new ObservableCollection<CalculationRecord>(await App.UserDatabase.GetAllRecordAsync());
        ViewsRecord.ItemsSource = RecordInfo;
        OnPropertyChanged(nameof(CalculationRecord));
    }
}