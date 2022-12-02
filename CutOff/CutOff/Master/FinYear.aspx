<%@ Page Title="" Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" CodeBehind="FinYear.aspx.cs" Inherits="CutOff.CutOff.Master.FinYear" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script>
        function ValidateAndSave() {

            var d = ctxtStart.GetDate();
            var year = d.getFullYear();
            var month = d.getMonth();
            var day = d.getDate();
            var c = new Date(year + 2, month, day)

            if(date_diff_indays(c, ctxtEnd.GetDate())>-1)
            {
                jAlert('More than 2 year difference is not allowed.');
                return;
            }
            if (ctxtEnd.GetDate() < ctxtStart.GetDate()) {
                jAlert('End date can not be lesser than start date.');
                return;
            }


            cSaveFinYear.PerformCallback('SaveData');
        }
        function SaveFinYearEndCallBack() {
            if (cSaveFinYear.cpResult != null) {
                jAlert(cSaveFinYear.cpResult, 'Alert', function () {
                    location.reload();
                });
            }
        }


        var date_diff_indays = function (date1, date2) {
            dt1 = new Date(date1);
            dt2 = new Date(date2);
            return Math.floor((Date.UTC(dt2.getFullYear(), dt2.getMonth(), dt2.getDate()) - Date.UTC(dt1.getFullYear(), dt1.getMonth(), dt1.getDate())) / (1000 * 60 * 60 * 24));
        }

        function Close() {
            window.location.href = "FinYearList.aspx";
        }

    </script>
    <style>
        .tableInput {
            width:100%;
        }
        .tableInput>tbody>tr>td {
            padding:4px 0;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">



    <div class="panel-heading">
        
        <div class="panel-title">
            <h3><%Response.Write((Request.QueryString["id"] != null ? Request.QueryString["id"].ToString() : "").ToString()); %> Financial Year</h3>
            <div class="crossBtn"><a href="FinYearList.aspx"><i class="fa fa-times"></i></a></div>
        </div>
    </div>
    <div class="form_main">
        <div class="col-md-4">
            <table class="tableInput" class="pad">

                        <tr>
                            <td class="gridcellleft">
                                <span class="Ecoheadtxt">Unique ID:<span style="color: red">*</span></span>
                            </td>
                            <td class="gridcellleft">
                            <asp:TextBox ID="txtFinYearUniqueId" ReadOnly="true" Enabled="false" runat="server" Width="100%" MaxLength="9" TabIndex="1" placeholder="Unique ID" ></asp:TextBox>
                                           
                                <span id="redtxtFinYear" class="pullleftClass fa fa-exclamation-circle iconRed " style="color:red; position:absolute;top: 62px;left: 400px;display:none" title="Mandatory/Invalid"></span>
                            <span id="redtxtFinYear1" class="pullleftClass fa fa-exclamation-circle iconRed " style="color:red; position:absolute;top: 62px;left: 400px;display:none" title="Mandatory/Invalid"></span>
                            </td>
                        </tr>
                        <tr>
                            <td class="gridcellleft">
                                <span class="Ecoheadtxt">Start Date:<span style="color: red">*</span></span>
                            </td>
                            <td class="gridcellleft">
                                <dxe:ASPxDateEdit  ID="txtStart" runat="server" EditFormat="Custom" UseMaskBehavior="True"  ClientInstanceName="ctxtStart"
                                    TabIndex="3" Width="100%" DisplayFormatString="dd-MM-yyyy" EditFormatString="dd-MM-yyyy" AllowNull="false">
                                    <buttonstyle width="13px">
                                    </buttonstyle>
                        
                                </dxe:ASPxDateEdit>
                                <span id="redtxtStart" class="pullleftClass fa fa-exclamation-circle iconRed " style="color:red; position:absolute; top:118px; left: 400px; display:none" title="Mandatory"></span>
                            </td>
                        </tr>
                        <tr>
                            <td class="gridcellleft">
                                <span class="Ecoheadtxt">End Date:<span style="color: red">*</span></span>
                            </td>
                            <td class="gridcellleft">
                                <dxe:ASPxDateEdit ID="txtEnd" runat="server" EditFormat="Custom" UseMaskBehavior="True"  ClientInstanceName="ctxtEnd"
                                    TabIndex="4" Width="100%" DisplayFormatString="dd-MM-yyyy" EditFormatString="dd-MM-yyyy" AllowNull="false">
                                    <buttonstyle width="13px">
                                    </buttonstyle>
                                </dxe:ASPxDateEdit>
                                <span id="redtxtEnd" class="pullleftClass fa fa-exclamation-circle iconRed " style="color:red; position:absolute; top: 155px;left: 400px;display:none" title="Mandatory"></span>
                            </td>
                        </tr>
                        <tr>
                            <td class="gridcellleft">
                                <span class="Ecoheadtxt">Remarks:</span>
                            </td>
                            <td class="gridcellleft">
                                <asp:TextBox TextMode="SingleLine" ID="txtRemarks" MaxLength="100" runat="server" Width="100%" TabIndex="5"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td></td>
                            <td colspan="2" class="gridcellleft">
                                <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="btn btn-primary btnUpdate" UseSubmitBehavior="false" OnClientClick="return ValidateAndSave();" 
                                    TabIndex="6" ValidationGroup="a" />
                  
                                <input type="button" id="btnCancel" value="Cancel" class="btn btn-danger btnUpdate" onclick="Close()" />
                   

                            </td>
                        </tr>
                    </table>
        </div>
        
    </div>
    <dxe:ASPxCallbackPanel runat="server" ID="SaveFinYear" ClientInstanceName="cSaveFinYear" OnCallback="SaveFinYear_Callback">
        <ClientSideEvents EndCallback="SaveFinYearEndCallBack" />
    </dxe:ASPxCallbackPanel>
    
</asp:Content>

