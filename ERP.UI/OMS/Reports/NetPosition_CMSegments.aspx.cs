using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessLogicLayer;

namespace ERP.OMS.Reports
{
    public partial class Reports_NetPosition_CMSegments : System.Web.UI.Page, System.Web.UI.ICallbackEventHandler
    {
        #region Global Variable
        string data;
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
        GenericMethod oGenericMethod;
        GenericExcelExport oGenericExcelExport;
        string CombinedGroupByQuery = null;
        protected DateTime currentFromDate;
        protected DateTime currentToDate;
        //protected string dislayReportType;
        protected string TotPur, TotPurMkt, TotPurAvg, TotPurNet, TotPurNetAvg, TotSale, TotSaleMkt, TotSaleAvg,
                        TotSaleNet, TotSaleNetAvg, TotSqrQty, TotSqrPL, TotDlvPur, TotDlvPurMkt, TotDlvPurAvg,
                        TotDlvPurNet, TotDlvPurNetAvg, TotDlvSale, TotDlvSaleMkt, TotDlvSaleAvg, TotDlvSaleNet,
                        TotDlvSaleNetAvg, TotNetDlv, TotClPrice, TotMtmPL, TotTotalPL, TotExposure, TotDlvTurnover,
                        TotDlvBrokerage, TotSqrTurnover, TotSqrBrokerage, TotTurnover, TotTotalBrokerage;
        #endregion

        #region PropertyVariable
        static string Company; static string DateFrom; static string DateTo; static string Segment; static string SegmentName;
        static string Product; static string GroupBy; static string GroupByID; static string GroupByType;
        static string ReportType; static string ReportBy; static int PageSize; static int PageNumber;
        static string FilterParam;
        #endregion

        #region Page Properties
        public string P_Company
        {
            get { return Company; }
            set { Company = value; }
        }
        public string P_DateFrom
        {
            get { return DateFrom; }
            set { DateFrom = value; }
        }
        public string P_DateTo
        {
            get { return DateTo; }
            set { DateTo = value; }
        }
        public string P_Segment
        {
            get { return Segment; }
            set { Segment = value; }
        }
        public string P_SegmentName
        {
            get { return SegmentName; }
            set { SegmentName = value; }
        }
        public string P_Product
        {
            get { return Product; }
            set { Product = value; }
        }
        public string P_GroupBy
        {
            get { return GroupBy; }
            set { GroupBy = value; }
        }
        public string P_GroupByID
        {
            get { return GroupByID; }
            set { GroupByID = value; }
        }
        public string P_GroupByType
        {
            get { return GroupByType; }
            set { GroupByType = value; }
        }
        public string P_FilterParam
        {
            get { return FilterParam; }
            set { FilterParam = value; }
        }
        public string P_ReportType
        {
            get { return ReportType; }
            set { ReportType = value; }
        }
        public string P_ReportBy
        {
            get { return ReportBy; }
            set { ReportBy = value; }
        }
        public int P_PageSize
        {
            get { return PageSize; }
            set { PageSize = value; }
        }
        public int P_PageNumber
        {
            get { return PageNumber; }
            set { PageNumber = value; }
        }
        #endregion

        #region CallAjax
        void CallUserList(string WhichCall)
        {
            oGenericMethod = new GenericMethod();
            if (WhichCall == "CallAjax-Segment")
            {
                oGenericMethod.GetSegments("A", ref CombinedGroupByQuery, 10, Session["LastCompany"].ToString(), "CM");
            }
            if (WhichCall == "CallAjax-Product")
            {
                oGenericMethod.GetProducts_Equity_InnerJoinByProductID("A", ref CombinedGroupByQuery, 0, 0, 0, 1);
            }
            if (WhichCall == "CallAjax-Branch")
            {
                CombinedGroupByQuery = oGenericMethod.GetAllBranch();
            }
            if (WhichCall == "CallAjax-Group")
            {
                CombinedGroupByQuery = oGenericMethod.GetAllGroups(ddlGrouptype.SelectedValue.ToString());
            }
            if (WhichCall == "CallAjax-Client")
            {
                CombinedGroupByQuery = oGenericMethod.GetClient_SegmentFilter(Session["LastCompany"].ToString(), SegmentName);
            }
        }
        #endregion

        #region Page Methods
        void ICallbackEventHandler.RaiseCallbackEvent(string eventArgument)
        {
            string id = eventArgument.ToString();
            if (id == "CallAjax-Segment" || id == "CallAjax-Product" || id == "CallAjax-Branch" || id == "CallAjax-Group" || id == "CallAjax-Client")
            {
                CallUserList(id);
                CombinedGroupByQuery = CombinedGroupByQuery.Replace("\\'", "'");
                data = "AjaxQuery@" + CombinedGroupByQuery;
            }
            else
            {
                string[] idlist = id.Split('^');
                string recieveServerIDs = "";
                for (int i = 0; i < idlist.Length; i++)
                {
                    string[] strVal = idlist[i].Split('!');
                    string[] ids = strVal[0].Split('~');
                    string whichCall = ids[ids.Length - 1];
                    if (whichCall == "COMPANYSEGMENT")
                    {
                        if (recieveServerIDs == "")
                            recieveServerIDs = idlist[i];
                        else
                            recieveServerIDs += "^" + idlist[i];
                        data = "Segment@" + recieveServerIDs.ToString();
                    }
                    else if (whichCall == "PRODUCTINNEREQUITY")
                    {
                        if (recieveServerIDs == "")
                            recieveServerIDs = idlist[i];
                        else
                            recieveServerIDs += "^" + idlist[i];
                        data = "Product@" + recieveServerIDs.ToString();
                    }
                    else if (whichCall == "BRANCH")
                    {
                        if (recieveServerIDs == "")
                            recieveServerIDs = idlist[i];
                        else
                            recieveServerIDs += "^" + idlist[i];
                        data = "Branch@" + recieveServerIDs.ToString();
                    }
                    else if (whichCall == "GROUP")
                    {
                        if (recieveServerIDs == "")
                            recieveServerIDs = idlist[i];
                        else
                            recieveServerIDs += "^" + idlist[i];
                        data = "Group@" + recieveServerIDs.ToString();
                    }
                    else if (whichCall == "CLIENT")
                    {
                        if (recieveServerIDs == "")
                            recieveServerIDs = idlist[i];
                        else
                            recieveServerIDs += "^" + idlist[i];
                        data = "Client@" + recieveServerIDs.ToString();
                    }
                }
            }
        }

        string ICallbackEventHandler.GetCallbackResult()
        {
            return data;
        }

        protected void btnAddItem_Click(object sender, EventArgs e)
        {
            if (lstAllListBox.SelectedItem.Text == "")
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ListValidate1", "ListValidate(1);", true);
            else
            {
                int lastRemoveIndex = 0;
                int itemsCount = lstAllListBox.Items.Count;
                for (int i = lstAllListBox.Items.Count - 1; i >= 0; i--)
                {
                    if (lstAllListBox.Items[i].Selected)
                    {
                        lstFilteredListBox.Items.Add(lstAllListBox.Items[i]);
                        lstAllListBox.Items.Remove(lstAllListBox.Items[i]);
                        if (i >= lastRemoveIndex)
                            lastRemoveIndex = i;
                    }
                }
                if (lstAllListBox.Items.Count > 0)
                {
                    int maxIndex = lstAllListBox.Items.Count - 1;

                    int toBeSelectedIndex = lastRemoveIndex - (itemsCount - lstAllListBox.Items.Count) + 1;
                    if (maxIndex >= toBeSelectedIndex)
                        lstAllListBox.SelectedIndex = toBeSelectedIndex;
                    else
                        lstAllListBox.SelectedIndex = 0;
                }
            }
        }
        protected void btnRemoveItem_Click(object sender, EventArgs e)
        {
            if (lstFilteredListBox.SelectedItem.Text == "")
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ListValidate2", "ListValidate(2);", true);
            else
            {
                int lastRemoveIndex = 0;
                int itemsCount = lstFilteredListBox.Items.Count;
                for (int i = lstFilteredListBox.Items.Count - 1; i >= 0; i--)
                {
                    if (lstFilteredListBox.Items[i].Selected)
                    {
                        lstAllListBox.Items.Add(lstFilteredListBox.Items[i]);
                        lstFilteredListBox.Items.Remove(lstFilteredListBox.Items[i]);
                        if (i >= lastRemoveIndex)
                            lastRemoveIndex = i;
                    }
                }
                if (lstFilteredListBox.Items.Count > 0)
                {
                    int maxIndex = lstFilteredListBox.Items.Count - 1;

                    int toBeSelectedIndex = lastRemoveIndex - (itemsCount - lstFilteredListBox.Items.Count) + 1;
                    if (maxIndex >= toBeSelectedIndex)
                        lstFilteredListBox.SelectedIndex = toBeSelectedIndex;
                    else
                        lstFilteredListBox.SelectedIndex = 0;
                }
            }
        }
        protected void btnAddAllItem_Click(object sender, EventArgs e)
        {
            foreach (ListItem i in lstAllListBox.Items)
            {
                lstFilteredListBox.Items.Add(i);
            }
            lstAllListBox.Items.Clear();
        }
        protected void btnRemoveAllItem_Click(object sender, EventArgs e)
        {
            foreach (ListItem i in lstFilteredListBox.Items)
            {
                lstAllListBox.Items.Add(i);
            }
            lstFilteredListBox.Items.Clear();
        }
        #endregion

        #region User Define Methods
        protected void FillFilterColumn()
        {
            string[] str ={"ISIN", "Pur", "PurMkt", "PurAvg", "PurNet",
            "PurNetAvg", "Sale", "SaleMkt", "SaleAvg", "SaleNet", "SaleNetAvg", "SqrQty", "SqrPL",
            "DlvPur", "DlvPurMkt", "DlvPurAvg", "DlvPurNet", "DlvPurNetAvg", "DlvSale", "DlvSaleMkt",
            "DlvSaleAvg", "DlvSaleNet", "DlvSaleNetAvg", "NetDlv", "ClPrice", "MtmPL", "TotalPL",
            "Exposure", "DlvTurnover", "DlvBrokerage", "SqrTurnover", "SqrBrokerage", "TurnOver",
            "TotalBrokerage"};
            List<String> allFilteredItems = new List<string>(str);
            lstAllListBox.DataSource = allFilteredItems;
            lstAllListBox.DataBind();
        }

        protected void AllSegment()
        {
            string hiddenSegment = string.Empty;
            oGenericMethod = new GenericMethod();
            string refStr = string.Empty;
            DataTable dtSegment = oGenericMethod.GetSegments("D", ref refStr, 0, Session["LastCompany"].ToString(), "CM");
            SegmentName = null;
            for (int i = 0; i <= dtSegment.Rows.Count - 1; i++)
            {
                hiddenSegment += dtSegment.Rows[i]["ValueField"].ToString() + "!" + dtSegment.Rows[i]["TextField"].ToString() + ",";
                if (SegmentName == null)
                    SegmentName = "'" + dtSegment.Rows[i]["ExchSegmentName"].ToString() + "'";
                else
                    SegmentName += ",'" + dtSegment.Rows[i]["ExchSegmentName"].ToString() + "'";
            }
            hiddenSegment += dtSegment.Rows[dtSegment.Rows.Count - 1]["ValueField"].ToString() + "!" + dtSegment.Rows[dtSegment.Rows.Count - 1]["TextField"].ToString();
            HDNSegment.Value = hiddenSegment;
        }

        string FilterColumnCheck()
        {
            string ISIN = "N"; string Pur = "N"; string PurMkt = "N"; string PurAvg = "N";
            string PurNet = "N"; string PurNetAvg = "N"; string Sale = "N"; string SaleMkt = "N";
            string SaleAvg = "N"; string SaleNet = "N"; string SaleNetAvg = "N"; string SqrQty = "N";
            string SqrPL = "N"; string DlvPur = "N"; string DlvPurMkt = "N"; string DlvPurAvg = "N";
            string DlvPurNet = "N"; string DlvPurNetAvg = "N"; string DlvSale = "N"; string DlvSaleMkt = "N";
            string DlvSaleAvg = "N"; string DlvSaleNet = "N"; string DlvSaleNetAvg = "N"; string NetDlv = "N";
            string ClPrice = "N"; string MtmPL = "N"; string TotalPL = "N"; string Exposure = "N";
            string DlvTurnover = "N"; string DlvBrokerage = "N"; string SqrTurnover = "N"; string SqrBrokerage = "N";
            string TurnOver = "N"; string TotalBrokerage = "N";

            foreach (ListItem filteredListItem in lstAllListBox.Items)
            {
                if (filteredListItem.Value == "ISIN") ISIN = "Y";
                if (filteredListItem.Value == "Pur") Pur = "Y";
                if (filteredListItem.Value == "PurMkt") PurMkt = "Y";
                if (filteredListItem.Value == "PurAvg") PurAvg = "Y";
                if (filteredListItem.Value == "PurNet") PurNet = "Y";
                if (filteredListItem.Value == "PurNetAvg") PurNetAvg = "Y";

                if (filteredListItem.Value == "Sale") Sale = "Y";
                if (filteredListItem.Value == "SaleMkt") SaleMkt = "Y";
                if (filteredListItem.Value == "SaleAvg") SaleAvg = "Y";
                if (filteredListItem.Value == "SaleNet") SaleNet = "Y";
                if (filteredListItem.Value == "SaleNetAvg") SaleNetAvg = "Y";

                if (filteredListItem.Value == "SqrQty") SqrQty = "Y";
                if (filteredListItem.Value == "SqrPL") SqrPL = "Y";

                if (filteredListItem.Value == "DlvPur") DlvPur = "Y";
                if (filteredListItem.Value == "DlvPurMkt") DlvPurMkt = "Y";
                if (filteredListItem.Value == "DlvPurAvg") DlvPurAvg = "Y";
                if (filteredListItem.Value == "DlvPurNet") DlvPurNet = "Y";
                if (filteredListItem.Value == "DlvPurNetAvg") DlvPurNetAvg = "Y";

                if (filteredListItem.Value == "DlvSale") DlvSale = "Y";
                if (filteredListItem.Value == "DlvSaleMkt") DlvSaleMkt = "Y";
                if (filteredListItem.Value == "DlvSaleAvg") DlvSaleAvg = "Y";
                if (filteredListItem.Value == "DlvSaleNet") DlvSaleNet = "Y";
                if (filteredListItem.Value == "DlvSaleNetAvg") DlvSaleNetAvg = "Y";

                if (filteredListItem.Value == "NetDlv") NetDlv = "Y";
                if (filteredListItem.Value == "ClPrice") ClPrice = "Y";
                if (filteredListItem.Value == "MtmPL") MtmPL = "Y";
                if (filteredListItem.Value == "TotalPL") TotalPL = "Y";
                if (filteredListItem.Value == "Exposure") Exposure = "Y";
                if (filteredListItem.Value == "DlvTurnover") DlvTurnover = "Y";
                if (filteredListItem.Value == "DlvBrokerage") DlvBrokerage = "Y";
                if (filteredListItem.Value == "SqrTurnover") SqrTurnover = "Y";
                if (filteredListItem.Value == "SqrBrokerage") SqrBrokerage = "Y";
                if (filteredListItem.Value == "TurnOver") TurnOver = "Y";
                if (filteredListItem.Value == "TotalBrokerage") TotalBrokerage = "Y";
            }
            FilterParam = ISIN.ToString().Trim() + '~' + Pur.ToString().Trim() + '~' + PurMkt.ToString().Trim() + '~' + PurAvg.ToString().Trim() + '~' +
                          PurNet.ToString().Trim() + '~' + PurNetAvg.ToString().Trim() + '~' + Sale.ToString().Trim() + '~' + SaleMkt.ToString().Trim() + '~' +
                          SaleAvg.ToString().Trim() + '~' + SaleNet.ToString().Trim() + '~' + SaleNetAvg.ToString().Trim() + '~' + SqrQty.ToString().Trim() + '~' +
                          SqrPL.ToString().Trim() + '~' + DlvPur.ToString().Trim() + '~' + DlvPurMkt.ToString().Trim() + '~' + DlvPurAvg.ToString().Trim() + '~' +
                          DlvPurNet.ToString().Trim() + '~' + DlvPurNetAvg.ToString().Trim() + '~' + DlvSale.ToString().Trim() + '~' + DlvSaleMkt.ToString().Trim() + '~' +
                          DlvSaleAvg.ToString().Trim() + '~' + DlvSaleNet.ToString().Trim() + '~' + DlvSaleNetAvg.ToString().Trim() + '~' + NetDlv.ToString().Trim() + '~' +
                          ClPrice.ToString().Trim() + '~' + MtmPL.ToString().Trim() + '~' + TotalPL.ToString().Trim() + '~' + Exposure.ToString().Trim() + '~' +
                          DlvTurnover.ToString().Trim() + '~' + DlvBrokerage.ToString().Trim() + '~' + SqrTurnover.ToString().Trim() + '~' + SqrBrokerage.ToString().Trim() + '~' +
                          TurnOver.ToString().Trim() + '~' + TotalBrokerage.ToString().Trim();
            return FilterParam;
        }

        void SetPropertiesValue()
        {
            //Company
            Company = Session["LastCompany"].ToString();
            //Segment
            if (RblSegment.SelectedIndex == 0)
            {
                Segment = string.Empty;
                oGenericMethod = new GenericMethod();
                string refStr = string.Empty;
                DataTable dtSegment = oGenericMethod.GetSegments("D", ref refStr, 0, Session["LastCompany"].ToString(), "CM");

                for (int i = 0; i < dtSegment.Rows.Count - 1; i++)
                {
                    Segment += dtSegment.Rows[i]["Session_UserSegID"].ToString() + ",";
                }
                Segment += dtSegment.Rows[dtSegment.Rows.Count - 1]["Session_UserSegID"].ToString();
            }
            else
            {
                if (HDNSegment.Value.Trim() != "")
                {
                    string[] segItems = (HDNSegment.Value.Trim()).Split('^');
                    string receiveSegID = null;
                    string receiveSegName = null;
                    for (int i = 0; i < segItems.Length; i++)
                    {
                        string[] segItem = segItems[i].Split('!');
                        string[] segIds = segItem[0].Split('~');
                        if (receiveSegID == null && receiveSegName == null)
                        {
                            receiveSegID = segIds[0];
                            receiveSegName = "'" + segItem[1] + "'";
                        }
                        else
                        {
                            receiveSegID = receiveSegID + "," + segIds[0];
                            receiveSegName = receiveSegName + ",'" + segItem[1] + "'";
                        }
                        Segment = receiveSegID;
                        SegmentName = receiveSegName;
                    }
                }
                else
                    Segment = "Error:Segment";
            }
            //Product
            if (RblProduct.SelectedIndex == 0) Product = "ALL";
            else
            {
                Product = string.Empty;
                if (HDNProduct.Value.Trim() != "")
                {
                    string[] prodSeriesItems = (HDNProduct.Value.Trim()).Split('^');
                    for (int i = 0; i < prodSeriesItems.Length; i++)
                    {
                        string[] prodSeriesItem = prodSeriesItems[i].Split('!');
                        string[] prodSeriesIds = prodSeriesItem[0].Split('~');
                        if (Product == "")
                            Product = prodSeriesIds[0];
                        else
                            Product = Product + "," + prodSeriesIds[0];
                    }
                }
                else
                    Product = "Error:Product";
            }
            //Date
            DateFrom = Convert.ToDateTime(DtFrom.Value).ToString("yyyy-MM-dd");
            DateTo = Convert.ToDateTime(DtTo.Value).ToString("yyyy-MM-dd");
            //GroupBy ,GroupByID and GroupByType
            if (CmbGroupBy.SelectedIndex == 0)
            {
                GroupBy = "BRANCH";
                if (RblBranch.SelectedIndex == 0) GroupByID = "ALL";
                else
                {
                    GroupByID = string.Empty;
                    if (HDNBranch.Value.Trim() != "")
                    {
                        string[] branchItems = (HDNBranch.Value.Trim()).Split('^');
                        for (int i = 0; i < branchItems.Length; i++)
                        {
                            string[] branchItem = branchItems[i].Split('!');
                            string[] branchIds = branchItem[0].Split('~');
                            if (GroupByID == "")
                                GroupByID = branchIds[0];
                            else
                                GroupByID = GroupByID + "," + branchIds[0];
                        }
                    }
                    else
                        GroupByID = "Error:Branch";
                }
                GroupByType = "";
            }
            else if (CmbGroupBy.SelectedIndex == 1)
            {
                GroupBy = "GROUP";
                if (RblGroup.SelectedIndex == 0) GroupByID = "ALL";
                else
                {
                    GroupByID = string.Empty;
                    if (HDNGroup.Value.Trim() != "")
                    {
                        string[] grpItems = (HDNGroup.Value.Trim()).Split('^');
                        for (int i = 0; i < grpItems.Length; i++)
                        {
                            string[] grpItem = grpItems[i].Split('!');
                            string[] grpIds = grpItem[0].Split('~');
                            if (GroupByID == "")
                                GroupByID = grpIds[0];
                            else
                                GroupByID = GroupByID + "," + grpIds[0];
                        }
                    }
                    else
                        GroupByID = "Error:Group";
                }
                if (ddlGrouptype.SelectedIndex != 0) GroupByType = ddlGrouptype.SelectedValue.Trim();
            }
            else
            {
                GroupBy = "CLIENT";
                if (RblClient.SelectedIndex == 0) GroupByID = "ALL";
                else
                {
                    GroupByID = string.Empty;
                    if (HDNClient.Value.Trim() != "")
                    {
                        string[] clntItems = (HDNClient.Value.Trim()).Split('^');
                        for (int i = 0; i < clntItems.Length; i++)
                        {
                            string[] clntItem = clntItems[i].Split('!');
                            string[] clntIds = clntItem[0].Split('~');
                            if (GroupByID == "")
                                GroupByID = clntIds[4];
                            else
                                GroupByID = GroupByID + "," + clntIds[4];
                        }
                    }
                    else
                        GroupByID = "Error:Client";
                }
                GroupByType = "";
            }
            //FilterParam
            FilterParam = FilterColumnCheck();
            HDNFilterCol.Value = FilterParam;
            //ReportType
            ReportType = RblReportType.SelectedItem.Value.ToString();
            HDNReportType.Value = ReportType;
            //ReportBy
            ReportBy = CmbReportBy.SelectedItem.Value.ToString();
            //PageSize
            PageSize = 15;
        }

        string PageValidation()
        {
            string strError = String.Empty;
            if (Segment != "")
                if (Segment.Split(':')[0] == "Error")
                    strError = "There is No Proper Segment Selection!!!";
            if (Product != "" && strError != String.Empty)
                if (Product.Split(':')[0] == "Error")
                    strError = "There is No Proper Product Selection!!!";
            if (GroupBy == "BRANCH")
            {
                if (GroupByID != "" && strError != String.Empty)
                    if (GroupByID.Split(':')[0] == "Error")
                        strError = "There is No Proper Branch Selection!!!";
            }
            else if (GroupBy == "GROUP")
            {
                if (GroupByID != "" && strError != String.Empty)
                    if (GroupByID.Split(':')[0] == "Error")
                        strError = "There is No Proper Group Selection!!!";
            }
            else
            {
                if (GroupByID != "" && strError != String.Empty)
                    if (GroupByID.Split(':')[0] == "Error")
                        strError = "There is No Proper Client Selection!!!";
            }
            return strError;
        }

        protected string ConvertToShortString(string input)
        {
            if (input.Length > 25)
            {
                input = input.Substring(0, 25);
            }
            if ((input.Contains("*Branch")) || (input.Contains("*Group")) || (input.Contains("*Client")) || (input.Contains("*Total (Branch")) || (input.Contains("*Total (Group")) || (input.Contains("*Total (Client")) || (input.Contains("*Grand")))
                input = "<b>" + input + "</b>";

            return input;
        }

        protected string ConvertToNegetive(string input)
        {
            if (input.Contains("-"))
            {
                input = input.Replace("-", "");
                input = "<span style=\"color:red\">(" + input + ")</span>";
            }
            return input;
        }
        #endregion

        # region Bind Group Type
        public void BindGroup()
        {
            ddlGrouptype.Items.Clear();
            // oDBEngine = new DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
            DataTable DtGroup = oDBEngine.GetDataTable("tbl_master_groupmaster", "distinct gpm_Type", null);
            if (DtGroup.Rows.Count > 0)
            {
                ddlGrouptype.DataSource = DtGroup;
                ddlGrouptype.DataTextField = "gpm_Type";
                ddlGrouptype.DataValueField = "gpm_Type";
                ddlGrouptype.DataBind();
                ddlGrouptype.Items.Insert(0, new ListItem("Select GroupType", "0"));
                DtGroup.Dispose();
            }
            else
            {
                ddlGrouptype.Items.Insert(0, new ListItem("Select GroupType", "0"));
            }
        }
        protected void btnhide_Click(object sender, EventArgs e)
        {
            if (CmbGroupBy.SelectedItem.Value.ToString() == "G")
            {
                if (HDNGroup.Value.ToString().Trim() == "")
                {
                    BindGroup();
                }
            }
        }
        # endregion

        #region BusinessLogic
        protected void Page_PreInit(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string[] PageSession = null;
                oGenericMethod = new GenericMethod();
                oGenericMethod.PageInitializer(GenericMethod.WhichCall.DistroyUnWantedSession_AllExceptPage, PageSession);
                oGenericMethod.PageInitializer(GenericMethod.WhichCall.DistroyUnWantedSession_Page, PageSession);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (HttpContext.Current.Session["userid"] == null)
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "SighOff", "<script>SignOff();</script>");
            }
            Page.ClientScript.RegisterStartupScript(GetType(), "PageLoad", "<script>Page_Load();</script>");
            if (!IsPostBack)
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "Reset", "<script>Reset();</script>");
                // oDBEngine = new DBEngine(null);
                DateTime Date = oDBEngine.GetDate();
                currentFromDate = Date.AddDays((-1 * Date.Day) + 1);
                currentToDate = Date;
                DtFrom.Value = currentFromDate;
                DtTo.Value = currentToDate;
                FillFilterColumn();
                AllSegment();
            }

            //_____For performing operation without refreshing page___//
            String cbReference = Page.ClientScript.GetCallbackEventReference(this, "arg", "ReceiveServerData", "context");
            String callbackScript = "function CallServer(arg, context){ " + cbReference + ";}";
            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "CallServer", callbackScript, true);
            //this.Page.ClientScript.RegisterStartupScript(GetType(), "heightL", "<script>height();</script>");
            //___________-end here___//      
        }

        private static DataSet GetData()
        {
            string[] InputName = new string[13];
            string[] InputType = new string[13];
            string[] InputValue = new string[13];

            InputName[0] = "CompanyID";
            InputName[1] = "DateFrom";
            InputName[2] = "DateTo";
            InputName[3] = "SegmentID";
            InputName[4] = "ProductID";
            InputName[5] = "GrpBy";
            InputName[6] = "GrpByID";
            InputName[7] = "GrpType";
            InputName[8] = "FilterParam";
            InputName[9] = "ReportType";
            InputName[10] = "ReportBy";
            InputName[11] = "PageSize";
            InputName[12] = "PageNumber";


            InputType[0] = "V";
            InputType[1] = "V";
            InputType[2] = "V";
            InputType[3] = "V";
            InputType[4] = "V";
            InputType[5] = "V";
            InputType[6] = "V";
            InputType[7] = "V";
            InputType[8] = "V";
            InputType[9] = "V";
            InputType[10] = "V";
            InputType[11] = "V";
            InputType[12] = "V";

            InputValue[0] = Company;
            InputValue[1] = DateFrom;
            InputValue[2] = DateTo;
            InputValue[3] = Segment;
            InputValue[4] = Product;
            InputValue[5] = GroupBy;
            InputValue[6] = GroupByID;
            InputValue[7] = GroupByType;
            InputValue[8] = "Y~Y~Y~Y~Y~Y~Y~Y~Y~Y~Y~Y~Y~Y~Y~Y~Y~Y~Y~Y~Y~Y~Y~Y~Y~Y~Y~Y~Y~Y~Y~Y~Y~Y";//FilterParam;
            InputValue[9] = ReportType;
            InputValue[10] = ReportBy;
            InputValue[11] = PageSize.ToString();
            InputValue[12] = PageNumber.ToString();

            return SQLProcedures.SelectProcedureArrDS("NetPosition_CMSegments", InputName, InputType, InputValue);
        }

        public static DataSet GetCMNetPositionData()
        {
            return GetData();
        }

        [WebMethod]
        public static string GetCMNetPositions(int pageNumber)
        {
            PageNumber = pageNumber;
            //GroupByID = branch;

            return GetCMNetPositionData().GetXml();
        }

        protected void BtnShow_Click(object sender, EventArgs e)
        {
            SetPropertiesValue();
            //dislayReportType = ReportType;
            PageNumber = 1;

            string strPageValidationMsg = PageValidation();
            if (strPageValidationMsg == String.Empty)
            {
                DataSet ds = new DataSet();
                ds = GetCMNetPositionData();
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 1)
                    {
                        if (ReportBy == "E")
                        {
                            ExportToExcel(ds);
                        }
                        else if (ReportBy == "S")
                        {
                            gvwNetPosition.DataSource = ds.Tables[0];
                            if (ReportType == "C")
                            {
                                gvwNetPosition.Columns.RemoveAt(GetGridVwColIndex(gvwNetPosition, "Date"));
                                gvwNetPosition.Columns.RemoveAt(GetGridVwColIndex(gvwNetPosition, "SettNum"));
                            }
                            gvwNetPosition.DataBind();


                            //Showing Total Calculation 
                            DataTable dtTotal = new DataTable();
                            dtTotal = ds.Tables[2];
                            if (dtTotal.Rows.Count > 0)
                            {
                                GetTotalRecords(dtTotal);
                            }
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "SearchFilter_Hide1", "fn_SearchFilter_Hide();", true);
                        }
                    }
                    else
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "NORECORD1", "NORECORD();", true);
                }
                else
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "NORECORD12", "NORECORD();", true);

            }
        }

        protected void GetTotalRecords(DataTable dt)
        {
            string[] filterTotCol = FilterParam.Split('~');

            TotPur = dt.Rows[0]["Pur"].ToString();
            if (filterTotCol[1].ToString() == "N")
            {
                totHeaderPur.Visible = false;
                footerTotPur.Visible = false;
            }
            TotPurMkt = dt.Rows[0]["PurMkt"].ToString();
            if (filterTotCol[2].ToString() == "N")
            {
                totHeaderPurMkt.Visible = false;
                footerTotPurMkt.Visible = false;
            }
            TotPurAvg = dt.Rows[0]["PurAvg"].ToString();
            if (filterTotCol[3].ToString() == "N")
            {
                totHeaderPurAvg.Visible = false;
                footerTotPurAvg.Visible = false;
            }
            TotPurNet = dt.Rows[0]["PurNet"].ToString();
            if (filterTotCol[4].ToString() == "N")
            {
                totHeaderPurNet.Visible = false;
                footerTotPurNet.Visible = false;
            }
            TotPurNetAvg = dt.Rows[0]["PurNetAvg"].ToString();
            if (filterTotCol[5].ToString() == "N")
            {
                totHeaderPurNetAvg.Visible = false;
                footerTotPurNetAvg.Visible = false;
            }
            TotSale = dt.Rows[0]["Sale"].ToString();
            if (filterTotCol[6].ToString() == "N")
            {
                totHeaderSale.Visible = false;
                footerTotSale.Visible = false;
            }
            TotSaleMkt = dt.Rows[0]["SaleMkt"].ToString();
            if (filterTotCol[7].ToString() == "N")
            {
                totHeaderSaleMkt.Visible = false;
                footerTotSaleMkt.Visible = false;
            }
            TotSaleAvg = dt.Rows[0]["SaleAvg"].ToString();
            if (filterTotCol[8].ToString() == "N")
            {
                totHeaderSaleAvg.Visible = false;
                footerTotSaleAvg.Visible = false;
            }
            TotSaleNet = dt.Rows[0]["SaleNet"].ToString();
            if (filterTotCol[9].ToString() == "N")
            {
                totHeaderSaleNet.Visible = false;
                footerTotSaleNet.Visible = false;
            }
            TotSaleNetAvg = dt.Rows[0]["SaleNetAvg"].ToString();
            if (filterTotCol[10].ToString() == "N")
            {
                totHeaderSaleNetAvg.Visible = false;
                footerTotSaleNetAvg.Visible = false;
            }
            TotSqrQty = dt.Rows[0]["SqrQty"].ToString();
            if (filterTotCol[11].ToString() == "N")
            {
                totHeaderSqrQty.Visible = false;
                footerTotSqrQty.Visible = false;
            }
            TotSqrPL = dt.Rows[0]["SqrPL"].ToString();
            if (filterTotCol[12].ToString() == "N")
            {
                totHeaderSqrPL.Visible = false;
                footerTotSqrPL.Visible = false;
            }
            TotDlvPur = dt.Rows[0]["DlvPur"].ToString();
            if (filterTotCol[13].ToString() == "N")
            {
                totHeaderDlvPur.Visible = false;
                footerTotDlvPur.Visible = false;
            }
            TotDlvPurMkt = dt.Rows[0]["DlvPurMkt"].ToString();
            if (filterTotCol[14].ToString() == "N")
            {
                totHeaderDlvPurMkt.Visible = false;
                footerTotDlvPurMkt.Visible = false;
            }
            TotDlvPurAvg = dt.Rows[0]["DlvPurAvg"].ToString();
            if (filterTotCol[15].ToString() == "N")
            {
                totHeaderDlvPurAvg.Visible = false;
                footerTotDlvPurAvg.Visible = false;
            }
            TotDlvPurNet = dt.Rows[0]["DlvPurNet"].ToString();
            if (filterTotCol[16].ToString() == "N")
            {
                totHeaderDlvPurNet.Visible = false;
                footerTotDlvPurNet.Visible = false;
            }
            TotDlvPurNetAvg = dt.Rows[0]["DlvPurNetAvg"].ToString();
            if (filterTotCol[17].ToString() == "N")
            {
                totHeaderDlvPurNetAvg.Visible = false;
                footerTotDlvPurNetAvg.Visible = false;
            }
            TotDlvSale = dt.Rows[0]["DlvSale"].ToString();
            if (filterTotCol[18].ToString() == "N")
            {
                totHeaderDlvSale.Visible = false;
                footerTotDlvSale.Visible = false;
            }
            TotDlvSaleMkt = dt.Rows[0]["DlvSaleMkt"].ToString();
            if (filterTotCol[19].ToString() == "N")
            {
                totHeaderDlvSaleMkt.Visible = false;
                footerTotDlvSaleMkt.Visible = false;
            }
            TotDlvSaleAvg = dt.Rows[0]["DlvSaleAvg"].ToString();
            if (filterTotCol[20].ToString() == "N")
            {
                totHeaderDlvSaleAvg.Visible = false;
                footerTotDlvSaleAvg.Visible = false;
            }
            TotDlvSaleNet = dt.Rows[0]["DlvSaleNet"].ToString();
            if (filterTotCol[21].ToString() == "N")
            {
                totHeaderDlvSaleNet.Visible = false;
                footerTotDlvSaleNet.Visible = false;
            }
            TotDlvSaleNetAvg = dt.Rows[0]["DlvSaleNetAvg"].ToString();
            if (filterTotCol[22].ToString() == "N")
            {
                totHeaderDlvSaleNetAvg.Visible = false;
                footerTotDlvSaleNetAvg.Visible = false;
            }
            TotNetDlv = dt.Rows[0]["NetDlv"].ToString();
            if (filterTotCol[23].ToString() == "N")
            {
                totHeaderNetDlv.Visible = false;
                footerTotNetDlv.Visible = false;
            }
            TotClPrice = dt.Rows[0]["ClPrice"].ToString();
            if (filterTotCol[24].ToString() == "N")
            {
                totHeaderClPrice.Visible = false;
                footerTotClPrice.Visible = false;
            }
            TotMtmPL = dt.Rows[0]["MtmPL"].ToString();
            if (filterTotCol[25].ToString() == "N")
            {
                totHeaderMtmPL.Visible = false;
                footerTotMtmPL.Visible = false;
            }
            TotTotalPL = dt.Rows[0]["TotalPL"].ToString();
            if (filterTotCol[26].ToString() == "N")
            {
                totHeaderTotalPL.Visible = false;
                footerTotTotalPL.Visible = false;
            }
            TotExposure = dt.Rows[0]["Exposure"].ToString();
            if (filterTotCol[27].ToString() == "N")
            {
                totHeaderExposure.Visible = false;
                footerTotExposure.Visible = false;
            }
            TotDlvTurnover = dt.Rows[0]["DlvTurnover"].ToString();
            if (filterTotCol[28].ToString() == "N")
            {
                totHeaderDlvTurnover.Visible = false;
                footerTotDlvTurnover.Visible = false;
            }
            TotDlvBrokerage = dt.Rows[0]["DlvBrokerage"].ToString();
            if (filterTotCol[29].ToString() == "N")
            {
                totHeaderDlvBrokerage.Visible = false;
                footerTotDlvBrokerage.Visible = false;
            }
            TotSqrTurnover = dt.Rows[0]["SqrTurnover"].ToString();
            if (filterTotCol[30].ToString() == "N")
            {
                totHeaderSqrTurnover.Visible = false;
                footerTotSqrTurnover.Visible = false;
            }
            TotSqrBrokerage = dt.Rows[0]["SqrBrokerage"].ToString();
            if (filterTotCol[31].ToString() == "N")
            {
                totHeaderSqrBrokerage.Visible = false;
                footerTotSqrBrokerage.Visible = false;
            }
            TotTurnover = dt.Rows[0]["TurnOver"].ToString();
            if (filterTotCol[32].ToString() == "N")
            {
                totHeaderTurnover.Visible = false;
                footerTotTurnover.Visible = false;
            }
            TotTotalBrokerage = dt.Rows[0]["TotalBrokerage"].ToString();
            if (filterTotCol[33].ToString() == "N")
            {
                totHeaderTotalBrokerage.Visible = false;
                footerTotTotalBrokerage.Visible = false;
            }
        }

        public static int GetGridVwColIndex(GridView grd, string fieldName)
        {
            for (int i = 0; i < grd.Columns.Count; i++)
            {
                DataControlField field = grd.Columns[i];
                TemplateField tfield = field as TemplateField;
                //BoundField bfield = field as BoundField;

                if (tfield != null && tfield.HeaderText == fieldName)
                    return i;
                // For remove an HyperLink=======================
                //HyperLinkField hfield = field as HyperLinkField;
                //if (hfield != null && hfield.DataTextField == fieldName)
                //if (hfield != null && hfield.HeaderText == fieldName) return i;
            }
            return -1;
        }

        protected void gvwNetPosition_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header || e.Row.RowType == DataControlRowType.DataRow)
            {
                string[] filterColumn = FilterParam.Split('~');

                for (int i = 0; i <= 33; i++)
                {
                    if (filterColumn[i].ToString() == "N")
                    {
                        if (ReportType == "D") //AutoID,Client,Segment,[1]Date,[2]SettNum,Symbol,--Isin(F-0)
                            e.Row.Cells[i + 6].Visible = false;
                        else
                            e.Row.Cells[i + 4].Visible = false;
                    }
                }
            }

        }

        void ExportToExcel(DataSet DsExport)
        {
            string strSavePath = String.Empty;
            // oDBEngine = new DBEngine(null);
            string exlDateTime = oDBEngine.GetDate(113).ToString();
            string exlTime = exlDateTime.Replace(":", "");
            exlTime = exlTime.Replace(" ", "");
            string FileName = "NetPos_CMSegReport_" + exlTime;
            strSavePath = "~/Documents/";

            string strReportHeader = null;
            strReportHeader = "NetPosition CMSegments ";
            if (ReportType == "D") strReportHeader = strReportHeader + " Detail Report";
            else if (ReportType != "C") strReportHeader = strReportHeader + " Consolidated Report";

            string searchCriteria = null;
            searchCriteria = "Search By ";

            if (CmbGroupBy.SelectedItem.Value.ToString() == "B")
            {
                if (RblBranch.SelectedIndex == 0)
                    searchCriteria += "All";
                else
                    searchCriteria += "Selected";
                searchCriteria += " Branch";
            }
            else if (CmbGroupBy.SelectedItem.Value.ToString() == "G")
            {
                if (RblGroup.SelectedIndex == 0)
                    searchCriteria += "All";
                else if (RblGroup.SelectedIndex == 1)
                    searchCriteria += "Selected";
                searchCriteria += " Group Of" + ddlGrouptype.SelectedItem.Text.ToString().Trim() + " Group Type ";
            }
            else if (CmbGroupBy.SelectedItem.Value.ToString() == "C")
            {
                if (RblClient.SelectedIndex == 0)
                    searchCriteria += "All";
                else
                    searchCriteria += "Selected";
                searchCriteria += " Client";
            }

            if (RblSegment.SelectedIndex == 0)
                searchCriteria += "All Segment";
            else
                searchCriteria += "Selected Segment";

            if (RblProduct.SelectedIndex == 0)
                searchCriteria += "All Product";
            else
                searchCriteria += "Selected Product";


            string[] strHead = new string[3];
            strHead[0] = exlDateTime;
            strHead[1] = searchCriteria;
            strHead[2] = strReportHeader;

            oGenericExcelExport = new GenericExcelExport();
            // oDBEngine = new DBEngine(null);
            DataTable DtExport = new DataTable();

            if (DsExport.Tables[0].Rows.Count > 1)
            {
                DtExport = DsExport.Tables[0];
                DtExport.Columns.Remove("AutoID");
                DtExport.AcceptChanges();
                if (ReportType == "D")
                {
                    //Client,Segment,Date,SettNum,Symbol,Isin-> TotalBrokerage (5->38) Total(39 Without [AutoID])
                    string[] ColumnType = { "V", "V", "V", "V", "V", "V", "N", "N", "N", "N", "N", "N", "N", "N", "N", "N", "N", "N", "N", "N", "N", "N", "N", "N", "N", "N", "N", "N", "N", "N", "N", "N", "N", "N", "N", "N", "N", "N", "N" };
                    string[] ColumnSize = { "150", "10", "15", "10", "50", "12", "20,0", "28,2", "19,4", "28,2", "19,4", "20,0", "28,2", "19,4", "28,2", "19,4", "20,0", "28,2", "20,0", "28,2", "19,4", "28,2", "19,4", "20,0", "28,2", "19,4", "28,2", "19,4", "20,0", "24,4", "28,2", "28,2", "24,2", "28,2", "28,2", "28,2", "28,2", "28,2", "28,2" };
                    string[] ColumnWidthSize = { "30", "10", "15", "10", "20", "12", "20", "20", "19", "20", "19", "20", "20", "19", "20", "19", "20", "20", "20", "20", "19", "20", "19", "20", "20", "19", "20", "19", "20", "24", "20", "20", "24", "20", "20", "20", "20", "20", "20" };

                    ArrayList ColumnTypeList = new ArrayList(ColumnType);
                    ArrayList ColumnSizeList = new ArrayList(ColumnSize);
                    ArrayList ColumnWidthSizeList = new ArrayList(ColumnWidthSize);

                    int removeCounter = 0;
                    string[] filterDXLCol = FilterParam.Split('~');
                    for (int i = 0; i <= 33; i++)
                    {
                        if (filterDXLCol[i].ToString() == "N")
                        {
                            ColumnTypeList.RemoveAt(5 + i - removeCounter);
                            ColumnSizeList.RemoveAt(5 + i - removeCounter);
                            ColumnWidthSizeList.RemoveAt(5 + i - removeCounter);

                            DtExport.Columns.RemoveAt(5 + i - removeCounter);
                            DtExport.AcceptChanges();

                            removeCounter = removeCounter + 1;
                        }
                    }

                    ColumnType = (string[])ColumnTypeList.ToArray(typeof(string));
                    ColumnSize = (string[])ColumnSizeList.ToArray(typeof(string));
                    ColumnWidthSize = (string[])ColumnWidthSizeList.ToArray(typeof(string));

                    oGenericExcelExport.ExportToExcel(ColumnType, ColumnSize, ColumnWidthSize, DtExport, Server.MapPath(strSavePath), "2007", FileName, strHead, null);
                }
                else
                {
                    //Client,Segment,Symbol,Isin-> TotalBrokerage (3->36) Total(37 Without [AutoID])
                    string[] ColumnType = { "V", "V", "V", "V", "N", "N", "N", "N", "N", "N", "N", "N", "N", "N", "N", "N", "N", "N", "N", "N", "N", "N", "N", "N", "N", "N", "N", "N", "N", "N", "N", "N", "N", "N", "N", "N", "N" };
                    string[] ColumnSize = { "150", "10", "50", "12", "20,0", "28,2", "19,4", "28,2", "19,4", "20,0", "28,2", "19,4", "28,2", "19,4", "20,0", "28,2", "20,0", "28,2", "19,4", "28,2", "19,4", "20,0", "28,2", "19,4", "28,2", "19,4", "20,0", "24,4", "28,2", "28,2", "24,2", "28,2", "28,2", "28,2", "28,2", "28,2", "28,2" };
                    string[] ColumnWidthSize = { "30", "10", "20", "12", "20", "20", "19", "20", "19", "20", "20", "19", "20", "19", "20", "20", "20", "20", "19", "20", "19", "20", "20", "19", "20", "19", "20", "24", "20", "20", "24", "20", "20", "20", "20", "20", "20" };

                    ArrayList ColumnTypeList = new ArrayList(ColumnType);
                    ArrayList ColumnSizeList = new ArrayList(ColumnSize);
                    ArrayList ColumnWidthSizeList = new ArrayList(ColumnWidthSize);

                    int removeCounter = 0;
                    string[] filterCXLCol = FilterParam.Split('~');
                    for (int i = 0; i <= 33; i++)
                    {
                        if (filterCXLCol[i].ToString() == "N")
                        {
                            ColumnTypeList.RemoveAt(3 + i - removeCounter);
                            ColumnSizeList.RemoveAt(3 + i - removeCounter);
                            ColumnWidthSizeList.RemoveAt(3 + i - removeCounter);

                            DtExport.Columns.RemoveAt(3 + i - removeCounter);
                            DtExport.AcceptChanges();

                            removeCounter = removeCounter + 1;
                        }
                    }

                    ColumnType = (string[])ColumnTypeList.ToArray(typeof(string));
                    ColumnSize = (string[])ColumnSizeList.ToArray(typeof(string));
                    ColumnWidthSize = (string[])ColumnWidthSizeList.ToArray(typeof(string));

                    oGenericExcelExport.ExportToExcel(ColumnType, ColumnSize, ColumnWidthSize, DtExport, Server.MapPath(strSavePath), "2007", FileName, strHead, null);
                }
            }
        }
        #endregion

    }
}