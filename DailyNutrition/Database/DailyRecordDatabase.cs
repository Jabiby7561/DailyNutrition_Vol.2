using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DailyNutrition.Models;
using SQLite;

namespace DailyNutrition.Database
{
    public class DailyRecordDatabase
    {
        public SQLiteAsyncConnection _database;

        public async Task Init()
        {
            if (_database != null)
                return;

            // สร้างการเชื่อมต่อกับฐานข้อมูล SQLite
            _database = new SQLiteAsyncConnection(
                Constants.DatabasePath,
                Constants.Flags);

            // สร้างตารางสำหรับ DailyRecord
            var result = await _database.CreateTableAsync<DailyRecord>();
        }

        // ดึงข้อมูลแคลอรี่ที่บันทึกในแต่ละวันทั้งหมดจากฐานข้อมูล
        public async Task<List<DailyRecord>> GetAllDateAsync()
        {
            await Init(); // ตรวจสอบว่าฐานข้อมูลถูกตั้งค่าแล้ว
            return await _database.Table<DailyRecord>().ToListAsync();
        }

        // เพิ่มข้อมูลแคลอรี่ที่บันทึกในแต่ละวันลงในฐานข้อมูล
        public async Task<int> AddDateAsync(DailyRecord daily)
        {
            await Init(); // ตรวจสอบว่าฐานข้อมูลพร้อมใช้งาน
            return await _database.InsertAsync(daily);
        }

    }
}

