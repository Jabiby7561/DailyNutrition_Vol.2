using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using SQLite;
using DailyNutrition.Models;

namespace DailyNutrition.Database
{
    // Download Plug-in อีก 2 ตัว
    // 1. sqlite-net-pcl : Version 1.9.172
    // 2. SQLitePCLRaw.bundle_green : 2.1.11
    public class NutritionDatabase
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

            // สร้างตารางสำหรับ ClassMenu
            var result = await _database.CreateTableAsync<ClassMenu>();
        }

        // ดึงข้อมูลเมนูทั้งหมดจากฐานข้อมูล
        public async Task<List<ClassMenu>> GetAllMenuAsync()
        {
            await Init(); // ตรวจสอบว่าฐานข้อมูลถูกตั้งค่าแล้ว
            return await _database.Table<ClassMenu>().ToListAsync();
        }

        //public async Task<ClassMenu> GetMenuAsync(int menuid)
        //{
        //    await Init();
        //    return await _database.Table<ClassMenu>()
        //                          .Where(n => n.MenuId == menuid)
        //                          .FirstOrDefaultAsync();
        //}

        // อัปเดตเมนูในฐานข้อมูล
        public async Task<int> AddMenuAsync(ClassMenu menu)
        {
            await Init(); // ตรวจสอบว่าฐานข้อมูลถูกตั้งค่าแล้ว
            if (menu.MenuId != 0)
            {
                return await _database.UpdateAsync(menu);
            }
            else
            {
                return await _database.InsertAsync(menu);
            }
        }

        // ลบข้อมูลเมนูออกจากฐานข้อมูล
        public async Task<int> DeleteMenuAsync(ClassMenu id)
        {
            await Init(); // ตรวจสอบว่าฐานข้อมูลถูกตั้งค่าแล้ว
            return await _database.DeleteAsync(id);
        }
    }
}
