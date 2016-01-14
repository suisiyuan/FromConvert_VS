using System.Windows;

namespace FromConvert_VS.View
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
