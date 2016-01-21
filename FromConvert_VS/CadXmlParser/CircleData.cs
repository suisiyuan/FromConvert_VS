using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FromConvert_VS.Common;

namespace FromConvert_VS.CadXmlParser
{
    class CircleData
    {
        private String layer;
        private Coordinate coordinate;
        private Double radious;

        public CircleData()
        {
            coordinate = new Coordinate();
        }


        public string Layer
        {
            get
            {
                return layer;
            }

            set
            {
                layer = value;
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

        public double Radious
        {
            get
            {
                return radious;
            }

            set
            {
                radious = value;
            }
        }
    }
}
