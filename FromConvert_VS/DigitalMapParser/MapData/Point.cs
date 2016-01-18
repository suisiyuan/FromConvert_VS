using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FromConvert_VS.DigitalMapParser.Utils;

namespace FromConvert_VS.DigitalMapParser.MapData
{
    class Point
    {
        //经纬度
        private double latitude;
        private double longitude;

        //构造方法
        public Point(double longitude, double latitude)
        {
            double[] BL = CoordinateConverter.UTMWGSXYtoBL(longitude, latitude);
            this.latitude = BL[0];
            this.longitude = BL[1];
        }


        /************************************* 设置、获取数据 ****************************************/
        public double getLongitude()
        {
            return longitude;
        }

        public double getLatitude()
        {
            return latitude;
        }

        public void setLongitude(double longitude)
        {
            this.longitude = longitude;
        }

        public void setLatitude(double latitude)
        {
            this.latitude = latitude;
        }
    }
}
