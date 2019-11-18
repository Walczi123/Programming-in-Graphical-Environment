using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace WPF_Homework
{
    class TreeItem : TreeViewItem
    {
        public string FilePath;

        public TreeItem(string fp) : base()
        {
            FilePath = fp;
        }
    }
}
