using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using Syncfusion.Windows.Forms;

namespace Grade_Calculator_3
{
    public static class SyncHandler
    {
        public static HttpClient HttpClient;

        private static Uri BuildUri(string canvasURL, string accessToken, string accessArea)
        {
            return new Uri(canvasURL + @"/api/v1/" + accessArea + @"?access_token=" + accessToken);
        }

        public static List<String> ImportClassList(string canvasURL, string accessToken)
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

                return retVal;
            }
            catch
            {
                MessageBox.Show(@"Error downloading data from Canvas. Check that the URL and access token are correct.", "Warning!", MessageBoxButtons.OK,
                    MessageBoxIcon.Exclamation);
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
                        MessageBox.Show(@"Error downloading data from Canvas. Check that the URL and access token are correct.", "Warning!", MessageBoxButtons.OK,
                            MessageBoxIcon.Exclamation);
                        return null;
                    }

                    var content = response.Content.ReadAsStringAsync().Result;

                    try
                    {
                        JObject jObj = JObject.Parse(content);
                        newSchoolClass.canvasData.name = jObj.GetValue("name").ToString();
                        newSchoolClass.canvasData.startDate = jObj.GetValue("start_at").ToString();
                        newSchoolClass.canvasData.courseCode = jObj.GetValue("course_code").ToString();
                    }
                    catch
                    {
                        if(warning) MessageBox.Show(@"Error updating semi-static Canvas data.", "Warning!", MessageBoxButtons.OK,
                            MessageBoxIcon.Exclamation);
                    }
                }
                catch
                { 
                    //very weird, but we can continue anyway
                }
            }

            newSchoolClass = SyncCanvasCategories(newSchoolClass);
            newSchoolClass.MergeAssignments(SyncCanvasAssignments(newSchoolClass));

            return newSchoolClass;

        }

        public static Double[] SyncGradeScale(SchoolClass schoolClass)
        {

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
                MessageBox.Show(@"Error downloading data from Canvas. Check that the URL and access token are correct.", "Warning!", MessageBoxButtons.OK,
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
                                newSchoolClass.catNames.Append(jObj.GetValue("name").ToString());
                                newSchoolClass.catWorths.Append(jObj.GetValue("group_weight").ToObject<Double>());
                            }

                            newSchoolClass.canvasData.canvasCategoryIDtoGCCatName.Add(jObj.GetValue("id")
                                .ToString(), jObj.GetValue("name").ToString());
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
                MessageBox.Show(@"Error downloading data from Canvas. Check that the URL and access token are correct.", "Warning!", MessageBoxButtons.OK,
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
            Assignment newAssignment = new Assignment();
            newAssignment.active = true;
            newAssignment.catIndex =
                schoolClass.CatExists(
                    schoolClass.canvasData.canvasCategoryIDtoGCCatName
                        [assgn.GetValue("assignment_group_id").ToString()]);
            newAssignment.name = assgn.GetValue("name").ToString();
            newAssignment.outOf = assgn.GetValue("points_possible").ToObject<Double>();

            HttpClient?.Dispose();
            HttpClient = new HttpClient
            {
                BaseAddress = BuildUri(SyncSettings.CanvasURL, SyncSettings.AccessToken, "courses/" + schoolClass.canvasData.id + "/assignments/" + assgn.GetValue("id") + "/submissions"),
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
                    try
                    {
                        if (jObj.GetValue("grade_matches_current_submission").ToObject<Boolean>())
                        {
                            pointsEarned = jObj.GetValue("score").ToObject<Double>();
                            break;
                        }
                    }
                    catch
                    {
                    }
                }
            }
            catch
            {
                ;
            }

            newAssignment.points = pointsEarned;
            return newAssignment;
        }
    }
}
