using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewPortNet.ViewModel;

namespace KeyPortNet.ViewModel
{
    class dlgSelectImageViewModel:ViewModelBase
    {

        public string HeaderImage { get; set; } = "/KeyPortNet;component/Resources/emoticon_smile.png";
        public string HeaderTextTop { get; set; } = "Icon auswählen";
        public string HeaderTextBottom { get; set; } = "Wählen Sie aus den verfügbaren Icons aus.";

        #region Actions..

        public MyCommand acFunction1
        {
            get;
            set;
        }

        public void afFunction1(object parameter)
        {
            acFunction1 = new MyCommand();
            acFunction1.CanExecuteFunc = obj => true;
            acFunction1.ExecuteFunc = afFunction1;
        }

        public MyCommand acCloseDialog
        {
            get;
            set;
        }

        public void afCloseDialog(object parameter)
        {
            System.Windows.Window OwnerWindow = (System.Windows.Window)parameter;
            OwnerWindow.DialogResult = false;
        }


        #endregion

        public dlgSelectImageViewModel()
        {



        }

    }
}
