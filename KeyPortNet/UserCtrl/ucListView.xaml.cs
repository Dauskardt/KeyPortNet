using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace KeyPortNet.UserCtrl
{
    /// <summary>
    /// Interaktionslogik für ucListView.xaml
    /// </summary>
    public partial class ucListView : UserControl
    {
        public ucListView()
        {
            acStartURL = new ViewPortNet.ViewModel.MyCommand();
            acStartURL.CanExecuteFunc = obj => true;
            acStartURL.ExecuteFunc = acStartURLFunc;

            InitializeComponent();
        }


        public ViewPortNet.ViewModel.MyCommand acStartURL
        {
            get;
            set;
        }

        public void acStartURLFunc(object parameter)
        {
            try
            {
                System.Diagnostics.Process.Start(parameter.ToString());
            }
            catch (Exception){}
            
        }

        public IEnumerable ItemsSource
        {
            get { return (IEnumerable)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        public static readonly DependencyProperty ItemsSourceProperty =
            DependencyProperty.Register("ItemsSource", typeof(IEnumerable),
                typeof(ucListView), new PropertyMetadata(null));



        protected void HandleDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var dataContext = ((FrameworkElement)e.OriginalSource).DataContext;

            if (dataContext != null)
            {
                Window parentWindow = Window.GetWindow(this);

                Model.KeyEntry OriginalKey = (Model.KeyEntry)dataContext;

                //Dialog mit Temporärem Key starten..
                View.dlgKeyEditor ke = new View.dlgKeyEditor(OriginalKey, parentWindow);

                if ((bool)ke.ShowDialog())
                {

                }
            }

        }

        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var dataContext = ((FrameworkElement)e.OriginalSource).DataContext;

            if (dataContext != null)
            {
                if (e.AddedItems.Count > 0)
                {
                    Window parentWindow = Window.GetWindow(this);
                    ViewModel.MainWindowViewModel mvm = (ViewModel.MainWindowViewModel)parentWindow.DataContext;
                    mvm.SelectedKey = (Model.KeyEntry)e.AddedItems[0];

                }

            }
        }
    }
}
