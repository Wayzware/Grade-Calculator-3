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

        public static SchoolClass[] Data;

        public static SchoolClass[] ReadSchoolClasses()
        {
            bool flag_needsUpdate = false;
            bool flag_error = false;
            SchoolClass[] schoolClasses = new SchoolClass[0];
            string[] GC3Files = Directory.GetFiles(DIRECTORY, "*" + FILE_EXT);
            string[] useableFiles = new string[0];
            foreach(string file in GC3Files)
            {
                XElement workingXE;
                try
                {
                    workingXE = XElement.Load(file); //<GC3_Data>
                }
                catch
                {
                    flag_error = true;
                    continue;
                }
                var temp = workingXE.Element("SCHEMA_VER").Value;
                if(temp == null)
                {
                    continue;
                }
                if(!ErrorChecking.textIsType("int", temp))
                {
                    flag_error = true;
                    continue;
                }
                int fileSchemaVer = Convert.ToInt32(temp);
                if(fileSchemaVer > SCHEMA_VER)
                {
                    flag_needsUpdate = true;
                    continue;
                }
                else if(fileSchemaVer < SCHEMA_VER)
                {
                    //For future versions, use this to update a file's schema version
                }

                //the file is good, add it to the list to be imported
                Array.Resize(ref useableFiles, useableFiles.Length + 1);
                useableFiles[useableFiles.Length - 1] = file;
            }
            //error notifications
            if (flag_error)
            {
                MessageBox.Show("At least one " + FILE_EXT + " file was not in a readable format!", "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            if (flag_needsUpdate)
            {
                MessageBox.Show("At least one " + FILE_EXT + " file was made for a newer version of Grade Calculator 3 than is installed. There may be an update.", "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            if (useableFiles.Length == 0)
            {
                return null;
            }

            //continue on to converting the XML to SchoolClass type
            bool flag_invalidEntryInFile = false;
            foreach(string file in useableFiles)
            {
                try
                {
                    XElement XE = XElement.Load(file);
                    XE = XE.Element("ClassData");
                    SchoolClass CC = new SchoolClass();
                    CC.className = XE.Element("ClassName").Value;
                    CC.professor = XE.Element("Professor").Value;
                    XElement XETerm = XE.Element("Term");
                    CC.termSeason = XETerm.Element("Season").Value;
                    CC.termYear = Convert.ToInt32(XETerm.Element("Year").Value);
                    CC.credits = Convert.ToInt32(XE.Element("Credits").Value);
                    CC.gradeScaleFormat = Convert.ToInt32(XE.Element("GradeScaleFormat").Value);
                    CC.gradeScale = XGradeScaleToDouble(CC.gradeScaleFormat, XE.Element("GradeScale"));
                    (CC.catNames, CC.catWorths) = XCatsToArray(XE.Element("Categories"));
                    Array.Resize(ref schoolClasses, schoolClasses.Length + 1);
                    schoolClasses[schoolClasses.Length - 1] = CC;
                }
                catch
                {
                    flag_invalidEntryInFile = true;
                    continue;
                }
            }
            if (flag_invalidEntryInFile)
            {
                MessageBox.Show("At least one " + FILE_EXT + " file had an invalid entry!", "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            //MessageBox.Show("Loaded " + Convert.ToString(schoolClasses.Length)+ " classes successfully!", "Success!", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return schoolClasses;
        }

        private static (string[], double[]) XCatsToArray(XElement categories)
        {
            string[] catNames = new string[0];
            Double[] catWorths = new double[0];
            foreach (XElement category in categories.Elements())
            {
                Array.Resize(ref catNames, catNames.Length + 1);
                Array.Resize(ref catWorths, catWorths.Length + 1);
                catNames[catNames.Length - 1] = category.Element("Name").Value;
                catWorths[catWorths.Length - 1] = Convert.ToDouble(category.Element("Worth").Value);
            }
            return (catNames, catWorths);
        }

        //note: this assumes the file is ordered from A-F, and will not work if unordered
        private static Double[] XGradeScaleToDouble(int format, XElement xGradeScale)
        {
            if (format == 1) {
                int c = 0;
                Double[] gradeScale = new double[11];
                foreach (XElement xGradeVal in xGradeScale.Elements())
                {
                    if(c == 11) //for the F grade, kind of a hack
                    {
                        continue;
                    }
                    //error checking is handled by parent thread
                    gradeScale[c] = Convert.ToDouble(xGradeVal.Value);
                    c++;
                }
                return gradeScale;
            }
            throw new NotImplementedException("XGradeScaleToDouble has not been updated for S/N");
        }

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
                var result = MessageBox.Show("Data already exists for " + schoolClass.className + ". Overwrite the file?", "Warning!", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
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

            MessageBox.Show("File saved successfully!", "Success!", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
