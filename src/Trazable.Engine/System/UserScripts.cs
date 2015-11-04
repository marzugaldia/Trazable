using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace Trazable.Engine.System
{
    public class UserScripts : List<UserScript>
    {
        public static void Save(UserScripts scripts, string fileName = "scripts.xml")
        {
            XmlDocument xmlDocument = new XmlDocument();
            XmlSerializer serializer = new XmlSerializer(scripts.GetType());
            using (MemoryStream stream = new MemoryStream())
            {
                serializer.Serialize(stream, scripts);
                stream.Position = 0;
                xmlDocument.Load(stream);
                xmlDocument.Save(fileName);
                stream.Close();
            }
        }

        public static UserScripts Load(string fileName = "scripts.xml")
        {
            try
            {
                UserScripts result = null;
                XmlDocument xmlDocument = new XmlDocument();
                xmlDocument.Load(fileName);
                string xmlString = xmlDocument.OuterXml;
                using (StringReader read = new StringReader(xmlString))
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(UserScripts));
                    using (XmlReader reader = new XmlTextReader(read))
                    {
                        result = (UserScripts)serializer.Deserialize(reader);
                        reader.Close();
                    }
                    read.Close();
                }
                return result;
            }
            catch
            {
                return new UserScripts();
            }
        }



    }
}
