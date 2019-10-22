using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using MHWDecorationsModifier.Beans;
using MHWDecorationsModifier.Code;
using NLog;

namespace MHWDecorationsModifier.xaml
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// 要读取的存档号
        /// </summary>
        private readonly int _archive;

        private int _nowPage = 1;

        private int _maxPage = 1;

        public MainWindow()
        {
            InitializeComponent();
            // 添加关闭方法
            Closing += Window_Closing;
            // 刚打开程序弹出提示框选择存档
            _archive = MyMessageBox.Show();
            Init();
        }

        /// <summary>
        /// 初始化方法，在窗口加载时添加50个自定义控件用于显示珠子
        /// </summary>
        private void Init()
        {
            // 向UniformGrid中添加自定义控件
            for (var i = 0; i < 50; i++)
            {
                var userControl1 = new UserControl1
                {
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center,
                    DecorationName = "空",
                    DecorationNumber = "0"
                };
                UniformGrid.Children.Add(userControl1);
            }

            // 扫描内存，获取珠子
            DecorationListDisposer();
        }

        /// <summary>
        /// 窗口关闭方法，添加确认框
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Closing(object sender, CancelEventArgs e)
        {
            if (MessageBox.Show("是否退出应用程序？", "提示", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                e.Cancel = false;
                // 关闭应用
                Application.Current.Shutdown();
            }
            else
            {
                e.Cancel = true;
            }
        }

        /// <summary>
        /// 菜单中的退出按钮
        /// 使用Close()将会正常调用Window_Closing()方法
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Exit_OnClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

        /// <summary>
        /// 菜单中的教程按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Teach_OnClick(object sender, RoutedEventArgs e)
        {
            var proc = new Process {StartInfo = {FileName = "https://www.bilibili.com/video/av70022048"}};
            proc.Start();
        }

        /// <summary>
        /// 菜单中的关于按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void About_OnClick(object sender, RoutedEventArgs e)
        {
            var proc = new Process {StartInfo = {FileName = "https://github.com/MadYeling/MHWDecorationsModifierTest"}};
            proc.Start();
        }

        /// <summary>
        /// 菜单中的刷新按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Refresh_OnClick(object sender, RoutedEventArgs e)
        {
            DecorationListDisposer();
        }

        private void FrontPage_OnClick(object sender, RoutedEventArgs e)
        {
            _nowPage--;
            DecorationListDisposer();
        }

        private void NextPage_OnClick(object sender, RoutedEventArgs e)
        {
            _nowPage++;
            DecorationListDisposer();
        }

        private void DecorationListDisposer()
        {
            var memoryHandler = new MemoryHandler(_archive);
            var allList = memoryHandler.GetArchiveDecorations();
            var list = new ArrayList();
            Logger.Debug("allList.Count:" + allList.Count);
            _maxPage = allList.Count / 50 + 1;
            if (allList.Count < 50)
            {
                RefreshUi(allList);
                return;
            }

            for (var i = (_nowPage - 1) * 50; i < _nowPage * 50; i++)
            {
                list.Add(i >= allList.Count ? new DecorationBean("锁定", 0, 0, 0) : allList[i]);
            }

            RefreshUi(list);
        }

        /// <summary>
        /// 刷新UI
        /// </summary>
        /// <param name="list">用于UI显示的珠子列表</param>
        /// <param name="nowPage">当前页</param>
        /// <param name="maxPage">最大页</param>
        private void RefreshUi(IList list)
        {
            FrontPage.Visibility = _nowPage == 1 ? Visibility.Hidden : Visibility.Visible;
            NextPage.Visibility = _nowPage == _maxPage ? Visibility.Hidden : Visibility.Visible;

            for (var i = 0; i < 50; i++)
            {
                var userControl = (UserControl1) UniformGrid.Children[i];
                if (userControl == null) continue;
                if (i >= list.Count) break;
                userControl.DecorationName = ((DecorationBean) list[i]).Name;
                userControl.DecorationNumber = ((DecorationBean) list[i]).Number + "";
            }
        }
    }
}