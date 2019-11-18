using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Threading;


namespace WPF_Homework
{
    /// <summary>
    /// Interaction logic for SlideWindow.xaml
    /// </summary>
    public partial class SlideWindow : Window
    {
        private DispatcherTimer dtClockTime { get; set; }
        private List<string> imagePaths = new List<string>();
        private int currentIndex = 0;
        private ISlideshow.ISlideshow plugin;

        public SlideWindow(ISlideshow.ISlideshow plugin_tmp)
        {
            InitializeComponent();
            plugin = plugin_tmp;

            foreach (ImageInfo i in (List<ImageInfo>)((MainWindow)Application.Current.MainWindow).DataContext)
            {
                imagePaths.Add(i.FullPath);
            }

            dtClockTime = new DispatcherTimer();
            dtClockTime.Interval = new TimeSpan(0, 0, 5);
            dtClockTime.Tick += DtClockTime_Tick;
            dtClockTime.Start();

            slide.Source = new BitmapImage(new Uri((string)imagePaths[0]));
            nextslides.Source = new BitmapImage(new Uri((string)imagePaths[currentIndex]));
            plugin.PlaySlideshow(nextslides, slide, 1024, 768);
        }

        private void DtClockTime_Tick(object sender, EventArgs e)
        {
            int oldindex = currentIndex;
            currentIndex = ++currentIndex % imagePaths.Count;

            slide.Source = new BitmapImage(new Uri((string)imagePaths[oldindex]));
            nextslides.Source = new BitmapImage(new Uri((string)imagePaths[currentIndex]));
            plugin.PlaySlideshow(nextslides, slide, 1024, 768);
        }

        private void ShowRightClickMenu(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            ContextMenu cmxd = this.FindResource("rightClickMenu") as ContextMenu;
            cmxd.PlacementTarget = sender as Button;
            cmxd.IsOpen = true;
        }

        private void PlayPause(object sender, RoutedEventArgs e)
        {
            if (dtClockTime.IsEnabled)
                dtClockTime.Stop();
            else
                dtClockTime.Start();
        }

        private void CloseWindow(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}

