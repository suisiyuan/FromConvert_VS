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


        /// <summary>
        /// 检查excel文件中解析出来的数据是否正确
        /// </summary>
        public Boolean IsDataValid()
        {
            foreach (ExcelData excelData in ExcelDataList)
            {
                if (excelData.Kilometer_mark.Length == 0 || excelData.Side_direction.Length == 0)
                {
                    return false;
                }
            }
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
                if (row.GetCell(1).ToString().Length == 0)
                {
                    continue;
                }

                excelData.Id = row.GetCell(0).ToString();
                excelData.Device_type = row.GetCell(1).ToString();
                excelData.Kilometer_mark = row.GetCell(2).ToString();
                if (row.GetCell(3).ToString().Length != 0)
                {
                    excelData.Side_direction = row.GetCell(3).ToString();
                }
                excelData.Distance_to_rail = Convert.ToDouble(row.GetCell(4).ToString());
                if (row.GetCell(5).ToString().Length != 0)
                {
                    excelData.Coordinate.Longitude = Convert.ToDouble(row.GetCell(5).ToString());
                }
                if (row.GetCell(6).ToString().Length != 0)
                {
                    excelData.Coordinate.Latitude = Convert.ToDouble(row.GetCell(6).ToString());
                }           
                excelData.Comment = row.GetCell(7).ToString();
                ExcelDataList.Add(excelData);
            }
        }

    }
}
