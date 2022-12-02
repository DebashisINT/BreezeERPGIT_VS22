<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/OMS/MasterPage/ERP.Master"
    Inherits="ERP.OMS.Management.SettingsOptions.management_SettingsOptions_UploadDigitalSignature_AuthUser" Codebehind="UploadDigitalSignature_AuthUser.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

   

    <style type="text/css">
	
	/* Big box with list of options */
	#ajax_listOfOptions{
		position:absolute;	/* Never change this one */
		width:50px;	/* Width of box */
		height:auto;	/* Height of box */
		overflow:auto;	/* Scrolling features */
		border:1px solid Blue;	/* Blue border */
		background-color:#FFF;	/* White background color */
		text-align:left;
		font-size:0.9em;
		z-index:100;
	}
	#ajax_listOfOptions div{	/* General rule for both .optionDiv and .optionDivSelected */
		margin:1px;		
		padding:1px;
		cursor:pointer;
		font-size:0.9em;
	}
	#ajax_listOfOptions .optionDiv{	/* Div for each item in list */
		
	}
	#ajax_listOfOptions .optionDivSelected{ /* Selected item in the list */
		background-color:#DDECFE;
		color:Blue;
	}
	#ajax_listOfOptions_iframe{
		background-color:#F00;
		position:absolute;
		z-index:5;
	}
	
	form{
		display:inline;
	}
	
	</style>

    <script language="javascript" type="text/javascript">
   
    function PageLoad()
    {
    FieldName='A4'; 

      document.getElementById('txtValidUser_hidden').style.display="none";

    }
    function CallAjax(obj1,obj2,obj3)
         {
          ajax_showOptions(obj1,obj2,obj3);
         }
    function ShowHideFilter(obj) 
    { 
    grid.PerformCallback(obj);
    }
    function closeWin()
        {
        parent.editwin.close();
       // parent.DhtmlClose();
        }
    
    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
        <table id="tblSummary" border="0" cellpadding="0" cellspacing="0" class="TableMain100"
            style="width: 100%; padding-right: 0px; padding-left: 0px; padding-bottom: 0px;
            padding-top: 0px;">
            <tr>
                <td colspan="2" style="text-align: left;">
                </td>
                <td class="gridcellright" colspan="4" valign="top">
                </td>
            </tr>
            <tr>
                <td colspan="6" style="text-align: left">
                    <dxe:ASPxGridView ID="gridAuthUser" runat="server" AutoGenerateColumns="False"
                        ClientInstanceName="grid" KeyFieldName="col" Width="100%" OnRowDeleting="gridAuthUser_RowDeleting"
                        OnRowInserting="gridAuthUser_RowInserting">
                        <templates><EditForm>
                                <table>
                                    <tr>
                                        <td>
                                            Name:
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtValidUser" runat="server" onkeyup="CallAjax(this,'SearchEmployeesForDigitalSignatureUser',event)"
                                                Width="238px"></asp:TextBox>
                                            <asp:HiddenField ID="txtValidUser_hidden" runat="server" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                        </td>
                                        <td>
                                            <table>
                                                <tr>
                                                    <td>
                                                        <dxe:ASPxButton ID="btnAdd" runat="server" AutoPostBack="False" Text="Add" ValidationGroup="a">
                                                            <ClientSideEvents Click="function(s, e) {grid.UpdateEdit();}" />
                                                        </dxe:ASPxButton>
                                                    </td>
                                                    <td>
                                                        <dxe:ASPxButton ID="btnCancel" runat="server" AutoPostBack="False" Text="Cancel"
                                                            ValidationGroup="a">
                                                            <ClientSideEvents Click="function(s, e) {grid.CancelEdit();}" />
                                                        </dxe:ASPxButton>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            
</EditForm>
</templates>
                        <settingsbehavior allowfocusedrow="True" confirmdelete="True" />
                        <styles>
<Header CssClass="gridheader"></Header>

<FocusedGroupRow CssClass="gridselectrow"></FocusedGroupRow>

<FocusedRow CssClass="gridselectrow"></FocusedRow>
</styles>
                        <settingspager alwaysshowpager="True" showseparators="True">
<FirstPageButton Visible="True"></FirstPageButton>

<LastPageButton Visible="True"></LastPageButton>
</settingspager>
                        <columns>
<dxe:GridViewDataTextColumn FieldName="name" Caption="Name" VisibleIndex="0">
<Settings FilterMode="DisplayText" AutoFilterCondition="Contains"></Settings>

<CellStyle Wrap="False" CssClass="gridcellleft"></CellStyle>
</dxe:GridViewDataTextColumn>
<dxe:GridViewCommandColumn Width="10%" VisibleIndex="1" ShowDeleteButton="True">
    <HeaderTemplate>
        <a href="javascript:void(0);" onclick="grid.AddNewRow()">
            <span style="color: #000099;
                                                    text-decoration: underline">Add New</span>
        </a>
    </HeaderTemplate>
</dxe:GridViewCommandColumn>
</columns>
                        <settings showstatusbar="Visible" />
                        <styleseditors>
<ProgressBar Height="25px"></ProgressBar>
</styleseditors>
                    </dxe:ASPxGridView>
                </td>
            </tr>
            <tr>
                <td colspan="6" style="text-align: left; padding-left: 10px">
                    <br />
                    <asp:Button ID="btnClose" runat="server" CssClass="btnUpdate btn btn-primary" Height="23px" OnClientClick="javascript:closeWin();"
                        Text="Close" Width="101px" />
                </td>
            </tr>
        </table>
</asp:Content>
