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
        public float DailyEnergy { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
