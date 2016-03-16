using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FromConvert_VS.Common;

namespace FromConvert_VS.KmlParser
{
    class PolyData
    {
        
        public PolyData()
        {
            coordinateList = new List<Coordinate>();
        }

        private String content;
        private List<Coordinate> coordinateList;

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

        internal List<Coordinate> CoordinateList
        {
            get
            {
                return coordinateList;
            }

            set
            {
                coordinateList = value;
            }
        }
    }
}
