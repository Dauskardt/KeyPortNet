using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KeyPortNet.Model;
using ViewPortNet.ViewModel;
using Microsoft.Win32;
using System.Windows;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Globalization;
using System.Collections;
using System.Diagnostics;

namespace KeyPortNet.ViewModel
{
    class MainWindowViewModel:ViewModelBase
    {
        private CryptoSerializer.GenericCryptoClass CS = new CryptoSerializer.GenericCryptoClass();

        private string DBPWField = string.Empty;

        public Properties.Settings ProgrammSettings
        {
            get { return Properties.Settings.Default;  }
        }



        public string DBPW
        {
            set
            {
                DBPWField = value;
                RaisePropertyChangedEvent("DBPW");
            }
        }


        public MyCommand acOpenDB
        {
            get;
            set;
        }

        public void afOpenDB(object parameter)
        {
            if (string.IsNullOrEmpty(DBPWField))
            {
                //TODO: Wenn parameter leer, dann hier Abfrage Masterkey für DB (Dialog Schlüsseleingabe)
            }

            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Title = "Datenbank öffnen..";
            ofd.AddExtension = true;
            ofd.Filter = "Datenbanken (*.kpn)|*.kpn|Alle Dateien (*.*)|*.*";
            ofd.RestoreDirectory = true;

            try
            {
                if ((bool)ofd.ShowDialog())
                {
                    DB = Helper.IOHelper.LoadDB(ofd.FileName, DBPWField);
                }
            }
            catch (Exception ex)
            {
                DB = null;
                MessageBox.Show("Unbekanntes Format oder defekte Datei!\r\n\r\nIntern:\r\n" + ex.Message.ToString(), "IO-Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }


        public MyCommand acSaveDB
        {
            get;
            set;
        }

        public void afSaveDB(object parameter)
        {

            if (string.IsNullOrEmpty(DBPWField))
            {
                //TODO: Wenn parameter leer, dann hier Abfrage Masterkey für DB (Dialog Schlüsseleingabe)
            }

            if (parameter == null)
            {
                string dbpath = KeyPortNet.Properties.Settings.Default.DBPfad;

                if (!string.IsNullOrEmpty(dbpath))
                {
                    Helper.IOHelper.SaveDB(dbpath, DBPWField, DB);
                }
            }


            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Title = "Datenbank öffnen..";
            sfd.AddExtension = true;
            sfd.Filter = "Datenbanken (*.kpn)|*.kpn|Alle Dateien (*.*)|*.*";
            sfd.RestoreDirectory = true;

            try
            {
                if ((bool)sfd.ShowDialog())
                {
                    Helper.IOHelper.SaveDB(sfd.FileName, DBPWField, DB);
                }
            }
            catch (Exception ex)
            {
                DB = null;
                MessageBox.Show("Unbekanntes Format oder defekte Datei!\r\n\r\nIntern:\r\n" + ex.Message.ToString(), "IO-Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }


        public MyCommand acNewDB
        {
            get;
            set;
        }
        
        public void afNewDB(object parameter)
        {

            if (string.IsNullOrEmpty(DBPWField))
            {
                //TODO: Wenn parameter leer, dann hier Abfrage Masterkey für DB (Dialog Schlüsseleingabe)
            }

            DB = new DataBase();
            DB.KeyGroups.Add(new KeyGroup("Root"));
            DB.KeyGroups["Root"].KeyGroups.Add(new KeyGroup("Allgemein"));
            DB.KeyGroups["Root"].KeyGroups.Add(new KeyGroup("Windows"));
            DB.KeyGroups["Root"].KeyGroups.Add(new KeyGroup("Internet"));
            DB.KeyGroups["Root"].KeyGroups["Allgemein"].KeyGroups.Add(new KeyGroup("Privat"));
            DB.KeyGroups["Root"].KeyGroups["Internet"].KeyGroups.Add(new KeyGroup("Homepage"));
            DB.KeyGroups["Root"].KeyGroups["Internet"].KeyGroups["Homepage"].KeyGroups.Add(new KeyGroup("Admin"));

            DB.KeyGroups["Root"].KeyGroups["Allgemein"].KeyGroups["Privat"].KeyEntries.Add(new KeyEntry("Döner"));

            DB.KeyGroups["Root"].KeyGroups["Allgemein"].KeyEntries.Add(new KeyEntry("Systempasswort"));
            DB.KeyGroups["Root"].KeyGroups["Internet"].KeyEntries.Add(new KeyEntry("Sparkasse"));
           
            //OpenFileDialog ofd = new OpenFileDialog();
            //ofd.Title = "Datenbank öffnen..";
            //ofd.AddExtension = true;
            //ofd.Filter = "Datenbanken (*.kpn)|*.kpn|Alle Dateien (*.*)|*.*";
            //ofd.RestoreDirectory = true;

            //try
            //{
            //    if ((bool)ofd.ShowDialog())
            //    {
            //        DB = CS.CryptoDeSerialize<DataBase>(ofd.FileName, (string)parameter);
            //    }
            //}
            //catch (Exception ex)
            //{
            //    DB = null;
            //    MessageBox.Show("Unbekanntes Format oder defekte Datei!\r\n\r\nIntern:\r\n" + ex.Message.ToString(), "IO-Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
            //}

        }

        public MyCommand acCloseDB
        {
            get;
            set;
        }

        public void afCloseDB(object parameter)
        {
            SelectedKey = null;
            SelectedGroup = null;
            DB = null;
        }

        public MyCommand acEditGroup
        {
            get;
            set;
        }

        public void afEditGroup(object parameter)
        {
            View.dlgGroupEditor dlgGE = new View.dlgGroupEditor(DB, SelectedGroup);

            if ((bool)dlgGE.ShowDialog())
            {

            }

        }


        public MyCommand acCloseApp
        {
            get;
            set;
        }

        public void afCloseApp(object parameter)
        {
            if (MessageBox.Show("Programm beenden?", "Goodby", MessageBoxButton.YesNo,
                MessageBoxImage.Question) == MessageBoxResult.Yes){ Environment.Exit(0);}
        }

        public MyCommand acNewKey
        {
            get;
            set;
        }

        public void afNewKey(object parameter)
        {
            if (SelectedGroup != null)
            {
                this.SelectedGroup.KeyEntries.Add(new KeyEntry("Neuer Schlüssel"));
            }
        }

        public MyCommand acDeleteKey
        {
            get;
            set;
        }

        public void afDeleteKey(object parameter)
        {
            if (SelectedKey != null)
            {
                this.SelectedGroup.KeyEntries.Remove(SelectedKey);
            }
        }

        public MyCommand acStartBrowser
        {
            get;
            set;
        }

        public void afStartBrowser(object parameter)
        {
            Process.Start(parameter.ToString());
        }

        private Model.DataBase DBField;

        public DataBase DB
        {
            get
            {
                return DBField;
            }

            set
            {
                DBField = value;
                RaisePropertyChangedEvent("DB");
            }
        }


        private Model.KeyGroup selectedGroupField;

        public KeyGroup SelectedGroup
        {
            get
            {
                return selectedGroupField;
            }

            set
            {
                selectedGroupField = value;
                SelectedKey = null;
                RaisePropertyChangedEvent("SelectedGroup");
            }
        }


        private Model.KeyEntry SelectedKeyField;

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


        public MainWindowViewModel()
        {
            acOpenDB = new MyCommand();
            acOpenDB.CanExecuteFunc = obj => true;
            acOpenDB.ExecuteFunc = afOpenDB;

            acSaveDB = new MyCommand();
            acSaveDB.CanExecuteFunc = obj => true;
            acSaveDB.ExecuteFunc = afSaveDB;

            acNewDB = new MyCommand();
            acNewDB.CanExecuteFunc = obj => true;
            acNewDB.ExecuteFunc = afNewDB;

            acCloseDB = new MyCommand();
            acCloseDB.CanExecuteFunc = obj => true;
            acCloseDB.ExecuteFunc = afCloseDB;

            acEditGroup = new MyCommand();
            acEditGroup.CanExecuteFunc = obj => true;
            acEditGroup.ExecuteFunc = afEditGroup;

            acCloseApp = new MyCommand();
            acCloseApp.CanExecuteFunc = obj => true;
            acCloseApp.ExecuteFunc = afCloseApp;

            acNewKey = new MyCommand();
            acNewKey.CanExecuteFunc = obj => true;
            acNewKey.ExecuteFunc = afNewKey;

            acDeleteKey = new MyCommand();
            acDeleteKey.CanExecuteFunc = obj => true;
            acDeleteKey.ExecuteFunc = afDeleteKey;

            acStartBrowser = new MyCommand();
            acStartBrowser.CanExecuteFunc = obj => true;
            acStartBrowser.ExecuteFunc = afStartBrowser;

            string dbpath = KeyPortNet.Properties.Settings.Default.DBPfad;

            if (!string.IsNullOrEmpty(dbpath))
            {
                if (System.IO.File.Exists(dbpath))
                {
                    int to = 0;

                    while (PasswortInput(dbpath) == false && to < 2)
                    {
                        to++;
                    }
                }
            }

            GetAllIcons();
        }

        public List<string> GetAllIcons()
        {
            List<string> IconsInRessource = new List<string>();

            var images = Properties.Resources.ResourceManager
                       .GetResourceSet(CultureInfo.CurrentCulture, true, true)
                       .Cast<DictionaryEntry>()
                       .Where(x => x.Value.GetType() == typeof(Bitmap))
                       .Select(x => new { Name = x.Key.ToString(), Image = x.Value })
                       .ToList();

            for (int i = 0; i < images.Count; i++)
            {
                if (images[i].Name.StartsWith("_"))
                {

                }
                IconsInRessource.Add("/KeyPortNet;component/Resources/" + images[i].Name.ToString() + ".png"); //"/KeyPortNet;component/Resources/" +
                 // <Image Source = "/KeyPortNet;component/Resources/disk.png" Stretch = "None" />
                 IconsInRessource.Sort();
            }

            return IconsInRessource;
        }

        private bool PasswortInput(string dbpath)
        {
            View.dlgPasswortInput dlgpw = new View.dlgPasswortInput();
            dlgPasswortInputViewModel vm = (dlgPasswortInputViewModel)dlgpw.DataContext;
            vm.HeaderTextTop = System.IO.Path.GetFileName(dbpath);

            if ((bool)dlgpw.ShowDialog())
            {
                //DB = Helper.IOHelper.LoadDB(dbpath, "1234567890");
                //TODO: Passwortabfrage
                try
                {
                    DB = Helper.IOHelper.LoadDB(dbpath, dlgpw.txtPW.Password);

                }
                catch (Exception)
                {
                    if (MessageBox.Show("Falsches Passwort!", "Fehler", MessageBoxButton.OKCancel, MessageBoxImage.Asterisk) == MessageBoxResult.OK)
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }

                return true;
            }
            else
            {
                return true;
            }
        }

    }
}
