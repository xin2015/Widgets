using Common.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Widgets.QHEnvQFWSDataSync40
{
    public partial class SyncForm : Form
    {
        private ILog logger;
        private string sourceModel;
        private string targetModel;
        private string websiteDirectory;
        private DateTime beginTime;
        private DateTime endTime;

        public SyncForm()
        {
            InitializeComponent();
            logger = LogManager.GetLogger<SyncForm>();
            sourceModel = ConfigurationManager.AppSettings["SourceModel"];
            targetModel = ConfigurationManager.AppSettings["TargetModel"];
            websiteDirectory = ConfigurationManager.AppSettings["WebsiteDirectory"];
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
                    string cmdText = "select * from MP_CityPollutantForecastDay where TimePoint >= @BeginTime and TimePoint <= @EndTime and ModelType = @ModelType";
                    SqlParameter[] parameters = new SqlParameter[]{
                        new SqlParameter("@BeginTime",beginTime),
                        new SqlParameter("@EndTime",endTime),
                        new SqlParameter("@ModelType",sourceModel)
                    };
                    DataTable dt = SqlHelper.Default.ExecuteDataTable(cmdText, parameters);
                    dt.TableName = "MP_CityPollutantForecastDay";
                    foreach (DataRow dr in dt.Rows)
                    {
                        dr["ModelType"] = targetModel;
                    }
                    SqlHelper.Default.Insert(dt);
                    result = string.Format("CMAQ模型预报成功！{0}", DateTime.Now);
                }
                else
                {
                    result = "请输入正确的时间";
                }
            }
            catch (Exception ex)
            {
                result = string.Format("CMAQ模型预报失败！{0}", DateTime.Now);
                logger.Error("CMAQ模型预报失败！", ex);
            }
            textBox3.Text = result;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string result;
            try
            {
                if (GetDateTime())
                {
                    string cmdText = "select * from MP_ProductInfo where TimePoint >= @BeginTime and TimePoint <= @EndTime and ModelType = @ModelType and ProductCode = '1001'";
                    SqlParameter[] parameters = new SqlParameter[]{
                        new SqlParameter("@BeginTime",beginTime),
                        new SqlParameter("@EndTime",endTime),
                        new SqlParameter("@ModelType",sourceModel)
                    };
                    DataTable dt = SqlHelper.Default.ExecuteDataTable(cmdText, parameters);
                    dt.TableName = "MP_ProductInfo";
                    foreach (DataRow dr in dt.Rows)
                    {
                        string sourceFile = string.Format("{0}\\{1}\\{2}", websiteDirectory, dr["FilePath"], dr["FileName"]);
                        dr["ModelType"] = targetModel;
                        dr["FileName"] = (dr["FileName"] as string).Replace(sourceModel, targetModel);
                        string targetFile = string.Format("{0}\\{1}\\{2}", websiteDirectory, dr["FilePath"], dr["FileName"]);
                        File.Copy(sourceFile, targetFile, true);
                    }
                    SqlHelper.Default.Insert(dt);
                    result = string.Format("CMAQ预报面图成功！{0}", DateTime.Now);
                }
                else
                {
                    result = "请输入正确的时间";
                }
            }
            catch (Exception ex)
            {
                result = string.Format("CMAQ预报面图失败！{0}", DateTime.Now);
                logger.Error("CMAQ预报面图失败！", ex);
            }
            textBox3.Text = result;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string result;
            try
            {
                if (GetDateTime())
                {
                    string cmdText = "select * from MP_ProductInfo where TimePoint >= @BeginTime and TimePoint <= @EndTime and ModelType = @ModelType and ProductCode = '1002'";
                    SqlParameter[] parameters = new SqlParameter[]{
                        new SqlParameter("@BeginTime",beginTime),
                        new SqlParameter("@EndTime",endTime),
                        new SqlParameter("@ModelType",sourceModel)
                    };
                    DataTable dt = SqlHelper.Default.ExecuteDataTable(cmdText, parameters);
                    dt.TableName = "MP_ProductInfo";
                    foreach (DataRow dr in dt.Rows)
                    {
                        string sourceFile = string.Format("{0}\\{1}\\{2}", websiteDirectory, dr["FilePath"], dr["FileName"]);
                        dr["ModelType"] = targetModel;
                        dr["FileName"] = (dr["FileName"] as string).Replace(sourceModel, targetModel);
                        string targetFile = string.Format("{0}\\{1}\\{2}", websiteDirectory, dr["FilePath"], dr["FileName"]);
                        File.Copy(sourceFile, targetFile, true);
                    }
                    SqlHelper.Default.Insert(dt);
                    result = string.Format("CMAQ剖面分析成功！{0}", DateTime.Now);
                }
                else
                {
                    result = "请输入正确的时间";
                }
            }
            catch (Exception ex)
            {
                result = string.Format("CMAQ剖面分析失败！{0}", DateTime.Now);
                logger.Error("CMAQ剖面分析失败！", ex);
            }
            textBox3.Text = result;
        }
    }
}
