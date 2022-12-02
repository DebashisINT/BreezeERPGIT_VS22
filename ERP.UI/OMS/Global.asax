<%@ Application Language="C#" %>

<script runat="server">

    void Application_Start(object sender, EventArgs e) 
    {
         // Code that runs on application startup
        System.Reflection.PropertyInfo p = typeof(System.Web.HttpRuntime).GetProperty("FileChangesMonitor",System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Public | 	System.Reflection.BindingFlags.Static);
        object o = p.GetValue(null, null);
        System.Reflection.FieldInfo f = o.GetType().GetField("_dirMonSubdirs", System.Reflection.BindingFlags.Instance | 	System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.IgnoreCase);
        object monitor = f.GetValue(o);
        System.Reflection.MethodInfo m = monitor.GetType().GetMethod("StopMonitoring", 	System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
        m.Invoke(monitor, new object[] { }); 

    }
    
    void Application_End(object sender, EventArgs e) 
    {
        //  Code that runs on application shutdown

    }
    void Application_Error(object sender, EventArgs e)
    {
        HttpException lastErrorWrapper = Server.GetLastError() as HttpException;
        Exception lastError = lastErrorWrapper;
        if (lastErrorWrapper.InnerException != null)
        {
            lastError = lastErrorWrapper.InnerException;
            string url = "";
            string page = "";
            string lastErrorMessage = "";
            string lastErrorStackTrace = "";
            string CompanyName = "";
            string username = "";
            string segmentname = "";
            string procname = "";
            string linenum = "";
            string message = "";
            string servername = "";
            string lastErrorTypeName = lastError.GetType().ToString();
            if (lastErrorTypeName.ToLower().Contains("sqlexception"))
            {
                System.Data.SqlClient.SqlException sqex = lastError as System.Data.SqlClient.SqlException;
                lastErrorMessage = sqex.Message.ToString();
                procname = sqex.Procedure.ToString();
                linenum = sqex.LineNumber.ToString();
                message = sqex.Message.ToString();
                servername = sqex.Server.ToString();
                lastErrorStackTrace = sqex.StackTrace.ToString();
                CompanyName = Convert.ToString(Session["LastCompany"]);
                username = Convert.ToString(Session["username"]);
                segmentname = Convert.ToString(Session["Segmentname"]);
                if (HttpContext.Current != null)
                {
                    url = HttpContext.Current.Request.Url.ToString();
                    page = (HttpContext.Current.Handler as System.Web.UI.Page).ToString();
                }
                CreateXML_AppError objerror = new CreateXML_AppError();
                objerror.createorappend(lastErrorTypeName, lastErrorMessage, url, page, lastErrorStackTrace, CompanyName, username, segmentname, procname, linenum, servername);
            }
            else
            {
                lastErrorMessage = lastError.Message;
                lastErrorStackTrace = lastError.StackTrace;
                CompanyName = Convert.ToString(Session["LastCompany"]);
                username = Convert.ToString(Session["username"]);
                segmentname = Convert.ToString(Session["Segmentname"]);
                if (HttpContext.Current != null)
                {
                    url = HttpContext.Current.Request.Url.ToString();
                    page = (HttpContext.Current.Handler as System.Web.UI.Page).ToString();
                }
                CreateXML_AppError objerror = new CreateXML_AppError();
                objerror.createorappend(lastErrorTypeName, lastErrorMessage, url, page, lastErrorStackTrace, CompanyName, username, segmentname, procname, linenum, servername);
            }
        }
    }

    void Session_Start(object sender, EventArgs e) 
    {
        // Code that runs when a new session is started
        Session.Timeout = 90000;
    }
    void Application_Disposed(object sender, EventArgs e)
    {
        
    }
    void Session_End(object sender, EventArgs e) 
    {
        // Code that runs when a session ends. 
        // Note: The Session_End event is raised only when the sessionstate mode
        // is set to InProc in the Web.config file. If session mode is set to StateServer 
        // or SQLServer, the event is not raised.
       HttpContext.Current.Response.Redirect("~/Login.aspx", false);
    }    
</script>
