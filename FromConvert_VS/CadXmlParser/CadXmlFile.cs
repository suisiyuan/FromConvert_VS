using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using FromConvert_VS.Common;

namespace FromConvert_VS.CadXmlParser
{
    class CadXmlFile
    {
        private String cadXmlPath;

        List<LineData> lineDataList = new List<LineData>();
        List<PolyData> polyDataList = new List<PolyData>();
        List<CircleData> circleDataList = new List<CircleData>();
        List<TextData> textDataList = new List<TextData>();
        List<P2DPolyData> p2DPolyDataList = new List<P2DPolyData>();

        internal List<LineData> LineDataList
        {
            get
            {
                return lineDataList;
            }

            set
            {
                lineDataList = value;
            }
        }

        internal List<PolyData> PolyDataList
        {
            get
            {
                return polyDataList;
            }

            set
            {
                polyDataList = value;
            }
        }

        internal List<CircleData> CircleDataList
        {
            get
            {
                return circleDataList;
            }

            set
            {
                circleDataList = value;
            }
        }

        internal List<TextData> TextDataList
        {
            get
            {
                return textDataList;
            }

            set
            {
                textDataList = value;
            }
        }

        internal List<P2DPolyData> P2DPolyDataList
        {
            get
            {
                return p2DPolyDataList;
            }

            set
            {
                p2DPolyDataList = value;
            }
        }

        public CadXmlFile(String cadXmlPath)
        {
            this.cadXmlPath = cadXmlPath;
        }


        public void CadXmlRead()
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(cadXmlPath);

            XmlElement root = doc.DocumentElement;
            string nameSpace = root.NamespaceURI;

            XmlNamespaceManager nsmgr = new XmlNamespaceManager(doc.NameTable);
            XmlNode Data = doc.SelectSingleNode("Data", nsmgr);

            XmlNodeList LineList = Data.SelectNodes("LINE", nsmgr);
            XmlNodeList PolyList = Data.SelectNodes("POLY", nsmgr);
            XmlNodeList CircleList = Data.SelectNodes("CIRCLE", nsmgr);
            XmlNodeList TextList = Data.SelectNodes("TEXT", nsmgr);
            XmlNodeList P2DPolyList = Data.SelectNodes("P2DPOLY", nsmgr);

            foreach (XmlNode Line in LineList)
            {
                LineData lineData = new LineData();
                lineData.Layer = Line.Attributes.GetNamedItem("layer").InnerText;
                lineData.Coordinate_start.Longitude = Convert.ToDouble(Line.Attributes.GetNamedItem("longitude_start").InnerText);
                lineData.Coordinate_start.Latitude = Convert.ToDouble(Line.Attributes.GetNamedItem("latitude_start").InnerText);
                lineData.Coordinate_end.Longitude = Convert.ToDouble(Line.Attributes.GetNamedItem("longitude_end").InnerText);
                lineData.Coordinate_end.Latitude = Convert.ToDouble(Line.Attributes.GetNamedItem("latitude_end").InnerText);
                LineDataList.Add(lineData);
            }

            foreach (XmlNode Poly in PolyList)
            {
                PolyData polyData = new PolyData();
                polyData.Layer = Poly.Attributes.GetNamedItem("layer").InnerText;
                polyData.Id = Convert.ToInt32(Poly.Attributes.GetNamedItem("id").InnerText);
                polyData.OrderId = Convert.ToInt32(Poly.Attributes.GetNamedItem("order").InnerText);
                polyData.Coordinate.Longitude = Convert.ToDouble(Poly.Attributes.GetNamedItem("longitude").InnerText);
                polyData.Coordinate.Latitude = Convert.ToDouble(Poly.Attributes.GetNamedItem("latitude").InnerText);
                PolyDataList.Add(polyData);
            }

            foreach (XmlNode Circle in CircleList)
            {
                CircleData circleData = new CircleData();
                circleData.Layer = Circle.Attributes.GetNamedItem("layer").InnerText;
                circleData.Coordinate.Longitude = Convert.ToDouble(Circle.Attributes.GetNamedItem("longitude").InnerText);
                circleData.Coordinate.Latitude = Convert.ToDouble(Circle.Attributes.GetNamedItem("latitude").InnerText);
                circleData.Radious = Convert.ToDouble(Circle.Attributes.GetNamedItem("radious").InnerText);
                CircleDataList.Add(circleData);
            }

            foreach (XmlNode Text in TextList)
            {
                TextData textData = new TextData();
                textData.Layer = Text.Attributes.GetNamedItem("layer").InnerText;
                textData.Coordinate.Longitude = Convert.ToDouble(Text.Attributes.GetNamedItem("longitude").InnerText);
                textData.Coordinate.Latitude = Convert.ToDouble(Text.Attributes.GetNamedItem("latitude").InnerText);
                textData.Content = Text.Attributes.GetNamedItem("value").InnerText.Replace("%%d", "°").Replace("'", "''");
                TextDataList.Add(textData);
            }

            foreach (XmlNode P2DPoly in P2DPolyList)
            {
                P2DPolyData p2DPoly = new P2DPolyData();

                p2DPoly.Layer = P2DPoly.Attributes.GetNamedItem("layer").InnerText;
                p2DPoly.Id = Convert.ToInt32(P2DPoly.Attributes.GetNamedItem("id").InnerText);
                p2DPoly.OrderId = Convert.ToInt32(P2DPoly.Attributes.GetNamedItem("order").InnerText);
                p2DPoly.Coordinate.Longitude = Convert.ToDouble(P2DPoly.Attributes.GetNamedItem("longitude").InnerText);
                p2DPoly.Coordinate.Latitude = Convert.ToDouble(P2DPoly.Attributes.GetNamedItem("latitude").InnerText);

                P2DPolyDataList.Add(p2DPoly);
            }
        }






    }
}
