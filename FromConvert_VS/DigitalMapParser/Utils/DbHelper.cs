using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.SqlServer.Server;
using Point = FromConvert_VS.DigitalMapParser.MapData.Point;
using Vector = FromConvert_VS.DigitalMapParser.MapData.Vector;
using FromConvert_VS.DigitalMapParser.MapData;
using System.Data.Common;

namespace FromConvert_VS.DigitalMapParser.Utils
{
    internal class DbHelper
    {
        

        /// <summary>
        /// 包含一个数字地图的所有数据
        /// </summary>
        private PrjItem prjItem;

        /// <summary>
        /// 传入一个PrjItem的构造方法
        /// </summary>
        /// <param name="prjItem"></param>
        public DbHelper(PrjItem prjItem)
        {
            this.prjItem = prjItem;
        }

        /// <summary>
        /// 生成数据库文件
        /// </summary>
        /// <param name="path"></param>
        public void generateDbFile(String path)
        {
            //创建数据库，并打开连接
            SQLiteConnection connection = new SQLiteConnection("Data Source=" + path);
            connection.Open();

            /******************************** 创建table *********************************/
            //按照最新标准建立数据库
            SQLiteCommand cmd = connection.CreateCommand();

           

            //尝试使用事物进行数据库操作
            DbTransaction trans = connection.BeginTransaction();
            try
            {
                //TODO---将TextPoint写进去
                foreach (TextPoint textPoint in prjItem.getTextParser().getTextPointList())
                {
                    cmd.CommandText = "INSERT INTO TextPoint VALUES " +
                                      "("
                                      + "'" + textPoint.getLongitude() + "', "
                                      + "'" + textPoint.getLatitude() + "', "
                                      + "'" + textPoint.getContent() + "'"
                                      + ")";
                    cmd.ExecuteNonQuery();
                }

                //将Vector中的数据写进去
                foreach (Vector vector in prjItem.getVectorParser().getVectorList())
                {
                    //将每个Vector中的Point放进去
                    for (int i = 0; i < vector.getPointList().Count; i++)
                    {
                        Point point = vector.getPointList()[i];
                        cmd.CommandText = "INSERT INTO Vector (name, longitude, latitude, orderInVector) "
                                            + "VALUES "
                                            + "("
                                            + "'" + vector.getContent() + "', "
                                            + "'" + point.getLongitude() + "', "
                                            + "'" + point.getLatitude() + "', "
                                            + "'" + i + "'"
                                            + ")";
                        cmd.ExecuteNonQuery();
                    }
                }
                //提交事务
                trans.Commit();
            }
            catch (Exception e)
            {
                trans.Rollback(); //回滚事务
            }

            //释放资源
            cmd.Dispose();
            connection.Close();



            MessageBox.Show("文件生成成功", "提示");
        }
    }
}