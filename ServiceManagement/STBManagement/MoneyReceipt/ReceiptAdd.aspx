<%@ Page Title="" Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" CodeBehind="ReceiptAdd.aspx.cs" Inherits="ServiceManagement.STBManagement.MoneyReceipt.ReceiptAdd" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../../ServiceManagement/Transaction/CSS/ReceiptChallan.css" rel="stylesheet" />
    <style>
        .btn:focus {
            outline:none
        }
        .mtop8 {
            margin-top: 8px;
        }
        .mTop4 {
            margin-top: 4px;
        }
        .ptTbl > tbody > tr > td {
            padding-right: 10px;
        }

        .headerPy {
            background: #66b1c7;
            /* display: inline-block; */
            padding: 4px 10px;
            /* transform: translate(-4px); */
            border-radius: 5px 5px 0 0;
            /* border: 1px solid #858eb7; */
            font-weight: 500;
            color: #f1f1f1;
            margin-top: 2px;
        }
        .trY {
            -webkit-transform: translateY(-3px);
            transform: translateY(-3px);
        }
    </style>
    <style>
         /* for pop */
        .popupWraper {
            position: fixed;
            top: 0;
            left: 0;
            height: 100vh;
            width: 100%;
            background: rgba(0,0,0,0.85);
            z-index: 10;
            display: flex;
            justify-content: center;
            align-items: center;
        }
        .popBox {
                width: 670px;
                background: #fff;
                padding: 35px;
                text-align: center;
                min-height: 350px;
                display: flex;
                align-items: center;
                flex-direction: column;
                justify-content: center;
                background:#fff url("/assests/images/popupBack.png") no-repeat top left;
                box-shadow: 0px 14px 14px rgba(0,0,0,0.56);
        
            }
        .popBox  h1, .popBox p{
            font-family: 'Poppins', sans-serif !important;
            margin-bottom:20px !important;
        }
        .popBox p {
            font-size: 15px;
        }
        .btn-sign {
            background: #3680fb;
            color: #fff;
            padding: 10px 25px;
            box-shadow: 0px 5px 5px rgba(0,0,0,0.22);
        }

        .btn-sign:hover {
            background: #2e71e1;
            color: #fff;
           }
    </style>
    <script src="../JS/MoneyReceiptJs.js?v=2.0"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
     <div class="popupWraper hide" id="divPopHead" runat="server">
        <div class="popBox">
            <img src="/assests/images/warningAlert.png" class="mBot10" style="width:70px;" />
            <h1 id="h1heading" class="red">Your Access is Denied</h1>
            <p id="pParagraph" class="red">
               You can access this section starting from <span id="spnbegin"></span> upto <span id="spnEnd"></span>
            </p>
            <button type="button" class="btn btn-sign" onclick="WorkingRosterClick()">OK</button>
        </div>
    </div>

    <div class="panel-heading clearfix">
        <div class="panel-title pull-left">
            <h3>
                <asp:Label runat="server" ID="HeaderName"></asp:Label>
            </h3>
        </div>

        <div id="divcross" class="crossBtn"><a href="index.aspx"><i class="fa fa-times"></i></a></div>
    </div>
    <div class="form_main clearfix">
        <div class="pmsForm slick boxModel clearfix">
            
            <div class="row" style="margin-top: 5px">

                <div class="col-md-2" id="divNumberingScheme" runat="server">
                    <label>Numbering Scheme <span class="red">*</span></label>
                    <div id="divScheme" class="dropDev">
                        <dxe:ASPxComboBox ID="CmbScheme" runat="server" ClientInstanceName="cCmbScheme"
                            Width="100%">
                            <clientsideevents selectedindexchanged="CmbScheme_ValueChange" />
                        </dxe:ASPxComboBox>
                    </div>
                </div>
                <div class="col-md-3 noLabelmargin">
                    <label>Document Number  <span class="red">*</span></label>
                    <div>
                        <asp:TextBox ID="txtDocumentNumber" runat="server" Width="95%" MaxLength="30" CssClass="form-control">                             
                        </asp:TextBox>
                        <span id="MandatoryBillNo" class="voucherno  pullleftClass fa fa-exclamation-circle iconRed " style="color: red; position: absolute; display: none" title="Mandatory"></span>
                    </div>
                </div>
                <div class="col-md-2 noLabelmargin">
                    <label>Location  <span class="red">*</span></label>
                    <div class="dropDev">
                        <dxe:ASPxComboBox ID="ddlBranch" runat="server" ClientInstanceName="cddlBranch" Width="100%">
                        </dxe:ASPxComboBox>
                    </div>
                </div>
                <div class="col-md-2">
                    <label>Date  <span class="red">*</span></label>
                    <div class="dropDev">
                        <dxe:ASPxDateEdit ID="FormDate" runat="server" EditFormat="Custom" EditFormatString="dd-MM-yyyy" ClientInstanceName="cFormDate" Width="100%" DisplayFormatString="dd-MM-yyyy" UseMaskBehavior="True">
                            <buttonstyle width="13px"></buttonstyle>
                        </dxe:ASPxDateEdit>
                    </div>
                </div>
                <div class="col-md-3 hide" id="DivCancel_Status">
                    <div style="background: #fbcccc;
                    padding: 5px;
                    border-radius: 4px;
                    font-weight: 500;
                    text-align: center;
                    margin-top: 21px;
                    font-size: 13px;">
                        <asp:Label runat="server" ID="lblCancelStatus" Style="color: red"></asp:Label>
                    </div>
                </div>
                <div class="clear"></div>
                <hr class="hrBoder" />

            </div>

            <div class="row">
                <div class="col-md-3">
                    <label>Entity Code <span class="red">*</span></label>
                    <div>
                        <input type="text" class="form-control" id="txtEntityCode" maxlength="300" runat="server" onblur="Devicenumber_change()" />

                    </div>
                </div>
                <div class="col-md-3">
                    <label>Network Name <span class="red">*</span></label>
                    <div>
                        <input type="text" class="form-control" id="txtNetworkName" maxlength="300" runat="server" disabled />
                    </div>
                </div>
                <div class="col-md-2 mtop8">
                    <label>Contact Person <span class="red">*</span></label>
                    <div>
                        <input type="text" class="form-control" maxlength="200" id="txtContactPerson" runat="server" disabled />
                    </div>
                </div>
                <div class="col-md-2 mtop8">
                    <label>Contact Number <span class="red">*</span></label>
                    <div>
                        <input type="text" class="form-control" id="txtContactNo" maxlength="15" runat="server" disabled />
                    </div>
                </div>
                <div class="col-md-2 mtop8">
                    <label>Type <span class="red">*</span></label>
                    <div class="dropDev">
                        <dxe:ASPxComboBox ID="DiviceTyp" runat="server" ClientInstanceName="cDiviceTyp" Width="100%">
                        </dxe:ASPxComboBox>
                    </div>
                </div>
            </div>
        </div>
        <div class="headerPy">Payment Details</div>
        <div class="pmsForm slick boxModel clearfix" style="border-radius: 0 0 5px 5px">

            <table class="ptTbl">
                <thead>
                    <tr>
                        <th>Mode<span class="red">*</span></th>
                        <th>Amount <span class="red">*</span></th>
                        <th>Ref.No. </th>
                        <th>Cheque No. </th>
                        <th>Cheque Date</th>
                        <th>Bank Name</th>
                        <th>Branch</th>
                        <th></th>
                    </tr>
                </thead>
                <tbody>
                    <tr>
                        <td>
                            <div style=" margin-top: 2px;">
                                <asp:DropDownList ID="ddlModeType" runat="server" Width="100%" CssClass="js-example-basic-single" onchange="ddlModeType_SelectedIndexChanged()">

                                    <asp:ListItem Value="0">Select</asp:ListItem>
                                    <asp:ListItem Value="Cash">Cash</asp:ListItem>
                                    <asp:ListItem Value="Cheque">Cheque</asp:ListItem>
                                    <asp:ListItem Value="NEFT">NEFT</asp:ListItem>
                                    <asp:ListItem Value="RTGS">RTGS</asp:ListItem>
                                    <asp:ListItem Value="OtherOnline">Other Online</asp:ListItem>
                                </asp:DropDownList>
                              <%--  <span id="MandatoryModeType" class="voucherno  pullleftClass fa fa-exclamation-circle iconRed " style="color: red; position: absolute; display: none; top: 281px; right: 1017px" title="Mandatory"></span>--%>
                            </div>
                        </td>
                        <td>
                            <div class="dropDev " >
                                <dxe:ASPxTextBox ID="txtAmount" ClientInstanceName="ctxtAmount" runat="server" Width="100%" class="form-control">
                                    <masksettings mask="<0..999999999>.<0..99>" allowmousewheel="false" />
                                </dxe:ASPxTextBox>
                            </div>


                        </td>
                        <td>
                            <input type="text" class="form-control mTop4" id="txtRefNo" maxlength="15" runat="server"  /></td>
                        <td>
                            <input type="text" class="form-control mTop4" id="txtChequeNo" maxlength="6" runat="server" /></td>
                        <td>
                            <div class="dropDev ">
                                <dxe:ASPxDateEdit ID="dtChequeDate" runat="server" EditFormat="Custom" EditFormatString="dd-MM-yyyy" ClientInstanceName="cdtChequeDate" Width="100%" DisplayFormatString="dd-MM-yyyy" UseMaskBehavior="True">
                                    <buttonstyle width="13px"></buttonstyle>
                                </dxe:ASPxDateEdit>
                            </div>
                        </td>

                        <td>
                            <div class="dropDev ">
                                <dxe:ASPxComboBox ID="ddlBank" runat="server" ClientInstanceName="cddlBank" Width="100%">
                                </dxe:ASPxComboBox>
                            </div>
                        </td>
                        <td>
                            <input type="text" class="form-control mTop4" id="txtBranch" maxlength="15" runat="server" /></td>

                    </tr>

                </tbody>

            </table>
            <div class="row ">
                <div class="col-md-6">
                    <label>Remarks</label>
                    <div>
                        <textarea class="form-control textareaBig" id="txtRemarks" maxlength="500"></textarea>
                    </div>
                </div>
                <div class="col-md-6" style="margin-top: 23px;">
                    <button type="button" id="btnAdd" class="btn btn-success" onclick="AddDevice()"><i class="fa fa-plus-circle mr-2"></i>Add</button>
                </div>
            </div>
        </div>
        <div style="margin-top: 2px;">
            <div class="row">
                <div class="col-md-12" style="padding-left: 15px;">
                    <dxe:ASPxGridView ID="GrdDevice" runat="server" KeyFieldName="HIddenID" AutoGenerateColumns="False" SettingsBehavior-AllowFocusedRow="true"
                        SettingsPager-Mode="ShowAllRecords"  OnDataBinding="grid_DataBinding" OnCustomCallback="GrdDevice_CustomCallback"
                        Width="100%" ClientInstanceName="cGrdDevice" >
                       <%--  Settings-HorizontalScrollBarMode="auto"--%>
                        <settingssearchpanel visible="false" delay="5000" />
                        <columns>
                    <dxe:GridViewDataTextColumn Caption="ID" FieldName="HIddenID"
                        VisibleIndex="0" FixedStyle="Left" Width="0" Visible="false">
                        <CellStyle CssClass="gridcellleft" Wrap="true">
                        </CellStyle>
                        <Settings AllowAutoFilterTextInputTimer="False" />
                        <Settings AutoFilterCondition="Contains" />
                    </dxe:GridViewDataTextColumn>

                     <dxe:GridViewDataTextColumn Caption="Mode" FieldName="Payment_Mode"
                        VisibleIndex="1" FixedStyle="Left" Width="200px" >
                        <CellStyle CssClass="gridcellleft" Wrap="true">
                        </CellStyle>
                        <Settings AllowAutoFilterTextInputTimer="False" />
                        <Settings AutoFilterCondition="Contains" />
                    </dxe:GridViewDataTextColumn>

                     <dxe:GridViewDataTextColumn Caption="Amount" FieldName="Payment_Amount"
                        VisibleIndex="2" FixedStyle="Left" Width="200px" >
                        <CellStyle CssClass="gridcellleft" Wrap="true">
                        </CellStyle>
                        <Settings AllowAutoFilterTextInputTimer="False" />
                        <Settings AutoFilterCondition="Contains" />
                    </dxe:GridViewDataTextColumn>

                    <dxe:GridViewDataTextColumn Caption="Ref. No." FieldName="Ref_No"
                        VisibleIndex="3" Width="150px">
                        <CellStyle CssClass="gridcellleft" Wrap="true">
                        </CellStyle>
                        <Settings AllowAutoFilterTextInputTimer="False" />
                        <Settings AutoFilterCondition="Contains" />
                    </dxe:GridViewDataTextColumn>                   

                    <dxe:GridViewDataTextColumn Caption="Cheque No" FieldName="Cheque_No"
                        VisibleIndex="4" Width="150px">
                        <CellStyle CssClass="gridcellleft" Wrap="true">
                        </CellStyle>
                        <Settings AllowAutoFilterTextInputTimer="False" />
                        <Settings AutoFilterCondition="Contains" />
                    </dxe:GridViewDataTextColumn>

                    <dxe:GridViewDataTextColumn Caption="Cheque Date" FieldName="Cheque_date" 
                        VisibleIndex="5" Width="150px">
                         <PropertiesTextEdit DisplayFormatString="dd-MM-yyyy"></PropertiesTextEdit>
                       
                    </dxe:GridViewDataTextColumn>

                    <dxe:GridViewDataTextColumn Caption="Bank Name" FieldName="PaymentDetails_BankName" Width="150px" VisibleIndex="6">
                       
                    </dxe:GridViewDataTextColumn>

                    <dxe:GridViewDataTextColumn Caption="Branch Name" FieldName="PaymentDetails_BranchName"
                        VisibleIndex="7" Width="200px">
                        <CellStyle CssClass="gridcellleft" Wrap="true">
                        </CellStyle>
                        <Settings AllowAutoFilterTextInputTimer="False" />
                        <Settings AutoFilterCondition="Contains" />
                    </dxe:GridViewDataTextColumn>

                    <dxe:GridViewDataTextColumn Caption="Remarks" FieldName="Remarks"
                        VisibleIndex="8" Width="200px">
                        <CellStyle CssClass="gridcellleft" Wrap="true">
                        </CellStyle>
                        <Settings AllowAutoFilterTextInputTimer="False" />
                        <Settings AutoFilterCondition="Contains" />
                    </dxe:GridViewDataTextColumn>

                     <dxe:GridViewDataTextColumn Caption="Bank_ID" FieldName="Bank_ID"
                        VisibleIndex="12" FixedStyle="Left" Width="0" Visible="false">
                        <CellStyle CssClass="gridcellleft" Wrap="true">
                        </CellStyle>                        
                    </dxe:GridViewDataTextColumn>                    

                    <dxe:GridViewDataTextColumn HeaderStyle-HorizontalAlign="Center" CellStyle-HorizontalAlign="center" VisibleIndex="9" Width="150px" >
                        <DataItemTemplate>

                            <a href="javascript:void(0);" onclick="ClickOnEdit('<%# Container.KeyValue %>')" id="a_editInvoice" class="pad" title="Edit">
                                <img src="../../../assests/images/info.png" /></a>

                            <a href="javascript:void(0);" onclick="OnClickDelete('<%# Container.KeyValue %>')" class="pad" title="Delete" id="a_delete">
                                <img src="../../../assests/images/Delete.png" /></a>
                        </DataItemTemplate>
                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                        <CellStyle HorizontalAlign="Center"></CellStyle>
                        <HeaderTemplate><span>Actions</span></HeaderTemplate>
                        <EditFormSettings Visible="False"></EditFormSettings>
                        <Settings AllowAutoFilterTextInputTimer="False"  />
                    </dxe:GridViewDataTextColumn>
                </columns>
                        <settingscontextmenu enabled="true"></settingscontextmenu>
                        <clientsideevents endcallback="grid_EndCallBack" />
                        <settingspager numericbuttoncount="10" pagesize="10" showseparators="True" mode="ShowPager">
                    <PageSizeItemSettings Visible="true" ShowAllItem="false" Items="10,50,100,150,200" />
                    <FirstPageButton Visible="True">
                    </FirstPageButton>
                    <LastPageButton Visible="True">
                    </LastPageButton>
                </settingspager>
                        <settings showgrouppanel="False" showstatusbar="Hidden" showhorizontalscrollbar="False" showfilterrow="true" showfilterrowmenu="true" />
                        <settingsloadingpanel text="Please Wait..." />
                    </dxe:ASPxGridView>
                </div>

            </div>
        </div>
         <div class="headerPy hide" id="DivCancelDetails">Cancellation Details</div>
        <div id="DivCancelData" class="pmsForm slick boxModel clearfix" style="border-radius: 0 0 5px 5px">


            <div class="row ">
                <div class="col-md-6">
                    <label class="red">Reason For Cancellation Request:</label>
                    <div>
                        <input type="text" class="form-control" id="txtReasonCancel" maxlength="500" runat="server" disabled />
                    </div>
                </div>
            
                <div class="col-md-3 trY" >
                    <label class="red">Cancellation Request Made By:</label>
                    <div>
                        <input type="text" class="form-control" id="txtReasonCancelMadeBy" maxlength="500" runat="server" disabled />
                    </div>
                </div>
                <div class="col-md-3 trY">
                    <label class="red">Cancellation Request Made On:</label>
                    <div>
                        <input type="text" class="form-control" id="txtReasonCancelMadeOn" maxlength="500" runat="server" disabled />
                    </div>
                </div>
            </div>
        </div>
        <dxe:ASPxLoadingPanel ID="LoadingPanel" runat="server" ClientInstanceName="LoadingPanel" ContainerElementID="divsave"
            Modal="True">
        </dxe:ASPxLoadingPanel>
        <div class="row">
        </div>

        <div class="row">
            <div class="col-md-12 mtop8">
                <div class="checkbox">
                    <label>
                        <input type="checkbox" id="chkTnC" value="" />Yes, I want to save This Payment Details</label>
                </div>
            </div>
            <div class="col-md-12 mtop8">
                <button type="button" onclick="SaveButtonClick('new');" id="btnSaveNew" class="btn btn-success">Save & New</button>
                <button type="button" onclick="SaveButtonClick('Exit');" id="btnSaveExit" class="btn btn-primary">Save & Exit</button>
                <button type="button" class="btn btn-danger" onclick="cancel();">Cancel</button>
            </div>
        </div>
        <div class="row" >
            <div class="col-md-12 hide" id="divPrintCount">
                <h3>
                    <div id="" class="" style="font-size: large;">
                        <label class="red">Print Count: </label>
                        <label class="red" id="lblPrintCount" runat="server">0 </label>
                    </div>
                </h3>
            </div>
        </div>
        <asp:HiddenField ID="hdAddEdit" runat="server" />
        <asp:HiddenField runat="server" ID="hdnMoneyReceiptID" />
        <asp:HiddenField runat="server" ID="hdnOnlinePrint" />
        <asp:HiddenField runat="server" ID="hdnGuid" />
        <asp:HiddenField runat="server" ID="hdnIsCancel" />

         <asp:HiddenField runat="server" ID="hdnIsEntityInformationEditableInWRMR" />
    </div>
</asp:Content>
