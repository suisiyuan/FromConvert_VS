using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FromConvert_VS.KmlParser
{
    class Data
    {
        public Data() { }

        private string name;

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        private string longtitude;

        public string Longtitude
        {
            get { return longtitude; }
            set { longtitude = value; }
        }

        private string latitude;

        public string Latitude
        {
            get { return latitude; }
            set { latitude = value; }
        }

        private string coordinates;

        public string Coordinates
        {
            get { return coordinates; }
            set { coordinates = value; }
        }
    }
}
