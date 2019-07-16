using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace KeyPortNet.UserCtrl
{
    //TODO: SelectedItemChanged auf original, darin ObjectProperty füllen, diese als Binding für Detailview in GUI
    public class ExtendedTreeView : TreeView
    {
        Point _lastMouseDown;
        Model.KeyGroup draggedItem, _target;
        TreeViewItem RootTreeViewItem;

        public ExtendedTreeView()
            : base()
        {
            this.AllowDrop = true;
            this.SelectedItemChanged += new RoutedPropertyChangedEventHandler<object>(___ICH);

            this.MouseDown += ExtendedTreeView_MouseDown;
            this.MouseMove += ExtendedTreeView_MouseMove;
            this.DragOver += ExtendedTreeView_DragOver;
            this.Drop += ExtendedTreeView_Drop;
            this.SelectedItemChanged += ExtendedTreeView_SelectedItemChanged;
           
        }

        private void ExtendedTreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            Model.KeyGroup kg = (Model.KeyGroup)e.NewValue;
            TreeViewItem tvi = GetItemFromObject(kg);
            RootTreeViewItem = GetRootItem(tvi);

            if (RootTreeViewItem != null)
            {
                System.Diagnostics.Debug.Print(RootTreeViewItem.Header.ToString());
            }
        }

        #region DragnDrop..

        private void ExtendedTreeView_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                _lastMouseDown = e.GetPosition(this);
                //UIElement element = e.OriginalSource as UIElement;
            }
        }

        private void ExtendedTreeView_MouseMove(object sender, MouseEventArgs e)
        {
            try
            {
                if (e.LeftButton == MouseButtonState.Pressed)
                {
                    UIElement element = e.OriginalSource as UIElement;


                    Point currentPosition = e.GetPosition(this);


                    if ((Math.Abs(currentPosition.X - _lastMouseDown.X) > 10.0) ||
                        (Math.Abs(currentPosition.Y - _lastMouseDown.Y) > 10.0))
                    {
                        draggedItem = (Model.KeyGroup)this.SelectedItem;

                        if ((draggedItem != null))
                        {
                            DragDropEffects finalDropEffect = DragDrop.DoDragDrop(this, this.SelectedValue,
                                DragDropEffects.Move);
                            //Checking target is not null and item is dragging(moving)
                            if ((finalDropEffect == DragDropEffects.Move) && (_target != null))
                            {

                                // A Move drop was accepted
                                if (draggedItem.Gruppenname != _target.Gruppenname)
                                {
                                    CopyItem(draggedItem, _target);
                                    

                                    _target = null;
                                    draggedItem = null;
                                }

                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.Print("MouseMove:" + ex.Message.ToString());
            }
        }

        private void ExtendedTreeView_DragOver(object sender, DragEventArgs e)
        {
            try
            {
                Point currentPosition = e.GetPosition(this);

                if ((Math.Abs(currentPosition.X - _lastMouseDown.X) > 10.0) ||
                    (Math.Abs(currentPosition.Y - _lastMouseDown.Y) > 10.0))
                {
                    // Verify that this is a valid drop and then store the drop target
                    Model.KeyGroup item = GetNearestContainer(e.OriginalSource as UIElement);

                    if (item != null)
                    {
                        if (CheckDropTarget(draggedItem, item))
                        {
                            e.Effects = DragDropEffects.Move;
                        }
                        else
                        {
                            e.Effects = DragDropEffects.None;
                        }
                    }
                }

                e.Handled = true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.Print("DragOver:" + ex.Message.ToString());
            }
        }

        private void ExtendedTreeView_Drop(object sender, DragEventArgs e)
        {
            try
            {
                e.Effects = DragDropEffects.None;
                e.Handled = true;

                // Verify that this is a valid drop and then store the drop target
                Model.KeyGroup TargetItem = GetNearestContainer(e.OriginalSource as UIElement);
                if (TargetItem != null && draggedItem != null)
                {
                    _target = TargetItem;
                    e.Effects = DragDropEffects.Move;

                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.Print("Drop:" + ex.Message.ToString());
            }
        }

        private bool CheckDropTarget(Model.KeyGroup _sourceItem, Model.KeyGroup _targetItem)
        {
            //Check whether the target item is meeting your condition
            bool _isEqual = false;

            if (_sourceItem.Gruppenname != _targetItem.Gruppenname)
            {
                _isEqual = true;
            }

            return _isEqual;

        }

        private void CopyItem(Model.KeyGroup _sourceItem, Model.KeyGroup _targetItem)
        {
            if (_sourceItem.Gruppenname == "Root") { return; }

            //Asking user wether he want to drop the dragged TreeViewItem here or not
            if (MessageBox.Show("Wollen sie die Gruppe " + _sourceItem.Gruppenname + " in die Gruppe " + _targetItem.Gruppenname + " verschieben?", "Bearbeitung Gruppen", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                try
                {
                    _targetItem.KeyGroups.Add(_sourceItem);

                    if (RootTreeViewItem != null)
                    {
                        Model.KeyGroup tmp = RootTreeViewItem.DataContext as Model.KeyGroup;

                        tmp.KeyGroups.Remove(_sourceItem);
                    }
                    else
                    {

                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.Print("CopyItem:" + ex.Message.ToString());
                }
            }

        }

        private static Parent FindParent<Parent>(DependencyObject child) where Parent : DependencyObject
        {
            if (child == null) { return null; }

            DependencyObject parentObject = child;
            parentObject = VisualTreeHelper.GetParent(parentObject);

            //check if the parent matches the type we're looking for
            if (parentObject is Parent || parentObject == null)
            {
                return parentObject as Parent;
            }
            else
            {
                //use recursion to proceed with next level
                return FindParent<Parent>(parentObject);
            }
        }

        private Model.KeyGroup GetNearestContainer(UIElement element)
        {
            // Walk up the element tree to the nearest tree view item.
            TreeViewItem UIContainer = FindParent<TreeViewItem>(element);

            Model.KeyGroup NVContainer = null;

            if (UIContainer != null)
            {
                NVContainer = UIContainer.DataContext as Model.KeyGroup;
            }


            return NVContainer;
        }

        private TreeViewItem GetRootItem(UIElement element)
        {
            return FindParent<TreeViewItem>(element);
        }

        #endregion

        /// <summary>
        /// Returns the TreeViewItem of a data bound object.
        /// </summary>
        /// <param name="treeView">TreeView</param>
        /// <param name="obj">Data bound object</param>
        /// <returns>The TreeViewItem of the data bound object or null.</returns>
        private TreeViewItem GetItemFromObject(object obj)
        {
            try
            {

                DependencyObject dObject = GetContainerFormObject(this, obj);
                TreeViewItem tvi = dObject as TreeViewItem;
                while (tvi == null)
                {
                    dObject = VisualTreeHelper.GetParent(dObject);
                    tvi = dObject as TreeViewItem;
                }

                return tvi;
            }
            catch(Exception ex)
            {
                System.Diagnostics.Debug.Print("GetItemFromObject:" + ex.Message.ToString());
            }

            return null;
        }

        private  DependencyObject GetContainerFormObject(ItemsControl item, object obj)
        {
            DependencyObject dObject = null;
            dObject = item.ItemContainerGenerator.ContainerFromItem(obj);
            if (dObject == null)
            {
                if (item.Items.Count > 0)
                {
                    foreach (object childItem in item.Items)
                    {
                        ItemsControl childControl = item.ItemContainerGenerator.
                              ContainerFromItem(childItem) as ItemsControl;
                        dObject = GetContainerFormObject(childControl, obj);
                        if (dObject != null)
                        {
                            break;
                        }
                    }
                }
            }
            return dObject;
        }


        #region SelectedItem..

        void ___ICH(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (SelectedItem != null)
            {
                SetValue(SelectedItem_Property, SelectedItem);
            }
        }

        public object SelectedItem_
        {
            get { return (object)GetValue(SelectedItem_Property); }
            set {SetValue(SelectedItem_Property, value);}
        }

        public static readonly DependencyProperty SelectedItem_Property = DependencyProperty.Register("SelectedItem_", typeof(object), typeof(ExtendedTreeView), new UIPropertyMetadata(null));

        #endregion

    }
}
