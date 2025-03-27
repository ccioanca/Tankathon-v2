using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tankathon.API
{
    public interface ITankSetup
    {
        public string name { set; }
        public string primaryColor { set; }
        public string secondaryColor { set; }
    }
}
