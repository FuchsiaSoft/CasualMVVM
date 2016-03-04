using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FuchsiaSoft.CasualMVVM.WindowMediation.WindowCreation
{
    /// <summary>
    /// Exception that indicates there was an invalid pairing of
    /// property Type in a viewmodel and DisplayType for an
    /// auto generated window.
    /// </summary>
    [Serializable]
    class DisplayTypeException : Exception
    {
        public DisplayTypeException(string message) : base(message)
        {
            
        }

        /// <summary>
        /// Returns a new <see cref="DisplayTypeException"/> and provides
        /// a list of allowable property Types that can be paired with
        /// the given DisplayType
        /// </summary>
        /// <param name="displayType"></param>
        /// <param name="allowableTypes"></param>
        /// <returns></returns>
        internal static DisplayTypeException GetFromDisplayType
            (DisplayType displayType, IEnumerable<Type> allowableTypes)
        {
            StringBuilder builder = new StringBuilder();

            builder.Append("The DisplayType is invalid for the provided property Type, " +
                "the allowable Types for ");

            builder.Append(Enum.GetName(typeof(DisplayType), displayType));

            builder.Append(" are the following: ");

            foreach (Type allowedType in allowableTypes)
            {
                builder.Append(allowedType.FullName);
                builder.AppendLine(",");
            }

            builder.AppendLine
                ("Review your code and make sure that the correct DisplayType is being used");

            builder.AppendLine
                ("If you need to do more complex binding, then it may be more appropriate to use" +
                "a custom view rather than relying on the auto generated windows");

            return new DisplayTypeException(builder.ToString());
        }
    }
}
