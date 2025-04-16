using DailyNutrition.Database;

namespace DailyNutrition
{
    public partial class App : Application
    {
        public static NutritionDatabase MenuDatabase;
        public static RecordDatabase UserDatabase;
        public App()
        {
            InitializeComponent();
            MenuDatabase = new NutritionDatabase();
            UserDatabase = new RecordDatabase();
            MainPage = new Views.TabSimplePage();
        }
    }
}
