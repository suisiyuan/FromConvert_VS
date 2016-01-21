using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FromConvert_VS.Common;

namespace FromConvert_VS.ExcelParser
{
    //定义数据结构
    class ExcelData
    {
        //用户数据
        private Double distance_to_rail;
        private String side_direction;
        private Coordinate coordinate;
        private String device_type;
        private String kilometer_mark;
        private String id;
        private String comment;

        public ExcelData()
        {
            coordinate = new Coordinate();
        }


        public double Distance_to_rail
        {
            get
            {
                return distance_to_rail;
            }

            set
            {
                distance_to_rail = value;
            }
        }

        public string Side_direction
        {
            get
            {
                return side_direction;
            }

            set
            {
                side_direction = value;
            }
        }

        internal Coordinate Coordinate
        {
            get
            {
                return coordinate;
            }

            set
            {
                coordinate = value;
            }
        }

        public string Device_type
        {
            get
            {
                return device_type;
            }

            set
            {
                device_type = value;
            }
        }

        public string Kilometer_mark
        {
            get
            {
                return kilometer_mark;
            }

            set
            {
                kilometer_mark = value;
            }
        }

        public string Id
        {
            get
            {
                return id;
            }

            set
            {
                id = value;
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
    }
}
