using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace WpfApplication2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        public Window1()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(Window1_Loaded);
            
        }

        ObservableCollection<DirectoryEntry> entries = new ObservableCollection<DirectoryEntry>();
        ObservableCollection<DirectoryEntry> subEntries = new ObservableCollection<DirectoryEntry>();
        string[] driveLetters = Directory.GetLogicalDrives();


        void Window1_Loaded(object sender, RoutedEventArgs e)
        {

            InitializeDrives();
        }

        private void InitializeDrives()
        {
            foreach (string s in Directory.GetLogicalDrives())
            {
                drives.Items.Add(s);
            }
            //drives.SelectedIndex = 0;
        }

        void listViewItem_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            ListViewItem item = e.Source as ListViewItem;

            DirectoryEntry entry = item.DataContext as DirectoryEntry;

            if (entry.Type == EntryType.Dir)
            {
                subEntries.Clear();

                foreach (string s in Directory.GetDirectories(entry.Fullpath))
                {
                    DirectoryInfo dir = new DirectoryInfo(s);
                    DirectoryEntry d = new DirectoryEntry(
                        dir.Name, dir.FullName, "<Folder>", "<DIR>",
                        Directory.GetLastWriteTime(s),
                        "Images/folder.gif", EntryType.Dir);
                    subEntries.Add(d);
                }
                foreach (string f in Directory.GetFiles(entry.Fullpath))
                {
                    FileInfo file = new FileInfo(f);
                    DirectoryEntry d = new DirectoryEntry(
                        file.Name, file.FullName, file.Extension, file.Length.ToString(),
                        file.LastWriteTime,
                        "Images/file.gif", EntryType.File);
                    subEntries.Add(d);
                }

                //listview2.datacontext = subentries;
            }
        }
       

        public enum EntryType
        {
            Dir,
            File
        }

        public class DirectoryEntry
        {
            private string _name;
            private string _fullpath;
            private string _ext;
            private string _size;
            private DateTime _date;
            private string _imagepath;
            private EntryType _type;

            public DirectoryEntry(string name, string fullname, string ext, string size, DateTime date, string imagepath, EntryType type)
            {
                _name = name;
                _fullpath = fullname;
                _ext = ext;
                _size = size;
                _date = date;
                _imagepath = imagepath;
                _type = type;
            }

            public string Name
            {
                get { return _name; }
                set { _name = value; }
            }

            public string Ext
            {
                get { return _ext; }
                set { _ext = value; }
            }

            public string Size
            {
                get { return _size; }
                set { _size = value; }
            }

            public DateTime Date
            {
                get { return _date; }
                set { _date = value; }
            }

            public string Imagepath
            {
                get { return _imagepath; }
                set { _imagepath = value; }
            }

            public EntryType Type
            {
                get { return _type; }
                set { _type = value; }
            }

            public string Fullpath
            {
                get { return _fullpath; }
                set { _fullpath = value; }
            }
        }

        private void drives_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Console.WriteLine("kke");
            Console.WriteLine(2+3);
        }
    }
}
