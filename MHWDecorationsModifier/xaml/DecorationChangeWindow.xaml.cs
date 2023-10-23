using System;
using System.Collections.Generic;
using System.Windows;
using MHWDecorationsModifier.Beans;
using MHWDecorationsModifier.Code;
using NLog;

namespace MHWDecorationsModifier.xaml
{
    /// <summary>
    /// DecorationChangeWindow.xaml 的交互逻辑
    /// </summary>
    public partial class DecorationChangeWindow
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly DecorationBean _decoration;
        private readonly JsonHandler _jsonHandler = new JsonHandler();
        private readonly MemoryHandler _memoryHandler;

        public DecorationChangeWindow(DecorationBean decoration, MemoryHandler memoryHandler)
        {
            InitializeComponent();
            _decoration = decoration;
            _memoryHandler = memoryHandler;
            Title = decoration.Name;
            Closed += (sender, args) => { Owner.Activate(); };
            Init();
        }

        private void Init()
        {
            var categoryList = new List<CategoryInfo>();
            var allDecorations = _jsonHandler.ReadAllDecorations();
            var count = 0;
            var selectIndex = 0;
            foreach (var dec in allDecorations)
            {
                categoryList.Add(new CategoryInfo { Name = dec.Value, Value = dec.Key.ToString() });
                if (dec.Key == _decoration.Code)
                {
                    selectIndex = count;
                }

                count++;
            }

            SelectDecoration.ItemsSource = categoryList;
            SelectDecoration.DisplayMemberPath = "Name";
            SelectDecoration.SelectedValuePath = "Value";
            SelectDecoration.SelectedIndex = selectIndex;
            DecorationNumber.Text = _decoration.Number.ToString();
        }

        private void Submit_OnClick(object sender, RoutedEventArgs e)
        {
            var name = SelectDecoration.Text;
            var decCode = Convert.ToInt32(SelectDecoration.SelectedValue.ToString());
            var number = Convert.ToInt32(DecorationNumber.Text);
            var newDecoration = new DecorationBean(name, decCode, number, _decoration.Address);
            var res = _memoryHandler.ChangeDecoration(newDecoration);
            Logger.Debug($"\r\n将\t\t{_decoration}\r\n更改为\t{newDecoration}\r\n更改结果：{res}");
            ((MainWindow)Owner).ForceRefreshUi();
            Close();
        }
    }
}