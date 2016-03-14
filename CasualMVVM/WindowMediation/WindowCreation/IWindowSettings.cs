using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Vaper.WindowMediation.WindowCreation
{
    public interface IWindowSettings
    {
        int DefaultWidth { get; set; }

        ResizeMode DefaultResizeMode { get; set; }

        int DefaultLabelWidth { get; set; }

        int MaximumHeight { get; set; }
    }
}
