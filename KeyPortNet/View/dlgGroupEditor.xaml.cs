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

namespace KeyPortNet.View
{
    /// <summary>
    /// Interaktionslogik für dlgGroupEditor.xaml
    /// </summary>
    public partial class dlgGroupEditor : Window
    {
        public Model.DataBase DB { get; set; }

        public Model.KeyGroup KG { get; set; }

        public dlgGroupEditor(Model.DataBase db, Model.KeyGroup kg)
        {
            this.DataContext = KG;
            InitializeComponent();
        }

        private void btnOK_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
