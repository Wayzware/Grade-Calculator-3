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
        public static readonly string[] GradesAF = { "A", "AM", "BP", "B", "BM", "CP", "C", "CM", "DP", "D", "DM", "F" };
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


            XElement xSchoolClass = new XElement("GC3_Data", new XElement("SCHEMA_VER", SCHEMA_VER), 
                new XElement("ClassData",
                    new XElement("ClassName", schoolClass.className),
                    new XElement("Professor", schoolClass.professor),
                    new XElement("Term",
                        new XElement("Year", schoolClass.termYear),
                        new XElement("Season", schoolClass.termSeason)),
                    new XElement("Credits", schoolClass.credits),
                    new XElement("GradeScaleFormat", schoolClass.gradeScaleFormat),
                    GradeScaleToXElement(schoolClass.gradeScaleFormat, schoolClass.gradeScale),
                    CatToXElement(schoolClass.catNames, schoolClass.catWorths)));

            XDocument xDocument = new XDocument(xSchoolClass);
            xDocument.Save(fullFilePath);
        }

        private static XElement CatToXElement(string[] catNames, Double[] catWorths)
        {
            XElement[] Categories = new XElement[catNames.Length];
            int c = 0;
            foreach(string cat in catNames)
            {
                Categories[c] = CatXElementCombiner(catNames[c], catWorths[c]);
                c++;
            }
            return new XElement("Categories", Categories);

        }

        private static XElement CatXElementCombiner(string name, double worth)
        {
            return new XElement("Category",
                new XElement("Name", name),
                new XElement("Worth", worth));
        }

        private static XElement GradeScaleToXElement(int format, double[] gradeScale)
        {
            XElement xGradeScale = null;
            //A-F
            if (format == 1)
            {
                xGradeScale = new XElement("GradeScale", GradeScaleAF(gradeScale));
            }

            return xGradeScale;
        }

        private static XElement[] GradeScaleAF(double[] gradeScale)
        {
            XElement[] retVal = new XElement[12];
            int c = 0;
            foreach(Double value in gradeScale)
            {
                retVal[c] = new XElement(GradesAF[c], value);
                c++;
            }
            retVal[c] = new XElement(GradesAF[c], 0);
            return retVal;
        }
    }
}
