using FuchsiaSoft.CasualMVVM.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
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

        private const string WRONG_VIEWMODEL_TYPE_MESSAGE =
            "The IViewModel supplied does not implement IDataEntryViewModel, this interface is " +
            "the minimum requirement for an auto generated window.";

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
            viewModel.SetActiveWindow(window, true);

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
                    CheckViewModelType(viewModel);
                    CreateWindow(window, viewModel, settings);
                    window.Show();
                    break;

                case WindowType.NewModalAutoWindowRequest:
                    CheckViewModelType(viewModel);
                    CreateWindow(window, viewModel, settings);
                    window.ShowDialog();
                    break;
            }
        }

        /// <summary>
        /// Checks to make sure that the viewmodel implements IDataEntryViewModel
        /// </summary>
        /// <param name="viewModel"></param>
        private void CheckViewModelType(IViewModel viewModel)
        {
            IEnumerable<Type> interfaces = viewModel.GetType().GetInterfaces();

            if (!interfaces.Any(i=>i == typeof(IDataEntryViewModel)))
            {
                throw new InvalidOperationException(WRONG_VIEWMODEL_TYPE_MESSAGE);
            }
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
                (p => Attribute.IsDefined(p, typeof(Displayable), true));

            if (properties.Count() == 0)
            {
                throw new InvalidOperationException(NO_PROPERTIES_MESSAGE);
            }

            ValidatePropertyAttributes(properties);

            IList<Displayable> attributes = new List<Displayable>();

            DrawControls(window, properties, settings, viewModel);

        }

        /// <summary>
        /// Checks to make sure that the property <see cref="Displayable"/> attributes
        /// are valid for this WindowService
        /// </summary>
        /// <param name="properties">The list pf properties to check.</param>
        private void ValidatePropertyAttributes(IEnumerable<PropertyInfo> properties)
        {
            foreach (PropertyInfo property in properties)
            {
                Displayable attribute = property.GetCustomAttribute<Displayable>(true);

                switch (attribute.GetDisplayType())
                {
                    case DisplayType.SimpleTextBox:
                    case DisplayType.LargeTextBox:
                    case DisplayType.CheckBox:
                    case DisplayType.DatePicker:
                        CheckType(property, attribute);
                        break;


                    case DisplayType.ComboBox:
                    case DisplayType.ListBox:
                    case DisplayType.Button:
                        CheckTypeInterfaces(property, attribute);
                        break;
                }
            }
        }

        private static void CheckTypeInterfaces(PropertyInfo property, Displayable attribute)
        {
            Type[] interfaces = property.PropertyType.GetInterfaces();
            IEnumerable<Type> allowableTypes = attribute.GetAllowableTypes(attribute.GetDisplayType());

            //interfaces that are generic are returned as their specified type, need to
            //make sure they're generic for this test.
            for (int i = 0; i < interfaces.Count(); i++)
            {
                if (interfaces[i].IsGenericType)
                {
                    interfaces[i] = interfaces[i].GetGenericTypeDefinition();
                }
            }

            if (!allowableTypes.Intersect(interfaces).Any())
            {
                throw DisplayTypeException.GetFromDisplayType(attribute.GetDisplayType(), allowableTypes);
            }
        }

        /// <summary>
        /// Performs a straightforward type check.  Not used for checking against whether
        /// a property type implements an interface.
        /// </summary>
        /// <param name="property"></param>
        /// <param name="attribute"></param>
        private static void CheckType(PropertyInfo property, Displayable attribute)
        {
            IEnumerable<Type> allowableTypes = attribute.GetAllowableTypes(attribute.GetDisplayType());
            Type propertyType = property.PropertyType;

            if (!allowableTypes.Any(t => t == propertyType))
            {
                throw DisplayTypeException.GetFromDisplayType
                    (attribute.GetDisplayType(), allowableTypes);
            }
        }

        protected virtual void DrawControls(Window window, IEnumerable<PropertyInfo> properties,
            IWindowSettings settings, IViewModel viewModel)
        {
            if (settings == null)
            {
                settings = new WindowSettings();
            }

            window.MaxHeight = settings.MaximumHeight;
            window.SizeToContent = SizeToContent.Height;
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
            AddWindowTitle(root);
            AddStandardButtons(root);
            AddControls(properties, settings, root, viewModel);

            window.Content = root;
        }

        private void AddWindowTitle(DockPanel root)
        {
            //TODO: add window title bit here
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
                HorizontalScrollBarVisibility = ScrollBarVisibility.Disabled
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

                if (displayAttribute.GetDisplayType() != DisplayType.Button)
                {
                    Label label = new Label()
                    {
                        Content = displayAttribute.GetLabel(),
                        Width = settings.DefaultLabelWidth,
                        VerticalAlignment = VerticalAlignment.Center
                    };
                    controlDock.Children.Add(label); 
                }

                if (displayAttribute.GetEnabledBy() != null &&
                    displayAttribute.GetEnabledBy() != String.Empty)
                {
                    controlDock.SetBinding(Control.IsEnabledProperty, displayAttribute.GetEnabledBy());
                }

                Binding binding = new Binding();
                //this auto window we want to validate for every keystroke
                binding.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
                binding.Source = viewModel;
                binding.Path = new PropertyPath(property.Name);
                binding.TargetNullValue = String.Empty;

                switch (displayAttribute.GetDisplayType())
                {
                    case DisplayType.SimpleTextBox:
                        AddSimpleTextBox(controlDock, binding);
                        break;

                    case DisplayType.LargeTextBox:
                        AddLargeTextBox(controlDock, binding);
                        break;

                    case DisplayType.ComboBox:
                        AddComboBox(displayAttribute, controlDock, binding);
                        break;

                    case DisplayType.ListBox:
                        AddListBox(displayAttribute, controlDock, binding);
                        break;

                    case DisplayType.CheckBox:
                        AddCheckBox(controlDock, binding);
                        break;

                    case DisplayType.DatePicker:
                        AddDatePicker(controlDock, binding);
                        break;

                    case DisplayType.Button:
                        AddButton(displayAttribute, controlDock, binding);
                        break;

                    case DisplayType.SearchableField:
                        AddSearchField(displayAttribute, controlDock, binding, property);
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

        private static void AddSearchField(Displayable displayAttribute, DockPanel controlDock, Binding binding, PropertyInfo property)
        {
            string bindingPath = property.Name;

            if (!String.IsNullOrEmpty(displayAttribute.GetDisplayMemberPath()))
            {
                bindingPath += displayAttribute.GetDisplayMemberPath();
            }

            binding.Path = new PropertyPath(bindingPath);

            Button button = new Button()
            {
                Margin = new Thickness(5),
                Padding = new Thickness(3),
                Content = "Search",
                HorizontalAlignment = HorizontalAlignment.Right
            };

            Binding searchCommandBinding = new Binding();
            searchCommandBinding.Path = new PropertyPath(displayAttribute.SearchCommandPath);

            BindingOperations.SetBinding(button, Button.CommandProperty, searchCommandBinding);

            DockPanel.SetDock(button, Dock.Right);
            controlDock.Children.Add(button);

            TextBox textbox = new TextBox()
            {
                IsReadOnly = true,
                Margin = new Thickness(5),
                BorderThickness = new Thickness(0),
                BorderBrush = Brushes.Transparent
            };

            binding.Mode = BindingMode.OneWay;

            textbox.SetBinding(TextBox.TextProperty, binding);
        }

        private static void AddButton(Displayable displayAttribute, DockPanel controlDock, Binding binding)
        {
            Button button = new Button()
            {
                Margin = new Thickness(5),
                Padding = new Thickness(3),
                Content = displayAttribute.GetLabel(),
                HorizontalAlignment = HorizontalAlignment.Right
            };

            BindingOperations.SetBinding(button, System.Windows.Controls.Button.CommandProperty, binding);
            DockPanel.SetDock(button, Dock.Right);
            controlDock.Children.Add(button);
        }

        private static void AddDatePicker(DockPanel controlDock, Binding binding)
        {
            DatePicker datePicker = new DatePicker()
            {
                Margin = new Thickness(5)
            };

            BindingOperations.SetBinding(datePicker, DatePicker.SelectedDateProperty, binding);
            controlDock.Children.Add(datePicker);
        }

        private static void AddCheckBox(DockPanel controlDock, Binding binding)
        {
            CheckBox checkBox = new CheckBox()
            {
                Margin = new Thickness(5),
                HorizontalAlignment = HorizontalAlignment.Left
            };

            BindingOperations.SetBinding(checkBox, CheckBox.IsCheckedProperty, binding);
            controlDock.Children.Add(checkBox);
        }

        private static void AddListBox(Displayable displayAttribute, DockPanel controlDock, Binding binding)
        {
            ListBox listBox = new ListBox()
            {
                Margin = new Thickness(5),
                DisplayMemberPath = displayAttribute.GetDisplayMemberPath(),
                Height = 100
            };

            BindingOperations.SetBinding(listBox, ListBox.ItemsSourceProperty, binding);
            listBox.SetBinding(ListBox.SelectedItemProperty, displayAttribute.GetSelectedItemPath());
            controlDock.Children.Add(listBox);
        }

        private static void AddComboBox(Displayable displayAttribute, DockPanel controlDock, Binding binding)
        {
            ComboBox comboBox = new ComboBox()
            {
                Margin = new Thickness(5),
                DisplayMemberPath = displayAttribute.GetDisplayMemberPath()
            };

            BindingOperations.SetBinding(comboBox, ComboBox.ItemsSourceProperty, binding);
            comboBox.SetBinding(ComboBox.SelectedItemProperty, displayAttribute.GetSelectedItemPath());
            controlDock.Children.Add(comboBox);
        }

        private static void AddLargeTextBox(DockPanel controlDock, Binding binding)
        {
            TextBox largeBox = new TextBox()
            {
                Margin = new Thickness(5),
                Height = 90,
                VerticalContentAlignment = VerticalAlignment.Top,
                TextWrapping = TextWrapping.Wrap,
                AcceptsReturn = true,
                VerticalScrollBarVisibility = ScrollBarVisibility.Visible
            };

            BindingOperations.SetBinding(largeBox, TextBox.TextProperty, binding);
            controlDock.Children.Add(largeBox);
        }

        private static void AddSimpleTextBox(DockPanel controlDock, Binding binding)
        {
            TextBox simpleBox = new TextBox()
            {
                Margin = new Thickness(5)
            };

            BindingOperations.SetBinding(simpleBox, TextBox.TextProperty, binding);
            controlDock.Children.Add(simpleBox);
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

            saveButton.SetBinding(System.Windows.Controls.Button.CommandProperty, "SaveCommand");
            cancelButton.SetBinding(System.Windows.Controls.Button.CommandProperty, "CancelCommand");

            DockPanel.SetDock(buttonDock, Dock.Bottom);
            buttonDock.Children.Add(saveButton);
            buttonDock.Children.Add(cancelButton);
            root.Children.Add(buttonDock);
        }

        public void ShowSearchWindow(ISearchViewModel viewModel)
        {
            Window window = new Window();
            SearchPage page = new SearchPage();

            IEnumerable<Searchable> attributes = 
                viewModel.GetColumns()
                .OrderBy(o => o.DisplayOrder);

            foreach (Searchable attribute in attributes)
            {
                BindingBase binding = new Binding(attribute.DisplayPath);

                GridViewColumn column = new GridViewColumn()
                {
                    Header = attribute.Header,
                    DisplayMemberBinding = binding
                };

                if (attribute.InitialWidth != null)
                {
                    column.Width = (int)attribute.InitialWidth;
                }

                page.gvData.Columns.Add(column);
            }

            window.Content = page;
            window.DataContext = viewModel;
            viewModel.SetActiveWindow(window, true);

            window.ShowDialog();
        }
    }
}
