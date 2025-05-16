using DailyNutrition.Database;

namespace DailyNutrition
{
    public partial class App : Application
    {
        public static NutritionDatabase MenuDatabase;
        public static UserRecordDatabase UserDatabase;
        public static DailyRecordDatabase DailyDatabase;
        public App()
        {
            InitializeComponent();
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("Ngo9BigBOggjHTQxAR8/V1NNaF1cWWhPYVJ1WmFZfVtgdV9HYFZVQ2Y/P1ZhSXxWdkBiXX5dcXdQQGVUVE19XUs=");
            MenuDatabase = new NutritionDatabase();
            UserDatabase = new UserRecordDatabase();
            DailyDatabase = new DailyRecordDatabase();
            MainPage = new Views.TabSimplePage();
        }
    }
}
