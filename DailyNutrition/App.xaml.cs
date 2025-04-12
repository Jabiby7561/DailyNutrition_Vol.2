using DailyNutrition.Database;

namespace DailyNutrition
{
    public partial class App : Application
    {
        public static NutritionDatabase MenuDatabase;
        public App()
        {
            InitializeComponent();
            MenuDatabase = new NutritionDatabase();
            MainPage = new Views.TabSimplePage();
        }
    }
}
