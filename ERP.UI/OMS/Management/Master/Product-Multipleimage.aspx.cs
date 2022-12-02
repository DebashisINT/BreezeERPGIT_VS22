using DataAccessLayer;
using DevExpress.Web;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using UtilityLayer;

namespace ERP.OMS.Management.Master
{
    public partial class Product_Multipleimage : System.Web.UI.Page
    {

        //BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);MULTI
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();

       string ImageURL=ConfigurationManager.AppSettings["ProductImageURL"];
        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
            {
               Session["Productimagelist"] = null;

               lblproductname.InnerText = Request.QueryString["name"];
               hdnprodID.Value = Request.QueryString["prodid"];
            }
        }

        [WebMethod]
        public static List<products> Imagepopulate(string prodId)
        {
            List<products> omodel = new List<products>();

            ProcedureExecute proc = new ProcedureExecute("Sp_GetProductImages");
            proc.AddPara("@ProdID", prodId);
            proc.AddPara("@Action", "ProductImages");
            DataTable dtproduct = proc.GetTable();

            if (dtproduct.Rows.Count > 0)
            {
                omodel = APIHelperMethods.ToModelList<products>(dtproduct);
            }

            return omodel;
        }

        [WebMethod]
        public static List<products> ImageUpload(string prodId)
        {
            HttpFileCollection Files = HttpContext.Current.Request.Files;

            
            for (int i = 0; i < Files.Count; i++)
            {

                HttpPostedFile file = Files[i];

                if (file.ContentLength > 0)
                {

                    file.SaveAs(ConfigurationManager.AppSettings["FilePath"] + System.IO.Path.GetFileName(file.FileName));

                }

            }

            if (HttpContext.Current.Request.Files.Count > 0)
            {
                foreach (HttpPostedFile file in HttpContext.Current.Request.Files)
                {
                    if (file.ContentLength > 0)
                    {
                        //save it
                    }
                }
            }





         
            List<products> omodel = new List<products>();

            ProcedureExecute proc = new ProcedureExecute("Sp_GetProductImages");
            proc.AddPara("@ProdID", prodId);
            DataTable dtproduct = proc.GetTable();
            omodel = APIHelperMethods.ToModelList<products>(dtproduct);
            return omodel;
        }


        public DataTable Populateimage(string prodId)
        {
            ProcedureExecute proc = new ProcedureExecute("Sp_GetProductImages");
            proc.AddPara("@ProdID", prodId);
            proc.AddPara("@ImageURL", ImageURL);
            proc.AddPara("@Action", "ProductImagePopulate");
            DataTable dtproduct = proc.GetTable();
            return dtproduct;
        }

        protected void UploadButton_Click(object sender, EventArgs e)
        {
            string User = Convert.ToString(HttpContext.Current.Session["userid"]).Trim();
            DataTable dtproducts = CreateTempTable("MultipleImage");

            if (file_product.HasFile)
            {
                ImageResolution model = new ImageResolution();
                model = Getresoluion();
                int height = Convert.ToInt32(model.height);
                int width = Convert.ToInt32(model.width);
                string prodid = hdnprodID.Value;
                foreach (HttpPostedFile uploadedFile in file_product.PostedFiles)
                    try
                    {
                        uploadedFile.SaveAs(Server.MapPath("~/CommonFolder/ProductImages/") +
                                            uploadedFile.FileName);

                        FileUploadedList.Text += "File name: " +
                           uploadedFile.FileName + "<br>" +
                           uploadedFile.ContentLength + " kb<br>" +
                           "Content type: " + uploadedFile.ContentType + "<br><br>";

                        Bitmap OriginalBM = new Bitmap(Server.MapPath("~/CommonFolder/ProductImages/" + uploadedFile.FileName));

                        if (width == 0)
                        {

                            width = 400;
                        }

                        if(height==0)
                        {

                            height = width;
                        }
                        Size newSize = new Size(width, height);
                        Bitmap ResizedBM = new Bitmap(OriginalBM, newSize);

                        string FileName = prodid + Guid.NewGuid() + uploadedFile.FileName;

                        ResizedBM.Save(Server.MapPath("~/CommonFolder/ProductImages/" + FileName), ImageFormat.Jpeg);
                        ResizedBM.Dispose();
                        OriginalBM.Dispose();
                        var filePath = Server.MapPath("~/CommonFolder/ProductImages/" + uploadedFile.FileName);
                        if (File.Exists(filePath))
                        {
                            File.Delete(filePath);
                        }

                        dtproducts.Rows.Add(prodid,FileName,chkmainimage.Checked,chkActivestatus.Checked);

                    }
                    catch (Exception ex)
                    {
                        FileUploadedList.Text = "ERROR: " + ex.Message.ToString();
                    }

                ProductMultipleImage(dtproducts, User, prodid);

            }
            else
            {
                FileUploadedList.Text = "You have not specified a file.";
            }

            ScriptManager.RegisterClientScriptBlock(this, GetType(), "none", "<script>Productimagepopulate(" + hdnprodID.Value + ");</script>", false);
        }

        public ImageResolution Getresoluion()
        {
            ImageResolution model = new ImageResolution();
            ProcedureExecute proc = new ProcedureExecute("Sp_GetProductImages");
            proc.AddPara("@Action", "Resolution");
            DataTable dtresolution = proc.GetTable();
            if (dtresolution.Rows.Count > 0)
            {
                model = APIHelperMethods.ToModel<ImageResolution>(dtresolution);
            }
            else
            {
                model.width = "0";
                model.height = "0";
            }
            return model;
        }

        public class products
        {
            public string prod_image { get; set; }
            public long product_id { get; set; }
        }

        public class ImageResolution
        {
            public string width { get; set; }
            public string height { get; set; }
        }

        public DataTable CreateTempTable(string Type)
        {

            DataTable dt = new DataTable();

           if (Type == "MultipleImage")
            {
                dt.Columns.Add("ProdId", typeof(string));
                dt.Columns.Add("Image", typeof(string));
                dt.Columns.Add("Main_image", typeof(bool));
                dt.Columns.Add("ActiveStatus", typeof(bool));
      
            }


            return dt;
        }

        public DataTable ProductMultipleImage(DataTable dtjournaldetails, string User,string prodID)
        {

            ProcedureExecute proc = new ProcedureExecute("Sp_ProductImagesave");
            proc.AddPara("@Createduser", User);
            proc.AddPara("@tbl_ProductImage", dtjournaldetails);
            proc.AddPara("@Action", "ProductImagesSave");
            proc.AddPara("@ProdID", prodID);
            DataTable dtproduct = proc.GetTable();
            return dtproduct;

        }

        protected void gridProductImage_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            string WhichCall = Convert.ToString(e.Parameters).Split('~')[0];
            DataTable dtproduct = new DataTable();
            if (WhichCall == "ProductImagePopulate")
            {
                string product = Convert.ToString(e.Parameters).Split('~')[1];
                dtproduct=Populateimage(product);

                if (dtproduct != null && dtproduct.Rows.Count > 0)
                {

                    gridProductimage.DataSource = dtproduct;
                    Session["Productimagelist"] = dtproduct;
                    gridProductimage.DataBind();

                }
                else
                {

                    gridProductimage.DataSource = null;
                    gridProductimage.DataBind();

                }


            }

        }

        protected void gridProductImage_DataBinding(object sender, EventArgs e)
        {
            if(Session["Productimagelist"] !=null)
            {

                gridProductimage.DataSource = (DataTable)Session["Productimagelist"];
            
            }

        }


        [WebMethod]
        public static List<products> DeleteImage(string ImageID)
        {
            List<products> omodel = new List<products>();
            try
            {
                ProcedureExecute proc = new ProcedureExecute("Sp_GetProductImages");
                proc.AddPara("@ImageID", ImageID);
                proc.AddPara("@Action", "DeleteImage");
                DataTable dtproduct = proc.GetTable();

                if (dtproduct.Rows.Count > 0)
                {
                    var filePath = HttpContext.Current.Server.MapPath("~/CommonFolder/ProductImages/" + Convert.ToString(dtproduct.Rows[0]["image_path"]));
                    if (File.Exists(filePath))
                    {
                        File.Delete(filePath);
                    }

                }
                return omodel;
            }
            catch
            {

                return omodel;
            }
         
        }


        [WebMethod]
        public static string Mainstatus(string ImageID, string prodId)
        {
            ProcedureExecute proc = new ProcedureExecute("Sp_GetProductImages");
            proc.AddPara("@ImageID", ImageID);
            proc.AddPara("@ProdID", prodId);
            proc.AddPara("@Action", "Mainstatus");
            DataTable dtproduct = proc.GetTable();

            if (dtproduct.Rows.Count > 0)
            {
                return "Success";
            }
            else
            {
                return "Failure";
            }
        }

        [WebMethod]
        public static string  ChangeActivity(string ImageID)
        {
            ProcedureExecute proc = new ProcedureExecute("Sp_GetProductImages");
            proc.AddPara("@ImageID", ImageID);
            proc.AddPara("@Action", "ChangeActivity");
            DataTable dtproduct = proc.GetTable();

            if (dtproduct.Rows.Count > 0)
            {
                return "Success";
            }
            else
            {
                return "Failure";
            }
        }


        protected void image2_Init(object sender, EventArgs e)
        {

            ASPxImage image = (ASPxImage)sender;
            GridViewDataItemTemplateContainer container = (GridViewDataItemTemplateContainer)image.NamingContainer;

            int value = int.Parse(gridProductimage.GetRowValuesByKeyValue(container.KeyValue, "ID").ToString());


            ProcedureExecute proc = new ProcedureExecute("Sp_GetProductImages");
            proc.AddPara("@ImageID", value);
            proc.AddPara("@Action", "GetImage");
            DataTable dtproduct = proc.GetTable();

            if (dtproduct !=null && dtproduct.Rows.Count > 0)
            {
                image.ImageUrl = "~/CommonFolder/ProductImages/" + Convert.ToString(dtproduct.Rows[0]["image_path"]);
            }


        }

    }
}