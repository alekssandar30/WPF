    using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Win32;
using PZ3_NetworkService.Model;

namespace PZ3_NetworkService.ViewModel
{
    public class NetworkDataViewModel : BindableBase  //ViewModel za drugi view
    {
        //objekat koji ce biti predstavljen u tabeli u networkData view-u
       
        private Model.Object trenutniObjekat = new Model.Object();
        private Model.Type trenutniTip = new Model.Type();
        private Model.Object selektovanObjekat;
        private ObservableCollection<Model.Object> searchLista = new ObservableCollection<Model.Object>();
        private ObservableCollection<Model.Object> pomocnaLista = new ObservableCollection<Model.Object>();
        
        private List<string> cmbItems = new List<string>() { "ELEKTROMAGNETNI MERAČI PROTOKA",
        "UBODNI MERAČI PROTOKA", "IMPULSNI VODOMERI", "ULTRAZVUČNI MERAČI PROTOKA"};

        private string searchText = "SEARCH";
        private string searchWord = "";
        private bool isNameChecked = false;
        private bool isTypeChecked = false;
        private bool first = false;
        private bool isOutChecked;
        private bool isExpectedChecked;

        #region PROPS
        public MyICommand DeleteCommand { get; set; }
        public MyICommand AddCommand { get; set; }
        
        //trebace mi ovde jos komande za pretragu...
        public MyICommand SearchCommand { get; set; }
        public MyICommand OutOfRangeCommand { get; set; }
        public MyICommand ExpectedCommand { get; set; }
        public MyICommand CancelFilters { get; set; }


        public List<string> CmbItems { get { return cmbItems; } }

        public Model.Object TrenutniObjekat { get => trenutniObjekat; set => trenutniObjekat = value; }
        public Model.Type TrenutniTip
        {
            get { return trenutniTip; }
            set
            {
                trenutniTip = value;
               
            }
        }
        public string SearchWord
        {
            get { return searchWord; }
            set
            {
                if(searchWord != value)
                {
                    searchWord = value;
                    OnPropertyChanged("SearchWord");
                }
            }
        }
        public bool IsNameChecked
        {
            get { return isNameChecked; }
            set
            {
                if (isNameChecked != value)
                {
                    isNameChecked = value;
                    OnPropertyChanged("IsNameChecked");
                }
            }
        }
        public bool IsTypeChecked
        {
            get { return isTypeChecked; }
            set
            {
                if (isTypeChecked != value)
                {
                    isTypeChecked = value;
                    OnPropertyChanged("IsTypeChecked");
                }
            }
        }

        public Model.Object SelektovanObjekat
        {
            get { return selektovanObjekat; }
            set
            {
                selektovanObjekat = value;
                DeleteCommand.RaiseCanExecuteChanged();
            }
        }

        public ObservableCollection<Model.Object> SearchLista { get => searchLista; set => searchLista = value; }

        public string SearchText {
            get { return searchText; }
            set
            {
                searchText = value;
                OnPropertyChanged("SearchText");
            }
        }

        public ObservableCollection<Model.Object> PomocnaLista {
            get { return pomocnaLista; }
            set
            {
                pomocnaLista = value;
                OnPropertyChanged("PomocnaLista");
            }
        }

        public bool IsOutChecked { get => isOutChecked; set => isOutChecked = value; }
        public bool IsExpectedChecked { get => isExpectedChecked; set => isExpectedChecked = value; }
        #endregion




        public NetworkDataViewModel()
        {
            AddCommand = new MyICommand(OnAdd);
            DeleteCommand = new MyICommand(OnDelete, CanDelete);
           
            SearchCommand = new MyICommand(OnSearch);
            OutOfRangeCommand = new MyICommand(OnOutRange);
            ExpectedCommand = new MyICommand(OnExpected);
            CancelFilters = new MyICommand(OnCancel);
            foreach(var el in MainWindowViewModel.Objekti)
            {
                PomocnaLista.Add(el);
            }
           
        }

        private void OnAdd()
        {
            //ovde treba prvo validacija
            TrenutniObjekat.Validate();
            TrenutniTip.Validate();
            if(TrenutniObjekat.IsValid && trenutniTip.IsValid)
            {
                var o = new Model.Object(TrenutniObjekat.Id, TrenutniObjekat.Name, TrenutniTip.Name, TrenutniTip.Img, 0);
                MainWindowViewModel.Objekti.Add(o);
                PomocnaLista.Add(o);
       
            }

        }
        private void OnDelete()
        {
            MainWindowViewModel.Objekti.Remove(SelektovanObjekat);
            PomocnaLista.Remove(SelektovanObjekat);
           
        }
        private bool CanDelete()
        {
            if (SelektovanObjekat != null)
                return true;
            return false;
        }
        

        private void OnSearch()
        {
            //u zavisnosti da l je izabran type ili name pogleda se tekst u 
            //textboxu i klikom na dugme search primeni se pretraga
            if (!first)
            {
                if (!string.IsNullOrWhiteSpace(SearchWord) && IsNameChecked == true)
                {
                   
                    SearchLista.Clear();
                    foreach (var el in MainWindowViewModel.Objekti)
                    {
                        if (el.Name.ToUpper().Contains(SearchWord.ToUpper()))
                        {
                            SearchLista.Add(el);
                        }
                    }
                    PomocnaLista.Clear();
                    foreach (var s in SearchLista)
                    { //menjam originalnu kolekciju 
                        PomocnaLista.Add(s);
                    }
                    OnPropertyChanged("PomocnaLista");
                    first = true;
                    SearchText = "BACK";
                    OnPropertyChanged("SearchText");
                }
                else if (!string.IsNullOrWhiteSpace(SearchWord) && IsTypeChecked == true)
                {
                    //SearchText = "SEARCH";
                    //OnPropertyChanged("SearchText");
                    SearchLista.Clear();
                    foreach (var el in MainWindowViewModel.Objekti)
                    {
                        if (el.Type.Name.ToUpper().Contains(SearchWord.ToUpper()))
                        {
                            SearchLista.Add(el);
                        }
                    }
                    PomocnaLista.Clear();
                    foreach (var s in SearchLista)
                    { //menjam originalnu kolekciju 
                        PomocnaLista.Add(s);
                    }
                    OnPropertyChanged("PomocnaLista");
                    first = true;
                    SearchText = "BACK";
                    OnPropertyChanged("SearchText");
                }
            }
            else
            {
                //ponistavanje search-a
                SearchText = "SEARCH";
                OnPropertyChanged("SearchText");
                PomocnaLista.Clear();
                foreach(var el in MainWindowViewModel.Objekti)
                {
                    PomocnaLista.Add(el);
                }
                first = false;
            }
        }

        private void OnOutRange()
        {
            // prikazuju se samo vrednosti manje od 670 ili vece od 735 litara
            //try
            //{
            //    for (int i = 0; i < PomocnaLista.Count; i++)
            //    {
            //        if (PomocnaLista[i].Value >= 670 || PomocnaLista[i].Value <= 735)
            //        {
            //            //MainWindowViewModel.Objekti.RemoveAt(i);
            //            PomocnaLista.RemoveAt(i);
            //           // OnPropertyChanged("PomocnaLista");
            //        }
            //    }
            //}catch(Exception ex)
            //{
            //    MessageBox.Show(ex.Message.ToString());
            //}
            //IsOutChecked = true;
            foreach(var el in PomocnaLista.ToArray())
            {
                if (el.Value >= 670 && el.Value <= 735)
                    PomocnaLista.Remove(el);
            }
        }

        private void OnExpected()
        {
            // prikazuju se samo vrednosti izmedju 670 i 735 litara
            //try
            //{
            //    for (int i = 0; i < PomocnaLista.Count; i++)
            //    {
            //        if (PomocnaLista[i].Value < 670 || PomocnaLista[i].Value > 735)
            //        {
            //           // MainWindowViewModel.Objekti.RemoveAt(i);
            //            PomocnaLista.RemoveAt(i);
            //            //OnPropertyChanged("PomocnaLista");
            //        }
            //    }
            //}catch(Exception ex)
            //{
            //    MessageBox.Show(ex.Message.ToString());
            //}
            //IsExpectedChecked = true;
            foreach(var el in PomocnaLista.ToArray())
            {
                if (el.Value < 670 || el.Value > 735)
                    PomocnaLista.Remove(el);
            }
        }

        private void OnCancel()
        {
            IsOutChecked = false;
            IsExpectedChecked = false;
            foreach(var el in MainWindowViewModel.Objekti)
            {
                if (!PomocnaLista.Contains(el))
                {
                    PomocnaLista.Add(el);
                }
            }
        }
    }
}
