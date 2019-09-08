using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.Xml.Serialization;

namespace KeyPortNet.Model
{
    [Serializable]
    public class DataBase:ViewModel.ViewModelBase
    {
        [XmlIgnore]
        private DBAttribute AttributeField = new DBAttribute();
        [XmlIgnore]
        private KeyGroupList KeyGroupsField = new KeyGroupList();

        public DBAttribute Attribute
        {
            get
            {
                return AttributeField;
            }

            set
            {
                AttributeField = value;
                RaisePropertyChangedEvent("DBAttribute");
            }
        }

        public KeyGroupList KeyGroups
        {
            get
            {
                return KeyGroupsField;
            }

            set
            {
                KeyGroupsField = value;

                RaisePropertyChangedEvent("KeyGroups");
            }
        }

        public DataBase()
        {
            

        }

        [XmlIgnore]
        public List<KeyGroup> GetGroupList
        {
            get
            {
                List<KeyGroup> results = new List<KeyGroup>();

                GetRecursive(this.KeyGroups,ref results);

                return results;
            }
        }

        private void GetRecursive(KeyGroupList root, ref List<KeyGroup> results)
        {
            foreach (KeyGroup kg in root)
            {
                results.Add(kg);

                if (kg.KeyGroups.Count > 0)
                {
                    GetRecursive(kg.KeyGroups, ref results);
                }
            }

        }
    }

    [Serializable]
    public class DBAttribute : ViewModel.ViewModelBase
    {
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string Crc32Field;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string UserField;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string ComputerField;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private DateTime LastWriteField;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string LastSavePathField;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private DateTime LastPWChangeField;

        public string Crc32
        {
            get
            {
                return Crc32Field;
            }

            set
            {
                Crc32Field = value;
                RaisePropertyChangedEvent("Crc32");
            }
        }

        public string User
        {
            get
            {
                return UserField;
            }

            set
            {
                UserField = value;
                RaisePropertyChangedEvent("User");
            }
        }

        public string Computer
        {
            get
            {
                return ComputerField;
            }

            set
            {
                ComputerField = value;
            }
        }

        public DateTime LastWrite
        {
            get
            {
                return LastWriteField;
            }

            set
            {
                LastWriteField = value;
                RaisePropertyChangedEvent("LastWrite");
            }
        }

        public string LastSavePath
        {
            get
            {
                return LastSavePathField;
            }

            set
            {
                LastSavePathField = value;
                RaisePropertyChangedEvent("LastSavePath");
            }
        }

        public DateTime LastPWChange
        {
            get
            {
                return LastPWChangeField;
            }

            set
            {
                LastPWChangeField = value;
                RaisePropertyChangedEvent("LastPWChange");
            }
        }
    }

    [Serializable]
    public class KeyGroupList : ObservableCollection<KeyGroup>
    {
        public KeyGroup this[string Key]
        {
            get
            {
                KeyGroup result = this.First(gruppe => gruppe.Gruppenname.Equals(Key));

                return result;
            }
            set
            {
                KeyGroup k = this.First(gruppe => gruppe.Gruppenname.Equals(Key));
                k = value;
            }
        }
    }

    [Serializable]
    public class KeyGroup : ViewModel.ViewModelBase
    {
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string ImageSourceField;

        public string ImageSource
        {
            get
            {
                return ImageSourceField;
            }

            set
            {
                this.ImageSourceField = value;
                RaisePropertyChangedEvent("ImageSource");
            }
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string GruppennameField;
        [XmlAttribute]
        public string Gruppenname
        {
            get
            {
                return GruppennameField;
            }

            set
            {
                GruppennameField = value;
                RaisePropertyChangedEvent("Gruppenname");
            }
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private ObservableCollection<KeyEntry> KeyEntriesField = new ObservableCollection<KeyEntry>();

        public ObservableCollection<KeyEntry> KeyEntries
        {
            get
            {
                return KeyEntriesField;
            }

            set
            {
                KeyEntriesField = value;
                RaisePropertyChangedEvent("KeyEntries");
            }
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private KeyGroupList KeyGroupsField = new KeyGroupList();

        public KeyGroupList KeyGroups
        {
            get
            {
                return KeyGroupsField;
            }

            set
            {
                KeyGroupsField = value;
                RaisePropertyChangedEvent("KeyGroups");
            }
        }

        public KeyGroup(){ }

        public KeyGroup(string gruppenname)
        {
            this.Gruppenname = gruppenname;
        }

        public override string ToString()
        {
            return Gruppenname;
        }
    }

    [Serializable]
    public class KeyEntry : ViewModel.ViewModelBase
    {
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string CRCField;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string ImageSourceField = "/KeyPortNet;component/Resources/key_add.png";
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string TitelField;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string BenutzernameField;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string PasswortField;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string URLField;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string KommentarField;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private DateTime Anlegedatum_Field = DateTime.Now;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private DateTime Aenderungsdatum_Field = DateTime.Now;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private DateTime Zugriffsdatum_Field = DateTime.Now;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private DateTime Ablaufdatum_Field = DateTime.Now;


        public string ImageSource
        {
            get
            {
                return ImageSourceField;
            }

            set
            {
                ImageSourceField = value;
                RaisePropertyChangedEvent("ImageSource");
            }
        }

        [XmlAttribute]
        public string Titel
        {
            get
            {
                return TitelField;
            }

            set
            {
                TitelField = value;
                RaisePropertyChangedEvent("Titel");
            }
        }

        public string Benutzername
        {
            get
            {
                return BenutzernameField;
            }

            set
            {
                BenutzernameField = value;
                RaisePropertyChangedEvent("Benutzername");
            }
        }

        public string Passwort
        {
            get
            {
                return PasswortField;
            }

            set
            {
                PasswortField = value;
                RaisePropertyChangedEvent("Passwort");
            }
        }
        [XmlIgnore]
        public string PasswortHide
        {

            get {

                if (string.IsNullOrEmpty(Passwort))
                {
                    return string.Empty;
                }
                else

                    return new string('•', Passwort.Length);
            }
        }

        public string URL
        {
            get
            {
                return URLField;
            }

            set
            {
                URLField = value;
                RaisePropertyChangedEvent("URL");
            }
        }

        public string Kommentar
        {
            get
            {
                return KommentarField;
            }

            set
            {
                KommentarField = value;
                RaisePropertyChangedEvent("Kommentar");
            }
        }

        public DateTime Anlegedatum
        {
            get
            {
                return Anlegedatum_Field;
            }

            set
            {
                Anlegedatum_Field = value;
                RaisePropertyChangedEvent("Anlegedatum");
            }
        }

        public DateTime Aenderungsdatum
        {
            get
            {
                return Aenderungsdatum_Field;
            }

            set
            {
                Aenderungsdatum_Field = value;
                RaisePropertyChangedEvent("Aenderungsdatum");
            }
        }

        public DateTime Ablaufdatum
        {
            get
            {
                return Ablaufdatum_Field;
            }

            set
            {
                Ablaufdatum_Field = value;
                RaisePropertyChangedEvent("Ablaufdatum");
            }
        }
        [XmlAttribute]
        public DateTime Zugriffsdatum
        {
            get
            {
                return Zugriffsdatum_Field;
            }

            set
            {
                Zugriffsdatum_Field = value;
                RaisePropertyChangedEvent("Zugriffsdatum");
            }
        }

        [XmlAttribute]
        public string CRC
        {
            get
            {
                return CRCField;
            }

            set
            {
                CRCField = value;
                RaisePropertyChangedEvent("CRC");
            }
        }

        public KeyEntry() { }

        public KeyEntry(string titel)
        {
            this.Titel = titel;
        }

        public string GetHash()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(ImageSource);
            sb.AppendLine(Titel);
            sb.AppendLine(Benutzername);
            sb.AppendLine(Passwort);
            sb.AppendLine(URL);
            sb.AppendLine(Kommentar);
            sb.AppendLine(Anlegedatum.ToString());
            sb.AppendLine(Aenderungsdatum.ToString());
            //sb.AppendLine(Zugriffsdatum.ToString()); Keine Änderung!
            sb.AppendLine(Ablaufdatum.ToString());

            Crc32 crc = new Crc32();
            return BitConverter.ToString(crc.ComputeHash(ObjectToByteArray(sb.ToString()))).Replace("-", string.Empty).ToUpper();
        }
        // Convert an object to a byte array
        private  byte[] ObjectToByteArray(Object obj)
        {
            BinaryFormatter bf = new BinaryFormatter();
            using (var ms = new MemoryStream())
            {
                bf.Serialize(ms, obj);
                return ms.ToArray();
            }
        }

        public KeyEntry Clone()
        {
            KeyEntry ke = new KeyEntry(Titel);
            ke.ImageSource = ImageSource;
            ke.Benutzername = Benutzername;
            ke.Passwort = Passwort;
            ke.URL = URL;
            ke.Kommentar = Kommentar;
            ke.Anlegedatum = Anlegedatum;
            ke.Aenderungsdatum = Aenderungsdatum;
            ke.Ablaufdatum = Ablaufdatum;
            ke.Zugriffsdatum = Ablaufdatum;
            ke.CRC = CRC;

            return ke;
        }

        public void GetValues(KeyEntry Source)
        {
            ImageSource = Source.ImageSource;
            Titel = Source.Titel;
            Benutzername = Source.Benutzername;
            Passwort = Source.Passwort;
            URL = Source.URL;
            Kommentar = Source.Kommentar;
            Anlegedatum = Source.Anlegedatum;
            Aenderungsdatum = Source.Aenderungsdatum;
            Ablaufdatum = Source.Ablaufdatum;
            Zugriffsdatum = Source.Zugriffsdatum;
            CRC = Source.CRC;

        }

        public override string ToString()
        {
            return this.Titel;
        }
    }
}
