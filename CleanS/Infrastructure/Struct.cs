using System;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;

namespace CleanS.Infrastructure
{
    public class Struct
    {
        public class CustomDisplayName : System.ComponentModel.DisplayNameAttribute
        {
            public readonly string _prop;

            public CustomDisplayName() : base()
            {
            }

            public CustomDisplayName(string resourceId, string prop) : base(resourceId)
            {
                _prop = prop;
            }

            public override string DisplayName { get { return GetValueFromResource(base.DisplayName); } }
            private string GetValueFromResource(string resourceId)
            {
                var resManager = new ResourceManager(typeof(ResourceCleanS));
                return resManager.GetString(resourceId);
            }
        }

        [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
        public class FieldTypeAttribute : Attribute
        {
            public FieldTypeAttribute(string table, string value, string display, string caption)
            {
                Table = table;
                Value = value;
                Display = display;
                Caption = caption;
            }

            public FieldTypeAttribute(string table, string value, string display, string caption, int index) : this(table, value, display, caption)
            {
                Index = index;
            }

            public string Table { get; }
            public string Value { get; }
            public string Display { get; }
            public string Caption { get; }
            public int Index { get; }
        }

        public class ValueObject
        {
            public int Value { get; set; }
            public string Description { get; set; }
        }

    }
}
