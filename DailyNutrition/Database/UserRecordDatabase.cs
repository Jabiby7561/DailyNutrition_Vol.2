using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using SQLite;
using DailyNutrition.Models;

namespace DailyNutrition.Database
{
    public class UserRecordDatabase
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

            // สร้างตารางสำหรับ CalculationRecord
            var result = await _database.CreateTableAsync<CalculationRecord>();
        }

        // ดึงบันทึกข้อมูลผู้ใช้ทั้งหมดจากฐานข้อมูล
        public async Task<List<CalculationRecord>> GetAllRecordAsync()
        {
            await Init(); // ตรวจสอบว่าฐานข้อมูลถูกตั้งค่าแล้ว
            return await _database.Table<CalculationRecord>().ToListAsync();
        }

        public async Task<CalculationRecord> GetLatestRecordAsync()
        {
            await Init(); // ตรวจสอบให้แน่ใจว่าฐานข้อมูลถูกเตรียมไว้แล้ว
            return await _database.Table<CalculationRecord>()
                .OrderByDescending(record => record.RecordCreated) // ใช้ RecordCreated แทน Date
                .FirstOrDefaultAsync();
        }

        // เพิ่มบันทึกข้อมูลผู้ใช้ลงในฐานข้อมูล
        public async Task<int> AddCalculationAsync(CalculationRecord record)
        {
            await Init(); // ตรวจสอบว่าฐานข้อมูลพร้อมใช้งาน
            return await _database.InsertAsync(record);
        }

    }
}
