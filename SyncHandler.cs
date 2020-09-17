using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using System.Windows.Forms.VisualStyles;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace Grade_Calculator_3
{
    public static class SyncHandler
    {
        public static HttpClient HttpClient;

        private static Uri BuildUri(string canvasURL, string accessToken, string accessArea)
        {
            return new Uri(canvasURL + @"/api/v1/" + accessArea + @"?access_token=" + SyncSettings.AccessToken + @"&per_page=" + SyncSettings.ResponsePageLength);
        }

        public static List<String> ImportClassList(string canvasURL, string accessToken, out JObject[] jObjClasses)
        {
            HttpClient?.Dispose();
            HttpClient = new HttpClient
            {
                BaseAddress = BuildUri(canvasURL, accessToken,"courses"),
                Timeout = new TimeSpan(0, 0, SyncSettings.TimeoutLength)
            };
            HttpResponseMessage response;
            try
            {
                response = HttpClient.GetAsync(HttpClient.BaseAddress).Result;
            }
            catch
            {
                jObjClasses = null;
                return null;
            }

            if (!response.IsSuccessStatusCode)
            {
                MessageBox.Show(@"Error downloading data from Canvas. Check that the URL and access token are correct.", "Warning!", MessageBoxButtons.OK,
                    MessageBoxIcon.Exclamation);
                jObjClasses = null;
                return null;
            }

            var content = response.Content.ReadAsStringAsync().Result;

            try
            {
                JArray jArray = JArray.Parse(content);
                List<String> retVal = new List<string>();
                var courses = new JObject[jArray.Count];
                int c = 0;
                foreach (JObject jObj in jArray)
                {
                    try
                    {
                        courses[c] = jObj;
                        retVal.Add(jObj.GetValue("name").ToString());
                    }
                    catch
                    {
                        courses[c] = jObj;
                        retVal.Add("(Error)");
                    }
                    c++;
                }

                jObjClasses = courses;
                return retVal;
            }
            catch
            {
                MessageBox.Show(@"Error downloading data from Canvas. Check that the URL and access token are correct.", "Warning!", MessageBoxButtons.OK,
                    MessageBoxIcon.Exclamation);
                jObjClasses = null;
                return null;
            }

        }

        public static SchoolClass SyncWithCanvas(SchoolClass schoolClass, bool warning = false)
        {
            if (schoolClass.canvasData is null)
            {
                return schoolClass;
            }

            SchoolClass newSchoolClass = schoolClass;

            if (SyncSettings.ReSyncAllNonStaticData)
            {
                HttpClient?.Dispose();
                HttpClient = new HttpClient
                {
                    BaseAddress = BuildUri(SyncSettings.CanvasURL, SyncSettings.CanvasURL, "courses/" + schoolClass.canvasData.id),
                    Timeout = new TimeSpan(0, 0, SyncSettings.TimeoutLength)
                };
                HttpResponseMessage response;
                try
                {
                    response = HttpClient.GetAsync(HttpClient.BaseAddress).Result;
                    if (!response.IsSuccessStatusCode)
                    {
                        MessageBox.Show(response.ToString(), "Warning!", MessageBoxButtons.OK,
                            MessageBoxIcon.Exclamation);
                    }

                    var content = response.Content.ReadAsStringAsync().Result;

                    try
                    {
                        JObject jObj = JObject.Parse(content);
                        newSchoolClass.canvasData.name = jObj.GetValue("name").ToString();
                        newSchoolClass.canvasData.startDate = jObj.GetValue("start_at").ToString();
                        newSchoolClass.canvasData.courseCode = jObj.GetValue("course_code").ToString();
                        newSchoolClass.canvasData.gradingStandardID = jObj.GetValue("grading_standard_id").ToString();
                    }
                    catch
                    {
                        if(warning) MessageBox.Show(schoolClass.className + ":\nError updating semi-static Canvas data.", "Warning!", MessageBoxButtons.OK,
                            MessageBoxIcon.Exclamation);
                    }
                }
                catch
                { 
                    //very weird, but we can continue anyway
                }
            }

            newSchoolClass = SyncCanvasCategories(newSchoolClass); //TODO: should prob make this optional later
            if(newSchoolClass.canvasData.syncAssignments) newSchoolClass.OverrideAssignments(SyncCanvasAssignments(newSchoolClass));
            if (newSchoolClass.canvasData.syncSemiStatics)
            {
                double[] defaultGradeScale =
                {
                    0.01, -1.0, -1.0, -1.0, -1.0, -1.0, -1.0, -1.0, -1.0, -1.0, -1.0, 0.0
                };
                var gradeScale = SyncGradeScale(newSchoolClass);
                bool isEqualToDefault = true;
                int c = 0;

                if (gradeScale != null)
                {
                    foreach (double val in gradeScale)
                    {
                        if (val != defaultGradeScale[c])
                        {
                            isEqualToDefault = false;
                        }

                        c++;
                    }
                }

                if(!isEqualToDefault)
                {
                    newSchoolClass.gradeScale = gradeScale;
                }
            }
            return newSchoolClass;

        }

        public static Double[] SyncGradeScale(SchoolClass schoolClass)
        {
            Double[] gradeScale = {-1.0, -1.0, -1.0, -1.0, -1.0, -1.0, -1.0, -1.0, -1.0, -1.0, -1.0, 0.0};
            double[] defaultGradeScale =
            {
                0.01, -1.0, -1.0, -1.0, -1.0, -1.0, -1.0, -1.0, -1.0, -1.0, -1.0, 0.0
            };

            HttpClient?.Dispose();
            HttpClient = new HttpClient
            {
                BaseAddress = BuildUri(SyncSettings.CanvasURL, SyncSettings.CanvasURL, "courses/" + schoolClass.canvasData.id + "/grading_standards/" + schoolClass.canvasData.gradingStandardID),
                Timeout = new TimeSpan(0, 0, SyncSettings.TimeoutLength)
            };
            try
            {
                var response = HttpClient.GetAsync(HttpClient.BaseAddress).Result;
                if (!response.IsSuccessStatusCode)
                {
                    MessageBox.Show(schoolClass.className + ":\nError downloading grade scale data from Canvas.", "Warning!", MessageBoxButtons.OK,
                        MessageBoxIcon.Exclamation);
                    return defaultGradeScale;
                }

                var content = response.Content.ReadAsStringAsync().Result;

                try
                {
                    bool changed = false;
                    JObject temp = JObject.Parse(content);
                    var jTok = temp.GetValue("grading_scheme");
                    foreach (JObject jObj in jTok)
                    {
                        double val = jObj.GetValue("value").ToObject<Double>() * 100;
                        switch (jObj.GetValue("name").ToString())
                        {
                            case "A":
                                gradeScale[0] = val;
                                changed = true;
                                break;
                            case "A-":
                                gradeScale[1] = val;
                                changed = true;
                                break;
                            case "B+":
                                gradeScale[2] = val;
                                changed = true;
                                break;
                            case "B":
                                gradeScale[3] = val;
                                changed = true;
                                break;
                            case "B-":
                                gradeScale[4] = val;
                                changed = true;
                                break;
                            case "C+":
                                gradeScale[5] = val;
                                changed = true;
                                break;
                            case "C":
                                gradeScale[6] = val;
                                changed = true;
                                break;
                            case "C-":
                                gradeScale[7] = val;
                                changed = true;
                                break;
                            case "D+":
                                gradeScale[8] = val;
                                changed = true;
                                break;
                            case "D":
                                gradeScale[9] = val;
                                changed = true;
                                break;
                            case "D-":
                                gradeScale[10] = val;
                                changed = true;
                                break;
                        }
                        
                    }

                    if (changed)
                    {
                        return gradeScale;
                    }
                    else
                    {
                        
                        return defaultGradeScale;
                    }
                }
                catch
                {
                    return defaultGradeScale;
                }

            }
            catch
            {
                MessageBox.Show(schoolClass.className + ":\nError updating semi-static Canvas data.", "Warning!", MessageBoxButtons.OK,
                    MessageBoxIcon.Exclamation);
                return defaultGradeScale;
            }
            
        }

        public static SchoolClass SyncCanvasCategories(SchoolClass schoolClass)
        {
            HttpClient?.Dispose();
            HttpClient = new HttpClient
            {
                BaseAddress = BuildUri(SyncSettings.CanvasURL, SyncSettings.AccessToken, "courses/" + schoolClass.canvasData.id +"/assignment_groups"),
                Timeout = new TimeSpan(0, 0, SyncSettings.TimeoutLength)
            };
            HttpResponseMessage response;
            try
            {
                response = HttpClient.GetAsync(HttpClient.BaseAddress).Result;
            }
            catch
            {
                return null;
            }

            if (!response.IsSuccessStatusCode)
            {
                MessageBox.Show(schoolClass.className + ":\nError downloading data from Canvas. Check that the URL and access token are correct.", "Warning!", MessageBoxButtons.OK,
                    MessageBoxIcon.Exclamation);
                return null;
            }

            var content = response.Content.ReadAsStringAsync().Result;

            SchoolClass newSchoolClass = schoolClass;

            try
            {
                JArray jArray = JArray.Parse(content);
                int c = 0;
                foreach (JObject jObj in jArray)
                {
                    try
                    {
                        if (newSchoolClass.canvasData.canvasCategoryIDtoGCCatName == null)
                        {
                            newSchoolClass.canvasData.canvasCategoryIDtoGCCatName = new Dictionary<string, string>();
                        }

                        if (schoolClass.canvasData.canvasCategoryIDtoGCCatName.ContainsKey(jObj.GetValue("id")
                            .ToString())) //this category has already been synced, so we just need to update it
                        {
                            int index = schoolClass.CatExists(schoolClass.canvasData.canvasCategoryIDtoGCCatName[jObj.GetValue("id")
                                .ToString()]);
                            if (index == -1)
                            {
                                //this should probably never happen, but if it somehow does...
                                throw new ArgumentOutOfRangeException();
                            }

                            if (newSchoolClass.catNames[c] != jObj.GetValue("name").ToString())
                            {
                                //if the category name needs to be updated, remap the current assignments
                                SchoolClass temp = newSchoolClass;
                                temp.catNames[c] = jObj.GetValue("name").ToString();
                                newSchoolClass.RemapAssignments(temp, false);
                                newSchoolClass.RemapCurves(temp);
                                newSchoolClass.catWorths[c] = jObj.GetValue("group_weight").ToObject<Double>();
                            }
                        }
                        else //we need to add this category to the newSchoolClass
                        {
                            int index = newSchoolClass.CatExists(jObj.GetValue("name").ToString());
                            if (index == -1)
                            {
                                //the category does not exist, so we need to create it
                                Array.Resize(ref newSchoolClass.catNames, newSchoolClass.catNames.Length + 1);
                                newSchoolClass.catNames[newSchoolClass.catNames.Length - 1] = jObj.GetValue("name").ToString();

                                Array.Resize(ref newSchoolClass.catWorths, newSchoolClass.catWorths.Length + 1);
                                newSchoolClass.catWorths[newSchoolClass.catWorths.Length - 1] = jObj.GetValue("group_weight").ToObject<Double>();
                            }

                            newSchoolClass.canvasData.canvasCategoryIDtoGCCatName.Add(jObj.GetValue("id")
                                .ToString(), jObj.GetValue("name").ToString());
                        }

                        if (jObj.ContainsKey("rules") && jObj["rules"] != null)
                        {
                            var rules = JObject.Parse(jObj.GetValue("rules").ToString());
                            if (rules.ContainsKey("drop_lowest"))
                            {
                                int toDrop = rules["drop_lowest"].ToObject<int>();
                                Curve tempCurve = new Curve("$$$ADJUST$$$" + "Drop lowest in " + jObj.GetValue("name"));
                                tempCurve.active = true;
                                //TODO: Add support for "never_drop" assignments from the Canvas API
                                tempCurve.kept = toDrop;
                                tempCurve.appliedCatIndexes = new int[] { newSchoolClass.CatExists(jObj.GetValue("name").ToString()) };
                                XMLHandler.SaveCurveToFile(newSchoolClass, tempCurve, false);
                            }
                        }
                    }
                    catch
                    {
                        //the response from canvas was messed up, but we can safely ignore it and move onto the next
                    }
                    c++;
                }
                return newSchoolClass;
            }
            catch
            {
                //the JArray could not be parsed, and therefore something is critically wrong and we cannot sync, so we return the input
                MessageBox.Show(schoolClass.className + ":\nError downloading data from Canvas. Check that the URL and access token are correct.", "Warning!", MessageBoxButtons.OK,
                    MessageBoxIcon.Exclamation);
                return schoolClass;
            }
        }

        public static Assignment[] SyncCanvasAssignments(SchoolClass schoolClass)
        {
            HttpClient?.Dispose();
            HttpClient = new HttpClient
            {
                BaseAddress = BuildUri(SyncSettings.CanvasURL, SyncSettings.AccessToken, "courses/" + schoolClass.canvasData.id + "/assignments"),
                Timeout = new TimeSpan(0, 0, SyncSettings.TimeoutLength)
            };
            HttpResponseMessage response;
            try
            {
                response = HttpClient.GetAsync(HttpClient.BaseAddress).Result;
            }
            catch
            {
                return null;
            }

            if (!response.IsSuccessStatusCode)
            {
                MessageBox.Show(@"Error downloading data from Canvas. Check that the URL and access token are correct.", "Warning!", MessageBoxButtons.OK,
                    MessageBoxIcon.Exclamation);
                return null;
            }

            var content = response.Content.ReadAsStringAsync().Result;

            try
            {
                JArray jArray = JArray.Parse(content);
                List<Assignment> retVal = new List<Assignment>();
                var JAssigns = new JObject[jArray.Count];
                int c = 0;
                foreach (JObject jObj in jArray)
                {
                    try
                    {
                        retVal.Add(ParseAssgnJObjToAssignment(schoolClass, jObj));
                    }
                    catch
                    {
                        ;
                    }
                    c++;
                }

                return retVal.ToArray();
            }
            catch
            {
                MessageBox.Show(@"Error downloading data from Canvas. Check that the URL and access token are correct.", "Warning!", MessageBoxButtons.OK,
                    MessageBoxIcon.Exclamation);
                return null;
            }
        }

        public static Assignment ParseAssgnJObjToAssignment(SchoolClass schoolClass, JObject assgn)
        {
            //TODO: add a date handler that will auto set if the assgn is active based on if the assignment due date has passed
            Assignment newAssignment = new Assignment
            {
                catIndex =
                schoolClass.CatExists(
                    schoolClass.canvasData.canvasCategoryIDtoGCCatName
                        [assgn.GetValue("assignment_group_id").ToString()]),
                name = CleanseName(assgn.GetValue("name").ToString()),
                outOf = assgn.GetValue("points_possible").ToObject<Double>()
            };
            try
            {
                newAssignment.active = !assgn.GetValue("omit_from_final_grade").ToObject<Boolean>();
            }
            catch
            {
                newAssignment.active = true;
            }

            HttpClient?.Dispose();
            HttpClient = new HttpClient
            {
                BaseAddress = BuildUri(SyncSettings.CanvasURL, SyncSettings.AccessToken, "courses/" + schoolClass.canvasData.id + "/assignments/" + assgn.GetValue("id") + "/submissions/self"),
                Timeout = new TimeSpan(0, 0, SyncSettings.TimeoutLength)
            };
            HttpResponseMessage response;
            try
            {
                response = HttpClient.GetAsync(HttpClient.BaseAddress).Result;
            }
            catch
            {
                return null;
            }

            if (!response.IsSuccessStatusCode)
            {
                MessageBox.Show(@"Error downloading data from Canvas. Check that the URL and access token are correct.", "Warning!", MessageBoxButtons.OK,
                    MessageBoxIcon.Exclamation);
                return null;
            }

            var content = response.Content.ReadAsStringAsync().Result;
            double pointsEarned = 0;

            try
            {
                JArray jArray = JArray.Parse(content);
                foreach (JObject jObj in jArray)
                {
                    if (jObj == null) continue;
                    try
                    {
                        if (jObj.GetValue("grade_matches_current_submission") != null)
                        {
                            if (jObj.GetValue("grade_matches_current_submission").ToObject<Boolean>())
                            {
                                var temp = jObj.GetValue("score");
                                if (temp != null) pointsEarned = temp.ToObject<Double>();
                                break;
                            }
                        }
                    }
                    catch
                    {
                        continue;
                    }
                }
            }
            catch
            {
                try
                {
                    JObject jObj = JObject.Parse(content);
                    if (jObj.GetValue("grade_matches_current_submission").ToObject<Boolean>())
                    {
                        var temp = jObj.GetValue("score");
                        if (temp != null) pointsEarned = temp.ToObject<Double>();
                    }
                }
                catch
                {
                    ;
                }
            }
            newAssignment.points = pointsEarned;
            
            return newAssignment;
        }

        public static string CleanseName(string rawName)
        {
            string retVal = rawName;
            retVal = retVal.Replace(@":", " ");
            retVal = retVal.Replace(@"/", " ");
            retVal = retVal.Replace(@"\", " ");
            retVal = retVal.Replace(@"?", " ");
            retVal = retVal.Replace(@"|", " ");
            retVal = retVal.Replace(@"*", " ");
            return retVal;
        }
    }
}
