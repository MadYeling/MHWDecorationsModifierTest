using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using MHWDecorationsModifier.Beans;
using MHWDecorationsModifier.Code;

namespace MHWDecorationsModifier.xaml
{
    public partial class ChooseArchiveWindow
    {
        private MemoryBean _archive;

        private ComboBox _comboBox;

        private readonly MemoryHandler _memoryHandler;
        private readonly string[] _names = new string[3];
        private readonly MemoryBean[] _memoryInfos;

        public ChooseArchiveWindow(MemoryBean[] memoryInfos)
        {
            InitializeComponent();
            _memoryHandler = new MemoryHandler();
            _memoryInfos = memoryInfos;
            Closing += OnClosing;
            Init();
        }

        private void OnClosing(object sender, CancelEventArgs e)
        {
            if (MessageBox.Show("是否退出应用？", "提示", MessageBoxButton.YesNo, MessageBoxImage.Warning) ==
                MessageBoxResult.Yes)
            {
                e.Cancel = false;
                // 关闭应用
                Environment.Exit(0);
            }
            else
            {
                e.Cancel = true;
            }
        }

        private void Init()
        {
            for (var i = 0; i < _names.Length; i++)
            {
                _names[i] = _memoryHandler.GetPlayerName(_memoryInfos[i]);
            }

            _comboBox = (ComboBox)FindName("ComboBox");
            var categoryList = new List<CategoryInfo>
            {
                new CategoryInfo { Name = $"存档1：{_names[0]}", Value = 0 },
                new CategoryInfo { Name = $"存档2：{_names[1]}", Value = 1 },
                new CategoryInfo { Name = $"存档3：{_names[2]}", Value = 2 }
            };
            if (_comboBox == null) return;
            _comboBox.ItemsSource = categoryList;
            _comboBox.DisplayMemberPath = "Name";
            _comboBox.SelectedValuePath = "Value";
            _comboBox.SelectedIndex = 0;
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            var select = (int)_comboBox.SelectedValue;
            new MainWindow(this, _memoryInfos[select]).Show();
            Hide();
        }
    }


    internal class CategoryInfo
    {
        public string Name { get; set; }
        public int Value { get; set; }
    }
}