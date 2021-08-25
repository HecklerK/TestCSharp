using System;
using System.Collections.Generic;
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
using System.IO;

namespace TestCSharp
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void creatHTML(string path, int space, GetD_F.directory directory)
        {
            StreamWriter streamwriter = new StreamWriter(path + @"/index.html");
            streamwriter.WriteLine("<html>");
            streamwriter.WriteLine("<head>");
            streamwriter.WriteLine("  <title>HTML-Document</title>");
            streamwriter.WriteLine("  <meta http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\" />");
            streamwriter.WriteLine("</head>");
            streamwriter.WriteLine("<body>");
            streamwriter.WriteLine("<div>");
            streamwriter.WriteLine("<h2>Всего файлов: " + directory.amount.ToString() + "</h2>");
            foreach (var mime in directory.MimeTypeAmount)
            {
                streamwriter.WriteLine("<h3>количество типа " + mime.Key + ": " + mime.Value.ToString() + "(" + String.Format("{0:F2}", mime.Value * 100.0 / directory.amount) + " %)</h3>");
            }
            streamwriter.WriteLine("</div>");
            streamwriter.WriteLine("<div>");
            streamwriter.WriteLine("<h2>Средний размер файла для каждого типа</h2>");
            foreach (var mime in directory.MimeTypeSize)
            {
                streamwriter.WriteLine("<h3>средний размер для типа " + mime.Key + ": " + (mime.Value / directory.MimeTypeAmount[mime.Key]).ToString() + " бит</h3>");
            }
            streamwriter.WriteLine("</div>");
            streamwriter.WriteLine(directory.CreatHTML(space));
            streamwriter.WriteLine("/<body>");
            streamwriter.WriteLine("/<html>");
            streamwriter.Close();
        }

        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            bool load = false;
            if (!load)
            {
                GetD_F.directory directory = new GetD_F.directory(Directory.GetCurrentDirectory());
                TextBlock.Inlines.Add(directory.GetMimeTypeAmount());
                TextBlock.Inlines.Add(directory.GetMimeTypeSize());
                TextBlock.Inlines.Add(directory.GetD_F_String("    "));
                creatHTML(Directory.GetCurrentDirectory(), 5, directory);
                load = true;
            }
        }
    }
}
