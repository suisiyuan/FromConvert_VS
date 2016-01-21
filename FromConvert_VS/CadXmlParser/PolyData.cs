using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FromConvert_VS.Common;

namespace FromConvert_VS.CadXmlParser
{
    class PolyData
    {
        private String layer;
        private Int32 id;
        private Int32 orderId;
        private Coordinate coordinate;
        private String name;

        public PolyData()
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

        public int Id
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

        public int OrderId
        {
            get
            {
                return orderId;
            }

            set
            {
                orderId = value;
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

        public string Name
        {
            get
            {
                return name;
            }

            set
            {
                name = value;
            }
        }
    }
}
