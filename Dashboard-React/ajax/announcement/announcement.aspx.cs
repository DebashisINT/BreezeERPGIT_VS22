using BusinessLogicLayer;
using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Dashboard_React.ajax.announcement
{
    public partial class announcement : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        [WebMethod]
        public static object GetMyAnnouncement()
        {
            MasterSettings masterbl = new MasterSettings();
            string mastersettings = masterbl.GetSettings("isServiceManagementRequred");
            List<announcements> anc = new List<announcements>();

            ProcedureExecute proc = new ProcedureExecute("prc_AnnouncementAddEdit");
            proc.AddPara("@Action", "MyAnnouncement");
            proc.AddPara("@isServiceManagement", mastersettings);
            proc.AddPara("@userid", HttpContext.Current.Session["userid"]);
            DataTable Dt = proc.GetTable();

            anc = (from DataRow dr in Dt.Rows
                   select new announcements()
                   {
                       title = Convert.ToString(dr["title"]),
                       msg = Convert.ToString(dr["annoucement"]),
                       anninHtml = Convert.ToString(dr["anninHtml"]),
                       AncId = Convert.ToInt32(dr["AncId"]),
                       allowCmnt = Convert.ToBoolean(dr["AllowComment"])
                   }).ToList();

            return anc;

        }
        [WebMethod]
        public static bool SaveComment(string comment, string AncId)
        {
            ProcedureExecute proc = new ProcedureExecute("prc_AnnouncementAddEdit");
            proc.AddPara("@Action", "AddComment");
            proc.AddPara("@userid", HttpContext.Current.Session["userid"]);
            proc.AddPara("@comment", comment);
            proc.AddPara("@id", AncId);
            proc.RunActionQuery();

            return true;
        }



        [WebMethod]
        public static object Gettop5Comment(string AncId)
        {
            commentfor5box commentbox = new commentfor5box();

            List<commentList> anc = new List<commentList>();

            ProcedureExecute proc = new ProcedureExecute("prc_AnnouncementAddEdit");
            proc.AddPara("@Action", "GetLast5Comment");
            proc.AddPara("@id", AncId);
            DataSet Dt = proc.GetDataSet();

            anc = (from DataRow dr in Dt.Tables[0].Rows
                   select new commentList()
                   {
                       user_name = Convert.ToString(dr["user_name"]),
                       Comment = Convert.ToString(dr["Comment"]),
                       CommentOn = Convert.ToString(dr["CommentOn"])
                   }).ToList();

            commentbox.cmt = anc;
            commentbox.totalCount = Convert.ToInt32(Dt.Tables[1].Rows[0][0]);

            return commentbox;

        }

        [WebMethod]
        public static object GetAllComment(string AncId)
        {

            List<commentList> anc = new List<commentList>();

            ProcedureExecute proc = new ProcedureExecute("prc_AnnouncementAddEdit");
            proc.AddPara("@Action", "GetAllComment");
            proc.AddPara("@id", AncId);
            DataSet Dt = proc.GetDataSet();

            anc = (from DataRow dr in Dt.Tables[0].Rows
                   select new commentList()
                   {
                       user_name = Convert.ToString(dr["user_name"]),
                       Comment = Convert.ToString(dr["Comment"]),
                       CommentOn = Convert.ToString(dr["CommentOn"])
                   }).ToList();


            return anc;

        }



        public class announcements
        {
            public string title { get; set; }
            public string msg { get; set; }
            public string anninHtml { get; set; }
            public int AncId { get; set; }
            public bool allowCmnt { get; set; }
        }

        public class commentList
        {
            public string user_name { get; set; }
            public string Comment { get; set; }
            public string CommentOn { get; set; }
        }

        public class commentfor5box
        {
            public int totalCount { get; set; }
            public List<commentList> cmt { get; set; }
        }


    }
    

}