using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace HorizontalPlugin 
{
    public class HorizontalPlugin : ISlideshow.ISlideshow {
        public string Name {
            get { return "Horizontal Effect"; }
        }
        public void PlaySlideshow(Image imageIn, Image imageOut, double windowWidth, double windowHeight) {
            Storyboard mystoryboard;
            Storyboard mystoryboard2;
            imageIn.HorizontalAlignment = HorizontalAlignment.Right;
            imageOut.HorizontalAlignment = HorizontalAlignment.Left;
            mystoryboard = new Storyboard();
            mystoryboard2 = new Storyboard();
            DoubleAnimation animation = new DoubleAnimation(0.0, windowWidth, new TimeSpan(0, 0, 0, 0, 500));
            Storyboard.SetTargetProperty(animation, new PropertyPath(FrameworkElement.WidthProperty));
            Storyboard.SetTarget(animation, imageIn);
            DoubleAnimation animation2 = new DoubleAnimation(windowWidth, 0.0, new TimeSpan(0, 0, 0, 0, 500));
            Storyboard.SetTargetProperty(animation2, new PropertyPath(FrameworkElement.WidthProperty));
            Storyboard.SetTarget(animation2, imageOut);
            mystoryboard.Children.Add(animation);
            mystoryboard2.Children.Add(animation2);
            mystoryboard.Begin();
            mystoryboard2.Begin();
        }
    }
}
