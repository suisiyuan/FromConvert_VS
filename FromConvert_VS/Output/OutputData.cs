using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FromConvert_VS.Output
{
    class OutputData
    {
        private String prjName;             //工程名
        private String longitude;           //经度
        private String latitude;            //纬度
        private String deviceType;          //设备类型
        private String kilometerMark;       //公里标
        private String sideDirection;       //侧向
        private String distanceToRail;      //距线路中心距离
        private String comment;             //备注
        private String towerType;           //杆塔类型
        private String towerHeight;         //杆塔高度
        private String antennaDirection1;   //天线方向1
        private String antennaDirection2;   //天线方向2
        private String antennaDirection3;   //天线方向3
        private String antennaDirection4;   //天线方向4
        private String photoPathName;       //照片路径

        public string PrjName
        {
            get
            {
                return prjName;
            }

            set
            {
                prjName = value;
            }
        }

        public string Longitude
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

        public string Latitude
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

        public string DeviceType
        {
            get
            {
                return deviceType;
            }

            set
            {
                deviceType = value;
            }
        }

        public string KilometerMark
        {
            get
            {
                return kilometerMark;
            }

            set
            {
                kilometerMark = value;
            }
        }

        public string SideDirection
        {
            get
            {
                return sideDirection;
            }

            set
            {
                sideDirection = value;
            }
        }

        public string DistanceToRail
        {
            get
            {
                return distanceToRail;
            }

            set
            {
                distanceToRail = value;
            }
        }

        public string PhotoPathName
        {
            get
            {
                return photoPathName;
            }

            set
            {
                photoPathName = value;
            }
        }

        public string Comment
        {
            get
            {
                return comment;
            }

            set
            {
                comment = value;
            }
        }

        public string TowerType
        {
            get
            {
                return towerType;
            }

            set
            {
                towerType = value;
            }
        }

        public string TowerHeight
        {
            get
            {
                return towerHeight;
            }

            set
            {
                towerHeight = value;
            }
        }

        public string AntennaDirection1
        {
            get
            {
                return antennaDirection1;
            }

            set
            {
                antennaDirection1 = value;
            }
        }

        public string AntennaDirection2
        {
            get
            {
                return antennaDirection2;
            }

            set
            {
                antennaDirection2 = value;
            }
        }

        public string AntennaDirection3
        {
            get
            {
                return antennaDirection3;
            }

            set
            {
                antennaDirection3 = value;
            }
        }

        public string AntennaDirection4
        {
            get
            {
                return antennaDirection4;
            }

            set
            {
                antennaDirection4 = value;
            }
        }
    }
}
