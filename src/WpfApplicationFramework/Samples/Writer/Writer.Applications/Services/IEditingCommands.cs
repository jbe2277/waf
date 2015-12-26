using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Waf.Writer.Applications.Services
{
    public interface IEditingCommands : INotifyPropertyChanged
    {
        bool IsBold { get; set; }

        bool IsItalic { get; set; }

        bool IsUnderline { get; set; }

        bool IsNumberedList { get; set; }

        bool IsBulletList { get; set; }

        bool IsSpellCheckAvailable { get; }

        bool IsSpellCheckEnabled { get; set; }
    }
}
