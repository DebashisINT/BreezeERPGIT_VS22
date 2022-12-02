<%@ Page Title="" Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" CodeBehind="ServiceManagement.aspx.cs" Inherits="ERP.OMS.Management.CRM.ServiceManagement.ServiceManagemrnt" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script>
        function CraeteSalesActivity(module) {
            if (module == 'QO') {
                window.location.href = "../../Activities/SalesQuotation.aspx?key=ADD";
            }
            else if (module == 'OR') {
                window.location.href = "../../Activities/SalesOrderAdd.aspx?key=ADD";
            }
            else if (module == 'IN') {
                window.location.href = "../../Activities/SalesInvoice.aspx?key=ADD&&InvType=S";
            }
        }

    </script>

    <style>
        #EmployeeGrid_DXPagerBottom {
            min-width: 100% !important;
        }

        #EmployeeGrid {
            width: 100 % !important;
        }

        .myAssignTarget {
            margin-bottom: 0;
        }

        #cmbPriority {
            border-radius: 3px;
        }

        .myAssignTarget > li {
            list-style-type: none;
            display: inline-block;
            font-size: 11px;
            text-align: center;
        }

            .myAssignTarget > li:not(:last-child) {
                margin-right: 15px;
            }

            .myAssignTarget > li.mainCircle {
                border: 1px solid #a2d3d8;
                border-radius: 8px;
                overflow: hidden;
            }

            .myAssignTarget > li .heading {
                padding: 2px 12px;
                background: #6d82c5;
                color: #fff;
            }

            .myAssignTarget > li .Num {
                font-size: 14px;
            }

            .myAssignTarget > li.mainHeadCenter {
                font-size: 12px;
                transform: translateY(-16px);
            }

        #myAssignTargetpopup {
            padding: 0;
        }

            #myAssignTargetpopup > li .heading {
                padding: 6px 12px;
                background: #7f96dc;
                font-weight: 600;
                color: #fff;
            }

            #myAssignTargetpopup li .Num {
                font-size: 14px;
                padding: 5px;
            }

        .modal-footer .btn {
            margin-top: 0;
            margin-bottom: 0;
        }

        .mleft15 {
            margin-left: 15px;
        }

        #SalesActivityPopup_PW-1, #popupShowHistory_PW-1 {
            border-radius: 15px;
        }

            #SalesActivityPopup_PW-1 .dxpc-header, #popupShowHistory_PW-1 .dxpc-header {
                background: #3ca1e8;
                background-image: none !important;
                padding: 11px 20px;
                border: none;
                border-radius: 15px 15px 0 0;
            }

            #SalesActivityPopup_PW-1 .dxpc-contentWrapper, #popupShowHistory_PW-1 .dxpc-contentWrapper {
                background: #fff;
                border-radius: 0 0 15px 15px;
            }

            #SalesActivityPopup_PW-1 .dxpc-mainDiv, #popupShowHistory_PW-1 .dxpc-mainDiv {
                background-color: transparent !important;
            }

            #SalesActivityPopup_PW-1 .modal-footer, #popupShowHistory_PW-1 .modal-footer {
                text-align: left;
            }

            #SalesActivityPopup_PW-1 .dxpc-shadow, #popupShowHistory_PW-1 .dxpc-shadow {
                box-shadow: none;
            }
    </style>
    <style>
        .table-primary {
            width: 100%;
            border: 1px solid #ccc;
            border-top-color: #3f7ba0;
        }

        .sendMailCheckbox {
            padding-top: 16px;
            padding-left: 15px;
        }


            .sendMailCheckbox > label {
                -webkit-transform: translateY(-3px);
                -moz-transform: translateY(-3px);
                transform: translateY(-3px);
            }

        .table-primary > tbody > tr > td, .table-primary > thead > tr > th {
            padding: 6px 10px;
        }

        .table-primary > thead > tr > th {
            background: #3f7ba0;
            color: #fff;
        }

        .table-primary > tbody > tr > td:not(:last-child) {
            border-right: 1px solid #ccc;
        }

        .table-primary > tbody > tr:not(:last-child) > td {
            /*border-bottom:1px solid #ccc;*/
        }

        ul.ks-cboxtags {
            list-style: none;
            padding: 0;
        }

            ul.ks-cboxtags li {
                display: inline;
            }

                ul.ks-cboxtags li label {
                    display: inline-block;
                    background-color: rgba(255, 255, 255, .9);
                    border: 2px solid rgba(139, 139, 139, .3);
                    color: #adadad;
                    border-radius: 25px;
                    white-space: nowrap;
                    margin: 3px 0px;
                    -webkit-touch-callout: none;
                    -webkit-user-select: none;
                    -moz-user-select: none;
                    -ms-user-select: none;
                    user-select: none;
                    -webkit-tap-highlight-color: transparent;
                    transition: all .2s;
                }

                ul.ks-cboxtags li label {
                    padding: 8px 12px;
                    cursor: pointer;
                }

                    ul.ks-cboxtags li label::before {
                        display: inline-block;
                        font-style: normal;
                        font-variant: normal;
                        text-rendering: auto;
                        -webkit-font-smoothing: antialiased;
                        font-family: "Font Awesome 5 Free";
                        font-weight: 900;
                        font-size: 12px;
                        padding: 2px 6px 2px 2px;
                        content: "\f058";
                        transition: transform .3s ease-in-out;
                    }

                ul.ks-cboxtags li input[type="checkbox"]:checked + label::before {
                    content: "\f057";
                    transform: rotate(-360deg);
                    transition: transform .3s ease-in-out;
                }

                ul.ks-cboxtags li input[type="checkbox"]:checked + label {
                    border: 2px solid #1bdbf8;
                    background-color: #12bbd4;
                    color: #fff;
                    transition: all .2s;
                }

                ul.ks-cboxtags li input[type="checkbox"] {
                    display: absolute;
                }

                ul.ks-cboxtags li input[type="checkbox"] {
                    position: absolute;
                    opacity: 0;
                }

                    ul.ks-cboxtags li input[type="checkbox"]:focus + label {
                        border: 2px solid #e9a1ff;
                    }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="panel-heading">
        <div class="panel-title clearfix">

            <h3>
                <asp:Label ID="lblHeading" runat="server" Text="Service Management"></asp:Label>


            </h3>

            <div id="ApprovalCross" runat="server" class="crossBtn"><a href=""><i class="fa fa-times"></i></a></div>
            <div id="divcross" runat="server" class="crossBtn"><a href="ServiceManagementList.aspx"><i class="fa fa-times"></i></a></div>

        </div>
    </div>
    <div class="form_main">
        <div class="styledBox">
            <div class="row">
                <div class="col-md-3">
                    Entity
            <asp:DropDownList DataSourceID="dsCustomer" DataValueField="ID" DataTextField="Name" runat="server" CssClass="form-control"></asp:DropDownList>
                </div>
                <asp:SqlDataSource runat="server" ID="dsCustomer" SelectCommand="Select TOP 50 cnt_firstname name,cnt_id ID from tbl_master_contact where cnt_contacttype='CL'"></asp:SqlDataSource>
                <div class="col-md-3">
                    Service Name
            <dxe:ASPxTextBox runat="server" ID="txtServiceName" Width="100%"></dxe:ASPxTextBox>
                </div>
                <div class="col-md-3">
                    Service ID
            <dxe:ASPxTextBox runat="server" ID="ASPxMemo1" ClientEnabled="false" Width="100%"></dxe:ASPxTextBox>
                </div>
                <div class="col-md-3">
                    Description
            <dxe:ASPxMemo runat="server" ID="memDesc" Width="100%"></dxe:ASPxMemo>
                </div>


            </div>
            <div class="row">
                <div class="col-md-4">
                    Start Date
            <dxe:ASPxDateEdit ID="dtTodate" runat="server" DisplayFormatString="dd-MM-yyyy" Width="100%"></dxe:ASPxDateEdit>
                </div>
                <div class="col-md-4">
                    End Date
            <dxe:ASPxDateEdit ID="dtEndDate" runat="server" DisplayFormatString="dd-MM-yyyy" Width="100%"></dxe:ASPxDateEdit>
                </div>
                <div class="col-md-4">
                    Renewal Date
            <dxe:ASPxDateEdit runat="server" ID="dtRewDate" Width="100%" DisplayFormatString="dd-MM-yyyy"></dxe:ASPxDateEdit>
                </div>

            </div>

            <div class="row">
                <div class="col-md-4">
                    Attach SLA
            <div id="Div_FileUpload" runat="server">
                <asp:FileUpload ID="file_product" runat="server" AllowMultiple="true" Width="100%" />
                <span style="color: red;">Maximum 5 Mb allowed(pdf,word,excel)</span>

            </div>
                </div>
                <div class="col-md-4">
                    Product/Service Cost
            <dxe:ASPxTextBox runat="server" ID="txtProdCost" Width="100%" DisplayFormatString="0.00"></dxe:ASPxTextBox>
                </div>
                <div class="col-md-4">
                    Service Amount
            <dxe:ASPxTextBox runat="server" ID="txtServiceAmount" Width="100%" DisplayFormatString="0.00"></dxe:ASPxTextBox>
                </div>

            </div>

            <div class="row">
                <div class="col-md-4">
                    Additional Cost
            <dxe:ASPxTextBox runat="server" ID="txtAddCost" Width="100%" DisplayFormatString="0.00"></dxe:ASPxTextBox>
                </div>

                <div class="col-md-4">
                    Service Status
            <select style="width: 100%">
                <option>Active</option>
                <option>Stop</option>

            </select>
                </div>
            </div>
        </div>
        <div class="row">
            <table>
                <tr>
                    <td class="sendMailCheckbox ">
                        <ul class="ks-cboxtags">
                            <li>
                                <asp:CheckBox ID="chkmail" runat="server" Text="Send Mail" />
                            </li>
                        </ul>
                    </td>
                    <td class="sendMailCheckbox ">
                        <ul class="ks-cboxtags">
                            <li>
                                <asp:CheckBox ID="chkSMS" runat="server" Text="Send SMS" />
                            </li>
                        </ul>
                    </td>
                    <td class=" ">

                        <button type="button" class="btn btn-success" onclick="return cSalesActivityPopup.Show();">Create Activity</button>

                    </td>
                    <td class=" ">

                        <button type="button" class="btn btn-success" onclick="CraeteSalesActivity('QO');">Create Quotation</button>

                    </td>
                    <td class=" ">

                        <button type="button" class="btn btn-success" onclick="CraeteSalesActivity('OR');">Create Order</button>

                    </td>
                    <td class=" ">

                        <button type="button" class="btn btn-success" onclick="CraeteSalesActivity('IN');">Create Invoice</button>

                    </td>

                </tr>
            </table>

        </div>

        <br />
        <div class="styledBox">
            <div class="">
                <table class="table-primary">
                    <thead>
                        <tr>
                            <th>Prducts/Services</th>
                            <th>Price</th>
                            <th>Amount</th>
                            <th>Warranty Start</th>
                            <th>Warranty End</th>
                        </tr>
                    </thead>
                    <tbody>
                        <tr>
                            <td>
                                <select style="width: 100%">
                                    <option>Select</option>
                                    <option>Computer</option>
                                </select></td>
                            <td>
                                <input type="text" value="0.00" /></td>
                            <td>
                                <input type="text" value="0.00" /></td>
                            <td>
                                <dxe:ASPxDateEdit runat="server" ID="txtWarrStart" Width="100%" DisplayFormatString="dd-MM-yyyy"></dxe:ASPxDateEdit>
                            </td>
                            <td>
                                <dxe:ASPxDateEdit runat="server" ID="txtWarrEnd" Width="100%" DisplayFormatString="dd-MM-yyyy"></dxe:ASPxDateEdit>
                            </td>

                        </tr>
                        <tr>
                            <td>
                                <select style="width: 100%">
                                    <option>Select</option>
                                    <option>Computer</option>
                                </select></select></td>
                            <td>
                                <input type="text" value="0.00" /></td>
                            <td>
                                <input type="text" value="0.00" /></td>
                            <td>
                                <dxe:ASPxDateEdit runat="server" ID="ASPxDateEdit1" Width="100%" DisplayFormatString="dd-MM-yyyy"></dxe:ASPxDateEdit>
                            </td>
                            <td>
                                <dxe:ASPxDateEdit runat="server" ID="ASPxDateEdit2" Width="100%" DisplayFormatString="dd-MM-yyyy"></dxe:ASPxDateEdit>
                            </td>

                        </tr>
                        <tr>
                            <td>
                                <select style="width: 100%">
                                    <option>Select</option>
                                    <option>Computer</option>
                                </select></select></td>
                            <td>
                                <input type="text" value="0.00" /></td>
                            <td>
                                <input type="text" value="0.00" /></td>
                            <td>
                                <dxe:ASPxDateEdit runat="server" ID="ASPxDateEdit3" Width="100%" DisplayFormatString="dd-MM-yyyy"></dxe:ASPxDateEdit>
                            </td>
                            <td>
                                <dxe:ASPxDateEdit runat="server" ID="ASPxDateEdit4" Width="100%" DisplayFormatString="dd-MM-yyyy"></dxe:ASPxDateEdit>
                            </td>

                        </tr>
                        <tr>
                            <td>
                                <select style="width: 100%">
                                    <option>Select</option>
                                    <option>Computer</option>
                                </select></select></td>
                            <td>
                                <input type="text" value="0.00" /></td>
                            <td>
                                <input type="text" value="0.00" /></td>
                            <td>
                                <dxe:ASPxDateEdit runat="server" ID="ASPxDateEdit5" Width="100%" DisplayFormatString="dd-MM-yyyy"></dxe:ASPxDateEdit>
                            </td>
                            <td>
                                <dxe:ASPxDateEdit runat="server" ID="ASPxDateEdit6" Width="100%" DisplayFormatString="dd-MM-yyyy"></dxe:ASPxDateEdit>
                            </td>

                        </tr>
                        <tr>
                            <td>
                                <select style="width: 100%">
                                    <option>Select</option>
                                    <option>Computer</option>
                                </select></select></td>
                            <td>
                                <input type="text" value="0.00" /></td>
                            <td>
                                <input type="text" value="0.00" /></td>
                            <td>
                                <dxe:ASPxDateEdit runat="server" ID="ASPxDateEdit7" Width="100%" DisplayFormatString="dd-MM-yyyy"></dxe:ASPxDateEdit>
                            </td>
                            <td>
                                <dxe:ASPxDateEdit runat="server" ID="ASPxDateEdit8" Width="100%" DisplayFormatString="dd-MM-yyyy"></dxe:ASPxDateEdit>
                            </td>

                        </tr>
                        <tr>
                            <td>
                                <select style="width: 100%">
                                    <option>Select</option>
                                    <option>Computer</option>
                                </select></select></td>
                            <td>
                                <input type="text" value="0.00" /></td>
                            <td>
                                <input type="text" value="0.00" /></td>
                            <td>
                                <dxe:ASPxDateEdit runat="server" ID="ASPxDateEdit9" Width="100%" DisplayFormatString="dd-MM-yyyy"></dxe:ASPxDateEdit>
                            </td>
                            <td>
                                <dxe:ASPxDateEdit runat="server" ID="ASPxDateEdit10" Width="100%" DisplayFormatString="dd-MM-yyyy"></dxe:ASPxDateEdit>
                            </td>

                        </tr>
                    </tbody>
                </table>
            </div>
        </div>
        <div class="clearfix"></div>
        <div>
            <button type="button" class="btn btn-primary mTop5">Save</button>
        </div>
    </div>


    <dxe:ASPxPopupControl ID="SalesActivityPopup" runat="server" ClientInstanceName="cSalesActivityPopup"
        Width="650px" HeaderText="Service Activity" PopupHorizontalAlign="WindowCenter" Height="500px"
        PopupVerticalAlign="WindowCenter" CssClass="DevPopTypeNew" CloseAction="CloseButton"
        Modal="True" ContentStyle-VerticalAlign="Top" EnableHierarchyRecreation="True">
        <ContentStyle VerticalAlign="Top"></ContentStyle>
        <ContentCollection>
            <dxe:PopupControlContentControl runat="server">
                <dxe:ASPxCallbackPanel runat="server" ID="CallbackPanelLeadActivity" ClientInstanceName="cCallbackPanelLeadActivity">
                    <PanelCollection>
                        <dxe:PanelContent runat="server">

                            <div class="col-md-12">
                                <ul class="myAssignTarget" id="myAssignTargetpopup">

                                    <li class="mainCircle">
                                        <div class="heading">
                                            <dxe:ASPxLabel ID="ASPxLabel1" runat="server" Text="Service Name ">
                                            </dxe:ASPxLabel>
                                        </div>
                                        <div id="lblsource" class="Num">
                                            &nbsp;<dxe:ASPxLabel ID="lblshowLeadName" runat="server" Text=""></dxe:ASPxLabel>
                                        </div>
                                    </li>
                                    <li class="mainCircle">
                                        <div class="heading">
                                            <dxe:ASPxLabel ID="ASPxLabel2" runat="server" Text="Due Date "></dxe:ASPxLabel>
                                        </div>
                                        <div id="lblIndustry" class="Num">
                                            &nbsp;<dxe:ASPxLabel ID="lblshowDueDate" runat="server" Text=""></dxe:ASPxLabel>
                                        </div>
                                    </li>
                                    <li class="mainCircle">
                                        <div class="heading">
                                            <dxe:ASPxLabel ID="ASPxLabel4" runat="server" Text="Priority ">
                                            </dxe:ASPxLabel>

                                        </div>
                                        <div id="lblMiscComments" class="Num">
                                            &nbsp;
                                                    <dxe:ASPxLabel ID="lblshowPriority" runat="server" Text="">
                                                    </dxe:ASPxLabel>
                                        </div>
                                    </li>

                                </ul>

                            </div>
                            <div class="clear"></div>
                            <div class="clearfix">


                                <div class="col-md-6">
                                    <div class="visF">
                                        <div id="ltd_ActivityDate" class="labelt">
                                            <div class="visF">
                                                <dxe:ASPxLabel ID="ASPxLabel5" runat="server" Text="Activity Date">
                                                </dxe:ASPxLabel>
                                                <span style="color: red;">*</span>
                                            </div>
                                        </div>
                                        <div id="td_ActivityDate">
                                            <div class="visF">
                                                <dxe:ASPxDateEdit ID="dtActivityDate" TabIndex="9" runat="server" Date="" Width="100%" ClientInstanceName="cdtActivityDate">
                                                    <TimeSectionProperties>
                                                        <TimeEditProperties EditFormatString="hh:mm tt" />
                                                    </TimeSectionProperties>
                                                </dxe:ASPxDateEdit>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-4">
                                    <label>
                                        <dxe:ASPxLabel ID="ASPxLabel6" runat="server" Text="" CssClass="pdl8"></dxe:ASPxLabel>
                                    </label>
                                    <button type="button" class="btn btn-primary btn-block hide" onclick="Products('POE')">Product</button>
                                </div>
                                <div class="clear"></div>
                                <div class="col-md-6">
                                    <div class="visF">
                                        <div id="td_Activity" class="labelt">
                                            <div class="visF">
                                                <dxe:ASPxLabel ID="lblActivity" runat="server" Text="Activity">
                                                </dxe:ASPxLabel>
                                                <span style="color: red;">*</span>
                                            </div>
                                        </div>
                                        <div id="td_Type">
                                            <div class="visF">
                                                <asp:DropDownList ID="cmbActivity" runat="server" TabIndex="2" Width="100%">
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-6">
                                    <div class="visF">
                                        <div class="labelt">
                                            <label>
                                                <dxe:ASPxLabel ID="blnActivityType" runat="server" Text="Type" CssClass="pdl8"></dxe:ASPxLabel>
                                                <span style="color: red;">*</span>

                                            </label>
                                            <div id="td_Type">
                                                <div class="">
                                                    <asp:DropDownList ID="cmbType" runat="server" TabIndex="3" Width="100%">
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="clear"></div>
                                <div class="col-md-12">
                                    <div class="">
                                        <label>
                                            <dxe:ASPxLabel ID="lblSubject" runat="server" Text="Subject" CssClass="pdl8"></dxe:ASPxLabel>
                                            <span style="color: red;">*</span>

                                        </label>
                                        <div id="td_Type">
                                            <div class="">
                                                <asp:TextBox ID="txtSubject" runat="server" CssClass="form-control" TabIndex="4" MaxLength="500" Width="100%">
                                                </asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="clear"></div>
                                <div class="col-md-12">
                                    <div class="">
                                        <label>
                                            <dxe:ASPxLabel ID="lblDetails" runat="server" Text="Details" CssClass="pdl8"></dxe:ASPxLabel>
                                            <span style="color: red;">*</span>

                                        </label>
                                        <div id="td_Details">
                                            <div class="">
                                                <asp:TextBox ID="txtDetails" runat="server" TextMode="MultiLine" Columns="20" Rows="4" CssClass="form-control" TabIndex="5" MaxLength="500" Width="100%">
                                                </asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="clear"></div>
                                <div class="col-md-12">
                                    <div class="">
                                        <div id="td_lAssignto" class="labelt">
                                            <dxe:ASPxLabel ID="lblSalesActivityAssignTo" runat="server" Text="Assign To">
                                            </dxe:ASPxLabel>
                                            <span style="color: red;">*</span>
                                        </div>
                                        <div id="td_dAssignto">
                                            <asp:DropDownList ID="cmbSalesActivityAssignTo" runat="server" TabIndex="6" Width="100%">
                                            </asp:DropDownList>
                                        </div>
                                    </div>
                                </div>
                                <div class="clear"></div>
                                <div class="col-md-6">
                                    <div class="">
                                        <div id="td_lDuration" class="labelt">
                                            <dxe:ASPxLabel ID="lblDuration" runat="server" Text="Duration">
                                            </dxe:ASPxLabel>
                                            <span style="color: red;">*</span>
                                        </div>
                                        <div id="td_dDuration">
                                            <asp:DropDownList ID="cmbDuration" runat="server" TabIndex="7" Width="100%">
                                            </asp:DropDownList>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-6">
                                    <div class="">
                                        <div id="td_lPriority" class="labelt">
                                            <dxe:ASPxLabel ID="lblPriority" runat="server" Text="Priority">
                                            </dxe:ASPxLabel>
                                            <span style="color: red;">*</span>
                                        </div>
                                        <div id="td_dPriority">
                                            <asp:DropDownList ID="cmbPriority" runat="server" TabIndex="8" Width="100%">
                                            </asp:DropDownList>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-6">
                                    <div class="">
                                        <div id="td_lDue" class="labelt">
                                            <dxe:ASPxLabel ID="lblDue" runat="server" Text="Due" CssClass="pdl8"></dxe:ASPxLabel>
                                        </div>
                                        <div>
                                            <%-- <dxe:ASPxDateEdit ID="DtxtDue" runat="server" EditFormatString="dd-MM-yyyy hh:mm:ss"  ClientInstanceName="cDtxtDue" EditFormat="Custom" UseMaskBehavior="True" TabIndex="20" Width="100%">
                                                    <ButtonStyle Width="13px">
                                                    </ButtonStyle>
                                                    <TimeSectionProperties>
                                                        <TimeEditProperties EditFormatString="hh:mm tt" />
                                                    </TimeSectionProperties>
                                                </dxe:ASPxDateEdit>--%>

                                            <dxe:ASPxDateEdit ID="DtxtDue" TabIndex="9" runat="server" Date="" Width="100%" ClientInstanceName="cDtxtDue">
                                                <TimeSectionProperties>
                                                    <TimeEditProperties EditFormatString="hh:mm tt" />
                                                </TimeSectionProperties>
                                                <%-- <ClientSideEvents DateChanged="Enddate" />--%>
                                            </dxe:ASPxDateEdit>
                                        </div>
                                    </div>
                                </div>
                            </div>


                            <div class="clearfix">








                                <asp:HiddenField ID="hdnEntityID" runat="server" />
                            </div>

                            <div class="modal-footer">
                                <button type="button" class="btnOkformultiselection btn btn-success" onclick="SaveSalesActivity()">Save</button>
                                <button type="button" class="btnOkformultiselection btn btn-danger" onclick="return cSalesActivityPopup.Hide();">Cancel</button>
                                <button type="button" class="btnOkformultiselection btn btn-info hide" onclick="ShowHistory()">Show History</button>
                            </div>

                        </dxe:PanelContent>
                    </PanelCollection>

                </dxe:ASPxCallbackPanel>
            </dxe:PopupControlContentControl>
        </ContentCollection>

    </dxe:ASPxPopupControl>

</asp:Content>
