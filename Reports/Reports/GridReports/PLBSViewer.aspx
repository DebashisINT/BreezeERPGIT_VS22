<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PLBSViewer.aspx.cs" Inherits="Reports.Reports.GridReports.PLBSViewer" %>
<%@ Register Assembly="DevExpress.XtraReports.v15.1.Web, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.XtraReports.Web" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.XtraCharts.v15.1.Web, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.XtraCharts.Web" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.XtraCharts.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.XtraCharts" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dxe" %>



<!DOCTYPE html>
    <link rel="stylesheet" type="text/css" href="/assests/fonts/font-awesome/css/font-awesome.min.css" />
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>


    <script>

        function docprintinit(s, e) {
            var createFrameElement = s.viewer.printHelper.createFrameElement;
            s.viewer.printHelper.createFrameElement = function (name) {
                var frameElement = createFrameElement.call(this, name);
                if (ASPx.Browser.Chrome) {
                    frameElement.addEventListener("load", function (e) {
                        if (frameElement.contentDocument.contentType !== "text/html")
                            frameElement.contentWindow.print();
                    });
                }
                return frameElement;
            }

            s.Print();

        }

    </script>
    <style>
        .crossBtn {
            border: 1px solid #e84a30;
            border-radius: 50%;
            color: #F44336;
            font-size: 20px !important;
            height: 35px;
            line-height: 33px !important;
            position: absolute;
            right: 15px;
            text-align: center;
            top: 55px;
            width: 35px;
        }
        .crossBtn>a {
            color:#F44336;
        }
        .crossBtn:hover{
            background:#F44336;
            color:#fff;
            box-shadow:0px 3px 4px rgba(0,0,0,0.12);
        }
        .crossBtn > a:hover, .crossBtn:hover > a {
            color:#fff;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="panel-heading">
            <div class="panel-title">
                <div class="crossBtn"><a id="lnkClose" runat="server" href=""><i class="fa fa-times"></i></a></div>
            </div> 
        </div>
        <div class="form_main">
    

        <dx:ASPxDocumentViewer ID="ASPxDocumentViewer1" runat="server" ClientInstanceName="DocViewer">
                        <ClientSideEvents Init="docprintinit" />
                        <ToolbarItems>
                            <dx:ReportToolbarButton ItemKind="Search" />
                            <dx:ReportToolbarSeparator />
                            <dx:ReportToolbarButton ItemKind="PrintReport" />
                            <dx:ReportToolbarButton ItemKind="PrintPage" />
                            <dx:ReportToolbarSeparator />
                            <dx:ReportToolbarButton Enabled="False" ItemKind="FirstPage" />
                            <dx:ReportToolbarButton Enabled="False" ItemKind="PreviousPage" />
                            <dx:ReportToolbarLabel ItemKind="PageLabel" />
                            <dx:ReportToolbarComboBox ItemKind="PageNumber" Width="65px">
                            </dx:ReportToolbarComboBox>
                            <dx:ReportToolbarLabel ItemKind="OfLabel" />
                            <dx:ReportToolbarTextBox IsReadOnly="True" ItemKind="PageCount" />
                            <dx:ReportToolbarButton ItemKind="NextPage" />
                            <dx:ReportToolbarButton ItemKind="LastPage" />
                            <dx:ReportToolbarSeparator />
                            <dx:ReportToolbarButton ItemKind="SaveToDisk" />
                            <dx:ReportToolbarButton ItemKind="SaveToWindow" />
                            <dx:ReportToolbarComboBox ItemKind="SaveFormat" Width="70px">
                                <Elements>
                                    <dx:ListElement Value="pdf" />
                                    <dx:ListElement Value="xls" />
                                    <dx:ListElement Value="xlsx" />
                                    <dx:ListElement Value="rtf" />
                                    <dx:ListElement Value="mht" />
                                    <dx:ListElement Value="html" />
                                    <dx:ListElement Value="txt" />
                                    <dx:ListElement Value="csv" />
                                    <dx:ListElement Value="png" />
                                </Elements>
                            </dx:ReportToolbarComboBox>

                            <%--<dx:ReportToolbarButton IconID="mail_mail_16x16" Name="Email" ToolTip="Email" />--%>
                            <dx:ReportToolbarButton ItemKind="Custom" Name="CustomEmail" ToolTip="Email" Enabled="true" IconID="mail_mail_16x16"></dx:ReportToolbarButton>
                        </ToolbarItems>
                        
                    </dx:ASPxDocumentViewer>







    </div>
    </form>
</body>
</html>
