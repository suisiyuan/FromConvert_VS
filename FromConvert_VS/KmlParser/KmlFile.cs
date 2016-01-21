using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Diagnostics;
using FromConvert_VS.Common;

namespace FromConvert_VS.KmlParser
{
    class KmlFile
    {
        private String KmlPath;     //kml文件路径
        private List<PolyData> polyDataList = new List<PolyData>();
        private List<DotData> dotDataList = new List<DotData>();


        public List<PolyData> PolyDataList
        {
            get { return polyDataList; }
        }

        public List<DotData> DotDataList
        {
            get { return dotDataList;  }
        }


        //构造函数 获取Kml文件路径
        public KmlFile(String KmlPath)
        {
            this.KmlPath = KmlPath;
        }

        //解析Kml文件内容
        public void KmlRead()
        {
            XmlDocument doc = new XmlDocument();  
            doc.Load(KmlPath);

            //准备读取文件  
            XmlElement root = doc.DocumentElement; 
            string nameSpace = root.NamespaceURI;
            XmlNamespaceManager nsmgr = new XmlNamespaceManager(doc.NameTable);
            nsmgr.AddNamespace("my", nameSpace);
            XmlNode node = doc.SelectSingleNode("my:kml/my:Document", nsmgr);
            
            //获取Folder节点列表
            XmlNodeList folderList = node.SelectNodes("my:Folder", nsmgr);


            foreach (XmlNode folder in folderList)
            {
                XmlNode folderName = folder.SelectSingleNode("my:name", nsmgr);
                XmlNodeList placeMarkList = folder.SelectNodes("my:Placemark", nsmgr);
                foreach (XmlNode placeMark in placeMarkList)
                {
                    XmlNode name = placeMark.SelectSingleNode("my:name", nsmgr);
                    XmlNode LookAt = placeMark.SelectSingleNode("my:LookAt", nsmgr);
                    XmlNode styleUrl = placeMark.SelectSingleNode("my:styleUrl", nsmgr);

                    //线 polystyle
                    if (styleUrl.InnerText.Equals("#polystyle"))
                    {
                        XmlNode Polygon = placeMark.SelectSingleNode("my:Polygon", nsmgr);
                        XmlNode outerBoundaryIs = Polygon.SelectSingleNode("my:outerBoundaryIs", nsmgr);
                        XmlNode LinearRing = outerBoundaryIs.SelectSingleNode("my:LinearRing", nsmgr);
                        XmlNode coordinates = LinearRing.SelectSingleNode("my:coordinates", nsmgr);
                        PolyData data = new PolyData();

                        data.Content = name.InnerText;
                        String[] split = coordinates.InnerText.Split(new char[] { '\n' });
                        for (Int16 i = 1; i < split.GetLength(0)-1; i++)
                        {
                            Coordinate coordinate = new Coordinate();
                            coordinate = coordinate.KmlConvert(split[i]);
                            data.CoordinateList.Add(coordinate);
                        }
                        polyDataList.Add(data);

                    }
                    //点 msn_shaded_dot
                    else if (styleUrl.InnerText.Equals("#msn_shaded_dot"))
                    {
                        XmlNode Point = placeMark.SelectSingleNode("my:Point", nsmgr);
                        XmlNode coordinates = Point.SelectSingleNode("my:coordinates", nsmgr);

                        DotData data = new DotData();
                        data.Content = name.InnerText;
                        data.Coordinate = data.Coordinate.KmlConvert(coordinates.InnerText);
                        dotDataList.Add(data);

                    }
                    else
                    {

                    }
                }
            }
        }
    }
}
