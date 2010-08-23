using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

namespace Samples.Validation
{
    public class DataAnnotationValidator
    {
        public static void Validate(object objectToValidate, IEnumerable<PropertyInfo> properties)
        {
            foreach (PropertyInfo prop in properties)
            {
                Validate(objectToValidate, prop);
            }
        }

        public static void Validate(object objectToValidate, PropertyInfo property)
        {
            IEnumerable<ValidationAttribute> validationAttrs =
                property.GetCustomAttributes(typeof(ValidationAttribute), true)
                    .OfType<ValidationAttribute>();

            foreach (ValidationAttribute attr in validationAttrs)
            {
                object valueToCheck = property.GetValue(objectToValidate, null);
                if (!attr.IsValid(valueToCheck))
                {
                    throw new ValidationException(attr.ErrorMessage, attr, valueToCheck);
                }
            }
        }
    }
}