using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using MHWDecorationsModifier.Code;

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
            var categoryList = new List<CategoryInfo>
            {
                new CategoryInfo {Name = "存档1", Value = "archive1"},
                new CategoryInfo {Name = "存档2", Value = "archive2"},
                new CategoryInfo {Name = "存档3", Value = "archive3"}
            };
            if (_comboBox == null) return;
            _comboBox.ItemsSource = categoryList;
            _comboBox.DisplayMemberPath = "Name";
            _comboBox.SelectedValuePath = "Value";
            _comboBox.SelectedIndex = 0;
        }

        /// <summary>
        /// 显示方法
        /// 在此将类新建并显示窗口，返回选择的存档号
        /// </summary>
        /// <returns>选择的存档号</returns>
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
                    _archive = JsonHandler.Archive1;
                    break;
                case "archive2":
                    _archive = JsonHandler.Archive2;
                    break;
                case "archive3":
                    _archive = JsonHandler.Archive3;
                    break;
                default:
                    _archive = JsonHandler.Archive1;
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