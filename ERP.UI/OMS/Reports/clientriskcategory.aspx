<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/OMS/MasterPage/ERP.Master" Inherits="ERP.OMS.Reports.Reports_clientriskcategory" EnableEventValidation="false" CodeBehind="clientriskcategory.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script language="javascript" type="text/javascript">
        function selecttion() {
            var combo = document.getElementById('cmbExport');
            combo.value = '0';
        }
        function SignOff() {
            window.parent.SignOff();
        }
        function height() {

            if (document.body.scrollHeight >= 600)
                window.frameElement.height = document.body.scrollHeight;
            else
                window.frameElement.height = '700px';
            window.frameElement.Width = document.body.scrollWidth;
        }
        function ValidatePage() {

            selecttion();
            //      if(document.getElementById("txtClientID").value=='')
            //      {
            //         alert('Please Type Client ID!..');
            //         return false;
            //      }

        }
        function ShowHideFilter(obj) {
            grid.PerformCallback(obj);
        }


        //    function SearchOpt(obj)
        //    {
        //   
        //         var cmbt=document.getElementById('cmbDuplicate');
        //         if(cmbt.value == 'None')
        //         {
        //             document.getElementById("txtClientID").style.display="inline";
        //            document.getElementById("TrFilter").style.display="none"
        //         }
        //         else if (cmbt.value == 'PANEXEMPT') 
        //         {
        //             document.getElementById("txtClientID").style.display="none";
        //             document.getElementById("TrFilter").style.display="inline";
        //             
        //         
        //         }
        //         else
        //         {
        //         document.getElementById("txtClientID").style.display="none";
        //             document.getElementById("TrFilter").style.display="none";
        //         }
        //     
        //    }
        //    function ShowHideFilter(obj)
        //    {
        //         if(document.getElementById('TxtSeg').value=='N')
        //           {
        //             document.getElementById('TxtTCODE').style.display="none";
        //           }
        //           else
        //           {
        //            document.getElementById('TxtTCODE').style.display="inline";
        //           }
        //        InitialTextVal();
        //        if(obj=="s")
        //            document.getElementById('TrFilter').style.display="inline";
        //        else
        //        {
        //            document.getElementById('TrFilter').style.display="none";
        //            grid.PerformCallback(obj);
        //        }
        //    }
        function btnSearch_click() {
            document.getElementById('TrFilter').style.display = "none";
            grid.PerformCallback('s');
        }
        //   function InitialTextVal()
        //   {
        //   

        //        document.getElementById('txtName').value = "ADD1";
        //        document.getElementById('txtBranchName').value = "ADD2";
        //        document.getElementById('txtCode').value = "ADD3";
        //        document.getElementById('txtRelationManager').value = "Landmark";
        //        document.getElementById('txtReferedBy').value = "Country";
        //        document.getElementById('txtPhNumber').value = "State";
        //        document.getElementById('txtContactStatus').value = "City";
        ////        document.getElementById('txtStatus').value = "Status";
        //        
        //        document.getElementById('TxtTCODE').value = "Area";
        //        //document.getElementById('txtPAN').value = "PAN No.";
        //   }  
        //    function ClearTextboxes()
        //        {
        //            document.getElementById('txtName').value = '';
        //           
        //            document.getElementById('txtBranchName').value = ''; 
        //            document.getElementById('txtCode').value= '';
        //            document.getElementById('TxtTCODE').value= '';
        //            document.getElementById('txtPAN').value= '';
        //            document.getElementById('txtRelationManager').value= '';
        //            document.getElementById('txtReferedBy').value= '';
        //            document.getElementById('txtPhNumber').value= '';           
        //}

    </script>
    <script language="javascript" type="text/javascript">
        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_initializeRequest(InitializeRequest);
        prm.add_endRequest(EndRequest);
        var postBackElement;
        function InitializeRequest(sender, args) {
            if (prm.get_isInAsyncPostBack())

                args.set_cancel(true);
            postBackElement = args.get_postBackElement();
            $get('UpdateProgress1').style.display = 'block';

        }
        function EndRequest(sender, args) {
            $get('UpdateProgress1').style.display = 'none';
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div>
        <table class="TableMain100">
            <tr>
                <td class="EHEADER" style="text-align: center">
                    <strong><span style="color: #000099">Client Risk Category</span></strong></td>
            </tr>
            <tr>
                <td>
                    <table cellspacing="0" cellpadding="0" style="background-color: #B7CEEC; border: solid 1px #ffffff"
                        border="0" width="100%">
                        <tr>
                            <td class="gridcellleft" width="150px">
                                <span style="font-weight: bold">Risk Category </span>
                            </td>
                            <td class="gridcellleft" colspan="2">
                                <asp:DropDownList ID="cmbDuplicate" runat="server" OnSelectedIndexChanged="cmbDuplicate_SelectedIndexChanged">

                                    <asp:ListItem Text="Low" Value="Low"></asp:ListItem>
                                    <asp:ListItem Text="Medium" Value="Midium"></asp:ListItem>
                                    <asp:ListItem Text="High" Value="High"></asp:ListItem>
                                </asp:DropDownList>



                            </td>

                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td>
                    <table>
                        <td class="gridcellright">
                            <asp:Button ID="btnSave" runat="server" TabIndex="3" Text="Show" CssClass="btnUpdate"
                                OnClick="btnSave_Click" />

                            <asp:Button ID="btnExport" runat="server" TabIndex="4" Text="Export to Excel" CssClass="btnUpdate"
                                OnClick="btnExport_Click" />
                        </td>
                    </table>
                </td>
            </tr>
            <tr>
                <td>
                    <table>
                        <tr>
                            <td id="Td1" align="left">
                                <a href="javascript:ShowHideFilter('s');"><span style="color: #000099; text-decoration: underline">Show Filter</span></a> || <a href="javascript:ShowHideFilter('All');"><span style="color: #000099; text-decoration: underline">All Records</span></a>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpdatePanel1">
                        <ProgressTemplate>
                            <div id='Div1' style='position: absolute; font-family: arial; font-size: 30; left: 50%; top: 8; background-color: white; layer-background-color: white; height: 80; width: 150;'>
                                <table width='100' height='35' border='1' cellpadding='0' cellspacing='0' bordercolor='#C0D6E4'>
                                    <tr>
                                        <td>
                                            <table>
                                                <tr>
                                                    <td height='25' align='center' bgcolor='#FFFFFF'>
                                                        <img src='/assests/images/progress.gif' width='18' height='18'></td>
                                                    <td height='10' width='100%' align='center' bgcolor='#FFFFFF'>
                                                        <font size='2' face='Tahoma'><strong align='center'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Loading..</strong></font></td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </ProgressTemplate>
                    </asp:UpdateProgress>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <table class="TableMain100">
                                <tr>
                                    <td>
                                        <dxe:aspxgridview id="gridContract" clientinstancename="grid" width="100%"
                                            runat="server" autogeneratecolumns="False" oncustomcallback="gridContract_CustomCallback">
                                                <SettingsBehavior AllowFocusedRow="True" ConfirmDelete="True" />
                                                <Styles>
                                                    <Header ImageSpacing="5px" SortingImageSpacing="5px">
                                                    </Header>
                                                    <LoadingPanel ImageSpacing="10px">
                                                    </LoadingPanel>
                                                    <FocusedRow BackColor="#FEC6AB">
                                                    </FocusedRow>
                                                </Styles>
                                                <Columns>
                                                <dxe:GridViewDataTextColumn VisibleIndex="1"  Visible="false" FieldName="CustomerID" Caption="CustomerID">
                                                        <CellStyle Wrap="True" CssClass="gridcellleft">
                                                        </CellStyle>
                                                        <EditFormSettings Visible="False"></EditFormSettings>
                                                    </dxe:GridViewDataTextColumn>
                                                    
                                                    <dxe:GridViewDataTextColumn VisibleIndex="2" FieldName="ClientName" Caption="Name">
                                                        <CellStyle Wrap="True" CssClass="gridcellleft">
                                                        </CellStyle>
                                                        <EditFormSettings Visible="False"></EditFormSettings>
                                                    </dxe:GridViewDataTextColumn>
                                                     <dxe:GridViewDataTextColumn VisibleIndex="3" FieldName="Ucc" Caption="Ucc">
                                                        <CellStyle Wrap="True" CssClass="gridcellleft">
                                                        </CellStyle>
                                                        <EditFormSettings Visible="False"></EditFormSettings>
                                                    </dxe:GridViewDataTextColumn>
                                                    <dxe:GridViewDataTextColumn VisibleIndex="4" FieldName="branch" Caption="Branch">
                                                        <CellStyle Wrap="True" CssClass="gridcellleft">
                                                        </CellStyle>
                                                        <EditFormSettings Visible="False"></EditFormSettings>
                                                    </dxe:GridViewDataTextColumn>
                                                    <dxe:GridViewDataTextColumn VisibleIndex="5" FieldName="PhoneNo" Caption="Phone">
                                                        <CellStyle Wrap="True" CssClass="gridcellleft">
                                                        </CellStyle>
                                                        <EditFormSettings Visible="False"></EditFormSettings>
                                                    </dxe:GridViewDataTextColumn>
                                                    <dxe:GridViewDataTextColumn VisibleIndex="6" FieldName="Email" Caption="Email">
                                                        <CellStyle Wrap="True" CssClass="gridcellleft">
                                                        </CellStyle>
                                                        <EditFormSettings Visible="False"></EditFormSettings>
                                                    </dxe:GridViewDataTextColumn>
                                                   
                                                </Columns>
                                                <StylesEditors>
                                                    <ProgressBar Height="25px">
                                                    </ProgressBar>
                                                </StylesEditors>
                                                <SettingsBehavior AllowFocusedRow="True" AllowSort="False" AllowMultiSelection="True" />
                                                <SettingsPager AlwaysShowPager="True" NumericButtonCount="20" ShowSeparators="True"
                                                    PageSize="15">
                                                    <FirstPageButton Visible="True">
                                                    </FirstPageButton>
                                                    <LastPageButton Visible="True">
                                                    </LastPageButton>
                                                </SettingsPager>
                                                <SettingsText Title="Template" />
                                                <Settings ShowGroupedColumns="True" ShowGroupPanel="True" />
                                            </dxe:aspxgridview>
                                        <dxe:aspxgridviewexporter id="exporter" runat="server">
                                            </dxe:aspxgridviewexporter>
                                    </td>
                                </tr>
                            </table>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                        </Triggers>
                    </asp:UpdatePanel>
                </td>
            </tr>

        </table>
    </div>
</asp:Content>
