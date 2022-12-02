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
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;

namespace BusinessLogicLayer
{
   public class Log_DataContainer
    {

        DBEngine oDbEngine;
        //string ConnectionString = ConfigurationManager.AppSettings["DBConnectionDefault"].ToString();
        public Log_DataContainer()
        {
            oDbEngine = new DBEngine();
        }
        private void CreateFolders(string PrmSharedFolderPath)
        {
            if (!Directory.Exists(PrmSharedFolderPath))
            {
                string strDate = String.Empty;
                string strYear = String.Empty;
                string strMonth = String.Empty;
                string strDay = String.Empty;
                string[] DateArray = GetDBDateTime();
                strYear = DateArray[0].ToString();
                strMonth = DateArray[1].ToString();
                strDay = DateArray[2].ToString();
                strDate = DateArray[3].ToString();
                string SharedFolderPath = String.Empty;

                int Year = Convert.ToInt32(strYear);
                int month = 1;
                int Day = 1;
                string MonthName = String.Empty;
                string FolderName = String.Empty;
                DateTime Date;
                for (; month <= 12; month++)
                {
                    MonthName = String.Empty;
                    Day = 1;
                    Date = new DateTime(Year, month, Day);
                    SharedFolderPath = ConfigurationManager.AppSettings["SaveCSVsql"].ToString() + "LogFiles\\ProcessLog\\" + Year + "\\";
                    MonthName = month.ToString() + "." + Date.ToString("MMMM").ToString();
                    SharedFolderPath = SharedFolderPath + MonthName;
                    for (Day = 1; Day <= DateTime.DaysInMonth(Year, month); Day++)
                    {
                        FolderName = String.Empty;
                        FolderName = SharedFolderPath + "\\" + Year.ToString().Trim() + ((month.ToString().Length > 1) ? month.ToString() : "0" + month.ToString()) + ((Day.ToString().Length > 1) ? Day.ToString() : "0" + Day.ToString());
                        Directory.CreateDirectory(FolderName);
                    }
                }
            }
        }
        public string[] GetDBDateTime()
        {
            string Query = "Select Year(GetDate()) [Year],Month(GetDate()) [Month],Day(GetDate()) [Day],GetDate() [Date]";
            string[] DateArray = new string[4];
            SqlDataReader Dr = oDbEngine.GetReader(Query);
            try
            {
                while (Dr.Read())
                {
                    if (Dr.HasRows)
                    {
                        DateArray[0] = Dr[0].ToString();
                        DateArray[1] = Dr[1].ToString();
                        DateArray[2] = Dr[2].ToString();
                        DateArray[3] = Dr[3].ToString();
                    }
                }
                Dr.Close();
            }
            catch { Dr.Close(); }
            finally { Dr.Close(); }
            return DateArray;
        }
        private string GetFolderPath()
        {
            string SharedFolderPath = ConfigurationManager.AppSettings["SaveCSVsql"].ToString() + "LogFiles\\ProcessLog\\";
            string[] DateArray = GetDBDateTime();
            string strDate = String.Empty;
            string strYear = String.Empty;
            string strMonth = String.Empty;
            string strDay = String.Empty;
            strYear = DateArray[0].ToString();
            strMonth = DateArray[1].ToString();
            strDay = DateArray[2].ToString();
            strDate = DateArray[3].ToString();
            CreateFolders(SharedFolderPath + "\\" + strYear + "\\");
            string FolderName = SharedFolderPath + "\\" + strYear + "\\" + strMonth.Trim() + "." + Convert.ToDateTime(strDate).ToString("MMMM") + "\\" + strYear.ToString().Trim() + ((strMonth.ToString().Length > 1) ? strMonth.ToString() : "0" + strMonth.ToString()) + ((strDay.ToString().Length > 1) ? strDay.ToString() : "0" + strDay.ToString());
            return FolderName;
        }
        public string GetAutoGenerateNo(DateTime Date)
        {
            string datetd = Date.ToString();
            string[] slashsp = datetd.Split('/');
            string mm = slashsp[0].ToString();
            string dd = slashsp[1].ToString();
            string[] dashsp = slashsp[2].ToString().Split(' ');
            string yy = dashsp[0].ToString();
            string ampm = dashsp[2].ToString();
            string[] tmsp = dashsp[1].ToString().Split(':');
            string hr = tmsp[0].ToString();
            string min = tmsp[1].ToString();
            string mls = tmsp[2].ToString();
            string totnum = mm + dd + yy + hr + min + mls + ampm;
            return totnum;
        }
        private string GetFileName(string UserID)
        {
            DataTable Dt = new DataTable();
            string[] DateArray = GetDBDateTime();
            DateTime Date = Convert.ToDateTime(DateArray[3].ToString());
            Dt = oDbEngine.GetDataTable("tbl_Master_User", "User_Activity", "User_ID=" + HttpContext.Current.Session["UserID"].ToString() +
                " and User_LastSegement=" + HttpContext.Current.Session["userLastSegment"].ToString());
            if (Dt.Rows.Count > 0)
                return Dt.Rows[0][0].ToString().Trim() + "_" + UserID.Trim() + "_" + GetAutoGenerateNo(Date);

            return null;
        }
        public int Log_Container(DataSet DsLog)
        {
            DataSet TempDs;
            string FileName = GetFileName(HttpContext.Current.Session["userid"].ToString());
            if (FileName != null)
            {
                foreach (DataTable DT in DsLog.Tables)
                {
                    TempDs = new DataSet();
                    TempDs.Tables.Add(DT.Copy());
                    TempDs.WriteXml(GetFolderPath() + "\\" + FileName);
                    TempDs.Dispose();
                }
                return 1;
            }
            return 0;
        }
        public void Log_Container(DataTable DtLog)
        {

        }
        public void TakeScreeShot()
        {
            //Bitmap bitmap = new Bitmap(1024, 768);
            //Rectangle bitmapRect = new Rectangle(0, 0, 1024, 768);
            //// This is a method of the WebBrowser control, and the most important part
            //webBrowser1.DrawToBitmap(bitmap, bitmapRect);

            //// Generate a thumbnail of the screenshot (optional)
            //System.Drawing.Image origImage = bitmap;
            //System.Drawing.Image origThumbnail = new Bitmap(120, 90, origImage.PixelFormat);

            //Graphics oGraphic = Graphics.FromImage(origThumbnail);
            //oGraphic.CompositingQuality = CompositingQuality.HighQuality;
            //oGraphic.SmoothingMode = SmoothingMode.HighQuality;
            //oGraphic.InterpolationMode = InterpolationMode.HighQualityBicubic;
            //Rectangle oRectangle = new Rectangle(0, 0, 120, 90);
            //oGraphic.DrawImage(origImage, oRectangle);

            //// Save the file in PNG format
            //origThumbnail.Save(GetFolderPath()+"\\Screenshot.png", ImageFormat.Png);
            //origImage.Dispose();
        }

    }
}
