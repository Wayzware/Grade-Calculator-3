using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.IO;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using System.Xml.Linq;


namespace Grade_Calculator_3
{
    static class XMLHandler
    {
        public static readonly string[] GradesAF = { "A", "AM", "BP", "B", "BM", "CP", "C", "CM", "DP", "D", "DM", "F" };
        public static string DIRECTORY;
        private const string CLASS_DIR = "Classes/";
        private const string D_FILE_EXT = ".gcdx";
        public const int D_SCHEMA_VER = 2;
        private const string ASSGN_DIR = "Assignments/";
        private const string A_FILE_EXT = ".gcax";
        public const int A_SCHEMA_VER = 1;
        private const string CURVE_DIR = "Curves/";
        private const string C_FILE_EXT = ".gccx";
        private const int C_SCHEMA_VER = 1;
        private const string SETTINGS_DIR = "Settings/";
        private const string S_FILE_EXT = ".gcsx";
        private const int S_SCHEMA_VER = 1;

        public static SchoolClass[] Data;

        /*
         *  Schema version history:
         *      D_SCHEMA_VER:
         *          1 : v0.1-v0.3.2
         *          2 : v0.4-Present
         *      A_SCHEMA_VER:
         *          1 : v0.3-Present
         *      C_SCHEMA_VER:
         *          1 : v0.4-Present
         *      S_SCHEMA_VER:
         *          1 : v0.5-Present
         */

        public static SchoolClass[] ReadSchoolClasses()
        {
            if (!Directory.Exists(DIRECTORY + CLASS_DIR))
            {
                Directory.CreateDirectory(DIRECTORY + CLASS_DIR);
            }

            bool flag_needsUpdate = false;
            bool flag_error = false;
            int updated = 0;
            SchoolClass[] schoolClasses = new SchoolClass[0];
            string[] GC3Files = Directory.GetFiles(DIRECTORY + CLASS_DIR, "*" + D_FILE_EXT);
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
                if(!ErrorChecking.TextIsType("int", temp))
                {
                    flag_error = true;
                    continue;
                }
                int fileSchemaVer = Convert.ToInt32(temp);
                if(fileSchemaVer > D_SCHEMA_VER)
                {
                    flag_needsUpdate = true;
                    continue;
                }
                else if(fileSchemaVer < D_SCHEMA_VER)
                {
                    //the following line should be used for non-backwards compatible schema changes
                    updated += UpdateSchema(fileSchemaVer, file);
                }

                //the file is good, add it to the list to be imported
                Array.Resize(ref useableFiles, useableFiles.Length + 1);
                useableFiles[useableFiles.Length - 1] = file;
            }
            if(updated > 0)
            {
                schoolClasses = ReadSchoolClasses();
            }

            //error notifications
            if (flag_error)
            {
                MessageBox.Show("At least one " + D_FILE_EXT + " file was not in a readable format!", "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            if (flag_needsUpdate)
            {
                MessageBox.Show("At least one " + D_FILE_EXT + " file was made for a newer version of Grade Calculator 3 than is installed. There may be an update.", "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
                    XElement xCanvasData = XE.Element("CanvasData");
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
                    CC.enrolled = Convert.ToInt32(XE.Element("Enrolled").Value);

                    
                    if (xCanvasData != null)
                    {
                        CC.canvasData = XElementToCanvasData(xCanvasData);
                    }

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
                MessageBox.Show("At least one " + D_FILE_EXT + " file had an invalid entry!", "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
        //this should be fixed in future versions
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

        public static bool SaveSchoolClassToFile(SchoolClass schoolClass, int schemaVer, bool warning=true)
        {
            //make the directory if it does not exist
            if (!Directory.Exists(DIRECTORY + CLASS_DIR))
            {
                Directory.CreateDirectory(DIRECTORY + CLASS_DIR);
            }
            //check that overwriting data is ok (if overwriting is going to happen)
            string fullFilePath = DIRECTORY + CLASS_DIR + schoolClass.className + D_FILE_EXT;
            if (File.Exists(fullFilePath) && warning)
            {
                var result = MessageBox.Show("Data already exists for " + schoolClass.className + ". Overwrite the file?", "Warning!", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (result == DialogResult.No)
                {
                    return false;
                }
            }

            XElement xSchoolClass = null;

            if (schemaVer == 1)
            {
                xSchoolClass = new XElement("GC3_Data",
                    new XElement("SCHEMA_VER", schemaVer),
                    new XElement("ClassData",
                        new XElement("ClassName", schoolClass.className),
                        new XElement("Professor", schoolClass.professor),
                        new XElement("Term",
                            new XElement("Year", schoolClass.termYear),
                            new XElement("Season", schoolClass.termSeason)),
                        new XElement("Credits", schoolClass.credits),
                        new XElement("GradeScaleFormat", schoolClass.gradeScaleFormat),
                        GradeScaleToXElement(schoolClass.gradeScaleFormat, schoolClass.gradeScale),
                        CatToXElement(schoolClass.catNames, schoolClass.catWorths)
                    )
                );
            }
            else if (schemaVer == 2 && schoolClass.canvasData == null)
            {
                xSchoolClass = new XElement("GC3_Data",
                    new XElement("SCHEMA_VER", schemaVer),
                    new XElement("ClassData",
                        new XElement("ClassName", schoolClass.className),
                        new XElement("Professor", schoolClass.professor),
                        new XElement("Term",
                            new XElement("Year", schoolClass.termYear),
                            new XElement("Season", schoolClass.termSeason)),
                        new XElement("Credits", schoolClass.credits),
                        new XElement("GradeScaleFormat", schoolClass.gradeScaleFormat),
                        GradeScaleToXElement(schoolClass.gradeScaleFormat, schoolClass.gradeScale),
                        CatToXElement(schoolClass.catNames, schoolClass.catWorths),
                        new XElement("Enrolled", schoolClass.enrolled)
                    )
                );
            }
            else if (schemaVer == 2)
            {
                xSchoolClass = new XElement("GC3_Data",
                    new XElement("SCHEMA_VER", schemaVer),
                    new XElement("ClassData",
                        new XElement("ClassName", schoolClass.className),
                        new XElement("Professor", schoolClass.professor),
                        new XElement("Term",
                            new XElement("Year", schoolClass.termYear),
                            new XElement("Season", schoolClass.termSeason)),
                        new XElement("Credits", schoolClass.credits),
                        new XElement("GradeScaleFormat", schoolClass.gradeScaleFormat),
                        GradeScaleToXElement(schoolClass.gradeScaleFormat, schoolClass.gradeScale),
                        CatToXElement(schoolClass.catNames, schoolClass.catWorths),
                        new XElement("Enrolled", schoolClass.enrolled)
                    ),
                    schoolClass.canvasData.ToXElement()
                );
            }


            XDocument xDocument = new XDocument(xSchoolClass);
            try
            {
                xDocument.Save(fullFilePath);
            }
            catch
            {
                return false;
            }

            if (warning)
            {
                MessageBox.Show("File saved successfully!", "Success!", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            return true;
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
            if (c >= retVal.Length)
            {
                c = retVal.Length - 1;
            }
            retVal[c] = new XElement(GradesAF[c], 0);
            return retVal;
        }

        public static XElement DictionaryToXElement<TKey, TValue>(string XName, Dictionary<TKey, TValue> dictionary)
        {
            List<XElement> xElements = new List<XElement>(0);
            if (dictionary == null)
            {
                return new XElement(XName, null);
            }
            foreach (TKey key in dictionary.Keys)
            {
                try
                {
                    XElement temp = new XElement(key.ToString(), dictionary[key].ToString());
                    xElements.Add(temp);
                }
                catch
                {
                    ;
                }
            }
            return new XElement(XName, xElements);
        }

        public static XElement ArrayToXElement<T>(string XName, IEnumerable<T> array, string childrenName="Value")
        {
            List<XElement> xElements = new List<XElement>(0);
            if (array == null)
            {
                return new XElement(XName, null);
            }
            foreach (T val in array)
            {
                XElement temp = new XElement(childrenName, val);
                xElements.Add(temp);
            }
            return new XElement(XName, xElements);
        }

        public static SchoolClass.CanvasData XElementToCanvasData(XElement xCanvasData)
        {
            SchoolClass.CanvasData retVal = new SchoolClass.CanvasData();
            retVal.id = xCanvasData.Element("ID").Value;
            retVal.name = xCanvasData.Element("Name").Value;
            var test = xCanvasData.Element("SyncSemiStatics").ToString();
            retVal.syncSemiStatics = bool.Parse(xCanvasData.Element("SyncSemiStatics").Value);
            retVal.syncOnLoad = bool.Parse(xCanvasData.Element("SyncOnLoad").Value);
            retVal.syncAssignments = bool.Parse(xCanvasData.Element("SyncAssignments").Value);
            return retVal;

        }

        private static int UpdateSchema(int oldSchema, string file)
        {
            //update from 1 to 2
            if(oldSchema == 1)
            {
                try
                {
                    //Begin schema v1 loading
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
                    //end schema v1 loading, being adding v2 elements
                    CC.enrolled = 0;
                    //end adding v2 elements, write to file and start over
                    SaveSchoolClassToFile(CC, 2, warning:false);
                    return 1;
                }
                catch
                {
                    return 0;
                }
            }
            return 0;
        }

        public static bool DeleteClass(string className, bool warning=true)
        {
            string fullFilePath = DIRECTORY + CLASS_DIR + className + D_FILE_EXT;
            string assgnDir = DIRECTORY + ASSGN_DIR + className + "/";
            string curveDir = DIRECTORY + CURVE_DIR + className + "/";
            if (ClassFileExists(className))
            {
                if (warning)
                {
                    var result = MessageBox.Show("Are you sure you wish to delete all data for " + className + "?" 
                        + "\n" + "\n" + "NOTE: This will also delete all assignments associated with this class", "Warning!",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    if (result == DialogResult.No)
                    {
                        return false;
                    }
                }
                foreach (var file in Directory.GetFiles(assgnDir))
                {
                    File.Delete(file);
                }
                Directory.Delete(assgnDir);
                foreach (var file in Directory.GetFiles(curveDir))
                {
                    File.Delete(file);
                }
                Directory.Delete(curveDir);
                File.Delete(fullFilePath);
                if (warning) MessageBox.Show("File Deleted!", "Success!", MessageBoxButtons.OK, MessageBoxIcon.Information);

                return true;
            }
            else
            {
                if (warning)
                {
                    MessageBox.Show("File could not be deleted. It has probably already been deleted.",
                        "Notice", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                return false;
            }
        }

        private static bool ClassFileExists(string className)
        {
            string fullFilePath = DIRECTORY + CLASS_DIR + className + D_FILE_EXT;
            return File.Exists(fullFilePath);
        }

        public static void SaveAssignmentToFile(SchoolClass schoolClass, Assignment assignment, bool warning=true)
        {
            //checks to make sure DirectoryNotFoundException does not occur
            if (!Directory.Exists(DIRECTORY + ASSGN_DIR))
            {
                Directory.CreateDirectory(DIRECTORY + ASSGN_DIR);
            }
            string fullFilePath = DIRECTORY + ASSGN_DIR + schoolClass.className + "/";
            if (!Directory.Exists(fullFilePath))
            {
                Directory.CreateDirectory(fullFilePath);
            }

            fullFilePath += assignment.name + A_FILE_EXT;

            if (File.Exists(fullFilePath) && warning)
            {
                var result = MessageBox.Show("Data already exists for " + assignment.name + ". Overwrite the file?", "Warning!", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (result == DialogResult.No)
                {
                    return;
                }
            }

            XElement xAssignment = 
                new XElement("GC3_Assignment", 
                new XElement("SCHEMA_VER", A_SCHEMA_VER),
                new XElement("AssignmentData",
                    new XElement("Name", assignment.name),
                    new XElement("CatIndex", assignment.catIndex),
                    new XElement("Real", assignment.real),
                    new XElement("Active", assignment.active),
                    new XElement("Points", assignment.points),
                    new XElement("OutOf", assignment.outOf),
                    new XElement("MeanPoints", assignment.meanPoints)
                    )
                );
            XDocument xDocument = new XDocument(xAssignment);
            try
            {
                xDocument.Save(fullFilePath);
            }
            catch
            {
                MessageBox.Show(@"Name: " + assignment.name + @" is invalid", @"Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (warning)
            {
                MessageBox.Show("File saved successfully!", "Success!", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        public static Assignment[] ReadAssignments(SchoolClass schoolClass)
        {
            //checks to make sure DirectoryNotFoundException does not occur
            if (!Directory.Exists(DIRECTORY + ASSGN_DIR))
            {
                Directory.CreateDirectory(DIRECTORY + ASSGN_DIR);
            }
            string fullDirPath = DIRECTORY + ASSGN_DIR + schoolClass.className + "/";
            if (!Directory.Exists(fullDirPath))
            {
                Directory.CreateDirectory(fullDirPath);
            }

            bool flag_needsUpdate = false;
            bool flag_error = false;
            int updated = 0;
            Assignment[] assignments = new Assignment[0];
            string[] GC3Files = Directory.GetFiles(fullDirPath, "*" + A_FILE_EXT);
            string[] useableFiles = new string[0];
            foreach (string file in GC3Files)
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
                if (temp == null)
                {
                    continue;
                }
                if (!ErrorChecking.TextIsType("int", temp))
                {
                    flag_error = true;
                    continue;
                }
                int fileSchemaVer = Convert.ToInt32(temp);
                if (fileSchemaVer > A_SCHEMA_VER)
                {
                    flag_needsUpdate = true;
                    continue;
                }
                else if (fileSchemaVer < A_SCHEMA_VER)
                {
                    //implement a way to update Assignment schema versions
                }

                //the file is good, add it to the list to be imported
                Array.Resize(ref useableFiles, useableFiles.Length + 1);
                useableFiles[useableFiles.Length - 1] = file;
            }
            if (updated > 0)
            {
                assignments = ReadAssignments(schoolClass);
            }

            //error notifications
            if (flag_error)
            {
                MessageBox.Show("At least one " + A_FILE_EXT + " file was not in a readable format!", "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            if (flag_needsUpdate)
            {
                MessageBox.Show("At least one " + A_FILE_EXT + " file was made for a newer version of Grade Calculator 3 than is installed. There may be an update.", "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            if (useableFiles.Length == 0)
            {
                return null;
            }

            //continue on to converting the XML to <Assignment> type
            bool flag_invalidEntryInFile = false;
            foreach (string file in useableFiles)
            {
                try
                {
                    XElement XE = XElement.Load(file);
                    XE = XE.Element("AssignmentData");
                    Assignment assgn = new Assignment();
                    assgn.name = XE.Element("Name").Value;
                    assgn.catIndex = Convert.ToInt32(XE.Element("CatIndex").Value);
                    assgn.real = Convert.ToBoolean(XE.Element("Real").Value);
                    assgn.active = Convert.ToBoolean(XE.Element("Active").Value);
                    assgn.points = Convert.ToDouble(XE.Element("Points").Value);
                    assgn.outOf = Convert.ToDouble(XE.Element("OutOf").Value);
                    assgn.meanPoints = Convert.ToDouble(XE.Element("MeanPoints").Value);
                    Array.Resize(ref assignments, assignments.Length + 1);
                    assignments[assignments.Length - 1] = assgn;
                }
                catch
                {
                    flag_invalidEntryInFile = true;
                    continue;
                }
            }
            if (flag_invalidEntryInFile)
            {
                MessageBox.Show(@"At least one " + A_FILE_EXT + @" file had an invalid entry!", "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            if (Settings.DebugMsg)
            {
                MessageBox.Show(@"Loaded " + Convert.ToString(assignments.Length) + @" assignments successfully!",
                    @"Success!",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            return assignments;
        }

        public static bool AssignmentFileExists(SchoolClass schoolClass, Assignment assignment)
        {
            string fullFilePath = DIRECTORY + ASSGN_DIR + schoolClass.className + "/" + assignment.name + A_FILE_EXT;
            return File.Exists(fullFilePath);
        }

        public static void DeleteAssignment(SchoolClass schoolClass, Assignment assignment, bool warning = true)
        {
            string fullFilePath = DIRECTORY + ASSGN_DIR + schoolClass.className + "/" + assignment.name + A_FILE_EXT;
            File.Delete(fullFilePath);
        }

        public static void ChangeAssignmentDirName(SchoolClass newClass, SchoolClass oldClass)
        {
            string oldDir = DIRECTORY + ASSGN_DIR + oldClass.className + "/";
            string newDir = DIRECTORY + ASSGN_DIR + newClass.className + "/";
            Directory.Move(oldDir, newDir);
        }

        public static Curve[] ReadCurves(SchoolClass schoolClass)
        {
            if (!Directory.Exists(DIRECTORY + CURVE_DIR))
            {
                Directory.CreateDirectory(DIRECTORY + CURVE_DIR);
            }
            string fullDirPath = DIRECTORY + CURVE_DIR + schoolClass.className + "/";
            if (!Directory.Exists(fullDirPath))
            {
                Directory.CreateDirectory(fullDirPath);
            }

            bool flag_needsUpdate = false;
            bool flag_error = false;
            int updated = 0;
            Curve[] curves = new Curve[0];
            string[] GC3Files = Directory.GetFiles(fullDirPath, "*" + C_FILE_EXT);
            string[] useableFiles = new string[0];
            foreach (string file in GC3Files)
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
                if (temp == null)
                {
                    continue;
                }
                if (!ErrorChecking.TextIsType("int", temp))
                {
                    flag_error = true;
                    continue;
                }
                int fileSchemaVer = Convert.ToInt32(temp);
                if (fileSchemaVer > C_SCHEMA_VER)
                {
                    flag_needsUpdate = true;
                    continue;
                }
                else if (fileSchemaVer < C_SCHEMA_VER)
                {
                    //implement a way to update curve schema versions
                }

                //the file is good, add it to the list to be imported
                Array.Resize(ref useableFiles, useableFiles.Length + 1);
                useableFiles[useableFiles.Length - 1] = file;
            }
            if (updated > 0)
            {
                curves = ReadCurves(schoolClass);
            }

            //error notifications
            if (flag_error)
            {
                MessageBox.Show("At least one " + C_FILE_EXT + " file was not in a readable format!", "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            if (flag_needsUpdate)
            {
                MessageBox.Show("At least one " + C_FILE_EXT + " file was made for a newer version of Grade Calculator 3 than is installed. There may be an update.", "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            if (useableFiles.Length == 0)
            {
                return null;
            }

            //continue on to converting the XML to <Assignment> type
            bool flag_invalidEntryInFile = false;
            foreach (string file in useableFiles)
            {
                try
                {
                    XElement XE = XElement.Load(file);
                    XE = XE.Element("CurveData");
                    Curve curve = new Curve(XE.Element("Name").Value);
                    curve.active = Convert.ToBoolean(XE.Element("Active").Value);
                    curve.ignoreUserInactives = Convert.ToBoolean(XE.Element("IgnoreUserActives").Value);
                    curve.appliedCatIndexes = XCurveCatIndexesToInt(XE.Element("CatIndexes"));
                    curve.appliedAssgnNames = XCurveAssgnNamesToString(XE.Element("AssgnNames"));
                    curve.kept = Convert.ToInt32(XE.Element("Kept").Value);
                    curve.conDropPercent = Convert.ToDouble(XE.Element("ConDropPercent").Value);
                    curve.conDropPoints = Convert.ToDouble(XE.Element("ConDropPoints").Value);
                    curve.additive = Convert.ToDouble(XE.Element("Additive").Value);
                    curve.multiplicative = Convert.ToDouble(XE.Element("Multiplicative").Value);
                    curve.additivePercent = Convert.ToDouble(XE.Element("AdditivePercent").Value);
                    curve.goalMeanPercent = Convert.ToDouble(XE.Element("GoalMeanPercent").Value);
                    curve.goalMeanPercentMethod = Convert.ToInt32(XE.Element("GoalMeanPercentMethod").Value);

                    Array.Resize(ref curves, curves.Length + 1);
                    curves[curves.Length - 1] = curve;
                }
                catch
                {
                    flag_invalidEntryInFile = true;
                    continue;
                }
            }
            if (flag_invalidEntryInFile)
            {
                MessageBox.Show(@"At least one " + C_FILE_EXT + @" file had an invalid entry!", "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            if (Settings.DebugMsg)
            {
                MessageBox.Show(@"Loaded " + Convert.ToString(curves.Length) + @" curves successfully!",
                    @"Success!",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            return curves;
        }

        public static void SaveCurveToFile(SchoolClass schoolClass, Curve curve, bool warning = true)
        {
            //checks to make sure DirectoryNotFoundException does not occur
            if (!Directory.Exists(DIRECTORY + CURVE_DIR))
            {
                Directory.CreateDirectory(DIRECTORY + CURVE_DIR);
            }
            string fullFilePath = DIRECTORY + CURVE_DIR + schoolClass.className + "/";
            if (!Directory.Exists(fullFilePath))
            {
                Directory.CreateDirectory(fullFilePath);
            }

            fullFilePath += curve.name + C_FILE_EXT;

            if (File.Exists(fullFilePath) && warning)
            {
                var result = MessageBox.Show("Data already exists for " + curve.name + ". Overwrite the file?", "Warning!", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (result == DialogResult.No)
                {
                    return;
                }
            }

            XElement xCurve =
                new XElement("GC3_Curve",
                    new XElement("SCHEMA_VER", C_SCHEMA_VER),
                    new XElement("CurveData",
                        new XElement("Name", curve.name),
                        new XElement("Active", curve.active),
                        new XElement("IgnoreUserActives", curve.ignoreUserInactives),
                        CurveCatIndexesToXElement(curve.appliedCatIndexes),
                        CurveAssgnNamesToXElement(curve.appliedAssgnNames),
                        new XElement("Kept", curve.kept),
                        new XElement("ConDropPercent", curve.conDropPercent),
                        new XElement("ConDropPoints", curve.conDropPoints),
                        new XElement("Additive", curve.additive),
                        new XElement("Multiplicative", curve.multiplicative),
                        new XElement("AdditivePercent", curve.additivePercent),
                        new XElement("GoalMeanPercent", curve.goalMeanPercent),
                        new XElement("GoalMeanPercentMethod", curve.goalMeanPercentMethod)
                    )
                );
            XDocument xDocument = new XDocument(xCurve);
            try
            {
                xDocument.Save(fullFilePath);
            }
            catch
            {
                MessageBox.Show(@"Name is invalid", @"Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (warning)
            {
                MessageBox.Show("File saved successfully!", "Success!", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private static XElement CurveCatIndexesToXElement(int[] indexes)
        {
            XElement[] nestedXE = new XElement[indexes.Length];
            int c = 0;
            foreach (int val in indexes)
            {
                nestedXE[c] = new XElement("CatIndex", val);
                c++;
            }
            XElement retVal = new XElement("CatIndexes", nestedXE);
            return retVal;
        }

        private static XElement CurveAssgnNamesToXElement(string[] names)
        {
            XElement[] nestedXE = new XElement[names.Length];
            int c = 0;
            foreach (string val in names)
            {
                nestedXE[c] = new XElement("Name", val);
                c++;
            }
            XElement retVal = new XElement("AssgnNames", nestedXE);
            return retVal;
        }

        private static int[] XCurveCatIndexesToInt(XElement catIndexes)
        {
            int[] indexes = new int[0];
            int c = 0;
            foreach (XElement XIndex in catIndexes.Elements())
            {
                //error checking is handled by parent thread
                Array.Resize(ref indexes, c + 1);
                indexes[c] = Convert.ToInt32(XIndex.Value);
                c++;
            }
            return indexes;
        }

        private static string[] XCurveAssgnNamesToString(XElement assgnNames)
        {
            string[] names = new string[0];
            int c = 0;
            foreach (XElement XIndex in assgnNames.Elements())
            {
                //error checking is handled by parent thread
                Array.Resize(ref names, c + 1);
                names[c] = XIndex.Value;
                c++;
            }
            return names;
        }

        public static bool CurveFileExists(SchoolClass schoolClass, Curve curve)
        {
            string fullFilePath = DIRECTORY + CURVE_DIR + schoolClass.className + "/" + curve.name + C_FILE_EXT;
            return File.Exists(fullFilePath);
        }

        public static void DeleteCurve(SchoolClass schoolClass, Curve curve)
        {
            string fullFilePath = DIRECTORY + CURVE_DIR + schoolClass.className + "/" + curve.name + C_FILE_EXT;
            File.Delete(fullFilePath);
        }

        public static int ClassExists(string className)
        {
            int c = 0;
            foreach (SchoolClass schoolClass in Data)
            {
                if (schoolClass.className == className)
                {
                    return c;
                }
                c++;
            }
            return -1;
        }

    }

    static class Settings
    {

        public static bool DebugMsg = false;
        public static int WarningLevel = 0;
        public static bool AlwaysDeleteOldAssignment = false;
        public static bool unrestrictedCurves = false;
    }

    static class SyncSettings
    {
        public static string AccessToken = "";
        public static string CanvasURL = "https://umn.instructure.com/";
        public static int TimeoutLength = 100;
        public static string ResponsePageLength = "256";
        public static bool ReSyncAllNonStaticData = true;

        public static void LoadSettings()
        {
            AccessToken = Properties.Settings.Default.AccessToken;
            CanvasURL = Properties.Settings.Default.CanvasURL;
            if (AccessToken == null) AccessToken = "";
            if (CanvasURL == null) CanvasURL = "";
        }

        public static void SaveSettings()
        {
            Properties.Settings.Default.AccessToken = AccessToken;
            Properties.Settings.Default.CanvasURL = CanvasURL;
            Properties.Settings.Default.Save();
        }
    }
}
