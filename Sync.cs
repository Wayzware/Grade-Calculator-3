using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
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

        public Sync()
        {
            InitializeComponent();

        }

        public Uri BuildUri(string accessArea)
        {
            return new Uri(SyncSettings.CanvasURL + @"api/v1/" + accessArea + @"?access_token=" + SyncSettings.AccessToken);
        }

        private async void button1_ClickAsync(object sender, EventArgs e)
        {
            _httpClient.Dispose();
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = BuildUri("courses");
            var result = await _httpClient.GetAsync(_httpClient.BaseAddress);
            var content = await result.Content.ReadAsStringAsync();
            content = content.Remove(0, 1);
            content = content.Remove(content.Length - 2, 1);

            var json = JsonConvert.SerializeObject(content);
            JObject.Parse(json);



        }
    }
}
