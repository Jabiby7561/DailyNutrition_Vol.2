using System;
using System.ComponentModel;
using System.Xml.Linq;
using SQLite;

namespace DailyNutrition.Models
{
    public class ClassMenu 
    {
        [PrimaryKey, AutoIncrement]
        public int MenuId { get; set; }
        public string Name { get; set; }
        public float Protein { get; set; }
        public float Carbohydrates { get; set; }
        public float Fat { get; set; }
        public string ImagePath { get; set; }

        public float Energy => CalculateEnergy();
        
        public float CalculateEnergy()
        {
            // สมมติว่าพลังงาน = โปรตีน * 4 + คาร์โบไฮเดรต * 4 + ไขมัน * 9
            return Protein * 4 + Carbohydrates * 4 + Fat * 9;
        }
    }
}
