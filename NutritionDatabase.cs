using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using SQLite;
using DailyNutrition;

namespace DailyNutrition
{
    public class NutritionDatabase
    {
        private SQLiteAsyncConnection _database;

        private async Task Init()
        {
            if (_database != null)
                return;

            // สร้างการเชื่อมต่อกับฐานข้อมูล SQLite
            var dbPath = Path.Combine(FileSystem.AppDataDirectory, "NutritionApp.db");
            _database = new SQLiteAsyncConnection(dbPath);

            // สร้างตารางสำหรับ ClassMenu
            await _database.CreateTableAsync<ClassMenu>();
        }

        // เพิ่มเมนูใหม่ลงในฐานข้อมูล
        public async Task<int> AddMenuAsync(ClassMenu menu)
        {
            await Init(); // ตรวจสอบว่าฐานข้อมูลถูกตั้งค่าแล้ว
            return await _database.InsertAsync(menu);
        }

        // ดึงข้อมูลเมนูทั้งหมดจากฐานข้อมูล
        public async Task<List<ClassMenu>> GetAllMenusAsync()
        {
            await Init(); // ตรวจสอบว่าฐานข้อมูลถูกตั้งค่าแล้ว
            return await _database.Table<ClassMenu>().ToListAsync();
        }

        // ลบข้อมูลเมนูออกจากฐานข้อมูล
        public async Task<int> DeleteMenuAsync(int id)
        {
            await Init(); // ตรวจสอบว่าฐานข้อมูลถูกตั้งค่าแล้ว
            return await _database.DeleteAsync<ClassMenu>(id);
        }
    }
}
