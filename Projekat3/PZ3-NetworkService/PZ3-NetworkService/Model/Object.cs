using PZ3_NetworkService.ViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PZ3_NetworkService.Model
{
    [Serializable]
    public class Object : ValidationBase
    {
        private int id;
        private string name;
        private Type type;
        private double value;
        int parsedValue;


        #region props

        public double Value
        {
            get { return value; }
            set
            {
                if (this.value != value)
                {
                    this.value = value;
                    OnPropertyChanged("Value");
                }
            }
        }

        public int Id { get => id; set => id = value; }
        public string Name { get => name; set => name = value; }
        public Type Type { get => type; set => type = value; }
        #endregion


        public Object(int i=0, string n="", string tn="", string img_src="", double v = 0)
        {
            Id = i;
            Name = n;
            var tip = new Type { Name = tn, Img = img_src };
            Type = tip;
            Value = v;
        }

     

        protected override void ValidateSelf()
        {
            //ovde treba validaciju uraditi
            
            if(MainWindowViewModel.Objekti.Any(p=>p.Id == id))
            {
                this.ValidationErrors["Id"] = $"Object with id : {id} already exists.";
            }else if (id <= 0)
            {
                this.ValidationErrors["Id"] = "Id of object is not valid number.";
            }

            if (string.IsNullOrWhiteSpace(name))
            {
                this.ValidationErrors["Name"] = "Name is required.";
            }else if(int.TryParse(name, out parsedValue))
            {
                this.ValidationErrors["Name"] = "Name can not be number";
            }
        }

        public override string ToString()
        {
            return "ID: " + Id + "\nName: " + Name + "\nType: " + Type.Name+
                "\nValue:" + Value+Environment.NewLine;
        }
    }
}
