using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ERP.OMS.Management.Master
{
    public partial class VersionUpload : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        [WebMethod]
        public static void UploadVersion(HttpContext con)
        {
        }

        protected void Unnamed_Click(object sender, EventArgs e)
        {
            string strFileName = System.IO.Path.GetFileName(upUserCtrl.PostedFile.FileName);

            try
            {

                if (strFileName != "")
                {

                    //print progressbar

                    PrintProgressBar();

                    string path = Server.MapPath("~/Backup") + upUserCtrl.FileName;

                    upUserCtrl.PostedFile.SaveAs(path);

                    FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read);
                    byte[] data = new byte[fs.Length];
                    fs.Read(data, 0, data.Length);
                    fs.Close();

                    // Generate post objects
                    Dictionary<string, object> postParameters = new Dictionary<string, object>();
                    postParameters.Add("name", "YourEmail");
                    postParameters.Add("whatsnew", "YourPassword");
                    postParameters.Add("versiondate", "YourPassword");
                    postParameters.Add("zip_file[zip]", new FileParameter(data, upUserCtrl.FileName, "application/zip"));

                    // Create request and receive response
                    string postURL = "http://localhost:58426/Upload/FileUpload";
                    string userAgent = "Whatever you like";
                    HttpWebResponse webResponse = MultipartFormDataPost(postURL, userAgent, postParameters);

                    // Process response
                    string status = "";
                    if (webResponse.StatusCode == HttpStatusCode.OK)
                    {
                        status = "OK";
                    }
                    if (webResponse.StatusCode == HttpStatusCode.Forbidden)
                    {
                        status = "Forbidden";
                    }
                    StreamReader responseReader = new StreamReader(webResponse.GetResponseStream());
                    string fullResponse = responseReader.ReadToEnd();
                    webResponse.Close();
                    string sEvent = fullResponse;

                    Thread.Sleep(2000);

                    ClearProgressBar();

                }

            }

            catch (Exception ex)
            {

                Response.Write(ex.Message);

            }
        }
        public static void PrintProgressBar()
        {

            StringBuilder sb = new StringBuilder();

            sb.Append("<div id='updiv' style='Font-weight:bold;font-size:11pt;Left:320px;COLOR:black;font-family:verdana;Position:absolute;Top:140px;Text-Align:center;'>");

            sb.Append("&nbsp;<script> var up_div=document.getElementById('updiv');up_div.innerText='';</script>");

            sb.Append("<script language=javascript>");

            sb.Append("var dts=0; var dtmax=10;");

            sb.Append("function ShowWait(){var output;output='Please wait while uploading!';dts++;if(dts>=dtmax)dts=1;");

            sb.Append("for(var x=0;x < dts; x++){output+='';}up_div.innerText=output;up_div.style.color='red';}");

            sb.Append("function StartShowWait(){up_div.style.visibility='visible';ShowWait();window.setInterval('ShowWait()',100);}");

            sb.Append("StartShowWait();</script>");

            HttpContext.Current.Response.Write(sb.ToString());

            HttpContext.Current.Response.Flush();

        }

        public static void ClearProgressBar()
        {

            StringBuilder sbc = new StringBuilder();

            sbc.Append("<script language='javascript'>");

            sbc.Append("alert('Upload process completed successfully!');");

            sbc.Append("up_div.style.visibility='hidden';");

            sbc.Append("history.go(-1)");

            sbc.Append("</script>");

            HttpContext.Current.Response.Write(sbc);

        }


        //private static readonly Encoding encoding = Encoding.UTF8;
        //public static HttpWebResponse MultipartFormPost(string postUrl, string userAgent, Dictionary<string, object> postParameters, string headerkey, string headervalue)
        //{
        //    string formDataBoundary = String.Format("----------{0:N}", Guid.NewGuid());
        //    string contentType = "multipart/form-data; boundary=" + formDataBoundary;

        //    byte[] formData = GetMultipartFormData(postParameters, formDataBoundary);

        //    return PostForm(postUrl, userAgent, contentType, formData, headerkey, headervalue);
        //}
        //private static HttpWebResponse PostForm(string postUrl, string userAgent, string contentType, byte[] formData, string headerkey, string headervalue)
        //{
        //    HttpWebRequest request = WebRequest.Create(postUrl) as HttpWebRequest;

        //    if (request == null)
        //    {
        //        throw new NullReferenceException("request is not a http request");
        //    }

        //    // Set up the request properties.  
        //    request.Method = "POST";
        //    request.ContentType = contentType;
        //    request.UserAgent = userAgent;
        //    request.CookieContainer = new CookieContainer();
        //    request.ContentLength = formData.Length;

        //    // You could add authentication here as well if needed:  
        //    // request.PreAuthenticate = true;  
        //    // request.AuthenticationLevel = System.Net.Security.AuthenticationLevel.MutualAuthRequested;  

        //    //Add header if needed  
        //    request.Headers.Add(headerkey, headervalue);

        //    // Send the form data to the request.  
        //    using (Stream requestStream = request.GetRequestStream())
        //    {
        //        requestStream.Write(formData, 0, formData.Length);
        //        requestStream.Close();
        //    }

        //    return request.GetResponse() as HttpWebResponse;
        //}

        //private static byte[] GetMultipartFormData(Dictionary<string, object> postParameters, string boundary)
        //{
        //    Stream formDataStream = new System.IO.MemoryStream();
        //    bool needsCLRF = false;

        //    foreach (var param in postParameters)
        //    {

        //        if (needsCLRF)
        //            formDataStream.Write(encoding.GetBytes("\r\n"), 0, encoding.GetByteCount("\r\n"));

        //        needsCLRF = true;

        //        if (param.Value is FileParameter) // to check if parameter if of file type   
        //        {
        //            FileParameter fileToUpload = (FileParameter)param.Value;

        //            // Add just the first part of this param, since we will write the file data directly to the Stream  
        //            string header = string.Format("--{0}\r\nContent-Disposition: form-data; name=\"{1}\"; filename=\"{2}\"\r\nContent-Type: {3}\r\n\r\n",
        //                boundary,
        //                param.Key,
        //                fileToUpload.FileName ?? param.Key,
        //                fileToUpload.ContentType ?? "application/octet-stream");

        //            formDataStream.Write(encoding.GetBytes(header), 0, encoding.GetByteCount(header));

        //            // Write the file data directly to the Stream, rather than serializing it to a string.  
        //            formDataStream.Write(fileToUpload.File, 0, fileToUpload.File.Length);
        //        }
        //        else
        //        {
        //            string postData = string.Format("--{0}\r\nContent-Disposition: form-data; name=\"{1}\"\r\n\r\n{2}",
        //                boundary,
        //                param.Key,
        //                param.Value);
        //            formDataStream.Write(encoding.GetBytes(postData), 0, encoding.GetByteCount(postData));
        //        }
        //    }

        //    // Add the end of the request.  Start with a newline  
        //    string footer = "\r\n--" + boundary + "--\r\n";
        //    formDataStream.Write(encoding.GetBytes(footer), 0, encoding.GetByteCount(footer));

        //    // Dump the Stream into a byte[]  
        //    formDataStream.Position = 0;
        //    byte[] formData = new byte[formDataStream.Length];
        //    formDataStream.Read(formData, 0, formData.Length);
        //    formDataStream.Close();

        //    return formData;
        //}
        //public  byte[] ReadAllBytes(Stream instream)
        //{
        //    if (instream is MemoryStream)
        //        return ((MemoryStream)instream).ToArray();

        //    using (var memoryStream = new MemoryStream())
        //    {
        //        instream.CopyTo(memoryStream);
        //        return memoryStream.ToArray();
        //    }
        //}


        private static readonly Encoding encoding = Encoding.UTF8;
        public static HttpWebResponse MultipartFormDataPost(string postUrl, string userAgent, Dictionary<string, object> postParameters)
        {
            string formDataBoundary = String.Format("----------{0:N}", Guid.NewGuid());
            string contentType = "multipart/form-data; boundary=" + formDataBoundary;

            byte[] formData = GetMultipartFormData(postParameters, formDataBoundary);

            return PostForm(postUrl, userAgent, contentType, formData);
        }
        private static HttpWebResponse PostForm(string postUrl, string userAgent, string contentType, byte[] formData)
        {
            HttpWebRequest request = WebRequest.Create(postUrl) as HttpWebRequest;

            if (request == null)
            {
                throw new NullReferenceException("request is not a http request");
            }

            // Set up the request properties.
            request.Method = "POST";
            request.ContentType = contentType;
            request.UserAgent = userAgent;
            request.CookieContainer = new CookieContainer();
            request.ContentLength = formData.Length;

            // You could add authentication here as well if needed:
            // request.PreAuthenticate = true;
            // request.AuthenticationLevel = System.Net.Security.AuthenticationLevel.MutualAuthRequested;
            // request.Headers.Add("Authorization", "Basic " + Convert.ToBase64String(System.Text.Encoding.Default.GetBytes("username" + ":" + "password")));

            // Send the form data to the request.
            using (Stream requestStream = request.GetRequestStream())
            {
                requestStream.Write(formData, 0, formData.Length);
                requestStream.Close();
            }

            return request.GetResponse() as HttpWebResponse;
        }

        private static byte[] GetMultipartFormData(Dictionary<string, object> postParameters, string boundary)
        {
            Stream formDataStream = new System.IO.MemoryStream();
            bool needsCLRF = false;

            foreach (var param in postParameters)
            {
                // Add a CRLF to allow multiple parameters to be added.
                // Skip it on the first parameter, add it to subsequent parameters.
                if (needsCLRF)
                    formDataStream.Write(encoding.GetBytes("\r\n"), 0, encoding.GetByteCount("\r\n"));

                needsCLRF = true;

                if (param.Value is FileParameter)
                {
                    FileParameter fileToUpload = (FileParameter)param.Value;

                    // Add just the first part of this param, since we will write the file data directly to the Stream
                    string header = string.Format("--{0}\r\nContent-Disposition: form-data; name=\"{1}\"; filename=\"{2}\"\r\nContent-Type: {3}\r\n\r\n",
                        boundary,
                        param.Key,
                        fileToUpload.FileName ?? param.Key,
                        fileToUpload.ContentType ?? "application/octet-stream");

                    formDataStream.Write(encoding.GetBytes(header), 0, encoding.GetByteCount(header));

                    // Write the file data directly to the Stream, rather than serializing it to a string.
                    formDataStream.Write(fileToUpload.File, 0, fileToUpload.File.Length);
                }
                else
                {
                    string postData = string.Format("--{0}\r\nContent-Disposition: form-data; name=\"{1}\"\r\n\r\n{2}",
                        boundary,
                        param.Key,
                        param.Value);
                    formDataStream.Write(encoding.GetBytes(postData), 0, encoding.GetByteCount(postData));
                }
            }

            // Add the end of the request.  Start with a newline
            string footer = "\r\n--" + boundary + "--\r\n";
            formDataStream.Write(encoding.GetBytes(footer), 0, encoding.GetByteCount(footer));

            // Dump the Stream into a byte[]
            formDataStream.Position = 0;
            byte[] formData = new byte[formDataStream.Length];
            formDataStream.Read(formData, 0, formData.Length);
            formDataStream.Close();

            return formData;
        }

        public class FileParameter
        {
            public byte[] File { get; set; }
            public string FileName { get; set; }
            public string ContentType { get; set; }
            public FileParameter(byte[] file) : this(file, null) { }
            public FileParameter(byte[] file, string filename) : this(file, filename, null) { }
            public FileParameter(byte[] file, string filename, string contenttype)
            {
                File = file;
                FileName = filename;
                ContentType = contenttype;
            }
        }
       

    
public  string returnResponseText { get; set; }}
    public class NameValue
    {
        public string name { get; set; }
        public string value { get; set; }
    }

}