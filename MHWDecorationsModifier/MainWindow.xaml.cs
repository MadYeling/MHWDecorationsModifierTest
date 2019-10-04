using System;
using System.Diagnostics;
using System.Windows;

namespace MHWDecorationsModifier
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Exit_OnClick(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("是否退出程序？", "警告", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
//                Environment.Exit(0);
                Application.Current.Shutdown();
            }
        }

        private void Teach_OnClick(object sender, RoutedEventArgs e)
        {
            Process proc = new Process();
            proc.StartInfo.FileName = "https://www.bilibili.com/video/av70022048";
            proc.Start();
        }
        
        private void About_OnClick(object sender, RoutedEventArgs e)
        {
            Process proc = new Process();
            proc.StartInfo.FileName = "https://github.com/MadYeling/MHWDecorationsModifierTest";
            proc.Start();
        }
    }
}