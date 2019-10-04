using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace MHWDecorationsModifier.xaml
{
    public partial class MyMessageBox : Window
    {
        private int _archive = 1;

        private ComboBox _comboBox;

        private MyMessageBox()
        {
            InitializeComponent();
            Init();
        }

        private void Init()
        {
            _comboBox = (ComboBox) FindName("ComboBox");
            var categoryList = new List<CategoryInfo>();
            categoryList.Add(new CategoryInfo {Name = "存档1", Value = "archive1"});
            categoryList.Add(new CategoryInfo {Name = "存档2", Value = "archive2"});
            categoryList.Add(new CategoryInfo {Name = "存档3", Value = "archive3"});
            if (_comboBox == null) return;
            _comboBox.ItemsSource = categoryList;
            _comboBox.DisplayMemberPath = "Name";
            _comboBox.SelectedValuePath = "Value";
            _comboBox.SelectedIndex = 0;
        }

        public new static int Show()
        {
            var messageBox = new MyMessageBox()
            {
                Title = "提示"
            };
            messageBox.ShowDialog();
            return messageBox._archive;
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            var select = _comboBox.SelectedValue.ToString();
            switch (select)
            {
                case "archive1":
                    _archive = 1;
                    break;
                case "archive2":
                    _archive = 2;
                    break;
                case "archive3":
                    _archive = 3;
                    break;
                default:
                    _archive = 1;
                    break;
            }
            Close();
        }
    }

    internal class CategoryInfo
    {
        public string Name { get; set; }
        public string Value { get; set; }
    }
}