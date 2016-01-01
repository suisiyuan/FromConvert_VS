﻿using System;
using System.Windows;
using System.Windows.Forms;
using System.ComponentModel;
using System.Diagnostics;

namespace FromConvert_VS.Main
{
    /// <summary>
    /// NewProjectWindow.xaml 的交互逻辑
    /// </summary>
    public partial class NewProjectWindow : Window
    {
        
        //构造函数
        public NewProjectWindow()
        {
            InitializeComponent();
        }

        //是否已经保存
        Boolean saved = false;

        //存储路径
        String projectPath = "", mapPath = "", excelPath = "", kmlPath = "";




        /****************************** 事件响应 *******************************/

        //获取xml文件或者数字地图文件路径
        private void MapPath_button_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.InitialDirectory = "d:\\";
            dialog.RestoreDirectory = true;
            if (MapPath_comboBox.SelectedIndex == 0)
            {
                dialog.Filter = "xml文件 (*.xml) | *.xml";
            }else
            {
                dialog.Filter = "rar文件 (*.rar) | *.rar";
            }
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                MapPath_textBox.Text = dialog.FileName;
                mapPath = dialog.FileName;
            }
        }


        //获取excel文件路径
        private void ExcelPath_button_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.InitialDirectory = "d:\\";
            dialog.RestoreDirectory = true;
            dialog.Filter = "excel文件 | *.xls";
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                ExcelPath_textBox.Text = dialog.FileName;
                excelPath = dialog.FileName;
            }
        }


        //获取kml文件路径
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
            }
        }


        //点击保存工程按钮
        private void save_button_Click(object sender, RoutedEventArgs e)
        {
            if (ProjectName_textbox.Text.Length == 0 && MapPath_textBox.Text.Length != 0)
            {
                System.Windows.Forms.MessageBox.Show("缺少工程文件名", "信息不全", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else if (ProjectName_textbox.Text.Length != 0 && MapPath_textBox.Text.Length == 0)
            {
                System.Windows.Forms.MessageBox.Show("缺少铁路信息文件", "信息不全", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else if (ProjectName_textbox.Text.Length == 0 && MapPath_textBox.Text.Length == 0)
            {
                System.Windows.Forms.MessageBox.Show("缺少工程文件名和铁路信息文件", "信息不全", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                //检查工程文件路径是否合理

                saved = true;
                this.Close();
            }
        }



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


        /****************************** 处理函数 *******************************/
        Int16 CheckValidation()
        {
            Int16 a = 0;
            
            
            return a;
        }
    }
}