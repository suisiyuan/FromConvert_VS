﻿using System;
using System.Threading;
using System.Windows;
using System.Windows.Forms;
using System.ComponentModel;
using System.Diagnostics;
using DigitalMapToDB.DigitalMapParser;
using DigitalMapToDB.DigitalMapParser.Utils;

namespace FromConvert_VS.Main
{
    /// <summary>
    /// NewProjectWindow.xaml 的交互逻辑
    /// </summary>
    public partial class NewProjectWindow : Window
    {
        
        //是否已经保存
        Boolean saved = false;

        //路径字符串
        String projectName = "", mapPath = "", excelPath = "", kmlPath = "", outputPath = "";

        //数据管理类
        private PrjItem prjItem;
        private DbHelper dbHelper;


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

                //CAD文件
                if (MapPath_comboBox.SelectedIndex == 0)
                {

                }
                //数字地图
                else if (MapPath_comboBox.SelectedIndex == 1)
                {
                    if (prjItem == null)
                    {
                        prjItem = new PrjItem(mapPath);
                    }
                    if (dbHelper == null)
                    {
                        dbHelper = new DbHelper(prjItem);
                    }
                    dbHelper.generateDbFile(outputPath);
                }

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


        /*************************************** 数据处理函数 ***************************************/


    }
}
