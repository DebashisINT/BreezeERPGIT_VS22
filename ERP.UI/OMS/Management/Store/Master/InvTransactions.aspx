<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/OMS/MasterPage/ERP.Master"
    Inherits="ERP.OMS.Management.Store.Master.Management_Store_Master_InvTransactions" Codebehind="InvTransactions.aspx.cs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .dxeMemoEditArea
        {
            height: 80px;
        }
    </style>
    <title></title>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div style="background-color: #d3ddeb; overflow: hidden;width:1024px">
        <div style="background-color: #4d74a8">
            <h3 style="color: #fff; margin: 0; font-size: 12px; text-transform: uppercase; font-family: Arial, Helvetica, sans-serif;
                padding: 10px 0; text-align: center">
                ENTER TRANSACTION
            </h3>
        </div>
        <div class="field" style="border: 2px solid #fff; margin: 5px; overflow: hidden">
            <div style="float: left; width: 55%; border-right: 2px solid #fff">
                <div style="padding: 10px 20px">
                    <div style="margin: 10px 0; overflow: hidden">
                        <div style="display: inline-block; vertical-align: middle; width: 30%; font-size: 12px;
                            font-weight: bold; font-family: Arial, Helvetica, sans-serif;">
                            <label>
                                Type
                            </label>
                        </div>
                        <div style="display: inline-block; vertical-align: middle; width: 64%; background-color: #d3ddeb;
                            border: 1px solid #fff; padding: 3px 5px">
                            <dxe:ASPxComboBox ID="cmbType" runat="server" Style="width: 100%">
                            </dxe:ASPxComboBox>
                        </div>
                    </div>
                    <div style="margin: 10px 0; overflow: hidden">
                        <div style="display: inline-block; vertical-align: middle; width: 30%; font-size: 12px;
                            font-weight: bold; font-family: Arial, Helvetica, sans-serif;">
                            <label>
                                Date
                            </label>
                        </div>
                        <div style="display: inline-block; vertical-align: middle; width: 25%; background-color: #d3ddeb;
                            border: 1px solid #fff; padding: 3px 5px">
                            <dxe:ASPxDateEdit ID="dtDate" runat="server" style="width:100%">
                            </dxe:ASPxDateEdit>
                        </div>
                    </div>
                    <div style="margin: 10px 0; overflow: hidden">
                        <div style="display: inline-block; vertical-align: middle; width: 30%; font-size: 12px;
                            font-weight: bold; font-family: Arial, Helvetica, sans-serif;">
                            <label>
                                Order Number
                            </label>
                        </div>
                        <div style="display: inline-block; vertical-align: middle; width: 65%; background-color: #d3ddeb;
                            border: 1px solid #fff; padding: 3px 5px">
                            <dxe:ASPxTextBox ID="txtOrderNo" runat="server" Style="width: 100%">
                            </dxe:ASPxTextBox>
                        </div>
                    </div>
                    <div style="margin: 10px 0; overflow: hidden">
                        <div style="display: inline-block; vertical-align: middle; width: 30%; font-size: 12px;
                            font-weight: bold; font-family: Arial, Helvetica, sans-serif;">
                            <label>
                                Order Date
                            </label>
                        </div>
                        <div style="display: inline-block; vertical-align: middle; width: 25%; background-color: #d3ddeb;
                            border: 1px solid #fff; padding: 3px 5px">
                            <dxe:ASPxDateEdit ID="dtOrderDate" runat="server" style="width:100%">
                            </dxe:ASPxDateEdit>
                        </div>
                    </div>
                    <div style="margin: 10px 0; overflow: hidden">
                        <div style="display: inline-block; vertical-align: middle; width: 30%; font-size: 12px;
                            font-weight: bold; font-family: Arial, Helvetica, sans-serif;">
                            <label>
                                Customer/Vendor
                            </label>
                        </div>
                        <div style="display: inline-block; vertical-align: middle; width: 65%; background-color: #d3ddeb;
                            border: 1px solid #fff; padding: 3px 5px">
                            <dxe:ASPxTextBox ID="txtCustomer" runat="server" Style="width: 100%">
                            </dxe:ASPxTextBox>
                        </div>
                    </div>
                </div>
            </div>
            <div style="float: right; width: 40%; overflow: hidden">
                <div style="padding: 10px 20px">
                    <div style="margin: 10px 0; overflow: hidden">
                        <div style="display: inline-block; vertical-align: middle; width: 30%; font-size: 12px;
                            font-weight: bold; font-family: Arial, Helvetica, sans-serif;">
                            <label>
                                Delivery Reference
                            </label>
                        </div>
                        <div style="display: inline-block; vertical-align: middle; width: 65%; background-color: #d3ddeb;
                            border: 1px solid #fff; padding: 3px 5px" rows="3">
                            <dxe:ASPxMemo ID="memoDeliveryRef" runat="server" Style="width: 100%; height: 80px;">
                            </dxe:ASPxMemo>
                        </div>
                    </div>
                    <div style="margin: 10px 0; overflow: hidden">
                        <div style="display: inline-block; vertical-align: middle; width: 30%; font-size: 12px;
                            font-weight: bold; font-family: Arial, Helvetica, sans-serif;">
                            <label>
                                Remarks
                            </label>
                        </div>
                        <div style="display: inline-block; vertical-align: middle; width: 65%; background-color: #d3ddeb;
                            border: 1px solid #fff; padding: 3px 5px" rows="3">
                            <dxe:ASPxMemo ID="memoRemarks" runat="server" Style="width: 100%">
                            </dxe:ASPxMemo>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div style="clear: both">
        </div>
        <div style="border: 2px solid #fff; margin: 5px; overflow: hidden; float: left; padding: 5px;
            width: 65%">
            <div style="background-color: #4d74a8">
                <h3 style="color: #fff; margin: 0; font-size: 12px; text-transform: uppercase; font-family: Arial, Helvetica, sans-serif;
                    padding: 10px 0; text-align: center">
                    Product Detail
                </h3>
            </div>
            <div style="float: left; width: 55%;">
                <div style="padding: 10px 20px">
                    <div style="margin: 10px 0; overflow: hidden">
                        <div style="display: inline-block; vertical-align: middle; width: 30%; font-size: 12px;
                            font-weight: bold; font-family: Arial, Helvetica, sans-serif;">
                            <label>
                                Product
                            </label>
                        </div>
                        <div style="display: inline-block; vertical-align: middle; width: 65%; background-color: #d3ddeb;
                            border: 1px solid #fff; padding: 3px 5px">
                            <dxe:ASPxTextBox ID="txtProduct" runat="server" Style="width: 100%">
                            </dxe:ASPxTextBox>
                        </div>
                    </div>
                    <div style="margin: 10px 0; overflow: hidden">
                        <div style="display: inline-block; vertical-align: middle; width: 30%; font-size: 12px;
                            font-weight: bold; font-family: Arial, Helvetica, sans-serif;">
                            <label>
                                Brand
                            </label>
                        </div>
                        <div style="display: inline-block; vertical-align: middle; width: 65%; background-color: #d3ddeb;
                            border: 1px solid #fff; padding: 3px 5px">
                            <dxe:ASPxTextBox ID="txtBrand" runat="server" Style="width: 100%">
                            </dxe:ASPxTextBox>
                        </div>
                    </div>
                    <div style="margin: 10px 0; overflow: hidden">
                        <div style="display: inline-block; vertical-align: middle; width: 30%; font-size: 12px;
                            font-weight: bold; font-family: Arial, Helvetica, sans-serif;">
                            <label>
                                Size/Strength
                            </label>
                        </div>
                        <div style="display: inline-block; vertical-align: middle; width: 65%; background-color: #d3ddeb;
                            border: 1px solid #fff; padding: 3px 5px">
                            <dxe:ASPxComboBox ID="cmbSize" runat="server" Style="width: 100%">
                            </dxe:ASPxComboBox>
                        </div>
                    </div>
                    <div style="margin: 10px 0; overflow: hidden">
                        <div style="display: inline-block; vertical-align: middle; width: 30%; font-size: 12px;
                            font-weight: bold; font-family: Arial, Helvetica, sans-serif;">
                            <label>
                                Color
                            </label>
                        </div>
                        <div style="display: inline-block; vertical-align: middle; width: 65%; background-color: #d3ddeb;
                            border: 1px solid #fff; padding: 3px 5px">
                            <dxe:ASPxComboBox ID="cmbColor" runat="server" Style="width: 100%">
                            </dxe:ASPxComboBox>
                        </div>
                    </div>
                    <div style="margin: 10px 0; overflow: hidden">
                        <div style="display: inline-block; vertical-align: middle; width: 30%; font-size: 12px;
                            font-weight: bold; font-family: Arial, Helvetica, sans-serif;">
                            <label>
                                Quantity
                            </label>
                        </div>
                        <div style="display: inline-block; vertical-align: middle; width: 20%; background-color: #d3ddeb;
                            border: 1px solid #fff; padding: 3px 5px; margin-right: 5px">
                            <dxe:ASPxTextBox ID="txtQuantity" runat="server" Style="width: 100%">
                            </dxe:ASPxTextBox>
                        </div>
                        <div style="display: inline-block; vertical-align: middle; width: 15%; background-color: #d3ddeb;
                            border: 1px solid #fff; padding: 3px 5px" placeholder="UM">
                            <dxe:ASPxTextBox ID="txtQuantityUnit" runat="server">
                            </dxe:ASPxTextBox>
                        </div>
                    </div>
                    <div style="margin: 10px 0; overflow: hidden">
                        <div style="display: inline-block; vertical-align: middle; width: 30%; font-size: 12px;
                            font-weight: bold; font-family: Arial, Helvetica, sans-serif;">
                            <label>
                                Currency
                            </label>
                        </div>
                        <div style="display: inline-block; vertical-align: middle; width: 25%; background-color: #d3ddeb;
                            border: 1px solid #fff; padding: 3px 5px">
                            <dxe:ASPxComboBox ID="cmbCurrency" runat="server" Style="width: 100%">
                            </dxe:ASPxComboBox>
                        </div>
                    </div>
                    <div style="margin: 10px 0; overflow: hidden">
                        <div style="display: inline-block; vertical-align: middle; width: 30%; font-size: 12px;
                            font-weight: bold; font-family: Arial, Helvetica, sans-serif;">
                            <label>
                                Price
                            </label>
                        </div>
                        <div style="display: inline-block; width: 20%; background-color: #d3ddeb; border: 1px solid #fff;
                            padding: 3px 5px; margin-right: 5px">
                            <dxe:ASPxTextBox ID="txtPrice" runat="server">
                            </dxe:ASPxTextBox>
                        </div>
                        <div style="display: inline-block; width: 15%; background-color: #d3ddeb; border: 1px solid #fff;
                            padding: 3px 5px; margin-right: 5px">
                            <dxe:ASPxTextBox ID="txtPerPrice" runat="server">
                            </dxe:ASPxTextBox>
                        </div>
                        <div style="display: inline-block; width: 15%; background-color: #d3ddeb; border: 1px solid #fff;
                            padding: 3px 5px">
                            <dxe:ASPxTextBox ID="txtPriceUnit" runat="server">
                            </dxe:ASPxTextBox>
                        </div>
                    </div>
                    <div style="margin: 10px 0; overflow: hidden">
                        <div style="display: inline-block; vertical-align: middle; width: 30%; font-size: 12px;
                            font-weight: bold; font-family: Arial, Helvetica, sans-serif;">
                            <label>
                                Batch Number
                            </label>
                        </div>
                        <div style="display: inline-block; vertical-align: middle; width: 65%; background-color: #d3ddeb;
                            border: 1px solid #fff; padding: 3px 5px">
                             <dxe:ASPxTextBox ID="txtBatchNo" runat="server" style="width:100%">
                            </dxe:ASPxTextBox>
                        </div>
                    </div>
                </div>
            </div>
            <div style="float: right; width: 45%; overflow: hidden">
                <div style="padding: 10px 20px">
                    <div style="margin: 10px 0; overflow: hidden">
                        <div style="display: inline-block; vertical-align: middle; width: 25%; font-size: 12px;
                            font-weight: bold; font-family: Arial, Helvetica, sans-serif;">
                            <label>
                                Description
                            </label>
                        </div>
                        <div style="display: inline-block; vertical-align: middle; width: 68%; background-color: #d3ddeb;
                            border: 1px solid #fff; padding: 3px 5px" rows="16">
                            <dxe:ASPxMemo ID="memoDescription" runat="server" Style="width: 100%">
                            </dxe:ASPxMemo>
                            
                        </div>
                    </div>
                </div>
            </div>
            
          
            <div style="overflow: hidden; clear: both">
                <div style="padding: 0px 20px">
                    <div style="margin: 10px 0; overflow: hidden; float: left; width: 30%">
                        <div style="float: left; width: 46%; font-size: 12px; font-weight: bold; font-family: Arial, Helvetica, sans-serif;">
                            <label>
                                Manufacture Date
                            </label>
                        </div>
                        <div style="float: left; width: 30%; background-color: #d3ddeb; border: 1px solid #fff;
                            padding: 3px 5px; margin-left: 20px">
                            <dxe:ASPxDateEdit ID="dtManufactureDate" runat="server" Style="width: 100%">
                            </dxe:ASPxDateEdit>
                        </div>
                    </div>
                    <div style="margin: 10px 0; overflow: hidden; float: left; width: 25%">
                        <div style="float: left; width: 35%; font-size: 12px; font-weight: bold; font-family: Arial, Helvetica, sans-serif;
                            margin-left: 20px">
                            <label>
                                Expiry Date
                            </label>
                        </div>
                        <div style="float: left; width: 30%; background-color: #d3ddeb; border: 1px solid #fff;
                            padding: 3px 5px; margin-left: 20px">
                            <dxe:ASPxDateEdit ID="dtExpiryDate" runat="server" Style="width: 100%">
                            </dxe:ASPxDateEdit>
                        </div>
                    </div>
                    <div style="margin: 10px 0; overflow: hidden; float: left; width: 35%">
                        <div style="float: left; width: 50%; font-size: 12px; font-weight: bold; font-family: Arial, Helvetica, sans-serif;
                            margin-left: 20px">
                            <label>
                                Best Before [Use]
                            </label>
                        </div>
                        <div style="float: left; width: 30%; background-color: #d3ddeb; border: 1px solid #fff;
                            padding: 3px 5px; margin-left: 20px">
                            <dxe:ASPxDateEdit ID="dtBestBefore" runat="server" Style="width: 100%">
                            </dxe:ASPxDateEdit>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div style="border: 2px solid #fff; margin: 5px; overflow: hidden; float: right;
            padding: 5px; width: 30%">
            <div style="background-color: #4d74a8">
                <h3 style="color: #fff; margin: 0; font-size: 12px; text-transform: uppercase; font-family: Arial, Helvetica, sans-serif;
                    padding: 10px 0; text-align: center">
                    delivery location
                </h3>
            </div>
            <div style="padding: 3px; border: 1px solid #fff; margin: 5px 0">
                <h3 style="font-size: 12px; font-family: Arial, Helvetica, sans-serif; text-align: center">
                    Warehouse
                </h3>
                <div style="margin: 10px 0; overflow: hidden; padding: 0 3px">
                    <div style="display: inline-block; vertical-align: top; width: 30%; font-size: 12px;
                        font-weight: bold; font-family: Arial, Helvetica, sans-serif;">
                        <label>
                            Delivered from
                        </label>
                    </div>
                    <div style="display: inline-block; width: 64%; background-color: #d3ddeb; border: 1px solid #fff;
                        padding: 3px 5px">
                        <dxe:ASPxComboBox ID="cmbWHDeliveryFrom" runat="server" Style="width: 100%">
                        </dxe:ASPxComboBox>
                    </div>
                </div>
                <div style="margin: 10px 0; overflow: hidden; padding: 0 3px">
                    <div style="display: inline-block; vertical-align: top; width: 30%; font-size: 12px;
                        font-weight: bold; font-family: Arial, Helvetica, sans-serif;">
                        <label>
                            Delivered To
                        </label>
                    </div>
                    <div style="display: inline-block; width: 64%; background-color: #d3ddeb; border: 1px solid #fff;
                        padding: 3px 5px">
                        <dxe:ASPxComboBox ID="cmbWHDeliveryTo" runat="server" Style="width: 100%">
                        </dxe:ASPxComboBox>
                    </div>
                </div>
            </div>
            <div style="padding: 3px; border: 1px solid #fff; margin: 5px 0">
                <h3 style="font-size: 12px; font-family: Arial, Helvetica, sans-serif; text-align: center">
                    Customer/Vendor's Location
                </h3>
                <div style="margin: 10px 0; overflow: hidden; padding: 0 3px">
                    <div style="display: inline-block; vertical-align: top; width: 30%; font-size: 12px;
                        font-weight: bold; font-family: Arial, Helvetica, sans-serif;">
                        <label>
                            Delivered from
                        </label>
                    </div>
                    <div style="display: inline-block; width: 64%; background-color: #d3ddeb; border: 1px solid #fff;
                        padding: 3px 5px"> 
                            <dxe:ASPxComboBox ID="cmbClDeliveryFrom" runat="server" Style="width: 100%">
                            </dxe:ASPxComboBox>
                    </div>
                </div>
                <div style="margin: 10px 0; overflow: hidden; padding: 0 3px">
                    <div style="display: inline-block; vertical-align: top; width: 30%; font-size: 12px;
                        font-weight: bold; font-family: Arial, Helvetica, sans-serif;">
                        <label>
                            Delivered To
                        </label>
                    </div>
                    <div style="display: inline-block; width: 64%; background-color: #d3ddeb; border: 1px solid #fff;
                        padding: 3px 5px">
                        <dxe:ASPxComboBox ID="cmbClDeliveryTo" runat="server" Style="width: 100%">
                        </dxe:ASPxComboBox>
                    </div>
                </div>
            </div>
            <div style="padding: 3px; border: 1px solid #fff; margin: 5px 0">
                <h3 style="font-size: 12px; font-family: Arial, Helvetica, sans-serif; text-align: center">
                    Other Location
                </h3>
                <div style="margin: 10px 0; overflow: hidden; padding: 0 3px">
                    <div style="display: inline-block; vertical-align: top; width: 30%; font-size: 12px;
                        font-weight: bold; font-family: Arial, Helvetica, sans-serif;">
                        <label>
                            Delivered from
                        </label>
                    </div>
                    <div style="display: inline-block; width: 64%; background-color: #d3ddeb; border: 1px solid #fff;
                        padding: 3px 5px">
                        <dxe:ASPxComboBox ID="cmbOlDeliveryFrom" runat="server" Style="width: 100%">
                        </dxe:ASPxComboBox>
                    </div>
                </div>
                <div style="margin: 10px 0; overflow: hidden; padding: 0 3px">
                    <div style="display: inline-block; vertical-align: top; width: 30%; font-size: 12px;
                        font-weight: bold; font-family: Arial, Helvetica, sans-serif;">
                        <label>
                            Delivered To
                        </label>
                    </div>
                    <div style="display: inline-block; width: 64%; background-color: #d3ddeb; border: 1px solid #fff;
                        padding: 3px 5px">
                        <dxe:ASPxComboBox ID="cmbOlDeliveryTo" runat="server" Style="width: 100%">
                        </dxe:ASPxComboBox>
                    </div>
                </div>
            </div>
        </div>
        <div style="clear: both">
        </div>
        <div class="fieldBtns" style="background-color: #4d74a8; margin: 0 auto; text-align: center;
            padding: 10px 0">
            <div class="saveBtn" style="display: inline-block; margin: 0 10px">
                <div style="background: #4d74a8; color: #fff; border: 2px solid #fff; padding: 5px 10px;
                    text-transform: uppercase; font-weight: bold; font-size: 12px; letter-spacing: 0.8px">
                    <dxe:ASPxButton ID="btnSave" runat="server" Text="Save">
                    </dxe:ASPxButton>
                </div>
            </div>
            <div class="cancelBtn" style="display: inline-block; margin: 0 10px">
                <div style="background: #4d74a8; color: #fff; border: 2px solid #fff; padding: 5px 10px;
                    text-transform: uppercase; font-weight: bold; font-size: 12px; letter-spacing: 0.8px">
                    <dxe:ASPxButton ID="btnCancel" runat="server" Text="Cancel">
                    </dxe:ASPxButton>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
