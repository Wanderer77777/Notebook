using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
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
using System.Windows.Threading;
using Microsoft.Win32;

namespace Notebook
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
         TextBox.DataContext = TextNotebook;
        }

        public static GUIText TextNotebook = new GUIText();

        private void LoadBtn_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter = "Txt|*.txt";
            openFileDialog1.Title = "Загрузка файла";
            openFileDialog1.ShowDialog();
            if (openFileDialog1.FileName != "")
            {
                Thread myThread = new Thread(() => LoadBase(openFileDialog1.FileName, Dispatcher)) { IsBackground = true };
                myThread.Start();
            }
        }

        private void SaveBtn_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "Txt|*.txt";
            saveFileDialog1.ShowDialog();
            if (saveFileDialog1.FileName != "")
            {
                TextNotebook.MyText = string.Empty;
                Thread myThread = new Thread(() => SaveBase(saveFileDialog1.FileName)) { IsBackground = true };
                myThread.Start();
            }
        }
        //Загрузка
        public static void LoadBase(string path, Dispatcher a)
        {
            using (StreamReader sr = new StreamReader(path, System.Text.Encoding.UTF8))
            {
                string s;
                try
                {
                    while ((s = sr.ReadLine()) != null)
                    {
                        a.Invoke((Action)(() =>
                        {
                            TextNotebook.MyText += (s+"\n");
                        }));
                    }
                }
                catch (Exception e)
                {

                }
            }
        }
        //Сохранение
        public static void SaveBase(string path)
        {
            using (StreamWriter sw = new StreamWriter(path, false, System.Text.Encoding.Default))
            {
                sw.Write(TextNotebook.MyText);
            }
        }
    }
    //Биндинг
    public class GUIText : INotifyPropertyChanged
    {
        private string _MyText;

        public string MyText
        {
            get
            {
                return _MyText;
            }
            set
            {
                if (value != _MyText)
                {
                    _MyText = value;
                    OnPropertyChanged("MyText");
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string p_propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(p_propertyName));
            }
        }
    }
}
