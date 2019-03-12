using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PZ3_NetworkService
{
    public class ValidationErrors : BindableBase
    {
        // I string  - naziv property-ja
        // II string - koja greska je napravljena za njega, odnostno opis greske
        private readonly Dictionary<string, string> validationErrors = new Dictionary<string, string>();

        public bool IsValid
        {
            get { return validationErrors.Count < 1; }
        }

        public string this[string fieldName]
        {
            get
            {
                // proverava da li postoji naziv property-ja u recniku
                // ako postoji, vrati opis greske za taj property
                // ako ne, onda vrati prazan string
                return this.validationErrors.ContainsKey(fieldName) ? this.validationErrors[fieldName] : String.Empty;
            }
            set
            {
                if (this.validationErrors.ContainsKey(fieldName))   // ako greska postoji
                {
                    if (string.IsNullOrWhiteSpace(value))           // i ako je ispravljena u toku rada
                    {
                        this.validationErrors.Remove(fieldName);
                    }
                    else
                    {
                        this.validationErrors[fieldName] = value;   // ako nije, onda samo update-uje
                    }
                }
                else                                        // ako greska ne postoji
                {
                    if (!string.IsNullOrWhiteSpace(value))          // i ako je opis razlicit on null ili space-ova
                    {
                        this.validationErrors.Add(fieldName, value);    // doda gresku
                    }
                }
                this.OnPropertyChanged("IsValid");
            }
        }

        public void Clear()
        {
            validationErrors.Clear();
        }
    }
}
