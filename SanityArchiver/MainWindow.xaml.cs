﻿using SanityArchiver;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
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
        ObservableCollection<DirectoryEntry> entries = new ObservableCollection<DirectoryEntry>();
        ObservableString currentPath = new ObservableString();
        string[] driveLetters;
        DirectoryEntry selectedFile;

        public Window1()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(Window1_Loaded);
            currentPath.onStringChange += OnStringChange;
        }

        void Window1_Loaded(object sender, RoutedEventArgs e)
        {
            InitializeDrives();
        }

        private void InitializeDrives()
        {
            Array.ForEach(Directory.GetLogicalDrives(), (drive => drives.Items.Add(drive)));
            drives.SelectedIndex = 0;
        }

        private void drives_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateListView(e.AddedItems[0]?.ToString());
        }

        private void DirectoryEntry_SingleClick(object sender, SelectionChangedEventArgs e)
        {
            selectedFile = (DirectoryEntry)listView1.SelectedItem ?? selectedFile;
            SelectedFileLabel.Content = selectedFile.Name;
        }

        private void DirecrotyEntry_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            DirectoryEntry selectedItem = (DirectoryEntry)listView1.SelectedItem;
            if (selectedItem.isDirectory)
            {
                string fullpath = selectedItem.Fullpath;
                UpdateListView(fullpath);
                currentPath.Content = fullpath;
            }
        }

        private void UpdateListView(string path)
        {
            entries.Clear();
            foreach (string entry in Directory.GetFileSystemEntries(path))
            {
                FileInfo file = new FileInfo(entry);
                bool isDirectory = file.Attributes.HasFlag(FileAttributes.Directory);

                DirectoryEntry directoryEntry = new DirectoryEntry(entry);

                entries.Add(directoryEntry);
            }
            listView1.DataContext = entries;
        }

        protected void OnStringChange(ObservableString sender, string args)
        {
            pathTextBox.Text = args;
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
            private EntryType _type;


            public DirectoryEntry(string name, string fullname, string ext, string size, DateTime date, EntryType type)
                : this(name, fullname, ext, size, date)
            {
                Type = type;
            }
            public DirectoryEntry(string name, string fullname, string ext, string size, DateTime date)
            {
                Name = name;
                Fullpath = fullname;
                Ext = ext;
                Size = size;
                Date = date;
            }
            public DirectoryEntry(string path)
            {
                FileInfo file = new FileInfo(path);
                bool isDirectory = file.Attributes.HasFlag(FileAttributes.Directory);

                Name = file.Name;
                Fullpath = file.FullName;
                Ext = isDirectory ? "dir" : file.Extension;
                Size = isDirectory ? "" : file.Length / 1024 / 1024 + " MB";
                Date = file.LastAccessTimeUtc;
                Type = isDirectory ? EntryType.Dir : EntryType.File;
            }

            public bool isDirectory { get => new FileInfo(Fullpath).Attributes.HasFlag(FileAttributes.Directory); }

            public string Name { get => _name; set => _name = value; }
            public string Fullpath { get => _fullpath; set => _fullpath = value; }
            public string Ext { get => _ext; set => _ext = value; }
            public string Size { get => _size; set => _size = value; }
            public DateTime Date { get => _date; set => _date = value; }
            public EntryType Type { get => _type; set => _type = value; }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            DirectoryInfo parentDirectory = new DirectoryInfo(currentPath.Content).Parent;
            if (parentDirectory == null) return;
            string parentDirectoryPath = parentDirectory.FullName;
            currentPath.Content = parentDirectoryPath;
            UpdateListView(parentDirectoryPath);
        }
    }
}
