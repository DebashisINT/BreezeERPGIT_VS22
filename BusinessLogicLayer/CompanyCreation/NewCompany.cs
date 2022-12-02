using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.CompanyCreation
{
    public class NewCompany
    {
        public DataTable GetNewCompanyData(string Action, string Level, string constring)
        {

            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("Prc_CompanyCreation");
            proc.Connection = new System.Data.SqlClient.SqlConnection(constring);
            proc.AddVarcharPara("@Action", 100, Action);
            //proc.AddVarcharPara("@masterdbname", -1, masterdbname);
            proc.AddVarcharPara("@level", 100, Level);
            dt = proc.GetTable();
            return dt;
        }

        public DataTable SaveNewCompanyData(string Action, string Level, string constring, string DB_NAME, string cmp_Name, string PARENTID,DateTime start_dt,DateTime end_dt)
        {


            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("Prc_CompanyCreation");
            proc.Connection = new System.Data.SqlClient.SqlConnection(constring);
            proc.AddVarcharPara("@Action", 100, Action);
            proc.AddVarcharPara("@cmp_Name", 100, cmp_Name);
            proc.AddVarcharPara("@PARENTID", 100, PARENTID);
            proc.AddVarcharPara("@DB_NAME", 200, DB_NAME);
            proc.AddVarcharPara("@level", 100, Level);
            proc.AddPara("@start_dt",  start_dt);
            proc.AddPara("@end_dt",  end_dt);
            dt = proc.GetTable();
            return dt;
        }

        public void UpdateCompanyEncodeCode(string Action, string CompanyCode, string Encode_String, string constring)
        {


            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("Prc_CompanyCreation");
            proc.Connection = new System.Data.SqlClient.SqlConnection(constring);
            proc.AddVarcharPara("@Action", 100, Action);
            proc.AddVarcharPara("@CompanyCode", 100, CompanyCode);
            proc.AddVarcharPara("@Encode_String", 200, Encode_String);
            dt = proc.GetTable();

        }

        public DataTable GetDatabasFromCompanyCode(string Action, string CompanyCode, string constring)
        {


            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("Prc_CompanyCreation");
            proc.Connection = new System.Data.SqlClient.SqlConnection(constring);
            proc.AddVarcharPara("@Action", 100, Action);
            proc.AddVarcharPara("@CompanyCode", 100, CompanyCode);
            dt = proc.GetTable();
            return dt;
        }

        public DataTable GetCompanyList(string Action, string constring)
        {


            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("Prc_CompanyCreation");
            proc.Connection = new System.Data.SqlClient.SqlConnection(constring);
            proc.AddVarcharPara("@Action", 100, Action);
            dt = proc.GetTable();
            return dt;
        }

        public DataTable CompanyIfExists(string Action, string constring,string DbName)
        {


            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("Prc_CompanyCreation");
            proc.Connection = new System.Data.SqlClient.SqlConnection(constring);
            proc.AddVarcharPara("@Action", 100, Action);
            proc.AddVarcharPara("@DB_NAME", 100, DbName);
            dt = proc.GetTable();
            return dt;
        }

        public string Encrypt(string textToEncrypt)
        {
            try
            {
                string ToReturn = "";
                string _key = "ay$a5%&jwrtmnh;lasjdf98787";
                string _iv = "abc@98797hjkas$&asd(*$%";
                byte[] _ivByte = { };
                _ivByte = System.Text.Encoding.UTF8.GetBytes(_iv.Substring(0, 8));
                byte[] _keybyte = { };
                _keybyte = System.Text.Encoding.UTF8.GetBytes(_key.Substring(0, 8));
                MemoryStream ms = null;
                CryptoStream cs = null;
                byte[] inputbyteArray = System.Text.Encoding.UTF8.GetBytes(textToEncrypt);
                using (DESCryptoServiceProvider des = new DESCryptoServiceProvider())
                {
                    ms = new MemoryStream();
                    cs = new CryptoStream(ms, des.CreateEncryptor(_keybyte, _ivByte), CryptoStreamMode.Write);
                    cs.Write(inputbyteArray, 0, inputbyteArray.Length);
                    cs.FlushFinalBlock();
                    ToReturn = Convert.ToBase64String(ms.ToArray());
                }
                return ToReturn;
            }
            catch (Exception ae)
            {
                throw new Exception(ae.Message, ae.InnerException);
            }
        }
        public string Decrypt(string textToDecrypt)
        {
            try
            {
                string ToReturn = "";
                string _key = "ay$a5%&jwrtmnh;lasjdf98787";
                string _iv = "abc@98797hjkas$&asd(*$%";
                byte[] _ivByte = { };
                _ivByte = System.Text.Encoding.UTF8.GetBytes(_iv.Substring(0, 8));
                byte[] _keybyte = { };
                _keybyte = System.Text.Encoding.UTF8.GetBytes(_key.Substring(0, 8));
                MemoryStream ms = null; CryptoStream cs = null;
                byte[] inputbyteArray = new byte[textToDecrypt.Replace(" ", "+").Length];
                inputbyteArray = Convert.FromBase64String(textToDecrypt.Replace(" ", "+"));
                using (DESCryptoServiceProvider des = new DESCryptoServiceProvider())
                {
                    ms = new MemoryStream();
                    cs = new CryptoStream(ms, des.CreateDecryptor(_keybyte, _ivByte), CryptoStreamMode.Write);
                    cs.Write(inputbyteArray, 0, inputbyteArray.Length);
                    cs.FlushFinalBlock();
                    Encoding encoding = Encoding.UTF8;
                    ToReturn = encoding.GetString(ms.ToArray());
                }
                return ToReturn;
            }
            catch (Exception ae)
            {
                return "Decryption Failed";
            }
        }

        public DataTable CreateDb(string DbName, string sqlConnectionString)
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("NEWDBBACKUPANDRESTORE");
            proc.Connection = new System.Data.SqlClient.SqlConnection(sqlConnectionString);
            proc.AddVarcharPara("@DbName", 100, DbName);
            dt = proc.GetTable();
            return dt;
        }
    }
}
