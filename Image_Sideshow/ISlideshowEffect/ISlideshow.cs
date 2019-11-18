using System;
using System.Collections.Generic;

using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Reflection;
using Microsoft.Win32;

namespace ISlideshow {
    public interface ISlideshow {
        string Name { get; }
        void PlaySlideshow(Image imageIn, Image imageOut, double windowWidth, double windowHeight);
    }
}
