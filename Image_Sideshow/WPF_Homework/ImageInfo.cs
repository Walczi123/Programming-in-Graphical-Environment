using System;

namespace WPF_Homework
{
    class ImageInfo
    {
        public double Size { get; set; }
        public string FullPath { get; set; }
        public string Name { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }
        public static bool IsSelected { get; set; }

        public ImageInfo(string name, double width, double height, double size)
        {
            Name = name;
            Width = (int)width;
            Height = (int)height;
            Size = Math.Round(size / 1024, 2);
        }
    }
}
