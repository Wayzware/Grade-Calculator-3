using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using System.Xml;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Grade_Calculator_3
{
    public partial class Sync : Form
    {
        private List<string> _importedClasses;
        private string[] _selectedClasses;
        private JObject[] _courses;
        private string _canvasUrl;
        private string _accessToken;
        private int _currentPage;
        private Main _sender;
        private List<SchoolClass> _classesToSave;

        public Sync(Main sender, int mode = 0)
        {
            InitializeComponent();
            _sender = sender;
            ComboBoxSelectClass.DropDownStyle = ComboBoxStyle.DropDown;

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
            SyncSettings.LoadSettings();
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
                groupBox3.Visible = false;
            }
            else if (page == 2)
            {
                SyncSettings.AccessToken = TextBoxAccessToken.Text;
                SyncSettings.CanvasURL = TextBoxCanvasURL.Text;
                SyncSettings.SaveSettings();

                //fail conditions
                if ((TextBoxCanvasURL.Text == "") || (TextBoxAccessToken.Text == ""))
                {
                    MessageBox.Show(@"You must enter a URL and access token!", @"Warning!", MessageBoxButtons.OK,
                        MessageBoxIcon.Exclamation);
                    return;
                }

                _canvasUrl = TextBoxCanvasURL.Text;
                _accessToken = TextBoxAccessToken.Text;

                groupBox2.Visible = true;
                groupBox3.Visible = false;
                ButtonBack.Enabled = true;
                if (CheckedListBoxCourses.Items.Count == 0)
                {
                    ButtonRefreshCourses.Text = @"Loading...";
                    ButtonRefreshCourses.Enabled = false;
                    _importedClasses = SyncHandler.ImportClassList(_canvasUrl, _accessToken, out _courses);
                    if (_importedClasses != null)
                    {
                        CheckedListBoxCourses.Items.Clear();
                        CheckedListBoxCourses.Items.AddRange(_importedClasses.ToArray());
                    }
                    ButtonRefreshCourses.Text = @"Refresh";
                    ButtonRefreshCourses.Enabled = true;
                }
            }
            else if (_currentPage == 2 && page == 3)
            {
                //fail conditions
                if (CheckedListBoxCourses.CheckedItems.Count == 0)
                {
                    MessageBox.Show(@"You must select at least one class to sync.", @"Warning!", MessageBoxButtons.OK,
                        MessageBoxIcon.Exclamation);
                    return;
                }

                groupBox3.Visible = true;
                //initial combo box setup
                ComboBoxSelectClass.Items.Clear();
                _selectedClasses = new string[CheckedListBoxCourses.CheckedItems.Count];
                int i = 0;
                foreach (string className in CheckedListBoxCourses.CheckedItems)
                {
                    _selectedClasses[i] = className;
                    i++;
                }

                foreach (SchoolClass schoolClass in XMLHandler.Data)
                {
                    ComboBoxSelectClass.Items.Add(schoolClass.className);
                }

            }
            if (page >= 3)
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

                //subpage, starting at 0
                int subpage = page - 3;
                int totalSubpages = _selectedClasses.Length;

                //we need to save the data entered to the correct class
                if (subpage > 0 && _currentPage < page)
                {
                    //error checking
                    if (ComboBoxSelectClass.Text == "" || (SyncHandler.CleanseName(ComboBoxSelectClass.Text) != ComboBoxSelectClass.Text))
                    {
                        MessageBox.Show("Invalid class name", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }

                    int dataIndex = XMLHandler.ClassExists(ComboBoxSelectClass.Text);
                    bool createNewClass = dataIndex == -1;

                    if (createNewClass)
                    {
                        SchoolClass temp = new SchoolClass
                        {
                            className = ComboBoxSelectClass.Text,
                            professor = "",
                            termSeason = "",
                            termYear = DateTime.Now.Year,
                            credits = 0,
                            gradeScaleFormat = 1,
                            enrolled = 0,
                            gradeScale = new double[]{ 0.01, -1.0, -1.0, -1.0, -1.0, -1.0, -1.0, -1.0, -1.0, -1.0, -1.0, 0.0 },
                            catNames = new string[0],
                            catWorths = new double[0],
                            assignments = null,
                            curves = null,
                            curvedAssignments = null
                        };
                        SchoolClass.CanvasData tempCanvasData = new SchoolClass.CanvasData();
                        string canvasClassName = _selectedClasses[subpage - 1];
                        int jObjIndex = ClassJObjectExists(canvasClassName);
                        if (jObjIndex == -1)
                        {
                            MessageBox.Show("An unknown error occurred.", "Error!", MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                            return;
                        }

                        tempCanvasData.id = _courses[jObjIndex].GetValue("id").ToString();
                        tempCanvasData.syncSemiStatics = CheckBoxGradeScale.Checked;
                        tempCanvasData.syncAssignments = CheckBoxAssignments.Checked;
                        tempCanvasData.syncOnLoad = false;
                        temp.canvasData = tempCanvasData;

                        if (_classesToSave == null)
                        {
                            _classesToSave = new List<SchoolClass>();
                        }
                        _classesToSave.Add(temp);
                    }
                    else
                    {
                        SchoolClass.CanvasData tempCanvasData = new SchoolClass.CanvasData();
                        string canvasClassName = _selectedClasses[subpage - 1];
                        int jObjIndex = ClassJObjectExists(canvasClassName);
                        if (jObjIndex == -1)
                        {
                            MessageBox.Show("An unknown error occurred.", "Error!", MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                            return;
                        }
                        tempCanvasData.id = _courses[jObjIndex].GetValue("id").ToString();
                        tempCanvasData.syncSemiStatics = CheckBoxGradeScale.Checked;
                        tempCanvasData.syncOnLoad = false;
                        tempCanvasData.syncAssignments = CheckBoxAssignments.Checked;
                        XMLHandler.Data[dataIndex].canvasData = tempCanvasData;
                        if (_classesToSave == null)
                        {
                            _classesToSave = new List<SchoolClass>();
                        }
                        _classesToSave.Add(XMLHandler.Data[dataIndex]);
                    }

                    if (subpage == totalSubpages)
                    {
                        bool errors = false;
                        foreach (SchoolClass schoolClass in _classesToSave)
                        {
                            if (!XMLHandler.SaveSchoolClassToFile(schoolClass, XMLHandler.D_SCHEMA_VER, false))
                            {
                                errors = true;
                            }
                        }

                        if (errors)
                        {
                            MessageBox.Show("At least one class could not be saved due to an unknown error.", "Error!", MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                        }

                        _sender.InitialSetup();
                        _sender.SyncAll();
                        this.Close();
                        return;
                    }
                }

                groupBox3.Text = @"Setup Sync for: " + _selectedClasses[subpage];
                ComboBoxSelectClass.Text = "";
                CheckBoxGradeScale.Checked = false;
                CheckBoxAssignments.Checked = false;
                //CheckBoxSyncOnLaunch.Checked = false;
                ButtonNext.Text = subpage == totalSubpages ? @"Finish" : @"Next";

            }

            SyncSettings.SaveSettings();
            _currentPage = page;
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
            _importedClasses = SyncHandler.ImportClassList(_canvasUrl, _accessToken, out _courses);
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
                catch
                {
                    ;
                }
                c++;
            }
            return -1;
        }
    }
}
