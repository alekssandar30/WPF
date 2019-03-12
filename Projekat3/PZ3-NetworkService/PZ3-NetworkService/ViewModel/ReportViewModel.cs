using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace PZ3_NetworkService.ViewModel
{
    public class ReportViewModel : BindableBase
    {
        private string startDate;
        private string endDate;
        private string line;
        int x = 0;
        private List<string> datumi = new List<string>();
        public MyICommand Show { get; set; }
        private string text;
        private List<string> myList = new List<string>();

        public string Text {
            get { return text; }
            set
            {
                if (text != value)
                {
                    text = value;
                    OnPropertyChanged("Text");
                }
            }
        }

        public string StartDate
        {
            get { return startDate; }
            set { startDate = value; }
        }
        public string EndDate
        {
            get { return endDate; }
            set { endDate = value; }
        }

        public List<string> MyList { get => myList; set => myList = value; }
        public List<string> Datumi { get => datumi; set => datumi = value; }

        public ReportViewModel()
        {
            Show = new MyICommand(OnShow);
        }

        private void OnShow()
        {
            //ako fajl postoji ucitaj fajl u report box
            if (File.Exists("Log.txt"))
            {
                // Formatiran_text = "Object_" + split[1] + " chagned state to: " + split[2];
                if(string.IsNullOrWhiteSpace(StartDate) || string.IsNullOrWhiteSpace(EndDate))
                {
                    if (MainWindowViewModel.Objekti.Count > 0)
                        CitajSve();
                    else
                        MessageBox.Show("There is no object for today.");
                }
                else
                {
                    CitajDatume();
                }
                
                
            }
        }

        private void CitajSve()
        {

            Text = String.Join("\n", MainWindowViewModel.MojText); //joinujem samo niz stringova
            
          
        }
        private void CitajDatume()
        {
            //11/01/2019 22:25:51 : Objekat 0 Vrednost 704
            #region Komentar
            //string[] start;
            //string[] end;
            //start = StartDate.Split('/');
            //end = EndDate.Split('/');
            //int s = Int32.Parse(start[0]);
            //int e = Int32.Parse(end[0]);
            //StreamReader file = new StreamReader("Log.txt");
            //string[] splitDatum;
            //while((line=file.ReadLine()) != null)
            //{
            //    splitDatum = line.Split(' ');
            //    Datumi.Add(splitDatum[0]);
            //}
            ////sad u Listi Datumi imam sve datume?
            //string[] vreme;
            //int dan;
            //for(int i=0; i<Datumi.Count; i++)
            //{
            //    vreme = Datumi[i].Split('/');
            //    dan = Int32.Parse(vreme[0]);
            //    //idem od starta do enda i dodajem te promene u myList:
            //    for(int j = s; j<e; j++)
            //    {
            //        //treba da proverim koji objekti imaju s za start dane i j za end dane...
            //        MyList.Add("uspelo?");
            //    }
            //}

            //file.Close();
            #endregion

            //try
            //{


            //    using (StreamReader sr = new StreamReader("Log.txt"))
            //    {
            //        //15/01/2019 10:28:50 : Objekat 0 Vrednost 659
            //        foreach (string l in File.ReadAllLines("Log.txt"))
            //        {
            //            string linija = l.Substring(10, l.Length - 10);
            //            string[] split = linija.Split('/'); //split[0]->dan, split[1]->mesec
  
            //            string[] start = StartDate.ToString().Split('/');
            //            string[] end = EndDate.ToString().Split('/');
            //            if(Int32.Parse(split[0]) >= Int32.Parse(start[0]) && Int32.Parse(split[0]) <= Int32.Parse(end[0])) //ako je dan izmedju start_date-a i end_date-a prikazi to
            //            {
            //                //myList.Add(l);
            //                if (MainWindowViewModel.MojText.Contains(l))
            //                {
            //                    Text = String.Join("\n", MainWindowViewModel.MojText);
            //                }
            //            }
                        
            //        }

            //    }
                

                

            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.Message.ToString());
            //}
        }




    }
}
