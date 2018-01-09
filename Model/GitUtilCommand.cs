using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitUtilSimulate.Model
{
    
    public class GitUtilCommand
    {

        public string ExternalFTPPath { get; set; }

        public string ExternalFTPUser { get; set; }

        public string ExternalFTPPassword { get; set; }

        public string BuildNumber { get; set; }

        public string PathToBuild { get; set; }

        public DateTime CreatedAt { get; set; }


    }
}
