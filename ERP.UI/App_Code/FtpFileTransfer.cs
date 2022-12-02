using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Net;
using System.IO;


public class FtpFileTransfer

{
    
    
    public void uploadFileUsingFTPn(string CompleteFTPPath, string CompleteLocalPath, string UName, string PWD)
    {
        //Create a FTP Request Object and Specfiy a Complete Path
        FileInfo fs = new FileInfo(CompleteLocalPath);
        String DirName = fs.DirectoryName;
        FtpWebRequest reqObj = (FtpWebRequest)WebRequest.Create(CompleteFTPPath);
        //Call A FileUpload Method of FTP Request Object

        reqObj.Method = WebRequestMethods.Ftp.UploadFile;

        //If you want to access Resourse Protected You need to give User Name and PWD

        reqObj.Credentials = new NetworkCredential(UName, PWD);

        reqObj.KeepAlive = false;
        reqObj.UseBinary = true;
        //FileStream object read file from Local Drive
        FileStream fsnew = fs.OpenRead();
        reqObj.ContentLength = fs.Length;
        // int buffLength = 500447300;
        byte[] buff = new byte[fsnew.Length + 1];
        // byte[] buff = new byte[streamObj.Length + 1];
        int contentLen;
        // Opens a file stream (System.IO.FileStream) to read the file
        // to be uploaded

        try
        {
            // Stream to which the file to be upload is written
            Stream strm = reqObj.GetRequestStream();
            // Read from the file stream 2kb at a time
            contentLen = fsnew.Read(buff, 0, buff.Length);
            while (contentLen != 0)
            {

                strm.Write(buff, 0, contentLen);
                contentLen = fsnew.Read(buff, 0, buff.Length);
            }

            strm.Close();
            fsnew.Close();
            strm = null;
            fsnew = null;
            reqObj = null;

        }
        catch (Exception ex)
        {

            fsnew.Close();

            fsnew = null;
            reqObj = null;

        }
    }
  
}  
