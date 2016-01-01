using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Diagnostics;

namespace FromConvert_VS.Main
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            //注册事件
            InitEvents();
        } 
            
        private void InitEvents()
        {
            //进入新建工程界面窗口
            this.new_project.Click += delegate(object sender, RoutedEventArgs args)
            {
                this.Hide();
                NewProjectWindow new_project = new NewProjectWindow();
                new_project.ShowDialog();
                this.ShowDialog();

            };

            //进入工程导出文件窗口
            this.output_project.Click += delegate(object sender, RoutedEventArgs args)
            {
                this.Hide();
                OutputProjectWindow output_project = new OutputProjectWindow();
                output_project.ShowDialog();             
                this.ShowDialog();
            };

            //点击退出按钮，退出应用
            this.exit_button.Click += delegate(object sender, RoutedEventArgs args)
            {
                this.Close();
            };


        }

    }
}
