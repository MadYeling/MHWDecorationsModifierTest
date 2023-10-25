using System;
using System.ComponentModel;
using System.Threading;
using System.Windows;
using MHWDecorationsModifier.Beans;
using MHWDecorationsModifier.Code;
using NLog;


namespace MHWDecorationsModifier.xaml
{
    /// <summary>
    /// LoadWindow.xaml 的交互逻辑
    /// </summary>
    public partial class LoadWindow
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly MemoryHandler _memoryHandler;

        private readonly MemoryBean[] _memoryInfoList = new MemoryBean[3];

        private readonly BackgroundWorker _backgroundWorker;

        public LoadWindow()
        {
            InitializeComponent();
            _memoryHandler = new MemoryHandler();
            _backgroundWorker = (BackgroundWorker)FindResource("BackgroundWorker");
            _backgroundWorker.RunWorkerAsync();
        }

        private void BackgroundWorker_OnDoWork(object sender, DoWorkEventArgs e)
        {
            for (var i = 0; i < 3; i++)
            {
                // 跑太快了UI来不及反应，给UI点时间
                Thread.Sleep(200);
                var memoryInfo = _memoryHandler.GetDecorationsAddress(i,
                    (percent) =>
                    {
                        _backgroundWorker.ReportProgress((int)percent);
                    });
                if (memoryInfo == null)
                {
                    MessageBox.Show("不中嘞，特征码得更新了哥", "不中！", MessageBoxButton.OK, MessageBoxImage.Error);
                    Environment.Exit(0);
                }
                _memoryInfoList[i] = memoryInfo;
            }
        }

        private void BackgroundWorker_OnProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            LoadProgressBar.Value = e.ProgressPercentage;
        }

        private void BackgroundWorker_OnRunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            LoadText.Content = "加载完成！";
            Hide();
            new ChooseArchiveWindow(_memoryInfoList).Show();
        }
    }
}