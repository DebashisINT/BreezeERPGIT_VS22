using System;
using System.Data;
using DevExpress.XtraCharts;

namespace ERP.OMS.Management.Master
{
    public partial class management_master_DStatChart : ERP.OMS.ViewState_class.VSPage
    {
        public String[] InputName = new String[20];
        public String[] InputType = new String[20];
        public String[] InputValue = new String[20];
        public string id, FrmDt, ToDt;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["id"] != null)
            {
                if (Request.QueryString["id"].ToString() != "")
                {
                    id = Request.QueryString["id"].ToString();
                }
                else
                {
                    id = "0";
                }
            }
            else
            {
                id = "0";
            }
            if (Request.QueryString["Flag"].ToString() == "SR")
            {
                if (Request.QueryString["FrmDt"] != null)
                {
                    if (Request.QueryString["FrmDt"].ToString() != "")
                    {
                        FrmDt = Request.QueryString["FrmDt"].ToString();
                    }

                }
                if (Request.QueryString["ToDt"] != null)
                {
                    if (Request.QueryString["ToDt"].ToString() != "")
                    {
                        ToDt = Request.QueryString["ToDt"].ToString();
                    }

                }

                InputName[0] = "Module";
                InputName[1] = "ProductID";
                InputName[2] = "ExchSegmentID";
                InputName[3] = "FromDt";
                InputName[4] = "ToDt";

                InputType[0] = "V";
                InputType[1] = "I";
                InputType[2] = "I";
                InputType[3] = "V";
                InputType[4] = "V";

                InputValue[0] = "SelDailyStatistics";

                InputValue[1] = id.ToString();
                if (Session["userlastsegment"] != null)
                {
                    if (Session["userlastsegment"].ToString() != "")
                    {
                        InputValue[2] = "1";//Session["userlastsegment"].ToString();

                    }
                    else
                    {
                        InputValue[2] = "0";
                    }
                }
                else
                {
                    InputValue[2] = "0";
                }
                InputValue[3] = FrmDt.ToString();
                InputValue[4] = ToDt.ToString();

                DataTable dtDS = BusinessLogicLayer.SQLProcedures.SelectProcedureArr("SP_EQTY_NEW", InputName, InputType, InputValue);
                if (dtDS.Rows.Count > 0)
                {
                    double TotalValueClose;


                    Series series2 = new Series("Closing Rates", ViewType.Line);
                    series2.Label.LineStyle.DashStyle = DashStyle.Dot;
                    series2.Label.Shadow.Visible = false;

                    for (int i = 0; i < dtDS.Rows.Count - 1; i++)
                    {
                        TotalValueClose = Convert.ToDouble(dtDS.Rows[i]["DailyStat_Close"].ToString());

                        series2.Points.Add(new SeriesPoint(i, new double[] { Convert.ToDouble(TotalValueClose) }));

                    }

                    // Add the series to the chart.
                    //chrtDalyStat.Series.Clear();

                    chrtDalyStat.Series.Add(series2);
                }
            }
        }
    }
}