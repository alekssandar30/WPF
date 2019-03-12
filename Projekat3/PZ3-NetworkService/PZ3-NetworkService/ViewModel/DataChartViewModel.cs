using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace PZ3_NetworkService.ViewModel
{
    public class DataChartViewModel : BindableBase
    {
        //private Model.Object trenutniObjekat = new Model.Object();
        private Model.Type trenutniTip = new Model.Type();
        private Model.Object selektovanObjekat = new Model.Object();
        private static List<string> vrednosti = new List<string>();
        private List<string> comboLista = new List<string>();
        private List<Point> points = new List<Point>();
        
        private string cmbOdabir;
        private double rectWidth1;
        private double rectWidth2;
        private double rectWidth3;
        private double rectWidth4;

        private int brojacTipa1 = 0;
        private int brojacTipa2 = 0;
        private int brojacTipa3 = 0;
        private int brojacTipa4 = 0;

        #region PROPS
        public MyICommand ShowCommand { get; set; }
        public Model.Type TrenutniTip
        {
            get { return trenutniTip; }
            set
            {
                if (trenutniTip != null)
                    trenutniTip = value;

            }
        }
        public Model.Object SelektovanObjekat
        {
            get { return selektovanObjekat; }
            set
            {
                if (selektovanObjekat != null)
                {
                    selektovanObjekat = value;
                    OnPropertyChanged("SelektovanObjekat");
                }
                    
                    
            }
        }

        public static List<string> Vrednosti { get => vrednosti; set => vrednosti = value; }
        public List<string> ComboLista { get => comboLista; set => comboLista = value; }

        public List<Point> Points
        {
            get { return points; }
            set
            {
                points = value;
                OnPropertyChanged("Points");
            }
        }

        public string CmbOdabir {
            get { return cmbOdabir; }
            set
            {
                cmbOdabir = value;
                OnPropertyChanged("CmbOdabir");
            }
        }

        public double RectWidth1 { get => rectWidth1;
            set
            {
                rectWidth1 = value;
                OnPropertyChanged("RectWidth1");
                OnPropertyChanged("RectWidth2");
                OnPropertyChanged("RectWidth3");
                OnPropertyChanged("RectWidth4");
            }
        }
        public double RectWidth2 { get => rectWidth2;
            set
            {
                rectWidth2 = value;
                OnPropertyChanged("RectWidth1");
                OnPropertyChanged("RectWidth2");
                OnPropertyChanged("RectWidth3");
                OnPropertyChanged("RectWidth4");
            }
        }
        public double RectWidth3 { get => rectWidth3;
            set
            {
                rectWidth3 = value;
                OnPropertyChanged("RectWidth1");
                OnPropertyChanged("RectWidth2");
                OnPropertyChanged("RectWidth3");
                OnPropertyChanged("RectWidth4");
            }
        }
        public double RectWidth4 { get => rectWidth4;
            set
            {
                rectWidth4 = value;
                OnPropertyChanged("RectWidth1");
                OnPropertyChanged("RectWidth2");
                OnPropertyChanged("RectWidth3");
                OnPropertyChanged("RectWidth4");
            }
        }

     
        #endregion
        public DataChartViewModel()
        {
            ShowCommand = new MyICommand(OnShow);
            for(int i = 0; i<MainWindowViewModel.Objekti.Count; i++)
            {
                ComboLista.Add($"Objekat {i}");
            }
            
        }

        
        private void OnShow()
        {
            //treba da pokupim sve vrednosti izabranog objekta iz log fajla u  jednu listu
            //try
            //{

            //    using (StreamReader sr = new StreamReader("Log.txt"))
            //    {
            //        //15/01/2019 10:28:50 : Objekat 0 Vrednost 659
            //        foreach (string l in File.ReadAllLines("Log.txt"))
            //        {
            //            string value = l.Substring(30, l.Length - 30);
            //            string[] split = value.Split(' '); //split[0]->ID, split[2]->vrednost
            //            string[] o = CmbOdabir.Split(' ');
            //            if (split[0] == o[1]) //izabrani objekat
            //            {
            //                Vrednosti.Add(split[2]+","); // dodam u listu vrednosti tog obj

            //            }
            //        }
            
            //    }

            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.Message.ToString());
            //}

            //minimalna tacka  ce mi biti 670 a maximalna 735???
            int min = 670;
            int max = 735;
            //na osnovu njih obelezavam vrednosti izabranog objekta i spajam te tacke...

            #region CRTANJE GRAFA


            //drugi graf:
            for (int i = 0; i < MainWindowViewModel.Objekti.Count; i++)
            {
                switch (MainWindowViewModel.Objekti[i].Type.Name)
                {
                    case "ELEKTROMAGNETNI MERAČI PROTOKA":
                        brojacTipa1++;
   
                        break;
                    case "UBODNI MERAČI PROTOKA":
                        brojacTipa2++;
                       
                        break;
                    case "IMPULSNI VODOMERI":
                        brojacTipa3++;
                        
                        break;
                    case "ULTRAZVUČNI MERAČI PROTOKA":
                        brojacTipa4++;
                        
                        break;
                    default:
                        break;
                }

                

                #endregion
            }
            // 800:ukupno = sirina:ukupnooTipa,     800*ukupnoTipa = ukupno*sirina 
            int ukupno = brojacTipa1 + brojacTipa2 + brojacTipa3 + brojacTipa4;
            int sirina1 = 800 * brojacTipa1 / ukupno;
            int sirina2 = 800 * brojacTipa2 / ukupno;
            int sirina3 = 800 * brojacTipa3 / ukupno;
            int sirina4 = 800 * brojacTipa4 / ukupno;
            

            RectWidth1 = sirina1;
            RectWidth2 = sirina2;
            RectWidth3 = sirina3;
            RectWidth4 = sirina4;


        }
    }
}

