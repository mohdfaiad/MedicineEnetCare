using System.Linq;
using System.Xml.Linq;
using System.Collections.Generic;

namespace ENetCareMVC.Repository
{
    public static class BarCodeListExtension
    {
        public static XElement GetBarCodeXML(this List<string> barCodeList)
        {
            return new XElement("Root",
                barCodeList.Select(b => new XElement("BarCode",
                        new XAttribute("Text", b))));        
        }
    }
}
