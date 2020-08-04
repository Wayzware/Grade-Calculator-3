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
        private JObject[] _courses;
        private string _canvasUrl;
        private string _accessToken;
        private int _currentPage;
        private int _subpage;
        private JObject _coursesToSync;

        public Sync(int mode = 0)
        {
            InitializeComponent();
            if (mode == 0) //setup
            {
                LoadPage(1);
            }
            else if (mode == 1) //change just credentials/url
            {

            }
        }

        public void LoadPage(int page)
        {
            if (page == 1)
            {
                if (SyncSettings.CanvasURL != null && TextBoxCanvasURL.Text == "")
                {
                    TextBoxCanvasURL.Text = SyncSettings.CanvasURL;
                }

                if (SyncSettings.AccessToken != null && TextBoxAccessToken.Text == "")
                {
                    TextBoxAccessToken.Text = SyncSettings.AccessToken;
                }

                ButtonBack.Enabled = false;
                groupBox1.Visible = true;
                groupBox2.Visible = false;
            }
            else if (page == 2)
            {
                //fail conditions
                if ((TextBoxCanvasURL.Text == "") || (TextBoxAccessToken.Text == ""))
                {
                    MessageBox.Show("You must enter a URL and access token!", "Warning!", MessageBoxButtons.OK,
                        MessageBoxIcon.Exclamation);
                    return;
                }

                _canvasUrl = TextBoxCanvasURL.Text;
                _accessToken = TextBoxAccessToken.Text;

                groupBox2.Visible = true;
                ButtonBack.Enabled = true;
                if (CheckedListBoxCourses.Items.Count == 0)
                {
                    ButtonRefreshCourses.Text = @"Loading...";
                    ButtonRefreshCourses.Enabled = false;
                    _importedClasses = SyncHandler.ImportClassList(_canvasUrl, _accessToken);
                    if (_importedClasses != null)
                    {
                        CheckedListBoxCourses.Items.Clear();
                        CheckedListBoxCourses.Items.AddRange(_importedClasses.ToArray());
                    }
                    ButtonRefreshCourses.Text = @"Refresh";
                    ButtonRefreshCourses.Enabled = true;
                }
            }
            else if (page == 3)
            {
                //fail conditions
                if (CheckedListBoxCourses.CheckedItems.Count == 0)
                {
                    MessageBox.Show(@"You must select at least one class to sync.", @"Warning!", MessageBoxButtons.OK,
                        MessageBoxIcon.Exclamation);
                    return;
                }

            }
            else if (page > 3)
            {
                /*
                 * each class should have 1 or 2 pages to do the following:
                 *      sync on program load (y/n)
                 *      select which assignments to be added (and maybe if they should be synced on program load)
                 *      link to an already entered class (select existing or create new)
                 *      import grade scale (y/n)
                 *      import categories (y (required if adding assignments) / n)
                 *      
                 */
                goto skipChangingPage;
            }


            _currentPage = page;
            skipChangingPage:;
        }

        private void InitIndividualClassSetup()
        {

        }

        private void TextBoxCanvasURL_TextChanged(object sender, EventArgs e)
        {
            if (TextBoxCanvasURL.Text != null)
            {
                _canvasUrl = TextBoxCanvasURL.Text;
                linkLabel1.Enabled = true;
            }
            else
            {
                _canvasUrl = null;
                linkLabel1.Enabled = false;
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start(_canvasUrl + @"/profile/settings");
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
            _importedClasses = SyncHandler.ImportClassList(_canvasUrl, _accessToken);
            if (_importedClasses != null)
            {
                CheckedListBoxCourses.Items.Clear();
                CheckedListBoxCourses.Items.AddRange(_importedClasses.ToArray());
            }
            ButtonRefreshCourses.Text = @"Refresh";
            ButtonRefreshCourses.Enabled = true;
        }

        private int ClassJObjectExists(string name)
        {
            int c = 0;
            foreach (JObject jObj in _courses)
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
