using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.IO;
using System.Data;
namespace BusinessLogicLayer
{
    public class XmlReadWriteBL
    {
        public string WriteNewCompanyToXml(string companyName, string dbName)
        {

            string output = "";
            try
            {
                if (companyName != "" && dbName != "")
                {
                    string XmlFileName = ConfigurationSettings.AppSettings["XMLConnectionFileName"];

                    XmlDocument XDOC = new XmlDocument();

                    string path = AppDomain.CurrentDomain.BaseDirectory + (XmlFileName + ".xml");


                    XDOC.Load(path);

                    DataTable dtCon = XmlRead();

                    int maxColumn = Convert.ToInt32(dtCon.Compute("min([id])", string.Empty));

                    
                   // xmlDoc.LoadXml(@"<NonFuel><Desc>Non-Fuel</Desc><Description></Description><Quantity/><Amount/><Additional/><Dispute>0</Dispute></NonFuel>");

                   //XmlNode nonFuel = xmlDoc.SelectSingleNode("//ConnectionString");
                   // XmlNode dispute = xmlDoc.SelectSingleNode("//Company");


                    XmlNode RootNode = XDOC.SelectSingleNode("//ConnectionString");
                    XmlNode TestChild = XDOC.CreateNode(XmlNodeType.Element, "Company", null);

                    XmlNode TestName = XDOC.CreateNode(XmlNodeType.Element, "name", null);
                    TestName.InnerText = companyName;
                    TestChild.AppendChild(TestName);

                    XmlNode TestdbName = XDOC.CreateNode(XmlNodeType.Element, "dbName", null);
                    TestdbName.InnerText = dbName;
                    TestChild.AppendChild(TestdbName);
                    RootNode.AppendChild(TestChild);
                    XDOC.Save(path);





                    //XmlNode xmlRecordNo = xmlDoc.CreateNode(XmlNodeType.Element, "name", null);
                    //xmlRecordNo.InnerText = companyName;
                    //nonFuel.InsertAfter(xmlRecordNo, dispute);

                    //XmlNode xmldbName = xmlDoc.CreateNode(XmlNodeType.Element, "dbName", null);
                    //xmldbName.InnerText = dbName;
                    //nonFuel.InsertAfter(xmlRecordNo, dispute);

                }

            }
            catch (Exception ex)
            {

            }

            return output;

        }
        public DataTable XmlRead()
        {
            try
            {
                string XmlFileName = ConfigurationSettings.AppSettings["XMLConnectionFileName"];
                XmlDocument Docs = new XmlDocument();
                string path = AppDomain.CurrentDomain.BaseDirectory + (XmlFileName + ".xml");
                Docs.Load(path);
               // Docs.Load("XMLconnectionString.xml");
                XmlElement root = Docs.DocumentElement;
                DataTable dt = new DataTable();

                dt.Columns.Add("id",typeof(Int32));
                dt.Columns.Add("name");
                dt.Columns.Add("dbName");
                //dt.Columns.Add("dateColumn");
                XmlNodeList nodes = root.SelectNodes("//ConnectionString/Company");
                foreach (XmlNode node in nodes)
                {
                    dt.Rows.Add( Convert.ToInt32(node["id"].InnerText), node["name"].InnerText.ToString(),
                                node["dbName"].InnerText.ToString()
                                //node["dateColumn"].InnerText.ToString()
                               );
                }
                return dt;
            }
            catch (Exception es)
            {
                throw es;
            }
        }

    }
}
