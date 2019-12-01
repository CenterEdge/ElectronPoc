using System;

namespace WinFormsTest.Actions
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class ActionTypeAttribute : Attribute
    {
        public string Type { get; set; }

        public ActionTypeAttribute(string type)
        {
            if (string.IsNullOrEmpty(type))
            {
                throw new ArgumentException("Value cannot be null or empty.", nameof(type));
            }

            Type = type;
        }
    }
}
