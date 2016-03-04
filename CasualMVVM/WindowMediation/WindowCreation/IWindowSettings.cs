using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace FuchsiaSoft.CasualMVVM.WindowMediation.WindowCreation
{
    public interface IWindowSettings
    {
        int DefaultWidth { get; set; }

        int DefaultHeight { get; set; }

        ResizeMode DefaultResizeMode { get; set; }

        int DefaultLabelWidth { get; set; }
    }
}
