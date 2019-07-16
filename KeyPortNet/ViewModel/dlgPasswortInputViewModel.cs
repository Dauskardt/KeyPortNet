using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeyPortNet.ViewModel
{
    class dlgPasswortInputViewModel:ViewModelBase
    {
        public string HeaderImage { get; set; } = "/KeyPortNet;component/Resources/key.png";
        public string HeaderTextTop { get; set; } = "Datenbank";
        public string HeaderTextBottom { get; set; } = "Hauptschlüssel eingeben.";

    }
}
