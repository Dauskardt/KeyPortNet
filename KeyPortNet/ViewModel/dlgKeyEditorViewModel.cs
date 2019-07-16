using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using KeyPortNet.Model;
using System.Drawing;

namespace KeyPortNet.ViewModel
{
    class dlgKeyEditorViewModel:ViewModelBase
    {
        public string HeaderImage { get; set; } = "/KeyPortNet;component/Resources/pencil.png";
        public string HeaderTextTop { get; set; } = "Eintrag bearbeiten";
        public string HeaderTextBottom { get; set; } = "Existierenden Passwort-Eintrag ändern.";

        private Model.KeyEntry SelectedKeyField;

        private Window WindowOwnerField;

        private Visibility passwortPlainField;

        private Visibility passwortHideField;

        public Visibility PasswortPlain
        {
            get
            {
                return passwortPlainField;
            }

            set
            {
                passwortPlainField = value;
                RaisePropertyChangedEvent("PasswortPlain");
            }
        }

        public Visibility PasswortHide
        {
            get
            {
                return passwortHideField;
            }

            set
            {
                passwortHideField = value;
                RaisePropertyChangedEvent("PasswortHide");
            }
        }

        private bool ShowPWField;

        public bool ShowPW
        {
            get
            {
                return ShowPWField;
            }

            set
            {
                ShowPWField = value;

                if (value)
                {
                    PasswortPlain = Visibility.Visible;
                    PasswortHide = Visibility.Collapsed;
                }
                else
                {
                    PasswortPlain = Visibility.Collapsed;
                    PasswortHide = Visibility.Visible;
                }


                RaisePropertyChangedEvent("ShowPW");
            }
        }

        public KeyEntry SelectedKey
        {
            get
            {
                return SelectedKeyField;
            }

            set
            {
                SelectedKeyField = value;
                RaisePropertyChangedEvent("SelectedKey");
            }
        }

        public Window WindowOwner
        {
            get
            {
                return WindowOwnerField;
            }

            set
            {
                WindowOwnerField = value;
                RaisePropertyChangedEvent("WindowOwner");
            }
        }
    }
}
