using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SanityArchiver
{
    public class ObservableString
    {
        private string content;

        public ObservableString(string content)
        {
            Content = content;
        }

        public ObservableString()
        {
        }

        public string Content
        {
            get { return content; }
            set
            {
                onStringChange?.Invoke(this, value);
                content = value;
            }
        }
        public event OnStringChange onStringChange;

    }
    public delegate void OnStringChange(ObservableString sender, string e);
}
