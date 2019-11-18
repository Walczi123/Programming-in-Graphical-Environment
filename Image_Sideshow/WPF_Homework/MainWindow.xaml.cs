using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Forms;
using System.IO;
using System.Reflection;
using ISlideshow;

namespace WPF_Homework
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //Dictionary<string, IPlug> _Plugins;
        public Dictionary<string,ISlideshow.ISlideshow> plugins = new Dictionary<string,ISlideshow.ISlideshow>();
        public MainWindow()
        {

            InitializeComponent();
            foreach (string s in Directory.GetLogicalDrives())
            {
                TreeItem item = new TreeItem(s);
                item.Header = s;
                item.Tag = s;
                item.FontWeight = FontWeights.Normal;
                item.Items.Add(dummyNode);
                item.Expanded += new RoutedEventHandler(folder_Expanded);
                TreeViewExpander.Items.Add(item);
            }
            LoadPlugins();
        }

        public void LoadPlugins()
        {
            string[] dllFileNames = null;

            if (Directory.Exists(AppDomain.CurrentDomain.BaseDirectory))
            {
                dllFileNames = Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory, "*.dll");

                ICollection<Assembly> assemblies = new List<Assembly>(dllFileNames.Length);
                foreach (string dllFile in dllFileNames)
                {
                    AssemblyName an = AssemblyName.GetAssemblyName(dllFile);
                    Assembly assembly = Assembly.Load(an);
                    assemblies.Add(assembly);
                }

                Type pluginType = typeof(ISlideshow.ISlideshow);
                ICollection<Type> pluginTypes = new List<Type>();
                foreach (Assembly assembly in assemblies)
                {
                    if (assembly != null)
                    {
                        Type[] types = assembly.GetTypes();

                        foreach (Type type in types)
                        {
                            if (type.IsInterface || type.IsAbstract)
                            {
                                continue;
                            }
                            else
                            {
                                if (type.GetInterface(pluginType.FullName) != null)
                                {
                                    pluginTypes.Add(type);
                                }
                            }
                        }
                    }
                }

                foreach (Type type in pluginTypes)
                {
                    ISlideshow.ISlideshow plugin = (ISlideshow.ISlideshow)Activator.CreateInstance(type);
                    plugins.Add(plugin.Name, plugin);
                }

                List<string> pluginnames = new List<string>();
                foreach (var plugin in plugins)
                {
                    pluginnames.Add(plugin.Key);
                }
                effectComboBox.DataContext = plugins;
                MenuItem_Show_Click.DataContext = plugins;

            }

        }

        private void MenuItem_Click_Start(object sender, RoutedEventArgs e)
        {
            if (plugins.Count > 0)
            {
                ISlideshow.ISlideshow tmp = plugins[(string)((System.Windows.Controls.MenuItem)sender).Header];
                StartSlidesShow(tmp);
            }
            else
            {
                System.Windows.MessageBox.Show("There is no plugin or image", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void MenuItem_Exit_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }


        private void MenuItem_Open_Click(object sender, RoutedEventArgs e)
        {
            List<ImageInfo> images = new List<ImageInfo>();
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            dialog.Description = "Open Folder";
            DialogResult result = dialog.ShowDialog();

            if (result == System.Windows.Forms.DialogResult.OK)
            {
                string path = dialog.SelectedPath;
                DirectoryInfo folder = new DirectoryInfo(path);
                if (folder.Exists)
                {
                    foreach (FileInfo finfo in folder.GetFiles())
                    {
                        if (finfo.Extension == ".jpg" || finfo.Extension == ".png" || finfo.Extension == ".PNG" || finfo.Extension == ".JPG")
                        {
                            try
                            {
                                BitmapImage xd = new BitmapImage(new Uri(finfo.FullName));
                                images.Add(new ImageInfo(finfo.Name, xd.Width, xd.Height, finfo.Length));
                            }
                            catch { }
                        }
                    }
                }
                foreach (ImageInfo img in images)
                {
                    img.FullPath = path + "\\" + img.Name;
                }
            }
            if (images.Any() == true)
                ischosen = images.Any();
            else
                ischosen = false;
            this.DataContext = images;
        }

        private void MenuItem_Click_About(object sender, RoutedEventArgs e)
        {
            System.Windows.MessageBox.Show("This is Image Slideshow Application", "About", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private object dummyNode = null;
        void folder_Expanded(object sender, RoutedEventArgs e)
        {
            TreeItem item = (TreeItem)sender;
            if (item.Items.Count == 1 && item.Items[0] == dummyNode)
            {
                item.Items.Clear();
                try
                {
                    foreach (string s in Directory.GetDirectories(item.Tag.ToString()))
                    {
                        TreeItem subitem = new TreeItem(s);
                        subitem.Header = s.Substring(s.LastIndexOf("\\") + 1);
                        subitem.Tag = s;
                        subitem.FontWeight = FontWeights.Normal;
                        subitem.Items.Add(dummyNode);
                        subitem.Expanded += new RoutedEventHandler(folder_Expanded);
                        item.Items.Add(subitem);
                    }
                }
                catch (Exception) { }
            }
        }
        private void SourceListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sourceListView.SelectedIndex == -1)
            {
                DataSource.Instance.IsSelected = false;
            }
            else
            {
                DataSource.Instance.IsSelected = true;
                fileinfoExpander.IsExpanded = true;
                fileinfoExpander.DataContext = sourceListView.SelectedItem;
            }
        }

        private void TreeViewExpander_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            List<ImageInfo> images = new List<ImageInfo>();
            string path = ((TreeItem)e.NewValue).FilePath;
            DirectoryInfo folder = new DirectoryInfo(path);
            if (folder.Exists)
            {
                try
                {
                    foreach (FileInfo finfo in folder.GetFiles())
                    {
                        if (finfo.Extension == ".jpg" || finfo.Extension == ".png" || finfo.Extension == ".PNG" || finfo.Extension == ".JPG")
                        {
                            BitmapImage temp = new BitmapImage(new Uri(finfo.FullName));
                            images.Add(new ImageInfo(finfo.Name, temp.Width, temp.Height, finfo.Length));
                        }
                    }
                }
                catch
                {
                    new System.UnauthorizedAccessException();
                }
            }
            foreach (ImageInfo img in images)
            {
                img.FullPath = path + "\\" + img.Name;
            }
            if (images.Any() == true)
                ischosen = images.Any();
            else
                ischosen = false;
            this.DataContext = images;

        }

        private bool ischosen= false;
        private void StartShow_Click(object sender, RoutedEventArgs e)
        {
            if (ischosen == true)
            {
                StartSlidesShow(effectComboBox.SelectedIndex);
            }
            else
                System.Windows.MessageBox.Show("There is no plugin or image", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
        }
        private void StartSlidesShow(int effect)
        {
            if (this.DataContext is List<ImageInfo> && plugins.Count > 0)
            {
                if (((List<ImageInfo>)this.DataContext).Count > 0)
                {
                    SlideWindow slideWindow = new SlideWindow(plugins.ElementAt(effect).Value);
                    slideWindow.ShowDialog();
                }
                else
                {
                    System.Windows.MessageBox.Show("There is no plugin or image", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            else
                System.Windows.MessageBox.Show("There is no plugin or image", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
        }

        private void StartSlidesShow(ISlideshow.ISlideshow plugin)
        {
            if (this.DataContext is List<ImageInfo> && plugins.Count > 0)
            {
                if (((List<ImageInfo>)this.DataContext).Count > 0)
                {
                    SlideWindow slideWindow = new SlideWindow(plugin);
                    slideWindow.ShowDialog();
                }
                else
                {
                    System.Windows.MessageBox.Show("There is no plugin or image", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            else
                System.Windows.MessageBox.Show("There is no plugin or image", "Error",MessageBoxButton.OK, MessageBoxImage.Warning);
        }
    
    }
}
