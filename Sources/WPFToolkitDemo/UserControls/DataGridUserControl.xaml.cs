using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using WPFToolkit.Attributes;
using WPFToolkit.DragDrop;
using WPFToolkit.MVVM;

namespace WPFToolkitDemo.UserControls
{
    public class DataGridItem
    {
        [DataGridColumn("名字", DataTemplateKey = "DataTemplateTest")]
        public string Name { get; set; }

        [DataGridColumn("编号")]
        public string ID { get; set; }

        public string EditName { get; set; }

        public DataGridItem()
        {
            this.Name = Guid.NewGuid().ToString();
            this.EditName = this.Name;
            this.ID = Guid.NewGuid().ToString();
        }
    }

    public partial class DataGridUserControl : UserControl
    {
        private TreeViewModel<TreeViewModelContext> treeViewModel;

        public DataGridUserControl()
        {
            InitializeComponent();

            this.InitializeUserControl();
        }

        private void InitializeUserControl()
        {
            List<DataGridItem> items = new List<DataGridItem>();

            for (int i = 0; i < 100; i++)
            {
                items.Add(new DataGridItem());
            }

            DataGrid1.ItemsSource = items;
            DataGridEditing.ItemsSource = items;

            DataGridBindTreeViewModel.ItemsSource = DataSource.TreeVM.Roots;
        }

        private void DataGridEditing_BeginningEdit(object sender, DataGridBeginningEditEventArgs e)
        {
            this.ShowMessage("BeginningEdit");

            // 阻止双击进入编辑模式
            MouseButtonEventArgs mouseButtonEventArgs = e.EditingEventArgs as MouseButtonEventArgs;
            if (mouseButtonEventArgs.ClickCount == 2) 
            {
                e.Cancel = true;
            }
        }

        private void DataGridEditing_PreparingCellForEdit(object sender, DataGridPreparingCellForEditEventArgs e)
        {
            this.ShowMessage("PreparingCellForEdit");

            ContentPresenter contentPresenter = e.EditingElement as ContentPresenter;
            TextBox textBox = contentPresenter.ContentTemplate.FindName("TextBoxEditName", contentPresenter) as TextBox;
            textBox.Focus();
            textBox.SelectAll();
        }

        private void DataGridEditing_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            this.ShowMessage("CellEditEnding, {0}", e.EditAction);

            if (e.EditAction == DataGridEditAction.Commit)
            {
                DataGridItem dataGridItem = e.EditingElement.DataContext as DataGridItem;
                if (string.IsNullOrEmpty(dataGridItem.EditName))
                {
                    MessageBox.Show("请输入正确的数据");
                    e.Cancel = true;
                    return;
                }
            }
            else if (e.EditAction == DataGridEditAction.Cancel)
            {
            }
        }

        private void TextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.A && Keyboard.Modifiers == ModifierKeys.Control)
            {
                if (sender is TextBox tb)
                {
                    tb.SelectAll();     // 选中所有文本
                    e.Handled = true;   // 阻止事件冒泡到 DataGrid
                }
            }
        }

        private void ShowMessage(string message, params object[] objects)
        {
            string messageText = string.Format(message, objects);
            TextBoxMessage.AppendText(string.Format("{0}\r\n", messageText));
            ScrollViewer1.ScrollToEnd();
        }

        private void DataGridEditing_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            TextBox textBox = VisualTreeExtensions.GetVisualAncestor<TextBox>(e.OriginalSource as DependencyObject);
            if (textBox != null)
            {
                e.Handled = true;
                return;
            }

            DataGridItem dataGridItem = DataGridEditing.SelectedItem as DataGridItem;
            if (dataGridItem == null)
            {
                return;
            }

            MessageBox.Show(dataGridItem.Name);
        }

        private void ButtonFindTextBox_Click(object sender, RoutedEventArgs e)
        {
            DataGridRow dataGridRow = DataGridEditing.ItemContainerGenerator.ContainerFromIndex(0) as DataGridRow;

            FrameworkElement frameworkElement = DataGridTemplateColumnName.GetCellContent(dataGridRow);

            ContentPresenter contentPresenter = frameworkElement as ContentPresenter;

            TextBlock textBox = contentPresenter.ContentTemplate.FindName("TextBlockName", contentPresenter) as TextBlock;

            if (textBox != null) 
            {
                MessageBox.Show(textBox.Text);
            }
        }

        private void ButtonGetSelectedItem_Click(object sender, RoutedEventArgs e)
        {
            TreeNodeViewModel selectedItem = DataSource.TreeVM.SelectedItem;

            Console.WriteLine();
        }
    }
}
