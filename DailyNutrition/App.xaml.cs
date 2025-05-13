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
            MenuDatabase = new NutritionDatabase();
            UserDatabase = new UserRecordDatabase();
            DailyDatabase = new DailyRecordDatabase();
            MainPage = new Views.TabSimplePage();
        }
    }
}
