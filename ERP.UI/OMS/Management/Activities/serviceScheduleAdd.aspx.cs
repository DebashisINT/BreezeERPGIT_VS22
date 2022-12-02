using BusinessLogicLayer;
using DataAccessLayer;
using DevExpress.Web;
using DevExpress.Web.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ERP.OMS.Management.Activities
{
    public partial class serviceScheduleAdd : System.Web.UI.Page
    {
        MasterSettings objmaster = new MasterSettings();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                hdnDocumentSegmentSettings.Value = objmaster.GetSettings("DocumentSegment");

                if (hdnDocumentSegmentSettings.Value == "0")
                {
                    DivSegment1.Attributes.Add("style", "display:none");
                    DivSegment2.Attributes.Add("style", "display:none");
                    DivSegment3.Attributes.Add("style", "display:none");
                    DivSegment4.Attributes.Add("style", "display:none");
                    DivSegment5.Attributes.Add("style", "display:none");

                    grid.Columns[4].Visible = false;
                    grid.Columns[5].Visible = false;
                    grid.Columns[6].Visible = false;
                    grid.Columns[7].Visible = false;
                    grid.Columns[8].Visible = false;
                }
                else
                {
                    grid.Columns[4].Visible = true;
                    grid.Columns[5].Visible = true;
                    grid.Columns[6].Visible = true;
                    grid.Columns[7].Visible = true;
                    grid.Columns[8].Visible = true;
                }
            }
        }
        protected void grid_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            if (e.Parameters.Split()[0] == "BindSchedule")
            {
                grid.JSProperties["cpmsg"] = null;
                string customer = hdnCustomerId.Value;
                string segment1 = hdnSegment1.Value;
                string segment2 = hdnSegment2.Value;
                string segment3 = hdnSegment3.Value;
                string segment4 = hdnSegment4.Value;
                string segment5 = hdnSegment5.Value;
                string day = ASPxDropDownEdit2.Text;
                string date = ASPxDropDownEdit1.Text;
                string order_id = hdnServiceContractId.Value;
                string order_no = txtServiceContract.Text;
                string orderdetails_id = Convert.ToString(ddlProduct.Value);
                string Frequency = txtFrequency.Text;
                string NoOfService = txtNoOfService.Text;
                //Mantis Issue 24306
                string start_date = OnceDate.Text;
                //End Of Mantis Issue 24306

                //OnceDate.Date = DateTime.Now;
                DateTime dtOnceDate = OnceDate.Date;

                string[] arrDays = day.Split(';');
                string[] arrDate = date.Split(';');


                string day1 = "";
                string day2 = "";
                string day3 = "";
                string date1 = "";
                string date2 = "";
                string date3 = "";


                string validation = "";
                if (0 == 1)
                {
                    if (string.IsNullOrEmpty(customer))
                    {
                        validation = "Please select customer to proceed.";
                        goto DIRECT_PASS;

                    }

                    if (string.IsNullOrEmpty(order_id))
                    {
                        validation = "Please select a contract to proceed.";
                        goto DIRECT_PASS;

                    }
                    if (string.IsNullOrEmpty(orderdetails_id))
                    {
                        validation = "Please select a service to proceed.";
                        goto DIRECT_PASS;

                    }
                    if (string.IsNullOrEmpty(Frequency))
                    {
                        validation = "Frequency can not be blank(Please select a Frequency from service contract.).";
                        goto DIRECT_PASS;

                    }

                    if (Frequency == "DAILY")
                    {


                    }
                    else if (Frequency == "HALF-YEARLY")
                    {
                        if (arrDate.Length == 0)
                        {
                            validation = "Please select date to proceed.";
                            goto DIRECT_PASS;
                        }
                        if (arrDate.Length > 1)
                        {
                            validation = "You can select only one date for this frequency.";
                            goto DIRECT_PASS;

                        }
                        else
                        {
                            day1 = arrDate[0];
                        }
                    }
                    else if (Frequency == "MONTHLY ONCE")
                    {
                        if (arrDate.Length == 0)
                        {
                            validation = "Please select date to proceed.";
                            goto DIRECT_PASS;

                        }
                        if (arrDate.Length > 1)
                        {
                            validation = "You can select only one date for this frequency.";
                            goto DIRECT_PASS;

                        }
                        else
                        {
                            day1 = arrDate[0];
                        }
                    }
                    else if (Frequency == "MONTHLY THRICE")
                    {
                        if (arrDate.Length == 0)
                        {
                            validation = "Please select date to proceed.";
                            goto DIRECT_PASS;

                        }
                        if (arrDate.Length != 3)
                        {
                            validation = "You have to select 3 date for this frequency.";
                            goto DIRECT_PASS;

                        }
                        else
                        {
                            day1 = arrDate[0];
                            day2 = arrDate[1];
                            day3 = arrDate[2];

                        }
                    }
                    else if (Frequency == "MONTHLY TWISE")
                    {
                        if (arrDate.Length == 0)
                        {
                            validation = "Please select date to proceed.";
                            goto DIRECT_PASS;

                        }
                        if (arrDate.Length != 2)
                        {
                            validation = "You have to select 2 date for this frequency.";
                            goto DIRECT_PASS;

                        }
                        else
                        {
                            day1 = arrDate[0];
                            day2 = arrDate[1];
                        }
                    }
                    else if (Frequency == "QUARTERLY ONCE")
                    {
                        if (arrDate.Length == 0)
                        {
                            validation = "Please select date to proceed.";
                            goto DIRECT_PASS;

                        }
                        if (arrDate.Length > 1)
                        {
                            validation = "You can select only one date for this frequency.";
                            goto DIRECT_PASS;

                        }
                        else
                        {
                            day1 = arrDate[0];
                        }
                    }
                    else if (Frequency == "SINGLE-ONCE")
                    {
                        if (dtOnceDate == null)
                        {
                            validation = "Please enter date to proceed.";
                            goto DIRECT_PASS;

                        }

                    }
                    else if (Frequency == "WEEKLY ONCE")
                    {
                        if (arrDays.Length == 0)
                        {
                            validation = "Please select day to proceed.";
                            goto DIRECT_PASS;

                        }
                        if (arrDays.Length > 1)
                        {
                            validation = "You can select only one day for this frequency.";
                            goto DIRECT_PASS;

                        }
                        else
                        {
                            date1 = arrDays[0];
                        }
                    }
                    else if (Frequency == "WEEKLY THRICE")
                    {
                        if (arrDays.Length == 0)
                        {
                            validation = "Please select day to proceed.";
                            goto DIRECT_PASS;

                        }
                        if (arrDays.Length != 3)
                        {
                            validation = "You have to select 3 day for this frequency.";
                            goto DIRECT_PASS;

                        }
                        else
                        {
                            date1 = arrDays[0];
                            date2 = arrDays[1];
                            date3 = arrDays[2];
                        }
                    }
                    else if (Frequency == "WEEKLY TWISE")
                    {
                        if (arrDays.Length == 0)
                        {
                            validation = "Please select day to proceed.";
                            goto DIRECT_PASS;

                        }
                        if (arrDays.Length != 2)
                        {
                            validation = "You have to select 2 day for this frequency.";
                            goto DIRECT_PASS;

                        }
                        else
                        {
                            date1 = arrDays[0];
                            date2 = arrDays[1];
                        }
                    }
                    else if (Frequency == "YEARLY THRICE")
                    {
                        if (arrDate.Length == 0)
                        {
                            validation = "Please select date to proceed.";
                            goto DIRECT_PASS;

                        }
                        if (arrDate.Length > 1)
                        {
                            validation = "You can select only one date for this frequency.";
                            goto DIRECT_PASS;

                        }
                        else
                        {
                            day1 = arrDate[0];
                        }
                    }
                }
                else
                {
                    if (dtOnceDate.Date != null && dtOnceDate.Date.Year == 1900)
                    {
                        validation = "Please select a valid start date to proceed.";
                        goto DIRECT_PASS;
                    }
                    else if (string.IsNullOrEmpty(NoOfService) || NoOfService =="0")
                    {
                        validation = "No of days can not be blank(Please select a Frequency from service contract.).";
                        goto DIRECT_PASS;
                    }
                }
                //Mantis Issue 24306
                if (start_date == null || start_date == "")
                {
                    validation = "Please select a valid start date to proceed.";
                   
                    goto DIRECT_PASS;
                }
            //End of Mantis Issue 24306
            DIRECT_PASS:

                if (string.IsNullOrEmpty(validation))
                {
                    DataTable dt = new DataTable();
                    SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
                    SqlCommand cmd = new SqlCommand("PRC_ServiceSchedule", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@action", "ScheduleList");
                    cmd.Parameters.AddWithValue("@Customer_id", customer);
                    cmd.Parameters.AddWithValue("@ORDER_ID", order_id);
                    cmd.Parameters.AddWithValue("@ORDERDETAILS_ID", orderdetails_id.Split('~')[0]);
                    cmd.Parameters.AddWithValue("@ORDER_NUMBER", order_no);
                    cmd.Parameters.AddWithValue("@Frequency", Frequency);
                    cmd.Parameters.AddWithValue("@Segment_Id1", segment1);
                    cmd.Parameters.AddWithValue("@Segment_Id2", segment2);
                    cmd.Parameters.AddWithValue("@Segment_Id3", segment3);
                    cmd.Parameters.AddWithValue("@Segment_Id4", segment4);
                    cmd.Parameters.AddWithValue("@Segment_Id5", segment5);
                    cmd.Parameters.AddWithValue("@DAY1", day1);
                    cmd.Parameters.AddWithValue("@DATE1", date1);
                    cmd.Parameters.AddWithValue("@DAY2", day2);
                    cmd.Parameters.AddWithValue("@DATE2", date2);
                    cmd.Parameters.AddWithValue("@DAY3", day3);
                    cmd.Parameters.AddWithValue("@DATE3", date3);
                    cmd.Parameters.AddWithValue("@ONCE_DATE", dtOnceDate);
                    cmd.Parameters.AddWithValue("@NoOfService", NoOfService);
                    cmd.Parameters.AddWithValue("@START_DATE", dtOnceDate);


                    cmd.CommandTimeout = 0;
                    SqlDataAdapter da = new SqlDataAdapter();
                    da.SelectCommand = cmd;
                    da.Fill(dt);

                    cmd.Dispose();
                    con.Dispose();


                    Session["Schedule_Data"] = dt;
                    grid.DataSource = dt;
                    grid.DataBind();

                    if (hdnValueSegment1.Value == "1")
                    {
                        grid.Columns[4].Visible = true;
                    }
                    else
                    {
                        grid.Columns[4].Visible = false;
                    }

                    if (hdnValueSegment2.Value == "1")
                    {
                        grid.Columns[5].Visible = true;
                    }
                    else
                    {
                        grid.Columns[5].Visible = false;
                    }

                    if (hdnValueSegment3.Value == "1")
                    {
                        grid.Columns[6].Visible = true;
                    }
                    else
                    {
                        grid.Columns[6].Visible = false;
                    }

                    if (hdnValueSegment4.Value == "1")
                    {
                        grid.Columns[7].Visible = true;
                    }
                    else
                    {
                        grid.Columns[7].Visible = false;
                    }

                    if (hdnValueSegment5.Value == "1")
                    {
                        grid.Columns[8].Visible = true;
                    }
                    else
                    {
                        grid.Columns[8].Visible = false;
                    }
                   
                   
                }
                else
                {
                    grid.JSProperties["cpmsg"] = validation;
                }

            }
        }

        protected void ddlProduct_Callback(object sender, CallbackEventArgsBase e)
        {
            DBEngine objDb = new DBEngine();

            DataTable dt = objDb.GetDataTable("SELECT Convert(varchar(10),OrderDetails_Id)+'~'+ISNULL(frequency,'')+'~'+ISNULL(CAST(NoOfService AS VARCHAR(10)),'') Details_id,OrderDetails_ProductDescription Product_Name FROM tbl_trans_SalesOrderProducts where OrderDetails_OrderId='" + e.Parameter + "'");
            ddlProduct.Items.Clear();
            ddlProduct.DataSource = dt;
            ddlProduct.ValueField = "Details_id";
            ddlProduct.TextField = "Product_Name";
            ddlProduct.DataBind();
        }

        protected void grid_DataBinding(object sender, EventArgs e)
        {
            if (Session["Schedule_Data"] != null)
            {
                grid.DataSource = (DataTable)Session["Schedule_Data"];
            }
        }

        protected void grid_CellEditorInitialize(object sender, ASPxGridViewEditorEventArgs e)
        {
            //if (e.Column.FieldName == "QUANTITY")
            //{
            //    e.Editor.Enabled = true;
            //}           
            //else
            //{
            //    e.Editor.Enabled = false;
            //}
        }

        protected void grid_BatchUpdate(object sender, DevExpress.Web.Data.ASPxDataBatchUpdateEventArgs e)
        {
            grid.JSProperties["cpmsg"] = null;
            string validation = "";
            DataTable dt = new DataTable();
            if (Session["Schedule_Data"] != null)
            {
                dt = (DataTable)Session["Schedule_Data"];


                foreach (var args in e.UpdateValues)
                {
                    string quantity = Convert.ToString(args.NewValues["QUANTITY"]);
                    string ID = Convert.ToString(args.Keys["ID"]);

                    foreach (DataRow drr in dt.Rows)
                    {
                        string Updated_ID = Convert.ToString(drr["ID"]);

                        if (Updated_ID == ID)
                        {
                            drr["QUANTITY"] = quantity;
                        }
                    }
                }
                dt.AcceptChanges();


                DataRow[] dr = dt.Select("QUANTITY=0");

                foreach (DataRow drr in dr)
                {
                    string Schedule_No = Convert.ToString(drr["SCH_CODE"]);
                    validation = "Quantity can not be ZERO. (Schedule No. " + Schedule_No + ")";
                }

                string customer = hdnCustomerId.Value;
                string segment1 = hdnSegment1.Value;
                string segment2 = hdnSegment2.Value;
                string segment3 = hdnSegment3.Value;
                string segment4 = hdnSegment4.Value;
                string segment5 = hdnSegment5.Value;
                string day = ASPxDropDownEdit2.Text;
                string date = ASPxDropDownEdit1.Text;
                string order_id = hdnServiceContractId.Value;
                string order_no = txtServiceContract.Text;
                string orderdetails_id = Convert.ToString(ddlProduct.Value).Split('~')[0];
                string Frequency = txtFrequency.Text;


                //object sumObject;
                //sumObject = dt.Compute("Sum(QUANTITY)", string.Empty);

                //DBEngine objDB = new DBEngine();
                //DataTable dtQuantity = objDB.GetDataTable("select OrderDetails_Quantity from tbl_trans_SalesOrderProducts where OrderDetails_Id='" + orderdetails_id + "'");

                //if(Convert.ToDecimal(sumObject)!=Convert.ToDecimal(dtQuantity.Rows[0][0]))
                //{
                //    validation = "Order Quamtity and Enter Quantity mismatch. can not be proceed.";
                //}


                if (validation == "")
                {
                    DataTable dtResult = new DataTable();
                    SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
                    SqlCommand cmd = new SqlCommand("PRC_ServiceSchedule", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@action", "SaveSchedule");
                    cmd.Parameters.AddWithValue("@Customer_id", customer);
                    cmd.Parameters.AddWithValue("@ORDER_ID", order_id);
                    cmd.Parameters.AddWithValue("@ORDERDETAILS_ID", orderdetails_id.Split('~')[0]);
                    cmd.Parameters.AddWithValue("@ORDERDETAILS", dt);
                    cmd.Parameters.AddWithValue("@ORDER_NUMBER", order_no);
                    cmd.Parameters.AddWithValue("@Frequency", Frequency);
                    cmd.Parameters.AddWithValue("@Segment_Id1", segment1);
                    cmd.Parameters.AddWithValue("@Segment_Id2", segment2);
                    cmd.Parameters.AddWithValue("@Segment_Id3", segment3);
                    cmd.Parameters.AddWithValue("@Segment_Id4", segment4);
                    cmd.Parameters.AddWithValue("@Segment_Id5", segment5);
                    cmd.CommandTimeout = 0;
                    SqlDataAdapter da = new SqlDataAdapter();
                    da.SelectCommand = cmd;
                    da.Fill(dtResult);

                    cmd.Dispose();
                    con.Dispose();
                    validation = "Saved Sucessfully.";
                }



            }
            else
            {
                validation = "Schedule can not be blank.";
            }
            grid.JSProperties["cpmsg"] = validation;




        }
        protected void Grid_RowInserting(object sender, ASPxDataInsertingEventArgs e)
        {
            e.Cancel = true;
        }
        protected void Grid_RowUpdating(object sender, ASPxDataUpdatingEventArgs e)
        {
            e.Cancel = true;
        }
        protected void Grid_RowDeleting(object sender, ASPxDataDeletingEventArgs e)
        {
            e.Cancel = true;
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static object GetSegmentDetails(string CustomerId)
        {
            if (HttpContext.Current.Session["userid"] != null)
            {
                ProcedureExecute proc = new ProcedureExecute("prc_getCreditDays");
                proc.AddVarcharPara("@Action", 50, "GetSegmentDetails");
                proc.AddVarcharPara("@CustomerId", 50, CustomerId);
                DataTable Address = proc.GetTable();

                if (Address.Rows.Count > 0)
                {
                    SegmentDetails details = new SegmentDetails();
                    details.Segment1 = Convert.ToString(Address.Rows[0]["Segment1"]);
                    details.Segment2 = Convert.ToString(Address.Rows[0]["Segment2"]);
                    details.Segment3 = Convert.ToString(Address.Rows[0]["Segment3"]);
                    details.Segment4 = Convert.ToString(Address.Rows[0]["Segment4"]);
                    details.Segment5 = Convert.ToString(Address.Rows[0]["Segment5"]);

                    details.SegmentName1 = Convert.ToString(Address.Rows[0]["UsedFor1"]);
                    details.SegmentName2 = Convert.ToString(Address.Rows[0]["UsedFor2"]);
                    details.SegmentName3 = Convert.ToString(Address.Rows[0]["UsedFor3"]);
                    details.SegmentName4 = Convert.ToString(Address.Rows[0]["UsedFor4"]);
                    details.SegmentName5 = Convert.ToString(Address.Rows[0]["UsedFor5"]);
                    return details;

                }

            }
            return null;
        }
        public class SegmentDetails
        {
            public string Segment1 { get; set; }
            public string Segment2 { get; set; }
            public string Segment3 { get; set; }
            public string Segment4 { get; set; }
            public string Segment5 { get; set; }
            public string SegmentName1 { get; set; }
            public string SegmentName2 { get; set; }
            public string SegmentName3 { get; set; }
            public string SegmentName4 { get; set; }
            public string SegmentName5 { get; set; }

        }
    }
}