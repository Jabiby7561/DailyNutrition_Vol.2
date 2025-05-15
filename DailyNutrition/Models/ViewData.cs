using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DailyNutrition.Models
{
    public class ViewData
    {
        public List<DailyRecord> DailyView { get; set; } = new List<DailyRecord>();

        //public ViewData() 
        //{
        //    DailyView = new List<DailyRecord>() 
        //    {
        //        new DailyRecord
        //        {
        //            DateId = 1,
        //            DateLabel = "2023-10-01",
        //            DailyEnergy = 2000,
        //            DateCreated = DateTime.Now
        //        },
        //        new DailyRecord
        //        {
        //            DateId = 2,
        //            DateLabel = "2023-10-02",
        //            DailyEnergy = 2500,
        //            DateCreated = DateTime.Now
        //        },
        //        new DailyRecord
        //        {
        //            DateId = 3,
        //            DateLabel = "2023-10-03",
        //            DailyEnergy = 1800,
        //            DateCreated = DateTime.Now
        //        },
        //        new DailyRecord 
        //        {
        //            DateId = 4,
        //            DateLabel = "2023-10-04",
        //            DailyEnergy = 2200,
        //            DateCreated = DateTime.Now
        //        },
        //        new DailyRecord
        //        {
        //            DateId = 5,
        //            DateLabel = "2023-10-05",
        //            DailyEnergy = 2100,
        //            DateCreated = DateTime.Now
        //        },
        //    };

    }
    
}
