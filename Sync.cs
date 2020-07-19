using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Grade_Calculator_3
{
    public partial class Sync : Form
    {
        private HttpClient _httpClient = new HttpClient();
        private List<string> _importedClasses;
        private JObject[] courses;
        private string _canvasURL;
        private string _accessToken;
        private int _currentPage;

        public Sync()
        {
            InitializeComponent();
            LoadPage(1);
        }

        public void LoadPage(int page)
        {
            if (page == 1)
            {
                if (SyncSettings.CanvasURL != null)
                {
                    TextBoxCanvasURL.Text = SyncSettings.CanvasURL;
                }

                if (SyncSettings.AccessToken != null)
                {
                    TextBoxAccessToken.Text = SyncSettings.AccessToken;
                }
                ButtonBack.Enabled = false;
                groupBox1.Visible = true;
                groupBox2.Visible = false;
            }
            else if (page == 2)
            {
                if ((TextBoxCanvasURL.Text == "") || (TextBoxAccessToken.Text == ""))
                {
                    MessageBox.Show("You must enter a URL and access token!", "Warning!", MessageBoxButtons.OK,
                        MessageBoxIcon.Exclamation);
                    return;
                }

                _canvasURL = TextBoxCanvasURL.Text;
                _accessToken = TextBoxAccessToken.Text;

                groupBox2.Visible = true;
                ButtonBack.Enabled = true;
                if (CheckedListBoxCourses.Items.Count == 0)
                {
                    ButtonRefreshCourses.Text = @"Loading...";
                    ButtonRefreshCourses.Enabled = false;
                    ImportClassList();
                    ButtonRefreshCourses.Text = @"Refresh";
                    ButtonRefreshCourses.Enabled = true;
                }
            }
            else if (page == 3)
            {
                if (CheckedListBoxCourses.CheckedItems.Count == 0)
                {
                    MessageBox.Show("You must select at least one class to sync.", "Warning!", MessageBoxButtons.OK,
                        MessageBoxIcon.Exclamation);
                    return;
                }

            }

            _currentPage = page;
        }

        public Uri BuildUri(string accessArea)
        {
            return new Uri(_canvasURL + @"/api/v1/" + accessArea + @"?access_token=" + _accessToken);
        }

        private void button1_ClickAsync(object sender, EventArgs e)
        {
            ImportClassList();

            //_httpClient.Dispose();
            //_httpClient = new HttpClient
            //{
            //    BaseAddress = BuildUri("courses")
            //};
            //var result = await _httpClient.GetAsync(_httpClient.BaseAddress);
            //foreach (var msg in result.Headers.GetValues("X-Rate-Limit-Remaining"))
            //{
            //    MessageBox.Show(msg);
            //}
            //var content = await result.Content.ReadAsStringAsync();
            ////content = "\"Classes\":" + content + "}";

            //textBox1.Text = content;
            //MessageBox.Show(content);
            //JArray jArray = JArray.Parse(content);
            //if (jArray.First != null) MessageBox.Show(jArray.First.ToString());
            //else MessageBox.Show("Null");
        }

        public void ImportClassList()
        {
            _httpClient.Dispose();
            _httpClient = new HttpClient
            {
                BaseAddress = BuildUri("courses"),
                Timeout = new TimeSpan(0, 0, SyncSettings.TimeoutLength)
            };
            HttpResponseMessage response;
            try
            {
                response = _httpClient.GetAsync(_httpClient.BaseAddress).Result;
            }
            catch
            {
                return;
            }

            if (!response.IsSuccessStatusCode)
            {
                MessageBox.Show(@"Error downloading data from Canvas. Check that the URL and access token are correct.");
                return;
            }

            var content = response.Content.ReadAsStringAsync().Result;

            try{
                JArray jArray = JArray.Parse(content);
                List<String> retVal = new List<string>();
                courses = new JObject[jArray.Count];
                int c = 0;
                foreach (JObject jObj in jArray)
                {
                    courses[c] = jObj;
                    c++;
                    retVal.Add(jObj.GetValue("name").ToString());
                }
                _importedClasses = retVal;
                CheckedListBoxCourses.Items.Clear();
                CheckedListBoxCourses.Items.AddRange(retVal.ToArray());
            }
            catch
            {
                MessageBox.Show(@"Error downloading data from Canvas. Check that the URL and access token are correct.");    
                return;
            }

        }

        private void TextBoxCanvasURL_TextChanged(object sender, EventArgs e)
        {
            if (TextBoxCanvasURL.Text != null)
            {
                _canvasURL = TextBoxCanvasURL.Text;
                linkLabel1.Enabled = true;
            }
            else
            {
                _canvasURL = null;
                linkLabel1.Enabled = false;
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start(_canvasURL + @"/profile/settings");
        }

        private void ButtonBack_Click(object sender, EventArgs e)
        {
            LoadPage(_currentPage - 1);
        }

        private void ButtonNext_Click(object sender, EventArgs e)
        {
            LoadPage(_currentPage + 1);
        }

        private void ButtonRefreshCourses_Click(object sender, EventArgs e)
        {
            ButtonRefreshCourses.Text = @"Loading...";
            ButtonRefreshCourses.Enabled = false;
            ImportClassList();
            ButtonRefreshCourses.Text = @"Refresh";
            ButtonRefreshCourses.Enabled = true;
        }

        private int ClassJObjectExists(string name)
        {
            int c = 0;
            foreach (JObject jObj in courses)
            {
                try
                {
                    if (jObj.GetValue("name").ToString().Equals(name))
                    {
                        return c;
                    }
                }
                catch{}
                c++;
            }
            return -1;
        }
    }
}
