using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DevExpress.Web;
using System.Data;
using System.Configuration;
using DataAccessLayer;

namespace ERP.OMS.Management.Activities.UserControls
{

    public partial class Import_ucPaymentDetails : System.Web.UI.UserControl
    {

        public string doc_type { get; set; }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
              //  createJsonForMainAccount();
              //  CreateBranchMapJson();
                if (cmbUcpaymentCashLedger.Items.Count == 0)
                {
                    LoadcmbUcpaymentCashLedger();
                }
            }
        }

        private void LoadcmbUcpaymentCashLedger()
        {
            DataTable ds = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_ucPayementDetails");
            proc.AddVarcharPara("@action", 20, "GetCashAccount");
            proc.AddVarcharPara("@BranchList", 1000, Convert.ToString(Session["userbranchHierarchy"]));
            ds = proc.GetTable();
            cmbUcpaymentCashLedger.DataSource = ds;
            cmbUcpaymentCashLedger.ValueField = "MainAccount_AccountCode";
            cmbUcpaymentCashLedger.TextField = "MainAccount_Name";
            cmbUcpaymentCashLedger.DataBind();
        }

        public void createJsonForMainAccount()
        {

            DataTable ds = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_ucPayementDetails");
            proc.AddVarcharPara("@action", 20, "GetMainAccount");
            proc.AddVarcharPara("@BranchList", 1000, Convert.ToString(Session["userbranchHierarchy"]));
            ds = proc.GetTable();

            List<JsonFormMainAccount> mainAccountList = new List<JsonFormMainAccount>();

            JsonFormMainAccount mainAccount = new JsonFormMainAccount();

            foreach (DataRow dr in ds.Rows)
            {
                mainAccount = new JsonFormMainAccount();
                mainAccount.MainAccount_AccountCode = Convert.ToString(dr["MainAccount_AccountCode"]);
                mainAccount.MainAccount_Name = Convert.ToString(dr["MainAccount_Name"]);
                mainAccount.MainAccount_BankCashType = Convert.ToString(dr["MainAccount_BankCashType"]);
                mainAccount.BranchId = Convert.ToString(dr["MainAccount_branchId"]);
                mainAccountList.Add(mainAccount);
            }


            var oSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            hdJsonMainAccountString.Value = oSerializer.Serialize(mainAccountList);

        }


        public void CreateBranchMapJson() 
        {
            DataTable ds = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_ucPayementDetails");
            proc.AddVarcharPara("@action", 20, "BranchMapJson"); 
            ds = proc.GetTable();

            List<JsonForBranchMainAct> BranchMainAccount = new List<JsonForBranchMainAct>();
            JsonForBranchMainAct branchMainAct = new JsonForBranchMainAct();

            foreach (DataRow dr in ds.Rows)
            {
                branchMainAct = new JsonForBranchMainAct();
                branchMainAct.BranchId = Convert.ToString(dr["branch_id"]);
                branchMainAct.MainAccount_Code = Convert.ToString(dr["MainAccount_AccountCode"]);                
                BranchMainAccount.Add(branchMainAct);
            }

            var oSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            HdJsonBranchMainAct.Value = oSerializer.Serialize(BranchMainAccount);
             

        }

        public DataTable GetPaymentTable()
        {

            int hdCount = ClientSaveData.Count();
            DataTable paymentTable = CreateDataTable();
            string data = "";
            for (int i = 0; i < hdCount; i++)
            {
                data = Convert.ToString(ClientSaveData.Get(Convert.ToString(i)));
                string[] splitdata = data.Split(new string[] { "|~|" }, StringSplitOptions.None);

                if (splitdata.Length > 0)
                {
                    paymentTable = addDataToTable(paymentTable, splitdata);
                }


            }

            return paymentTable;

        }

        public DataTable addDataToTable(DataTable paymentTable, string[] splitdata)
        {
            DataRow newRow = paymentTable.NewRow();
            newRow["Payment_type"] = splitdata[0];
            newRow["Doc_type"] = this.doc_type;


            if (splitdata[0] == "Card")
            {
                newRow["PaymentType_details"] = splitdata[1];
                newRow["cardType"] = splitdata[2];
                newRow["AuthNo"] = splitdata[3];
                newRow["payment_remarks"] = splitdata[4];
                newRow["Payment_mainAccount"] = splitdata[5];
                newRow["paymentAmount"] = splitdata[6];
            }
            else if (splitdata[0] == "Cheque")
            {
                newRow["PaymentType_details"] = splitdata[1];

                string dtPay = splitdata[2];
                if (dtPay != "")
                {
                    newRow["payment_date"] = dtPay.Split('-')[2] + "/" + dtPay.Split('-')[1] + "/" + dtPay.Split('-')[0];
                }
                else
                {
                    newRow["payment_date"] = splitdata[2];
                }

                string dtDrawee = splitdata[3];
                if (dtDrawee != "")
                {
                    newRow["Drawee_date"] = dtDrawee.Split('-')[2] + "/" + dtDrawee.Split('-')[1] + "/" + dtDrawee.Split('-')[0];
                }
                else
                {
                    newRow["Drawee_date"] = splitdata[3];
                }

                newRow["payment_remarks"] = splitdata[4];
                newRow["Payment_mainAccount"] = splitdata[5];
                newRow["paymentAmount"] = splitdata[6];
            }
            else if (splitdata[0] == "Coupon")
            {
                newRow["PaymentType_details"] = splitdata[1];
                newRow["Payment_mainAccount"] = splitdata[2];
                newRow["paymentAmount"] = splitdata[3];
            }
            else if (splitdata[0] == "E Transfer")
            {
                newRow["PaymentType_details"] = splitdata[1];
                //newRow["payment_date"] = splitdata[2];
                string dtPay = splitdata[2];
                if (dtPay != "")
                {
                    newRow["payment_date"] = dtPay.Split('-')[2] + "/" + dtPay.Split('-')[1] + "/" + dtPay.Split('-')[0];
                }
                else
                {
                    newRow["payment_date"] = splitdata[2];
                }
                newRow["Payment_mainAccount"] = splitdata[3];
                newRow["paymentAmount"] = splitdata[4];
            }
            else if (splitdata[0] == "Cash")
            {
                newRow["Payment_mainAccount"] = splitdata[1];
                newRow["paymentAmount"] = splitdata[2];
            }
            paymentTable.Rows.Add(newRow);

            return paymentTable;

        }
        public DataTable CreateDataTable()
        {
            DataTable paymentDetails = new DataTable();
            paymentDetails.Columns.Add("Doc_type", typeof(System.String));
            paymentDetails.Columns.Add("Payment_type", typeof(System.String));
            paymentDetails.Columns.Add("PaymentType_details", typeof(System.String));
            paymentDetails.Columns.Add("cardType", typeof(System.String));
            paymentDetails.Columns.Add("AuthNo", typeof(System.String));
            paymentDetails.Columns.Add("payment_remarks", typeof(System.String));
            paymentDetails.Columns.Add("paymentAmount", typeof(System.String));
            paymentDetails.Columns.Add("payment_date", typeof(System.String));
            paymentDetails.Columns.Add("Drawee_date", typeof(System.String));
            paymentDetails.Columns.Add("Payment_mainAccount", typeof(System.String));

            return paymentDetails;
        }


        public void StorePaymentDetailsToHiddenfield(int id)
        {
            if (!IsPostBack)
            {
                ClientSaveData.Clear();
                DataTable ds = new DataTable();
                ProcedureExecute proc = new ProcedureExecute("prc_ucPayementDetails");
                proc.AddVarcharPara("@Action", 500, "GetPaymentDetails");
                proc.AddIntegerPara("@doc_id", id);
                proc.AddVarcharPara("@doc_type", 20, this.doc_type);
                ds = proc.GetTable();

                string data = "";
                int count = 0;
                foreach (DataRow dr in ds.Rows)
                {
                    data = "";
                    data += Convert.ToString(dr["Payment_type"]);

                    if (Convert.ToString(dr["Payment_type"]) == "Card")
                    {
                        data += "|~|" + Convert.ToString(dr["PaymentType_details"]);
                        data += "|~|" + Convert.ToString(dr["CardType"]);
                        data += "|~|" + Convert.ToString(dr["AuthNo"]);
                        data += "|~|" + Convert.ToString(dr["Payment_Remarks"]);
                        data += "|~|" + Convert.ToString(dr["Payment_mainAccount"]);
                        data += "|~|" + Convert.ToString(dr["PaymentAmount"]);

                    }
                    else if (Convert.ToString(dr["Payment_type"]) == "Cash")
                    {
                        data += "|~|" + Convert.ToString(dr["Payment_mainAccount"]);
                        data += "|~|" + Convert.ToString(dr["PaymentAmount"]);
                        LoadcmbUcpaymentCashLedger();
                        cmbUcpaymentCashLedger.Value = Convert.ToString(dr["Payment_mainAccount"]);
                    }
                    else if (Convert.ToString(dr["Payment_type"]) == "Cheque")
                    {
                        data += "|~|" + Convert.ToString(dr["PaymentType_details"]);
                        string Payment_date = Convert.ToString(dr["Payment_date"]);
                        string[] dateSplit = Payment_date.Split('-');
                        if (dateSplit.Length > 0)
                        {
                            data += "|~|" + dateSplit[2].Trim()+ "-" + dateSplit[1] + "-" + dateSplit[0];
                        }

                        string Drawee_date = Convert.ToString(dr["Drawee_date"]);
                        dateSplit = Drawee_date.Split('-');
                        if (dateSplit.Length > 0)
                        {
                            data += "|~|" + dateSplit[2].Trim() + "-" + dateSplit[1] + "-" + dateSplit[0];
                        }
                        data += "|~|" + Convert.ToString(dr["Payment_Remarks"]);
                        data += "|~|" + Convert.ToString(dr["Payment_mainAccount"]);
                        data += "|~|" + Convert.ToString(dr["PaymentAmount"]);
                    }
                    else if (Convert.ToString(dr["Payment_type"]) == "Coupon")
                    {
                        data += "|~|" + Convert.ToString(dr["PaymentType_details"]);
                        data += "|~|" + Convert.ToString(dr["Payment_mainAccount"]);
                        data += "|~|" + Convert.ToString(dr["PaymentAmount"]);
                    }
                    else if (Convert.ToString(dr["Payment_type"]) == "E Transfer")
                    {
                        data += "|~|" + Convert.ToString(dr["PaymentType_details"]);
                        string Payment_date = Convert.ToString(dr["Payment_date"]);
                        string[] dateSplit = Payment_date.Split('-');
                        if (dateSplit.Length > 0)
                        {
                            data += "|~|" + dateSplit[2].Trim() + "-" + dateSplit[1] + "-" + dateSplit[0];
                        }
                        data += "|~|" + Convert.ToString(dr["Payment_mainAccount"]);
                        data += "|~|" + Convert.ToString(dr["PaymentAmount"]);
                    }
                    ClientSaveData.Add(Convert.ToString(count), data);
                    count++;
                }

            }
        }

        public void Setbranchvalue(string branchId) {

            HdSelectedBranch.Value = branchId;
        }
    }

    public class JsonFormMainAccount
    {
        public string MainAccount_AccountCode { get; set; }
        public string MainAccount_Name { get; set; }
        public string MainAccount_BankCashType { get; set; }

        public string BranchId { get; set; }

    }
    public class JsonForBranchMainAct
    {
        public string  BranchId { get; set; }
        public string MainAccount_Code { get; set; }

    }
}