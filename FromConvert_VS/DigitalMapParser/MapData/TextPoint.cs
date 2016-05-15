using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FromConvert_VS.DigitalMapParser.Utils;

namespace FromConvert_VS.DigitalMapParser.MapData
{
    class TextPoint
    {

        /**
         * 大地坐标
         */
        private double longitude;
        private double latitude;

        /**
         * 显示的数据
         */
        private string content;
        private string type;

        /**
         * 构造方法
         *
         * @param longitude 大地经度
         * @param latitude  大地纬度
         * @param content   文字内容
         */
        public TextPoint(double longitude, double latitude, string content, string type)
        {
            //进行坐标转换
            double[] BL = CoordinateConverter.UTMWGSXYtoBL(longitude, latitude);
            this.latitude = BL[0];
            this.longitude = BL[1];
            this.content = content;
            this.type = type;
        }

        // 返回描述的文字
        public override string ToString()
        {
            return "我的数据是:\t" + latitude + "\t" + longitude + "\t" + content;
        }



        /************************************* 设置、获取数据 ****************************************/
        public void setLongitude(double longitude)
        {
            this.longitude = longitude;
        }

        public void setLatitude(double latitude)
        {
            this.latitude = latitude;
        }

        public void setContent(string content)
        {
            this.content = content;
        }

        public double getLongitude()
        {
            return longitude;
        }

        public double getLatitude()
        {
            return latitude;
        }

        public string getContent()
        {
            return content;
        }

        public string getType()
        {
            return type;
        }
    }
}
