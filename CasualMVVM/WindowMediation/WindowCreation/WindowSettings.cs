using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Vaper.Core.ViewModels;

namespace Vaper.WindowMediation.WindowCreation
{
    public class WindowSettings : IWindowSettings
    {
        public ResizeMode DefaultResizeMode { get; set; } = ResizeMode.CanResizeWithGrip;

        public int DefaultWidth { get; set; } = 400;

        public int DefaultLabelWidth { get; set; } = 100;

        public int MaximumHeight { get; set; } = 600;
    }
}
