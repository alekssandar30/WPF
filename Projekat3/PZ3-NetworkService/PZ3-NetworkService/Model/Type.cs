using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PZ3_NetworkService.Model
{
    [Serializable]
    public class Type : ValidationBase
    {
        private string name;
        private string img;

        public string Name { get => name;
        set {
                name = value;
               
                switch (name)
                {
                    case "ELEKTROMAGNETNI MERAČI PROTOKA":
                        Img = @"C:\Users\sale\Desktop\HCI\Projekat3\PZ3-NetworkService\PZ3-NetworkService\Img\elektromagnetni.jpg";
                        break;
                    case "UBODNI MERAČI PROTOKA":
                        Img = @"C:\Users\sale\Desktop\HCI\Projekat3\PZ3-NetworkService\PZ3-NetworkService\Img\ubodni.jpg";
                        break;
                    case "IMPULSNI VODOMERI":
                        Img = @"C:\Users\sale\Desktop\HCI\Projekat3\PZ3-NetworkService\PZ3-NetworkService\Img\impulsni.jpg";
                        break;
                    case "ULTRAZVUČNI MERAČI PROTOKA":
                        Img = @"C:\Users\sale\Desktop\HCI\Projekat3\PZ3-NetworkService\PZ3-NetworkService\Img\ultrazvucni.jpg";
                        break;
                    default:
                        Img = "";
                        break;
                }

            }
        }
       
        public string Img
        {
            get { return img; }
            set
            {
                if (value != img)
                {
                    img = value;
                    
                    OnPropertyChanged("Img");
                }
            }
        }

        

        protected override void ValidateSelf()
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                this.ValidationErrors["Name"] = "Name is required.";
            }

            //if (string.IsNullOrWhiteSpace(img))
            //{
            //    this.ValidationErrors["Img"] = "Morate uneti sliku.";
            //}
        }
    }
}
