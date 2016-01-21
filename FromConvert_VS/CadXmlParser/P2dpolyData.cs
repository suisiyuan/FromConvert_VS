using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FromConvert_VS.Common;

namespace FromConvert_VS.CadXmlParser
{
    class P2DPolyData
    {
        private String layer;
        private Int32 id;
        private Int32 orderId;
        private Coordinate coordinate;

        public P2DPolyData()
        {
            Coordinate = new Coordinate();
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

        public Int32 Id
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

        public Int32 OrderId
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





    }
}
