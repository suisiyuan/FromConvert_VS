using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FromConvert_VS.Common;

namespace FromConvert_VS.KmlParser
{
    class DotData
    {
        public DotData()
        {
            coordinate = new Coordinate();
        }

        private Coordinate coordinate;
        private string content;

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

        public string Content
        {
            get
            {
                return content;
            }

            set
            {
                content = value;
            }
        }
    }
}
