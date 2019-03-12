using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using PZ3_NetworkService.Model;

namespace PZ3_NetworkService.ViewModel
{
    public class NetworkViewViewModel : BindableBase
    { //ViewModel za prvi view

       
        private static Model.Object selectedObj = new Model.Object();
        private static Model.Object draggedObj = new Model.Object();
        private static bool dragging = false;
        private static ObservableCollection<Model.Object> pomocna = new ObservableCollection<Model.Object>();
        private static Dictionary<Model.Object, Canvas> recnik = new Dictionary<Model.Object, Canvas>();
        #region PROPS
        public MyICommand<Canvas> DropCommand { get; set; }
        public MyICommand<Canvas> FreeCommand { get; set; }
        public MyICommand<ListView> SelectionChangedCommand { get; set; }
        public MyICommand MouseLeftButtonUpCommand { get; set; }
        public static Model.Object DraggedObj { get => draggedObj; set => draggedObj = value; }
        public static Model.Object SelectedObj { get => selectedObj; set => selectedObj = value; }
        public static bool Dragging { get => dragging; set => dragging = value; }
        public ObservableCollection<Model.Object> Pomocna {
            get { return pomocna; }
            set
            {
                if(pomocna != null)
                {
                    pomocna = value;
                    OnPropertyChanged("Pomocna");
                }
            }
        }

        public static Dictionary<Model.Object, Canvas> Recnik { get => recnik; set => recnik = value; }


        #endregion
        #region KONSTRUKTOR
        public NetworkViewViewModel()
        {
            DropCommand = new MyICommand<Canvas>(Drop);
            FreeCommand = new MyICommand<Canvas>(Free);
            SelectionChangedCommand = new MyICommand<ListView>(SelectionChanged);
            MouseLeftButtonUpCommand = new MyICommand(MouseLeftButtonUp);
            Pomocna.Clear();
            foreach (var el in MainWindowViewModel.Objekti)
            {
                Pomocna.Add(el); //preuzmem sve u pomocnu
            }
            Recnik.Clear();
        }
        #endregion

        #region METODE
        private void Drop(Canvas sender)
        {
            if(DraggedObj != null)
            {
                
                if (((Canvas)sender).Resources["taken"] == null && !Recnik.ContainsKey(DraggedObj))
                {
                    BitmapImage logo = new BitmapImage();
                    logo.BeginInit();
                    logo.UriSource = new Uri(NetworkViewViewModel.DraggedObj.Type.Img);
                    logo.EndInit();
                    ((Canvas)sender).Background = new ImageBrush(logo);
                    ((Canvas)sender).Resources.Add("taken", true);
                    ((TextBlock)(sender).Children[0]).Text = "TAKEN";
                    Pomocna.Remove(SelectedObj);
                    Recnik.Add(DraggedObj, sender);
                }
                SelectedObj = null;
                Dragging = false;
                
                

            }
        }

        private void Free(Canvas sender)
        {
            if(sender.Resources["taken"] != null)
            {
                sender.Background = Brushes.White;
                sender.Resources.Remove("taken");
                ((TextBlock)(sender).Children[0]).Text = "FREE";
                foreach(var el in MainWindowViewModel.Objekti)
                {
                    if (!Pomocna.Contains(el))
                    {
                        Pomocna.Add(el);
                    }
                }
                Model.Object obj = Recnik.FirstOrDefault(x => x.Value == sender).Key;
                Recnik.Remove(obj);
            }
        }

        private void SelectionChanged(ListView lv)
        {
            if (!Dragging)
            {
                Dragging = true;
                DraggedObj = SelectedObj;
                if(DraggedObj != null)
                    DragDrop.DoDragDrop(lv, DraggedObj, DragDropEffects.Copy | DragDropEffects.Move);
      
               
            }
        }

        private void MouseLeftButtonUp()
        {
            DraggedObj = null;
            SelectedObj = null;
            Dragging = false;
        }
        #endregion
    }
}
