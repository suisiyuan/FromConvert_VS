using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FromConvert_VS.Common;

namespace FromConvert_VS.CadXmlParser
{
    class TextData
    {
        private String layer;
        private Coordinate coordinate;
        private String content;
        private String type;

        public TextData()
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

        public string Type
        {
            get
            {
                return type;
            }

            set
            {
                type = value;
            }
        }
    }
}
