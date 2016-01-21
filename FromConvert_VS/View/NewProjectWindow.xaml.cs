using System;
using System.Threading;
using System.Windows;
using System.Windows.Forms;
using System.ComponentModel;
using System.Diagnostics;
using FromConvert_VS.DigitalMapParser;
using FromConvert_VS.Database;
using FromConvert_VS.KmlParser;
using FromConvert_VS.ExcelParser;
using FromConvert_VS.CadXmlParser;


namespace FromConvert_VS.View
{
    /// <summary>
    /// NewProjectWindow.xaml 的交互逻辑
    /// </summary>
    public partial class NewProjectWindow : Window
    {
        
        Boolean saved = false;
        String projectName = "", mapPath = "", excelPath = "", kmlPath = "", outputPath = "";

        //数据管理类
        private PrjItem prjItem;

        //xml文件操作类
        private KmlFile kmlFile;

        //excel文件操作类
        private ExcelFile excelFile;

        //CAD xml 文件操作类
        private CadXmlFile cadXmlFile;


        //数据库文件类
        private DatabaseFile databaseFile;


        //构造方法
        public NewProjectWindow()
        {
            InitializeComponent();
        }

        /*************************************** 事件响应函数 ***************************************/
        //实时获取工程文件名
        private void ProjectName_textbox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            projectName = ProjectName_textbox.Text;
        }


        //获取xml文件或者数字地图文件路径
        private void MapPath_button_Click(object sender, RoutedEventArgs e)
        {

            //CAD文件转换所得xml文件
            if (MapPath_comboBox.SelectedIndex == 0)
            {
                OpenFileDialog dialog = new OpenFileDialog();
                dialog.InitialDirectory = "d:\\";
                dialog.RestoreDirectory = true;
                dialog.Filter = "xml文件 (*.xml) | *.xml";
                if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    MapPath_textBox.Text = dialog.FileName;
                    mapPath = dialog.FileName;
                    cadXmlFile = new CadXmlFile(mapPath);
                }
            }
            //数字地图
            else if (MapPath_comboBox.SelectedIndex == 1)
            {
                FolderBrowserDialog dialog = new FolderBrowserDialog();
                if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    string path = dialog.SelectedPath;
                    if (!PrjItem.isDirectoryValid(path))
                    {
                        System.Windows.MessageBox.Show("当前路径不符合数字地图文件要求", "错误");
                        return;
                    }
                    prjItem = new PrjItem(path);
                    //获取选中的文件夹
                    this.MapPath_textBox.Text = dialog.SelectedPath;
                    mapPath = dialog.SelectedPath;
                }
            }
        }



        //获取excel文件路径 xlsx文件
        private void ExcelPath_button_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.InitialDirectory = "d:\\";
            dialog.RestoreDirectory = true;
            dialog.Filter = "excel文件 | *.xlsx";
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                ExcelPath_textBox.Text = dialog.FileName;
                excelPath = dialog.FileName;
                excelFile = new ExcelFile(excelPath);
            }
        }


        //获取kml文件路径 kml文件
        private void KmlPath_button_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.InitialDirectory = "d:\\";
            dialog.RestoreDirectory = true;
            dialog.Filter = "kml文件 | *.kml";
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                KmlPath_textBox.Text = dialog.FileName;
                kmlPath = dialog.FileName;
                kmlFile = new KmlFile(kmlPath);
            }

        }


        //点击保存工程按钮
        private void save_button_Click(object sender, RoutedEventArgs e)
        {
            if (projectName.Length == 0 && mapPath.Length != 0)
            {
                System.Windows.Forms.MessageBox.Show("缺少工程文件名", "信息不全", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else if (projectName.Length != 0 && mapPath.Length == 0)
            {
                System.Windows.Forms.MessageBox.Show("缺少铁路信息源", "信息不全", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else if (projectName.Length == 0 && mapPath.Length == 0)
            {
                System.Windows.Forms.MessageBox.Show("缺少工程文件名和铁路信息文件", "信息不全", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                SaveFileDialog dialog = new SaveFileDialog(); ;
                dialog.Filter = "db文件 (*.db)|*.db";
                dialog.FilterIndex = 1;
                dialog.InitialDirectory = "d:\\";
                dialog.RestoreDirectory = true;
                dialog.FileName = projectName;
                if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    outputPath = dialog.FileName;
                }

                databaseFile = new DatabaseFile(outputPath);
                SaveProject();


                saved = true;
                this.Close();
            }
        }


        //根据提供的信息向数据库写数据
        private void SaveProject()
        {
            //CAD文件
            if (MapPath_comboBox.SelectedIndex == 0)
            {
                databaseFile.WriteProjectInfo(0, projectName, "");
                databaseFile.WriteCadMap(cadXmlFile);
            }
            //数字地图
            else if (MapPath_comboBox.SelectedIndex == 1)
            {                
                databaseFile.WriteProjectInfo(1, projectName, "");
                databaseFile.WriteDigitalMap(prjItem);
            }

            //如果有kml文件
            if (kmlFile != null)
            {
                databaseFile.WriteKml(kmlFile);
            }

            //如果有excel文件
            if (excelFile != null)
            {
                databaseFile.WriteExcel(excelFile);
            }

            System.Windows.MessageBox.Show("文件生成成功", "提示");
        }




        /*************************************** 窗口关闭相关函数 ***************************************/
        //点击返回主菜单按钮 直接close窗口
        private void exit_button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }


        //窗口关闭的时候弹出弹窗
        private void Window_Closing(object sender, CancelEventArgs e)
        {
            const string message = "文件尚未保存，确认退出？";
            const string caption = "窗口退出";

            if (saved == true || (ProjectName_textbox.Text.Length == 0 && MapPath_textBox.Text.Length == 0)) ;
            else
            {
                System.Windows.Forms.DialogResult result = System.Windows.Forms.MessageBox.Show(message, caption, MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
                if (result == System.Windows.Forms.DialogResult.Cancel)
                {
                    //取消关闭窗口
                    e.Cancel = true;
                }
            }
        }


    }
}
