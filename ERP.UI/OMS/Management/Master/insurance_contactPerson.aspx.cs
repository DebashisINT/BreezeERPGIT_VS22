using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using BusinessLogicLayer;
using System.IO;
using DevExpress.Web;
using EntityLayer.CommonELS;
namespace ERP.OMS.Management.Master
{
    public partial class management_master_insurance_contactPerson : ERP.OMS.ViewState_class.VSPage
    {
        DataTable dt = new DataTable();
        // DBEngine oDBEngine = new DBEngine(string.Empty);
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(string.Empty);
        public EntityLayer.CommonELS.UserRightsForPage rights = new UserRightsForPage();
        protected void Page_Init(Object sender, EventArgs e)
        {
            SqlDesignation.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            SqlFamRelationShip.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            fillgrid();
        }
        protected void Page_Load(object sender, EventArgs e)
        {

            if (Session["requesttype"] != null)
            {
                //if (Session["requesttype"] == "Lead")
                //{
                //    this.Title = "Lead";
                //}
                //else if (Session["requesttype"] == "Customer/Client")
                //{
                //    this.Title = "Customer/Client";
                //}
                //else if (Session["requesttype"] == "Relationship Partners")
                //{
                //    this.Title = "Relationship Partners";
                //}
                //else if (Session["requesttype"] == "Franchisee")
                //{
                //    this.Title = "Franchisee";
                //}
                //else if (Session["requesttype"] == "Partner")
                //{
                //    this.Title = "Partner";
                //}
                //else if (Session["requesttype"] == "Consultant")
                //{
                //    this.Title = "Consultant";
                //}
                //else if (Session["requesttype"] == "Share Holder")
                //{
                //    this.Title = "Share Holder";
                //}
                //else if (Session["requesttype"] == "Debtor")
                //{
                //    this.Title = "Debtor";
                //}
                //else if (Session["requesttype"] == "Creditors")
                //{
                //    this.Title = "Creditors";
                //}
                //else if (Session["requesttype"] == "Salesman/Agents")
                //{
                //    this.Title = "Salesman/Agents";
                //}
                //else if (Session["requesttype"] == "OtherEntity")
                //{
                //    this.Title = "Customer/Client";
                //}

                //if (Request.QueryString["Id"] != null)
                //{
                //    lblHeadTitle.Text = "Add / Edit" + Session["requesttype"].ToString();
                //}
                //else
                //{
                //Code Commented and Added by Sanjib on 09122016 to change the header name. ................
                //lblHeadTitle.Text = Session["requesttype"].ToString() + " Details";

                // .............................Code Commented and Added by Sanjib on 09122016 to use Convert.tostring instead of tostring(). ................
                lblHeadTitle.Text = Convert.ToString(Session["requesttype"]);
                // .............................Code Above Commented and Added by Sanjib on 09122016 to use Convert.tostring instead of tostring(). ..................................... 
                // }
            }
            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/Management/Master/insurance_contactPerson.aspx");


            if (!IsPostBack)
            {
               
                Session["exportval"] = null;
                if (Request.QueryString["id"] != null)
                {
                    string ID = Request.QueryString["id"];
                    Session["KeyVal_InternalID"] = ID;
                }
                string previousPageUrl = string.Empty;
                if (Request.UrlReferrer != null)
                    previousPageUrl = Request.UrlReferrer.AbsoluteUri;
                else
                    previousPageUrl = Page.ResolveUrl("~/OMS/Management/ProjectMainPage.aspx");

                ViewState["previousPageUrl"] = previousPageUrl;
                goBackCrossBtn.NavigateUrl = previousPageUrl;
            }
            //fillgrid(); //Add comment by sanjib due to filter does not wokring 19122016.

            //SqlContactPerson.SelectCommand = "select A.cp_contactId as ContactId, A.cp_name as name,(select phf_phonenumber from tbl_master_phonefax where phf_cntId=A.cp_contactId and phf_type='Office') as Officephone,(select phf_phonenumber from tbl_master_phonefax where phf_cntId=A.cp_contactId and phf_type='Residence') as Residencephone,(select phf_phonenumber from tbl_master_phonefax where phf_cntId=A.cp_contactId and phf_type='Mobile') as Mobilephone,isnull(('(O)'+(select phf_phonenumber from tbl_master_phonefax where phf_cntId=A.cp_contactId and phf_type='Office')),'')+isnull(('(R)'+(select phf_phonenumber from tbl_master_phonefax where phf_cntId=A.cp_contactId and phf_type='Residence')),'')+isnull(('(M)'+(select phf_phonenumber from tbl_master_phonefax where phf_cntId=A.cp_contactId and phf_type='Mobile')),'') as Phone,(select eml_email from tbl_master_email where eml_cntId=A.cp_contactId) as email, (case when cp_status = 'Y' then 'Active' when cp_status = 'N' then 'Suspended' else 'N/A'  end) as status, ltrim(rtrim(cp_status)) as cp_status,ltrim(rtrim(cp_designation)) as cp_designation,ltrim(rtrim(cp_relationShip)) as cp_relationShip,cp_Pan from tbl_master_contactperson A  where cp_agentInternalId='" + Session["KeyVal_InternalID"].ToString() + "' ORDER BY cp_status desc";
        }
        void fillgrid()
        {
            // .............................Code Commented and Added by Sanjib on 09122016 to use Convert.tostring instead of tostring(). ................
            //dt = oDBEngine.GetDataTable("select  A.cp_contactId as ContactId, A.cp_name as name,(select  top 1  phf_phonenumber from tbl_master_phonefax where phf_cntId=A.cp_contactId and phf_type='Office') as Officephone,(select  top 1  phf_phonenumber from tbl_master_phonefax where phf_cntId=A.cp_contactId and phf_type='Residence') as Residencephone,(select  top 1  phf_phonenumber from tbl_master_phonefax where phf_cntId=A.cp_contactId and phf_type='Mobile') as Mobilephone,isnull(('(O)'+(select   top 1 phf_phonenumber from tbl_master_phonefax where phf_cntId=A.cp_contactId and phf_type='Office')),'')+isnull(('(R)'+(select  top 1  phf_phonenumber from tbl_master_phonefax where phf_cntId=A.cp_contactId and phf_type='Residence')),'')+isnull(('(M)'+(select  top 1  phf_phonenumber from tbl_master_phonefax where phf_cntId=A.cp_contactId and phf_type='Mobile')),'') as Phone,(select  top 1  eml_email from tbl_master_email where eml_cntId=A.cp_contactId) as email, (case when cp_status = 'Y' then 'Active' when cp_status = 'N' then 'Suspended' else 'N/A'  end) as status, ltrim(rtrim(cp_status)) as cp_status,(select top 1 deg_designation from tbl_master_designation where deg_id= ltrim(rtrim(  cp_designation))) as  cp_designation,(select top 1 fam_familyRelationship from tbl_master_familyrelationship where fam_familyRelationship=ltrim(rtrim( cp_relationShip))) as cp_relationShip,cp_Pan,cp_Din from tbl_master_contactperson A  where cp_agentInternalId='" + Session["KeyVal_InternalID"].ToString() + "' ORDER BY cp_status desc");
            
            //--Sanjib for designation and relationship memeber was throwing error to fix this change query12122016.
            //dt = oDBEngine.GetDataTable("select  A.cp_contactId as ContactId, A.cp_name as name,(select  top 1  phf_phonenumber from tbl_master_phonefax where phf_cntId=A.cp_contactId and phf_type='Office') as Officephone,(select  top 1  phf_phonenumber from tbl_master_phonefax where phf_cntId=A.cp_contactId and phf_type='Residence') as Residencephone,(select  top 1  phf_phonenumber from tbl_master_phonefax where phf_cntId=A.cp_contactId and phf_type='Mobile') as Mobilephone,isnull(('(O)'+(select   top 1 phf_phonenumber from tbl_master_phonefax where phf_cntId=A.cp_contactId and phf_type='Office')),'')+isnull(('(R)'+(select  top 1  phf_phonenumber from tbl_master_phonefax where phf_cntId=A.cp_contactId and phf_type='Residence')),'')+isnull(('(M)'+(select  top 1  phf_phonenumber from tbl_master_phonefax where phf_cntId=A.cp_contactId and phf_type='Mobile')),'') as Phone,(select  top 1  eml_email from tbl_master_email where eml_cntId=A.cp_contactId) as email, (case when cp_status = 'Y' then 'Active' when cp_status = 'N' then 'Suspended' else 'N/A'  end) as status, ltrim(rtrim(cp_status)) as cp_status,(select top 1 deg_designation from tbl_master_designation where deg_id= ltrim(rtrim(  cp_designation))) as  cp_designation,(select top 1 fam_familyRelationship from tbl_master_familyrelationship where fam_id=cast(ltrim(rtrim( cp_relationShip)) as int)) as cp_relationShip,cp_Pan,cp_Din from tbl_master_contactperson A  where cp_agentInternalId='" + Convert.ToString(Session["KeyVal_InternalID"]) + "' ORDER BY cp_status desc");

            dt = oDBEngine.GetDataTable("select  A.cp_contactId as ContactId, A.cp_name as name,(select  top 1  phf_phonenumber from tbl_master_phonefax where phf_cntId=A.cp_contactId and phf_type='Office') as Officephone,(select  top 1  phf_phonenumber from tbl_master_phonefax where phf_cntId=A.cp_contactId and phf_type='Residence') as Residencephone,(select  top 1  phf_phonenumber from tbl_master_phonefax where phf_cntId=A.cp_contactId and phf_type='Mobile') as Mobilephone,isnull(('(O)'+(select   top 1 phf_phonenumber from tbl_master_phonefax where phf_cntId=A.cp_contactId and phf_type='Office')),'')+isnull(('(R)'+(select  top 1  phf_phonenumber from tbl_master_phonefax where phf_cntId=A.cp_contactId and phf_type='Residence')),'')+isnull(('(M)'+(select  top 1  phf_phonenumber from tbl_master_phonefax where phf_cntId=A.cp_contactId and phf_type='Mobile')),'') as Phone,(select  top 1  eml_email from tbl_master_email where eml_cntId=A.cp_contactId) as email, (case when cp_status = 'Y' then 'Active' when cp_status = 'N' then 'Suspended' else 'N/A'  end) as status, ltrim(rtrim(cp_status)) as cp_status,(select top 1 deg_id from tbl_master_designation where deg_id= ltrim(rtrim(  cp_designation))) as  cp_designation_id,(select top 1 deg_designation from tbl_master_designation where deg_id= ltrim(rtrim(  cp_designation))) as  cp_designation,(select top 1 fam_familyRelationship from tbl_master_familyrelationship where fam_id=cast(ltrim(rtrim( cp_relationShip)) as int)) as cp_relationShip,(select top 1 fam_id from tbl_master_familyrelationship where fam_id=cast(ltrim(rtrim( cp_relationShip)) as int)) as cp_relationShip_id,ltrim(rtrim(cp_Pan)) as cp_Pan,cp_Din from tbl_master_contactperson A  where cp_agentInternalId='" + Convert.ToString(Session["KeyVal_InternalID"]) + "' ORDER BY cp_status desc");

            //end

            //   dt = oDBEngine.GetDataTable("select  A.cp_contactId as ContactId, A.cp_name as name,(select phf_phonenumber from tbl_master_phonefax where phf_cntId=A.cp_contactId and phf_type='Office') as Officephone,(select phf_phonenumber from tbl_master_phonefax where phf_cntId=A.cp_contactId and phf_type='Residence') as Residencephone,(select phf_phonenumber from tbl_master_phonefax where phf_cntId=A.cp_contactId and phf_type='Mobile') as Mobilephone,isnull(('(O)'+(select phf_phonenumber from tbl_master_phonefax where phf_cntId=A.cp_contactId and phf_type='Office')),'')+isnull(('(R)'+(select phf_phonenumber from tbl_master_phonefax where phf_cntId=A.cp_contactId and phf_type='Residence')),'')+isnull(('(M)'+(select phf_phonenumber from tbl_master_phonefax where phf_cntId=A.cp_contactId and phf_type='Mobile')),'') as Phone,(select eml_email from tbl_master_email where eml_cntId=A.cp_contactId) as email, (case when cp_status = 'Y' then 'Active' when cp_status = 'N' then 'Suspended' else 'N/A'  end) as status, ltrim(rtrim(cp_status)) as cp_status,(select deg_designation from tbl_master_designation where deg_id= ltrim(rtrim(  cp_designation))) as  cp_designation,(select fam_familyRelationship from tbl_master_familyrelationship where fam_id=ltrim(rtrim( cp_relationShip))) as cp_relationShip,cp_Pan,cp_Din from tbl_master_contactperson A  where cp_agentInternalId='" + Session["KeyVal_InternalID"].ToString() + "' ORDER BY cp_status desc");
            
            
            GridContactPerson.DataSource = dt.DefaultView;
            GridContactPerson.DataBind();
            // .............................Code Above Commented and Added by Sanjib on 09122016 to use Convert.tostring instead of tostring(). .....................................
        }
        protected void GridContactPerson_HtmlDataCellPrepared(object sender, DevExpress.Web.ASPxGridViewTableDataCellEventArgs e)
        {
            if (e.DataColumn.FieldName == "status")
            {
                if (e.CellValue.Equals("Suspended"))
                    e.Cell.BackColor = System.Drawing.Color.LightGray;
            }
        }
        protected void GridContactPerson_InitNewRow(object sender, DevExpress.Web.Data.ASPxDataInitNewRowEventArgs e)
        {
            e.NewValues["cp_status"] = "Y";
           
        }

        protected void GridContactPerson_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
        {
            // .............................Code Commented and Added by Sanjib on 09122016 to use Convert.tostring instead of tostring(). ................

            oDBEngine.DeleteValue("tbl_master_phoneFax", "phf_cntId='" + Convert.ToString(e.Keys[0]).Trim() + "'");
            oDBEngine.DeleteValue("tbl_master_Email", "eml_cntId='" + Convert.ToString(e.Keys[0]).Trim() + "'");
            oDBEngine.DeleteValue("tbl_master_address", "add_cntId='" + Convert.ToString(e.Keys[0]).Trim() + "'");
            oDBEngine.DeleteValue("tbl_master_contactPerson", "cp_contactId='" + Convert.ToString(e.Keys[0]).Trim() + "'");
            e.Cancel = true;
            fillgrid();
            e.Cancel = true;
            GridContactPerson.CancelEdit();
            // .............................Code Above Commented and Added by Sanjib on 09122016 to use Convert.tostring instead of tostring(). .....................................
        }

        protected void GridContactPerson_CellEditorInitialize(object sender, ASPxGridViewEditorEventArgs e)
        {
            if (!GridContactPerson.IsNewRowEditing)
            {

            }
        }
        protected void GridContactPerson_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
        {
            //ASPxComboBox tcscts = (ASPxComboBox)GridContactPerson.FindEditFormTemplateControl("cp_relationShip");
            //string istcsrate = Convert.ToString(tcscts.Value);


             // .............................Code Commented and Added by Sanjib on 09122016 to use Convert.tostring instead of tostring(). ................
            DataSet dsEmail = new DataSet();
            //String conn = ConfigurationSettings.AppSettings["DBConnectionDefault"];
            //SqlConnection con = new SqlConnection(conn);
            //SqlCommand cmd3 = new SqlCommand("ContactPersonUpdateforInsComp", con);
            //cmd3.CommandType = CommandType.StoredProcedure;

            string name = string.Empty;
            string Officephone = string.Empty;
            string Residencephone = string.Empty;
            string Mobilephone = string.Empty;
            string email = string.Empty;
            string cp_designation = string.Empty;
            string cp_relationShip = string.Empty;
            string cp_status = string.Empty;
            string cp_Pan = string.Empty;
            string cp_Din = string.Empty;
            string contactid = string.Empty;
            string userid = string.Empty;

            if (e.NewValues[0] == null)
            {
                name = "";
            }
            else
            {
                name = Convert.ToString(e.NewValues[0]);

            }
            if (e.NewValues[1] == null)
            {
                Officephone = "";
            }
            else
            {
                Officephone = Convert.ToString(e.NewValues[1]);
            }
            if (e.NewValues[2] == null)
            {
                Residencephone = "";
            }
            else
            {
                Residencephone = Convert.ToString(e.NewValues[2]);
            }
            if (e.NewValues[3] == null)
            {
                Mobilephone = "";
            }
            else
            {
                Mobilephone = Convert.ToString(e.NewValues[3]);
            }
            if (e.NewValues[4] == null)
            {
                email = "";
            }
            else
            {
                email = Convert.ToString(e.NewValues[4]);
            }
            if (e.NewValues[5] == null)
            {
                cp_designation = "0";
                cp_relationShip = "0";
            }
            else
            {

                if (Convert.ToString(e.NewValues[5]) == "20")
                {
                    cp_relationShip = Convert.ToString(e.NewValues[5]);
                    //cmd3.Parameters.AddWithValue("@cp_designation", e.NewValues[6]);
                    if (e.NewValues[6] == null)
                        cp_designation = "0";
                    else
                        cp_designation = Convert.ToString(e.NewValues[6]);
                }
                else
                {
                    cp_relationShip = Convert.ToString(e.NewValues[5]);
                    cp_designation = "0";
                }
            }
            if (e.NewValues[7] == null)
            {
                cp_status = "";
            }
            else
            {
                cp_status = Convert.ToString(e.NewValues[7]);
            }
            if (e.NewValues[8] == null)
            {
                cp_Pan = "";
            }
            else
            {
                cp_Pan = Convert.ToString(e.NewValues[8]);
            }
            if (e.NewValues[9] == null)
            {
                cp_Din = "";
            }
            else
            {
                cp_Din = Convert.ToString(e.NewValues[9]);
            }
            contactid = Convert.ToString(e.Keys[0]);
            userid = Convert.ToString(HttpContext.Current.Session["userid"]);

            Insurance objInsurance = new Insurance();

            dsEmail = objInsurance.GridContactPerson_BL(name, Officephone, Residencephone, Mobilephone, email,
                 cp_designation, cp_relationShip, cp_status, cp_Pan, cp_Din, contactid, userid);

            e.Cancel = true;
            fillgrid();
            e.Cancel = true;
            GridContactPerson.CancelEdit();

            // .............................Code Above Commented and Added by Sanjib on 09122016 to use Convert.tostring instead of tostring(). .....................................
        }
        protected void GridContactPerson_RowInserting(object sender, DevExpress.Web.Data.ASPxDataInsertingEventArgs e)
        {
            // .............................Code Commented and Added by Sanjib on 09122016 to use Convert.tostring instead of tostring(). ................
            DataSet dsEmail = new DataSet();
            //String conn = ConfigurationSettings.AppSettings["DBConnectionDefault"];MULTI
            String conn = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            SqlConnection con = new SqlConnection(conn);
            SqlCommand cmd3 = new SqlCommand("ContactPersonInsertforInsCompany", con);
            cmd3.CommandType = CommandType.StoredProcedure;
            if (e.NewValues[0] == null)
            {
                cmd3.Parameters.AddWithValue("@name", "");
            }
            else
            {
                cmd3.Parameters.AddWithValue("@name", Convert.ToString(e.NewValues[0]).Trim());
            }
            if (e.NewValues[1] == null)
            {
                cmd3.Parameters.AddWithValue("@Officephone", "");
            }
            else
            {
                cmd3.Parameters.AddWithValue("@Officephone", Convert.ToString(e.NewValues[1]).Trim());
            }
            if (e.NewValues[2] == null)
            {
                cmd3.Parameters.AddWithValue("@Residencephone", "");
            }
            else
            {
                cmd3.Parameters.AddWithValue("@Residencephone", Convert.ToString(e.NewValues[2]).Trim());
            }
            if (e.NewValues[3] == null)
            {
                cmd3.Parameters.AddWithValue("@Mobilephone", "");
            }
            else
            {
                cmd3.Parameters.AddWithValue("@Mobilephone", e.NewValues[3]);
            }
            if (e.NewValues[4] == null)
            {
                cmd3.Parameters.AddWithValue("@email", "");
            }
            else
            {
                cmd3.Parameters.AddWithValue("@email", Convert.ToString(e.NewValues[4]).Trim());
            }

            if (e.NewValues[5] == null)
            {
                cmd3.Parameters.AddWithValue("@cp_designation", "0");
                cmd3.Parameters.AddWithValue("@cp_relationShip", "0");
            }
            else
            {

                //cmd3.Parameters.AddWithValue("@cp_designation", e.NewValues[6]);

                if (Convert.ToString(e.NewValues[5]) == "20")
                {
                    cmd3.Parameters.AddWithValue("@cp_relationShip", e.NewValues[5]);
                    //cmd3.Parameters.AddWithValue("@cp_designation", e.NewValues[6]);
                    if (e.NewValues[6] == null)
                        cmd3.Parameters.AddWithValue("@cp_designation", "0");
                    else
                        cmd3.Parameters.AddWithValue("@cp_designation", e.NewValues[6]);
                }
                else
                {
                    cmd3.Parameters.AddWithValue("@cp_relationShip", e.NewValues[5]);
                    cmd3.Parameters.AddWithValue("@cp_designation", "0");
                }
            }
            if (e.NewValues[7] == null)
            {
                cmd3.Parameters.AddWithValue("@cp_status", "");
            }
            else
            {
                cmd3.Parameters.AddWithValue("@cp_status", e.NewValues[7]);
            }
            if (e.NewValues[8] == null)
            {
                cmd3.Parameters.AddWithValue("@cp_Pan", "");
            }
            else
            {
                cmd3.Parameters.AddWithValue("@cp_Pan", Convert.ToString(e.NewValues[8]).Trim());
            }
            if (e.NewValues[9] == null)
            {
                cmd3.Parameters.AddWithValue("@cp_Din", "");
            }
            else
            {
                cmd3.Parameters.AddWithValue("@cp_Din", e.NewValues[9]);
            }
            cmd3.Parameters.AddWithValue("@agentid", Convert.ToString(Session["KeyVal_InternalID"]));
            cmd3.Parameters.AddWithValue("@userid", Convert.ToString(HttpContext.Current.Session["userid"]));


            cmd3.CommandTimeout = 0;
            SqlDataAdapter Adap = new SqlDataAdapter();
            Adap.SelectCommand = cmd3;
            Adap.Fill(dsEmail);
            cmd3.Dispose();
            con.Dispose();
            GC.Collect();
            e.Cancel = true;
            fillgrid();
            e.Cancel = true;
            GridContactPerson.CancelEdit();

            // .............................Code Above Commented and Added by Sanjib on 09122016 to use Convert.tostring instead of tostring(). .....................................

        }

        //comment by sanjib due to changed implementaion 21122016
        //protected void cmbExport_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    Int32 Filter = int.Parse(cmbExport.SelectedItem.Value.ToString());
        //    switch (Filter)
        //    {
        //        case 1:
        //            exporter.WritePdfToResponse();
        //            break;
        //        case 2:
        //            exporter.WriteXlsToResponse();
        //            break;
        //        case 3:
        //            exporter.WriteRtfToResponse();
        //            break;
        //        case 4:
        //            exporter.WriteCsvToResponse();
        //            break;
        //    }
        //}
        //protected void cmbExport_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    //bindUserGroups();
        //    // .............................Code Commented and Added by Sanjib on 09122016 to use Convert.tostring instead of tostring(). ................
        //    //Int32 Filter = int.Parse(cmbExport.SelectedItem.Value.ToString());
        //    string filename = string.Empty;
        //    if (Session["Contactrequesttype"] != null)
        //    {
        //        filename = Convert.ToString(Session["Contactrequesttype"]);
                
        //    }
        //    else
        //    {
        //        filename = "Lead";

        //    }
        //    exporter.PageHeader.Left = filename;

        //    exporter.PageFooter.Center = "[Page # of Pages #]";
        //    exporter.PageFooter.Right = "[Date Printed]";
        //    GridContactPerson.Columns[8].Visible = false;

        //    Int32 Filter = int.Parse(Convert.ToString(drdExport.SelectedItem.Value));
        //    switch (Filter)
        //    {
        //        case 1:
        //            //exporter.WritePdfToResponse();

        //            using (MemoryStream stream = new MemoryStream())
        //            {
        //                exporter.WritePdf(stream);
        //                WriteToResponse("ExportEmployee", true, "pdf", stream);
        //            }
        //            //Page.Response.End();
        //            break;
        //        case 2:
        //            exporter.WriteXlsToResponse();
        //            break;
        //        case 3:
        //            exporter.WriteRtfToResponse();
        //            break;
        //        case 4:
        //            exporter.WriteCsvToResponse();
        //            break;
        //    }
        //    // .............................Code Above Commented and Added by Sanjib on 09122016 to use Convert.tostring instead of tostring(). .....................................
        //}

        public void bindexport(int Filter)
        {
            GridContactPerson.Columns[8].Visible = false;
            //MainAccountGrid.Columns[20].Visible = false;
            // MainAccountGrid.Columns[21].Visible = false;
            string filename = "Contact Person";
            exporter.FileName = filename;

            exporter.PageHeader.Left = "Contact Person";
            exporter.PageFooter.Center = "[Page # of Pages #]";
            exporter.PageFooter.Right = "[Date Printed]";

            switch (Filter)
            {
                case 1:
                    exporter.WritePdfToResponse();
                    break;
                case 2:
                    exporter.WriteXlsToResponse();
                    break;
                case 3:
                    exporter.WriteRtfToResponse();
                    break;
                case 4:
                    exporter.WriteCsvToResponse();
                    break;
            }
        }
        protected void cmbExport_SelectedIndexChanged(object sender, EventArgs e)
        {
            Int32 Filter = int.Parse(Convert.ToString(drdExport.SelectedItem.Value));
            if (Filter != 0)
            {
                if (Session["exportval"] == null)
                {
                    Session["exportval"] = Filter;
                    bindexport(Filter);
                }
                else if (Convert.ToInt32(Session["exportval"]) != Filter)
                {
                    Session["exportval"] = Filter;
                    bindexport(Filter);
                }
            }
        }
        protected void WriteToResponse(string fileName, bool saveAsFile, string fileFormat, MemoryStream stream)
        {
            if (Page == null || Page.Response == null) return;
            string disposition = saveAsFile ? "attachment" : "inline";
            Page.Response.Clear();
            Page.Response.Buffer = false;
            Page.Response.AppendHeader("Content-Type", string.Format("application/{0}", fileFormat));
            Page.Response.AppendHeader("Content-Transfer-Encoding", "binary");
            Page.Response.AppendHeader("Content-Disposition", string.Format("{0}; filename={1}.{2}", disposition, HttpUtility.UrlEncode(fileName).Replace("+", "%20"), fileFormat));
            if (stream.Length > 0)
                Page.Response.BinaryWrite(stream.ToArray());
            //Page.Response.End();
        }
    }
}