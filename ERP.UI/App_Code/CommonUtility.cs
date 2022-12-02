using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Web.Mail;
using System.Text;
using System.IO;
using System.Data.SqlClient;
using System.Drawing.Imaging;
using System.Runtime.Serialization;
using Ionic.Zip;
using DataAccessLayer;

/// <summary>
/// Summary description for CommonUtility
/// </summary>
public class CommonUtility
{
    ProcedureExecute proc = new ProcedureExecute();
    BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();

    public CommonUtility()
    {
        
    }

    #region DigitalSignature


    public string DigitalCertificate(String tempPdfPath, String signPath, String password,
                                         String signReason, String signLocation, String CompanyID,
                                              String Segment_OR_DPID, String DocumentType, String Signatory,
                                                  String RecipientEmailID, String strDate, String BranchID,
                                                      String VirtualPath, String signPdfPath,
                                                          String user, String LastFinYear, int EmailCreateAppMenuId)
    {
        bool isUsed;
        int numberofPages;
        string filename, certrificatesStatus;
        filename = tempPdfPath.Substring(tempPdfPath.LastIndexOf('\\') + 1);
        isUsed = false;

        try
        {
            string zipfilepath = signPdfPath;
            string zipfilename = filename.Split('.')[0];
            string compresstotalpath = "";
            signPdfPath = signPdfPath + "\\" + filename;

            if (File.Exists(signPdfPath))
            {
                isUsed = IsFileLocked(signPdfPath);
            }

            if (!isUsed)
            {
                //if (password.Length > 0)
                //{
                Aspose.Pdf.Kit.Certificate cert = new Aspose.Pdf.Kit.Certificate(signPath, password);

                Aspose.Pdf.Kit.PdfFileInfo pf = new Aspose.Pdf.Kit.PdfFileInfo(tempPdfPath);
                Aspose.Pdf.Kit.PdfFileSignature pdfSign = new Aspose.Pdf.Kit.PdfFileSignature(cert);

                numberofPages = pf.NumberofPages;
                pdfSign.BindPdf(tempPdfPath);
                pdfSign.Sign(numberofPages, signReason, "success", signLocation, true, new System.Drawing.Rectangle(500, 49, 300, 48));

                pdfSign.Save(signPdfPath);

                compresstotalpath = zipfilepath + "\\" + zipfilename + ".zip";
                using (ZipFile zip = new ZipFile())
                {
                    zip.CompressionLevel = Ionic.Zlib.CompressionLevel.BestCompression;
                    zip.AddFile(signPdfPath, zipfilename);
                    zip.Save(compresstotalpath);
                }
                //}
                filename = filename.Replace("pdf", "zip");

                if (File.Exists(tempPdfPath))
                {
                    File.Delete(tempPdfPath);
                }
                if (File.Exists(signPdfPath))
                {
                    File.Delete(signPdfPath);
                }
                int j = 0;

                string[] str = oDBEngine.GetFieldValue1("tbl_master_segment", "seg_name", "seg_id=" + HttpContext.Current.Session["userlastsegment"], 1);
                //if ((str[0].ToString() == "ICEX-COMM")  )
                //{
                //    j = oDBEngine.insertSignedDocument(CompanyID, Segment_OR_DPID, str[0].ToString(), strDate,
                //                                        BranchID, filename.Split('-').GetValue(4).ToString(), DocumentType,
                //                                        filename, VirtualPath + "/" + filename, Signatory, RecipientEmailID, LastFinYear, user, EmailCreateAppMenuId);
                //}
                if ((str[0].ToString() == "ICEX-COMM") || (str[0].ToString() == "NSE-CM") || (str[0].ToString() == "NSE-FO") || (str[0].ToString() == "BSE-CM") || (str[0].ToString() == "NSE-CDX") || (str[0].ToString() == "BSE-CDX") || (str[0].ToString() == "MCXSX-CDX") || (str[0].ToString() == "USE-CDX") || (str[0].ToString() == "MCX-COMM") || (str[0].ToString() == "NSEL-SPOT") || (str[0].ToString() == "NCDEX-COMM") || (str[0].ToString() == "BSE-FO") || (str[0].ToString() == "MCXSX-FO") || (str[0].ToString() == "MCXSX-CM") || (str[0].ToString() == "UCX-COMM"))
                {
                    if (RecipientEmailID != "")
                    {
                        j = oDBEngine.insertSignedDocument(CompanyID, Segment_OR_DPID, str[0].ToString(), strDate,
                                        BranchID, filename.Split('-').GetValue(4).ToString(), DocumentType,
                                        filename, VirtualPath + "/" + filename, Signatory, RecipientEmailID, LastFinYear, user, EmailCreateAppMenuId);
                    }
                    else
                    {
                        j = 1;
                    }

                }
                else
                {
                    j = oDBEngine.insertSignedDocument(CompanyID, Segment_OR_DPID, str[0].ToString(), strDate,
                                    BranchID, filename.Split('-').GetValue(3).ToString(), DocumentType,
                                    filename, VirtualPath + "/" + filename, Signatory, RecipientEmailID, LastFinYear, user, EmailCreateAppMenuId);

                }

                if (j != 0)
                {
                    certrificatesStatus = "Success";
                }
                else
                {
                    if (File.Exists(signPdfPath))
                    {
                        File.Delete(signPdfPath);
                    }
                    certrificatesStatus = "Cannot Generate Signed Document.";
                }
            }
            else
            {
                certrificatesStatus = "File Path is being Used by another Process.";
                if (File.Exists(tempPdfPath))
                {
                    File.Delete(tempPdfPath);
                }
            }
        }
        catch (Exception e)
        {

            certrificatesStatus = e.Message;
        }




        return certrificatesStatus;
    }
    public string DigitalCertificate1(String tempPdfPath, String signPath, String password,
                                        String signReason, String signLocation, String CompanyID,
                                             String Segment_OR_DPID, String DocumentType, String Signatory,
                                                 String RecipientEmailID, String strDate, String strDate2, String BranchID,
                                                     String VirtualPath, String signPdfPath,
                                                         String user, String LastFinYear, int EmailCreateAppMenuId)
    {
        bool isUsed;
        int numberofPages;
        string filename, certrificatesStatus;
        filename = tempPdfPath.Substring(tempPdfPath.LastIndexOf('\\') + 1);
        isUsed = false;

        try
        {
            string zipfilepath = signPdfPath;
            string zipfilename = filename.Split('.')[0];
            string compresstotalpath = "";
            signPdfPath = signPdfPath + "\\" + filename;

            if (File.Exists(signPdfPath))
            {
                isUsed = IsFileLocked(signPdfPath);
            }

            if (!isUsed)
            {
                Aspose.Pdf.Kit.Certificate cert = new Aspose.Pdf.Kit.Certificate(signPath, password);

                Aspose.Pdf.Kit.PdfFileInfo pf = new Aspose.Pdf.Kit.PdfFileInfo(tempPdfPath);
                Aspose.Pdf.Kit.PdfFileSignature pdfSign = new Aspose.Pdf.Kit.PdfFileSignature(cert);

                numberofPages = pf.NumberofPages;
                pdfSign.BindPdf(tempPdfPath);
                pdfSign.Sign(numberofPages, signReason, "success", signLocation, true, new System.Drawing.Rectangle(500, 49, 300, 48));

                pdfSign.Save(signPdfPath);


                compresstotalpath = zipfilepath + "\\" + zipfilename + ".zip";
                using (ZipFile zip = new ZipFile())
                {

                    zip.CompressionLevel = Ionic.Zlib.CompressionLevel.BestCompression;
                    zip.AddFile(signPdfPath, zipfilename);
                    zip.Save(compresstotalpath);
                }
                //}
                filename = filename.Replace("pdf", "zip");

                if (File.Exists(tempPdfPath))
                {
                    File.Delete(tempPdfPath);
                }
                if (File.Exists(signPdfPath))
                {
                    File.Delete(signPdfPath);
                }
                int j = 0;

                string[] str = oDBEngine.GetFieldValue1("tbl_master_segment", "seg_name", "seg_id=" + HttpContext.Current.Session["userlastsegment"], 1);
                //if ((str[0].ToString() == "ICEX-COMM"))
                //{
                //    j = oDBEngine.insertSignedDocument1(CompanyID, Segment_OR_DPID, str[0].ToString(), strDate,strDate2,
                //                                        BranchID, filename.Split('-').GetValue(4).ToString(), DocumentType,
                //                                        filename, VirtualPath + "/" + filename, Signatory, RecipientEmailID, LastFinYear, user, EmailCreateAppMenuId);
                //}
                if ((str[0].ToString() == "ICEX-COMM") || (str[0].ToString() == "NSE-CM") || (str[0].ToString() == "NSE-FO") || (str[0].ToString() == "BSE-CM") || (str[0].ToString() == "NSE-CDX") || (str[0].ToString() == "BSE-CDX") || (str[0].ToString() == "MCXSX-CDX") || (str[0].ToString() == "USE-CDX") || (str[0].ToString() == "MCX-COMM") || (str[0].ToString() == "NSEL-SPOT") || (str[0].ToString() == "NCDEX-COMM") || (str[0].ToString() == "BSE-FO") || (str[0].ToString() == "MCXSX-FO") || (str[0].ToString() == "MCXSX-CM") || (str[0].ToString() == "UCX-COMM"))
                {
                    if (RecipientEmailID != "")
                    {
                        j = oDBEngine.insertSignedDocument1(CompanyID, Segment_OR_DPID, str[0].ToString(), strDate, strDate2,
                                        BranchID, filename.Split('-').GetValue(4).ToString(), DocumentType,
                                        filename, VirtualPath + "/" + filename, Signatory, RecipientEmailID, LastFinYear, user, EmailCreateAppMenuId);
                    }
                    else
                    {
                        j = 1;
                    }

                }
                else
                {
                    j = oDBEngine.insertSignedDocument1(CompanyID, Segment_OR_DPID, str[0].ToString(), strDate, strDate2,
                                    BranchID, filename.Split('-').GetValue(3).ToString(), DocumentType,
                                    filename, VirtualPath + "/" + filename, Signatory, RecipientEmailID, LastFinYear, user, EmailCreateAppMenuId);

                }

                if (j != 0)
                {
                    certrificatesStatus = "Success";
                }
                else
                {
                    if (File.Exists(signPdfPath))
                    {
                        File.Delete(signPdfPath);
                    }
                    certrificatesStatus = "Cannot Generate Signed Document.";
                }
            }
            else
            {
                certrificatesStatus = "File Path is being Used by another Process.";
                if (File.Exists(tempPdfPath))
                {
                    File.Delete(tempPdfPath);
                }
            }
        }
        catch (Exception e)
        {

            certrificatesStatus = e.Message;
        }




        return certrificatesStatus;
    }
    public bool IsFileLocked(String Fullpath)
    {
        FileStream fs = null;

        try
        {
            fs = new FileStream(Fullpath, FileMode.Open, FileAccess.ReadWrite, FileShare.None);
        }
        catch
        {
            return true;
        }
        finally
        {
            if (fs != null)
                fs.Close();
        }

        //file is not locked 
        return false;


    }

    #endregion

}
