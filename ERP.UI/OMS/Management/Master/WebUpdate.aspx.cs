using System;
using System.Web;
//using DevExpress.Web;
using DevExpress.Web;
using System.Configuration;
using ClsDropDownlistNameSpace;
using EntityLayer.CommonELS;
using System.Collections.Generic;
using System.Data;
using System.Web.Services;
using System.IO;
using System.IO.Compression;


namespace ERP.OMS.Management.Master
{
    public partial class WebUpdate : ERP.OMS.ViewState_class.VSPage
    {
        private static String path, path1, FileName, s, time, cannotParse;
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
        public EntityLayer.CommonELS.UserRightsForPage rights = new UserRightsForPage();
        protected void Page_Load(object sender, EventArgs e)
        {
            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/management/Master/category.aspx");

            if (!IsPostBack)
            {
            }
        }

        protected void BtnSaveexcel_Click(object sender, EventArgs e)
        {
            string FilePath = "";
            string fName = string.Empty;
            Boolean HasLog = false;
            path = String.Empty;
            path1 = String.Empty;
            FileName = String.Empty;
            s = String.Empty;
            time = String.Empty;
            cannotParse = String.Empty;

            BusinessLogicLayer.TransctionDescription td = new BusinessLogicLayer.TransctionDescription();

            FilePath = Path.GetFullPath(OFDBankSelect.PostedFile.FileName);
            FileName = Path.GetFileName(FilePath);
            string fileExtension = Path.GetExtension(FileName);

            string _FilePath = @"D:\Priti\"+FilePath;

            string targetFolder = @"D:\Priti\TestDB";

            ZipFile.ExtractToDirectory(_FilePath, targetFolder);
           
        }
    }
}