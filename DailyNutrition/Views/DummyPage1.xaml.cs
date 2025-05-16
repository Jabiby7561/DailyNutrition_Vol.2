using System.Collections.ObjectModel;

namespace DailyNutrition.Views;

public partial class DummyPage1 : ContentPage
{
    ObservableCollection<UserRecord> RecordInfo { get; set; }
    public DummyPage1()
	{
		InitializeComponent();
        RecordInfo = new ObservableCollection<UserRecord>();
        ViewsRecord.ItemsSource = RecordInfo;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        LoadRecord();
    }

    private async void LoadRecord()
    {
        RecordInfo = new ObservableCollection<UserRecord>(await App.UserDatabase.GetAllRecordAsync());
        ViewsRecord.ItemsSource = RecordInfo;
        OnPropertyChanged(nameof(UserRecord));
    }
}