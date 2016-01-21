using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FromConvert_VS.Common
{
    class Coordinate
    {
        public Coordinate()
        {

        }

        private Double longitude;
        private Double latitude;

        public Double Longitude
        {
            get
            {
                return longitude;
            }

            set
            {
                longitude = value;
            }
        }

        public Double Latitude
        {
            get
            {
                return latitude;
            }

            set
            {
                latitude = value;
            }
        }

        //解析kml文件中的坐标字符串
        public Coordinate KmlConvert(String kmlCoordinateStr)
        {
            Coordinate coordinate = new Coordinate();
            coordinate.Longitude = Convert.ToDouble(kmlCoordinateStr.Split(new char[] { ',' })[0]);
            coordinate.Latitude = Convert.ToDouble(kmlCoordinateStr.Split(new char[] { ',' })[1]);
            return coordinate;
        }


    }
}
