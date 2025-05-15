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
            if (allRecords == null || allRecords.Count == 0)
                return;

            var latestDate = allRecords.Max(r => r.DateCreated);
            var startOfWeek = latestDate.Date.AddDays(-6);

            var recentRecords = allRecords
                .Where(r => r.DateCreated.Date >= startOfWeek && r.DateCreated.Date <= latestDate.Date)
                .OrderBy(r => r.DateCreated)
                .ToList();

            foreach (var record in recentRecords)
            {
                record.DateLabel = record.DateCreated.ToString("yyyy-MM-dd");
            }

            DailyView = recentRecords;
        }

    }
}
