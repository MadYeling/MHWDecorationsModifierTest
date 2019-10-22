using System.Windows;
using System.Windows.Controls;

namespace MHWDecorationsModifier.xaml
{
    /// <summary>
    /// UserControl1.xaml 的交互逻辑
    /// </summary>
    public partial class UserControl1 : UserControl
    {
        //定义依赖项属性
        public static readonly DependencyProperty DecorationNameProperty =
            DependencyProperty.Register("DecorationName", typeof(string), typeof(UserControl1),
                new PropertyMetadata(""));

        public static readonly DependencyProperty DecorationNumberProperty =
            DependencyProperty.Register("DecorationNumber", typeof(string), typeof(UserControl1),
                new PropertyMetadata(""));


        //声明属性        

        public string DecorationName
        {
            get { return (string) GetValue(DecorationNameProperty); }
            set { SetValue(DecorationNameProperty, value); }
        }

        public string DecorationNumber
        {
            get { return (string) GetValue(DecorationNumberProperty); }
            set { SetValue(DecorationNumberProperty, value); }
        }


        public UserControl1()
        {
            InitializeComponent();
        }
    }
}