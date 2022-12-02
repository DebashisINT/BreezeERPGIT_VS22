using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

/// <summary>
/// Summary description for NsdlClientSignatureTextInfo
/// </summary>
public class NsdlClientSignatureTextInfo
{
    private string Doc_ContactId = String.Empty;
    private string ref_No = String.Empty;
    private string Holder_Indicator = String.Empty;
    private string Authorized_Signatory_Name = String.Empty;
    private string Doc_Name = String.Empty;
    private string Create_Modify = String.Empty;

	public NsdlClientSignatureTextInfo()
	{
		//
		// TODO: Add constructor logic here
		//
	}
    public NsdlClientSignatureTextInfo(string Doc_ContactId, string ref_No, string Holder_Indicator, string Authorized_Signatory_Name, string Doc_Name, string Create_Modify)
    {
        this.Doc_ContactId = Doc_ContactId;
        this.ref_No = ref_No;
        this.Holder_Indicator = Holder_Indicator;
        this.Authorized_Signatory_Name = Authorized_Signatory_Name;
        this.Doc_Name = Doc_Name;
        this.Create_Modify = Create_Modify;
    }
    public string DocContactId
    {
        get
        {
            return Doc_ContactId;
        }
        set
        {
            Doc_ContactId = value;
        }
    }
    public string RefNo
    {
        get
        {
            return ref_No;
        }
        set
        {
            ref_No = value;
        }

    }
    public string HolderIndicator
    {
        get
        {
            return Holder_Indicator;
        }
        set
        {
            Holder_Indicator = value;
        }

    }
    public string AuthorizedSignatoryName
    {
        get
        {
            return Authorized_Signatory_Name;
        }
        set
        {
            Authorized_Signatory_Name = value;
        }

    }
    
    public string DocName
    {
        get
        {
            return Doc_Name;
        }
        set
        {
            Doc_Name = value;
        }

    }
    public string CreateModify
    {
        get
        {
            return Create_Modify;
        }
        set
        {
            Create_Modify = value;
        }

    }
}
