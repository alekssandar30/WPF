using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace PZ3_NetworkService.ViewModel
{
    public class MainWindowViewModel : BindableBase
    {
        private static ObservableCollection<Model.Object> objekti = new ObservableCollection<Model.Object>();
        BindableBase currentViewModel;
        public MyICommand<string> NavCommand { get; private set; }

        NetworkViewViewModel networkViewViewmodel = new NetworkViewViewModel();
        NetworkDataViewModel netDataViewModel = new NetworkDataViewModel();
        DataChartViewModel dataChartViewModel = new DataChartViewModel();
        ReportViewModel reportViewModel = new ReportViewModel();
        //trebace mi dictionary za id,canvas i u viewViewModelu preko njega cuvam stanje canvasa
        static string[] mojText;

        #region props
       
        public static ObservableCollection<Model.Object> Objekti { get => objekti; set => objekti = value; }
        public BindableBase CurrentViewModel
        {
            get { return currentViewModel; }
            set { SetProperty(ref currentViewModel, value); }
        }

        public static string[] MojText { get => mojText; set => mojText = value; }

        
        #endregion

        public MainWindowViewModel()
        {
            CurrentViewModel = netDataViewModel;
            NavCommand = new MyICommand<string>(OnNav);
            createListener(); //Povezivanje sa serverskom aplikacijom
            if (File.Exists("Log.txt"))
                File.Delete("Log.txt");
        }

        private void OnNav(string dest)
        {
            switch (dest) {
                case "NetworkViewView":
                    CurrentViewModel = networkViewViewmodel;
                    break;

                case "NetworkDataView":
                    CurrentViewModel = netDataViewModel;
                    break;

                case "DataChartView":
                    CurrentViewModel = dataChartViewModel;
                    break;

                case "ReportView":
                    CurrentViewModel = reportViewModel;
                    break;
            }

        }

        private void createListener()
        {
            var tcp = new TcpListener(IPAddress.Any, 25565);
            tcp.Start();

            var listeningThread = new Thread(() =>
            {
                while (true)
                {
                    var tcpClient = tcp.AcceptTcpClient();
                    ThreadPool.QueueUserWorkItem(param =>
                    {
                        //Prijem poruke
                        NetworkStream stream = tcpClient.GetStream();
                        string incomming;
                        byte[] bytes = new byte[1024];
                        int i = stream.Read(bytes, 0, bytes.Length);
                        //Primljena poruka je sacuvana u incomming stringu
                        incomming = System.Text.Encoding.ASCII.GetString(bytes, 0, i);

                        //Ukoliko je primljena poruka pitanje koliko objekata ima u sistemu -> odgovor
                        if (incomming.Equals("Need object count"))
                        {
                            //Response
                            /* Umesto sto se ovde salje count.ToString(), potrebno je poslati 
                             * duzinu liste koja sadrzi sve objekte pod monitoringom, odnosno
                             * njihov ukupan broj (NE BROJATI OD NULE, VEC POSLATI UKUPAN BROJ)
                             * */
                            Byte[] data = System.Text.Encoding.ASCII.GetBytes(Objekti.Count.ToString());
                            stream.Write(data, 0, data.Length);
                            //ovde treba da formatiram text za report
                            //sad je tekst Need object count
                            if (Objekti.Count > 0)
                            {
                                MojText = new string[Objekti.Count];
                                for (int k = 0; k < Objekti.Count; k++)
                                {
                                    MojText[k] = $"Object {k}\n";
                                }
                            }
                            else if (Objekti.Count>0 && MojText.Length < Objekti.Count)
                            {
                                //ako je korisnik dodao koji objekat, tek onda formatirani tekst treba da se prosiri
                                string[] niz = MojText;
                                MojText = new string[Objekti.Count];
                                //premestam mojtekst u niz 
                                for(int j = 0; j<niz.Length; j++)
                                {
                                    MojText[j] = niz[j];
                                }
                                for (int j = Objekti.Count - niz.Length; j < Objekti.Count; j++)
                                {
                                    MojText[j] = $"Object {j}\n";
                                }

                            }

                        }
                        else
                        {
                            //U suprotnom, server je poslao promenu stanja nekog objekta u sistemu
                            //Console.WriteLine(incomming); //Na primer: "Objekat_1:272"


                            //################ IMPLEMENTACIJA ####################
                            // Obraditi poruku kako bi se dobile informacije o izmeni
                            // Azuriranje potrebnih stvari u aplikaciji
                            //string[] split = incomming.Split('_', ':');
                            //int id = Convert.ToInt32(split[1]);
                            //int value = Convert.ToInt32(split[2]);

                            string value = incomming.Substring(8, incomming.Length-8);
                            string[] split = value.Split(':');

                            if (Objekti.Count > 0)
                            {

                                Objekti[Int32.Parse(split[0])].Value = Int32.Parse(split[1]);
                                File.AppendAllText("Log.txt", DateTime.Now.ToString() + " : " + "Objekat " + split[0] + " Vrednost "
                                    + split[1] + Environment.NewLine);
                                MojText[Int32.Parse(split[0])] += "\t" + DateTime.Now.ToString() + " : " + "Objekat " + split[0] + " Vrednost "
                                    + split[1] + Environment.NewLine;
                            }
                            //canvas
                            Model.Object o = Objekti[Int32.Parse(split[0])];
                            if (NetworkViewViewModel.Recnik.ContainsKey(o) && (o.Value<670 || o.Value>730) )
                            {

                                System.Windows.Application.Current.Dispatcher.Invoke((Action)delegate
                                {
                                    string uri = @"C:\Users\sale\Desktop\HCI\Projekat3\PZ3-NetworkService\PZ3-NetworkService\Img\danger.png";
                                    ImageBrush myBrush = new ImageBrush();
                                    Image img = new Image();
                                    img.Source = new BitmapImage(new Uri(uri));
                                    myBrush.ImageSource = img.Source;

                                    NetworkViewViewModel.Recnik[o].Background = myBrush;
                                });
                            }else if (NetworkViewViewModel.Recnik.ContainsKey(o) && (o.Value>=670 && o.Value<=730))
                            {
                                System.Windows.Application.Current.Dispatcher.Invoke((Action)delegate
                                {
                                    string uri = o.Type.Img;
                                    ImageBrush myBrush = new ImageBrush();
                                    Image img = new Image();
                                    img.Source = new BitmapImage(new Uri(uri));
                                    myBrush.ImageSource = img.Source;

                                    NetworkViewViewModel.Recnik[o].Background = myBrush;
                                });
                                
                            }
                          
                        }
                    }, null);
                }
            })
            {
                IsBackground = true
            };
            listeningThread.Start();
        }


        
    }
}
