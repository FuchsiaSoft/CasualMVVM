using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.ComponentModel.DataAnnotations
{

    public class DateRange : ValidationAttribute
    {
        private const string INVALID_DATE_MESSAGE =
            "The provided parmeters do not make valid dates";

        private const string NOT_DATETIME_MESSAGE =
            "DateRange Attribute has been assigned to a Type that " +
            "cannot be cast as a DateTime.";

        public DateTime _Minimum { get; set; } = DateTime.MinValue;
        public DateTime _Maximum { get; set; } = DateTime.MaxValue;

        public DateRange(int yearMin, int monthMin, int dayMin,
            int yearMax, int monthMax, int dayMax)
        {
            try
            {
                _Minimum = new DateTime(yearMin, monthMin, dayMin);
            }
            catch (Exception e)
            {
                throw new ArgumentException(INVALID_DATE_MESSAGE, e);
            }

            try
            {
                _Maximum = new DateTime(yearMax, monthMax, dayMax);
            }
            catch (Exception e)
            {
                throw new ArgumentException(INVALID_DATE_MESSAGE, e);
            }
        }

        public override bool IsValid(object value)
        {
            if (value == null) return false;

            try
            {
                DateTime temp = (DateTime)value;
            }
            catch (Exception e)
            {
                throw new InvalidOperationException(NOT_DATETIME_MESSAGE, e);
            }

            if ((DateTime)value < _Minimum) return false;
            if ((DateTime)value > _Maximum) return false;

            return true;
        }

        
    }
}
