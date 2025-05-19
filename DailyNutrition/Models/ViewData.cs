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
        public List<DailyRecord> DailyView { get; set; } = new();

        public ViewData()
        {
            LoadDataAsync();
        }

        public async Task LoadDataAsync()
        {
            var allRecords = await App.DailyDatabase.GetAllDateAsync();
            var latestUser = await App.UserDatabase.GetLatestRecordAsync();

            if (allRecords == null || latestUser == null)
            {
                DailyView = new List<DailyRecord>();
                return;
            }

            // เรียงตามวันที่ล่าสุด -> เก่าสุด แล้วเลือก 5 รายการล่าสุด
            var latestRecords = allRecords
                .Where(r => r.UserRecordId == latestUser.RecordId)
                .OrderByDescending(r => r.DateCreated)
                .Take(7)
                .OrderBy(r => r.DateCreated)
                .ToList();

            foreach (var record in latestRecords) //recentRecords
            {
                record.DateLabel = record.DateCreated.ToString("yyyy-MM-dd");
            }

            DailyView = latestRecords; //recentRecords
        }

    }
}
