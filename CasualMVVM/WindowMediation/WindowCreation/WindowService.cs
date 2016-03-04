using FuchsiaSoft.CasualMVVM.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Shapes;

namespace FuchsiaSoft.CasualMVVM.WindowMediation.WindowCreation
{
    /// <summary>
    /// For documentation of the contract refer to <see cref="IWindowService"/>
    /// ... It is possible to derive an instance of this class to provide
    /// a custom <see cref="IWindowService"/> that can be passed to
    /// the <see cref="WindowListener"/> on application startup to facilitate
    /// your own logic or control which Window to use as referred to in
    /// documentation for <see cref="ShowWindow(object, WindowType)"/>
    /// </summary>
    public class WindowService : IWindowService
    {
        private const string NO_PROPERTIES_MESSAGE =
            "No properties were found that have the Displayable attribute, either review " +
            "your code to make sure at least one property is Displayable, or request a window " +
            "that is not auto generated";

        /// <summary>
        /// For documentation refer to <see cref="IWindowService"/>
        /// This method can be overridden in a derived class to allow
        /// for custom Windows rather than standard WPF blank windows.
        /// </summary>
        /// <param name="viewModel"></param>
        /// <param name="settings">An instance of IWindowSettings to 
        /// control default window size etc.</param>
        /// <param name="type">The type of window to create</param>
        public virtual void ShowWindow(IViewModel viewModel, WindowType type, 
            IWindowSettings settings)
        {
            Window window = new Window();

            window.DataContext = viewModel;

            switch (type)
            {
                case WindowType.NewWindowRequest:
                    window.Content = viewModel;
                    window.Show();
                    break;

                case WindowType.NewModalWindowRequest:
                    window.Content = viewModel;
                    window.ShowDialog();
                    break;

                case WindowType.NewAutoWindowRequest:
                    CreateWindow(window, viewModel, settings);
                    window.Show();
                    break;

                case WindowType.NewModalAutoWindowRequest:
                    CreateWindow(window, viewModel, settings);
                    window.ShowDialog();
                    break;
            }

            viewModel.ActiveWindow = window;
        }

        /// <summary>
        /// Creates a set of controls for the <see cref="Window"/> based on
        /// the options specified in the viewmodel properties, using
        /// <see cref="Displayable"/> attributes
        /// </summary>
        /// <param name="window"></param>
        /// <param name="viewModel"></param>
        /// <param name="settings">An instance of IWindowSettings to control
        /// default window sizing etc.</param>
        protected virtual void CreateWindow(Window window, IViewModel viewModel,
            IWindowSettings settings)
        {
            IEnumerable<PropertyInfo> properties =
                viewModel.GetType().GetProperties().Where
                (p => Attribute.IsDefined(p, typeof(Displayable)));

            if (properties.Count() == 0)
            {
                throw new InvalidOperationException(NO_PROPERTIES_MESSAGE);
            }

            IList<Displayable> attributes = new List<Displayable>();

            DrawControls(window, properties, settings, viewModel);

        }

        protected virtual void DrawControls(Window window, IEnumerable<PropertyInfo> properties,
            IWindowSettings settings, IViewModel viewModel)
        {
            if (settings == null)
            {
                settings = new WindowSettings();
            }

            window.Height = settings.DefaultHeight;
            window.Width = settings.DefaultWidth;
            window.ResizeMode = settings.DefaultResizeMode;

            //The general gist of this design is a dock panel as the root of the window
            //with a save and cancel button docked at the bottom right.
            //then controls will be placed in the Window, each inside a dock
            //panel of their own to group them together.
            //the controls are contained in a stack panel, which itself is inside
            //a scrollviewer, so that if user resizes the window it still works.

            DockPanel root = new DockPanel();

            AddStatusBar(root);
            AddStandardButtons(root);
            AddControls(properties, settings, root, viewModel);

            window.Content = root;
        }

        private void AddStatusBar(DockPanel root)
        {
            ValidationStatusBar statusBar = new ValidationStatusBar();

            DockPanel.SetDock(statusBar, Dock.Bottom);

            root.Children.Add(statusBar);
        }

        private static void AddControls(IEnumerable<PropertyInfo> properties, 
            IWindowSettings settings, DockPanel root, IViewModel viewModel)
        {
            ScrollViewer scrollViewer = new ScrollViewer()
            {
                VerticalScrollBarVisibility = ScrollBarVisibility.Auto,
                HorizontalScrollBarVisibility = ScrollBarVisibility.Auto
            };

            root.Children.Add(scrollViewer);
            StackPanel stackPanel = new StackPanel();
            scrollViewer.Content = stackPanel;

            //The controls are all added to this list, in the same
            //index position as the attribute would have been if
            //in its own sorted list, this is to facilitate the ordering
            //of displayable properties, without having the below foreach
            //working on the attributes (which would make accessing their
            //associated properties impossible).

            IList<Displayable> attributes = new List<Displayable>();

            foreach (PropertyInfo property in properties)
            {
                attributes.Add(property.GetCustomAttribute<Displayable>());
            }

            attributes = attributes.OrderBy(o => o.DisplayOrder).ToList();

            IList<DockPanel> controls = new List<DockPanel>(attributes.Count);
            for (int i = 0; i < attributes.Count; i++)
            {
                controls.Add(null);
            }

            foreach (PropertyInfo property in properties)
            {
                Displayable displayAttribute =
                    property.GetCustomAttribute<Displayable>();

                DockPanel controlDock = new DockPanel();
                Label label = new Label()
                {
                    Content = displayAttribute.GetLabel(),
                    Width = settings.DefaultLabelWidth
                };
                controlDock.Children.Add(label);

                Binding binding = new Binding();
                binding.Source = viewModel;

                switch (displayAttribute.GetDisplayType())
                {
                    case DisplayType.SimpleTextBox:
                        TextBox simpleBox = new TextBox()
                        {
                            Margin = new Thickness(5)
                        };

                        binding.Path = new PropertyPath(property.Name);
                        binding.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
                        BindingOperations.SetBinding(simpleBox, TextBox.TextProperty, binding);
                        controlDock.Children.Add(simpleBox);

                        break;

                    case DisplayType.LargeTextBox:
                        TextBox largeBox = new TextBox()
                        {
                            Margin = new Thickness(5),
                            Height = 90,
                            VerticalContentAlignment = VerticalAlignment.Top,
                            TextWrapping = TextWrapping.Wrap,
                            AcceptsReturn = true
                        };
                        controlDock.Children.Add(largeBox);
                        break;

                    case DisplayType.ComboBox:
                        break;

                    case DisplayType.CheckBox:
                        break;

                    case DisplayType.DatePicker:
                        break;
                }

                int thisAttributesIndex = attributes.IndexOf(displayAttribute);
                controls[thisAttributesIndex] = controlDock;
            }

            foreach (DockPanel controlDock in controls)
            {
                stackPanel.Children.Add(controlDock);
            }
        }

        private static void AddStandardButtons(DockPanel root)
        {
            DockPanel buttonDock = new DockPanel()
            {
                HorizontalAlignment = HorizontalAlignment.Right
            };

            Button cancelButton = new Button()
            {
                Content = "Cancel",
                Width = 80,
                Margin = new Thickness(10),
                Padding = new Thickness(5)
            };

            

            Button saveButton = new Button()
            {
                Content = "Save",
                Width = 80,
                Margin = new Thickness(10),
                Padding = new Thickness(5)
            };

            saveButton.SetBinding(Button.CommandProperty, "SaveCommand");

            DockPanel.SetDock(buttonDock, Dock.Bottom);
            buttonDock.Children.Add(saveButton);
            buttonDock.Children.Add(cancelButton);
            root.Children.Add(buttonDock);
        }
    }
}
