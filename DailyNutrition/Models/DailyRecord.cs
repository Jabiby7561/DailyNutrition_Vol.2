using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DailyNutrition.Models
{
    public class DailyRecord
    {
        [PrimaryKey, AutoIncrement]
        public int DateId { get; set; }
        public float TotalProtein { get; set; }
        public float TotalCarbohydrates { get; set; }
        public float TotalFat { get; set; }
        public double DailyEnergy { get; set; }
        public int MenuAmuount { get; set; }
        public DateTime DateCreated { get; set; }
        public string DateLabel { get; set; }
    }
}
