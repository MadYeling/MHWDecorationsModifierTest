using System.Windows;
using System.Windows.Controls;
using MHWDecorationsModifier.Beans;
using MHWDecorationsModifier.Code;
using NLog;

namespace MHWDecorationsModifier.xaml
{
    /// <summary>
    /// UserControl1.xaml 的交互逻辑
    /// </summary>
    public partial class UserControl1 : UserControl
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private DecorationBean _decoration;
        private readonly Window _owner;
        private readonly MemoryHandler _memoryHandler;


        public UserControl1(Window owner, MemoryHandler memoryHandler)
        {
            InitializeComponent();
            _owner = owner;
            _memoryHandler = memoryHandler;
        }

        public void SetDecoration(DecorationBean decoration)
        {
            _decoration = decoration;
            DecrorationName.Text = _decoration.Name;
            DecrorationNumber.Content = _decoration.Number;
            FakeButton.Visibility = _decoration.Address == -1 ? Visibility.Hidden : Visibility.Visible;
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            Logger.Debug($"{_decoration.Name} 按钮被点击");
            var dialog = new DecorationChangeWindow(_decoration, _memoryHandler) { Owner = _owner };
            dialog.Show();
        }
    }
}