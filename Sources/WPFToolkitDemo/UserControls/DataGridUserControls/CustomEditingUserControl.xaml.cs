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
using System.Windows.Navigation;
using System.Windows.Shapes;
using WPFToolkit.DragDrop;

namespace WPFToolkitDemo.UserControls.DataGridUserControls
{
    /// <summary>
    /// 实现自定义编辑效果
    /// 不使用DataGrid的自带编辑功能
    /// 编辑的时候显示TextBox，不编辑的时候隐藏TextBox显示TextBlock，TextBox和TextBlock在同一个CellDataTemplate里
    /// </summary>
    public partial class CustomEditingUserControl : UserControl
    {
        public CustomEditingUserControl()
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

            DataGridEditing.ItemsSource = items;
        }

        private void ShowMessage(string message, params object[] objects)
        {
            string messageText = string.Format(message, objects);
            TextBoxMessage.AppendText(string.Format("{0}\r\n", messageText));
            ScrollViewer1.ScrollToEnd();
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

        private void ButtonEdit_Click(object sender, RoutedEventArgs e)
        {
            DataGridItem dataGridItem = DataGridEditing.SelectedItem as DataGridItem;
            if (dataGridItem == null)
            {
                return;
            }

            DataGridRow dataGridRow = DataGridEditing.ItemContainerGenerator.ContainerFromItem(dataGridItem) as DataGridRow;

            FrameworkElement frameworkElement = DataGridTemplateColumnName.GetCellContent(dataGridRow);

            ContentPresenter contentPresenter = frameworkElement as ContentPresenter;

            TextBox textBox = contentPresenter.ContentTemplate.FindName("TextBoxEditName", contentPresenter) as TextBox;
            textBox.PreviewTextInput += this.TextBox_PreviewTextInput;
            textBox.Visibility = Visibility.Visible;
            textBox.Focus();
            textBox.SelectAll();
        }

        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (e.Text == "1")
            {
                e.Handled = true;
            }
        }

        /// <summary>
        /// 当TextBox失去焦点的时候隐藏编辑并且结束编辑
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox textBox = sender as TextBox;

            // 相当于EditEnding事件
            if (string.IsNullOrEmpty(textBox.Text))
            {
            }

            textBox.LostFocus -= this.TextBox_LostFocus;
            textBox.Visibility = Visibility.Collapsed;
        }
    }
}