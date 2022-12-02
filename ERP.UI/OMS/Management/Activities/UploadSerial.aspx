<%@ Page Title="" Language="C#" MasterPageFile="~/OMS/MasterPage/PopUp.Master" AutoEventWireup="true" CodeBehind="UploadSerial.aspx.cs" Inherits="ERP.OMS.Management.Activities.UploadSerial" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script>
        function SubmitSerial() {
            upload.Upload();
        }

        function OnUploadComplete(args) {
            parent.UploadComplete();
        }

        function On_UploadComplete() {
            parent.UploadComplete();
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="clearfix col-md-12" style="background: #f5f4f3; padding: 8px 0; margin-bottom: 15px; border-radius: 4px; border: 1px solid #ccc;">
        <div class="col-md-3">
            <div>
                Upload Serial No (Only .xlsx)
            </div>
            <div class="Left_Content" style="">
              <%--  <dxe:ASPxUploadControl ID="ASPxUploadControl1" runat="server" ClientInstanceName="upload"
                    OnFileUploadComplete="ASPxUploadControl1_FileUploadComplete" FileInputCount="1">
                    <ClientSideEvents FileUploadComplete="function(s, e) { OnUploadComplete(e); }" />
                    <ValidationSettings MaxFileSize="3145728" AllowedFileExtensions=".csv">
                    </ValidationSettings>
                </dxe:ASPxUploadControl>--%>
                 <asp:FileUpload ID="uploadProdSalesPrice" runat="server" Width="100%"/>
            </div>
        </div>
        <div class="clear">
            <br />
        </div>
        <div class="col-md-3">
            <div></div>
            <div>
               <%-- <dxe:ASPxButton ID="btnSubmit_Serial" ClientInstanceName="cbtnSubmit_Serial" Width="50px" runat="server" AutoPostBack="False" Text="Add" CssClass="btn btn-primary">
                    <ClientSideEvents Click="SubmitSerial" />
                </dxe:ASPxButton>--%>
                <asp:Button ID="BtnSave" runat="server" Text="Import File" CssClass="btn btn-primary" OnClick="BtnSave_Click" />
            </div>
        </div>
    </div>
</asp:Content>
