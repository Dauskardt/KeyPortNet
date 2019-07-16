using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using KeyPortNet.Model;
using System.ComponentModel;

namespace KeyPortNet.View
{
    /// <summary>
    /// Interaktionslogik für dlgPasswortGen.xaml
    /// </summary>
    public partial class dlgPasswortGen : Window,INotifyPropertyChanged
    {
        [field: NonSerialized]
        public event PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChangedEvent(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChangedEventArgs e = new PropertyChangedEventArgs(propertyName);
                PropertyChanged(this, e);
            }
        }

        private Window ParentWindowField;
        private Model.KeyEntry KeyField;

        public Window ParentWindow
        {
            get
            {
                return ParentWindowField;
            }

            set
            {
                ParentWindowField = value;
                RaisePropertyChangedEvent("ParentWindow");
            }
        }

        public KeyEntry Key
        {
            get
            {
                return KeyField;
            }

            set
            {
                KeyField = value;
                RaisePropertyChangedEvent("Key");
            }
        }

        public dlgPasswortGen(Window parentwindow, Model.KeyEntry key)
        {
            InitializeComponent();

            ParentWindow = parentwindow;
            Key = key;
            this.Owner = parentwindow;
            this.ucKeyGen.DataContext = key;
            this.ucKeyGen.btnGenerate.Click += BtnGenerate_Click;
        }

        private void BtnGenerate_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.dlgPasswortGenViewModel vm = (ViewModel.dlgPasswortGenViewModel)this.DataContext;
            vm.CreatePasswort();
            Key.Passwort = vm.Passwort;
        }

        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
