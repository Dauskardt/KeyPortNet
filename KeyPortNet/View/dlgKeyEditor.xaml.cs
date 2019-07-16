using KeyPortNet.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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

namespace KeyPortNet.View
{
    /// <summary>
    /// Interaktionslogik für dlgKeyEditor.xaml
    /// </summary>
    public partial class dlgKeyEditor : Window, INotifyPropertyChanged
    {
        private ViewModel.dlgKeyEditorViewModel vm;

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

        ViewModel.MainWindowViewModel vmMain;

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

        private Model.KeyEntry TempKey { get; set; }
        private Model.KeyEntry OriginalKey { get; set; }

        public dlgKeyEditor(Model.KeyEntry key, Window owner)
        {

            InitializeComponent();

            TempKey = key.Clone();
            OriginalKey = key;

            vm = (ViewModel.dlgKeyEditorViewModel)this.DataContext;
            vm.SelectedKey = key;
            vm.SelectedKey.Zugriffsdatum = DateTime.Now;

            vmMain = (ViewModel.MainWindowViewModel)owner.DataContext;
            this.Owner = owner;

            //this.DataContext = key;
            ////this.txtPasswortPlain.DataContext = key.Passwort;
            this.cboGroups.ItemsSource = vmMain.DB.GetGroupList;
            this.cboGroups.SelectedItem = vmMain.SelectedGroup;

            
        }

        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            string hash = OriginalKey.GetHash();

            //Prüfung, ob Änderungen vorliegen..
            if (OriginalKey.CRC == null || !OriginalKey.CRC.Equals(hash))
            {
                //Änderung eintragen
                OriginalKey.Aenderungsdatum = DateTime.Now;
                OriginalKey.CRC = hash;
            }

            DialogResult = true;
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            if (OriginalKey.GetHash() != TempKey.GetHash())
            {
                OriginalKey.GetValues(TempKey);
            }

            DialogResult = false;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            dlgSelectImage se = new dlgSelectImage(vmMain.GetAllIcons(),this);

            if ((bool)se.ShowDialog())
            {
                ViewModel.dlgKeyEditorViewModel vm = (ViewModel.dlgKeyEditorViewModel)this.DataContext;
                vm.SelectedKey.ImageSource = se.SelectedImage;
            }

            se = null;
            GC.Collect();
        }

        private void btnStartKeyGen_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.dlgKeyEditorViewModel vm = (ViewModel.dlgKeyEditorViewModel)this.DataContext;
            
            View.dlgPasswortGen dlgpwg = new dlgPasswortGen(this, vm.SelectedKey);
            dlgpwg.ShowDialog();
        }

        private void cboGroups_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0)
            {
                Model.KeyGroup skg = (Model.KeyGroup)e.AddedItems[0];

                if (skg != vmMain.SelectedGroup)
                {
                    if (vmMain.SelectedGroup.KeyEntries.Contains(vm.SelectedKey))
                    {
                        skg.KeyEntries.Add(vm.SelectedKey);
                        vmMain.SelectedGroup.KeyEntries.Remove(vm.SelectedKey);

                        vmMain.SelectedGroup = skg;
                    }

                }

            }
        }
    }
}
