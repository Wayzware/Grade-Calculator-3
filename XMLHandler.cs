using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.IO;
using System.Windows.Forms;
using System.Xml.Linq;

namespace Grade_Calculator_3
{
    static class XMLHandler
    {
        public static string DIRECTORY;
        private static readonly string FILE_EXT = ".gcdx";
        private static readonly int SCHEMA_VER = 1;

        public static void SaveSchoolClassToFile(SchoolClass schoolClass)
        {
            //make the directory if it does not exist
            if (!Directory.Exists(DIRECTORY))
            {
                Directory.CreateDirectory(DIRECTORY);
            }
            //check that overwriting data is ok (if overwriting is going to happen)
            string fullFilePath = DIRECTORY + schoolClass.className + FILE_EXT;
            if (File.Exists(fullFilePath))
            {
                var result = MessageBox.Show("Data already exists for " + schoolClass.className + " . Overwrite the file?", "Warning!", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (result == DialogResult.No)
                {
                    return;
                }
            }
            XDocument xDocument = new XDocument();

            XElement xSCHEMA_VER = new XElement("SCHEMA_VER", SCHEMA_VER);
            xDocument.Add(xSCHEMA_VER);

            xDocument.Save(fullFilePath);

        }
    }
}
