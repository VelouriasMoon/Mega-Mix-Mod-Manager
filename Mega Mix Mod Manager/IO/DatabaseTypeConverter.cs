using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Globalization;
using System.Drawing.Design;
using System.Windows.Forms.Design;
using Mega_Mix_Mod_Manager.Editors;

namespace Mega_Mix_Mod_Manager.IO
{
    public class DatabaseObject
    {
        [Browsable(true)]
        [ReadOnly(false)]
        public string Name { get; set; }

        [Browsable(true)]
        [ReadOnly(false)]
        public uint ID { get; set; }

        [Browsable(true)]
        [ReadOnly(false)]
        public uint Index { get; set; }
    }

    internal class DatabaseTypeConverter : TypeConverter
    {
        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType != typeof(string))
                return base.ConvertTo(context, culture, value, destinationType);

            List<DatabaseObject> members = value as List<DatabaseObject>;
            if (members == null)
                return "-";

            return string.Join(", ", members.Select(m => m.Name));
        }

        public override bool GetPropertiesSupported(ITypeDescriptorContext context)
        {
            return true;
        }

        public override PropertyDescriptorCollection GetProperties(ITypeDescriptorContext context, object value, Attribute[] attributes)
        {
            List<PropertyDescriptor> list = new List<PropertyDescriptor>();
            List<DatabaseObject> members = value as List<DatabaseObject>;
            if (members != null)
            {
                foreach (DatabaseObject member in members)
                {
                    if (member.Name != null)
                    {
                        list.Add(new MemberDescriptor(member, list.Count));
                    }
                }
            }
            return new PropertyDescriptorCollection(list.ToArray());
        }

        private class MemberDescriptor : SimplePropertyDescriptor
        {
            public MemberDescriptor(DatabaseObject member, int index)
                : base(member.GetType(), index.ToString(), typeof(string))
            {
                Member = member;
            }

            public DatabaseObject Member { get; private set; }

            public override object GetValue(object component)
            {
                return Member.Name;
            }

            public override void SetValue(object component, object value)
            {
                Member.Name = (string)value;
            }
        }
    }
}
