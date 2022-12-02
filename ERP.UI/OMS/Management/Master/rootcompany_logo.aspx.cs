using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Data.SqlClient;
//using DevExpress.Web.ASPxTabControl;
using DevExpress.Web;
using BusinessLogicLayer;
using ClsDropDownlistNameSpace;

namespace ERP.OMS.Management.Master
{

    public partial class management_master_rootcompany_logo : ERP.OMS.ViewState_class.VSPage
    {
        BusinessLogicLayer.FileUploadBl upldObj = new FileUploadBl();
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(string.Empty);

        protected void Page_Load(object sender, EventArgs e)
        {
            SetImage();
        }
        private void SetImage()
        {
            string InternalId = Convert.ToString(HttpContext.Current.Session["KeyVal_InternalID"]);

            string[] filePath = oDBEngine.GetFieldValue1("tbl_master_company", "cmp_bigLogo,cmp_smallLogo", "cmp_internalid='" + InternalId + "'", 2);
             
                CompImageBig.ImageUrl = filePath[0];
            
             
                CompImageSmall.ImageUrl = filePath[1];
             
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            string ProductFilePath = "", ProductFilePathSmall="";
          string InternalId=Convert.ToString( HttpContext.Current.Session["KeyVal_InternalID"]);

          string[] filePath = oDBEngine.GetFieldValue1("tbl_master_company", "cmp_bigLogo,cmp_smallLogo", "cmp_internalid='" + InternalId+"'", 2);
          ProductFilePath = filePath[0];
            ProductFilePathSmall = filePath[1];
            // Upload big Logo
            string fileName = upldBigLogo.UploadedFiles[0].FileName;
            if (fileName.Trim() != "")
            {
                if (filePath[0] != "")
                {
                    if ((System.IO.File.Exists(Server.MapPath(filePath[0]))))
                    {
                        System.IO.File.Delete(Server.MapPath(filePath[0]));
                    }
                }

                string name = fileName.Substring(0, fileName.IndexOf('.'));
                string exten = fileName.Substring(fileName.IndexOf('.'), fileName.Length - fileName.IndexOf('.'));
                ProductFilePath = "/CommonFolderErpCRM/CompanyBigLogo/" + name + Guid.NewGuid() + exten;
                upldBigLogo.UploadedFiles[0].SaveAs(Server.MapPath(ProductFilePath));
            }
           
            //upload small logo
            string fileNameSmall = upldSmallLogo.UploadedFiles[0].FileName;
            if (fileNameSmall.Trim() != "")
            {
                if (filePath[1] != "")
                {
                    if ((System.IO.File.Exists(Server.MapPath(filePath[1]))))
                    {
                        System.IO.File.Delete(Server.MapPath(filePath[1]));
                    }
                }

                string nameSmall = fileNameSmall.Substring(0, fileNameSmall.IndexOf('.'));
                string extenSmall = fileNameSmall.Substring(fileNameSmall.IndexOf('.'), fileNameSmall.Length - fileNameSmall.IndexOf('.'));

                ProductFilePathSmall = "/CommonFolderErpCRM/CompanySmallLogo/" + nameSmall + Guid.NewGuid() + extenSmall;
                upldSmallLogo.UploadedFiles[0].SaveAs(Server.MapPath(ProductFilePathSmall));

            }
                upldObj.CompanyLogoUpload(ProductFilePath, ProductFilePathSmall, InternalId);

                SetImage();
        }
          
        protected void btnBigLogoDelete_Click(object sender, EventArgs e)
        {
            string ProductFilePath = "", ProductFilePathSmall = "";
            string InternalId = Convert.ToString(HttpContext.Current.Session["KeyVal_InternalID"]);
            string[] filePath = oDBEngine.GetFieldValue1("tbl_master_company", "cmp_bigLogo,cmp_smallLogo", "cmp_internalid='" + InternalId + "'", 2);

            // Delete big Logo
             
                if (filePath[0] != "")
                {
                    if ((System.IO.File.Exists(Server.MapPath(filePath[0]))))
                    {
                        System.IO.File.Delete(Server.MapPath(filePath[0]));
                    }
                }

                if (filePath[1].Trim() != "")
                {
                    ProductFilePathSmall = filePath[1].Trim();
                }
                upldObj.CompanyLogoUpload("", ProductFilePathSmall, InternalId);

                SetImage();
        }

        protected void btnSmallLogoDelete_Click(object sender, EventArgs e)
        {
            string ProductFilePath = "", ProductFilePathSmall = "";
            string InternalId = Convert.ToString(HttpContext.Current.Session["KeyVal_InternalID"]);
            string[] filePath = oDBEngine.GetFieldValue1("tbl_master_company", "cmp_bigLogo,cmp_smallLogo", "cmp_internalid='" + InternalId + "'", 2);

            // delete small Logo

            if (filePath[1] != "")
            {
                if ((System.IO.File.Exists(Server.MapPath(filePath[1]))))
                {
                    System.IO.File.Delete(Server.MapPath(filePath[1]));
                }
            }

            if (filePath[0].Trim() != "")
            {
                ProductFilePathSmall = filePath[0].Trim();
            }
            upldObj.CompanyLogoUpload("", ProductFilePathSmall, InternalId);

            SetImage();
        }
      

        
    }
}