using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ViewPortNet.ViewModel;

namespace KeyPortNet.ViewModel
{
    class dlgPasswortGenViewModel:ViewModelBase
    {
        public string HeaderImage { get; set; } = "/KeyPortNet;component/Resources/key_go.png";
        public string HeaderTextTop { get; set; } = "Zufälliges Passwort generieren";
        public string HeaderTextBottom { get; set; } = "Ein neues zufälliges Passwort generieren.";

        public MyCommand acLengthPlus
        {
            get;
            set;
        }

        public void afLengthPlus(object parameter)
        {
            if (PWLength < 50)
                PWLength++;
        }

        public MyCommand acLengthMinus
        {
            get;
            set;
        }

        public void afLengthMinus(object parameter)
        {
            if(PWLength > 0)
            PWLength--;
        }


        public dlgPasswortGenViewModel()
        {
            acLengthPlus = new MyCommand();
            acLengthPlus.CanExecuteFunc = obj => true;
            acLengthPlus.ExecuteFunc = afLengthPlus;

            acLengthMinus = new MyCommand();
            acLengthMinus.CanExecuteFunc = obj => true;
            acLengthMinus.ExecuteFunc = afLengthMinus;
        }

        private string PasswortField;
        //init default safe length (10)
        private int PWLengthField = 15;

        private bool AllowNumbersField = true;
        private bool AllowUperCaseField = true;
        private bool AllowLowerCaseField = true;
        private bool AllowSpecialField = true;
        private bool AllowKlammernField;
        private bool AllowMinusField;
        private bool AllowUnderlineField;
        private bool AllowSpaceField;
        private bool AllowHighANSIField;

        private string Pool { get; set; }

        private const string Numbers = "0123456789";
        private const string UperCase = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        private const string LowerCase = "abcdefghijklmnopqrstuvwxyz";
        private const string Special = "!#$*+,/:;=?@^";
        private const string Klammern = "()[]";
        private const string Minus = "-";
        private const string Underline = "_";
        private const string Space = " ";
        private const string HighANSI = "";

        public string Passwort
        {
            get
                { return PasswortField; }
            set { PasswortField = value; }
        }

        public int PWLength
        {
            get
            {
                return PWLengthField;
            }

            set
            {
                PWLengthField = value;
                RaisePropertyChangedEvent("PWLength");
            }
        }

        public bool AllowNumbers
        {
            get
            {
                return AllowNumbersField;
            }

            set
            {
                AllowNumbersField = value;
                RaisePropertyChangedEvent("AllowNumbers");
            }
        }

        public bool AllowUperCase
        {
            get
            {
                return AllowUperCaseField;
            }

            set
            {
                AllowUperCaseField = value;
                RaisePropertyChangedEvent("AllowUperCase");
            }
        }

        public bool AllowLowerCase
        {
            get
            {
                return AllowLowerCaseField;
            }

            set
            {
                AllowLowerCaseField = value;
                RaisePropertyChangedEvent("AllowLowerCase");
            }
        }

        public bool AllowSpecial
        {
            get
            {
                return AllowSpecialField;
            }

            set
            {
                AllowSpecialField = value;
                RaisePropertyChangedEvent("AllowSpecial");
            }
        }

        public bool AllowKlammern
        {
            get
            {
                return AllowKlammernField;
            }

            set
            {
                AllowKlammernField = value;
                RaisePropertyChangedEvent("AllowKlammern");
            }
        }

        public bool AllowMinus
        {
            get
            {
                return AllowMinusField;
            }

            set
            {
                AllowMinusField = value;
                RaisePropertyChangedEvent("AllowMinus");
            }
        }

        public bool AllowUnderline
        {
            get
            {
                return AllowUnderlineField;
            }

            set
            {
                AllowUnderlineField = value;
                RaisePropertyChangedEvent("AllowUnderline");
            }
        }

        public bool AllowSpace
        {
            get
            {
                return AllowSpaceField;
            }

            set
            {
                AllowSpaceField = value;
                RaisePropertyChangedEvent("AllowSpace");
            }
        }

        public bool AllowHighANSI
        {
            get
            {
                return AllowHighANSIField;
            }

            set
            {
                AllowHighANSIField = value;
                RaisePropertyChangedEvent("AllowHighANSI");
            }
        }


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



        private string GetPWPool()
        {
            string pwp = string.Empty;

            if (AllowUperCase)
            {
                pwp += UperCase;
            }
            if (AllowLowerCase)
            {
                pwp += LowerCase;
            }
            if (AllowNumbers)
            {
                pwp += Numbers;
            }
            if (AllowSpecial)
            {
                pwp += Special;
            }
            if (AllowKlammern)
            {
                pwp += Klammern;
            }
            if (AllowMinus)
            {
                pwp += Minus;
            }
            if (AllowUnderline)
            {
                pwp += Underline;
            }
            if (AllowSpace)
            {
                pwp += Space;
            }
            if (AllowHighANSI)
            {
                pwp += Space;
            }

            return pwp;
        }

        public void CreatePasswort()
        {
            Random rnd = new Random();
            Passwort = string.Empty;
            string pool = GetPWPool();

            for (int i = 0; i < PWLength; i++)
            {
                Passwort += pool.Substring(rnd.Next(0, pool.Length), 1);
            }
        }
    }
}
