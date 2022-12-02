<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/OMS/MasterPage/ERP.Master"
    Inherits="ERP.OMS.Management.management_frmSendMailForPhoneCall" Codebehind="frmSendMailForPhoneCall.aspx.cs" %>

<%--<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dxe000001" %>
<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dxe" %>--%>
<%@ Register Src="Headermain.ascx" TagName="Headermain" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../CSS/style.css" rel="stylesheet" type="text/css" />
    <title>Untitled </title>

    <script language="javascript" type="text/javascript">
    
    //function is called on changing Selection
    function OnGridServerSelectAll(obj)
    {
        OnGridSelectionChanged();
    }
    function OnGridSelectionChanged() 
    {
//        var noofrow=grid.GetSelectedRowCount().toString();
//        alert(noofrow);
        gridServer.GetSelectedFieldValues('FilePath', OnGridSelectionComplete);
    }
    function visibilityonoff()
    {
        document.getElementById("visibility").style.display ='none';
        document.getElementById("visibilityColumn").style.display ='none';
    }
    function OnGridSelectionComplete(values) 
    {
        counter = 'n';
        for(var i = 0; i < values.length; i ++) 
        {
            if(counter != 'n')
                counter += ',' + values[i];
            else
                counter = values[i];
        }
    }
    
    function OnGridLocalSelectAll(obj)
    {
        OnGridLocalSelectionChanged();  
    }
    function OnGridLocalSelectionChanged() 
    {
//        var noofrow=gridLocal.GetSelectedRowCount().toString();
//        alert(noofrow);
        gridLocal.GetSelectedFieldValues('filepathServer', OnGridLocalSelectionComplete);
    }
    function OnGridLocalSelectionComplete(values) 
    {
        counterLocal = 'n';
        for(var i = 0; i < values.length; i ++) {
            if(counterLocal != 'n')
                counterLocal += ',' + values[i];
            else
                counterLocal = values[i];
        }
        //alert(counterLocal);
    }
    //________End here on changing focused row
    
    
    </script>

    <script type="text/javascript" language="javascript">
    function AtTheTimePageLoad()
    {
        //alert('t');
        counterLocal='n';
        counter='n';
        FieldName='cmbFrom';
        document.getElementById("FormServerFile").style.display = 'none';
        document.getElementById("FormLocalFile").style.display = 'none';
        document.getElementById("txtUserId_hidden").style.visibility='hidden';
    }
    function ServerLocalCall(obj)
    {
        FieldName='cmbFrom';
        document.getElementById("txtUserId_hidden").style.visibility='hidden';
        if(obj == 'LocalCall')
        {   
            document.getElementById("FormServerFile").style.display = 'none';
            document.getElementById("FormLocalFile").style.display = 'inline';
        }
        if(obj == 'ServerCall')
        {   
            document.getElementById("FormServerFile").style.display = 'inline';
            document.getElementById("FormLocalFile").style.display = 'none';
        }
    }
        function height()
    {
      
        window.frameElement.height = document.body.scrollHeight;
        window.frameElement.Width = document.body.scrollWidth;
        
    }
    function btnCompose_click()
    {
        
    }
    function btnAdd_click()
    {
        var textmail = document.getElementById("txtUserId");
        var textmaillist = document.getElementById("txtUsermailIDs");
        if(textmail.value != '')
        {
            var filter=/^.+@.+\..{2,3}$/;
            var data = textmail.value;
            var idWname = data.split('<');
            var idlength = idWname.length;
            var mailID='';
            if(idlength == '1')
            {
                mailID = idWname[0];
            }
            else
            {
                mailID = idWname[1].substring(0, idWname[1].length - 1);
            }
            //alert(mailID);
            if (filter.test(mailID))
            {
                if(textmaillist.value == '')
                    textmaillist.value = textmail.value;
                else
                    textmaillist.value += ',' + textmail.value;
                
                textmail.value='';
            }
            else
            {
                alert('Input valid E-mail ID!');
                textmail.value='';
            }
        }
        else
            alert('Please fill ID first then Add to List of ID!');
    }
    
    function btnAddCC_click()
    {
        var textmail = document.getElementById("txtUserId");
        var textmaillist = document.getElementById("txtCC");
        if(textmail.value != '')
        {
            if(textmaillist.value == '')
                textmaillist.value = textmail.value;
            else
                textmaillist.value += ',' + textmail.value;
            
            textmail.value='';
        }
        else
            alert('Please fill E-mail ID first then Add to respective List of ID!');
    }
    function btnAddBCC_click()
    {
        var textmail = document.getElementById("txtUserId");
        var textmaillist = document.getElementById("txtBCc");
        if(textmail.value != '')
        {
            if(textmaillist.value == '')
                textmaillist.value = textmail.value;
            else
                textmaillist.value += ',' + textmail.value;
            
            textmail.value='';
        }
        else
            alert('Please fill ID first then Add to List of ID!');
    }
    function btnServerAtt_click()
    {
        document.getElementById("FormServerFile").style.display = 'inline';
        document.getElementById("FormLocalFile").style.display = 'none';
        document.getElementById("TRServerGrid").style.display = 'none';
        height();
        CallServer("ServerCall","");
        
    }
    function btnLocalAtt_click()
    {
        document.getElementById("FormServerFile").style.display = 'none';
        document.getElementById("FormLocalFile").style.display = 'inline';
        document.getElementById("TRLocalGrid").style.display = 'none';
        height();
        CallServer("LocalCall","");
    }
    
    
    function btnCancelAttach_click()
    {
        document.getElementById("FormServerFile").style.display = 'none';
        document.getElementById("FormLocalFile").style.display = 'none';
        grid.PerformCallback('cancel');
    }
    function btnremoveAttach_click()
    {
    
    }
    function btnAddLocal_click()
    {
        var fileupload = document.getElementById("FileUpload1");
        document.getElementById("TRLocalGrid").style.display = 'inline';
        var filename = fileupload.value;
        var file = filename.split('\\');
        var length = file.length;
        var data = 'addLocal' + '~' + filename + '~' + file[length-1];
        //alert(filename);
        var senddata="AttachLocal~"+filename;
        CallServer(senddata,"");
        gridLocal.PerformCallback(data);
    }
    function btnCancelAddLocal_click()
    {
        document.getElementById("FormServerFile").style.display = 'none';
        document.getElementById("FormLocalFile").style.display = 'none';
    }
    
    function btnCancelAttachLocal_click()
    {
        document.getElementById("FormServerFile").style.display = 'none';
        document.getElementById("FormLocalFile").style.display = 'none';
        var senddata = 'Canloc';
        gridLocal.PerformCallback(senddata);
    }
    function btnRemoveAttachLocal_click()
    {
        var senddata = 'remvloc~' + counterLocal;
        gridLocal.PerformCallback(senddata);
    }
    
    function btnCancelMail_click()
    {
    
    }
    function btnSearch_click()
    {
        var textboxDocu = document.getElementById("txtName");
        if(textboxDocu.value != '')
        {
            document.getElementById("TRServerGrid").style.display = 'inline';
            var combo = document.getElementById("drpDocumentEntity");
            var combo1 = document.getElementById("drpDocumentType");
            if(combo1.value == '')
            {
                alert('Please select Document Type!');
                return false;
            }
            var chek = document.getElementById("chkSearch");
            FieldName = 'cmbTemplate';
            var data= 'search~' + combo.value + '~' + combo1.value + '~' + chek.checked + '~' + textboxDocu.value;
            //CallServer(data,"");
            //alert(data);
            gridServer.PerformCallback(data);
            //alert('C');
        }
        else
            alert('Please Fill Document Name!');
    }
    
    </script>

    <script type="text/ecmascript">
    function callDoc()
    {
        var combo = document.getElementById("drpDocumentEntity");
        var data= 'Combo~' + combo.value;
        //alert(data);
        CallServer(data,"");
    }
    
    function callTemplateChange()
    {
        var combo = document.getElementById("cmbTemplate");
        var messageBox = document.getElementById("txtmailMessage");
        var senddata = '';
        if(combo.value !='')
        {
            var mailto = document.getElementById("txtUsermailIDs");
            var mailtolist = mailto.value;
            //alert(mailtolist);
            if(mailtolist == '')
            {
                alert('Please First Fill E-mail ID in Recipient!');
                var comboTemplate = document.getElementById("cmbTemplate");
                comboTemplate.selectedIndex  = 0;
                return false;
            }
            
            var maillistarray = mailtolist.split(',');
            //alert(maillistarray.length);
            //return false;
            if(maillistarray.length == 1)
            {
                    senddata = 'template~' + mailto.value + '~' + combo.value;
            }
            else
            {
                    senddata = 'template~all~' + combo.value;
                    
            }
           
            CallServer(senddata,"");
        }
        else
        {
            messageBox.value = '';
        }
    }
    
    function btnSendmail_click()
    {
        var messageBox = document.getElementById("txtmailMessage");
        var mailto = document.getElementById("txtUsermailIDs");
        var mailCC = document.getElementById("txtCC");
        var mailBCc = document.getElementById("txtBCc");
        var Subject = document.getElementById("txtSubject");
        var comboTemplate = document.getElementById("cmbTemplate");
        if(Subject.value == '')
        {
            alert('Please Fill Subject!');
            return false;
        }
        if(mailto.value == '')
        {
            alert('Please Fill E-mail Id of Recipient!');
            return false;
        }
        var senddata="sendmail~"+mailto.value+'~'+messageBox.value+'~'+Subject.value+'~'+mailCC.value+'~'+mailBCc.value+'~'+comboTemplate.value;
        CallServer(senddata,"");
    }
    function btnAttach_click()
    {
        
    }
    function btnAttachLocal_click()
    {
        var senddata="AttachLocal";
        CallServer(senddata,"");
    }
    
    
    
    function ReceiveServerData(rValue)
    {
        var DATA = rValue.split('~'); 
        //alert(rValue); 
        if(DATA[0]=="Combo")
        {
            var combo = document.getElementById("drpDocumentType");
            combo.length=0;
            var NoItems=DATA[1].split(';');
            var i;
            for(i=0;i<NoItems.length;i++)
            {
                var items = NoItems[i].split(',');
                var opt = document.createElement("option");
                opt.text = items[1];
                opt.value = items[0];
                combo.options.add(opt);
            }
        }
        if(DATA[0]=="template")
        {
            if(DATA[1] == 'Y')
            {
                var messageBox = document.getElementById("txtmailMessage");
                messageBox.value = DATA[2];
            }
            else
                alert(DATA[1]);
        }
        if(DATA[0]=="sendmail")
        {
            if(DATA[1] == 'Y')
            {
                alert(DATA[2]);
                AtTheTimePageLoad();
                var messageBox = document.getElementById("txtmailMessage");
                messageBox.value = '';
                var mailto = document.getElementById("txtUsermailIDs");
                mailto.value='';
                var mailCC = document.getElementById("txtCC");
                mailCC.value='';
                var mailBCc = document.getElementById("txtBCc");
                mailBCc.value='';
                var Subject = document.getElementById("txtSubject");
                Subject.value='';
                var comboTemplate = document.getElementById("cmbTemplate");
                comboTemplate.selectedIndex  = 0;
                
            }
            else
                alert(DATA[1]);
        }
    }

    </script>

    <script type="text/javascript" language="javascript">
    function callAjax(obj1,obj2,obj3)
    {
        var combo = document.getElementById("cmbContactType");
        var set_value = combo.value
        if (set_value=='16')
        {
           ajax_showOptions(obj1,'GetLeadId',obj3,set_value)
        }
        else
        {
            ajax_showOptions(obj1,obj2,obj3,set_value)	  
        }
    }
    
    function callAjaxDoc(obj1,obj2,obj3)
    {
        var ob = document.getElementById("drpDocumentEntity");
        var ob1 = document.getElementById("drpDocumentType");
        var ob2 = document.getElementById("chkSearch");
        var set_value = ob.value + '~' + ob1.value + '~' + ob2.checked;
        FieldName='cmbTemplate';
        ajax_showOptions(obj1,obj2,obj3,set_value);
        
    }

    </script>
    </asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
        <table class="TableMain100">
            <tr>
                <td class="EHEADER" colspan="2">
                    <asp:Label ID="lblHeader" runat="server" Font-Bold="True" ForeColor="#000099"></asp:Label></td>
            </tr>
            <tr>
                <td class="gridcellleft" style="vertical-align: top; width: 0%; visibility: hidden"
                    id="visibilityColumn">
                    <table>
                        <tr>
                            <td>
                                <input id="btnCompose" type="button" value="Compose" class="btnUpdate" onclick="btnCompose_click();"
                                    style="width: 62px; height: 19px" tabindex="1" />
                            </td>
                        </tr>
                    </table>
                </td>
                <td>
                    <asp:Panel ID="panel1" runat="server" BorderColor="#000099" BorderWidth="1px" Width="100%">
                        <table class="TableMain100">
                            <tr id="visibility" runat="server">
                                <td style="width: 10%">
                                    <asp:Label ID="Label13" runat="server" Text="To:" Width="103px" CssClass="mylabel1"></asp:Label></td>
                                <td class="gridcellleft">
                                    <asp:DropDownList ID="cmbContactType" runat="server" Width="150px" Font-Size="12px">
                                    </asp:DropDownList><asp:TextBox ID="txtUserId" runat="server" Font-Size="12px" Width="200px"></asp:TextBox><asp:TextBox
                                        ID="txtUserId_hidden" runat="server" Width="14px"></asp:TextBox>
                                    <input id="btnAdd" type="button" value="Add To" class="btnUpdate" onclick="btnAdd_click();"
                                        style="width: 66px; height: 19px" tabindex="1" />
                                    <input id="btnAddCC" type="button" value="Add Cc" class="btnUpdate" onclick="btnAddCC_click();"
                                        style="width: 66px; height: 19px" tabindex="1" />
                                    <input id="btnAddBc" type="button" value="Add Bcc" class="btnUpdate" onclick="btnAddBCC_click();"
                                        style="width: 66px; height: 19px" tabindex="1" />
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 10%">
                                    <asp:Label ID="Label1" runat="server" Text="To:" Width="103px" CssClass="mylabel1"></asp:Label></td>
                                <td class="gridcellleft">
                                    <asp:TextBox ID="txtUsermailIDs" runat="server" TextMode="MultiLine" Width="90%"
                                        Font-Size="12px"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 10%">
                                    <asp:Label ID="Label2" runat="server" Text="Cc:" Width="103px" CssClass="mylabel1"></asp:Label></td>
                                <td class="gridcellleft">
                                    <asp:TextBox ID="txtCC" runat="server" TextMode="MultiLine" Width="90%" Font-Size="12px"></asp:TextBox></td>
                            </tr>
                            <tr>
                                <td style="width: 10%">
                                    <asp:Label ID="Label3" runat="server" Text="Bcc:" Width="103px" CssClass="mylabel1"></asp:Label></td>
                                <td class="gridcellleft">
                                    <asp:TextBox ID="txtBCc" runat="server" TextMode="MultiLine" Width="90%" Font-Size="12px"></asp:TextBox></td>
                            </tr>
                            <tr>
                                <td style="width: 10%">
                                    <asp:Label ID="Label4" runat="server" Text="From:" Width="103px" CssClass="mylabel1"></asp:Label></td>
                                <td class="gridcellleft">
                                    <asp:DropDownList ID="cmbFrom" runat="server" Font-Size="12px" Width="304px">
                                    </asp:DropDownList></td>
                            </tr>
                            <tr>
                                <td style="width: 10%">
                                    <asp:Label ID="Label5" runat="server" Text="Subject:" Width="103px" CssClass="mylabel1"></asp:Label></td>
                                <td class="gridcellleft">
                                    <asp:TextBox ID="txtSubject" runat="server" Width="300px"></asp:TextBox></td>
                            </tr>
                            <tr>
                                <td style="width: 10%;">
                                    <asp:Label ID="Label6" runat="server" Text="Attachment:" Width="137px" CssClass="mylabel1"></asp:Label></td>
                                <td class="gridcellleft" style="height: 21px">
                                    <input id="btnServerAttach" type="button" value="Attach Server File" class="btnUpdate"
                                        onclick="btnServerAtt_click();" style="width: 121px; height: 19px" tabindex="1" />
                                    <input id="btnLocalAttach" type="button" value="Attach Local File" class="btnUpdate"
                                        onclick="btnLocalAtt_click();" style="width: 119px; height: 19px" tabindex="1" />
                                </td>
                            </tr>
                            <tr>
                            </tr>
                            <tr id="FormServerFile">
                                <td colspan="2">
                                    <table class="TableMain100">
                                        <tr>
                                            <td style="width: 10%">
                                                <asp:Label ID="Label7" runat="server" Text="Document Entry:" Width="103px" CssClass="mylabel1"></asp:Label></td>
                                            <td class="gridcellleft">
                                                <asp:DropDownList ID="drpDocumentEntity" runat="server" AutoPostBack="false" Width="304px"
                                                    Font-Size="12px">
                                                    <asp:ListItem>Products MF</asp:ListItem>
                                                    <asp:ListItem>Products Insurance</asp:ListItem>
                                                    <asp:ListItem>Products IPOs</asp:ListItem>
                                                    <asp:ListItem>Customer</asp:ListItem>
                                                    <asp:ListItem>Lead</asp:ListItem>
                                                    <asp:ListItem>Employee</asp:ListItem>
                                                    <asp:ListItem>Sub Brokers</asp:ListItem>
                                                    <asp:ListItem>Franchisees</asp:ListItem>
                                                    <asp:ListItem>Data Vendors</asp:ListItem>
                                                    <asp:ListItem>Referral Agents</asp:ListItem>
                                                    <asp:ListItem>Recruitment Agents</asp:ListItem>
                                                    <asp:ListItem>AMCs</asp:ListItem>
                                                    <asp:ListItem>Insurance Companies</asp:ListItem>
                                                    <asp:ListItem>RTAs</asp:ListItem>
                                                    <asp:ListItem>Branches</asp:ListItem>
                                                    <asp:ListItem>Companies</asp:ListItem>
                                                    <asp:ListItem>Building</asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 10%">
                                                <asp:Label ID="Label10" runat="server" Text="Document Type:" Width="103px" CssClass="mylabel1"></asp:Label></td>
                                            <td class="gridcellleft">
                                                <asp:DropDownList ID="drpDocumentType" runat="Server" Width="304px" Font-Size="12px">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 10%">
                                                <asp:Label ID="Label8" runat="server" Text="Search By Document Name:" Width="142px"
                                                    CssClass="mylabel1"></asp:Label>
                                            </td>
                                            <td class="gridcellleft">
                                                <asp:CheckBox ID="chkSearch" runat="server" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 10%">
                                                <asp:Label ID="Label9" runat="server" Text="Search By Name:" Width="103px" CssClass="mylabel1"></asp:Label></td>
                                            <td class="gridcellleft">
                                                <asp:TextBox ID="txtName" runat="Server" AutoCompleteType="Disabled" Width="300px"
                                                    Font-Size="12px"></asp:TextBox>
                                                <asp:TextBox ID="txtName_hidden" runat="server"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="gridcellright" style="width: 10%">
                                            </td>
                                            <td class="gridcellleft">
                                                <input id="btnSearch" type="button" value="Search" class="btnUpdate" onclick="btnSearch_click();"
                                                    style="width: 62px; height: 19px" tabindex="1" />
                                            </td>
                                        </tr>
                                        <tr id="TRServerGrid">
                                            <td colspan="2" class="gridcellleft">
                                                <table class="TableMain100">
                                                    <tr>
                                                        <td colspan="2">
                                                            <dxe:ASPxGridView ID="GridAttachment" ClientInstanceName="gridServer" runat="server"
                                                                CssFilePath="~/App_Themes/Office2003 Blue/{0}/styles.css" CssPostfix="Office2003_Blue"
                                                                Width="100%" KeyFieldName="FilePath" AutoGenerateColumns="False" OnCustomCallback="GridAttachment_CustomCallback">
                                                                <Styles CssFilePath="~/App_Themes/Office2003 Blue/{0}/styles.css" CssPostfix="Office2003_Blue">
                                                                    <LoadingPanel ImageSpacing="10px">
                                                                    </LoadingPanel>
                                                                    <Header ImageSpacing="5px" SortingImageSpacing="5px">
                                                                    </Header>
                                                                </Styles>
                                                                <SettingsPager AlwaysShowPager="True" NumericButtonCount="20" ShowSeparators="True"
                                                                    Visible="False">
                                                                    <FirstPageButton Visible="True">
                                                                    </FirstPageButton>
                                                                    <LastPageButton Visible="True">
                                                                    </LastPageButton>
                                                                </SettingsPager>
                                                                <SettingsBehavior AllowMultiSelection="True" AllowSort="False" />
                                                                <ClientSideEvents SelectionChanged="function(s, e) { OnGridSelectionChanged(); }" />
                                                                <Images ImageFolder="~/App_Themes/Office2003 Blue/{0}/">
                                                                    <ExpandedButton Height="12px" Url="~/App_Themes/Office2003 Blue/GridView/gvExpandedButton.png"
                                                                        Width="11px" />
                                                                    <CollapsedButton Height="12px" Url="~/App_Themes/Office2003 Blue/GridView/gvCollapsedButton.png"
                                                                        Width="11px" />
                                                                    <DetailCollapsedButton Height="12px" Url="~/App_Themes/Office2003 Blue/GridView/gvCollapsedButton.png"
                                                                        Width="11px" />
                                                                    <DetailExpandedButton Height="12px" Url="~/App_Themes/Office2003 Blue/GridView/gvExpandedButton.png"
                                                                        Width="11px" />
                                                                    <FilterRowButton Height="13px" Width="13px" />
                                                                </Images>
                                                                <Columns>
                                                                    <dxe:GridViewCommandColumn ShowSelectCheckbox="True" VisibleIndex="0" Width="3%">
                                                                        <HeaderStyle HorizontalAlign="Center">
                                                                            <Paddings PaddingBottom="1px" PaddingTop="1px" />
                                                                        </HeaderStyle>
                                                                        <HeaderTemplate>
                                                                            <input type="checkbox" onclick="gridServer.SelectAllRowsOnPage(this.checked);OnGridServerSelectAll(this.checked);"
                                                                                style="vertical-align: middle;" title="Select/Unselect all rows on the page"></input>
                                                                        </HeaderTemplate>
                                                                    </dxe:GridViewCommandColumn>
                                                                    <dxe:GridViewDataTextColumn Caption="Document Type" FieldName="Type" ReadOnly="True"
                                                                        VisibleIndex="1">
                                                                        <CellStyle CssClass="gridcellleft">
                                                                        </CellStyle>
                                                                        <EditFormSettings Visible="False" />
                                                                    </dxe:GridViewDataTextColumn>
                                                                    <dxe:GridViewDataTextColumn Caption="Document Name" FieldName="FileName" ReadOnly="True"
                                                                        VisibleIndex="2">
                                                                        <CellStyle CssClass="gridcellleft">
                                                                        </CellStyle>
                                                                        <EditFormSettings Visible="False" />
                                                                    </dxe:GridViewDataTextColumn>
                                                                    <dxe:GridViewDataTextColumn Caption="Document Physical Location" FieldName="FilePath"
                                                                        ReadOnly="True" VisibleIndex="3">
                                                                        <CellStyle CssClass="gridcellleft">
                                                                        </CellStyle>
                                                                        <EditFormSettings Visible="False" />
                                                                    </dxe:GridViewDataTextColumn>
                                                                    <dxe:GridViewDataHyperLinkColumn Caption="View Doc." VisibleIndex="4" Width="5%">
                                                                        <PropertiesHyperLinkEdit Target="_blank" Text="View" NavigateUrlFormatString="viewImage.aspx?id={3}">
                                                                        </PropertiesHyperLinkEdit>
                                                                    </dxe:GridViewDataHyperLinkColumn>
                                                                </Columns>
                                                            </dxe:ASPxGridView>
                                                            &nbsp; &nbsp;
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 10%">
                                                        </td>
                                                        <td class="gridcellleft">
                                                            <input id="btnAttach" type="button" value="Attach" class="btnUpdate" onclick="btnAttach_click();"
                                                                style="width: 62px; height: 19px" tabindex="1" />
                                                            <input id="btnCancelAttach" type="button" value="Cancel" class="btnUpdate" onclick="btnCancelAttach_click();"
                                                                style="width: 62px; height: 19px" tabindex="1" />
                                                            <input id="btnremoveAttach" type="button" value="Remove" class="btnUpdate" onclick="btnremoveAttach_click();"
                                                                style="width: 62px; height: 19px" tabindex="1" /></td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr id="FormLocalFile">
                                <td colspan="2" class="gridcellleft">
                                    <table class="TableMain100">
                                        <tr>
                                            <td class="gridcellright" style="width: 54px">
                                            </td>
                                            <td style="width: auto">
                                                <asp:FileUpload ID="FileUpload1" runat="server" Width="314px" Font-Size="12px" />
                                                <asp:Button ID="Button1" runat="server" OnClick="Button1_Click" Text="Upload" CssClass="btnUpdate"
                                                    Height="19px" Width="62px" />
                                                <input id="btnCancelAddLocal" type="button" value="Cancel" class="btnUpdate" onclick="btnCancelAddLocal_click();"
                                                    style="width: 62px; height: 19px" tabindex="1" />
                                            </td>
                                        </tr>
                                        <tr id="TRLocalGrid">
                                            <td colspan="2">
                                                <table class="TableMain100">
                                                    <tr>
                                                        <td>
                                                            <dxe:ASPxGridView ID="GridAttachmentLocal" ClientInstanceName="gridLocal" runat="server"
                                                                CssFilePath="~/App_Themes/Office2003 Blue/{0}/styles.css" CssPostfix="Office2003_Blue"
                                                                Width="100%" KeyFieldName="filepathServer" AutoGenerateColumns="False" OnCustomCallback="GridAttachmentLocal_CustomCallback">
                                                                <Styles CssFilePath="~/App_Themes/Office2003 Blue/{0}/styles.css" CssPostfix="Office2003_Blue">
                                                                    <LoadingPanel ImageSpacing="10px">
                                                                    </LoadingPanel>
                                                                    <Header ImageSpacing="5px" SortingImageSpacing="5px">
                                                                    </Header>
                                                                </Styles>
                                                                <SettingsPager AlwaysShowPager="True" NumericButtonCount="20" ShowSeparators="True"
                                                                    Visible="False">
                                                                    <FirstPageButton Visible="True">
                                                                    </FirstPageButton>
                                                                    <LastPageButton Visible="True">
                                                                    </LastPageButton>
                                                                </SettingsPager>
                                                                <SettingsBehavior AllowMultiSelection="True" />
                                                                <ClientSideEvents SelectionChanged="function(s, e) { OnGridLocalSelectionChanged(); }" />
                                                                <Images ImageFolder="~/App_Themes/Office2003 Blue/{0}/">
                                                                    <ExpandedButton Height="12px" Url="~/App_Themes/Office2003 Blue/GridView/gvExpandedButton.png"
                                                                        Width="11px" />
                                                                    <CollapsedButton Height="12px" Url="~/App_Themes/Office2003 Blue/GridView/gvCollapsedButton.png"
                                                                        Width="11px" />
                                                                    <DetailCollapsedButton Height="12px" Url="~/App_Themes/Office2003 Blue/GridView/gvCollapsedButton.png"
                                                                        Width="11px" />
                                                                    <DetailExpandedButton Height="12px" Url="~/App_Themes/Office2003 Blue/GridView/gvExpandedButton.png"
                                                                        Width="11px" />
                                                                    <FilterRowButton Height="13px" Width="13px" />
                                                                </Images>
                                                                <Columns>
                                                                    <dxe:GridViewCommandColumn ShowSelectCheckbox="True" VisibleIndex="0" Width="3%">
                                                                        <HeaderStyle HorizontalAlign="Center">
                                                                            <Paddings PaddingBottom="1px" PaddingTop="1px" />
                                                                        </HeaderStyle>
                                                                        <HeaderTemplate>
                                                                            <input type="checkbox" onclick="gridLocal.SelectAllRowsOnPage(this.checked);OnGridLocalSelectAll(this.checked);"
                                                                                style="vertical-align: middle;" title="Select/Unselect all rows on the page"></input>
                                                                        </HeaderTemplate>
                                                                    </dxe:GridViewCommandColumn>
                                                                    <dxe:GridViewDataTextColumn Caption="File Name" FieldName="filename" ReadOnly="True"
                                                                        VisibleIndex="1">
                                                                        <CellStyle CssClass="gridcellleft">
                                                                        </CellStyle>
                                                                        <EditFormSettings Visible="False" />
                                                                    </dxe:GridViewDataTextColumn>
                                                                    <dxe:GridViewDataTextColumn Caption="File Path" FieldName="filepath" ReadOnly="True"
                                                                        VisibleIndex="2">
                                                                        <CellStyle CssClass="gridcellleft">
                                                                        </CellStyle>
                                                                        <EditFormSettings Visible="False" />
                                                                    </dxe:GridViewDataTextColumn>
                                                                    <dxe:GridViewDataTextColumn Caption="Server File Path" FieldName="filepathServer"
                                                                        ReadOnly="True" VisibleIndex="2">
                                                                        <CellStyle CssClass="gridcellleft">
                                                                        </CellStyle>
                                                                        <EditFormSettings Visible="False" />
                                                                    </dxe:GridViewDataTextColumn>
                                                                </Columns>
                                                            </dxe:ASPxGridView>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="gridcellcenter">
                                                            <input id="btnRemoveAttachLocal" type="button" value="Remove" class="btnUpdate" onclick="btnRemoveAttachLocal_click();"
                                                                style="width: 62px; height: 19px" tabindex="1" />
                                                            <input id="btnCancelAttachLocal" type="button" value="Cancel" class="btnUpdate" onclick="btnCancelAttachLocal_click();"
                                                                style="width: 62px; height: 19px" tabindex="1" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td >
                                    <asp:Label ID="Label11" runat="server" Text="Templates:" Width="84px" CssClass="mylabel1"></asp:Label></td>
                                <td class="gridcellleft">
                                    <asp:DropDownList ID="cmbTemplate" runat="server" Font-Size="12px" Width="200px">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td style=" vertical-align: top">
                                    <asp:Label ID="Label12" runat="server" Text="Body: " Width="103px" CssClass="mylabel1"></asp:Label></td>
                                <td class="gridcellleft" style="vertical-align: top">
                                    <asp:TextBox ID="txtmailMessage" runat="server" TextMode="MultiLine" Width="90%"
                                        Height="200px" Font-Size="12px"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td >
                                </td>
                                <td class="gridcellleft">
                                    <input id="btnSendmail" type="button"   value="Send Mail" class="btnUpdate"  onclick ="btnSendmail_click();"
                                        style="width: 62px; height: 19px" tabindex="1" />
                                    <input id="btnCancelMail" type="button" value="Cancel" class="btnUpdate" onclick="btnCancelMail_click();"
                                        style="width: 62px; height: 19px" tabindex="1" />&nbsp;
                                </td>
                            </tr>
                        </table>
                        </asp:Panel>
                    <asp:Button ID="Button2" runat="server" OnClick="Button2_Click" Text="Button" /></td>
            </tr>
        </table>
      </asp:Content>