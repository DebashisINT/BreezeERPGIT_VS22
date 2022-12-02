using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using BusinessLogicLayer;
using System.IO;
using System.Web.UI.HtmlControls;
using System.Drawing;
using System.Drawing.Text;

namespace ERP.OMS.Management.DailyTask
{
    public partial class Management_DailyTask_InvTransactionPrintForm : ERP.OMS.ViewState_class.VSPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                DailyTask_Inventory oDailyTask = new DailyTask_Inventory();
                DataTable oDataTable = new DataTable();
                string IsContactnamePrint = Convert.ToString(Request.QueryString["IsContactnamePrint"]);
                if (!string.IsNullOrEmpty(Convert.ToString(Request.QueryString["StockPositionId"])))
                {
                    string Id = Convert.ToString(Request.QueryString["StockPositionId"]);
                    string pages = Convert.ToString(Request.QueryString["pages"]);


                    int StockPositionId = 0;
                    if (!string.IsNullOrEmpty(Id))
                    {
                        StockPositionId = Convert.ToInt32(Id);
                    }
                    string Mode = "Print";
                    oDataTable = oDailyTask.GetInvTransactionPrintData(StockPositionId, Mode, IsContactnamePrint);

                    foreach (DataRow item in oDataTable.Rows)
                    {
                        if (Request.QueryString["invNo"] != null)
                        {
                            item["INVOICE_NO"] = Convert.ToString(Request.QueryString["invNo"]);
                        }
                    }
                    if (pages != "0")
                    {
                        DataTable PrintTable = oDataTable.Clone();
                        string[] pgarr = pages.Split(',');
                        for (int i = 0; i < pgarr.Count(); i++)
                        {
                            IEnumerable<DataRow> rows = oDataTable.AsEnumerable()
                            .Where(r => r.Field<string>("Inventory_PieceNo") == Convert.ToString(pgarr[i])
                           );
                            if (rows.FirstOrDefault() != null)
                            {
                                PrintTable.Rows.Add(rows.FirstOrDefault().ItemArray);
                            }

                            /*int nRow=Convert.ToInt32(pgarr[i])-1;
                            var newDataRow = PrintTable.NewRow();
                            newDataRow.ItemArray = oDataTable.Rows[nRow].ItemArray;
                            PrintTable.Rows.Add(newDataRow.ItemArray);*/
                            //PrintTable.ImportRow(oDataTable.Rows[Convert.ToInt32(pgarr[i])]);
                        }
                        if (PrintTable.Rows.Count > 0)
                        {
                            RepDetails.DataSource = PrintTable;
                            RepDetails.DataBind();
                        }
                    }
                    else
                    {
                        if (oDataTable.Rows.Count > 0)
                        {
                            RepDetails.DataSource = oDataTable;
                            RepDetails.DataBind();
                        }
                    }



                    //DataRow dr = oDataTable.NewRow();
                    //for (int i = 0; i < 8; i++)
                    //{
                    //    oDataTable.Rows.Add(dr[1]);
                    //}
                }

            }
            catch (Exception ex)
            {
                lblError.Text = "Page_Load=" + ex.ToString();
                throw ex;
            }


        }

        /*private string GenerateBarCode1(string code)
        {
            BarCode barcode = new BarCode();
            barcode.Symbology = KeepAutomation.Barcode.Symbology.Code39;
            barcode.CodeToEncode = code; //"111222333";
            barcode.ChecksumEnabled = true;
            barcode.X = 1;
            barcode.Y = 50;
            barcode.BarCodeWidth = 100;
            barcode.BarCodeHeight = 70;
            barcode.Orientation = KeepAutomation.Barcode.Orientation.Degree0;
            barcode.BarcodeUnit = KeepAutomation.Barcode.BarcodeUnit.Pixel;
            barcode.DPI = 72;
            barcode.ImageFormat = System.Drawing.Imaging.ImageFormat.Gif;
            barcode.DisplayText = false;               
            string APP_PATH = System.Web.HttpContext.Current.Request.ApplicationPath.ToLower();
            string FileName = "barcode" + code + ".gif";
            string SaveFolder = System.AppDomain.CurrentDomain.BaseDirectory + "BarCode/";
            if (File.Exists(SaveFolder + FileName))
            {
                File.Delete(SaveFolder + FileName);
            }
            barcode.generateBarcodeToImageFile(SaveFolder + FileName);
            return FileName;
        }*/

        private string GenerateBarCode(string code)
        {
            try
            {
                // string fontpath = Server.MapPath("App_Data/IDAutomationHC39MCode39Barcode.ttf");
                //lblError1.Text = fontpath;

                System.Web.UI.WebControls.Image imgBarCode = new System.Web.UI.WebControls.Image();
                using (Bitmap bitMap = new Bitmap(code.Length * 80, 80))
                {
                    using (Graphics graphics = Graphics.FromImage(bitMap))
                    {
                        var pfc = new PrivateFontCollection();
                        //string fontpath = System.AppDomain.CurrentDomain.BaseDirectory + "App_Data/IDAutomationHC39M Code 39 Barcode.ttf"; //IDAutomationHC39M.ttf
                        pfc.AddFontFile(Server.MapPath("~/IDAHC39M.ttf"));
                        //pfc.AddFontFile(fontpath);
                        Font oFont = new Font(pfc.Families[0], 18);
                        PointF point = new PointF(2f, 2f);
                        SolidBrush blackBrush = new SolidBrush(Color.Black);
                        SolidBrush whiteBrush = new SolidBrush(Color.White);
                        graphics.FillRectangle(whiteBrush, 0, 0, 160, bitMap.Height);
                        graphics.DrawString("*" + code + "*", oFont, blackBrush, point);
                    }
                    string APP_PATH = System.Web.HttpContext.Current.Request.ApplicationPath.ToLower();
                    string FileName = "barcode" + code + ".gif";
                    string SaveFolder = System.AppDomain.CurrentDomain.BaseDirectory + "BarCode/";
                    if (File.Exists(SaveFolder + FileName))
                    {
                        File.Delete(SaveFolder + FileName);
                    }
                    bitMap.Save(SaveFolder + FileName, System.Drawing.Imaging.ImageFormat.Jpeg);
                    return FileName;
                }

            }
            catch (Exception ex)
            {
                lblError.Text = "GenerateBarCode=" + ex.ToString();
                throw ex;
                //return "";
            }
        }

        protected void RepDetails_ItemDataBound(Object Sender, RepeaterItemEventArgs e)
        {
            try
            {
                if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
                {
                    var pcNo = ((Label)e.Item.FindControl("lblPcNo")).Text;
                    string FileName = GenerateBarCode(pcNo);
                    ((System.Web.UI.WebControls.Image)e.Item.FindControl("imgBarCode")).ImageUrl = "~/BarCode/" + FileName;

                    if (Request.QueryString["lbl"] != null)
                    {
                        string lbl = Convert.ToString(Request.QueryString["lbl"]);
                        string Height = "0 px";
                        string Width = "0 px";
                        GetLblHeightAndWidth(lbl, out Height, out Width);
                        ((HtmlGenericControl)e.Item.FindControl("dvlbl")).Attributes.Add("height", Height);
                        ((HtmlGenericControl)e.Item.FindControl("dvlbl")).Attributes.Add("width", Width);
                    }
                }

            }
            catch (Exception ex)
            {
                lblError.Text = "RepDetails_ItemDataBound=" + ex.ToString();
                throw ex;
            }
        }

        private void GetLblHeightAndWidth(string lblid, out string Height, out string Width)
        {
            Height = "0 px";
            Width = "0 px";
            try
            {

                if (lblid == "0")
                {
                    Height = "298 px";
                    Width = "198 px";
                }
                else if (lblid == "1")
                {
                    Height = "213 px";
                    Width = "85 px";
                }
            }
            catch (Exception ex)
            {
                lblError.Text = "GetLblHeightAndWidth=" + ex.ToString();
                throw ex;
            }
        }
    }
}