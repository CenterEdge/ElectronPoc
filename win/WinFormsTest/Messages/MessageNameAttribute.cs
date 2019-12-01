using System;

namespace WinFormsTest.Messages
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class MessageNameAttribute : Attribute
    {
        public string Name { get; set; }

        public MessageNameAttribute(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentException("Value cannot be null or empty.", nameof(name));
            }

            Name = name;
        }
    }
}
