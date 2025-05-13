using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;

namespace DailyNutrition
{
    public class CalculationRecord
    {
        [PrimaryKey, AutoIncrement]
        public int RecordId { get; set; }
        public string Gender { get; set; }
        public float Weight { get; set; }
        public float Height { get; set; }
        public float Age { get; set; }
        public float BMR { get; set; }
        public float TDEE { get; set; }
        public string ActivityLevel { get; set; }
        public DateTime RecordCreated { get; set; }
    }
}
