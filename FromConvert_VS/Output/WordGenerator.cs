﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using NPOI.OpenXmlFormats.Dml.Picture;
using NPOI.XWPF.UserModel;
using NPOI.OpenXmlFormats.Wordprocessing;
using ICSharpCode.SharpZipLib.Zip;
using System.Data.SQLite;
using FromConvert_VS.DigitalMapParser.Utils;
using NPOI.OpenXmlFormats.Dml.WordProcessing;
using System.Drawing;

namespace FromConvert_VS.Output
{

    internal class WordGenerator
    {

        private WordGenerator()
        {
        }

        public static void word_creat_one(List<OutputData> datalist, string outputPath)
        {
            XWPFDocument m_Docx = new XWPFDocument();
            String prjName = datalist[0].PrjName;
            word_init(m_Docx, prjName);


            for (int i = 0; i < datalist.Count; i++){
                word_inster_table(m_Docx, datalist[i], i + 1, datalist[i].PhotoPathName);
            }

            FileStream sw = File.Create(outputPath);
            m_Docx.Write(sw);
            sw.Close();
        }


        private static void word_init(XWPFDocument m_Docx, String prjName)
        {
            //设置页面 将页面设置为A4 纵向
            //参考网站  http://www.aiuxian.com/article/p-1970779.html
            CT_SectPr m_SectPr = new CT_SectPr();
            m_SectPr.pgSz.w = (ulong) 11906;
            m_SectPr.pgSz.h = (ulong) 16838;
            m_Docx.Document.body.sectPr = m_SectPr;

            //第一页 封面
            word_insert_space(5, m_Docx);
            word_insert_text(m_Docx, "宋体", 22, prjName);
            word_insert_text(m_Docx, "宋体", 22, "GSM-R 通信系统");
            word_insert_text(m_Docx, "宋体", 22, "现场勘查报告");
            word_insert_space(7, m_Docx);
            word_insert_text(m_Docx, "宋体", 22, DateTime.Now.ToString("yyyy年MM月dd日"));
            word_insert_space(7, m_Docx);


            //创建表并获取该表
            XWPFTable table = m_Docx.CreateTable(4, 2);
            CT_Tbl ctbl = m_Docx.Document.body.GetTblArray()[0];
            //表居中
            ctbl.AddNewTblPr().jc = new CT_Jc();
            ctbl.AddNewTblPr().jc.val = ST_Jc.center;
            //表宽度为8000
            ctbl.AddNewTblPr().AddNewTblW().w = "8000";
            ctbl.AddNewTblPr().AddNewTblW().type = ST_TblWidth.dxa;

            //列宽设置
            CT_TcPr m_Pr = table.GetRow(0).GetCell(1).GetCTTc().AddNewTcPr();
            m_Pr.tcW = new CT_TblWidth();
            m_Pr.tcW.w = "3000";
            m_Pr.tcW.type = ST_TblWidth.dxa;

            //行高设置


            //设置表中文本
            table.GetRow(0).GetCell(0).SetText("项目");
            table.GetRow(1).GetCell(0).SetText("勘察日期");
            table.GetRow(2).GetCell(0).SetText("现场勘查人员");
            table.GetRow(3).GetCell(0).SetText("报告修正人员");

            word_insert_space(4, m_Docx);
        }



        //插入空行
        private static void word_insert_space(int n, XWPFDocument m_Docx, int longth = 250)
        {
            for (int i = 0; i < n; i++)
            {
                XWPFParagraph gp_space = m_Docx.CreateParagraph(); //创建XWPFParagraph
                gp_space.SetAlignment(ParagraphAlignment.CENTER);
                gp_space.SetSpacingBefore(longth); 
                gp_space.SpacingAfter = longth;
            }
        }



        //插入一段文字
        private static void word_insert_text(XWPFDocument m_Docx, string Font, int size, string text, ParagraphAlignment position = ParagraphAlignment.CENTER)
        {
            XWPFParagraph gp = m_Docx.CreateParagraph();
            gp.SetAlignment(position);
            XWPFRun gr = gp.CreateRun();
            gr.SetFontFamily(Font);
            gr.SetFontSize(size);
            gr.SetText(text);
        }



        private static void word_inster_table(XWPFDocument m_Docx, OutputData bean, int i, String photoPathName)
        {
            XWPFTable table = m_Docx.CreateTable(12, 2);
            CT_Tbl ctbl = m_Docx.Document.body.GetTblArray()[i];
            CT_TblPr ctblpr = ctbl.AddNewTblPr();
            ctblpr.jc = new CT_Jc();
            ctblpr.jc.val = ST_Jc.center;

            table.Width = 3500;
            table.GetRow(0).GetCell(0).SetText("设备类型");
            table.GetRow(0).GetCell(1).SetText(bean.DeviceType);
            table.GetRow(1).GetCell(0).SetText("公里标");
            table.GetRow(1).GetCell(1).SetText(bean.KilometerMark);
            table.GetRow(2).GetCell(0).SetText("下行侧向");
            table.GetRow(2).GetCell(1).SetText(bean.SideDirection);
            table.GetRow(3).GetCell(0).SetText("距线路中心距离(m)");
            table.GetRow(4).GetCell(0).SetText("经度");
            table.GetRow(4).GetCell(1).SetText(bean.Longitude);
            table.GetRow(5).GetCell(0).SetText("纬度");
            table.GetRow(5).GetCell(1).SetText(bean.Latitude);
            table.GetRow(6).GetCell(0).SetText("杆塔类型");
            table.GetRow(6).GetCell(1).SetText(bean.TowerType);
            table.GetRow(7).GetCell(0).SetText("杆塔高度");
            table.GetRow(7).GetCell(1).SetText(bean.TowerHeight);
            table.GetRow(8).GetCell(0).SetText("天线1方向角");
            table.GetRow(8).GetCell(1).SetText(bean.AntennaDirection1);
            table.GetRow(9).GetCell(0).SetText("天线2方向角");
            table.GetRow(9).GetCell(1).SetText(bean.AntennaDirection2);
            table.GetRow(10).GetCell(0).SetText("天线3方向角");
            table.GetRow(10).GetCell(1).SetText(bean.AntennaDirection3);
            table.GetRow(11).GetCell(0).SetText("天线4方向角");
            table.GetRow(11).GetCell(1).SetText(bean.AntennaDirection4);
            CT_TcPr m_Pr = table.GetRow(2).GetCell(1).GetCTTc().AddNewTcPr();
            m_Pr.tcW = new CT_TblWidth();
            m_Pr.tcW.w = "3500";
            m_Pr.tcW.type = ST_TblWidth.dxa; //设置单元格宽度

            XWPFTableRow m_Row = table.InsertNewTableRow(0);
            XWPFTableCell cell = m_Row.CreateCell();
            CT_Tc cttc = cell.GetCTTc();
            CT_TcPr ctPr = cttc.AddNewTcPr();
            ctPr.gridSpan = new CT_DecimalNumber();
            ctPr.gridSpan.val = "2";
            cttc.GetPList()[0].AddNewR().AddNewT().Value = "SITE: " + bean.MarkerId;


            System.IO.DirectoryInfo dir = new System.IO.DirectoryInfo(photoPathName);
            System.IO.FileInfo[] files = dir.GetFiles();
            foreach (System.IO.FileInfo file in files)
            {
                FileStream gfs = new FileStream(photoPathName + "\\" + file.Name, FileMode.Open, FileAccess.Read);
                Image image = Image.FromFile(photoPathName + "\\" + file.Name);
                Double ratio = (Double)image.Width / (Double)image.Height;
                image.Dispose();
                XWPFParagraph gp = m_Docx.CreateParagraph();
                gp.SetAlignment(ParagraphAlignment.CENTER);
                XWPFRun gr = gp.CreateRun();

                if (ratio > 1)
                {
                    gr.AddPicture(gfs, (int)PictureType.JPEG, file.Name, 3555556, 2000000);
                }
                else {
                    gr.AddPicture(gfs, (int)PictureType.JPEG, file.Name, 2000000, 3555556);
                }
                gfs.Close();

            }

            word_insert_space(3, m_Docx, 100);
        }

    }
}