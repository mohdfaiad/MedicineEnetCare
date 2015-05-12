using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

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
