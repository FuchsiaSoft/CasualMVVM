using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace FuchsiaSoft.CasualMVVM.WindowMediation.WindowCreation
{
    /// <summary>
    /// The type of control to auto generate onto the window
    /// </summary>
    public enum DisplayType
    {
        /// <summary>
        /// A simple, single line text box
        /// </summary>
        SimpleTextBox,
        /// <summary>
        /// A larger textbox which accepts return and
        /// has a vertical scroll bar enabled
        /// </summary>
        LargeTextBox,
        /// <summary>
        /// A typical combo box, for this to function the
        /// selectedItemPath and displayMemberPath parmeters
        /// should be set in the Displayable attribute
        /// constructor
        /// </summary>
        ComboBox,
        /// <summary>
        /// A typical check box
        /// </summary>
        CheckBox,
        /// <summary>
        /// A typical DatePicker
        /// </summary>
        DatePicker,
        /// <summary>
        /// A simple ListBox, for this to function the
        /// selectedItemPath and displayMemberPath parmeters
        /// should be set in the Displayable attribute
        /// </summary>
        ListBox,
        /// <summary>
        /// A button that binds to a command
        /// </summary>
        Button
    }

    /// <summary>
    /// Provides an attribute which facilitates dynamic window
    /// creation using the provided <see cref="WindowService"/>
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public sealed class Displayable : Attribute
    {
        /// <summary>
        /// The list of allowable Types for a <see cref="DisplayType.SimpleTextBox"/>
        /// </summary>
        private static IEnumerable<Type> _SimpleTextBoxTypes = new List<Type>()
        {
            typeof(string),
            typeof(int?),
            typeof(long?),
            typeof(double?),
            typeof(float?)
        };

        /// <summary>
        /// The list of allowable Types for a <see cref="DisplayType.LargeTextBox"/>
        /// </summary>
        private static IEnumerable<Type> _LargeTextBoxTypes = new List<Type>()
        {
            typeof(string)
        };

        /// <summary>
        /// The list of allowable Types for a <see cref="DisplayType.CheckBox"/>
        /// </summary>
        private static IEnumerable<Type> _CheckBoxTypes = new List<Type>()
        {
            typeof(bool),
            typeof(bool?)
        };

        /// <summary>
        /// The list of allowable Types for a <see cref="DisplayType.DatePicker"/>
        /// </summary>
        private static IEnumerable<Type> _DatePickerTypes = new List<Type>()
        {
            typeof(DateTime)
        };

        /// <summary>
        /// The list of allowable Types for a <see cref="DisplayType.ComboBox"/>
        /// </summary>
        private static IEnumerable<Type> _ComboBoxTypes = new List<Type>()
        {
            typeof(IEnumerable<>)
        };

        /// <summary>
        /// The list of allowable types for a <see cref="DisplayType.ListBox"/>
        /// </summary>
        private static IEnumerable<Type> _ListBoxTypes = new List<Type>()
        {
            typeof(IEnumerable<>)
        };

        /// <summary>
        /// The list of allowable Types for a <see cref="DisplayType.Button"/>
        /// </summary>
        private static IEnumerable<Type> _ButtonTypes = new List<Type>()
        {
            typeof(ICommand)
        };

        /// <summary>
        /// The text that will be put in a label next to the control
        /// </summary>
        private string _Label;

        /// <summary>
        /// The type of control to use, for information on
        /// what the types mean, refer to <see cref="DisplayType"/>
        /// </summary>
        private DisplayType _DisplayType;

        /// <summary>
        /// The name of the property/field to be used for combo
        /// boxes
        /// </summary>
        private string _DisplayMemberPath;

        /// <summary>
        /// The path to the property to bind selected items to
        /// for combo boxes
        /// </summary>
        private string _SelectedItemPath;

        /// <summary>
        /// The name of the property that the control
        /// for this property should have its IsEnabled
        /// property bound to.
        /// </summary>
        private string _EnabledBy;

        /// <summary>
        /// The order in which to display the control associated
        /// with the property.
        /// </summary>
        public int DisplayOrder { get; set; }


        /// <summary>
        /// Makes a ViewModel's property displayable, meaning
        /// that the <see cref="WindowService"/> can create
        /// controls automatically.
        /// </summary>
        /// <param name="label">The text to put inside
        /// the label that will be created for the control</param>
        /// <param name="displayType">The type of control
        /// to use</param>
        /// <param name="selectedItemPath">The binding path (name of property)
        /// for the selected item, only to be used for ComboBoxes</param>
        /// <param name="displayMemberPath">The displaymemberpath to use for
        /// combo boxes, only necessary to set if the DisplayType is combobox</param>
        /// <exception cref="DisplayTypeException">Will throw
        /// DisplayTypeException if the property type is not compatible
        /// with the supplied displayType</exception>
        /// TODO: finish off these comments for parameters!
        public Displayable(string label, DisplayType displayType, 
            int displayOrder,
            string selectedItemPath = null, string displayMemberPath = null,
            string enabledBy = null)
        {
            _Label = label;
            _DisplayType = displayType;
            _DisplayMemberPath = displayMemberPath;
            _SelectedItemPath = selectedItemPath;
            _EnabledBy = enabledBy;
            DisplayOrder = displayOrder;
        }

        /// <summary>
        /// Returns the relevant enumerable of Types for the provided DisplayType
        /// </summary>
        /// <param name="displayType"></param>
        /// <returns></returns>
        internal IEnumerable<Type> GetAllowableTypes(DisplayType displayType)
        {
            IEnumerable<Type> allowableTypes = null;

            switch (displayType)
            {
                case DisplayType.SimpleTextBox:
                    allowableTypes = _SimpleTextBoxTypes;
                    break;

                case DisplayType.LargeTextBox:
                    allowableTypes = _LargeTextBoxTypes;
                    break;

                case DisplayType.ComboBox:
                    allowableTypes = _ComboBoxTypes;
                    break;

                case DisplayType.ListBox:
                    allowableTypes = _ListBoxTypes;
                    break;

                case DisplayType.CheckBox:
                    allowableTypes = _CheckBoxTypes;
                    break;

                case DisplayType.DatePicker:
                    allowableTypes = _DatePickerTypes;
                    break;

                case DisplayType.Button:
                    allowableTypes = _ButtonTypes;
                    break;
            }

            return allowableTypes;
        }

        
        //These are just helper methods to get the properties,
        //because if we expose properties directly they make
        //the attribute constructor look rather messy
        //in Intellisense

        /// <summary>
        /// Returns the DisplayType for the attribute
        /// </summary>
        /// <returns></returns>
        public DisplayType GetDisplayType()
        {
            return _DisplayType;
        }

        /// <summary>
        /// Returns the LabelText for the attribute
        /// </summary>
        /// <returns></returns>
        public string GetLabel()
        {
            return _Label;
        }

        public string GetEnabledBy()
        {
            return _EnabledBy;
        }

        public string GetSelectedItemPath()
        {
            return _SelectedItemPath;
        }

        public string GetDisplayMemberPath()
        {
            return _DisplayMemberPath;
        }
    }
}
