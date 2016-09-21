using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace SortVisualizer
{
    public static class Extensions
    {
        /// <summary>
        /// Extension method to TextBox which allows a string to be appended with a newline character.
        /// </summary>
        /// <param name="source">The target textbox</param>
        /// <param name="value">The value to append</param>
        delegate void AppendLineCallback(TextBox source, string value);
        public static void AppendLine(this TextBox source, string value)
        {
            if (source.InvokeRequired)
            {
                AppendLineCallback a = new AppendLineCallback(AppendLine);
                source.Parent.Invoke(a, new object[] { source, value });
            }
            else
            {
                if (source.Text.Length == 0)
                {
                    source.Text = value;
                }
                else
                {
                    source.AppendText("\r\n" + value);
                }
            }
           
        }
    }
}
