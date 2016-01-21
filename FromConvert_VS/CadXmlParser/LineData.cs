using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FromConvert_VS.Common;

namespace FromConvert_VS.CadXmlParser
{
    class LineData
    {
        private String layer;
        private Coordinate coordinate_start, coordinate_end;

        public LineData()
        {
            coordinate_start = new Coordinate();
            coordinate_end = new Coordinate();
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

        internal Coordinate Coordinate_start
        {
            get
            {
                return coordinate_start;
            }

            set
            {
                coordinate_start = value;
            }
        }

        internal Coordinate Coordinate_end
        {
            get
            {
                return coordinate_end;
            }

            set
            {
                coordinate_end = value;
            }
        }
    }
}
