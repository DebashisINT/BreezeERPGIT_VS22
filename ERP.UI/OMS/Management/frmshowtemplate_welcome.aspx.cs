using System;
using System.Web.UI.WebControls;
using System.Configuration;
using BusinessLogicLayer;

namespace ERP.OMS.Management
{
    public partial class management_frmshowtemplate_welcome : System.Web.UI.Page
    {
        //BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);MULTI
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
        protected void Page_Load(object sender, EventArgs e)
        {
            MessageText();
        }
        public void MessageText()
        {
            int temp_id = Convert.ToInt32(Request.QueryString["tem_id"]);
            string[,] recipient = oDBEngine.GetFieldValue("tbl_master_template", "tem_recipients", " tem_id='" + temp_id + "'", 1);
            if (recipient[0, 0] != "n")
            {
                HREC.Value = recipient[0, 0];
            }
            string[,] msg1 = oDBEngine.GetFieldValue("tbl_master_template", "tem_msg", " tem_id='" + temp_id + "'", 1);
            if (msg1[0, 0] != "n")
            {
                HTEMPLATE.Value = msg1[0, 0].Trim();
            }
            string msg = msg1[0, 0].ToString().Trim();
            //string[] msg = msg2;
            int ij = 0;
            string st1 = "";
            string s1 = "";
            bool flag = false;
            string txt_text = "";
            string g = "";
            Table dt = new Table();
            TableRow dr = new TableRow();
            TableCell dc = new TableCell();
            dt.BackColor = System.Drawing.Color.Blue;
            dt.Rows.Add(dr);
            dr.Cells.Add(dc);
            for (int i = 0; i < msg.Length; i++)
            {
                if (flag == true)
                {
                    if (msg[i] == '>')
                    {
                        AjaxControlToolkit.TextBoxWatermarkExtender a = new AjaxControlToolkit.TextBoxWatermarkExtender();
                        flag = false;
                        g = ij.ToString();
                        TextBox T = new TextBox();
                        T.ID = "textbox_" + g;
                        T.BorderWidth = 1;
                        T.Font.Size = 8;
                        T.Height = 10;
                        T.ForeColor = System.Drawing.Color.Blue;
                        T.Font.Underline = true;
                        T.BackColor = System.Drawing.Color.Transparent;
                        a.ID = "water_" + g;
                        a.TargetControlID = "textbox_" + g;
                        a.WatermarkText = txt_text.ToUpper();
                        show_teplate.Controls.Add(a);
                        show_teplate.Controls.Add(T);
                        if (ij == 0)
                        {
                            T.Focus();
                        }
                        ij += 1;
                        txt_text = "";
                    }
                    else
                    {
                        txt_text += msg[i];
                    }
                }
                else
                {
                    if (msg[i] == '<')
                    {
                        flag = true;
                        Label l = new Label();
                        l.Text = st1;
                        l.Font.Size = 9;
                        st1 = "";
                        l.ID = "label" + g;
                        show_teplate.Controls.Add(l);
                    }
                    else
                    {
                        st1 += msg[i];
                    }
                }
            }
            if (st1.Trim() != "")
            {
                Label l1 = new Label();
                l1.Text = st1;
                st1 = "";
                l1.ID = "label" + g;
                show_teplate.Controls.Add(l1);
            }
        }
    }
}