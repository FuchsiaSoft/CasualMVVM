using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using FuchsiaSoft.CasualMVVM.Core.ViewModels;

namespace FuchsiaSoft.CasualMVVM.WindowMediation.WindowCreation
{
    public class WindowSettings : IWindowSettings
    {
        public int DefaultHeight { get; set; } = 300;

        public ResizeMode DefaultResizeMode { get; set; } = ResizeMode.CanResizeWithGrip;

        public int DefaultWidth { get; set; } = 400;

        public int DefaultLabelWidth { get; set; } = 100;
    }
}
