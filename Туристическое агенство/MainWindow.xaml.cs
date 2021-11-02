using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
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


namespace Туристическое_агенство
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Manager.MainFrame = MainFrame;

            ImportTour();
        }

        private void ImportTour()
        {
            var fileData = File.ReadAllLines(@"C:\Users\Lenovo\Desktop\Туры.txt");
            var images = Directory.GetFiles(@"C:\Users\Lenovo\Desktop\ДЭ\import\Туры фото");

            foreach (var line in fileData)
            {
                var data = line.Split('\t');

                var tempTour = new Tour
                {
                    Name = data[0].Replace("\"", ""),
                    TicketCount = int.Parse(data[2]),
                    Price = decimal.Parse(data[3]),
                    IsActual = (data[4] == "0") ? false : true
                };

                foreach (var tourType in data[5].Replace("\"", "").Split(new string[] { ","}, StringSplitOptions.RemoveEmptyEntries))
                {
                    var currentType = ТурАгенствоEntities7.GetContext().Type.ToList().FirstOrDefault(p => p.Name == tourType);
                    if (currentType != null)
                        tempTour.Type.Add(currentType);
                }

                try
                {
                    tempTour.ImagePreview = File.ReadAllBytes(images.FirstOrDefault(p => p.Contains(tempTour.Name)));
                }
                catch( Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

                ТурАгенствоEntities7.GetContext().Tour.Add(tempTour);
                ТурАгенствоEntities7.GetContext().SaveChanges();
             }
         }

    private void BtnНазад_Click(object sender, RoutedEventArgs e)
             {
                 Manager.MainFrame.GoBack();
             }

   

    private void BtnТур_Click(object sender, RoutedEventArgs e)
    {
        Manager.MainFrame.Navigate(new Tours());
            BtnТур.Visibility = Visibility.Hidden;
            BtnОтели.Visibility = Visibility.Hidden;
        }

    private void BtnОтели_Click(object sender, RoutedEventArgs e)
    {
        Manager.MainFrame.Navigate(new Hotels());
            BtnОтели.Visibility = Visibility.Hidden;
            BtnТур.Visibility = Visibility.Hidden;

        }

    }
}
