using DevExpress.Web;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ERP.OMS.Management
{
    public partial class Approvalmoduleconfig : System.Web.UI.Page
    {
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(string.Empty);

        protected void Page_Init(object sender, EventArgs e)
        {
            ((GridViewDataComboBoxColumn)grid.Columns["level1userid"]).PropertiesComboBox.DataSource = Cmblevel1();
            ((GridViewDataComboBoxColumn)grid.Columns["level2userid"]).PropertiesComboBox.DataSource = Cmblevel2();
            ((GridViewDataComboBoxColumn)grid.Columns["level3userid"]).PropertiesComboBox.DataSource = Cmblevel3();
            grid.DataSource = GetGriddata();

            if (!IsPostBack)
            {
                grid.DataBind();
              
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        private IEnumerable GetGriddata()
        {

            List<Approvallist> approvallist = new List<Approvallist>();


            DataTable dt = new DataTable();

            dt = oDBEngine.GetDataTable("exec prc_Approvalconfig @actiontype=0");//it is use for action like 0/1/2/3/4 ->select/UPDATE/insert/delete/selectbyid

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                Approvallist Vouchers = new Approvallist();
                Vouchers.moduleid = Convert.ToString(dt.Rows[i]["moduleid"]);
                Vouchers.acid = Convert.ToString(dt.Rows[i]["acid"]);
                Vouchers.active = Convert.ToString(dt.Rows[i]["active"]);
                Vouchers.modulename = Convert.ToString(dt.Rows[i]["modulename"]);
                Vouchers.level1userid = Convert.ToString(dt.Rows[i]["level1userid"]);
                Vouchers.level2userid = Convert.ToString(dt.Rows[i]["level2userid"]);
                Vouchers.level3userid = Convert.ToString(dt.Rows[i]["level3userid"]);

                approvallist.Add(Vouchers);
            }


            return approvallist;

        }

        public IEnumerable Cmblevel1()
        {
            List<Level> LevelList = new List<Level>();

            DataTable DT = oDBEngine.GetDataTable("select user_id ,user_name from tbl_master_user  order by user_name");

            for (int i = 0; i < DT.Rows.Count; i++)
            {
                Level Levels = new Level();
                Levels.LevelID = Convert.ToString(DT.Rows[i]["user_id"]);
                Levels.LevelName = Convert.ToString(DT.Rows[i]["user_name"]);
                LevelList.Add(Levels);
            }

            return LevelList;
        }
        public IEnumerable Cmblevel2()
        {
            List<Level> LevelList = new List<Level>();

            DataTable DT = oDBEngine.GetDataTable("select user_id ,user_name from tbl_master_user  order by user_name");

            for (int i = 0; i < DT.Rows.Count; i++)
            {
                Level Levels = new Level();
                Levels.LevelID = Convert.ToString(DT.Rows[i]["user_id"]);
                Levels.LevelName = Convert.ToString(DT.Rows[i]["user_name"]);
                LevelList.Add(Levels);
            }

            return LevelList;
        }
        public IEnumerable Cmblevel3()
        {
            List<Level> LevelList = new List<Level>();

            DataTable DT = oDBEngine.GetDataTable("select user_id ,user_name from tbl_master_user  order by user_name");

            for (int i = 0; i < DT.Rows.Count; i++)
            {
                Level Levels = new Level();
                Levels.LevelID = Convert.ToString(DT.Rows[i]["user_id"]);
                Levels.LevelName = Convert.ToString(DT.Rows[i]["user_name"]);
                LevelList.Add(Levels);
            }

            return LevelList;
        }
    }

    public class Approvallist
    {
        public string moduleid { get; set; }
        public string acid { get; set; }
        public string active { get; set; }
        public string modulename { get; set; }
        public string level1userid { get; set; }
        public string level2userid { get; set; }
        public string level3userid { get; set; }
    }

    public class Level
    {
        public string LevelID { get; set; }
        public string LevelName { get; set; }
    }
}