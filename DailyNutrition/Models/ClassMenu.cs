using System;
using System.ComponentModel;
using System.Xml.Linq;
using SQLite;

namespace DailyNutrition.Models
{
    public class ClassMenu : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        //public ClassMenu()
        //{
        //    // Constructor
        //}
        //public ClassMenu(string name, float protein, float carbohydrates, float fat)
        //{
        //    Name = name;
        //    Protein = protein;
        //    Carbohydrates = carbohydrates;
        //    Fat = fat;
        //}
    
        [PrimaryKey, AutoIncrement]
        public int MenuId { get; set; }
        public string Name { get; set; }
        public float Protein { get; set; }
        public float Carbohydrates { get; set; }
        public float Fat { get; set; }
        public string ImagePath { get; set; }

        public string MenuName
        {
            get => Name;
            set { Name = value; OnPropertyChanged(nameof(MenuName)); OnPropertyChanged(nameof(Energy)); }
        }

        public float MenuProtein
        {
            get => Protein;
            set { Protein = value; OnPropertyChanged(nameof(MenuProtein)); OnPropertyChanged(nameof(Energy)); }
        }

        public float MenuCarbohydrates
        {
            get => Carbohydrates;
            set { Carbohydrates = value; OnPropertyChanged(nameof(MenuCarbohydrates)); OnPropertyChanged(nameof(Energy)); }
        }

        public float MenuFat
        {
            get => Fat;
            set { Fat = value; OnPropertyChanged(nameof(MenuFat)); OnPropertyChanged(nameof(Energy)); }
        }

        public float Energy => CalculateEnergy();
        public float CalculateEnergy()
        {
            // สมมติว่าพลังงาน = โปรตีน * 4 + คาร์โบไฮเดรต * 4 + ไขมัน * 9
            return Protein * 4 + Carbohydrates * 4 + Fat * 9;
        }

        //public override string ToString()
        //{
        //    return $"เมนู: {Name}\n" +
        //           $"โปรตีน: {Protein}g\n" +
        //           $"คาร์โบไฮเดรต: {Carbohydrates}g\n" +
        //           $"ไขมัน: {Fat}g\n" +
        //           $"พลังงานทั้งหมด: {CalculateEnergy()} กิโลแคลอรี่";
        //}
    }
}
