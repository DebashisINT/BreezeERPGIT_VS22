using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Data.SqlClient;
using System.Net.Mail;
using System.Xml;
using System.IO;
using DevExpress.Web;


/// <summary>
/// Summary description for AspxHelper
/// </summary>
public class AspxHelper
{
    SqlConnection oSqlConnection = new SqlConnection();

    public AspxHelper()
    {
    }


    public void Bind_Combo(ASPxComboBox cComboBoxName,        // the name of the comboBox
                           SqlDataSource cSqlDataSourceObject,// the object of the data source
                           String cTextFieldName,             // the name of the text field
                           String cValueFieldName,            // the name of the value field
                           int cSelectedIndex                 // the selected index value
                           )
    {
        cComboBoxName.DataSource = cSqlDataSourceObject;
        cComboBoxName.TextField = cTextFieldName;
        cComboBoxName.ValueField = cValueFieldName;
        cComboBoxName.DataBind();
        cComboBoxName.SelectedIndex = cSelectedIndex;

    }




    public void Bind_Combo(ASPxComboBox cComboBoxName,   // the name of the comboBox 
                          DataSet cDataSetObject,        // the object of the DataSet
                          String cTextFieldName,         // the name of the text field
                          String cValueFieldName,        // the name of the value field
                          int cSelectedIndex             // the selected index value
                          )
    {
        if (cDataSetObject.Tables.Count > 0)
        {
            if (cDataSetObject.Tables[0].Rows.Count > 0)
            {
                cComboBoxName.DataSource = cDataSetObject;
                cComboBoxName.TextField = cTextFieldName;
                cComboBoxName.ValueField = cValueFieldName;
                cComboBoxName.DataBind();
                cComboBoxName.SelectedIndex = cSelectedIndex;
            }
        }

    }

    public void Bind_Combo(ASPxComboBox cComboBoxName,   // the name of the comboBox 
                          DataTable cDataTableObject,    // the object of the DataTable
                          String cTextFieldName,         // the name of the text field
                          String cValueFieldName       // the name of the value field
        // int? cSelectedIndex             // the selected index value
                          )
    {

        if (cDataTableObject.Rows.Count > 0)
        {
            cComboBoxName.DataSource = cDataTableObject;
            cComboBoxName.TextField = cTextFieldName;
            cComboBoxName.ValueField = cValueFieldName;
            cComboBoxName.DataBind();
            //  cComboBoxName.SelectedIndex = cSelectedIndex;
        }


    }

    public void Bind_Combo(ASPxComboBox cComboBoxName, // the name of the comboBox 
                                             DataSet cDataSetObject,     // the object of the DataSet
                                             String cTextFieldName,      // the name of the text field
                                             String cValueFieldName,     // the name of the value field
                                             object cSelectedValue       // the value that is selected
                                             )
    {
        if (cDataSetObject.Tables.Count > 0)
        {
            if (cDataSetObject.Tables[0].Rows.Count > 0)
            {
                cComboBoxName.DataSource = cDataSetObject;
                cComboBoxName.TextField = cTextFieldName;
                cComboBoxName.ValueField = cValueFieldName;
                cComboBoxName.DataBind();
                if (cSelectedValue.ToString().Trim() != "0")
                    cComboBoxName.Value = cSelectedValue;
                else
                    cComboBoxName.SelectedIndex = 0;
            }
        }

    }
    public void Bind_Combo(ASPxComboBox cComboBoxName, // the name of the comboBox 
                                             DataTable cDataTableObject,     // the object of the DataSet
                                             String cTextFieldName,      // the name of the text field
                                             String cValueFieldName,     // the name of the value field
                                           object cSelectedValue       // the value that is selected
                                             )
    {

        if (cDataTableObject.Rows.Count > 0)
        {
            cComboBoxName.DataSource = cDataTableObject;
            cComboBoxName.TextField = cTextFieldName;
            cComboBoxName.ValueField = cValueFieldName;
            cComboBoxName.DataBind();
            if (cSelectedValue != null)
            {
                if (cSelectedValue.ToString().Trim() != "0")
                    cComboBoxName.Value = cSelectedValue;
                else
                    cComboBoxName.SelectedIndex = 0;
            }
        }


    }

    public void Bind_Combo(ASPxComboBox cComboBoxName, // the name of the comboBox 
                                             DataSet cDataSetObject,     // the object of the DataSet
                                             String cTextFieldName,      // the name of the text field
                                             String cValueFieldName,     // the name of the value field
                                             String cSelectedText       // the Text that is selected
                                             )
    {
        if (cDataSetObject.Tables.Count > 0)
        {
            if (cDataSetObject.Tables[0].Rows.Count > 0)
            {
                cComboBoxName.DataSource = cDataSetObject;
                cComboBoxName.TextField = cTextFieldName;
                cComboBoxName.ValueField = cValueFieldName;
                cComboBoxName.DataBind();
                if (cSelectedText != null)
                    cComboBoxName.Text = cSelectedText;
                else
                    cComboBoxName.SelectedIndex = 0;
            }
        }

    }
    public void Bind_Combo(ASPxComboBox cComboBoxName, // the name of the comboBox 
                                             DataTable cDataTableObject,  // the object of the DataTable
                                             String cTextFieldName,      // the name of the text field
                                             String cValueFieldName,    // the name of the value field
                                             String cSelectedText      // the Text that is selected
                                             )
    {

        if (cDataTableObject.Rows.Count > 0)
        {
            cComboBoxName.DataSource = cDataTableObject;
            cComboBoxName.TextField = cTextFieldName;
            cComboBoxName.ValueField = cValueFieldName;
            cComboBoxName.DataBind();
            if (cSelectedText != null)
                cComboBoxName.Text = cSelectedText;
            else
                cComboBoxName.SelectedIndex = 0;
        }


    }


    public DataSet Bind_Combo(string cStringQuery   //the string querry
                             )
    {
        DataSet Ds = new DataSet();
        //using (SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"]))  MULTI
        using (SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"])))
        {
            using (SqlCommand com = new SqlCommand(cStringQuery, con))
            {
                using (SqlDataAdapter Da = new SqlDataAdapter(com))
                {
                    Da.Fill(Ds);
                }
            }
        }
        return Ds;
    }


    public DataSet Bind_Combo(string cStringQuery,
                        string cParametername,
                        string cParametervalue)
    {
        DataSet Ds = new DataSet();
        //using (SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"]))  MULTI
        using (SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"])))
        {
            using (SqlCommand com = new SqlCommand(cStringQuery, con))
            {
                com.Parameters.AddWithValue(cParametername, cParametervalue);
                using (SqlDataAdapter Da = new SqlDataAdapter(com))
                {
                    Da.Fill(Ds);
                }
            }
        }
        return Ds;
    }

    public void Bind_RadioButtonList(ASPxRadioButtonList cRdBtnName,   // the name of the comboBox 
                                      DataTable cDataTableObject,    // the object of the DataTable
                                      String cTextFieldName,         // the name of the text field
                                      String cValueFieldName,        // the name of the value field
                                      int cSelectedIndex             // the selected index value
                                     )
    {

        if (cDataTableObject.Rows.Count > 0)
        {
            cRdBtnName.DataSource = cDataTableObject;
            cRdBtnName.TextField = cTextFieldName;
            cRdBtnName.ValueField = cValueFieldName;
            cRdBtnName.DataBind();
            cRdBtnName.SelectedIndex = cSelectedIndex;
        }


    }






    public void BindGrid(ASPxGridView cGridViewName,    // the name of the gridview
                            DataSet cDataSetObject,    // the object of the DataSet
                            String AscOrDesc,         // in which way the string wants to be sorted
                            String ColumnName
                         )
    {
        if (cDataSetObject != null)
        {
            if (cDataSetObject.Tables.Count > 0)
            {
                if (cDataSetObject.Tables[0].Rows.Count > 0)
                {

                    DataView TempDV = new DataView(cDataSetObject.Tables[0]);
                    TempDV.Sort = ColumnName + " " + AscOrDesc;
                    cGridViewName.DataSource = TempDV;
                    cGridViewName.DataBind();
                }
            }

            else
            {
                BindGrid(cGridViewName);
            }
        }
        else
            BindGrid(cGridViewName);


    }


    public void BindGrid(ASPxGridView cGridViewName,
                            DataTable cDataTableObject,
                            String AscOrDesc,
                            String ColumnName
                         )
    {
        if (cDataTableObject != null)
        {
            if (cDataTableObject.Rows.Count > 0)
            {
                DataView TempDV = new DataView(cDataTableObject);
                TempDV.Sort = ColumnName + AscOrDesc;
                cGridViewName.DataSource = TempDV;
                cGridViewName.DataBind();
            }
            else
            {
                BindGrid(cGridViewName);
            }
        }
        else
        {
            BindGrid(cGridViewName);
        }
    }



    public void BindGrid(DataTable Dt, ASPxGridView Gv)
    {
        if (Dt != null)
        {
            if (Dt.Rows.Count > 0)
            {
                Gv.DataSource = Dt;
                Gv.DataBind();
            }
            else
            {
                BindGrid(Gv);
            }
        }
        else
        {
            BindGrid(Gv);
        }
    }

    public void BindGrid(ASPxGridView cGridViewName,    // the name of the gridview
                         DataSet cDataSetObject     // the object of the DataSet
                         )
    {
        if (cDataSetObject.Tables.Count > 0)
        {
            if (cDataSetObject.Tables[0].Rows.Count > 0)
            {
                DataView TempDV = new DataView(cDataSetObject.Tables[0]);
                cGridViewName.DataSource = TempDV;
                cGridViewName.DataBind();
            }
            else
            {
                BindGrid(cGridViewName);
            }

        }
        else
        {
            BindGrid(cGridViewName);
        }


    }

    public void BindGrid(ASPxGridView Gv)
    {
        Gv.DataSource = null;
        Gv.DataBind();
    }


}


