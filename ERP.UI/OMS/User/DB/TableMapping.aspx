<%@ Page Language="C#" AutoEventWireup="true" Inherits="ERP.OMS.User.DB.DP_User_DB_Cdsl_TableMapping_New" Codebehind="TableMapping.aspx.cs" %>

<%@ Register Assembly="DevExpress.Web.v10.2" Namespace="DevExpress.Web.ASPxCallbackPanel"
    TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v10.2" Namespace="DevExpress.Web.ASPxPanel"
    TagPrefix="dxp" %>
<%@ Register Assembly="DevExpress.Web.v10.2" Namespace="DevExpress.Web.ASPxEditors"
    TagPrefix="dxe000001" %>
<%@ Register Assembly="DevExpress.Web.v10.2" Namespace="DevExpress.Web.ASPxEditors"
    TagPrefix="dx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1">
    <title>Master Mapping</title>
    <link href="../../CentralData/CSS/GenericAjaxStyle.css" rel="stylesheet" type="text/css" />
    <link type="text/css" href="../../CentralData/CSS/GenericCss.css" rel="Stylesheet" />

    <script type="text/javascript" src="../../CentralData/JSScript/GenericJScript.js"></script>

    <script type="text/javascript" src="../../CentralData/JSScript/GenericAjaxList.js"></script>

    <script type="text/javascript" src="../../CentralData/JSScript/init.js"></script>

    <script type="text/javascript" src="../../CentralData/JSScript/jquery-1.3.2.js"></script>

    <script language="javascript" type="text/javascript">   
    tabcontent='Geographical';    
    KRATabContent='AddressProof'; 
    function isNumber(n) 
    {
      return !isNaN(parseFloat(n)) && isFinite(n);
    }
    function IsNumeric(input)
    {
        return (input - 0) == input && (input+'').replace(/^\s+|\s+$/g, "").length > 0;
    }   
    function PageLoad()
    {
        GetObjectID('Cdsl_Info').style.display='none';               
        GetObjectID('KRA_Info').style.display='none';
        clientbtn.SetEnabled(false);
        cBtnKRADone.SetEnabled(false);       
        Height('460','460');      
    }    
    function fn_divCdslTab(obj)
    {       
        if(obj=="1")
        {
            tabcontent='Geographical';
            GetObjectID('Geographical').className ='active_Tab';    
            GetObjectID('AnnualIncome').className ='inActive_Tab';    
            GetObjectID('Nationality').className ='inActive_Tab';    
            GetObjectID('Education').className ='inActive_Tab';
            GetObjectID('LegalStatus').className ='inActive_Tab';
            GetObjectID('Language').className ='inActive_Tab'; 
            GetObjectID('PanExemption').className ='inActive_Tab'; 
            GetObjectID('CdslOccupation').className ='inActive_Tab'; 
            GetObjectID('Currency').className ='inActive_Tab'; 
            GetObjectID('<%=txtpkcg.ClientID %>').value='';
            GetObjectID('<%=txtpkcg_hidden.ClientID %>').value='';
            GetObjectID('<%=txtcdsl.ClientID %>').value=''; 
            GetObjectID('<%=txtcdsl_hidden.ClientID %>').value='';                              
            cCbpContainer.PerformCallback("Geographical^");            
        }
        else if(obj=="2")
        {
            tabcontent='AnnualIncome';
            GetObjectID('Geographical').className ='inActive_Tab';    
            GetObjectID('AnnualIncome').className ='active_Tab';    
            GetObjectID('Nationality').className ='inActive_Tab';    
            GetObjectID('Education').className ='inActive_Tab';
            GetObjectID('LegalStatus').className ='inActive_Tab';
            GetObjectID('Language').className ='inActive_Tab';   
            GetObjectID('PanExemption').className ='inActive_Tab'; 
            GetObjectID('CdslOccupation').className ='inActive_Tab'; 
            GetObjectID('Currency').className ='inActive_Tab'; 
            GetObjectID('<%=txtpkcg.ClientID %>').value='';
            GetObjectID('<%=txtcdsl.ClientID %>').value=''; 
            GetObjectID('<%=txtpkcg_hidden.ClientID %>').value='';
            GetObjectID('<%=txtcdsl_hidden.ClientID %>').value='';          
            cCbpContainer.PerformCallback("AnnualIncome^");            
        }    
        else if(obj=="3")
        { 
            tabcontent='Nationality';
            GetObjectID('Geographical').className ='inActive_Tab';    
            GetObjectID('AnnualIncome').className ='inActive_Tab';    
            GetObjectID('Nationality').className ='active_Tab';    
            GetObjectID('Education').className ='inActive_Tab'; 
            GetObjectID('LegalStatus').className ='inActive_Tab';
            GetObjectID('Language').className ='inActive_Tab';  
            GetObjectID('PanExemption').className ='inActive_Tab'; 
            GetObjectID('CdslOccupation').className ='inActive_Tab'; 
            GetObjectID('Currency').className ='inActive_Tab'; 
            GetObjectID('<%=txtpkcg.ClientID %>').value='';
            GetObjectID('<%=txtcdsl.ClientID %>').value='';
            GetObjectID('<%=txtpkcg_hidden.ClientID %>').value='';
            GetObjectID('<%=txtcdsl_hidden.ClientID %>').value='';          
            cCbpContainer.PerformCallback("Nationality^");            
        }   
        else if(obj=="4")
        {
            tabcontent='Education';
            GetObjectID('Geographical').className ='inActive_Tab';    
            GetObjectID('AnnualIncome').className ='inActive_Tab';    
            GetObjectID('Nationality').className ='inActive_Tab';    
            GetObjectID('Education').className ='active_Tab'; 
            GetObjectID('LegalStatus').className ='inActive_Tab';
            GetObjectID('Language').className ='inActive_Tab'; 
            GetObjectID('PanExemption').className ='inActive_Tab'; 
            GetObjectID('CdslOccupation').className ='inActive_Tab'; 
            GetObjectID('Currency').className ='inActive_Tab'; 
            GetObjectID('<%=txtpkcg.ClientID %>').value='';
            GetObjectID('<%=txtcdsl.ClientID %>').value='';
            GetObjectID('<%=txtpkcg_hidden.ClientID %>').value='';
            GetObjectID('<%=txtcdsl_hidden.ClientID %>').value='';                       
            cCbpContainer.PerformCallback("Education^");
        }        
        else if(obj=="5")
        {
            tabcontent='LegalStatus';
            GetObjectID('Geographical').className ='inActive_Tab';    
            GetObjectID('AnnualIncome').className ='inActive_Tab';    
            GetObjectID('Nationality').className ='inActive_Tab';    
            GetObjectID('Education').className ='inActive_Tab'; 
            GetObjectID('LegalStatus').className ='active_Tab';
            GetObjectID('Language').className ='inActive_Tab';
            GetObjectID('PanExemption').className ='inActive_Tab'; 
            GetObjectID('CdslOccupation').className ='inActive_Tab'; 
            GetObjectID('Currency').className ='inActive_Tab'; 
            GetObjectID('<%=txtpkcg.ClientID %>').value='';
            GetObjectID('<%=txtcdsl.ClientID %>').value='';
            GetObjectID('<%=txtpkcg_hidden.ClientID %>').value='';
            GetObjectID('<%=txtcdsl_hidden.ClientID %>').value='';                       
            cCbpContainer.PerformCallback("LegalStatus^");
        }        
        else if(obj=="6")
        {
            tabcontent='Language';
            GetObjectID('Geographical').className ='inActive_Tab';    
            GetObjectID('AnnualIncome').className ='inActive_Tab';    
            GetObjectID('Nationality').className ='inActive_Tab';    
            GetObjectID('Education').className ='inActive_Tab'; 
            GetObjectID('LegalStatus').className ='inActive_Tab';
            GetObjectID('Language').className ='active_Tab'; 
            GetObjectID('PanExemption').className ='inActive_Tab'; 
            GetObjectID('CdslOccupation').className ='inActive_Tab'; 
            GetObjectID('Currency').className ='inActive_Tab'; 
            GetObjectID('<%=txtpkcg.ClientID %>').value='';
            GetObjectID('<%=txtcdsl.ClientID %>').value='';
            GetObjectID('<%=txtpkcg_hidden.ClientID %>').value='';
            GetObjectID('<%=txtcdsl_hidden.ClientID %>').value='' ;                     
            cCbpContainer.PerformCallback("Language^");
        }
        else if(obj=="7")
        {
            tabcontent='PanExemption';
            GetObjectID('Geographical').className ='inActive_Tab';    
            GetObjectID('AnnualIncome').className ='inActive_Tab';    
            GetObjectID('Nationality').className ='inActive_Tab';    
            GetObjectID('Education').className ='inActive_Tab'; 
            GetObjectID('LegalStatus').className ='inActive_Tab';
            GetObjectID('Language').className ='inActive_Tab'; 
            GetObjectID('PanExemption').className ='active_Tab'; 
            GetObjectID('CdslOccupation').className ='inActive_Tab'; 
            GetObjectID('Currency').className ='inActive_Tab'; 
            GetObjectID('<%=txtpkcg.ClientID %>').value='';
            GetObjectID('<%=txtcdsl.ClientID %>').value='';
            GetObjectID('<%=txtpkcg_hidden.ClientID %>').value='';
            GetObjectID('<%=txtcdsl_hidden.ClientID %>').value='' ;                     
            cCbpContainer.PerformCallback("PanExemption^");
        }else if(obj=="8")
        {
            tabcontent='CdslOccupation';
            GetObjectID('Geographical').className ='inActive_Tab';    
            GetObjectID('AnnualIncome').className ='inActive_Tab';    
            GetObjectID('Nationality').className ='inActive_Tab';    
            GetObjectID('Education').className ='inActive_Tab'; 
            GetObjectID('LegalStatus').className ='inActive_Tab';
            GetObjectID('Language').className ='inActive_Tab'; 
            GetObjectID('PanExemption').className ='inActive_Tab'; 
            GetObjectID('CdslOccupation').className ='active_Tab'; 
            GetObjectID('Currency').className ='inActive_Tab'; 
            GetObjectID('<%=txtpkcg.ClientID %>').value='';
            GetObjectID('<%=txtcdsl.ClientID %>').value='';
            GetObjectID('<%=txtpkcg_hidden.ClientID %>').value='';
            GetObjectID('<%=txtcdsl_hidden.ClientID %>').value='' ;                     
            cCbpContainer.PerformCallback("CdslOccupation^");
        }
        else if(obj=="9")
        {
            tabcontent='Currency';
            GetObjectID('Geographical').className ='inActive_Tab';    
            GetObjectID('AnnualIncome').className ='inActive_Tab';    
            GetObjectID('Nationality').className ='inActive_Tab';    
            GetObjectID('Education').className ='inActive_Tab'; 
            GetObjectID('LegalStatus').className ='inActive_Tab';
            GetObjectID('Language').className ='inActive_Tab'; 
            GetObjectID('PanExemption').className ='inActive_Tab'; 
            GetObjectID('CdslOccupation').className ='inActive_Tab'; 
            GetObjectID('Currency').className ='active_Tab'; 
            GetObjectID('<%=txtpkcg.ClientID %>').value='';
            GetObjectID('<%=txtcdsl.ClientID %>').value='';
            GetObjectID('<%=txtpkcg_hidden.ClientID %>').value='';
            GetObjectID('<%=txtcdsl_hidden.ClientID %>').value='' ;                     
            cCbpContainer.PerformCallback("Currency^");
        }
    }
    function Btndone_Click()
    {
        var geographyperform='';
        var cdslperform='';
        var TextToCompare="No Record Found";
        
        if (GetObjectID('<%=txtpkcg.ClientID %>').value!='')
            geographyperform=GetObjectID('<%=txtpkcg_hidden.ClientID %>').value.split('~')[0];
        if (GetObjectID('<%=txtcdsl.ClientID %>').value!='')
            cdslperform=GetObjectID('<%=txtcdsl_hidden.ClientID %>').value.split('~')[0];
       
        if(Control_Empty("<%=txtpkcg.ClientID %>","Please Select Mapping Value!!!"))
            if(Control_Empty("<%=txtcdsl.ClientID %>","Please Select CDSL Static Code!!!"))
                if(Control_CompareText("<%=txtpkcg.ClientID %>",TextToCompare,"Please Select Mapping Value Properly!!!"))
                    if(Control_CompareText("<%=txtcdsl.ClientID %>",TextToCompare,"Please Select CDSL Static Code Properly!!!"))
                        cCbpContainer.PerformCallback("update"+"^"+geographyperform+"^"+cdslperform+"^"+tabcontent);
    
    }
    function CbpContainer_EndCallBack()
    {       
        GetObjectID('Cdsl_Info').style.display='none';        
        if(cCbpContainer.cpAutoFillCDSL!=undefined)
        { 
            if(cCbpContainer.cpAutoFillCDSL=='T')
            {
                if(cCbpContainer.cpAutoFillCDSLData!=undefined)
                {
                     var selCDSLTab=cCbpContainer.cpAutoFillCDSLData.split('^')[0];                    
                     var CDSLContent= cCbpContainer.cpAutoFillCDSLData.split('^')[1];
                     GetObjectID('Geographical').className ='inActive_Tab';    
                     GetObjectID('AnnualIncome').className ='inActive_Tab';    
                     GetObjectID('Nationality').className ='inActive_Tab';    
                     GetObjectID('Education').className ='inActive_Tab';
                     GetObjectID('LegalStatus').className ='inActive_Tab';
                     GetObjectID('Language').className ='inActive_Tab';
                     GetObjectID('PanExemption').className ='inActive_Tab';
                     GetObjectID('CdslOccupation').className ='inActive_Tab';
                     GetObjectID('Currency').className ='inActive_Tab';
                     GetObjectID(selCDSLTab).className ='active_Tab';                     
                          
                     SetValue('<%=txtcdsl_hidden.ClientID %>',CDSLContent);
                     var staticCDSLData=new Array();
                     staticCDSLData=CDSLContent.split('~');                    
                     SetValue('<%=txtcdsl.ClientID %>',staticCDSLData[1]);                     
                     ctxtCdslCode.SetText(staticCDSLData[2] );
                }
            }
            else if(cCbpContainer.cpAutoFillCDSL=='F')
                alert('Mapped CDSL Result Not Found. Please Map Data Again.');           
        }
        cCbpContainer.cpAutoFillCDSL=null;
        cCbpContainer.cpAutoFillCDSLData=null;
        if(cCbpContainer.cpResult!=null)
        {        
            var splitendcallback=cCbpContainer.cpResult.split('~')[0];
            var splitendcallback1=cCbpContainer.cpResult.split('~')[1];            
            if(splitendcallback1=='Successfully Update')
            {        
                clientbtn.SetEnabled(false);    
                GetObjectID('Cdsl_Info').style.display='none';
                GetObjectID('<%=txtpkcg.ClientID %>').value='';
                GetObjectID('<%=txtcdsl.ClientID %>').value='';                 
                GetObjectID('<%=txtpkcg_hidden.ClientID %>').value='';
                GetObjectID('<%=txtcdsl_hidden.ClientID %>').value='';
                GetObjectID('Geographical').className ='inActive_Tab';    
                GetObjectID('AnnualIncome').className ='inActive_Tab';    
                GetObjectID('Nationality').className ='inActive_Tab';    
                GetObjectID('Education').className ='inActive_Tab';
                GetObjectID('LegalStatus').className ='inActive_Tab';
                GetObjectID('Language').className ='inActive_Tab';
                GetObjectID('PanExemption').className ='inActive_Tab';
                GetObjectID('CdslOccupation').className ='inActive_Tab';
                GetObjectID('Currency').className ='inActive_Tab';
                GetObjectID(splitendcallback).className ='active_Tab'; 
                alert('Successfully Updated');                 
                cCbpContainer.PerformCallback(splitendcallback+"^");
            }             
        } 
        cCbpContainer.cpResult = null;
    }
    </script>

    <script language="javascript" type="text/javascript">
    function fn_divKRATab(obj)
    {
        if(obj=="1")
        {
            KRATabContent='AddressProof';
            GetObjectID('AddressProof').className ='active_Tab';    
            GetObjectID('IdentityProof').className ='inActive_Tab';    
            GetObjectID('PanExemptCategory').className ='inActive_Tab';    
            GetObjectID('IndividualStatus').className ='inActive_Tab';
            GetObjectID('NRIStatusProof').className ='inActive_Tab';
            GetObjectID('NonIndividualStatus').className ='inActive_Tab'; 
            GetObjectID('Occupation').className ='inActive_Tab'; 
            GetObjectID('PoliticalConnection').className ='inActive_Tab'; 
            GetObjectID('Maritalstatus').className ='inActive_Tab'; 
            GetObjectID('State').className ='inActive_Tab'; 
            GetObjectID('Country').className ='inActive_Tab'; 
            GetObjectID('ContactType').className ='inActive_Tab'; 
            GetObjectID('KRANationality').className ='inActive_Tab'; 
            GetObjectID('KRAAnnualIncome').className ='inActive_Tab'; 
            GetObjectID('<%=txtKRApkcg.ClientID %>').value='';
            GetObjectID('<%=txtKRA.ClientID %>').value=''; 
            GetObjectID('<%=txtKRApkcg_hidden.ClientID %>').value='';
            GetObjectID('<%=txtKRA_hidden.ClientID %>').value='';
        }
        else if(obj=="2")
        {
            KRATabContent='IdentityProof';
            GetObjectID('AddressProof').className ='inActive_Tab';    
            GetObjectID('IdentityProof').className ='active_Tab';    
            GetObjectID('PanExemptCategory').className ='inActive_Tab';    
            GetObjectID('IndividualStatus').className ='inActive_Tab';
            GetObjectID('NRIStatusProof').className ='inActive_Tab';
            GetObjectID('NonIndividualStatus').className ='inActive_Tab'; 
            GetObjectID('Occupation').className ='inActive_Tab'; 
            GetObjectID('PoliticalConnection').className ='inActive_Tab'; 
            GetObjectID('Maritalstatus').className ='inActive_Tab'; 
            GetObjectID('State').className ='inActive_Tab'; 
            GetObjectID('Country').className ='inActive_Tab'; 
            GetObjectID('ContactType').className ='inActive_Tab'; 
            GetObjectID('KRANationality').className ='inActive_Tab'; 
            GetObjectID('KRAAnnualIncome').className ='inActive_Tab'; 
            GetObjectID('<%=txtKRApkcg.ClientID %>').value='';
            GetObjectID('<%=txtKRA.ClientID %>').value=''; 
            GetObjectID('<%=txtKRApkcg_hidden.ClientID %>').value='';
            GetObjectID('<%=txtKRA_hidden.ClientID %>').value='';          
        } 
        else if(obj=="3")
        {
            KRATabContent='PanExemptCategory';
            GetObjectID('AddressProof').className ='inActive_Tab';    
            GetObjectID('IdentityProof').className ='inActive_Tab';    
            GetObjectID('PanExemptCategory').className ='active_Tab';    
            GetObjectID('IndividualStatus').className ='inActive_Tab';
            GetObjectID('NRIStatusProof').className ='inActive_Tab';
            GetObjectID('NonIndividualStatus').className ='inActive_Tab'; 
            GetObjectID('Occupation').className ='inActive_Tab'; 
            GetObjectID('PoliticalConnection').className ='inActive_Tab'; 
            GetObjectID('Maritalstatus').className ='inActive_Tab'; 
            GetObjectID('State').className ='inActive_Tab'; 
            GetObjectID('Country').className ='inActive_Tab'; 
            GetObjectID('ContactType').className ='inActive_Tab'; 
            GetObjectID('KRANationality').className ='inActive_Tab'; 
            GetObjectID('KRAAnnualIncome').className ='inActive_Tab'; 
            GetObjectID('<%=txtKRApkcg.ClientID %>').value='';
            GetObjectID('<%=txtKRA.ClientID %>').value=''; 
            GetObjectID('<%=txtKRApkcg_hidden.ClientID %>').value='';
            GetObjectID('<%=txtKRA_hidden.ClientID %>').value='';             
        } 
        else if(obj=="4")
        {
            KRATabContent='IndividualStatus';
            GetObjectID('AddressProof').className ='inActive_Tab';    
            GetObjectID('IdentityProof').className ='inActive_Tab';    
            GetObjectID('PanExemptCategory').className ='inActive_Tab';    
            GetObjectID('IndividualStatus').className ='active_Tab';
            GetObjectID('NRIStatusProof').className ='inActive_Tab';
            GetObjectID('NonIndividualStatus').className ='inActive_Tab'; 
            GetObjectID('Occupation').className ='inActive_Tab'; 
            GetObjectID('PoliticalConnection').className ='inActive_Tab'; 
            GetObjectID('Maritalstatus').className ='inActive_Tab'; 
            GetObjectID('State').className ='inActive_Tab'; 
            GetObjectID('Country').className ='inActive_Tab'; 
            GetObjectID('ContactType').className ='inActive_Tab'; 
            GetObjectID('KRANationality').className ='inActive_Tab'; 
            GetObjectID('KRAAnnualIncome').className ='inActive_Tab'; 
            GetObjectID('<%=txtKRApkcg.ClientID %>').value='';
            GetObjectID('<%=txtKRA.ClientID %>').value=''; 
            GetObjectID('<%=txtKRApkcg_hidden.ClientID %>').value='';
            GetObjectID('<%=txtKRA_hidden.ClientID %>').value='';            
        } 
        else if(obj=="5")
        {
            KRATabContent='NRIStatusProof';
            GetObjectID('AddressProof').className ='inActive_Tab';    
            GetObjectID('IdentityProof').className ='inActive_Tab';    
            GetObjectID('PanExemptCategory').className ='inActive_Tab';    
            GetObjectID('IndividualStatus').className ='inActive_Tab';
            GetObjectID('NRIStatusProof').className ='active_Tab';
            GetObjectID('NonIndividualStatus').className ='inActive_Tab'; 
            GetObjectID('Occupation').className ='inActive_Tab'; 
            GetObjectID('PoliticalConnection').className ='inActive_Tab'; 
            GetObjectID('Maritalstatus').className ='inActive_Tab'; 
            GetObjectID('State').className ='inActive_Tab'; 
            GetObjectID('Country').className ='inActive_Tab'; 
            GetObjectID('ContactType').className ='inActive_Tab'; 
            GetObjectID('KRANationality').className ='inActive_Tab'; 
            GetObjectID('KRAAnnualIncome').className ='inActive_Tab'; 
            GetObjectID('<%=txtKRApkcg.ClientID %>').value='';
            GetObjectID('<%=txtKRA.ClientID %>').value=''; 
            GetObjectID('<%=txtKRApkcg_hidden.ClientID %>').value='';
            GetObjectID('<%=txtKRA_hidden.ClientID %>').value='';           
        } 
        else if(obj=="6")
        {
            KRATabContent='NonIndividualStatus';
            GetObjectID('AddressProof').className ='inActive_Tab';    
            GetObjectID('IdentityProof').className ='inActive_Tab';    
            GetObjectID('PanExemptCategory').className ='inActive_Tab';    
            GetObjectID('IndividualStatus').className ='inActive_Tab';
            GetObjectID('NRIStatusProof').className ='inActive_Tab';
            GetObjectID('NonIndividualStatus').className ='active_Tab'; 
            GetObjectID('Occupation').className ='inActive_Tab'; 
            GetObjectID('PoliticalConnection').className ='inActive_Tab'; 
            GetObjectID('Maritalstatus').className ='inActive_Tab'; 
            GetObjectID('State').className ='inActive_Tab'; 
            GetObjectID('Country').className ='inActive_Tab'; 
            GetObjectID('ContactType').className ='inActive_Tab'; 
            GetObjectID('KRANationality').className ='inActive_Tab'; 
            GetObjectID('KRAAnnualIncome').className ='inActive_Tab'; 
            GetObjectID('<%=txtKRApkcg.ClientID %>').value='';
            GetObjectID('<%=txtKRA.ClientID %>').value=''; 
            GetObjectID('<%=txtKRApkcg_hidden.ClientID %>').value='';
            GetObjectID('<%=txtKRA_hidden.ClientID %>').value='';            
        } 
        else if(obj=="7")
        {
            KRATabContent='Occupation';
            GetObjectID('AddressProof').className ='inActive_Tab';    
            GetObjectID('IdentityProof').className ='inActive_Tab';    
            GetObjectID('PanExemptCategory').className ='inActive_Tab';    
            GetObjectID('IndividualStatus').className ='inActive_Tab';
            GetObjectID('NRIStatusProof').className ='inActive_Tab';
            GetObjectID('NonIndividualStatus').className ='inActive_Tab'; 
            GetObjectID('Occupation').className ='active_Tab'; 
            GetObjectID('PoliticalConnection').className ='inActive_Tab'; 
            GetObjectID('Maritalstatus').className ='inActive_Tab'; 
            GetObjectID('State').className ='inActive_Tab'; 
            GetObjectID('Country').className ='inActive_Tab'; 
            GetObjectID('ContactType').className ='inActive_Tab'; 
            GetObjectID('KRANationality').className ='inActive_Tab'; 
            GetObjectID('KRAAnnualIncome').className ='inActive_Tab'; 
            GetObjectID('<%=txtKRApkcg.ClientID %>').value='';
            GetObjectID('<%=txtKRA.ClientID %>').value=''; 
            GetObjectID('<%=txtKRApkcg_hidden.ClientID %>').value='';
            GetObjectID('<%=txtKRA_hidden.ClientID %>').value='';      
        } 
        else if(obj=="8")
        {
            KRATabContent='PoliticalConnection';
            GetObjectID('AddressProof').className ='inActive_Tab';    
            GetObjectID('IdentityProof').className ='inActive_Tab';    
            GetObjectID('PanExemptCategory').className ='inActive_Tab';    
            GetObjectID('IndividualStatus').className ='inActive_Tab';
            GetObjectID('NRIStatusProof').className ='inActive_Tab';
            GetObjectID('NonIndividualStatus').className ='inActive_Tab'; 
            GetObjectID('Occupation').className ='inActive_Tab'; 
            GetObjectID('PoliticalConnection').className ='active_Tab'; 
            GetObjectID('Maritalstatus').className ='inActive_Tab'; 
            GetObjectID('State').className ='inActive_Tab'; 
            GetObjectID('Country').className ='inActive_Tab'; 
            GetObjectID('ContactType').className ='inActive_Tab'; 
            GetObjectID('KRANationality').className ='inActive_Tab'; 
            GetObjectID('KRAAnnualIncome').className ='inActive_Tab'; 
            GetObjectID('<%=txtKRApkcg.ClientID %>').value='';
            GetObjectID('<%=txtKRA.ClientID %>').value=''; 
            GetObjectID('<%=txtKRApkcg_hidden.ClientID %>').value='';
            GetObjectID('<%=txtKRA_hidden.ClientID %>').value='';            
        } 
        else if(obj=="9")
        {
            KRATabContent='MaritalStatus';
            GetObjectID('AddressProof').className ='inActive_Tab';    
            GetObjectID('IdentityProof').className ='inActive_Tab';    
            GetObjectID('PanExemptCategory').className ='inActive_Tab';    
            GetObjectID('IndividualStatus').className ='inActive_Tab';
            GetObjectID('NRIStatusProof').className ='inActive_Tab';
            GetObjectID('NonIndividualStatus').className ='inActive_Tab'; 
            GetObjectID('Occupation').className ='inActive_Tab'; 
            GetObjectID('PoliticalConnection').className ='inActive_Tab'; 
            GetObjectID('Maritalstatus').className ='active_Tab'; 
            GetObjectID('State').className ='inActive_Tab'; 
            GetObjectID('Country').className ='inActive_Tab'; 
            GetObjectID('ContactType').className ='inActive_Tab'; 
            GetObjectID('KRANationality').className ='inActive_Tab'; 
            GetObjectID('KRAAnnualIncome').className ='inActive_Tab'; 
            GetObjectID('<%=txtKRApkcg.ClientID %>').value='';
            GetObjectID('<%=txtKRA.ClientID %>').value=''; 
            GetObjectID('<%=txtKRApkcg_hidden.ClientID %>').value='';
            GetObjectID('<%=txtKRA_hidden.ClientID %>').value='';          
        } 
        else if(obj=="10")
        {
            KRATabContent='State';
            GetObjectID('AddressProof').className ='inActive_Tab';    
            GetObjectID('IdentityProof').className ='inActive_Tab';    
            GetObjectID('PanExemptCategory').className ='inActive_Tab';    
            GetObjectID('IndividualStatus').className ='inActive_Tab';
            GetObjectID('NRIStatusProof').className ='inActive_Tab';
            GetObjectID('NonIndividualStatus').className ='inActive_Tab'; 
            GetObjectID('Occupation').className ='inActive_Tab'; 
            GetObjectID('PoliticalConnection').className ='inActive_Tab'; 
            GetObjectID('Maritalstatus').className ='inActive_Tab'; 
            GetObjectID('State').className ='active_Tab'; 
            GetObjectID('Country').className ='inActive_Tab'; 
            GetObjectID('ContactType').className ='inActive_Tab'; 
            GetObjectID('KRANationality').className ='inActive_Tab'; 
            GetObjectID('KRAAnnualIncome').className ='inActive_Tab'; 
            GetObjectID('<%=txtKRApkcg.ClientID %>').value='';
            GetObjectID('<%=txtKRA.ClientID %>').value=''; 
            GetObjectID('<%=txtKRApkcg_hidden.ClientID %>').value='';
            GetObjectID('<%=txtKRA_hidden.ClientID %>').value='';  
        } 
        else if(obj=="11")
        {
            KRATabContent='Country';
            GetObjectID('AddressProof').className ='inActive_Tab';    
            GetObjectID('IdentityProof').className ='inActive_Tab';    
            GetObjectID('PanExemptCategory').className ='inActive_Tab';    
            GetObjectID('IndividualStatus').className ='inActive_Tab';
            GetObjectID('NRIStatusProof').className ='inActive_Tab';
            GetObjectID('NonIndividualStatus').className ='inActive_Tab'; 
            GetObjectID('Occupation').className ='inActive_Tab'; 
            GetObjectID('PoliticalConnection').className ='inActive_Tab'; 
            GetObjectID('Maritalstatus').className ='inActive_Tab'; 
            GetObjectID('State').className ='inActive_Tab'; 
            GetObjectID('Country').className ='active_Tab'; 
            GetObjectID('ContactType').className ='inActive_Tab'; 
            GetObjectID('KRANationality').className ='inActive_Tab'; 
            GetObjectID('KRAAnnualIncome').className ='inActive_Tab'; 
            GetObjectID('<%=txtKRApkcg.ClientID %>').value='';
            GetObjectID('<%=txtKRA.ClientID %>').value=''; 
            GetObjectID('<%=txtKRApkcg_hidden.ClientID %>').value='';
            GetObjectID('<%=txtKRA_hidden.ClientID %>').value='';    
        }
        else if(obj=="12")
        {
            KRATabContent='ContactType';
            GetObjectID('AddressProof').className ='inActive_Tab';    
            GetObjectID('IdentityProof').className ='inActive_Tab';    
            GetObjectID('PanExemptCategory').className ='inActive_Tab';    
            GetObjectID('IndividualStatus').className ='inActive_Tab';
            GetObjectID('NRIStatusProof').className ='inActive_Tab';
            GetObjectID('NonIndividualStatus').className ='inActive_Tab'; 
            GetObjectID('Occupation').className ='inActive_Tab'; 
            GetObjectID('PoliticalConnection').className ='inActive_Tab'; 
            GetObjectID('Maritalstatus').className ='inActive_Tab'; 
            GetObjectID('State').className ='inActive_Tab'; 
            GetObjectID('Country').className ='inActive_Tab'; 
            GetObjectID('ContactType').className ='active_Tab'; 
            GetObjectID('KRANationality').className ='inActive_Tab'; 
            GetObjectID('KRAAnnualIncome').className ='inActive_Tab'; 
            GetObjectID('<%=txtKRApkcg.ClientID %>').value='';
            GetObjectID('<%=txtKRA.ClientID %>').value=''; 
            GetObjectID('<%=txtKRApkcg_hidden.ClientID %>').value='';
            GetObjectID('<%=txtKRA_hidden.ClientID %>').value='';    
        }
        else if(obj=="13")
        {
            KRATabContent='KRANationality';
            GetObjectID('AddressProof').className ='inActive_Tab';    
            GetObjectID('IdentityProof').className ='inActive_Tab';    
            GetObjectID('PanExemptCategory').className ='inActive_Tab';    
            GetObjectID('IndividualStatus').className ='inActive_Tab';
            GetObjectID('NRIStatusProof').className ='inActive_Tab';
            GetObjectID('NonIndividualStatus').className ='inActive_Tab'; 
            GetObjectID('Occupation').className ='inActive_Tab'; 
            GetObjectID('PoliticalConnection').className ='inActive_Tab'; 
            GetObjectID('Maritalstatus').className ='inActive_Tab'; 
            GetObjectID('State').className ='inActive_Tab'; 
            GetObjectID('Country').className ='inActive_Tab'; 
            GetObjectID('ContactType').className ='inActive_Tab'; 
            GetObjectID('KRANationality').className ='active_Tab'; 
            GetObjectID('KRAAnnualIncome').className ='inActive_Tab'; 
            GetObjectID('<%=txtKRApkcg.ClientID %>').value='';
            GetObjectID('<%=txtKRA.ClientID %>').value=''; 
            GetObjectID('<%=txtKRApkcg_hidden.ClientID %>').value='';
            GetObjectID('<%=txtKRA_hidden.ClientID %>').value='';    
        }
        else if(obj=="14")
        {
            KRATabContent='KRAAnnualIncome';
            GetObjectID('AddressProof').className ='inActive_Tab';    
            GetObjectID('IdentityProof').className ='inActive_Tab';    
            GetObjectID('PanExemptCategory').className ='inActive_Tab';    
            GetObjectID('IndividualStatus').className ='inActive_Tab';
            GetObjectID('NRIStatusProof').className ='inActive_Tab';
            GetObjectID('NonIndividualStatus').className ='inActive_Tab'; 
            GetObjectID('Occupation').className ='inActive_Tab'; 
            GetObjectID('PoliticalConnection').className ='inActive_Tab'; 
            GetObjectID('Maritalstatus').className ='inActive_Tab'; 
            GetObjectID('State').className ='inActive_Tab'; 
            GetObjectID('Country').className ='inActive_Tab'; 
            GetObjectID('ContactType').className ='inActive_Tab'; 
            GetObjectID('KRANationality').className ='inActive_Tab'; 
            GetObjectID('KRAAnnualIncome').className ='active_Tab'; 
            GetObjectID('<%=txtKRApkcg.ClientID %>').value='';
            GetObjectID('<%=txtKRA.ClientID %>').value=''; 
            GetObjectID('<%=txtKRApkcg_hidden.ClientID %>').value='';
            GetObjectID('<%=txtKRA_hidden.ClientID %>').value='';    
        }
        cCbpKRAContainer.PerformCallback("FetchKRA^"+KRATabContent+"^"+cCmbKycRA.GetValue());       
    }
    function fn_CmbKycRA(obj)
    {   
        GetObjectID('<%=txtKRApkcg.ClientID %>').value='';
        GetObjectID('<%=txtKRA.ClientID %>').value=''; 
        if(obj=='1')//CVL
        {
            cCbpKRAContainer.PerformCallback("FetchKRA^"+KRATabContent+"^1"); 
        }
        else if(obj=='2')//NDML
        {
            cCbpKRAContainer.PerformCallback("FetchKRA^"+KRATabContent+"^2"); 
        }
        else if(obj=='3')//DotEx
        {
            cCbpKRAContainer.PerformCallback("FetchKRA^"+KRATabContent+"^3"); 
        }
    }
    function CbpKRAContainer_EndCallBack()
    {
        GetObjectID('KRA_Info').style.display='none';                     
        if(cCbpKRAContainer.cpAutoFillKRA!=undefined)
        { 
            if(cCbpKRAContainer.cpAutoFillKRA=='T')
            {
                if(cCbpKRAContainer.cpAutoFillKRAData!=undefined)
                {
                     var selTab=cCbpKRAContainer.cpAutoFillKRAData.split('^')[0];
                     var selCombo=cCbpKRAContainer.cpAutoFillKRAData.split('^')[1];
                     var KraContent= cCbpKRAContainer.cpAutoFillKRAData.split('^')[2];
                     GetObjectID('AddressProof').className ='inActive_Tab';    
                     GetObjectID('IdentityProof').className ='inActive_Tab';    
                     GetObjectID('PanExemptCategory').className ='inActive_Tab';    
                     GetObjectID('IndividualStatus').className ='inActive_Tab';
                     GetObjectID('NRIStatusProof').className ='inActive_Tab';
                     GetObjectID('NonIndividualStatus').className ='inActive_Tab'; 
                     GetObjectID('Occupation').className ='inActive_Tab'; 
                     GetObjectID('PoliticalConnection').className ='inActive_Tab'; 
                     GetObjectID('Maritalstatus').className ='inActive_Tab'; 
                     GetObjectID('State').className ='inActive_Tab'; 
                     GetObjectID('Country').className ='inActive_Tab'; 
                     GetObjectID('ContactType').className ='inActive_Tab'; 
                     GetObjectID('KRANationality').className ='inActive_Tab'; 
                     GetObjectID('KRAAnnualIncome').className ='inActive_Tab'; 
                     GetObjectID(selTab).className ='active_Tab'; 
                     
                     cCmbKycRA.SetValue(selCombo);                    
                     SetValue('<%=txtKRA_hidden.ClientID %>',KraContent);
                     var staticCode='';
                     var staticKRAData=new Array();
                     staticKRAData=KraContent.split('~');                    
                     SetValue('<%=txtKRA.ClientID %>',staticKRAData[1].trim());                     
                     if(staticKRAData[3].trim()!='')
                        staticCode=staticKRAData[2].trim()+'/'+staticKRAData[3].trim();
                     else
                        staticCode=staticKRAData[2].trim();   
                     ctxtKRACode.SetText(staticCode);
                }
            }
            else if(cCbpKRAContainer.cpAutoFillKRA=='F')
                alert('Mapped Result Not Found. Please Map Data Again.');           
        }
        cCbpKRAContainer.cpAutoFillKRA=null;
        cCbpKRAContainer.cpAutoFillKRAData=null;
        if(cCbpKRAContainer.cpUpdate!=undefined)
        {        
             var updateOn=cCbpKRAContainer.cpUpdate.split('~')[0];
             var updateResult=cCbpKRAContainer.cpUpdate.split('~')[1];            
             if(updateResult=='Successfully Update')
             {
                 cBtnKRADone.SetEnabled(false);    
                 GetObjectID('KRA_Info').style.display='none';
                 GetObjectID('<%=txtKRApkcg.ClientID %>').value='';
                 GetObjectID('<%=txtKRA.ClientID %>').value='';                 
                 GetObjectID('<%=txtKRApkcg_hidden.ClientID %>').value='';
                 GetObjectID('<%=txtKRA_hidden.ClientID %>').value='';
                 GetObjectID('AddressProof').className ='inActive_Tab';    
                 GetObjectID('IdentityProof').className ='inActive_Tab';    
                 GetObjectID('PanExemptCategory').className ='inActive_Tab';    
                 GetObjectID('IndividualStatus').className ='inActive_Tab';
                 GetObjectID('NRIStatusProof').className ='inActive_Tab';
                 GetObjectID('NonIndividualStatus').className ='inActive_Tab'; 
                 GetObjectID('Occupation').className ='inActive_Tab'; 
                 GetObjectID('PoliticalConnection').className ='inActive_Tab'; 
                 GetObjectID('Maritalstatus').className ='inActive_Tab'; 
                 GetObjectID('State').className ='inActive_Tab'; 
                 GetObjectID('Country').className ='inActive_Tab'; 
                 GetObjectID('ContactType').className ='inActive_Tab'; 
                 GetObjectID('KRANationality').className ='inActive_Tab'; 
                 GetObjectID('KRAAnnualIncome').className ='inActive_Tab'; 
                 GetObjectID(updateOn).className ='active_Tab'; 
                 alert('Successfully Updated'); 
                 KRATabContent=updateOn;                
                 cCbpKRAContainer.PerformCallback("FetchKRA^"+KRATabContent+"^"+cCmbKycRA.GetValue()); 
             }             
        }         
        Height('460','460');
    }
    function BtnKRADone_Click()
    {
        var masterKraMapID='';
        var staticKraMapID='';
        var KRAComboSelected=cCmbKycRA.GetValue();
       
        if (GetObjectID('<%=txtKRApkcg.ClientID %>').value!='')
            masterKraMapID=GetObjectID('<%=txtKRApkcg_hidden.ClientID %>').value.split('~')[0];
        if (GetObjectID('<%=txtKRA.ClientID %>').value!='')
            staticKraMapID=GetObjectID('<%=txtKRA_hidden.ClientID %>').value.split('~')[0];
        if(Control_Empty("<%=txtKRApkcg.ClientID %>","Please Select KRA Mapping Value!!!"))
            if(Control_Empty("<%=txtKRA.ClientID %>","Please Select KRA Static Code!!!"))
                if(Control_CompareText("<%=txtKRApkcg.ClientID %>",'No Record Found',"Please Select KRA Mapping Value Properly!!!"))
                    if(Control_CompareText("<%=txtKRA.ClientID %>",'No Record Found',"Please Select KRA Static Code Properly!!!"))
                        cCbpKRAContainer.PerformCallback("UpdateKRA^"+KRATabContent+"^"+KRAComboSelected+"^"+masterKraMapID+"^"+staticKraMapID);
    }    
    </script>

    <script language="javascript" type="text/javascript">    
    function CallAjax(obj1,obj2,obj3,Query)
    {                
         var CombinedQuery=new String(Query);
         var GenericAjaxListAspxPath = '../../CentralData/Pages/GenericAjaxList.aspx';
         ajax_showOptions(obj1,obj2,obj3,replaceChars(CombinedQuery),'Main',GenericAjaxListAspxPath);                  
    }
    function replaceChars(entry) 
    {
        out = "+"; // replace this
        add = "--"; // with this
        temp = "" + entry; // temporary holder

        while (temp.indexOf(out)>-1) {
        pos= temp.indexOf(out);
        temp = "" + (temp.substring(0, pos) + add + 
        temp.substring((pos + out.length), temp.length));
        }
        //alert(temp);
        return temp;            
    }      
    FieldName='abcd'; 
    function keyVal(obj)
    {
        var hdnArray=new Array();
        hdnArray=obj.split('~');
        var WhichCall=hdnArray[hdnArray.length-1];
        //alert(WhichCall);        
        //===========For CDSL=================        
        if(WhichCall=='CDSLMAPSTATIC'||WhichCall=='GEOGRAPHICAL'||WhichCall=='ANNUALINCOME'||WhichCall=='NATIONALITY'||WhichCall=='EDUCATION'
            ||WhichCall=='LEGALSTATUS'||WhichCall=='LANGUAGE'||WhichCall=='CDSLPANEXEMPTION'||WhichCall=='CDSLOCCUPATION'||WhichCall=='CURRENCY')
        {        
            var hdnCDSLMasterMap=document.getElementById('<%=txtpkcg_hidden.ClientID %>').value; 
            //alert('keyVal-hdnCDSLMasterMap-'+hdnCDSLMasterMap);
            var mappingCDSLMasterID=hdnCDSLMasterMap.split('~')[0];
            var mappingCDSLMasterDesc=hdnCDSLMasterMap.split('~')[1];
            var MappedCDSLID=hdnCDSLMasterMap.split('~')[2];
            var hdnStaticCDSL='';
            var staticCDSLID='';
            var staticCDSLCode='';       
            var staticCDSLDesc='';
            if(MappedCDSLID!='0')
            {                
                //Set Value in TxtCDSL and txtCDSLCode from static CDSL table 
                if(WhichCall!='CDSLMAPSTATIC')               
                    cCbpContainer.PerformCallback('AutoFillStaticCDSL^'+tabcontent+'^'+MappedCDSLID);
            }
            else
            {
                hdnStaticCDSL=document.getElementById('<%=txtcdsl_hidden.ClientID %>').value;
                staticCDSLID=hdnStaticCDSL.split('~')[0];
                staticCDSLDesc=hdnStaticCDSL.split('~')[1];               
                staticCDSLCode=hdnStaticCDSL.split('~')[2];
                //alert('keyVal-Else-hdnStaticCDSL-'+hdnStaticCDSL);
            
                if (mappingCDSLMasterID!='' && staticCDSLID!='')
                {           
                    if (mappingCDSLMasterDesc!=undefined && staticCDSLDesc!=undefined)
                    {
                        if (mappingCDSLMasterDesc.trim()==staticCDSLDesc.trim())
                        {
                            if(GetObjectID('<%=txtcdsl.ClientID %>').value !='')
                                ctxtCdslCode.SetText(staticCDSLCode);
                            GetObjectID('Cdsl_Info').style.display='none';
                        }
                        else
                        {
                             if(GetObjectID('<%=txtcdsl.ClientID %>').value !='')
                                ctxtCdslCode.SetText(staticCDSLCode);
                             GetObjectID('Cdsl_Info').style.display='inline';                    
                        }
                        clientbtn.SetEnabled(true);                     
                    }
                    else
                         clientbtn.SetEnabled(false);
                }         
            }
        }
        //======For KRA======================  
        if(WhichCall=='KRAMAPSTATIC'||WhichCall=='ADDRESSPROOF'||WhichCall=='IDENTITYPROOF'||
           WhichCall=='PANEXEMPTCATEGORY'||WhichCall=='INDIVIDUALSTATUS'||WhichCall=='NRISTATUSPROOF'||
           WhichCall=='NONINDIVIDUALSTATUS'||WhichCall=='OCCUPATION'||WhichCall=='POLITICALCONNECTION'||
           WhichCall=='MARITALSTATUS'||WhichCall=='STATE'||WhichCall=='COUNTRY'||WhichCall=='CONTACTTYPE'||
           WhichCall=='KRANATIONALITY'||WhichCall=='KRAANNUALINCOME') 
        {     
            var hdnKRAMasterMap=document.getElementById('<%=txtKRApkcg_hidden.ClientID %>').value;        
            var mappingKRAMasterID=hdnKRAMasterMap.split('~')[0];
            var mappingKRAMasterDesc=hdnKRAMasterMap.split('~')[1];
            var MappedKRAID=hdnKRAMasterMap.split('~')[2];
                            
            var hdnStaticKra='';
            var staticKraID='';
            var staticKraDesc='';
            var staticKraCode='';       
            if(MappedKRAID!='0')
            {
                //Set Value in TxtKRA and txtKraCode from static KRA table 
                var comboVal=cCmbKycRA.GetValue();               
                if(WhichCall!='KRAMAPSTATIC')               
                    cCbpKRAContainer.PerformCallback('AutoFillStaticKRA^'+KRATabContent+'^'+comboVal+'^'+MappedKRAID);
            }
            else
            {
                hdnStaticKra=document.getElementById('<%=txtKRA_hidden.ClientID %>').value;
                staticKraID=hdnStaticKra.split('~')[0];
                staticKraDesc=hdnStaticKra.split('~')[1]; 
              
                if(hdnStaticKra.split('~')[3]!=undefined && hdnStaticKra.split('~')[3]!='')
                    staticKraCode=hdnStaticKra.split('~')[2]+'/'+hdnStaticKra.split('~')[3]; 
                else
                     staticKraCode=hdnStaticKra.split('~')[2]
           
                if (mappingKRAMasterID!='' && staticKraID!='')
                {           
                    if (mappingKRAMasterDesc!=undefined && staticKraDesc!=undefined)
                    {
                        if (mappingKRAMasterDesc.trim()==staticKraDesc.trim())
                        {
                            if(GetObjectID('<%=txtKRA.ClientID %>').value !='')
                                ctxtKRACode.SetText(staticKraCode);
                            GetObjectID('KRA_Info').style.display='none';
                            GetObjectID('KRA_height').style.display='inline';
                        }
                        else
                        {
                             if(GetObjectID('<%=txtKRA.ClientID %>').value !='')
                                ctxtKRACode.SetText(staticKraCode);
                             GetObjectID('KRA_Info').style.display='inline';
                             GetObjectID('KRA_height').style.display='none';                             
                        }
                        cBtnKRADone.SetEnabled(true);                     
                    }
                    else
                         cBtnKRADone.SetEnabled(false);
                } 
            }
        }              
    }
    </script>

    <style type="text/css">
        #pageCont{width:985px; margin:5px;}
        .PanelHeader{ width:975px; border:1px solid #555; background-color:#55D4FF; padding:2px; text-align:center;}
        .tabCont{width:160px;z-index:10;position:relative;}       
        .PanelContainer{width:810px; background-color:#eee; border: 1px solid #555; padding :5px; margin-left:-1px;}        
        .leftPanel{width: 390px; float:left; border: 1px solid black; padding: 2px; margin-left: 4px; background-color: #E6E6E2; height: 105px;}
        .leftPanelCont{width: 388px; float:left; margin-bottom: 10px; text-align: center; background-color: #aaa; border: 1px solid #aaa; font-weight: bold}
        .inActive_Tab{border:1px solid #777; font-size:14px;font-Weight:normal; padding:0px 3px; line-height:1.5; background-color:#aaa; }
        .active_Tab{font-size:15px;font-Weight:bold; padding:0px 3px; line-height:1.5; background-color:#eee; border: 1px solid #555; border-right:none;}                        
    </style>
    <!--Accordin Style And Javascript-->
    <style type="text/css">
    .AccordionTitle, .AccordionContent, .AccordionContainer {position:relative; width:1150px;}
    .AccordionTitle {overflow:hidden; cursor:pointer; Margin:5px ; } 
    .AccordionContent {height:0px; overflow:auto; display:block;  Margin:-5px 0px 0px 5px;}
    </style>

    <script type="text/javascript">
    var ContentHeight = 330;
    var TimeToSlide = 250.0;
    var openAccordion = '';
    function runAccordion(index)
    {
       var nID = "Accordion" + index + "Content";
       if(openAccordion == nID)
          nID = '';        
       setTimeout("animate(" + new Date().getTime() + "," + TimeToSlide + ",'" 
            + openAccordion + "','" + nID + "')", 33);     
       openAccordion = nID;
    }
    function animate(lastTick, timeLeft, closingId, openingId)
    {
        var curTick = new Date().getTime();
        var elapsedTicks = curTick - lastTick;      
        var opening = (openingId == '') ? null : document.getElementById(openingId);
        var closing = (closingId == '') ? null : document.getElementById(closingId);     
        if(timeLeft <= elapsedTicks)
        {      
            if(opening != null)
                opening.style.height = ContentHeight + 'px';        
            if(closing != null)
            {
                closing.style.display = 'none';
                closing.style.height = '0px';
            }        
            if ((document.getElementById('T1').innerHTML=='[ + ]') &&(document.getElementById('Accordion1Content').style.display=='block'))
            {            
                document.getElementById('T1').innerHTML="[ - ]";            
            }
            else
            {            
                document.getElementById('T1').innerHTML="[ + ]";
            }            
            if ((document.getElementById('T2').innerHTML=='[ + ]') &&(document.getElementById('Accordion2Content').style.display=='block'))
            {            
                document.getElementById('T2').innerHTML="[ - ]";            
            }
            else
            {            
                document.getElementById('T2').innerHTML="[ + ]";
            }        
            return;
        }     
        timeLeft -= elapsedTicks;
        var newClosedHeight = Math.round((timeLeft/TimeToSlide) * ContentHeight);
        if(opening != null)
        {     
          if(opening.style.display != 'block')
            opening.style.display = 'block';
          opening.style.height = (ContentHeight - newClosedHeight) + 'px';
        }      
        if(closing != null)
          closing.style.height = newClosedHeight + 'px';
        setTimeout("animate(" + curTick + "," + timeLeft + ",'" 
            + closingId + "','" + openingId + "')", 33);
    }    
    </script>

</head>
<body onload="runAccordion(1)">
    <form id="form1" runat="server">
        <div id="MainDiv">
            <div id="Header" class="Header">
                Master Mapping
            </div>
            <div id="pageCont">
                <div id="AccordionContainer" class="AccordionContainer">
                    <div onclick="runAccordion(1);">
                        <div class="AccordionTitle" onselectstart="return false;">
                            <div class="PanelHeader">
                                Data Mapping With CDSL Static Code <span id="T1">[ + ]</span>
                            </div>
                        </div>
                    </div>
                    <div id="Accordion1Content" class="AccordionContent">
                        <div id="divCdslTab" class="left tabCont">
                            <a href="javascript:void(0);" onclick="fn_divCdslTab('1')">
                                <div id="Geographical" class="active_Tab">
                                    Geographical
                                </div>
                            </a><a href="javascript:void(0);" onclick="fn_divCdslTab('2')">
                                <div id="AnnualIncome" class="inActive_Tab">
                                    AnnualIncome
                                </div>
                            </a><a href="javascript:void(0);" onclick="fn_divCdslTab('3')">
                                <div id="Nationality" class="inActive_Tab">
                                    Nationality
                                </div>
                            </a><a href="javascript:void(0);" onclick="fn_divCdslTab('4')">
                                <div id="Education" class="inActive_Tab">
                                    Education
                                </div>
                            </a><a href="javascript:void(0);" onclick="fn_divCdslTab('5')">
                                <div id="LegalStatus" class="inActive_Tab">
                                    LegalStatus
                                </div>
                            </a><a href="javascript:void(0);" onclick="fn_divCdslTab('6')">
                                <div id="Language" class="inActive_Tab">
                                    Language
                                </div>
                            </a><a href="javascript:void(0);" onclick="fn_divCdslTab('7')">
                                <div id="PanExemption" class="inActive_Tab">
                                    PanExemption
                                </div>
                            </a><a href="javascript:void(0);" onclick="fn_divCdslTab('8')">
                                <div id="CdslOccupation" class="inActive_Tab">
                                    Occupation
                                </div>
                            </a><a href="javascript:void(0);" onclick="fn_divCdslTab('9')">
                                <div id="Currency" class="inActive_Tab">
                                    Currency
                                </div>
                            </a>
                        </div>
                        <div id="divCdslPanel" class="left PanelContainer">
                            <dxe:ASPxCallbackPanel ID="CbpContainer" runat="server" ClientInstanceName="cCbpContainer"
                                OnCallback="CbpContainer_Callback">
                                <PanelCollection>
                                    <dxe:panelcontent runat="server">
                                        <div id="divCdslResult">
                                            <div id="divCdslleft" class="leftPanel">
                                                <div class="leftPanelCont">
                                                    Mapping Value</div>
                                                <br class="clear" />
                                                <div class="left" style="padding-left: 5px;">
                                                    <span style="font-size: 12px">Description</span>
                                                </div>
                                                <div class="left" style="padding-left: 5px;">
                                                    <asp:TextBox runat="server" ID="txtpkcg" Width="370px">                                
                                                    </asp:TextBox>
                                                    <asp:HiddenField ID="txtpkcg_hidden" runat="server" />
                                                </div>
                                                <br class="clear" />
                                            </div>
                                            <div id="divCdslright" class="leftPanel">
                                                <div class="leftPanelCont">
                                                    CDSL Static Code</div>
                                                <br class="clear" />
                                                <div class="left" style="padding-left: 5px;">
                                                    <span style="font-size: 12px">Description</span>
                                                </div>
                                                <div class="left" style="padding-left: 5px;">
                                                    <asp:TextBox runat="server" ID="txtcdsl" Width="330px">                                
                                                    </asp:TextBox>
                                                    <asp:HiddenField ID="txtcdsl_hidden" runat="server" />
                                                </div>
                                                <div class="left" style="margin-left: 5px;">
                                                    <dxe:ASPxTextBox ID="txtCdslCode" runat="server" ClientInstanceName="ctxtCdslCode"
                                                        Width="40px" Enabled="false">
                                                    </dxe:ASPxTextBox>
                                                </div>
                                                <br class="clear" />
                                            </div>
                                        </div>
                                        <br class="clear" />
                                        <center style="margin-top: 95px;">
                                            <dxe:ASPxButton ID="Btndone" runat="server" AutoPostBack="False" Text="Done" Width="150px"
                                                ClientInstanceName="clientbtn">
                                                <ClientSideEvents Click="function (s, e) {Btndone_Click();}" />
                                            </dxe:ASPxButton>
                                        </center>
                                        <center>
                                            <div id="Cdsl_Info" style="margin: 20px;">
                                                <div class="Info" style="width: 300px;">
                                                    To make Sure That Both are not Same !!</div>
                                            </div>
                                        </center>
                                    </dxe:panelcontent>
                                </PanelCollection>
                                <ClientSideEvents EndCallback="function (s, e) {CbpContainer_EndCallBack();}" />
                            </dxe:ASPxCallbackPanel>
                        </div>
                        <br class="clear" />                        
                    </div>
                    <div onclick="runAccordion(2);">
                        <div class="AccordionTitle" onselectstart="return false;">
                            <div class="PanelHeader">
                                Data Mapping With KRA Static Code <span id="T2">[ + ]</span>
                            </div>
                        </div>
                    </div>
                    <div id="Accordion2Content" class="AccordionContent">
                        <div id="divKraTab" class="left tabCont">
                            <a href="javascript:void(0);" onclick="fn_divKRATab('1')">
                                <div id="AddressProof" class="active_Tab">
                                    Address Proof
                                </div>
                            </a><a href="javascript:void(0);" onclick="fn_divKRATab('2')">
                                <div id="IdentityProof" class="inActive_Tab">
                                    Identity Proof
                                </div>
                            </a><a href="javascript:void(0);" onclick="fn_divKRATab('3')">
                                <div id="PanExemptCategory" class="inActive_Tab">
                                    PanExempt Category
                                </div>
                            </a><a href="javascript:void(0);" onclick="fn_divKRATab('4')">
                                <div id="IndividualStatus" class="inActive_Tab">
                                    Individual Status
                                </div>
                            </a><a href="javascript:void(0);" onclick="fn_divKRATab('5')">
                                <div id="NRIStatusProof" class="inActive_Tab">
                                    NRI Status Proof
                                </div>
                            </a><a href="javascript:void(0);" onclick="fn_divKRATab('6')">
                                <div id="NonIndividualStatus" class="inActive_Tab">
                                    NonIndividual Status
                                </div>
                            </a><a href="javascript:void(0);" onclick="fn_divKRATab('7')">
                                <div id="Occupation" class="inActive_Tab">
                                    Occupation
                                </div>
                            </a><a href="javascript:void(0);" onclick="fn_divKRATab('8')">
                                <div id="PoliticalConnection" class="inActive_Tab">
                                    Political Connection
                                </div>
                            </a><a href="javascript:void(0);" onclick="fn_divKRATab('9')">
                                <div id="Maritalstatus" class="inActive_Tab">
                                    Marital Status
                                </div>
                            </a><a href="javascript:void(0);" onclick="fn_divKRATab('10')">
                                <div id="State" class="inActive_Tab">
                                    State
                                </div>
                            </a><a href="javascript:void(0);" onclick="fn_divKRATab('11')">
                                <div id="Country" class="inActive_Tab">
                                    Country
                                </div>
                            </a><a href="javascript:void(0);" onclick="fn_divKRATab('12')">
                                <div id="ContactType" class="inActive_Tab">
                                    ContactType
                                </div>
                            </a><a href="javascript:void(0);" onclick="fn_divKRATab('13')">
                                <div id="KRANationality" class="inActive_Tab">
                                    Nationality
                                </div>
                            </a><a href="javascript:void(0);" onclick="fn_divKRATab('14')">
                                <div id="KRAAnnualIncome" class="inActive_Tab">
                                    AnnualIncome
                                </div>
                            </a>
                        </div>
                        <div id="divKRAPanel" class="left PanelContainer">
                            <dxe:ASPxCallbackPanel ID="CbpKRAContainer" runat="server" ClientInstanceName="cCbpKRAContainer"
                                OnCallback="CbpKRAContainer_Callback">
                                <PanelCollection>
                                    <dxe:panelcontent runat="server">
                                        <div id="divKycRa" style="margin: 15px 0px 0px 5px;">
                                            <dxe:ASPxComboBox ID="CmbKycRA" AutoPostBack="false" Width="140px" runat="server"
                                                ValueType="System.String" ClientInstanceName="cCmbKycRA" SelectedIndex="0" TabIndex="0">
                                                <Items>
                                                    <dxe:ListEditItem Value="1" Text="CVL" />
                                                    <dxe:ListEditItem Value="2" Text="NDML" />
                                                    <dxe:ListEditItem Value="3" Text="DotEx" />
                                                </Items>
                                                <ClientSideEvents SelectedIndexChanged="function(s, e) {fn_CmbKycRA(s.GetValue());}" />
                                            </dxe:ASPxComboBox>
                                        </div>
                                        <br class="clear" />
                                        <div id="divKRAResult">
                                            <div id="divKRAleft" class="leftPanel">
                                                <div class="leftPanelCont">
                                                    Mapping Value</div>
                                                <br class="clear" />
                                                <div class="left" style="padding-left: 5px;">
                                                    <span style="font-size: 12px">Description</span>
                                                </div>
                                                <div class="left" style="padding-left: 5px;">
                                                    <asp:TextBox runat="server" ID="txtKRApkcg" Width="370px">                                
                                                    </asp:TextBox>
                                                    <asp:HiddenField ID="txtKRApkcg_hidden" runat="server" />
                                                </div>
                                                <br class="clear" />
                                            </div>
                                            <div id="divKRAright" class="leftPanel">
                                                <div class="leftPanelCont">
                                                    KRA Static Code</div>
                                                <br class="clear" />
                                                <div class="left" style="padding-left: 5px;">
                                                    <span style="font-size: 12px">Description</span>
                                                </div>
                                                <div class="left" style="padding-left: 5px;">
                                                    <asp:TextBox runat="server" ID="txtKRA" Width="330px">                                
                                                    </asp:TextBox>
                                                    <asp:HiddenField ID="txtKRA_hidden" runat="server" />
                                                </div>
                                                <div class="left" style="margin-left: 5px;">
                                                    <dxe:ASPxTextBox ID="txtKRACode" runat="server" ClientInstanceName="ctxtKRACode" Width="40px"
                                                        Enabled="false">
                                                    </dxe:ASPxTextBox>
                                                </div>
                                                <br class="clear" />
                                            </div>
                                        </div>
                                        <br class="clear" />
                                        <center style="margin-top: 80px;">
                                            <dxe:ASPxButton ID="BtnKRADone" runat="server" AutoPostBack="False" Text="Done" Width="150px"
                                                ClientInstanceName="cBtnKRADone">
                                                <ClientSideEvents Click="function (s, e) {BtnKRADone_Click();}" />
                                            </dxe:ASPxButton>
                                        </center>
                                        <center>
                                            <div id="kRA_Info" style="margin: 20px;">
                                                <div class="Info" style="width: 300px;">
                                                    To make Sure That Both are not Same !!</div>
                                            </div>
                                        </center>
                                        <div id="KRA_height" style="height:67px"></div>
                                        </dxe:panelcontent>
                                </PanelCollection>
                                <ClientSideEvents EndCallback="function (s, e) {CbpKRAContainer_EndCallBack();}" />
                            </dxe:ASPxCallbackPanel>
                        </div>
                        <br class="clear" />
                    </div>
                </div>
            </div>
        </div>
    </form>
</body>
</html>
