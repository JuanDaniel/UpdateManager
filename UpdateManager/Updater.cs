using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

namespace BBI.JD
{
    [XmlRoot("AddinsVersion", IsNullable = false)]
    public class AddinsVersion
    {
        public string UpdateDate;

        [XmlArray("Addins")]
        public Addin[] Addins;
    }

    public class Addin
    {
        public string Id;
        public string Name;
        public string Version;
        public string Install;
    }

    public static class Updater
    {
        public static AddinsVersion Read(string filename)
        {
            AddinsVersion addinsVersion = null;

            XmlSerializer serializer = new XmlSerializer(typeof(AddinsVersion));

            FileStream fs = new FileStream(filename, FileMode.Open);

            addinsVersion = (AddinsVersion)serializer.Deserialize(fs);

            return addinsVersion;
        }

        /* Only for test */
        public static void Write(string filename) {
            XmlSerializer serializer = new XmlSerializer(typeof(AddinsVersion));

            TextWriter writer = new StreamWriter(filename);

            AddinsVersion addinsVersion = new AddinsVersion();
            addinsVersion.UpdateDate = DateTime.Now.ToString();

            List<Addin> addins = new List<Addin>();

            Addin addin;
            
            /* ImportKeySchedule */
            addin = new Addin();
            addin.Id = "93768c10-f819-40b7-bd80-030ef355862f";
            addin.Name = "ImportKeySchedule";
            addin.Version = "1.0.0.6";
            addin.Install = @"\\10.72.221.15\aplicaciones\RevitAddins\INSTALL\ImportKeySchedule.msi";

            addins.Add(addin);

            /* CenterGravity */
            addin = new Addin();
            addin.Id = "3fc9040b-1de7-4a7f-9f3a-25d9c51b7217";
            addin.Name = "CenterGravity";
            addin.Version = "1.0.0.2";
            addin.Install = @"\\10.72.221.15\aplicaciones\RevitAddins\INSTALL\CenterGravity.msi";

            addins.Add(addin);

            /* ReplaceValueParameter */
            addin = new Addin();
            addin.Id = "61217e6a-7a87-4ed8-ae8e-ef74580812f8";
            addin.Name = "ReplaceValueParameter";
            addin.Version = "1.0.0.0";
            addin.Install = @"\\10.72.221.15\aplicaciones\RevitAddins\INSTALL\ReplaceValueParameter.msi";

            addins.Add(addin);

            addinsVersion.Addins = addins.ToArray();

            serializer.Serialize(writer, addinsVersion);

            writer.Close();
        }
    }
}
