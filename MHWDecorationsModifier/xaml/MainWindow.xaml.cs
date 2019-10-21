using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls.Primitives;
using MHWDecorationsModifier.Beans;
using MHWDecorationsModifier.Code;

namespace MHWDecorationsModifier.xaml
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private readonly int _archive;

        private UniformGrid _uniformGrid;

        public MainWindow()
        {
            InitializeComponent();
            Closing += Window_Closing;
            _archive = MyMessageBox.Show();
            Init();
        }

        private void Init()
        {
            _uniformGrid = (UniformGrid) FindName("UniformGrid");

            for (var i = 0; i < 50; i++)
            {
                var userControl1 = new UserControl1
                {
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center,
                    DecorationName = "空",
                    DecorationNumber = "0"
                };
                _uniformGrid?.Children.Add(userControl1);
            }

            ReadDecorations();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (MessageBox.Show("是否退出应用程序？", "提示", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                e.Cancel = false;
                Application.Current.Shutdown();
            }
            else
            {
                e.Cancel = true;
            }
        }

        private void Exit_OnClick(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void Teach_OnClick(object sender, RoutedEventArgs e)
        {
            var proc = new Process {StartInfo = {FileName = "https://www.bilibili.com/video/av70022048"}};
            proc.Start();
        }

        private void About_OnClick(object sender, RoutedEventArgs e)
        {
            var proc = new Process {StartInfo = {FileName = "https://github.com/MadYeling/MHWDecorationsModifierTest"}};
            proc.Start();
        }

        private void Refresh_OnClick(object sender, RoutedEventArgs e)
        {
            ReadDecorations();
        }

        private void ReadDecorations()
        {
            var memoryHandler = new MemoryHandler(_archive);
            var list = memoryHandler.GetArchiveDecorations();

            for (var i = 0; i < 50; i++)
            {
                var userControl = (UserControl1) _uniformGrid.Children[i];
                if (userControl == null) continue;
                if (i >= list.Count) break;
                userControl.DecorationName = ((DecorationBean) list[i]).Name;
                userControl.DecorationNumber = ((DecorationBean) list[i]).Number + "";
            }
        }
    }
}