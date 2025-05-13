using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Xml.Linq;
using SQLite;

namespace DailyNutrition.Models
{
    public class ClassMenu : INotifyPropertyChanged
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

        // เพิ่ม property Quantity ซึ่งเริ่มต้นเป็น 0
        private int quantity;
        public int Quantity
        {
            get => quantity;
            set
            {
                if (quantity != value)
                {
                    quantity = value;
                    OnPropertyChanged();
                }
            }
        }

        private bool isSelected;
        public bool IsSelected
        {
            get => isSelected;
            set
            {
                if (isSelected != value)
                {
                    isSelected = value;
                    OnPropertyChanged();
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string dummy = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(dummy));
        }

    }
}
