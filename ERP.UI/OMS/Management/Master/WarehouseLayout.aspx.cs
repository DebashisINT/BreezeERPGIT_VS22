using BusinessLogicLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ERP.OMS.Management.Master
{
    public partial class WarehouseLayout : System.Web.UI.Page
    {
        DBEngine objdb = new DBEngine();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                DataTable dtLayout = objdb.GetDataTable("Select * from Master_WarehouseLayout");

                DataTable dtEditable = objdb.GetDataTable("select top 1 1 from v_Stock_WarehouseDetails");
                if (dtEditable != null && dtEditable.Rows.Count > 0)
                {
                    SaveBtn.Visible = false;
                }




                foreach (DataRow dr in dtLayout.Rows)
                {
                    if (Convert.ToInt32(dr["level_id"]) == 1)
                    {
                        level1.Value = Convert.ToString(dr["level_Name"]);
                    }
                    else if (Convert.ToInt32(dr["level_id"]) == 2)
                    {
                        level2.Value = Convert.ToString(dr["level_Name"]);
                    }
                    else if (Convert.ToInt32(dr["level_id"]) == 3)
                    {
                        level3.Value = Convert.ToString(dr["level_Name"]);
                    }
                    else if (Convert.ToInt32(dr["level_id"]) == 4)
                    {
                        level4.Value = Convert.ToString(dr["level_Name"]);
                    }
                    else if (Convert.ToInt32(dr["level_id"]) == 5)
                    {
                        level5.Value = Convert.ToString(dr["level_Name"]);
                    }

                }

            }
        }

        protected void Unnamed_Click(object sender, EventArgs e)
        {
            DataTable i = new DataTable(); 
            i = objdb.GetDataTable("UPDATE Master_WarehouseLayout SET Level_Name='" + level1.Value + "' where Level_id=1");
            i = objdb.GetDataTable("UPDATE Master_WarehouseLayout SET Level_Name='" + level2.Value + "' where Level_id=2");
            i = objdb.GetDataTable("UPDATE Master_WarehouseLayout SET Level_Name='" + level3.Value + "' where Level_id=3");
            i = objdb.GetDataTable("UPDATE Master_WarehouseLayout SET Level_Name='" + level4.Value + "' where Level_id=4");
            i = objdb.GetDataTable("UPDATE Master_WarehouseLayout SET Level_Name='" + level5.Value + "' where Level_id=5");

        }
    }
}