using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

namespace FromConvert_VS.ExcelParser
{
    class ExcelFile
    {
        //Excel文件路径
        private String excelPath;

        /// <summary>
        /// 保存从excel文件中解析出的数据
        /// </summary>
        private List<ExcelData> excelDataList = new List<ExcelData>();

        internal List<ExcelData> ExcelDataList
        {
            get
            {
                return excelDataList;
            }

            set
            {
                excelDataList = value;
            }
        }


        //构造函数 获取excel文件路径
        public ExcelFile(String excelPath)
        {
            this.excelPath = excelPath;
        }
        
        //检查excel文件的有效性
        public Boolean CheckValidation()
        {
            XSSFWorkbook wk;
            using (FileStream fs = File.Open(excelPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                wk = new XSSFWorkbook(fs);
                fs.Close();
            }
            ISheet sheet = wk.GetSheetAt(0);
            IRow row = sheet.GetRow(0);
            if (!row.GetCell(0).ToString().Contains("序号"))
            {
                System.Windows.MessageBox.Show("Excel文件格式错误，第一列应为序号", "错误");
                return false;
            }
            else if (!row.GetCell(1).ToString().Contains("设备类型"))
            {
                System.Windows.MessageBox.Show("Excel文件格式错误，第二列应为设备类型", "错误");
                return false;
            }
            else if (!row.GetCell(2).ToString().Contains("公里标"))
            {
                System.Windows.MessageBox.Show("Excel文件格式错误，第三列应为公里标", "错误");
                return false;
            }
            else if (!row.GetCell(3).ToString().Contains("侧向"))
            {
                System.Windows.MessageBox.Show("Excel文件格式错误，第四列应为侧向", "错误");
                return false;
            }
            else if (!row.GetCell(4).ToString().Contains("距线路中心距离"))
            {
                System.Windows.MessageBox.Show("Excel文件格式错误，第五列应为距线路中心距离", "错误");
                return false;
            }
            else if (!row.GetCell(5).ToString().Contains("经度"))
            {
                System.Windows.MessageBox.Show("Excel文件格式错误，第六列应为经度", "错误");
                return false;
            }
            else if (!row.GetCell(6).ToString().Contains("纬度"))
            {
                System.Windows.MessageBox.Show("Excel文件格式错误，第七列应为纬度", "错误");
                return false;
            }
            else if (!row.GetCell(7).ToString().Contains("备注文本"))
            {
                System.Windows.MessageBox.Show("Excel文件格式错误，第八列应为备注文本", "错误");
                return false;
            }
            return true;
        }

        //检查有无数据
        public Boolean checkData(IRow row)
        {
            if (row.GetCell(1).ToString().Length == 0)
                return false;
            else if ((row.GetCell(2).ToString().Length == 0 || row.GetCell(3).ToString().Length == 0 || row.GetCell(4).ToString().Length == 0)
                    && (row.GetCell(5).ToString().Length == 0 || row.GetCell(6).ToString().Length == 0))
                return false;
            return true;
        }


        //读取excel文件内容
        public void ExcelRead()
        {
            XSSFWorkbook wk;
            using (FileStream fs = File.Open(excelPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                wk = new XSSFWorkbook(fs);
                fs.Close();
            }

            //获取第一个表格
            ISheet sheet = wk.GetSheetAt(0);
            for (int i = 1; i <= sheet.LastRowNum; i++)
            {
                IRow row = sheet.GetRow(i);
                ExcelData excelData = new ExcelData();
                if (!checkData(row))
                    continue;

                if (row.GetCell(0).ToString().Length != 0)
                {
                    excelData.Id = row.GetCell(0).ToString();
                }                
                excelData.Device_type = row.GetCell(1).ToString();
                if (row.GetCell(2).ToString().Length != 0)
                {
                    excelData.Kilometer_mark = row.GetCell(2).ToString();
                    excelData.Side_direction = row.GetCell(3).ToString();
                    excelData.Distance_to_rail = Convert.ToDouble(row.GetCell(4).ToString());
                }                            
                if (row.GetCell(5).ToString().Length != 0)
                {
                    excelData.Coordinate.Longitude = Convert.ToDouble(row.GetCell(5).ToString());
                    excelData.Coordinate.Latitude = Convert.ToDouble(row.GetCell(6).ToString());
                }
                if (row.GetCell(7).ToString().Length != 0)
                {
                    excelData.Comment = row.GetCell(7).ToString();
                }                    
                ExcelDataList.Add(excelData);
            }
        }

    }
}
