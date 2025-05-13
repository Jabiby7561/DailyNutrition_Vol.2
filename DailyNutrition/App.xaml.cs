using DailyNutrition.Database;

namespace DailyNutrition
{
    public partial class App : Application
    {
        public static NutritionDatabase MenuDatabase;
        public static UserRecordDatabase UserDatabase;
        public App()
        {
            InitializeComponent();
            MenuDatabase = new NutritionDatabase();
            UserDatabase = new UserRecordDatabase();
            MainPage = new Views.TabSimplePage();
        }
    }
}
