using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitUtilSimulate.ViewModel
{
    public interface IOService
    {
        string BrowseForFolder(string defaultPath);
    }
}
