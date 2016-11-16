using Common.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Widgets.DataCenterDataSync40.MeteorologyDataService;
using Widgets.DataCenterDataSync40.Extensions;

namespace Widgets.DataCenterDataSync40
{
    public partial class SyncForm : Form
    {
        private ILog logger;
        private DateTime beginTime;
        private DateTime endTime;
        private string CityCode;
        private string NMCCityCode;

        public SyncForm()
        {
            InitializeComponent();
            logger = LogManager.GetLogger<SyncForm>();
            CityCode = ConfigurationManager.AppSettings["CityCode"];
            NMCCityCode = ConfigurationManager.AppSettings["NMCCityCode"];
        }

        private bool GetDateTime()
        {
            bool result;
            try
            {
                beginTime = Convert.ToDateTime(textBox1.Text.Trim());
                if (string.IsNullOrWhiteSpace(textBox2.Text.Trim()))
                {
                    endTime = beginTime;
                }
                else
                {
                    endTime = Convert.ToDateTime(textBox2.Text.Trim());
                }
                result = true;
            }
            catch (Exception e)
            {
                logger.Error("GetDateTime failed.", e);
                result = false;
            }
            return result;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string result;
            try
            {
                if (GetDateTime())
                {
                    using (MeteorologyDataClient client = new MeteorologyDataClient())
                    {
                        List<City_WeatherForecastInfo> list = client.GetDayForecastData(new string[] { NMCCityCode }, beginTime, endTime).ToList();
                        list.ForEach(o => o.CityCode = CityCode);
                        SqlHelper.Default.Insert(list.GetDataTable("City_WeatherForeastInfo", "ExtensionData"));
                        result = string.Format("同步城市气象7天预报数据成功！{0}", DateTime.Now);
                    }
                }
                else
                {
                    result = "请输入正确的时间";
                }
            }
            catch (Exception ex)
            {
                result = string.Format("同步城市气象7天预报数据失败！{0}", DateTime.Now);
                logger.Error("同步城市气象7天预报数据失败！", ex);
            }
            textBox3.Text = result;
        }
    }
}
