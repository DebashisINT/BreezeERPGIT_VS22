<%--================================================== Revision History =============================================
Rev Number         DATE              VERSION          DEVELOPER           CHANGES
1.0                28-03-2023        2.0.36           Pallab              25733 : Master pages design modification
====================================================== Revision History =============================================--%>

<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/OMS/MasterPage/ERP.Master"
    Inherits="ERP.OMS.Management.Master.management_master_ProfBodies_general" CodeBehind="ProfBodies_general.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script language="javascript" type="text/javascript">
        function disp_prompt(name) {
            if (name == "tab0") {
                //alert(name);
                //document.location.href="ProfBodies_general.aspx"; 
            }
            if (name == "tab1") {
                //alert(name);
                document.location.href = "ProfBodies_correspondence.aspx";
            }
        }
    </script>
    <style>
        #RequiredFieldValidator2 {
            position: absolute;
            right: -20px;
            top: 6px;
        }

        #RequiredFieldValidator1 {
            position: absolute;
            right: -20px;
            top: 6px;
        }

        .dxtc-content {
            overflow: visible !important;
        }
    </style>
    <script type="text/javascript">
        function fn_ctxtPro_Name_TextChanged() {
            var ShortName = document.getElementById("txtShortName").value;

            $.ajax({
                type: "POST",
                url: "ProfBodies_general.aspx/CheckUniqueName",
                data: JSON.stringify({ ShortName: ShortName }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (msg) {
                    var data = msg.d;

                    if (data == true) {
                        jAlert("Please enter unique name");
                        document.getElementById("txtShortName").value='';
                        document.getElementById("txtShortName").focus();
                        return false;
                    }
                }
            });
        }
   </script>

    <style>
        /*Rev 1.0*/

        select
        {
            height: 30px !important;
            border-radius: 4px;
            -webkit-appearance: none;
            position: relative;
            z-index: 1;
            background-color: transparent;
            padding-left: 10px !important;
            padding-right: 22px !important;
        }

        .dxeButtonEditSys.dxeButtonEdit_PlasticBlue , .dxeTextBox_PlasticBlue
        {
            height: 30px;
            border-radius: 4px;
        }

        .dxeButtonEditButton_PlasticBlue
        {
            background: #094e8c !important;
            border-radius: 4px !important;
            padding: 0 4px !important;
        }

        .dxeDisabled_PlasticBlue
        {
            z-index: 0 !important;
        }

        .dxtcLite_PlasticBlue > .dxtc-stripContainer .dxtc-activeTab, .dxgvFooter_PlasticBlue
        {
            background: #1b5ea4 !important;
        }

        .simple-select::after {
            /*content: '<';*/
            content: url(../../../assests/images/left-arw.png);
            position: absolute;
            top: 26px;
            right: 13px;
            font-size: 16px;
            transform: rotate(269deg);
            font-weight: 500;
            background: #094e8c;
            color: #fff;
            height: 18px;
            display: block;
            width: 26px;
            /* padding: 10px 0; */
            border-radius: 4px;
            text-align: center;
            line-height: 19px;
            z-index: 0;
        }
        .simple-select {
            position: relative;
        }
        .simple-select:disabled::after
        {
            background: #1111113b;
        }
        select.btn
        {
            padding-right: 10px !important;
        }

        /*select.btn
        {
            padding-right: 10px !important;
        }*/

        /*.panel-group .panel
        {
            box-shadow: 1px 1px 8px #1111113b;
            border-radius: 8px;
        }*/

        .dxpLite_PlasticBlue .dxp-current
        {
            background-color: #1b5ea4;
            padding: 3px 5px;
            border-radius: 2px;
        }

        #accordion {
            margin-bottom: 20px;
            margin-top: 10px;
        }

        .dxgvHeader_PlasticBlue {
    background: #1b5ea4 !important;
    color: #fff !important;
}
        #ShowGrid
        {
            margin-top: 10px;
        }

        .pt-25{
                padding-top: 25px !important;
        }

        .dxgvEditFormDisplayRow_PlasticBlue td.dxgv, .dxgvDataRow_PlasticBlue td.dxgv, .dxgvDataRowAlt_PlasticBlue td.dxgv, .dxgvSelectedRow_PlasticBlue td.dxgv, .dxgvFocusedRow_PlasticBlue td.dxgv
        {
            padding: 6px 6px 6px !important;
        }

        #lookupCardBank_DDD_PW-1
        {
                left: -182px !important;
        }
        .plhead a>i
        {
                top: 9px;
        }

        .clsTo
        {
            display: flex;
    align-items: flex-start;
        }

        input[type="radio"], input[type="checkbox"]
        {
            margin-right: 5px;
        }
        .dxeCalendarDay_PlasticBlue
        {
                padding: 6px 6px;
        }

        .modal-dialog
        {
            width: 50%;
        }

        .modal-header
        {
            padding: 8px 4px 8px 10px;
            background: #094e8c !important;
        }

        .TableMain100 #ShowGrid , .TableMain100 #ShowGridList , .TableMain100 #ShowGridRet , .TableMain100 #EmployeeGrid , .TableMain100 #GrdEmployee,
        .TableMain100 #GrdHolidays 
        {
            max-width: 98% !important;
        }

        /*div.dxtcSys > .dxtc-content > div, div.dxtcSys > .dxtc-content > div > div
        {
            width: 95% !important;
        }*/

        .btn-info
        {
                background-color: #1da8d1 !important;
                background-image: none;
        }

        .for-cust-icon {
            position: relative;
            z-index: 1;
        }

        .dxeDisabled_PlasticBlue, .aspNetDisabled
        {
            background: #f3f3f3 !important;
        }

        .dxeButtonDisabled_PlasticBlue
        {
            background: #b5b5b5 !important;
            border-color: #b5b5b5 !important;
        }

        #ddlValTech
        {
            width: 100% !important;
            margin-bottom: 0 !important;
        }

        .dis-flex
        {
            display: flex;
            align-items: baseline;
        }

        input + label
        {
            line-height: 1;
                margin-top: 7px;
        }

        .dxtlHeader_PlasticBlue
        {
            background: #094e8c !important;
        }

        .dxeBase_PlasticBlue .dxichCellSys
        {
            padding-top: 2px !important;
        }

        .pBackDiv
        {
            border-radius: 10px;
            box-shadow: 1px 1px 10px #1111112e;
        }
        .HeaderStyle th
        {
            padding: 5px;
        }

        .dxtcLite_PlasticBlue.dxtc-top > .dxtc-stripContainer
        {
            padding-top: 15px;
        }

        .pt-2
        {
            padding-top: 5px;
        }
        .pt-10
        {
            padding-top: 10px;
        }

        .pt-15
        {
            padding-top: 15px;
        }

        .pb-10
        {
            padding-bottom: 10px;
        }

        .pTop10 {
    padding-top: 20px;
}
        .custom-padd
        {
            padding-top: 4px;
    padding-bottom: 10px;
        }

        input + label
        {
                margin-right: 10px;
        }

        .btn
        {
            margin-bottom: 0;
        }

        .pl-10
        {
            padding-left: 10px;
        }

        .col-md-3>label, .col-md-3>span
        {
            margin-top: 0 !important;
        }

        .devCheck
        {
            margin-top: 5px;
        }

        .mtc-5
        {
            margin-top: 5px;
        }

        .mtc-10
        {
            margin-top: 10px;
        }

        /*select.btn
        {
           position: relative;
           z-index: 0;
        }*/

        select
        {
            margin-bottom: 0;
        }

        .form-control
        {
            background-color: transparent;
        }

        select.btn-radius {
    padding: 4px 8px 6px 11px !important;
}
        .mt-30{
            margin-top: 30px;
        }

        .panel-title h3
        {
            padding-top: 0;
            padding-bottom: 0;
        }

        .btn-radius
        {
            padding: 4px 11px !important;
            border-radius: 4px !important;
        }

        .crossBtn
        {
             right: 30px;
             top: 25px;
        }

        .mb-10
        {
            margin-bottom: 10px;
        }

        .btn-cust
        {
            background-color: #108b47 !important;
            color: #fff;
        }

        .btn-cust:hover
        {
            background-color: #097439 !important;
            color: #fff;
        }

        .gHesder
        {
            background: #1b5ca0 !important;
            color: #ffffff !important;
            padding: 6px 0 6px !important;
        }

        .close
        {
             color: #fff;
             opacity: .5;
             font-weight: 400;
        }

        .mt-27
        {
            margin-top: 27px !important;
        }

        .col-md-3 , .col-md-6
        {
            margin-top: 8px;
        }

        #upldBigLogo , #upldSmallLogo
        {
            width: 100%;
        }

        #DivSetAsDefault
        {
            margin-top: 25px;
        }

        .btn.btn-xs
        {
                font-size: 14px !important;
        }

        .dxpc-content table
        {
             width: 100%;
        }

        input[type="text"], input[type="password"], textarea
        {
            margin-bottom: 0 !important;
        }
        /*Rev end 1.0*/
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <%--Rev 1.0: "outer-div-main" class add --%>
    <div class="outer-div-main">
        <div class="panel-heading">
        <div class="panel-title">
            <h3>Add Professional/Technical Bodies</h3>
            <div class="crossBtn"><a href="ProfBodies.aspx"><i class="fa fa-times"></i></a></div>
        </div>
    </div>
        <div class="form_main">
        <table class="TableMain100">

            <tr>
                <td>
                    <dxe:ASPxPageControl ID="ASPxPageControl1" runat="server" ActiveTabIndex="0" ClientInstanceName="page"
                        Font-Size="12px" Width="100%">
                        <TabPages>
                            <dxe:TabPage Text="General" Name="General">
                                <ContentCollection>
                                    <dxe:ContentControl runat="server">
                                        <div>
                                            <div class="col-md-3">
                                                <label class="Ecoheadtxt">
                                                    Name :  <span class="iconRed" runat="server">*</span>
                                                </label>

                                                <div style="position: relative">
                                                    <asp:TextBox ID="txtName" runat="server" MaxLength="100" Width="100%"></asp:TextBox>
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtName" ValidationGroup="prof" SetFocusOnError="true" ToolTip="Mandatory." class="pullrightClass fa fa-exclamation-circle abs" ErrorMessage="" ForeColor="red"></asp:RequiredFieldValidator>
                                                </div>
                                            </div>
                                            <div class="col-md-3">
                                                <label class="Ecoheadtxt">Short Name :  <span class="iconRed" runat="server">*</span></label>
                                                <div style="position: relative;">
                                                    <asp:TextBox ID="txtShortName" runat="server" Width="100%" MaxLength="50" onchange="fn_ctxtPro_Name_TextChanged(this);"></asp:TextBox>
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtShortName" ValidationGroup="prof" SetFocusOnError="true" ToolTip="Mandatory." class="pullrightClass fa fa-exclamation-circle abs" ErrorMessage="" ForeColor="red"></asp:RequiredFieldValidator>
                                                </div>
                                            </div>
                                            <div class="col-md-3">
                                                <label class="Ecoheadtxt" style="">
                                                    Registration No :
                                                </label>
                                                <div style="">
                                                    <asp:TextBox ID="txtRegnNumber" runat="server" MaxLength="50" Width="100%"></asp:TextBox>
                                                </div>
                                            </div>
                                            <div class="col-md-3">
                                                <label class="Ecoheadtxt">
                                                    Registration Date :
                                                </label>
                                                <div>
                                                    <dxe:ASPxDateEdit ID="txtRegndate" runat="server" EditFormat="Custom" UseMaskBehavior="true" Width="100%">
                                                        <ButtonStyle Width="13px">
                                                        </ButtonStyle>
                                                    </dxe:ASPxDateEdit>
                                                </div>
                                            </div>
                                            <div class="col-md-3">
                                                <label class="Ecoheadtxt">
                                                    Authority Name :
                                                </label>
                                                <div class="">
                                                    <asp:TextBox ID="txtAuthorityName" runat="server" MaxLength="100" Width="100%"></asp:TextBox>
                                                </div>
                                            </div>
                                            <%--Rev 1.0: "simple-select" class add --%>
                                            <div class="col-md-3 simple-select">
                                                <label class="Ecoheadtxt">
                                                    Country Of Registration :
                                                </label>
                                                <div>
                                                    <asp:DropDownList ID="drpcountry" runat="server" Width="100%">
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                            <div class="col-md-6">
                                                <label class="Ecoheadtxt">
                                                    Description
                                                </label>
                                                <div>
                                                    <asp:TextBox ID="txtDescription" runat="server" TextMode="MultiLine" Width="100%" MaxLength="2000"></asp:TextBox>
                                                </div>
                                            </div>
                                            <div style="clear: both"></div>
                                            <div class="col-md-12 mtc-10">
                                                <asp:Button ID="btnSave" runat="server" ValidationGroup="prof" Text="Save" CssClass="btn btn-primary" OnClick="btnSave_Click" />
                                            </div>
                                        </div>

                                    </dxe:ContentControl>
                                </ContentCollection>
                            </dxe:TabPage>
                            <dxe:TabPage Text="Correspondence" Name="CorresPondence">
                                <ContentCollection>
                                    <dxe:ContentControl runat="server">
                                    </dxe:ContentControl>
                                </ContentCollection>
                            </dxe:TabPage>
                        </TabPages>
                        <ClientSideEvents ActiveTabChanged="function(s, e) {
	                                            var activeTab   = page.GetActiveTab();
	                                            var Tab0 = page.GetTab(0);
	                                            var Tab1 = page.GetTab(1);
	                                            
	                                            if(activeTab == Tab0)
	                                            {
	                                                disp_prompt('tab0');
	                                            }
	                                            if(activeTab == Tab1)
	                                            {
	                                                disp_prompt('tab1');
	                                            }
	                                            }"></ClientSideEvents>
                        <ContentStyle>
                            <Border BorderColor="#002D96" BorderStyle="Solid" BorderWidth="1px" />
                        </ContentStyle>
                        <LoadingPanelStyle ImageSpacing="6px">
                        </LoadingPanelStyle>
                    </dxe:ASPxPageControl>
                </td>
            </tr>
        </table>
    </div>
    </div>
</asp:Content>

