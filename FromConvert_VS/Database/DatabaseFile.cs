﻿using System;
using System.IO;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.SqlServer.Server;
using System.Data.Common;
using Point = FromConvert_VS.DigitalMapParser.MapData.Point;
using Vector = FromConvert_VS.DigitalMapParser.MapData.Vector;
using FromConvert_VS.DigitalMapParser;
using FromConvert_VS.DigitalMapParser.MapData;
using FromConvert_VS.KmlParser;
using FromConvert_VS.ExcelParser;
using FromConvert_VS.CadXmlParser;
using FromConvert_VS.Common;
using FromConvert_VS.Output;
using System.Diagnostics;

namespace FromConvert_VS.Database
{
    /// <summary>
    /// 这个类用于提供数据库文件格式以及添加删除数据的方法
    /// </summary>
    class DatabaseFile
    {
        private String prjPath;
        private SQLiteConnection dbFileConnection;
        private SQLiteCommand cmd;
        private DbTransaction trans;


        private SQLiteDataReader reader;
        private List<OutputData> outputDataList = new List<OutputData>();

        internal List<OutputData> OutputDataList
        {
            get
            {
                return outputDataList;
            }

            set
            {
                outputDataList = value;
            }
        }



        //构造函数 按照文件路径建立数据库类
        public DatabaseFile(String prjPath)
        {
            //赋予数据库文件路径
            this.prjPath = prjPath;            
        }

        //按照标准初始化数据库文件格式
        public void InitDbFile()
        {
            //新建数据库连接 初始化数据库命令变量
            dbFileConnection = new SQLiteConnection("Data Source=" + prjPath);
            dbFileConnection.Open();
            cmd = dbFileConnection.CreateCommand();
            //trans = dbFileConnection.BeginTransaction();

            //初始化数据库文件格式
            cmd.CommandText = "CREATE TABLE ProjectInfo(mapInfo INTEGER, prjName TEXT, creationTime TEXT)";
            cmd.ExecuteNonQuery();

            cmd.CommandText = "CREATE TABLE BaseStation(markerId TEXT, distance_to_rail REAL, antenna_direction_1 TEXT, "
                            + "antenna_direction_2 TEXT, antenna_direction_3 TEXT, antenna_direction_4 TEXT, "
                            + "tower_height TEXT, side_direction TEXT, longitude REAL, "
                            + "latitude REAL, device_type TEXT, tower_type TEXT, "
                            + "kilometer_mark TEXT, id TEXT, comment TEXT)";
            cmd.ExecuteNonQuery();

            cmd.CommandText = "CREATE TABLE Line(layer TEXT, longitude_start REAL, latitude_start REAL, longitude_end REAL, latitude_end REAL)";
            cmd.ExecuteNonQuery();

            cmd.CommandText = "CREATE TABLE Circle(layer TEXT, longitude REAL, latitude REAL, radious REAL)";
            cmd.ExecuteNonQuery();

            cmd.CommandText = "CREATE TABLE Poly(layer TEXT, id INTEGER, orderId INTEGER, longitude REAL, latitude REAL, name TEXT)";
            cmd.ExecuteNonQuery();

            cmd.CommandText = "CREATE TABLE Text(layer TEXT, longitude REAL, latitude REAL, content TEXT, type TEXT)";
            cmd.ExecuteNonQuery();

            cmd.CommandText = "CREATE TABLE P2DPoly(layer TEXT, id INTEGER, orderId INTEGER, longitude REAL, latitude REAL)";
            cmd.ExecuteNonQuery();

            cmd.CommandText = "CREATE TABLE KMLText(longitude REAL, latitude REAL, content TEXT)";
            cmd.ExecuteNonQuery();

            cmd.CommandText = "CREATE TABLE KMLPoly(id INTEGER, orderId, longitude REAL, latitude REAL, content TEXT)";
            cmd.ExecuteNonQuery();

            cmd.CommandText = "CREATE TABLE Photo(markerId INTEGER, photoData BLOB, photoName TEXT)";
            cmd.ExecuteNonQuery();

            //释放资源
            cmd.Dispose();
            dbFileConnection.Close();
        }

        //写入工程信息
        public void WriteProjectInfo(Int16 mapInfo, String prjName)
        {
            dbFileConnection.Open();
            cmd = dbFileConnection.CreateCommand();
            trans = dbFileConnection.BeginTransaction();

            try
            {
                cmd.CommandText = "INSERT INTO ProjectInfo VALUES " +
                                  "("
                                  + "'" + mapInfo + "', "
                                  + "'" + prjName + "', "
                                  + "'" + DateTime.Now.ToString("yyyy年MM月dd日") + "'"
                                  + ")";
                cmd.ExecuteNonQuery();
                trans.Commit();
            }
            catch (Exception e)
            {
                trans.Rollback(); 
            }

            cmd.Dispose();
            dbFileConnection.Close();
        }

        //写入数字地图的信息
        public void WriteDigitalMap(PrjItem prjItem)
        {
            Int16 i, j;

            dbFileConnection.Open();
            cmd = dbFileConnection.CreateCommand();
            trans = dbFileConnection.BeginTransaction();

            try
            {
                //将text数据写进去
                foreach (TextPoint textPoint in prjItem.getTextParser().getTextPointList())
                {
                    cmd.CommandText = "INSERT INTO Text VALUES " +
                                      "("
                                      + "'" + "" + "', "
                                      + "'" + textPoint.getLongitude() + "', "
                                      + "'" + textPoint.getLatitude() + "', "
                                      + "'" + textPoint.getContent() + "', "
                                      + "'" + textPoint.getType() + "'"
                                      + ")";
                    cmd.ExecuteNonQuery();
                }

                //将Poly数据写进去
                j = 0;
                foreach (Vector vector in prjItem.getVectorParser().getVectorList())
                {
                    //将每个Vector中的Point放进去
                    for (i = 0; i < vector.getPointList().Count; i++)
                    {
                        Point point = vector.getPointList()[i];

                        cmd.CommandText = "INSERT INTO Poly (layer, id, orderId, longitude, latitude, name) "
                                            + "VALUES "
                                            + "("
                                            + "'" + "" + "', "
                                            + "'" + j + "', "
                                            + "'" + i + "', "
                                            + "'" + point.getLongitude() + "', "
                                            + "'" + point.getLatitude() + "', "
                                            + "'" + vector.getContent() + "'"
                                            + ")";
                        cmd.ExecuteNonQuery();
                    }
                    j++;
                }
                //提交事务
                trans.Commit();
            }
            catch (Exception e)
            {
                trans.Rollback(); //回滚事务
            }

            cmd.Dispose();
            dbFileConnection.Close();
        }

        //写入CAD地图信息
        public void WriteCadMap(CadXmlFile cadXmlFile)
        {
            dbFileConnection.Open();
            cmd = dbFileConnection.CreateCommand();
            trans = dbFileConnection.BeginTransaction();

            cadXmlFile.CadXmlRead();

            try
            {
                //写入Line表
                foreach (LineData lineData in cadXmlFile.LineDataList)
                {
                    cmd.CommandText = "INSERT INTO Line VALUES " +
                                  "("
                                  + "'" + lineData.Layer + "', "
                                  + "'" + lineData.Coordinate_start.Longitude + "', "
                                  + "'" + lineData.Coordinate_start.Latitude + "', "
                                  + "'" + lineData.Coordinate_end.Longitude + "', "
                                  + "'" + lineData.Coordinate_end.Latitude + "'"
                                  + ")";
                    cmd.ExecuteNonQuery();


                }

                //写入Circle表
                foreach (CircleData circleData in cadXmlFile.CircleDataList)
                {
                    cmd.CommandText = "INSERT INTO Circle VALUES " +
                                  "("
                                  + "'" + circleData.Layer + "', "
                                  + "'" + circleData.Coordinate.Longitude + "', "
                                  + "'" + circleData.Coordinate.Latitude + "', "
                                  + "'" + circleData.Radious + "'"
                                  + ")";
                    cmd.ExecuteNonQuery();
                }

                //写入Poly表
                foreach (CadXmlParser.PolyData polyData in cadXmlFile.PolyDataList)
                {
                    cmd.CommandText = "INSERT INTO Poly VALUES " +
                                  "("
                                  + "'" + polyData.Layer + "', "
                                  + "'" + polyData.Id + "', "
                                  + "'" + polyData.OrderId + "', "
                                  + "'" + polyData.Coordinate.Longitude + "', "
                                  + "'" + polyData.Coordinate.Latitude + "', "
                                  + "'" + polyData.Name + "'"
                                  + ")";
                    cmd.ExecuteNonQuery();
                }

                //写入Text表
                foreach (TextData textData in cadXmlFile.TextDataList)
                {
                    cmd.CommandText = "INSERT INTO Text VALUES " +
                                  "("
                                  + "'" + textData.Layer + "', "
                                  + "'" + textData.Coordinate.Longitude + "', "
                                  + "'" + textData.Coordinate.Latitude + "', "
                                  + "'" + textData.Content + "', "
                                  + "'" + textData.Type + "'"
                                  + ")";
                    cmd.ExecuteNonQuery();

                }

                //写入P2DPoly表
                foreach (P2DPolyData p2DPolyData in cadXmlFile.P2DPolyDataList)
                {
                    cmd.CommandText = "INSERT INTO P2DPoly VALUES " +
                                  "("
                                  + "'" + p2DPolyData.Layer + "', "
                                  + "'" + p2DPolyData.Id + "', "
                                  + "'" + p2DPolyData.OrderId + "', "
                                  + "'" + p2DPolyData.Coordinate.Longitude + "', "
                                  + "'" + p2DPolyData.Coordinate.Latitude + "'"
                                  + ")";
                    cmd.ExecuteNonQuery();
                }

                trans.Commit();
            }
            catch (Exception e)
            {
                trans.Rollback();
            }

            cmd.Dispose();
            dbFileConnection.Close();
        }

        //写入kml文件信息
        public void WriteKml(KmlFile kmlFile)
        {
            Int16 i, j;
            dbFileConnection.Open();
            cmd = dbFileConnection.CreateCommand();
            trans = dbFileConnection.BeginTransaction();

            kmlFile.KmlRead();

            try
            {
                //写入KMLText表
                foreach (DotData dotData in kmlFile.DotDataList)
                {
                    cmd.CommandText = "INSERT INTO KMLText VALUES " +
                                  "("
                                  + "'" + dotData.Coordinate.Longitude + "', "
                                  + "'" + dotData.Coordinate.Latitude + "', "
                                  + "'" + dotData.Content + "'"
                                  + ")";
                    cmd.ExecuteNonQuery();                  
                }

                //写入KMLPoly表
                i = 0;
                foreach (KmlParser.PolyData polyData in kmlFile.PolyDataList)
                {
                    j = 0;
                    foreach (Coordinate coordinate in polyData.CoordinateList)
                    {
                        cmd.CommandText = "INSERT INTO KMLPoly VALUES " +
                                  "("
                                  + "'" + i + "', "
                                  + "'" + j + "', "
                                  + "'" + coordinate.Longitude + "', "
                                  + "'" + coordinate.Latitude + "', "
                                  + "'" + polyData.Content + "'"
                                  + ")";
                        cmd.ExecuteNonQuery();
                        j++;
                    }
                    i++;
                }

                trans.Commit();
            }
            catch (Exception e)
            {
                trans.Rollback();
            }

            cmd.Dispose();
            dbFileConnection.Close();
        }
        
        //写入excel文件信息
        public void WriteExcel(ExcelFile excelFile)
        {
            dbFileConnection.Open();
            cmd = dbFileConnection.CreateCommand();
            trans = dbFileConnection.BeginTransaction();

            excelFile.ExcelRead();

            try
            {
                //写入BaseStation表
                foreach (ExcelData excelData in excelFile.ExcelDataList)
                {
                    cmd.CommandText = "INSERT INTO BaseStation VALUES " +
                                  "("
                                  + "'" + "" + "', "
                                  + "'" + excelData.Distance_to_rail + "', "
                                  + "'" + "" + "', "
                                  + "'" + "" + "', "
                                  + "'" + "" + "', "
                                  + "'" + "" + "', "
                                  + "'" + "" + "', "
                                  + "'" + excelData.Side_direction + "', "
                                  + "'" + excelData.Coordinate.Longitude + "', "
                                  + "'" + excelData.Coordinate.Latitude + "', "
                                  + "'" + excelData.Device_type + "', "
                                  + "'" + "" + "', "
                                  + "'" + excelData.Kilometer_mark + "', "
                                  + "'" + excelData.Id + "', "
                                  + "'" + excelData.Comment + "'"
                                  + ")";
                    cmd.ExecuteNonQuery();
                }

                trans.Commit();
            }
            catch (Exception e)
            {
                trans.Rollback();
            }

            cmd.Dispose();
            dbFileConnection.Close();
        }





        //加载数据文件时读取数据库文件 获取导出word和excel所需信息
        public void ReadDbFile()
        {
            dbFileConnection = new SQLiteConnection("Data Source=" + prjPath);
            dbFileConnection.Open();
            cmd = dbFileConnection.CreateCommand();


            //将基站照片存到系统临时文件夹
            String Path = System.IO.Path.GetTempPath() + "\\基站照片";
            if (!Directory.Exists(Path))
            {
                Directory.CreateDirectory(Path);
            }

            //获取工程名
            cmd.CommandText = "SELECT * FROM ProjectInfo";
            reader = cmd.ExecuteReader();
            String prjName = reader["prjName"] + "";
            reader.Close();
            if (!Directory.Exists(Path + "\\" + prjName))
            {
                Directory.CreateDirectory(Path + "\\" + prjName);
            }


            //获取基站信息
            cmd.CommandText = "SELECT * FROM BaseStation";
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                OutputData outputData = new OutputData();
                outputData.PrjName = prjName;
                outputData.MarkerId = reader["markerId"] + "";
                outputData.Longitude = reader["longitude"] + "";
                outputData.Latitude = reader["latitude"] + "";
                outputData.DeviceType = reader["device_type"] + "";
                outputData.KilometerMark = reader["kilometer_mark"] + "";
                outputData.SideDirection = reader["side_direction"] + "";
                outputData.DistanceToRail = reader["distance_to_rail"] + "";
                outputData.Comment = reader["comment"] + "";
                outputData.TowerType = reader["tower_type"] + "";
                outputData.TowerHeight = reader["tower_height"] + "";
                outputData.AntennaDirection1 = reader["antenna_direction_1"] + "";
                outputData.AntennaDirection2 = reader["antenna_direction_2"] + "";
                outputData.AntennaDirection3 = reader["antenna_direction_3"] + "";
                outputData.AntennaDirection4 = reader["antenna_direction_4"] + "";
                if (!Directory.Exists(Path + "\\" + prjName + "\\" + reader["markerId"]))
                {
                    Directory.CreateDirectory(Path + "\\" + prjName + "\\" + reader["markerId"]);
                }
                outputData.PhotoPathName = Path + "\\" + prjName + "\\" + reader["markerId"];
                OutputDataList.Add(outputData);
            }
            reader.Close();


            //获取图片数据并存储
            cmd.CommandText = "SELECT * FROM Photo";
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                //读取到数据就存储成照片
                MemoryStream streamImage = new MemoryStream(reader["photoData"] as byte[]);
                FileStream file = new FileStream(Path + "\\" + prjName + "\\" + reader["markerId"] + "\\" + reader["photoName"], FileMode.OpenOrCreate, FileAccess.Write);
                BinaryWriter w = new BinaryWriter(file);
                w.Write(streamImage.ToArray());
                file.Close();
                streamImage.Close();
            }
            reader.Close();
        }

    }
}
