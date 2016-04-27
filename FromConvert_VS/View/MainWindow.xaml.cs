using System;
using System.IO;
using System.Windows;
using System.Diagnostics;
using System.Windows.Forms;
using FromConvert_VS.ExcelParser;
using FromConvert_VS.KmlParser;
using FromConvert_VS.DigitalMapParser;
using FromConvert_VS.CadXmlParser;
using FromConvert_VS.Database;
using FromConvert_VS.Output;

namespace FromConvert_VS.View
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        private String projectName = "", mapPath = "", excelPath = "", kmlPath = "", outputPath = "";

        private PrjItem prjItem;
        private KmlFile kmlFile;
        private ExcelFile excelFile;
        private CadXmlFile cadXmlFile;
        private DatabaseFile databaseFile;


        private String projectPath_output = "", wordPath_output = "", excelPath_output = "";
        private String projectName_output = "", projectFolder_output = "";
        private DatabaseFile databaseFile_output;



        public MainWindow()
        {
            InitializeComponent();          
        }


        private void ProjectName_TextChanged(object sender, RoutedEventArgs e)
        {
            projectName = ProjectName_textbox.Text;
        }

        private void MapPath_Click(object sender, RoutedEventArgs e)
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


        private void ExcelPath_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.InitialDirectory = "d:\\";
            dialog.RestoreDirectory = true;
            dialog.Filter = "excel文件 | *.xlsx";
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                ExcelPath_textBox.Clear();
                excelPath = dialog.FileName;
                excelFile = new ExcelFile(excelPath);
                if (excelFile.CheckValidation())
                {
                    ExcelPath_textBox.Text = dialog.FileName;
                }                               
            }
        }

        private void KmlPath_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.InitialDirectory = "d:\\";
            dialog.RestoreDirectory = true;
            dialog.Filter = "kml文件 | *.kml";
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                kmlPath = dialog.FileName;
                kmlFile = new KmlFile(kmlPath);
                KmlPath_textBox.Clear();
                KmlPath_textBox.Text = dialog.FileName;                
            }
        }

        
        private void SaveProject_Click(object sender, RoutedEventArgs e)
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
                databaseFile.InitDbFile();
                SaveProject();
            }
        }

        private void SaveProject()
        {
            //CAD文件
            if (MapPath_comboBox.SelectedIndex == 0)
            {
                databaseFile.WriteProjectInfo(0, projectName);
                databaseFile.WriteCadMap(cadXmlFile);
            }
            //数字地图
            else if (MapPath_comboBox.SelectedIndex == 1)
            {
                databaseFile.WriteProjectInfo(1, projectName);
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




        private void LoadProject_Click(object sender, RoutedEventArgs e)
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
                projectPath_output = dialog.FileName;
                projectName = projectPath_output.Substring(0, projectPath_output.Length - 3);
                sArray = projectName.Split(delimiter);
                projectName = sArray[sArray.Length - 1];
                projectFolder_output = sArray[0];
                for (int i = 1; i < sArray.Length - 1; i++)
                {
                    projectFolder_output = projectFolder_output + "\\" + sArray[i];
                }

                this.ExportWord_button.IsEnabled = true;
                this.ExportExcel_button.IsEnabled = true;
                this.checkBox.IsEnabled = true;
           
                databaseFile = new DatabaseFile(projectPath_output);
                databaseFile.ReadDbFile();
            }
        }

        //导出Word文件
        private void ExportWord_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Filter = "word文件 (*.docx)|*.docx";
            dialog.FilterIndex = 1;
            dialog.InitialDirectory = projectFolder_output;
            dialog.RestoreDirectory = true;
            dialog.FileName = projectName + "_word";
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                WordGenerator.word_creat_one(databaseFile.OutputDataList, dialog.FileName);
                System.Windows.MessageBox.Show("Word文件导出成功", "完成");

                //保存基站照片
                if (checkBox.IsChecked == true)
                {
                    String path = Path.GetDirectoryName(dialog.FileName);
                    String prjName = databaseFile.OutputDataList[0].PrjName;

                    //创建文件夹
                    if (!Directory.Exists(path + "\\" + prjName + "工程基站照片"))
                        Directory.CreateDirectory(path + "\\" + prjName + "工程基站照片");

                    //从temp文件夹中将照片复制出来
                    foreach (OutputData outputData in databaseFile.OutputDataList)
                    {
                        if (!Directory.Exists(path + "\\" + prjName + "工程基站照片" + "\\" + outputData.MarkerId))
                            Directory.CreateDirectory(path + "\\" + prjName + "工程基站照片" + "\\" + outputData.MarkerId);
                        DirectoryInfo dir = new DirectoryInfo(Path.GetTempPath() + "\\" + "基站照片" + "\\" + prjName + "\\" + outputData.MarkerId);
                        FileInfo[] files = dir.GetFiles();
                        foreach (FileInfo file in files)
                        {
                            file.CopyTo(path + "\\" + prjName + "工程基站照片" + "\\" + outputData.MarkerId + "\\" + file.Name, true);
                        }
                    }
                }
            }
        }

        //导出Excel文件
        private void ExportExcel_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog dialog = new SaveFileDialog(); ;
            dialog.Filter = "excel文件 (*.xls)|*.xls";
            dialog.FilterIndex = 1;
            dialog.InitialDirectory = projectFolder_output;
            dialog.RestoreDirectory = true;
            dialog.FileName = projectName + "_excel";
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {            
                new ExcelGenerator(databaseFile.OutputDataList, dialog.FileName).Generate();
                System.Windows.MessageBox.Show("Excel文件导出成功", "完成");
            }
        }


    }
}
