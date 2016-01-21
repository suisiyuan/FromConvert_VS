using System;
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

        //构造函数 按照格式建立数据库
        public DatabaseFile(String prjPath)
        {
            //赋予数据库文件路径
            this.prjPath = prjPath;

            //新建数据库连接 初始化数据库命令变量
            dbFileConnection = new SQLiteConnection("Data Source=" + prjPath);
            dbFileConnection.Open();
            cmd = dbFileConnection.CreateCommand();
            //trans = dbFileConnection.BeginTransaction();

            //初始化数据库文件格式
            cmd.CommandText = "CREATE TABLE ProjectInfo(mapInfo INTEGER, prjName TEXT, createTime TEXT)";
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

            cmd.CommandText = "CREATE TABLE Text(layer TEXT, longitude REAL, latitude REAL, content TEXT)";
            cmd.ExecuteNonQuery();

            cmd.CommandText = "CREATE TABLE P2DPoly(layer TEXT, id INTEGER, orderId INTEGER, longitude REAL, latitude REAL)";
            cmd.ExecuteNonQuery();

            cmd.CommandText = "CREATE TABLE KMLText(longitude REAL, latitude REAL, content TEXT)";
            cmd.ExecuteNonQuery();

            cmd.CommandText = "CREATE TABLE KMLPoly(id INTEGER, orderId, longitude REAL, latitude REAL, content TEXT)";
            cmd.ExecuteNonQuery();

            cmd.CommandText = "CREATE TABLE Photo(prjName TEXT, markId INTEGER, photoData BLOB, photoName TEXT)";
            cmd.ExecuteNonQuery();

            //释放资源
            cmd.Dispose();
            dbFileConnection.Close();
        }

        //写入工程信息
        public void WriteProjectInfo(Int16 mapInfo, String prjName, String content)
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
                                  + "'" + content + "'"
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
                                      + "'" + textPoint.getContent() + "'"
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
                                  + "'" + textData.Content + "'"
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

    }
}
