using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ERP.OMS.Management.Activities.UserControls
{
    public partial class UOMConversion : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindTradingLotUnits();
            }
        }

        protected void BindTradingLotUnits()
        {
            //  / oGenericMethod = new GenericMethod();
            BusinessLogicLayer.GenericMethod oGenericMethod = new BusinessLogicLayer.GenericMethod();

            DataTable dtCmb = new DataTable();
            dtCmb = oGenericMethod.GetDataTable("select UOM_ID,UOM_Name from Master_UOM  order by UOM_Name");
            AspxHelper oAspxHelper = new AspxHelper();
            if (dtCmb.Rows.Count > 0)
            {
                //Added for packing uom
                oAspxHelper.Bind_Combo(cmbPackingUom, dtCmb, "UOM_Name", "UOM_ID", "");
                oAspxHelper.Bind_Combo(cmbPackingSelectUom, dtCmb, "UOM_Name", "UOM_ID", "");
                
            }

        }
    }
}