<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/OMS/MasterPage/ERP.Master"
    Inherits="ERP.OMS.Management.ToolsUtilities.management_utilities_showhistory_reminder" Codebehind="showhistory_reminder.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../../CSS/style.css" rel="stylesheet" type="text/css" />
    <%-- <script type="text/javascript" src="/assests/js/init.js"></script>

    <script type="text/javascript" src="/assests/js/ajaxList.js"></script>

    --%>
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
		z-index:32767;
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
		z-index:3000;
	}
	
	form{
		display:inline;
	}
	

	</style>
    
    <%--<script type="text/javascript" src="/assests/js/loaddata1.js">--%>
    <link rel="stylesheet" href="../../windowfiles/dhtmlwindow.css" type="text/css" />

    <script type="text/javascript" src="../../windowfiles/dhtmlwindow.js"></script>

    <link rel="stylesheet" href="../../modalfiles/modal.css" type="text/css" />

    <script type="text/javascript" src="../../modalfiles/modal.js"></script>

    <script language="javascript" type="text/javascript">

  FieldName='abcd';
   
    function SignOff()
    {
        window.parent.SignOff();
    }
      

    function height()
    {
        if(document.body.scrollHeight>=300)
            window.frameElement.height = document.body.scrollHeight;
        else
            window.frameElement.height = '300px';
        window.frameElement.Width = document.body.scrollWidth;
    }
    function Page_Load()
    {
        //document.getElementById('Div1').style.display="none";
    }
     function FillValues(obj)
    {
    parent.editwin.close(obj);
     
    }
    </script>
</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
        <div>
            <asp:ScriptManager ID="ScriptManager1" runat="server" AsyncPostBackTimeout="3600">
            </asp:ScriptManager>
            <table class="TableMain100">
                <tr>
                    <td class="EHEADER" style="text-align: center;">
                        <strong><span style="color: #000099">View Task Updation Log</span></strong>
                    </td>
                </tr>
                 <tr>
                                            <td style="text-align: left; width: 995px; vertical-align: top;" id="FormGrid">
                                                <dxe:ASPxGridView ID="GridReminder" ClientInstanceName="grid" runat="server" KeyFieldName="Rid" 
                                                    AutoGenerateColumns="False" 
                                                    
                                                    Width="100%">
                                                    <Styles>
                                                        <LoadingPanel ImageSpacing="10px">
                                                        </LoadingPanel>
                                                        <Header ImageSpacing="5px" SortingImageSpacing="5px">
                                                        </Header>
                                                        
                                                    </Styles>
                                                   
                                                    <%--<SettingsPager AlwaysShowPager="True" NumericButtonCount="20" ShowSeparators="True">
                                                        <FirstPageButton Visible="True">
                                                        </FirstPageButton>
                                                        <LastPageButton Visible="True">
                                                        </LastPageButton>
                                                    </SettingsPager>
                                                    <SettingsBehavior AllowFocusedRow="True" AllowSort="False" />--%>
                                                    <%--<ClientSideEvents FocusedRowChanged="function(s, e) { OnGridFocusedRowChanged(); }" />--%>
                                                    <ClientSideEvents SelectionChanged="function(s, e) { OnGridFocusedRowChanged(); }" />
                                        <Columns>
                                           
                                                        <dxe:GridViewDataTextColumn Caption="Create By" FieldName="CreateBy" ReadOnly="True"
                                                            VisibleIndex="1">
                                                            <EditFormSettings Visible="False" />
                                                        </dxe:GridViewDataTextColumn>
                                                        <dxe:GridViewDataTextColumn Caption="Create Date" FieldName="CreateDate" ReadOnly="True"
                                                            VisibleIndex="2">
                                                            <EditFormSettings Visible="False" />
                                                        </dxe:GridViewDataTextColumn>
                                                         <dxe:GridViewDataTextColumn Caption="Token #" FieldName="Rid" ReadOnly="True" VisibleIndex="3">
                                                            <EditFormSettings Visible="False" />
                                                        </dxe:GridViewDataTextColumn>
                                                        <dxe:GridViewDataTextColumn Caption="Task For" FieldName="Target" ReadOnly="True"
                                                            VisibleIndex="4">
                                                            <EditFormSettings Visible="False" />
                                                        </dxe:GridViewDataTextColumn>
                                                        <dxe:GridViewDataTextColumn Caption="Follow-up By" FieldName="incharge" ReadOnly="True"
                                                            VisibleIndex="5">
                                                            <EditFormSettings Visible="False" />
                                                        </dxe:GridViewDataTextColumn>
                                                        
                                                       <dxe:GridViewDataTextColumn Caption="Comp" FieldName="company" ReadOnly="True"
                                                            VisibleIndex="6">
                                                            <EditFormSettings Visible="False" />
                                                        </dxe:GridViewDataTextColumn>
                                                        <dxe:GridViewDataTextColumn Caption="Subject" FieldName="shortname" ReadOnly="True"
                                                            VisibleIndex="7">
                                                            <EditFormSettings Visible="False" />
                                                        </dxe:GridViewDataTextColumn>
                                                       
                                                        <dxe:GridViewDataTextColumn Caption="Task" FieldName="Content" ReadOnly="True"
                                                            VisibleIndex="8">
                                                            <EditFormSettings Visible="False" />
                                                        </dxe:GridViewDataTextColumn>
                                                        <dxe:GridViewDataTextColumn Caption="Task updates" FieldName="replycontent" ReadOnly="True"
                                                            VisibleIndex="9" Width="100px">
                                                            <EditFormSettings Visible="False" />
                                                            <%-- <DataItemTemplate>
                                        <a href="javascript:void(0);" onclick="OnCreateActivityClick('<%# Eval("Rid") %>')">
                                            View</a>
                                    
</DataItemTemplate>--%>
                                                        </dxe:GridViewDataTextColumn>
                                                         <dxe:GridViewDataTextColumn Caption="Prty" FieldName="priority" ReadOnly="True"
                                                            VisibleIndex="10">
                                                            <EditFormSettings Visible="False" />
                                                        </dxe:GridViewDataTextColumn>
                                                        <dxe:GridViewDataTextColumn Caption="Start By" FieldName="StartDate" ReadOnly="True"
                                                            VisibleIndex="11">
                                                            <EditFormSettings Visible="False" />
                                                        </dxe:GridViewDataTextColumn>
                                                        <dxe:GridViewDataTextColumn Caption="Finish By" FieldName="EndDate" ReadOnly="True"
                                                            VisibleIndex="12">
                                                            <EditFormSettings Visible="False" />
                                                        </dxe:GridViewDataTextColumn>
                                                        <dxe:GridViewDataTextColumn Caption="Status" FieldName="Status" ReadOnly="True"
                                                            VisibleIndex="13">
                                                            <EditFormSettings Visible="False" />
                                                        </dxe:GridViewDataTextColumn>
                                                        <dxe:GridViewDataTextColumn Caption="Attend Time" FieldName="attenddate" ReadOnly="True"
                                                            VisibleIndex="14">
                                                            <EditFormSettings Visible="False" />
                                                        </dxe:GridViewDataTextColumn>
                                                        <dxe:GridViewDataTextColumn Caption="Type" FieldName="flag" ReadOnly="True" VisibleIndex="15">
                                                            <EditFormSettings Visible="False" />
                                                        </dxe:GridViewDataTextColumn>
                                                        
                                                        
                                                        
                                                    </Columns>
                                                </dxe:ASPxGridView>
                                            </td>
                                        </tr>
            </table>
            <table>
            <tr>
           <%-- <td style="width: 20px;"></td>--%>
            <td align="left" id="td_print" runat="server">
                        <asp:Button ID ="btnprint" runat="server" CssClass="btn" Text="Print Task Updation Log" Width="150px"
                        OnClick="btnprint_Click" />
                        </td>
                        <%--<td style="width: 20px;"></td>--%>
                        <td>
                        <asp:Button ID="btnAdd" runat="server" Text="Add Remarks/Note" CssClass="btn" Width="150px" OnClick="btnAdd_Click" />
                        </td>
            </tr>
            
            </table>
            <table>
              <tr id="tr_insert" runat="server">
                <td>
                    <table>
                        <td style="width: 150px; text-align: right;">
                            Content :</td>
                        <td style="width: 150%" align="left">
                            <asp:TextBox ID="txtReportTo" runat="server" Width="600px" TextMode="MultiLine" Height="50px"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtReportTo"
                                ErrorMessage="Required." ValidationGroup="a"></asp:RequiredFieldValidator>
                            <%--<asp:HiddenField ID="txtReportTo_hidden" runat="server" />--%>
                        </td>
                        <%--</tr>
    <tr>--%>
                    </table>
                </td>
            </tr>
            <tr id="tr_btn" runat="server">
                <td>
                    <table>
                         <td style="width: 20px;"></td>
                        <td align="left" id="td_yes" runat="server">
                            <asp:Button ID="btnYes" runat="server" CssClass="btn" Text="Save" Width="120px" OnClick="btnYes_Click"
                                ValidationGroup="a" />
                        </td>
                       <td style="width: 20px;"></td>
                        <td align="left" id="td_no" runat="server">
                            <asp:Button ID="btnNo" runat="server" CssClass="btn" Text="Cancel" Width="120px"
                                OnClick="btnNo_Click" />
                        </td>
                       
                        
                    </table>
                </td>
            </tr>
            </table>
            <div style="border: 1px solid black">
                <asp:Repeater ID="rptConversion" runat="server">
                    <HeaderTemplate>
                        <div style="width: 100%">
                            <div style="float: left; text-align: left; width: 5%; font-weight: bold">
                                Serial</div>
                            <div style="float: left; text-align: left; width: 15%; font-weight: bold">
                                Name</div>
                            <div style="float: left; text-align: left; width: 66%; font-weight: bold">
                                Content</div>
                            <div style="float: left; text-align: left; width: 12%; font-weight: bold">
                                Time</div>
                        </div>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <div style="width: 100%;">
                            <div style="float: left; text-align: left; width: 5%">
                                <%#Container.ItemIndex+1%>
                            </div>
                            <div style="float: left; text-align: left; width: 15%">
                                <%#Eval("name")%>
                            </div>
                            <div style="float: left; text-align: left; width: 66%">
                                <%#Eval("content")%>
                            </div>
                            <div style="float: left; text-align: left; width: 12%">
                                <%#Eval("time")%>
                            </div>
                        </div>
                    </ItemTemplate>
                    <SeparatorTemplate>------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------</SeparatorTemplate>
                    <FooterTemplate>
                    </FooterTemplate>
                </asp:Repeater>
            </div>
          
        </div>
        <asp:HiddenField ID="HiddenField1" runat="server" />
         <tr> 
                <td>
                            <div id="display" runat="server">
                            </div>
                </td>
            </tr>
</asp:Content>
