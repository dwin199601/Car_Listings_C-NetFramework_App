using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace AS1ODolotii
{
     


    class LinqXmlClass
    {
       
     
       


        
    }


    [Serializable()]
    public class Car
    {
        public string Make { get; set; }
        public int Year { get; set; }

        public string Color { get; set; }

        public decimal EngineSize { get; set; }

        public int Price { get; set; }

        public string Dealer { get; set; }

        public Car()
        {

        }

        
    }
}
