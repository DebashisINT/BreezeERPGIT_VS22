using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Xml;

/// <summary>
/// Summary description for CreateXML_AppError
/// </summary>
public class CreateXML_AppError
{
	public CreateXML_AppError()
	{
		//
		// TODO: Add constructor logic here
		//
	}
    private void createNode(string lastErrorTypeName, string lastErrorMessage,string url, string page,string stacktrace,string companyname,string username,string segmentname, XmlTextWriter writer)
    {
        //parent node start
        //writer.WriteStartElement(lastErrorTypeName + "Error");
        //Error Type Name
        writer.WriteStartElement("Error_Type_Name");
        writer.WriteString(lastErrorTypeName);
        writer.WriteEndElement(); 
        //Error Message
        writer.WriteStartElement("Error_Message");
        writer.WriteString(lastErrorMessage);
        writer.WriteEndElement();   
        //Page
        writer.WriteStartElement("Page");
        writer.WriteString(page);
        writer.WriteEndElement();
        //URL
        writer.WriteStartElement("URL");
        writer.WriteString(url);
        writer.WriteEndElement();
        //CompanyName
        writer.WriteStartElement("Company_Name");
        writer.WriteString(companyname);
        writer.WriteEndElement();
        //UserName
        writer.WriteStartElement("User_Name");
        writer.WriteString(username);
        writer.WriteEndElement();
        //SegmentName
        writer.WriteStartElement("Segment_Name");
        writer.WriteString(segmentname);
        writer.WriteEndElement();
        //Stack Trace
        writer.WriteStartElement("Stack_Trace");
        writer.WriteString(stacktrace);
        writer.WriteEndElement(); 
    }

    private void createNodeSQlExcep(string lastErrorTypeName, string lastErrorMessage, string url, string page, string stacktrace, string companyname, string username, string segmentname,string procname,string linenum,string servername, XmlTextWriter writer)
    {
        //parent node start
        //writer.WriteStartElement(lastErrorTypeName + "Error");
        //Error Type Name
        writer.WriteStartElement("Error_Type_Name");
        writer.WriteString(lastErrorTypeName);
        writer.WriteEndElement();
        //Error Message
        writer.WriteStartElement("Error_Message");
        writer.WriteString(lastErrorMessage);
        writer.WriteEndElement();        
        //ProcName
        writer.WriteStartElement("Procedure_Name");
        writer.WriteString(procname);
        writer.WriteEndElement();
        //LineNumber
        writer.WriteStartElement("Line_Number");
        writer.WriteString(linenum);
        writer.WriteEndElement();
        //ServerName
        writer.WriteStartElement("Server_Name");
        writer.WriteString(servername);
        writer.WriteEndElement();
        //Page
        writer.WriteStartElement("Page");
        writer.WriteString(page);
        writer.WriteEndElement();
        //URL
        writer.WriteStartElement("URL");
        writer.WriteString(url);
        writer.WriteEndElement();
        //CompanyName
        writer.WriteStartElement("Company_Name");
        writer.WriteString(companyname);
        writer.WriteEndElement();
        //UserName
        writer.WriteStartElement("User_Name");
        writer.WriteString(username);
        writer.WriteEndElement();
        //SegmentName
        writer.WriteStartElement("Segment_Name");
        writer.WriteString(segmentname);
        writer.WriteEndElement();
        //Stack Trace
        writer.WriteStartElement("Stack_Trace");
        writer.WriteString(stacktrace);
        writer.WriteEndElement();
    }


    public void createorappend(string errortype,string errormsg, string url, string page,string stacktrace,string companyname,string username,string segmentname,string procname,string linenum,string servername)
    {
        if (errortype.ToLower().Contains("sqlexception"))
        {
            #region Sql Exception
            //Check if file exists       
            string filepath = System.Web.HttpContext.Current.Server.MapPath("~/XMLErrorFiles/" + "Error_" + companyname + ".xml");
            if (File.Exists(filepath))
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(filepath);
                //XmlElement el11 = doc.CreateElement(errortype);                
                XmlElement el0 = doc.CreateElement("Error_Type_Name");
                el0.InnerText = errortype;
                XmlElement el1 = doc.CreateElement("Error_Message");
                el1.InnerText = errormsg;
                XmlElement el2 = doc.CreateElement("Procedure_Name");
                el2.InnerText = procname;
                XmlElement el3 = doc.CreateElement("Line_Number");
                el3.InnerText = linenum;
                XmlElement el4 = doc.CreateElement("Server_Name");
                el4.InnerText = servername;
                XmlElement el5 = doc.CreateElement("Page");
                el5.InnerText = page;
                XmlElement el6 = doc.CreateElement("URL");
                el6.InnerText = url;
                XmlElement el7 = doc.CreateElement("Company_Name");
                el7.InnerText = companyname;
                XmlElement el8 = doc.CreateElement("User_Name");
                el8.InnerText = username;
                XmlElement el9 = doc.CreateElement("Segment_Name");
                el9.InnerText = segmentname;
                XmlElement el10 = doc.CreateElement("Stack_Trace");
                el10.InnerText = stacktrace;
                //doc.DocumentElement.AppendChild(el11);
                doc.DocumentElement.AppendChild(el0);
                doc.DocumentElement.AppendChild(el1);
                doc.DocumentElement.AppendChild(el2);
                doc.DocumentElement.AppendChild(el3);
                doc.DocumentElement.AppendChild(el4);
                doc.DocumentElement.AppendChild(el5);
                doc.DocumentElement.AppendChild(el6);
                doc.DocumentElement.AppendChild(el7);
                doc.DocumentElement.AppendChild(el8);
                doc.DocumentElement.AppendChild(el9);
                doc.DocumentElement.AppendChild(el10);
                doc.Save(filepath);
            }
            else
            {
                //Start writer
                XmlTextWriter writer = new XmlTextWriter(filepath, System.Text.Encoding.UTF8);
                //Start XM DOcument
                writer.WriteStartDocument(true);
                writer.Formatting = Formatting.Indented;
                writer.Indentation = 2;
                //ROOT Element
                writer.WriteStartElement("Errors");
                //call create nodes method
                createNodeSQlExcep(errortype, errormsg, url, page, stacktrace, companyname, username, segmentname,procname,linenum,servername, writer);
                writer.WriteEndElement();
                //End XML Document
                writer.WriteEndDocument();
                //Close writer
                writer.Close();
            }
            #endregion
        }
        else
        {
            #region General Exception
            //Check if file exists       
            string filepath = System.Web.HttpContext.Current.Server.MapPath("~/XMLErrorFiles/" + "Error_" + companyname + ".xml");
            if (File.Exists(filepath))
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(filepath);
                //XmlElement el0 = doc.CreateElement(errortype); 
                XmlElement el1 = doc.CreateElement("Error_Type_Name");
                el1.InnerText = errortype;
                XmlElement el2 = doc.CreateElement("Error_Message");
                el2.InnerText = errormsg;
                XmlElement el3 = doc.CreateElement("Page");
                el3.InnerText = page;
                XmlElement el4 = doc.CreateElement("URL");
                el4.InnerText = url;
                XmlElement el5 = doc.CreateElement("Company_Name");
                el5.InnerText = companyname;
                XmlElement el6 = doc.CreateElement("User_Name");
                el6.InnerText = username;
                XmlElement el7 = doc.CreateElement("Segment_Name");
                el7.InnerText = segmentname;
                XmlElement el8 = doc.CreateElement("Stack_Trace");
                el8.InnerText = stacktrace;
                //doc.DocumentElement.AppendChild(el0);
                doc.DocumentElement.AppendChild(el1);
                doc.DocumentElement.AppendChild(el2);
                doc.DocumentElement.AppendChild(el3);
                doc.DocumentElement.AppendChild(el4);
                doc.DocumentElement.AppendChild(el5);
                doc.DocumentElement.AppendChild(el6);
                doc.DocumentElement.AppendChild(el7);
                doc.DocumentElement.AppendChild(el8);
                doc.Save(filepath);
            }
            else
            {
                //Start writer
                XmlTextWriter writer = new XmlTextWriter(filepath, System.Text.Encoding.UTF8);
                //Start XM DOcument
                writer.WriteStartDocument(true);
                writer.Formatting = Formatting.Indented;
                writer.Indentation = 2;
                //ROOT Element
                writer.WriteStartElement("Errors");
                //call create nodes method
                createNode(errortype, errormsg, url, page, stacktrace, companyname, username, segmentname, writer);
                writer.WriteEndElement();
                //End XML Document
                writer.WriteEndDocument();
                //Close writer
                writer.Close();
            }
            #endregion
        }
    }
}