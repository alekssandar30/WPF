using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TextEditor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        bool saved = false;
        bool exists = false;
        string filePath = "";

        public MainWindow()
        {
            InitializeComponent();
            cmbFontFamily.ItemsSource = Fonts.SystemFontFamilies.OrderBy(f => f.Source);
            cmbFontSize.ItemsSource = new List<double>() { 8, 9, 10, 11, 12, 14, 16, 18, 20, 22, 24, 26, 28, 36, 48, 72 };
            
        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (saved==false)
            {
      
                MessageBoxResult res = System.Windows.MessageBox.Show("Do you want to save this file?", "Confirm", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);

                if(res == MessageBoxResult.Yes)
                {
                    Save();
                        
                }else if(res == MessageBoxResult.Cancel)
                {
                    return;
                }
                else
                    this.Close();
              
            }
            
            this.Close();
        }

        private void cmbFontFamily_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbFontFamily.SelectedItem != null && rtbEditor!=null)
            {
                rtbEditor.Selection.ApplyPropertyValue(Inline.FontFamilyProperty, cmbFontFamily.SelectedItem);
                rtbEditor.Focus(); //da odma premestim na richtextbox
            }
        }

        private void rtbEditor_SelectionChanged(object sender, RoutedEventArgs e)
        {
            object temp = rtbEditor.Selection.GetPropertyValue(Inline.FontWeightProperty); //selektovan tekst uzimam vrednost i iz nje vadim debljinu slova
            btnBold.IsChecked = (temp != DependencyProperty.UnsetValue) && (temp.Equals(FontWeights.Bold));

            temp = rtbEditor.Selection.GetPropertyValue(Inline.FontStyleProperty);
            btnItalic.IsChecked = (temp != DependencyProperty.UnsetValue) && (temp.Equals(FontStyles.Italic));

            temp = rtbEditor.Selection.GetPropertyValue(Inline.TextDecorationsProperty);
            btnUnderLine.IsChecked= (temp != DependencyProperty.UnsetValue) && (temp.Equals(TextDecorations.Underline));

            temp = rtbEditor.Selection.GetPropertyValue(Inline.FontFamilyProperty); //kog je fonta selektovan tekst?
            cmbFontFamily.SelectedItem = temp;

            temp = rtbEditor.Selection.GetPropertyValue(Inline.FontSizeProperty);
            cmbFontSize.Text = temp.ToString();

            
            
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void cmbFontSize_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(cmbFontSize.SelectedItem != null)
            {
                rtbEditor.Selection.ApplyPropertyValue(Inline.FontSizeProperty, cmbFontSize.SelectedItem);
                rtbEditor.Focus();
            }
            

        }



        //Open i Save:
        private void Save()
        {
            Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
            dlg.Filter = "Rich Text Format (*.rtf)|*.rtf|All files (*.*)|*.*";

            if (dlg.ShowDialog() == true)
            {
                FileStream fileStream = new FileStream(dlg.FileName, FileMode.Create);
                TextRange range = new TextRange(rtbEditor.Document.ContentStart, rtbEditor.Document.ContentEnd);
                range.Save(fileStream, System.Windows.DataFormats.Rtf);
                saved = true;
                exists = true;
                filePath = dlg.FileName;

            }
        }
        private void Open()
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.Filter = "Rich Text Format (*.rtf)|*.rtf|All files(*.*)|*.*";

            if (dlg.ShowDialog() == true)
            {
                FileStream fs = new FileStream(dlg.FileName, FileMode.Open);
                TextRange range = new TextRange(rtbEditor.Document.ContentStart, rtbEditor.Document.ContentEnd);
                range.Load(fs, System.Windows.DataFormats.Rtf);
                saved = true;
                exists = true;
                filePath = dlg.FileName;
            }
        }
        private void New()
        {
            rtbEditor.SelectAll();
            rtbEditor.Selection.Text = "";
            saved = false;
            exists = false;
        }
        private void Open_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            try
            {
                //TODO: proveriti da li zeli da sacuva postojeci fajl
                if (saved == false)
                {
                    if (label2.Content.ToString() != "0" || rtbEditor.Selection.Text != "") //i ako je nesto napisano
                    {
                        MessageBoxResult res = System.Windows.MessageBox.Show("Do you want to save this file?", "Warning", MessageBoxButton.YesNo, MessageBoxImage.Question);

                        if (res == MessageBoxResult.Yes)
                        {
                            Save();

                        }

                    }
                   
                }
                Open();
            }
            catch(Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message);
            }
            
            
        }

        private void Save_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            try
            {
                TextRange documentTextRange = new TextRange(
                rtbEditor.Document.ContentStart,
                rtbEditor.Document.ContentEnd);
                if (!exists && !saved)
                    Save();
                else
                {
                    using (FileStream fs = File.Create(filePath))
                    {
                        if (System.IO.Path.GetExtension(filePath).ToLower() == ".rtf")
                        {
                            documentTextRange.Save(fs, System.Windows.DataFormats.Rtf);
                        }
                        else
                        {
                            documentTextRange.Save(fs, System.Windows.DataFormats.Xaml);
                        }

                    }
                }
                
            }
            catch(Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message);
            }
            
        }

        private bool Validate()
        {
            bool res = true;

            if (searchTxtBox.Text.Trim() == "")
            {
                searchTxtBox.BorderBrush = Brushes.Red;
                searchTxtBox.BorderThickness = new Thickness(3);
                res = false;

            }
            else
            {
                searchTxtBox.BorderBrush = Brushes.GhostWhite;
                res = true;
            }

            if (replaceTextBox.Text.Trim() == "")
            {
                replaceTextBox.BorderBrush = Brushes.Red;
                replaceTextBox.BorderThickness = new Thickness(3);

            }
            else
            {
                replaceTextBox.BorderBrush = Brushes.GhostWhite;
                res = true;
            }

            return res;

        }

        private void rtbEditor_TextChanged(object sender, TextChangedEventArgs e)
        {
            saved = false;

            //treba brojati reci:
            string rec = new TextRange(rtbEditor.Document.ContentStart, rtbEditor.Document.ContentEnd).Text;

            MatchCollection broj_reci = Regex.Matches(rec, @"[\S]+");
            label2.Content = broj_reci.Count.ToString();
            //if (rtbEditor.Document.ContentStart.GetOffsetToPosition(rtbEditor.Document.ContentEnd) < 5)
            //    label2.Content = "0";
            //else
            //    label2.Content = broj_reci.Count.ToString();

        }


        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            //TODO: Trazi to sto sam uneo u txtbox
            //Paragraph p = rtbEditor.Document.Blocks.FirstBlock as Paragraph;
            TextRange tr = new TextRange(
                rtbEditor.Document.ContentStart,
                rtbEditor.Document.ContentEnd
                );
            TextPointer current = tr.Start.GetInsertionPosition(LogicalDirection.Forward);

            //provera

            if (Validate())
            {
                while (current != null)
                {
                    string text = current.GetTextInRun(LogicalDirection.Forward);
                    if (!string.IsNullOrWhiteSpace(text)) //ako to nije razmak
                    {
                        int ind = text.IndexOf(searchTxtBox.Text);
                        if (ind != -1)
                        {
                            TextPointer selectionStart = current.GetPositionAtOffset(ind, LogicalDirection.Forward);
                            TextPointer selectionEnd = selectionStart.GetPositionAtOffset(searchTxtBox.Text.Length, LogicalDirection.Forward);
                            TextRange selection = new TextRange(selectionStart, selectionEnd);
                            selection.ApplyPropertyValue(TextElement.BackgroundProperty, Brushes.Yellow);
                            rtbEditor.Selection.Select(selection.Start, selection.End);
                            rtbEditor.Focus();
                        }
                    }
                    current = current.GetNextContextPosition(LogicalDirection.Forward);
                }

                
            }
            
        }
        

        private void btnReplace_Click(object sender, RoutedEventArgs e)
        {

            if(searchTxtBox.Text == "")
            {
                System.Windows.MessageBox.Show("Unesite tekst koji se trazi!");
                return;
            }
            TextRange content = new TextRange(rtbEditor.Document.ContentStart, rtbEditor.Document.ContentEnd);
            string rtf;

            MemoryStream memoryStream = new MemoryStream();

            content.Save(memoryStream, System.Windows.DataFormats.Rtf);
            rtf = ASCIIEncoding.Default.GetString(memoryStream.ToArray());

            if (Validate())
            {
                rtf = rtf.Replace(searchTxtBox.Text, replaceTextBox.Text);
            }
           

            MemoryStream stream = new MemoryStream(ASCIIEncoding.Default.GetBytes(rtf));
            rtbEditor.SelectAll();
            rtbEditor.Selection.Load(stream, System.Windows.DataFormats.Rtf);
            rtbEditor.Selection.ApplyPropertyValue(TextElement.BackgroundProperty, Brushes.White);
            

        }

        private void New_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            try
            {
                //TODO: provera da li zeli da sacuva dosadasnji dokument
                if (saved == false)
                {
                    if (label2.Content.ToString() != "0" || rtbEditor.Selection.Text != "") //i ako je nesto napisano
                    {
                        MessageBoxResult res = System.Windows.MessageBox.Show("Do you want to save this file?", "Warning", MessageBoxButton.YesNo, MessageBoxImage.Question);

                        if (res == MessageBoxResult.Yes)
                            Save();
                        
                    }
                }
                New();
                
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message);
            }
        }

       
        private void btnSave_Click_1(object sender, RoutedEventArgs e)
        {

            TextRange documentTextRange = new TextRange(
                rtbEditor.Document.ContentStart,
                rtbEditor.Document.ContentEnd);

            // Ako postoji prepisi ga
            try
            {
                if (exists)
                {
                    using (FileStream fs = File.Create(filePath))
                    {
                        if (System.IO.Path.GetExtension(filePath).ToLower() == ".rtf")
                        {
                            documentTextRange.Save(fs, System.Windows.DataFormats.Rtf);
                        }
                        else
                        {
                            documentTextRange.Save(fs, System.Windows.DataFormats.Xaml);
                        }

                    }
                }
                System.Windows.MessageBox.Show("Uspesno ste sacuvali fajl.");
                saved = true;
                exists = true;
            }catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            
        }

        private void btnSearch_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            searchTxtBox.Visibility = Visibility.Visible;
        }

        private void btnReplace_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            replaceTextBox.Visibility = Visibility.Visible;
        }

        private void btnColorPick_Click(object sender, RoutedEventArgs e)
        {
            //TODO: color
            ColorDialog colorDialog = new ColorDialog();
            if(colorDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                var color = Color.FromArgb(colorDialog.Color.A, colorDialog.Color.R, colorDialog.Color.G, colorDialog.Color.B);
                //TextRange tr = new TextRange(
                //    rtbEditor.Selection.Start,
                //    rtbEditor.Selection.End);
                rtbEditor.Selection.ApplyPropertyValue(FlowDocument.ForegroundProperty, new SolidColorBrush(color));
            }

        }

        

        private void btnDate_Click(object sender, RoutedEventArgs e)
        {
            DateTime vreme = DateTime.Now;
            rtbEditor.CaretPosition.InsertTextInRun(vreme.ToString(" dd.MM.yyyy HH:mm:ss"));
            
            
        }

        private void rtbEditor_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            DateTime vreme = DateTime.Now;
            if(e.Key == Key.F5)
                rtbEditor.CaretPosition.InsertTextInRun(vreme.ToString(" dd.MM.yyyy HH:mm:ss"));
           
        }
    }
}
