using System;
using System.Collections;
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
using System.Windows.Resources;
using System.Windows.Shapes;

namespace KeyPortNet.View
{
    /// <summary>
    /// Interaktionslogik für dlgSelectImage.xaml
    /// </summary>
    public partial class dlgSelectImage : Window
    {
        public string SelectedImage { get; set; }

        public dlgSelectImage(List<string> Images, Window owner)
        {
            InitializeComponent();
            this.Owner = owner;

            foreach (string item in Images)
            {
                Image img = new Image();
                img.Stretch = Stretch.None;
                img.Source = GetImage(item);

                Button btn = new Button();
                btn.Margin = new Thickness(3,3,3,3);
                btn.Width = 30;
                btn.Height = 30;
                btn.Click += Btn_Click;
                btn.Content = img;
                btn.Tag = item;
                btn.ToolTip = System.IO.Path.GetFileName(item);
                btn.Background = new SolidColorBrush( Color.FromRgb(220, 220, 220));
                this.IconContainer.Children.Add(btn);
            }
        }

        private void Btn_Click(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)e.OriginalSource;
            SelectedImage = btn.Tag.ToString();

        }

        private ImageSource GetImage(string channel)
        {
            StreamResourceInfo sri = Application.GetResourceStream(new Uri(channel, UriKind.Relative));
            BitmapImage bmp = new BitmapImage();
            bmp.BeginInit();
            bmp.StreamSource = sri.Stream;
            bmp.EndInit();

            return bmp;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
