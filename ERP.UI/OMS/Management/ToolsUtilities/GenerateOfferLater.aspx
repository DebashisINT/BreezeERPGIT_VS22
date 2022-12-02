<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/OMS/MasterPage/ERP.Master"
    Inherits="ERP.OMS.Management.ToolsUtilities.management_utilities_GenerateOfferLater" Codebehind="GenerateOfferLater.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    

    

    <script language="javascript" type="text/javascript">
    
    //function callheight(obj)
    //{
    //    height();
    //   // parent.CallMessage();
    //}
    
    function ShowHideFilter(obj)
    {
        var showrecord='Show~'+obj
        grid.PerformCallback(showrecord);
         // height();
    } 
    
        function DeleteRow(keyValue)
        {
            doIt=confirm('Confirm delete?');
            if(doIt)
                {
                   grid.PerformCallback('Delete~'+keyValue);
                   //height();
                }
            else{
                  
                }

   
        }    
        function PageLoadFirst()
        {

        counter = 'n';
        FieldName = "drp_accesslevel";

        }
        function ReceiveServerData(rValue)
        {
        var DATA = rValue.split('~'); 
            if(DATA[0]=="read")
            {
                if(DATA[1]=="Y")
                {
                alert('Read Successfully!');
                grid.PerformCallback('read');
                grid.UnselectAllRowsOnPage();
                }
                else if(DATA[1]=="S")
                alert('Please Select a message!');
            }
        } 
       function callback()
        {
            grid.PerformCallback();
        } 
    
        function OnGridSelectionChanged() 
        {
        grid.GetSelectedFieldValues('rde_id', OnGridSelectionComplete);
        }
    
       function OnGridSelectionComplete(values) 
         {
         counter = 'n';
        
         for(var i = 0; i < values.length; i ++) {
            if(counter != 'n')
                counter += ',' + values[i];
             
            else
                counter = values[i];
              
          }
        
          var ReadIDs= 'read~' + counter;
            CallServer(ReadIDs,"");
        //alert(counter+'test');
      }
    
    
    //function SignOff()
    //{
    //    window.parent.SignOff();
    //}
    //function height()
    //{
   
    //    if(document.body.scrollHeight>=600)
    //        window.frameElement.height = document.body.scrollHeight+600;
    //    else
    //        window.frameElement.height = '600px';
    //    window.frameElement.Width = document.body.scrollWidth+100;
    //}
       function ClickOnAddButton()
    {
          var url='AddCandidateForOLetter.aspx?id=ADD&mode=new';
         OnMoreInfoClick(url,"Add Candidates",'960px','550px',"Y");
    }
   function OnMoreInfoClick(keyValue)
    {
    
       var url='AddCandidateForOLetter.aspx?id='+ keyValue+'&mode=edit' ;
       OnMoreInfoClick(url,"Edit Candidate",'960px','550px',"Y");
   
    }
   
   function OnMoreShowButtonClick(keyValue)
   {
   
       var url='AddCandidateForOLetter.aspx?id='+ keyValue+'&mode=show' ;
       OnMoreInfoClick(url,"Edit Candidate",'960px','550px',"Y");
   }
   
    
   function OnMoreAccessCick(keyValue)
    {
     grid.PerformCallback('Access~'+keyValue);
                   //height();
//       var url='AddCandidateForOLetter.aspx?id='+ keyValue+'&mode=edit' ;
//       OnMoreInfoClick(url,"Edit Candidate",'960px','550px',"Y");
   
    }
    
    </script>
</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
        <div>
            <table class="TableMain100">
                <tr>
                    <td class="EHEADER" style="text-align: center;">
                        <strong><span style="color: #000099">Generate Offer Letter</span></strong>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: left; vertical-align: top">
                        <table width="100%">
                            <tr>
                                <td>
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:Button ID="btnGenerate" Text="Generate" runat="server" CssClass="btnUpdate"
                                                    OnClick="btnGenerate_Click1" /></td>
                                            <td>
                                                Confirm Joining Date:
                                            </td>
                                            <td>
                                                <dxe:ASPxDateEdit ID="txtJD" runat="server" EditFormat="Custom" UseMaskBehavior="True"
                                                    TabIndex="20" Width="204px">
                                                    <ButtonStyle Width="13px">
                                                    </ButtonStyle>
                                                </dxe:ASPxDateEdit>
                                            </td>
                                            <td>
                                                <asp:Button ID="BtnJoin" runat="server" Text="Confirm Join" CssClass="btnUpdate"
                                                    OnClick="BtnJoin_Click" /></td>
                                            <td>
                                                <asp:Button ID="btnAdd" runat="server" Text="Add Employee" CssClass="btnUpdate" OnClick="btnAdd_Click"
                                                    Visible="false" /></td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td id="Td1" align="left">
                                    <a href="javascript:ShowHideFilter('s');" class="btn btn-success"><span >
                                        Show Filter</span></a> <a href="javascript:ShowHideFilter('All');" class="btn btn-primary"><span>All Records</span></a>
                                </td>
                                <td>
                                    <span id="spanshow2" style="color: Blue; font-weight: bold"></span>&nbsp;&nbsp;<span
                                        id="spanshow3"></span>
                                </td>
                                <td align="right" style="text-align: left">
                                    <span id="spanfltr" style="display: none"><a href="#" style="font-weight: bold; color: Blue"
                                        onclick="javascript:ForFilterOn();">Filter</a></span>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>
                       <%-- <div style="border: 1px solid black; width: 1000px; height: 650px; overflow: auto"
                            id="panecontainer">--%>
                            <dxe:ASPxGridView ID="GridMessage" ClientInstanceName="grid" runat="server" Width="100%"
                                KeyFieldName="rde_id" OnCustomCallback="GridMessage_CustomCallback" AutoGenerateColumns="False">
                                <ClientSideEvents SelectionChanged="function(s, e) { OnGridSelectionChanged(); }"
                                    BeginCallback="function(s, e) {
	callheight(s.cpHeight);
}" />
                                <SettingsBehavior AllowMultiSelection="True" />
                                <Styles>
                                    <Header SortingImageSpacing="5px" ImageSpacing="5px" BackColor="LightSteelBlue">
                                    </Header>
                                    <FocusedRow BackColor="#FFC080">
                                    </FocusedRow>
                                    <LoadingPanel ImageSpacing="10px">
                                    </LoadingPanel>
                                    <FocusedGroupRow BackColor="#FFC080">
                                    </FocusedGroupRow>
                                </Styles>
                                <SettingsPager AlwaysShowPager="True" NumericButtonCount="20" ShowSeparators="True">
                                    <FirstPageButton Visible="True">
                                    </FirstPageButton>
                                    <LastPageButton Visible="True">
                                    </LastPageButton>
                                </SettingsPager>
                                <Columns>
                                    <dxe:GridViewCommandColumn VisibleIndex="0" Width="3%" ShowSelectCheckbox="True">
                                        <HeaderStyle HorizontalAlign="Center">
                                            <Paddings PaddingTop="1px" PaddingBottom="1px"></Paddings>
                                        </HeaderStyle>
                                        <HeaderTemplate>
                                            <input type="checkbox" onclick="grid.SelectAllRowsOnPage(this.checked);" style="vertical-align: middle;"
                                                title="Select/Unselect all rows on the page"></input>
                                        </HeaderTemplate>
                                    </dxe:GridViewCommandColumn>
                                    <dxe:GridViewDataTextColumn ReadOnly="True" VisibleIndex="1" FieldName="rde_Name"
                                        Caption="Cand. Name">
                                        <CellStyle CssClass="gridcellleft">
                                        </CellStyle>
                                        <EditFormSettings Visible="False"></EditFormSettings>
                                    </dxe:GridViewDataTextColumn>
                                    <dxe:GridViewDataTextColumn Visible="False" ReadOnly="True" VisibleIndex="2" FieldName="rde_ResidenceLocation"
                                        Caption="Address">
                                        <CellStyle CssClass="gridcellleft">
                                        </CellStyle>
                                        <EditFormSettings Visible="False"></EditFormSettings>
                                    </dxe:GridViewDataTextColumn>
                                    <dxe:GridViewDataTextColumn ReadOnly="True" VisibleIndex="2" FieldName="company"
                                        Caption="Candt. Company">
                                        <CellStyle CssClass="gridcellleft">
                                        </CellStyle>
                                        <EditFormSettings Visible="False"></EditFormSettings>
                                    </dxe:GridViewDataTextColumn>
                                    <dxe:GridViewDataTextColumn ReadOnly="True" VisibleIndex="3" FieldName="Branch"
                                        Caption="Candt. Branch">
                                        <EditFormSettings Visible="False"></EditFormSettings>
                                    </dxe:GridViewDataTextColumn>
                                    <dxe:GridViewDataTextColumn ReadOnly="True" VisibleIndex="4" FieldName="Designation"
                                        Caption="Candt. Designation" >
                                        <EditFormSettings Visible="False"></EditFormSettings>
                                    </dxe:GridViewDataTextColumn>
                                    <dxe:GridViewDataTextColumn Visible="False" ReadOnly="True" VisibleIndex="6" FieldName="rde_ApprovedCTC"
                                        Caption="Approved CTC">
                                        <EditFormSettings Visible="False"></EditFormSettings>
                                    </dxe:GridViewDataTextColumn>
                                    <dxe:GridViewDataTextColumn ReadOnly="True" VisibleIndex="4" FieldName="CreateUserName"
                                        Caption="Created By">
                                        <EditFormSettings Visible="False"></EditFormSettings>
                                    </dxe:GridViewDataTextColumn>
                                    <dxe:GridViewDataTextColumn ReadOnly="True" VisibleIndex="5" FieldName="CreateDate"
                                        Caption="Create Date">
                                        <EditFormSettings Visible="False"></EditFormSettings>
                                    </dxe:GridViewDataTextColumn>
                                    <dxe:GridViewDataTextColumn ReadOnly="True" VisibleIndex="6" FieldName="Status"
                                        Caption="Status">
                                        <EditFormSettings Visible="False"></EditFormSettings>
                                    </dxe:GridViewDataTextColumn>
                                    <dxe:GridViewDataTextColumn ReadOnly="True" VisibleIndex="7" FieldName="rde_NoofDepedent"
                                        Caption="PAN No.">
                                        <EditFormSettings Visible="False"></EditFormSettings>
                                    </dxe:GridViewDataTextColumn>
                                    <dxe:GridViewDataTextColumn ReadOnly="True" VisibleIndex="9" FieldName="GenerateDate"
                                        Caption="Generate Date" Visible="False">
                                        <EditFormSettings Visible="False"></EditFormSettings>
                                    </dxe:GridViewDataTextColumn>
                                    <dxe:GridViewDataTextColumn ReadOnly="True" VisibleIndex="8" FieldName="JoiningDate"
                                        Caption="Joining Date">
                                        <EditFormSettings Visible="False"></EditFormSettings>
                                    </dxe:GridViewDataTextColumn>
                                    <dxe:GridViewDataTextColumn ReadOnly="True" VisibleIndex="9" FieldName="rde_IsConfirmJoin"
                                        Caption="Join Status">
                                        <EditFormSettings Visible="False"></EditFormSettings>
                                    </dxe:GridViewDataTextColumn>
                                    <dxe:GridViewDataTextColumn VisibleIndex="10" Width="60px" Caption="Details">
                                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                        <DataItemTemplate>
                                            <a href="javascript:void(0);" onclick="OnMoreShowButtonClick('<%# Container.KeyValue %>')">
                                                More Info</a>
                                        </DataItemTemplate>
                                        <CellStyle Wrap="False">
                                        </CellStyle>
                                        <HeaderTemplate>
                                            <a href="javascript:void(0);" onclick="javascript:ClickOnAddButton();"><span style="color: #000099;
                                                text-decoration: underline">Add New</span> </a>
                                        </HeaderTemplate>
                                        <EditFormSettings Visible="False"></EditFormSettings>
                                    </dxe:GridViewDataTextColumn>
                                    <dxe:GridViewDataTextColumn VisibleIndex="11" Width="60px" Caption="Details">
                                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                        <DataItemTemplate>
                                            <%if (Session["PageAccess"].ToString().Trim() == "All" || Session["PageAccess"].ToString().Trim() == "Add" || Session["PageAccess"].ToString().Trim() == "DelAdd")
                                   { %>
                                            <a href="javascript:void(0);" onclick="OnMoreInfoClick('<%# Container.KeyValue %>')">
                                                Edit</a> &nbsp;&nbsp; <a href="javascript:void(0);" onclick="DeleteRow('<%# Container.KeyValue %>')">
                                                    Delete</a> <a href="javascript:void(0);" onclick="OnMoreAccessCick('<%# Container.KeyValue %>')">
                                                        Allow</a> &nbsp;&nbsp;
                                            <%} %>
                                        </DataItemTemplate>
                                        <CellStyle Wrap="False">
                                        </CellStyle>
                                        <HeaderTemplate>
                                            <span style="color: #000099; text-decoration: underline">Access </span>
                                        </HeaderTemplate>
                                        <EditFormSettings Visible="False"></EditFormSettings>
                                    </dxe:GridViewDataTextColumn>
                                </Columns>
                                <Settings ShowGroupPanel="True" />
                            </dxe:ASPxGridView>
                      <%--  </div>--%>
                        <%--    <input id="btnRead" type="button" value="Read" class="btnUpdate" onclick="btnRead_click();"
                            style="width: 66px; height: 19px" tabindex="1" />--%>
                    </td>
                </tr>
            </table>
        </div>
</asp:Content>
