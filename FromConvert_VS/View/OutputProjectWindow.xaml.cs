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
using System.Windows.Shapes;
using System.Windows.Forms;
using System.Diagnostics;
using FromConvert_VS.Model.Database;



namespace FromConvert_VS.View
{
    /// <summary>
    /// OutputProjectWindow.xaml 的交互逻辑
    /// </summary>
    public partial class OutputProjectWindow : Window
    {
        //数据库数据
        private DbData daData;


        //文件路径字符串
        String projectPath = "", wordPath = "", excelPath = "";
        String projectName = "", projectFolder = "";


        public OutputProjectWindow()
        {
            InitializeComponent();
        }


        //获取工程文件路径
        private void ProjectPath_button_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.InitialDirectory = "d:\\";
            dialog.RestoreDirectory = true;
            dialog.Filter = "db文件 (*.db) | *.db";
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string delimStr = "\\";
                char[] delimiter = delimStr.ToCharArray();
                string[] sArray;

                ProjectPath_textBox.Text = dialog.FileName;
                projectPath = dialog.FileName;
                projectName = projectPath.Substring(0, projectPath.Length - 3);
                sArray = projectName.Split(delimiter);
                projectName = sArray[sArray.Length - 1];
                projectFolder = sArray[0];
                for (int i = 1; i < sArray.Length - 1; i++)
                {
                    projectFolder = projectFolder + "\\" + sArray[i];
                }
            }

        }

        //word文件输出路径
        private void WordPath_button_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Filter = "word文件 (*.docx)|*.docx";
            dialog.FilterIndex = 1;
            dialog.InitialDirectory = projectFolder;
            dialog.RestoreDirectory = true;
            dialog.FileName = projectName + "_word";
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                WordPath_textBox.Text = dialog.FileName;
                wordPath = dialog.FileName;
            }
        }

        //excel文件输出路径
        private void ExcelPath_button_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog dialog = new SaveFileDialog(); ;
            dialog.Filter = "excel文件 (*.xlsx)|*.xlsx";
            dialog.FilterIndex = 1;
            dialog.InitialDirectory = projectFolder;
            dialog.RestoreDirectory = true;
            dialog.FileName = projectName + "_excel";
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                ExcelPath_textBox.Text = dialog.FileName;
                excelPath = dialog.FileName;
            }
        }


        //输出word、excel文件
        private void output_button_Click(object sender, RoutedEventArgs e)
        {
            
        }

        //退出界面
        private void exit_button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }


    }
}
