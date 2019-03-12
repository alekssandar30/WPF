using PZ3_NetworkService.ViewModel;
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
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PZ3_NetworkService
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        
        public MainWindow()
        {
            InitializeComponent();
          
        }

        private void viewBtn_Click(object sender, RoutedEventArgs e)
        {
            leftPanel0.Visibility = Visibility.Visible;
            leftPanel1.Visibility = Visibility.Hidden;
            leftPanel2.Visibility = Visibility.Hidden;
            leftPanel3.Visibility = Visibility.Hidden;


        }

        private void dataBtn_Click(object sender, RoutedEventArgs e)
        {
            leftPanel1.Visibility = Visibility.Visible;
            leftPanel0.Visibility = Visibility.Hidden;
            leftPanel2.Visibility = Visibility.Hidden;
            leftPanel3.Visibility = Visibility.Hidden;

        }

        private void chartBtn_Click(object sender, RoutedEventArgs e)
        {
            leftPanel2.Visibility = Visibility.Visible;
            leftPanel0.Visibility = Visibility.Hidden;
            leftPanel1.Visibility = Visibility.Hidden;
            leftPanel3.Visibility = Visibility.Hidden;

        }

        private void reportBtn_Click(object sender, RoutedEventArgs e)
        {
            leftPanel3.Visibility = Visibility.Visible;
            leftPanel0.Visibility = Visibility.Hidden;
            leftPanel1.Visibility = Visibility.Hidden;
            leftPanel2.Visibility = Visibility.Hidden;

        }


        //    if (MessageBox.Show("Are you sure you want to exit?", "Warning", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
        //    {
        //        return;
        //    }
        //    else
        //    {
        //        this.Close();
        //    }

    }
}
