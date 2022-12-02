<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/OMS/MasterPage/ERP.Master" Inherits="ERP.OMS.Management.Store.Master.Management_Store_Master_CustomTagPrint" Codebehind="CustomTagPrint.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <link type="text/css" href="../../../CSS/style.css" rel="Stylesheet" />
    <link href="../../../CentralData/CSS/GenericCss.css" rel="stylesheet" type="text/css" />

    <script type="text/javascript" src="/assests/js/loaddata1.js"></script>

    <script type="text/javascript" src="../../../CentralData/JSScript/GenericJScript.js"></script>

    <script type="text/javascript" src="/assests/js/jquery-1.3.2.min.js"></script>
 
   

    <style type="text/css">
        .cityDiv
        {
            height: 25px;
            width: 130px;
            float: left;
            margin-left: 70px;
        }
        .cityTextbox
        {
            height: 25px;
            width: 50px;
        }
        .Top
        {
            height: 90px;
            width: 400px;
            background-color: Silver;
            padding-top: 5px;
            valign: top;
        }
        .Footer
        {
            height: 30px;
            width: 400px;
            background-color: Silver;
            padding-top: 10px;
        }
        .ScrollDiv
        {
            height: 250px;
            width: 400px;
            background-color: Silver;
            overflow-x: hidden;
            overflow-y: scroll;
        }
        .ContentDiv
        {
            width: 400px;
            height: 300px;
            border: 2px;
            background-color: Silver;
        }
        
        .TitleArea
        {
            height: 20px;
            padding-left: 10px;
            padding-right: 3px;
            background-image: url( '../images/EHeaderBack.gif' );
            background-repeat: repeat-x;
            background-position: bottom;
            text-align: center;
        }
        .FilterSide
        {
            float: left;
            padding-left: 15px;
            width: 50%;
        }
        .SearchArea
        {
            width: 100%;
            height: 30px;
            padding-top: 5px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div>
        <div class="Main">
            <div class="TitleArea">
                <strong><span style="color: #000099">Custom Tag print</span></strong>
            </div>
            <div class="SearchArea">
            
            </div>
        </div> 
    </div>
</asp:Content>

