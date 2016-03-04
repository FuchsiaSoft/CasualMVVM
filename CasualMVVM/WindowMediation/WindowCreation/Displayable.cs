using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FuchsiaSoft.CasualMVVM.WindowMediation.WindowCreation
{
    public enum DisplayType
    {
        SimpleTextBox,
        LargeTextBox,
        ComboBox,
        CheckBox,
        DatePicker
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
        /// The list of allowable Types for a <see cref="DisplayType.ComboBox"/>
        /// </summary>
        private static IEnumerable<Type> _ComboBoxTypes = new List<Type>()
        {
            typeof(object)
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
        /// The <see cref="Type"/> of the property
        /// </summary>
        private Type _PropertyType;

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
        /// <param name="propertyType">The <see cref="Type"/> of
        /// the property</param>
        /// <param name="selectedItemPath">The binding path (name of property)
        /// for the selected item, only to be used for ComboBoxes</param>
        /// <param name="displayMemberPath">The displaymemberpath to use for
        /// combo boxes, only necessary to set if the DisplayType is combobox</param>
        /// <exception cref="DisplayTypeException">Will throw
        /// DisplayTypeException if the property type is not compatible
        /// with the supplied displayType</exception>
        public Displayable(string label, DisplayType displayType, 
            Type propertyType, int displayOrder,
            string selectedItemPath = null, string displayMemberPath = null)
        {
            _Label = label;
            _DisplayType = displayType;
            _DisplayMemberPath = displayMemberPath;
            _PropertyType = propertyType;
            _SelectedItemPath = selectedItemPath;
            DisplayOrder = displayOrder;

            ValidateDisplayAttribute(_DisplayType, _PropertyType);
        }

        /// <summary>
        /// Validates the attribute to make sure that the supplied displaytype and
        /// property type match.  e.g. making sure that a string property is
        /// not going to be bound to a checkbox
        /// </summary>
        /// <param name="displayType"></param>
        /// <param name="propertyType"></param>
        private void ValidateDisplayAttribute(DisplayType displayType, Type propertyType)
        {
            IEnumerable<Type> allowableTypes = GetAllowableTypes(displayType);

            if (allowableTypes.Any(t=>t == propertyType) == false)
            {
                throw DisplayTypeException.GetFromDisplayType(displayType, allowableTypes);
            }
        }

        /// <summary>
        /// Returns the relevant enumerable of Types for the provided DisplayType
        /// </summary>
        /// <param name="displayType"></param>
        /// <returns></returns>
        private IEnumerable<Type> GetAllowableTypes(DisplayType displayType)
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

                case DisplayType.CheckBox:
                    allowableTypes = _CheckBoxTypes;
                    break;

                case DisplayType.DatePicker:
                    allowableTypes = _DatePickerTypes;
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

        /// <summary>
        /// Returns the <see cref="Type"/> of
        /// the property for this attribute
        /// </summary>
        /// <returns></returns>
        public Type GetPropertyType()
        {
            return _PropertyType;
        }
    }
}
