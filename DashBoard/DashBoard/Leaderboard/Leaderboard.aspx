<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Leaderboard.aspx.cs" Inherits="DashBoard.DashBoard.Leaderboard.Leaderboard" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="~/Scripts/pluggins/multiselect/bootstrap-multiselect.css" rel="stylesheet" />
    <script src="https://cdnjs.cloudflare.com/ajax/libs/jquery/3.3.1/jquery.js"></script>
    <script src="../Js/bootstrap.min.js"></script>
    <script src="../Js/moment.min.js"></script>
    <script src="../Js/PurchaseDb.js"></script>
    <link href="../css/bootstrap.min.css" rel="stylesheet" />
<link href="/Scripts/Charts/styles.css" rel="stylesheet" />

<link href="/Content/media.css" rel="stylesheet" />
<script src="/Scripts/Charts/apexcharts.min.js"></script>
<link href="/assests/pluggins/LightBox/lightbox.css" rel="stylesheet" />
<script src="/assests/pluggins/LightBox/lightbox.js"></script>
<script src="/Scripts/pluggins/multiselect/bootstrap-multiselect.js"></script>
<style type="text/css">
    /* set the size of the datepicker search control for Order Date*/
    #ui-datepicker-div {
        font-size: 11px;
    }

    /* set the size of the autocomplete search control*/
    .ui-menu-item {
    }

    .ui-autocomplete {
        font-size: 11px;
    }

     .mr0 {
        margin-right: 0 !important;
    }

    .widgetBox {
        position: relative;
        margin-top: 20px;
    }

    .numCnt {
        padding: 15px 25px;
    }

    .widgetBox .numCnt .number {
        font-size: 26px;
        font-weight: 700;
        color: #fff;
    }

    .widgetBox .numCnt .text {
        font-size: 15px;
        color: #fff;
    }

    .widgetBox h4 {
        padding: 10px 25px;
        background: #398b9e;
        color: #fff;
    }

    .widgetBox.one {
        background: #4bacc3;
    }

    .widgetBox.two {
        background: #4bc3ae;
    }

    .widgetBox.three {
        background: #b6c72f;
    }

    .widgetBox.four {
        background: #e47b5b;
    }

    .widgetBox.two h4 {
        background: #36a995;
    }

    .widgetBox.three h4 {
        background: #9ead26;
    }

    .widgetBox.four h4 {
        background: #ce6646;
    }

    .widgetBox .fa {
        position: absolute;
        font-size: 43px;
        color: rgba(53, 50, 50, 0.5);
        top: 18px;
        right: 18px;
    }

    .boxForChart {
        border-radius: 5px;
        overflow: hidden;
        margin-bottom: 10px;
        background: #fff;
        box-shadow: 0px 0px 5px rgba(0,0,0,0.06);
        margin-top: 5px;
        padding: 8px 0;
    }

    .SelectChart {
        width: 200px !important;
        max-width: 200px;
        margin: 8px 0;
    }

    .cngView {
        margin-right: 15px;
        padding-top: 7px;
    }

    .refine-btn {
        position: absolute;
        right: 50px;
        top: 15px;
    }

    .activeW {
        /*background: #bbc8ec !important;*/
        -webkit-transform: scale(1.1);
        -moz-transform: scale(1.1);
        transform: scale(1.1);
        z-index: 8;
        box-shadow: 0px 0px 10px rgba(0,0,0,0.9);
    }

    .dateT {
        margin-top: 8px;
    }

        .dateT > tbody > tr > td:first-child {
            font-size: 16px;
            padding-right: 15px;
        }

    .ltri {
        display: inline-block;
        background: #64c7ab;
        padding: 3px 8px;
        border-radius: 4px;
        position: relative;
        color: #fff;
    }

        .ltri:after {
            content: '';
            width: 0;
            height: 0;
            border-top: 11px solid transparent;
            border-bottom: 11px solid transparent;
            border-left: 12px solid #64c7ab;
            position: absolute;
            right: -7px;
        }

    #lblDate {
        font-size: 17px;
    }

    .rightSide {
        background: rgb(239, 244, 247) !important;
        margin-left: 168px;
    }

    .ClasicStyle{
        width:100%;
    }
    .ClasicStyle > thead > tr > th {
        background: #f3f3f3;
        font-weight: 500;
    }
    .ClasicStyle > thead > tr > th, .ClasicStyle > tbody > tr > td {
        border: 1px solid #ccc;
        font-size: 14px;
        padding: 3px 5px;
    }


    .mTop15 {
        margin-top: 15px;
    }
    #mixedChart {
        width: 100%;
        height: 150px;
    }
    .d-flex {
        display: -ms-flexbox !important;
        display: flex !important;
        font-family: 'Open Sans', sans-serif;
    }

    #map {
        margin-bottom: 20px;
    }

    .d-inline-flex {
        display: -ms-inline-flexbox !important;
        display: inline-flex !important;
    }

    .flex-row {
        -ms-flex-direction: row !important;
        flex-direction: row !important;
    }

    .flex-column {
        -ms-flex-direction: column !important;
        flex-direction: column !important;
    }

    .flex-row-reverse {
        -ms-flex-direction: row-reverse !important;
        flex-direction: row-reverse !important;
    }

    .flex-column-reverse {
        -ms-flex-direction: column-reverse !important;
        flex-direction: column-reverse !important;
    }

    .flex-wrap {
        -ms-flex-wrap: wrap !important;
        flex-wrap: wrap !important;
    }

    .flex-nowrap {
        -ms-flex-wrap: nowrap !important;
        flex-wrap: nowrap !important;
    }

    .flex-wrap-reverse {
        -ms-flex-wrap: wrap-reverse !important;
        flex-wrap: wrap-reverse !important;
    }

    .flex-fill {
        -ms-flex: 1 1 auto !important;
        flex: 1 1 auto !important;
    }

    .flex-grow-0 {
        -ms-flex-positive: 0 !important;
        flex-grow: 0 !important;
    }

    .flex-grow-1 {
        -ms-flex-positive: 1 !important;
        flex-grow: 1 !important;
    }

    .mr20 {
        margin-right: 20px;
    }

    .flex-shrink-0 {
        -ms-flex-negative: 0 !important;
        flex-shrink: 0 !important;
    }

    .flex-shrink-1 {
        -ms-flex-negative: 1 !important;
        flex-shrink: 1 !important;
    }

    .justify-content-start {
        -ms-flex-pack: start !important;
        justify-content: flex-start !important;
    }

    .justify-content-end {
        -ms-flex-pack: end !important;
        justify-content: flex-end !important;
    }

    .justify-content-center {
        -ms-flex-pack: center !important;
        justify-content: center !important;
    }

    .justify-content-between {
        -ms-flex-pack: justify !important;
        justify-content: space-between !important;
    }

    .justify-content-around {
        -ms-flex-pack: distribute !important;
        justify-content: space-around !important;
    }

    .align-items-start {
        -ms-flex-align: start !important;
        align-items: flex-start !important;
    }

    .align-items-end {
        -ms-flex-align: end !important;
        align-items: flex-end !important;
    }

    .align-items-center {
        -ms-flex-align: center !important;
        align-items: center !important;
    }

    .align-items-baseline {
        -ms-flex-align: baseline !important;
        align-items: baseline !important;
    }

    .align-items-stretch {
        -ms-flex-align: stretch !important;
        align-items: stretch !important;
    }

    .align-content-start {
        -ms-flex-line-pack: start !important;
        align-content: flex-start !important;
    }

    .align-content-end {
        -ms-flex-line-pack: end !important;
        align-content: flex-end !important;
    }

    .align-content-center {
        -ms-flex-line-pack: center !important;
        align-content: center !important;
    }

    .align-content-between {
        -ms-flex-line-pack: justify !important;
        align-content: space-between !important;
    }

    .align-content-around {
        -ms-flex-line-pack: distribute !important;
        align-content: space-around !important;
    }

    .align-content-stretch {
        -ms-flex-line-pack: stretch !important;
        align-content: stretch !important;
    }

    .align-self-auto {
        -ms-flex-item-align: auto !important;
        align-self: auto !important;
    }

    .align-self-start {
        -ms-flex-item-align: start !important;
        align-self: flex-start !important;
    }

    .align-self-end {
        -ms-flex-item-align: end !important;
        align-self: flex-end !important;
    }

    .align-self-center {
        -ms-flex-item-align: center !important;
        align-self: center !important;
    }

    .align-self-baseline {
        -ms-flex-item-align: baseline !important;
        align-self: baseline !important;
    }

    .align-self-stretch {
        -ms-flex-item-align: stretch !important;
        align-self: stretch !important;
    }

    a.infoButton {
        font-size: 18px;
        font-weight: bold;
        cursor: pointer;
        color: #2245c7;
        font-family: 'Montserrat', sans-serif !important;
    }

    .boxWidget {
        padding: 12px;
        background: #fff;
        border-radius: 1px;
        margin-bottom: 8px;
        font-size: 15px;
        cursor: pointer;
        position: relative;
        -webkit-transition: all 0.3s ease-out;
        -moz-transition: all 0.3s ease-out;
        transition: all 0.3s ease-out;
    }

        .boxWidget:before {
            top: 44px;
            content: '';
            width: 60%;
            height: 100%;
            background: rgba(255, 255, 255, 0.05);
            position: absolute;
            /* z-index: -1; */
            -webkit-transform: rotate(45deg);
            transform: rotate(45deg);
            left: -25px;
            border-radius: 10px;
            -webkit-transition: all 0.3s ease-out;
            -moz-transition: all 0.3s ease-out;
            transition: all 0.3s ease-out;
        }

        .boxWidget:hover {
            opacity: 0.9;
            /*box-shadow: 0px 0px 38px rgba(187, 187, 187, 0.4);*/
            /*transform: scale(1.06);*/
        }

            .boxWidget:hover::before {
                -webkit-transform: rotate(58deg) scale(1.2);
                transform: rotate(58deg) scale(1.2);
            }

    .flex1 {
        flex: 1;
    }

    .flex2 {
        flex: 2;
    }

    .flex3 {
        flex: 3;
    }

    .flex4 {
        flex: 4;
    }

    .flex50 {
        flex-basis: 49%;
    }

    .flex70 {
        flex-basis: 69.5%;
    }

    .flex30 {
        flex-basis: 29.5%;
    }

    .flex60 {
        flex-basis: 59.5%;
    }

    .flex40 {
        flex-basis: 39.5%;
    }

    .flex-full {
        height: 100%;
        width: 100%;
    }

    .heightFull {
        height: 100%;
    }

    .Widgheading {
        font-size: 16px;
        margin-bottom: 10px !important;
        font-weight: 700;
        font-family: 'Open Sans', sans-serif !important;
        letter-spacing: -1px;
    }

    .mainLabel {
        font-size: 14px;
        /*letter-spacing: -1px;*/
        font-weight: 500;
        font-family: 'Montserrat', sans-serif !important;
    }

    .subLabel {
        font-size: 9px;
        font-family: 'Montserrat', sans-serif;
    }

    .wdgContent {
        padding: 0px 0 0 0;
    }

    .WdgLetter {
        font-size: 36px;
        color: inherit;
        font-family: 'Open Sans Condensed', sans-serif !important;
        font-weight: bold;
    }

    .showD {
        font-size: 9px;
        display: block;
        text-align: right;
        position: absolute;
        right: 10px;
        bottom: 10px;
    }

    .pad30 {
        padding-top: 50px;
    }

    #panelRight {
        width: 300px;
        position: fixed;
        border-left: 4px solid #398b9e;
        right: -295px;
        height: calc(100vh - 60px);
        top: 60px;
        background: #fff;
        z-index: 99;
        -webkit-transition: all 0.3s ease-out;
        -moz-transition: all 0.3s ease-out;
        transition: all 0.3s ease-out;
    }

    #panelRight.open {
        right: 0px;
    }

    .panelAnchor {
        position: absolute;
        left: -50px;
        background: #398b9e;
        color: #fff;
        height: 50px;
        top: 60px;
        width: 50px;
        font-size: 22px;
        text-align: center;
        /* line-height: 30px; */
        display: inline-block;
        padding-top: 10px;
    }

    .panelAnchor:hover {
        background: #2fa0ec;
        box-shadow: 0px 0px 5px rgba(0,0,0,0.04);
        cursor: pointer;
    }

    .overlay {
        position: fixed;
        top: 0;
        bottom: 0;
        width: 100%;
        background: rgba(0,0,0,0.5);
    }

    #accordion .Widgheading {
        cursor: pointer;
    }

    .Widgheading > a, .Widgheading > a:focus, .Widgheading > a:active {
        color: #777;
        text-decoration: none;
        font-size: 18px;
    }

    .mr8 {
        margin-right: 8px;
    }

    #pie1, #pie2, #pie3 {
        margin-top: 25px;
    }

    .pl-0 {
        padding-left: 0;
    }



    /*dashboard grid*/
    /*#griddashboard .dxgvHeader_PlasticBlue, #griddashboard .dxgvHeader_PlasticBlue table {
        color: #333 !important;
    }*/
    #griddashboard .dxgvHeader_PlasticBlue {
        cursor: pointer;
        white-space: nowrap;
        padding: 7px 6px;
        border-top: none;
        border: none;
        background: #fff;
        overflow: hidden;
        font-weight: normal;
        text-align: left;
        font-family: 'Montserrat', sans-serif !important;
    }

    /*#griddashboard_DXFooterRow {
        display: none;
    }*/

    #griddashboard.dxgvControl_PlasticBlue, #griddashboard.dxgvDisabled_PlasticBlue {
        border-top: none !important;
        border: none !important;
        font: 10px 'Montserrat', sans-serif !important;
        background-color: none !important;
        color: #000000;
        cursor: default;
    }

    #griddashboard .dxgvTable_PlasticBlue {
        background-color: none !important;
        border-width: 0;
        border-collapse: separate !important;
        overflow: hidden;
    }

    #griddashboard .dxgvFilterRow_PlasticBlue td.dxgv, #griddashboard .dxgvPagerBottomPanel_PlasticBlue {
        padding: 6px 4px;
        background: #fff;
    }

    #griddashboard .dxgvEditFormDisplayRow_PlasticBlue td.dxgv,
    #griddashboard .dxgvDataRow_PlasticBlue td.dxgv,
    #griddashboard .dxgvDataRowAlt_PlasticBlue td.dxgv,
    #griddashboard .dxgvSelectedRow_PlasticBlue td.dxgv,
    #griddashboard .dxgvFocusedRow_PlasticBlue td.dxgv {
        padding: 6px 6px 7px;
    }

    #griddashboard .dxtc-link, .dxgvHeader_PlasticBlue td {
        font-size: 12px !important;
        font-weight: 500;
    }

    .closerCss, .closeAc, .closeAcDetails, .closer, .closeAcDetails2 {
        position: absolute;
        right: 2px;
        top: 0px;
    }

    #griddashboard > tbody>tr>td>table{
        table-layout: auto !important;
    }
    .col1 {
        background: #7a9eef;
        background-image: -webkit-linear-gradient(45deg,#7a9eef,#33b3a1);
        background-image: linear-gradient(45deg,#7a9eef,#33b3a1);
        background-repeat: repeat-x;
    }

    .col2 {
        background: #F79187;
        background-image: -webkit-linear-gradient(45deg,#9b3cb7,#ff396f);
        background-image: linear-gradient(45deg,#9b3cb7,#ff396f);
        background-repeat: repeat-x;
    }

    .col3 {
        background: #7D7D7D;
        background-image: -webkit-linear-gradient(45deg,#9b3cb7,#ff396f);
        background-image: linear-gradient(45deg,#454046,#9220e8);
        background-repeat: repeat-x;
    }

    .col4 {
        background: #23C398;
        background-image: -webkit-linear-gradient(45deg,#23C398,#d0c034);
        background-image: linear-gradient(45deg,#23C398,#d0c034);
        background-repeat: repeat-x;
    }

    .col5 {
        background: #c0ca8b;
        color: #fff;
    }

    .col6 {
        background: #7d845d;
        color: #fff;
    }

    .col7 {
        background: #aed08e;
        color: #fff;
    }




    .colorType3L {
        background: #6986a5;
        color: #fff;
    }

    .colorType3D {
        background: #415B76;
        color: #fff;
    }

    .colorType4L {
        background: #B376BD;
        color: #fff;
    }

    .colorType4D {
        background: #8D5596;
        color: #fff;
    }

    .text-white {
        color: #fff;
    }

    .noShaded:before {
        display: none;
    }

    #griddashboard_DXMainTable > tbody > tr:nth-child(even) {
        background-color: #f2f2f2;
    }

    #griddashboard_DXMainTable > tbody > tr > td.dxgvHeader_PlasticBlue {
        background-color: #23c398;
        color: #fff !important;
    }


    #griddashboarddetails_DXMainTable > tbody > tr:nth-child(even) {
        background-color: #f2f2f2;
    }

    #griddashboarddetails_DXMainTable > tbody > tr > td.dxgvHeader_PlasticBlue {
        background-color: #23c398;
        color: #fff !important;
    }


    #gridsalesman_DXMainTable > tbody > tr:nth-child(even) {
        background-color: #f2f2f2;
    }

    #gridsalesman_DXMainTable > tbody > tr > td.dxgvHeader_PlasticBlue {
        background-color: #23c398 !important;
        color: #fff !important;
    }

    #gridsummarydashboard_DXMainTable > tbody > tr > td.dxgvHeader_PlasticBlue {
        background-color: #23c398;
        color: #fff !important;
    }

    .shadow {
        /*box-shadow: 0 5px 5px 0 rgba(6, 28, 54, .091) !important;*/
        box-shadow: 0px 0px 10px rgba(70, 68, 68, 0.3);
    }

    .acCont {
        padding: 4px 5px 0 5px;
        border-bottom: 1px solid #efefef;
        margin-top: 4px;
    }

        .acCont:first-child {
            margin-top: 5px;
        }

    .Actholder .text {
        margin-left: 45px;
    }

    .Actholder .text > div {
        font-size: 12px;
        font-weight: 600;
    }

    .Actholder .text > p {
        font-size: 11px;
    }

    .avatar {
        width: 3rem;
        height: 3rem;
        line-height: 2rem;
        display: inline-block;
        background: #505050 no-repeat center/cover;
        position: relative;
        text-align: center;
        color: #fff;
        font-weight: 400;
        vertical-align: bottom;
        font-size: .875rem;
        -webkit-user-select: none;
        -moz-user-select: none;
        -ms-user-select: none;
        user-select: none;
        border-radius: 50% !important;
        float: left;
    }

    .mapContainer {
        border: 4px solid #FFF;
        box-shadow: 0px 0px 10px rgba(70, 68, 68, 0.3);
    }

    .cred {
        color: red;
    }

    .cgreen {
        color: #33CC66;
    }

    .greenTrack {
        color: #034C31;
    }

    .blink_me {
        animation: blinker 1s linear infinite;
    }

    .lds-ellipsis {
        display: inline-block;
        position: relative;
        width: 64px;
        height: 25px;
    }

    .lds-ellipsis div {
        position: absolute;
        top: 11px;
        width: 6px;
        height: 6px;
        border-radius: 50%;
        background: #97e292;
        animation-timing-function: cubic-bezier(0, 1, 1, 0);
    }

    .lds-ellipsis div:nth-child(1) {
        left: 6px;
        animation: lds-ellipsis1 0.6s infinite;
    }

    .lds-ellipsis div:nth-child(2) {
        left: 6px;
        animation: lds-ellipsis2 0.6s infinite;
    }

    .lds-ellipsis div:nth-child(3) {
        left: 26px;
        animation: lds-ellipsis2 0.6s infinite;
    }

    .lds-ellipsis div:nth-child(4) {
        left: 45px;
        animation: lds-ellipsis3 0.6s infinite;
    }

    .text-bold {
        font-weight: bold;
    }

    .min115 {
        min-height: 115px;
    }

    @@keyframes blinker {
        50% {
            opacity: 0;
        }
    }

    .mtop20 {
        margin-top: 20px;
    }

    .dxgvFooter_PlasticBlue {
        display: none;
    }

    #directions_panel {
        background-color: #FFFFFF;
        max-height: 394px;
        overflow-y: auto;
        padding: 15px;
    }

    .pdleft0 {
        padding-left: 0;
    }

    .pdright0 {
        padding-right: 0;
    }

    .dxgvHeader_PlasticBlue {
        border: none !important;
        background: #4dcba0 !important;
    }

    .btn-excel {
        float: right;
        margin-right: 38px;
        margin-top: -4px;
    }

    .btn-prime {
        float: right;
        margin-right: 20px;
        margin-top: -4px;
        background: #6ac270;
        color: #fff;
        font-size: 14px;
        padding: 4px 10px;
        display: none;
    }

        .btn-prime:hover {
            background: #5db362;
        }

    .clstyle {
        background: #f73e2c;
        width: 25px;
        height: 25px;
        text-align: center;
        color: #fff;
        border-radius: 50%;
        line-height: 24px;
        top: -5px;
        -webkit-transition: all 0.3s ease-in;
        transition: all 0.3s ease-in;
        cursor: pointer;
        font-size: 15px;
    }

        .clstyle:hover {
            background: #e63827;
            transform: scale(1.1);
        }

    .maxisize {
        max-width: 380px;
    }

    #directionname {
        font-size: 14px;
        background: #fff;
        padding: 8px 15px 0 15px;
        margin: 0 !important;
    }

    .mapShowlist {
        float: right;
        font-size: 14px;
    }

    .separate-left {
        border-left: 1px solid #ccc;
    }

    #gridsummarydashboard, #griddashboard {
        width: 100% !important;
    }
    .multiselect-container.dropdown-menu {
        right:0 !important;
        left:auto;
        min-width:340px;
    }
    .multiselect-container > li > a {
        padding: 5px 0;
    }
        .multiselect-container > li > a input[type="checkbox"]{
            transform:translateY(3px)
        } .multiselect-container > li.active > a {
            background: transparent !important;
            color: #333;
        }
        .multiselect-container > li > a:hover, .multiselect-container > li.active > a:hover {
            background: #f3f3f3 !important;
            color: #333 !important;
            padding: 5px 0;
        }
    .multiselect-item.multiselect-filter {
            padding-bottom: 6px;
            border-bottom: 1px solid #ccc;
    }
    .multiselect-item.multiselect-filter .form-control.multiselect-search {
        height: 32px;
    }
        .multiselect-item.multiselect-filter .input-group-addon {
            background: #41a8f1;
            color: #fff;
            border-color: #41a8f1;
        }
    .input-group-btn .multiselect-clear-filter {
        background: #f98181;
        color: #fffefe;
        /* box-shadow: none; */
        text-shadow: none;
        border-color: #f96a6a;
        padding: 5px 10px 4px 10px;
    }
    .multiselect.dropdown-toggle {
        background: #fff;
        border-radius: 0;
    }


    /*i con tab*/
    .iconicTabs {
        padding-bottom: 13px;
        border-bottom: 1px dashed #ccc;
        padding-top: 11px;
        margin-bottom:0;
        padding-left: 0;
    }
        .iconicTabs li {
            display: inline-block;
            list-style-type:none;
            position:relative;
        }
            .iconicTabs li > .tabitems {
                text-align: center;
                vertical-align: middle;
                margin-right: 15px;
                background: #e3e7e8;
                border: 1px solid #ccc;
                padding: 0 10px 5px 10px;
                border-radius: 5px;
                padding: 10px 25px;
                color: #8a8a8a;
            }
            .iconicTabs li.active > .tabitems {
                background: #272b3c;
                color: #fff;
                border-color: #272b3c;
            }
        .iconicTabsWrap > .tab-content {
            background: #fff;
            padding: 25px 20px;
        }
    .iconicTabs > .tabitems .tabText{
        font-weight: 500;
        font-family: 'Montserrat', sans-serif !important;
    }
    .iconicTabs li.active > a:after {
        content: '';
        width: 0;
        height: 0;
        border-left: 10px solid transparent;
        border-right: 10px solid transparent;
        border-top: 10px solid #272b3c;
        position: absolute;
        bottom: -19px;
        left: 50%;
        transform: translateX(-70%);
    }

        .iconicTabs li > a:focus {
            text-decoration:none;
        }




        /*svg tabs */
    /*****************************/
    /* Shape */
    /*****************************/

    .tabs {
        position: relative;
        overflow: hidden;
        margin: 0 auto;
        width: 100%;
        font-weight: 300;
        font-size: 1.25em;
    }

        /* Nav */
        .tabs nav {
            text-align: center;
        }

            .tabs nav ul {
                position: relative;
                display: -ms-flexbox;
                display: -webkit-flex;
                display: -moz-flex;
                display: -ms-flex;
                display: flex;
                margin: 0 auto;
                padding: 0;
                /*max-width: 1200px;*/
                list-style: none;
                -ms-box-orient: horizontal;
                -ms-box-pack: center;
                -webkit-flex-flow: row wrap;
                -moz-flex-flow: row wrap;
                -ms-flex-flow: row wrap;
                flex-flow: row wrap;
                /*-webkit-justify-content: center;
                -moz-justify-content: center;
                -ms-justify-content: center;
                justify-content: center;*/
            }

                .tabs nav ul li {
                    position: relative;
                    z-index: 1;
                    display: block;
                    margin: 0;
                    text-align: center;
                    /*-webkit-flex: 1;
                    -moz-flex: 1;
                    -ms-flex: 1;
                    flex: 1;*/
                }

            .tabs nav a {
                position: relative;
                display: block;
                overflow: hidden;
                text-overflow: ellipsis;
                white-space: nowrap;
                line-height: 2.1;
            }

                .tabs nav a span {
                    vertical-align: middle;
                    font-size: 0.75em;
                }

            .tabs nav li.active a {
                color: #313233;
            }

            .tabs nav a:focus {
                outline: none;
            }

    /* Icons */
    .icon::before {
        z-index: 10;
        display: inline-block;
        margin: 0 0.4em 0 0;
        vertical-align: middle;
        text-transform: none;
        font-weight: normal;
        font-variant: normal;
        font-size: 1.3em;
        font-family: 'stroke7pixeden';
        line-height: 1;
        speak: none;
        -webkit-backface-visibility: hidden;
        -webkit-font-smoothing: antialiased;
        -moz-osx-font-smoothing: grayscale;
    }

    .icon-upload::before {
        content: "\e68a";
    }

    .icon-tools::before {
        content: "\e60a";
    }

    .icon-plane::before {
        content: "\e625";
    }

    .icon-joy::before {
        content: "\e6a4";
    }

    .icon-plug::before {
        content: "\e69a";
    }

    .icon-home::before {
        content: "\e648";
    }

    .icon-gift::before {
        content: "\e652";
    }

    .icon-display::before {
        content: "\e65e";
    }

    .icon-date::before {
        content: "\e660";
    }

    .icon-config::before {
        content: "\e666";
    }

    .icon-coffee::before {
        content: "\e669";
    }

    .icon-camera::before {
        content: "\e66f";
    }

    .icon-box::before {
        content: "\e674";
    }

    /* Content */
    .content-wrap {
        position: relative;
    }

        .content-wrap section {
            display: none;
            margin: 0 auto;
            padding: 1em;
            max-width: 1200px;
            text-align: center;
        }

            .content-wrap section.content-current {
                display: block;
            }

            .content-wrap section p {
                margin: 0;
                padding: 0.75em 0;
                color: rgba(40,44,42,0.05);
                font-weight: 900;
                font-size: 4em;
                line-height: 1;
            }

    /* Fallback */
    .no-js .content-wrap section {
        display: block;
        padding-bottom: 2em;
        border-bottom: 1px solid rgba(255,255,255,0.6);
    }

    .no-flexbox nav ul {
        display: block;
    }

        .no-flexbox nav ul li {
            min-width: 15%;
            display: inline-block;
        }









        .tabs-style-shape nav ul li {
            margin: 0 3em;
        }

            .tabs-style-shape nav ul li:first-child {
                margin-left: 0;
            }

            .tabs-style-shape nav ul li.active {
                z-index: 100;
            }

        .tabs-style-shape nav li a {
            overflow: visible;
            margin: 0 -3em 0 0;
            padding: 0;
            color: #fff;
            font-weight: 500;
        }

        .tabs-style-shape nav li:first-child a span {
            padding-left: 2em;
            border-radius: 30px 0 0 0;
        }

        .tabs-style-shape nav li:last-child a span {
            padding-right: 2em;
            border-radius: 0 30px 0 0;
        }

        .tabs-style-shape nav li a svg {
            position: absolute;
            left: 100%;
            margin: 0;
            width: 3em;
            height: 100%;
            fill: #bdc2c9;
        }

            .tabs-style-shape nav li a svg:nth-child(2),
            .tabs-style-shape nav li:last-child a svg {
                right: 100%;
                left: auto;
                -webkit-transform: scale3d(-1,1,1);
                transform: scale3d(-1,1,1);
            }

    .tabs-style-shape nav li a span {
        display: block;
        overflow: hidden;
        padding: 0.32em 0;
        background-color: #bdc2c9;
        text-overflow: ellipsis;
        white-space: nowrap;
        font-size: 16px;
    }

        .tabs-style-shape nav li a:hover span {
            background-color: #2CC185;
        }

        .tabs-style-shape nav li a:hover svg {
            fill: #2CC185;
        }

        /* Make only shape clickable */
        .tabs-style-shape nav li a svg {
            pointer-events: none;
        }

            .tabs-style-shape nav li a svg use {
                pointer-events: auto;
            }

        .tabs-style-shape nav li.active a span,
        .tabs-style-shape nav li.active a svg {
            -webkit-transition: none;
            transition: none;
        }

        .tabs-style-shape nav li.active a span {
            background: #fff;
        }

        .tabs-style-shape nav li.active a svg {
            fill: #fff;
        }

        .tabs-style-shape .content-wrap {
            background: #fff;
        }
    .tabs-style-shape .tab-content {
        background:#fff;
        padding:20px;
    }

    .only-bordered {
        border: 1px solid #ccc;
        margin-top: 10px;
        margin-bottom: 0;
    }

        .only-bordered > tbody > tr > td {
            border-top: none;
            padding: 2px 8px;
            font-size: 14px;
        }

    #epTable .progress {
        margin: 3px 0;
        height: 10px;
    }

    .ptb0 {
        padding-top: 0 !important;
    }


    .cl3rd1 {
        background: #7bb1a2;
        background-image: -webkit-linear-gradient(45deg,#2379c3,#d07534);
        background-image: linear-gradient(45deg,#2379c3,#d07534);
        background-repeat: repeat-x;
    }
    .cl3rd2 {
        background: #d0a13f;
        background-image: -webkit-linear-gradient(45deg,#cdce80,#d09934);
        background-image: linear-gradient(45deg,#cdce80,#d09934);
        background-repeat: repeat-x;
    }
    .cl3rd3 {
        background: #967dd1;
        background-image: -webkit-linear-gradient(45deg,#c580ce,#3464d0);
        background-image: linear-gradient(45deg,#c580ce,#3464d0);
        background-repeat: repeat-x;
    }
    .cl3rd4 {
        background: #82b7a9;
        background-image: -webkit-linear-gradient(45deg,#7fc18a,#3464d0);
        background-image: linear-gradient(45deg,#7fc18a,#3464d0);
        background-repeat: repeat-x;
    }

    .cl2rd1 {
        background: #8238a3;
        background-image: -webkit-linear-gradient(45deg,#c3237c,#62219e);
        background-image: linear-gradient(45deg,#c3237c,#62219e);
        background-repeat: repeat-x;
    }
    .cl2rd2 {
        background: #5f7de5;
        background-image: -webkit-linear-gradient(45deg,#164bc3,#8d9dfb);
        background-image: linear-gradient(45deg,#164bc3,#8d9dfb);
        background-repeat: repeat-x;
    }
    .cl2rd3 {
        background: #3cb2aa;
        background-image: -webkit-linear-gradient(45deg,#50d0ca,#2b988f);
        background-image: linear-gradient(45deg,#50d0ca,#2b988f);
        background-repeat: repeat-x;
    }
    .cl2rd4 {
        background: #a97fc3;
        background-image: -webkit-linear-gradient(45deg,#816cc1,#ff98be);
        background-image: linear-gradient(45deg,#816cc1,#ff98be);
        background-repeat: repeat-x;
    }


    .cl2rd5 {
        background: #8238a3;
        background-image: -webkit-linear-gradient(45deg,#c3237c,#62219e);
        background-image: linear-gradient(45deg,#e8f906d9,#ff5bae);
        background-repeat: repeat-x;
    }
    .cl2rd6 {
        background: #7bb1a2;
        background-image: -webkit-linear-gradient(45deg,#52d7ff,#34d0b3);
        background-image: linear-gradient(45deg,#52d7ff,#34d0b3);
        background-repeat: repeat-x;
    }
    .cl2rd7 {
        background: #82b7a9;
        background-image: -webkit-linear-gradient(45deg,#7fc18a,#aba1a1);
        background-image: linear-gradient(45deg,#7fc18a,#aba1a1);
        background-repeat: repeat-x;
    }
    .resposive-image {
        max-width:100%;
    }

    .leadH {
        font-size: 18px;
        color: #7d7d7d;
        border-bottom: 1px solid #ccc;
        padding-bottom: 15px;
        margin-bottom:15px;
    }
    .card {
        position: relative;
        display: -ms-flexbox;
        display: flex;
        -ms-flex-direction: column;
        flex-direction: column;
        min-width: 0;
        word-wrap: break-word;
        background-color: #fff;
        background-clip: border-box;
        border: 1px solid rgba(0,0,0,.125);
        border-radius: 9px;
        margin-top: 1.25rem;
        box-shadow: 0 1px 2px rgba(0,0,0,.05);
    }
    .leaderboardHeader  {
        padding: .9375rem 1.25rem;
        margin-bottom: 0;
        background-color: rgba(0,0,0,.02);
        border-bottom: 1px solid rgba(0,0,0,.125);
    }
    .card-content {
        flex: 1 1 auto;
        padding: 1.25rem;
    }
    .full {
        width:100%
    }
    .mw90 {
        height: auto;
        width: 35px;
    }
    .leaderTbl > thead > tr > th {
        font-size: 13px;
        padding: 1.75rem 1.25rem;
        vertical-align: top;
        background: rgba(0, 0, 0, 0.05);
        border-bottom: 1px solid #afabab;
    }
    .leaderTbl > tbody > tr > td {
        padding: .75rem 1.25rem;
        vertical-align: middle;
        border-top: 1px solid #ddd;
        font-size:13px;
    }
    .nme {
        font-weight: 500;
        color: #635e5e;
    }

     ::-webkit-scrollbar-track {
        -webkit-box-shadow: inset 0 0 3px rgba(0,0,0,0.3);
        background-color: #F5F5F5;
    }

     ::-webkit-scrollbar {
        width: 5px;
        background-color: #F5F5F5;
    }

     ::-webkit-scrollbar-thumb {
        background-color: #FFFFFF;
        border: 1px solid #F9F9F9;
    }
    table.dataTable {
        margin-top:0 !important;
    }



        .os-tabs-controls .nav-pills {
            -webkit-box-align: center;
            -ms-flex-align: center;
            align-items: center;
        }

        .os-tabs-controls .nav-tabs {
            -webkit-box-flex: 1;
            -ms-flex: 1;
            flex: 1;
        }

        .os-tabs-controls.os-tabs-complex .nav-tabs {
            -webkit-box-align: stretch;
            -ms-flex-align: stretch;
            align-items: stretch;
        }

        .os-tabs-controls.os-tabs-complex .nav-item {
            white-space: nowrap;
            margin-right: 0px;
        }

            .os-tabs-controls.os-tabs-complex .nav-item.nav-actions {
                margin-left: auto;
            }

            .os-tabs-controls.os-tabs-complex .nav-item .btn, .os-tabs-controls.os-tabs-complex .nav-item .all-wrapper .fc-button, .all-wrapper .os-tabs-controls.os-tabs-complex .nav-item .fc-button {
                margin-left: 10px;
            }

            .os-tabs-controls.os-tabs-complex .nav-item .nav-link {
                color: #3E4B5B;
                opacity: 0.6;
                border-right: 1px solid rgba(0, 0, 0, 0.05);
                border-radius: 0px;
                padding: 8px 20px;
                margin-right: 0px;
            }

                .os-tabs-controls.os-tabs-complex .nav-item .nav-link span {
                    display: inline-block;
                    vertical-align: middle;
                    font-size: 0.9rem;
                }

                    .os-tabs-controls.os-tabs-complex .nav-item .nav-link span.badge {
                        font-size: 0.72rem;
                        padding: 3px 5px;
                    }

                        .os-tabs-controls.os-tabs-complex .nav-item .nav-link span.badge span {
                            font-size: 0.72rem;
                        }

                .os-tabs-controls.os-tabs-complex .nav-item .nav-link.active {
                    opacity: 1;
                }

                    .os-tabs-controls.os-tabs-complex .nav-item .nav-link.active .tab-label {
                        font-size: 1.26rem;
                        line-height: 1.25;
                    }

                .os-tabs-controls.os-tabs-complex .nav-item .nav-link .tab-label {
                    display: inline-block;
                    margin-right: 10px;
                    text-transform: none;
                    letter-spacing: normal;
                    font-size: 1.08rem;
                    vertical-align: middle;
                    -webkit-transition: all 0.2s ease;
                    transition: all 0.2s ease;
                }

                .os-tabs-controls.os-tabs-complex .nav-item .nav-link .tab-value {
                    display: inline-block;
                    color: #047bf8;
                    margin-right: 10px;
                    text-transform: none;
                    letter-spacing: normal;
                    vertical-align: middle;
                }

                .os-tabs-controls.os-tabs-complex .nav-item .nav-link .badge {
                    display: inline-block;
                    vertical-align: middle;
                    font-size: 0.72rem;
                    letter-spacing: normal;
                }

                    .os-tabs-controls.os-tabs-complex .nav-item .nav-link .badge i.os-icon {
                        color: #fff;
                        font-size: 12px;
                        display: inline-block;
                        vertical-align: middle;
                        margin-bottom: 0px;
                        margin-right: 3px;
                    }

                    .os-tabs-controls.os-tabs-complex .nav-item .nav-link .badge span {
                        display: inline-block;
                        vertical-align: middle;
                        font-size: 0.72rem;
                    }
    .nav-pills .nav-link {
        border-radius: 4px;
        color: rgba(0, 0, 0, 0.4);
        font-size: 12px;
    }
    .nav.smaller.nav-pills .nav-link {
        padding: 0.2em 1.1em;
    }
    .nav-pills .nav-link.active, .nav-pills .nav-item.show .nav-link {
        color: #fff;
        cursor: default;
        background-color: #047bf8;
    }
    .os-tabs-controls .nav-pills .nav-link:hover {
        color:#fff;
    }

    .leaderTbl .label {
        font-size: 88%;
    }
    .leaderTbl > tbody > tr {
        cursor: pointer;
    } .leaderTbl > tbody > tr.selected {
        background: #e7ffe7;

    }
    .statsWrap; {
        position: relative;
    }

    .statsWrap .statsHd {
        display: inline-block;

        padding: 4px 10px;
        font-size: 14px;
        font-weight: 400;
        color: #fff;
    }

    .statsList {
        list-style-type: none;

        padding: 0;
        margin-left: 30px;
        margin-bottom:0;
        padding:15px 0;
    }
        .statsList.cc1 {
            border-left: 1px solid #75b738;
        }
        .statsList.cc2 {
            border-left: 1px solid #f3cc40;
        }
    .statsList> li {
        padding:5px 5px 5px 20px;
        position:relative;
    }

        .statsList > li:before {
            content: '';
            width: 20px;
            height: 20px;
            border-radius: 50%;
            position: absolute;
            background: #44d4d4;
            border: 4px solid #fff;
            top: 10px;
            left: -11px;
            z-index: 1;
        }
        .statsList > li:after {
            content: '';
            position: absolute;
            width: 300px;
            height: 1px;
            background: #ccc;
            top: 20px;
            z-index: 1;
        }

        .statsList .blockage {
            font-size: 13px;
            background: #f7f7f7;
            padding: 6px 10px;
            border: 1px solid #c1bdbd;
            border-radius: 14px;
            display: inline-block;
        }
    .blckNum {
        position: absolute;
        right: 20px;
        width: 35px;
        height: 35px;
        background: #bbdebb;
        text-align: center;
        /* color: #fff; */
        font-weight: 500;
        line-height: 34px;
        border-radius: 50%;
        z-index: 5;
        top: 2px;
    }

    .sl1, .statsList.cc1 > li:before {
        background: #75b738;
    }
    .sl2, .statsList.cc2 > li:before {
        background: #f3cc40;
    }
    .chartbox {
        border: 1px solid #c7c4c4;
        border-radius: 17px;
        margin-top: 10px;
        box-shadow: 0px 0px 15px rgba(0,0,0,0.05);
    }
</style>

    <script>
        (function ($) {
            $(window).on("load", function () {
                $("#mscroll").DataTable({
                    "paging": false,
                    "searching": false,
                    "info": false,
                    "scrollY": "340px",
                    "scrollCollapse": true,
                });
            });


        })(jQuery);
        $(document).ready(function () {

            // table data
            var tb1 = '';
            tb1 += '<tr class="selected"><td><img src="~/assests/images/bt3.jpg" class="img-circle mw90" /></td><td><span class="nme">Susanta Roy</span></td><td><span class="label label-success">1</span></td><td>90</td></tr>';
            tb1 += '<tr><td><img src="~/assests/images/bt5.jpg" class="img-circle mw90" /></td><td><span class="nme">Arun Mahato</span></td><td><span class="label label-info">2</span></td><td>82</td></tr>';
            tb1 += '<tr><td><img src="~/assests/images/bt2.jpg" class="img-circle mw90" /></td><td><span class="nme">Rajbir Sharma</span></td><td><span class="label label-warning">3</span></td><td>76</td></tr>';
            tb1 += '<tr><td><img src="~/assests/images/bt1.jpg" class="img-circle mw90" /></td><td><span class="nme">Bijay Thakur</span></td><td><span class="label label-primary">4</span></td><td>68</td></tr>';
            tb1 += '<tr><td><img src="~/assests/images/bt9.jpg" class="img-circle mw90" /></td><td><span class="nme">Ruby Sil</span></td><td>5</td><td>68</td></tr>';
            tb1 += '<tr><td><img src="~/assests/images/bt6.jpg" class="img-circle mw90" /> </td><td><span class="nme">Goutam Nandi</span></td><td>6</td><td>65</td></tr>';
            tb1 += '<tr><td><img src="~/assests/images/bt3.jpg" class="img-circle mw90" /></td><td><span class="nme">R Mohan</span></td><td>7</td><td>63</td></tr>';
            tb1 += '<tr><td><img src="~/assests/images/bt7.jpg" class="img-circle mw90" /></td><td><span class="nme">K Benugopal</span></td><td>8</td><td>57</td></tr>';
            //
            var tb1 = '';
            tb1 += '<tr class="selected"><td><img src="~/assests/images/bt1.jpg" class="img-circle mw90" /></td><td><span class="nme">Mohit Roy</span></td><td><span class="label label-success">1</span></td><td>90</td></tr>';
            tb1 += '<tr><td><img src="~/assests/images/bt2.jpg" class="img-circle mw90" /></td><td><span class="nme">Gurucharan Mahato</span></td><td><span class="label label-info">2</span></td><td>82</td></tr>';
            tb1 += '<tr><td><img src="~/assests/images/bt3.jpg" class="img-circle mw90" /></td><td><span class="nme">Anand Sharma</span></td><td><span class="label label-warning">3</span></td><td>76</td></tr>';
            tb1 += '<tr><td><img src="~/assests/images/bt4.jpg" class="img-circle mw90" /></td><td><span class="nme">Rohit Thakur</span></td><td><span class="label label-primary">4</span></td><td>68</td></tr>';
            tb1 += '<tr><td><img src="~/assests/images/bt8.jpg" class="img-circle mw90" /></td><td><span class="nme">Ruby Sil</span></td><td>5</td><td>68</td></tr>';
            tb1 += '<tr><td><img src="~/assests/images/bt5.jpg" class="img-circle mw90" /> </td><td><span class="nme">Goutam Nandi</span></td><td>6</td><td>65</td></tr>';
            tb1 += '<tr><td><img src="~/assests/images/bt6.jpg" class="img-circle mw90" /></td><td><span class="nme">R Mohan</span></td><td>7</td><td>63</td></tr>';
            tb1 += '<tr><td><img src="~/assests/images/bt7.jpg" class="img-circle mw90" /></td><td><span class="nme">K Benugopal</span></td><td>8</td><td>57</td></tr>';

















            var js1 = '<div class="statsHd sl1">24.4.2019</div><ul class="statsList cc1"><li><span class="blockage">Savings Max Account</span> <span class="blckNum">35</span></li><li><span class="blockage">Senior Citizen Account</span><span class="blckNum">40</span></li><li><span class="blockage">Two Wheeler Loan</span><span class="blckNum">15</span></li></ul><div class="statsHd sl2">23.4.2019</div><ul class="statsList cc2"><li><span class="blockage">Credit Cards</span> <span class="blckNum">26</span></li><li><span class="blockage">Health & Accident Insurance</span><span class="blckNum">35</span></li><li><span class="blockage">Personal Loan</span><span class="blckNum">19</span></li></ul>';

            var js2 = '<div class="statsHd sl1">24.4.2019</div><ul class="statsList cc1"><li><span class="blockage">Savings Max Account</span> <span class="blckNum">38</span></li><li><span class="blockage">Senior Citizen Account</span><span class="blckNum">12</span></li><li><span class="blockage">Two Wheeler Loan</span><span class="blckNum">32</span></li></ul><div class="statsHd sl2">23.4.2019</div><ul class="statsList cc2"><li><span class="blockage">Credit Cards</span> <span class="blckNum">26</span></li><li><span class="blockage">Health & Accident Insurance</span><span class="blckNum">35</span></li><li><span class="blockage">Personal Loan</span><span class="blckNum">19</span></li></ul>';

            var js3 = '<div class="statsHd sl1">24.4.2019</div><ul class="statsList cc1"><li><span class="blockage">Savings Max Account</span> <span class="blckNum">35</span></li><li><span class="blockage">Senior Citizen Account</span><span class="blckNum">30</span></li><li><span class="blockage">Two Wheeler Loan</span><span class="blckNum">11</span></li></ul><div class="statsHd sl2">23.4.2019</div><ul class="statsList cc2"><li><span class="blockage">Credit Cards</span> <span class="blckNum">26</span></li><li><span class="blockage">Health & Accident Insurance</span><span class="blckNum">35</span></li><li><span class="blockage">Personal Loan</span><span class="blckNum">19</span></li></ul>';

            $('.leaderTbl > tbody > tr').click(function () {

                var rowCount = $(this).index() + 1;

                $('.leaderTbl>tbody>tr').removeClass('selected');
                $(this).addClass('selected');

                if (rowCount == 1) {
                    $('.statsWrap').html('');
                    $('.statsWrap').html(js1);
                } else if (rowCount == 2) {
                    $('.statsWrap').html('');
                    $('.statsWrap').html(js2);
                } else if (rowCount == 3) {
                    $('.statsWrap').html('');
                    $('.statsWrap').html(js3);
                } else if (rowCount == 4) {
                    $('.statsWrap').html('');
                    $('.statsWrap').html(js2);
                } else if (rowCount == 5) {
                    $('.statsWrap').html('');
                    $('.statsWrap').html(js1);
                } else if (rowCount == 6) {
                    $('.statsWrap').html('');
                    $('.statsWrap').html(js3);
                } else if (rowCount == 7) {
                    $('.statsWrap').html('');
                    $('.statsWrap').html(js1);
                } else if (rowCount == 8) {
                    $('.statsWrap').html('');
                    $('.statsWrap').html(js2);
                } else if (rowCount == 9) {
                    $('.statsWrap').html('');
                    $('.statsWrap').html(js3);
                } else if (rowCount == 10) {
                    $('.statsWrap').html('');
                    $('.statsWrap').html(js2);
                } else if (rowCount == 11) {
                    $('.statsWrap').html('');
                    $('.statsWrap').html(js1);
                }
            });



            var options = {
                chart: {
                    width: 480,
                    height: 350,
                    type: 'pie',
                },
                labels: ['LEADS', 'VISITS', 'CLOSED'],
                fill: {
                    colors: ['#01b8aa', '#374649', '#fd625e']
                },
                colors: ['#01b8aa', '#374649', '#fd625e'],
                label: {
                    position: 'bottom',
                },
                series: [250, 130, 24],
                responsive: [{
                    breakpoint: 480,
                    options: {
                        chart: {
                            width: 200
                        },
                        legend: {
                            position: 'bottom'
                        }
                    }
                }]
            }

            var chart = new ApexCharts(
                document.querySelector("#chartpie"),
                options
            );

            chart.render();

            var options2 = {
                chart: {
                    type: 'donut',
                    width: 480,
                    height: 350
                },
                labels: ['Qualify Lead', 'Develop', 'Propose', 'Close'],
                fill: {
                    colors: ['#01b8aa', '#374649', '#f2c80f', '#fd625e']
                },
                colors: ['#01b8aa', '#374649', '#f2c80f', '#fd625e'],
                label: {
                    position: 'bottom',
                },
                series: [250, 100, 30, 24],
                responsive: [{
                    breakpoint: 480,
                    options: {
                        chart: {
                            width: 200
                        },
                        legend: {
                            position: 'bottom'
                        }
                    }
                }]
            }

            var chart2 = new ApexCharts(
                 document.querySelector("#chartpie2"),
                 options2
             );

            chart2.render();




            //var options = {
            //    chart: {
            //        height: 350,
            //        type: 'bar',
            //    },
            //    plotOptions: {
            //        bar: {
            //            horizontal: false,
            //        }
            //    },
            //    labels: ['Bankura', 'Bardwan', 'Darjeeling', 'Kolkata', 'Midnapore'],
            //    markers: {
            //        colors: ['#33b2df', '#546E7A', '#d4526e', '#13d8aa', '#A5978B', '#2b908f', '#f9a3a4', '#90ee7e', '#f48024']
            //    },
            //    dataLabels: {
            //        enabled: true
            //    },
            //    series: [{
            //        data: [2, 5, 9, 10, 12, 15, 20, 20, 35]
            //    }],
            //    xaxis: {
            //        categories: ['National Pension System', 'Regalia', 'Loan on Credit Card', 'Life Insurance', 'Education Loan', 'Personal Loan', 'Demat Account', '2 in 1 Account ', 'Two Wheeler Loans'],
            //    }
            //}

            //var chart = new ApexCharts(
            //     document.querySelector("#productPerformance112"),
            //     options
            // );

            //chart.render();


            $('.shLeader').click(function () {
                $('#shLeaderView').css({ 'visibility': 'visible' });
            });

            $('#tbsSales').click(function () {
                chart2.refresh();
            });

        });
</script>
    <script>
        $(document).ready(function () {
            $('body').on('click', '.panelAnchor', function () {
                $('#panelRight').toggleClass('open');
                //if( $('#panelRight').hasClass('open') ){
                //    $('body').append('<div class="overlay"></div>');
                //}else{
                //    $('.overlay').remove();
                //}
            });
            var options = {
                chart: {
                    height: 350,
                    type: 'line',
                },
                series: [{
                    name: 'Website Blog',
                    type: 'bar',
                    columnWidth: '50%',
                    data: [440, 505, 414, 671, 227, 413, 201, 352, 752, 320, 257, 160]
                },
                {
                    name: 'Website 2',
                    type: 'bar',
                    columnWidth: '50%',
                    data: [440, 505, 414, 671, 227, 413, 201, 352, 752, 320, 257, 160]
                },
                {
                    name: 'Social Media',
                    type: 'line',
                    data: [250, 242, 335, 27, 443, 422, 117, 331, 222, 422, 212, 116]
                }],
                stroke: {
                    width: [0, 0, 4]
                },
                title: {
                    text: 'Traffic Sources'
                },
                // labels: ["Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"],
                labels: ['01 Jan 2001', '02 Jan 2001', '03 Jan 2001', '04 Jan 2001', '05 Jan 2001', '06 Jan 2001', '07 Jan 2001', '08 Jan 2001', '09 Jan 2001', '10 Jan 2001', '11 Jan 2001', '12 Jan 2001'],
                xaxis: {
                    type: 'datetime'
                },
                yaxis: [{
                    title: {
                        text: 'Website Blog',
                    },

                },
                //{
                //    opposite: true,
                //}
                ]

            }

            var chart = new ApexCharts(
              document.querySelector("#mixedChart"),
              options
            );

            chart.render();
        });


        $(function () {
            $("#tbsalesman").click(function () {
                dbgridsalesman.Refresh();
                dbgridsalesman.Refresh();
            });
            $("#tbdistributor").click(function () {
                dbgriddistributor.Refresh();
                dbgriddistributor.Refresh();
            });
        });

</script>

</head>
<body>
    <form id="form1" runat="server">
        <%--leaderboard--%>
        <div class="form_main ">

            <section class="clearfix col-md-12">
                <div id="shLeaderView">
                                    <h1 class="leadH">Leaderboard</h1>

                                    <div class="clearfix ">
                                        <div class="row">
                                            <div class="col-md-6">
                                                <div class="card">
                                                    <div class="leaderboardHeader">
                                                        Rank wise
                                                        <div class="pull-right os-tabs-controls">
                                                            <ul class="nav nav-pills smaller d-none d-lg-flex">
                                                                <li class="nav-item active">
                                                                    <a class="nav-link  " data-toggle="tab" href="#"> Today</a>
                                                                </li>
                                                                <li class="nav-item">
                                                                    <a class="nav-link" data-toggle="tab" href="#">This Week</a>
                                                                </li>
                                                                <li class="nav-item">
                                                                    <a class="nav-link" data-toggle="tab" href="#"> This Month</a>
                                                                </li>
                                                            </ul>
                                                            <div></div>
                                                        </div>
                                                    </div>
                                                    <div class="">
                                                        <table class="full leaderTbl" id="mscroll">
                                                            <thead>
                                                                <tr>
                                                                    <th style="width:50px"></th>
                                                                    <th>Name</th>
                                                                    <th>Rank</th>
                                                                    <th>Points</th>
                                                                </tr>
                                                            </thead>
                                                            <tbody>
                                                                <tr class="selected">
                                                                    <td><img src="/assests/images/AV1.png" class="img-circle mw90" /></td>
                                                                    <td><span class="nme">Srijit Dasgupta</span></td>
                                                                    <td><span class="label label-success">1</span></td>
                                                                    <td>90</td>
                                                                </tr>
                                                                <tr>
                                                                    <td><img src="/assests/images/AV2.png" class="img-circle mw90" /></td>
                                                                    <td><span class="nme">Tamal Sen</span></td>
                                                                    <td><span class="label label-info">2</span></td>
                                                                    <td>82</td>
                                                                </tr>
                                                                <tr>
                                                                    <td><img src="/assests/images/AV3.png" class="img-circle mw90" /></td>
                                                                    <td><span class="nme">Arup Das</span></td>
                                                                    <td><span class="label label-warning">3</span></td>
                                                                    <td>76</td>
                                                                </tr>
                                                                <tr>
                                                                    <td><img src="/assests/images/AV4.png" class="img-circle mw90" /></td>
                                                                    <td><span class="nme">Kamal Roy</span></td>
                                                                    <td><span class="label label-primary">4</span></td>
                                                                    <td>68</td>
                                                                </tr>
                                                                <tr>
                                                                    <td><img src="/assests/images/AV5.png" class="img-circle mw90" /></td>
                                                                    <td><span class="nme">Subhajit Nath</span></td>
                                                                    <td>5</td>
                                                                    <td>68</td>
                                                                </tr>
                                                                <tr>
                                                                    <td><img src="/assests/images/AV6.png" class="img-circle mw90" /> </td>
                                                                    <td><span class="nme">Subhajit Sen</span></td>
                                                                    <td>6</td>
                                                                    <td>65</td>
                                                                </tr>
                                                                <tr>
                                                                    <td><img src="/assests/images/AV7.png" class="img-circle mw90" /></td>
                                                                    <td><span class="nme">Subhra Tripathi</span></td>
                                                                    <td>7</td>
                                                                    <td>63</td>
                                                                </tr>
                                                                       
                                                                        
                                                            </tbody>

                                                        </table>
                                                    </div>

                                                </div>
                                            </div>

                                            <div class="col-md-6">
                                                <div class="card">
                                                    <div class="leaderboardHeader">
                                                        Data Breakup
                                                        <div class="pull-right os-tabs-controls">
                                                            <ul class="nav nav-pills smaller d-none d-lg-flex">
                                                                <li class="nav-item active">
                                                                    <a class="nav-link  " data-toggle="tab" href="#"> 500 Point</a>
                                                                </li>
                                                                <li class="nav-item">
                                                                    <a class="nav-link" data-toggle="tab" href="#">Previous Data</a>
                                                                </li>

                                                            </ul>
                                                            <div></div>
                                                        </div>
                                                    </div>
                                                    <div class="card-content">
                                                        <div class="statsWrap">
                                                            <div class="statsHd sl1">24.4.2019</div>
                                                            <ul class="statsList cc1">
                                                                <li><span class="blockage">Savings Max Account</span> <span class="blckNum">35</span></li>
                                                                <li><span class="blockage">Senior Citizen Account</span><span class="blckNum">40</span></li>
                                                                <li><span class="blockage">Two Wheeler Loan</span><span class="blckNum">15</span></li>
                                                            </ul>
                                                            <div class="statsHd sl2">23.4.2019</div>
                                                            <ul class="statsList cc2">
                                                                <li><span class="blockage">Credit Cards</span> <span class="blckNum">30</span></li>
                                                                <li><span class="blockage">Health & Accident Insurance</span><span class="blckNum">20</span></li>
                                                                <li><span class="blockage">Personal Loan</span><span class="blckNum">35</span></li>

                                                            </ul>
                                                        </div>
                                                    </div>

                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
            </section>
            <div class="clear"></div>


        </div>
        <%--leaderboard--%>   
        
        <script>

                    function NoFileAlert() {

                        jAlert('No data for export');
                    }

                    function mapcloseclick() {
                        $("#map").addClass("hide");
                    }



                    var objData = {};
                    $(document).ready(function () {

                        //Surojit

                        $('#cmbState').multiselect({
                            includeSelectAllOption: true,
                            enableFiltering: true,
                            enableCaseInsensitiveFiltering: true,
                            onDropdownHide: function (event) {
                                GetSateValue();
                            }
                        }).multiselect('selectAll', false).multiselect('updateButtonText');
                        stateids = $('#cmbState').val();

                        var statelistcount = $('#hdnStateListCount').val();
                        if (statelistcount > 0) {
                            var GroupBy = $('#hdnGridStatewiseSummaryGroupBy').val();
                            for (var i = objsettings.length - 1; i >= 0; i--) {
                                if (objsettings[i].ID == settingsid) {
                                    objsettings.splice(i, 1);
                                }
                            }

                            if (settingsid == "1") {
                                var obj = {};
                                obj.ID = "1";
                                obj.action = "AT_WORK";
                                obj.rptype = "Summary";
                                obj.empid = "";
                                obj.stateid = stateids.join(',');// cmbState.GetValue();
                                obj.designid = "";
                                objsettings.push(obj);
                            }

                            WindowSize = $(window).width();


                            $("#lblAtWork").html("<img src='/assests/images/Spinner.gif' />");
                            $("#lblOnLeave").html("<img src='/assests/images/Spinner.gif' />");
                            $("#lblNotLoggedIn").html("<img src='/assests/images/Spinner.gif' />");
                            $("#lblTotal").html("<img src='/assests/images/Spinner.gif' />");
                            stateid = stateids.join(',');// cmbState.GetValue();
                            $("#salesmanheader").html("State wise Summary");
                            stateid = stateids.join(',');// cmbState.GetValue();
                            //GetAddress(stateid);
                            objData = {};

                            var hdnTotalEmployees = $('#hdnTotalEmployees').val();
                            var hdnAtWork = $('#hdnAtWork').val();
                            var hdnOnLeave = $('#hdnOnLeave').val();
                            var hdnNotLoggedIn = $('#hdnNotLoggedIn').val();
                            var hdnStatewiseSummary = $('#hdnStatewiseSummary').val();

                            var obj = {};
                            obj.stateid = stateid;

                            if (hdnTotalEmployees > 0 || hdnAtWork > 0 || hdnOnLeave > 0 || hdnNotLoggedIn > 0) {
                                $.ajax({
                                    type: "POST",
                                    url: "/DashboardMenu/GetDashboardData",
                                    data: JSON.stringify(obj),
                                    async: true,
                                    contentType: "application/json; charset=utf-8",
                                    dataType: "json",
                                    success: function (response) {
                                        if (hdnAtWork > 0) {
                                            $("#lblAtWork").html(response.lblAtWork);
                                        }
                                        if (hdnOnLeave > 0) {
                                            $("#lblOnLeave").html(response.lblOnLeave);
                                        }
                                        if (hdnNotLoggedIn > 0) {
                                            $("#lblNotLoggedIn").html(response.lblNotLoggedIn);
                                        }
                                        if (hdnTotalEmployees > 0) {
                                            $("#lblTotal").html(response.lblTotal);
                                        }


                                        //$("#lbltotalshop").text(response.TotalVisit);
                                        //$("#lblnewvisit").text(response.NewVisit);
                                        //$("#lblrevisit").text(response.ReVisit);

                                        //$("#lblavgvisits").text(response.AvgPerDay);
                                        //$("#lblavgduration").text(response.AvgDurationPerShop);

                                        //$("#lbltodaysale").text(response.TODAYSALES);
                                        //$("#lblavgsale").text(response.AVGSALES);
                                        //$("#lbltotalsale").text(response.TOTALSALES);




                                    },
                                    error: function (response) {
                                        jAlert("Please try again later");
                                    }
                                });
                            }
                            if (hdnStatewiseSummary > 0) {
                                gridsalesman.ClearFilter();
                                gridsalesman.Refresh();
                            }
                            //gridsalesman.Refresh();
                            // $("#gridsalesman_DXMainTable > tbody > tr > td.dxgvHeader_PlasticBlue").css({ 'background': divBGcolor });
                            //setTimeout(function () { gridsalesman.GroupBy(GroupBy); }, 2000);
                        }
                        else {
                            $('.bodymain_areastatewise').hide();
                        }

                    });

                    function gridsummarydashboardExport() {
                        var url = '@Url.Action("ExportDashboardSummaryGridView", "DashboardMenu", new { type = "_type_" })'

                        window.location.href = url.replace("_type_", 3);


                    }

                    function gridsalesmanExport() {
                        var url = '@Url.Action("ExportDashboardGridViewSalesmanDetail", "DashboardMenu", new { type = "_type_" })'
                        window.location.href = url.replace("_type_", 3);
                    }

                    function griddashboardgridviewexport() {
                        var url = '@Url.Action("ExportDashboardGridView", "DashboardMenu", new { type = "_type_" })'
                        window.location.href = url.replace("_type_", 3);
                    }

                    function griddashboardgridviewdetailsexport() {
                        var url = '@Url.Action("ExportDashboardGridViewDetails", "DashboardMenu", new { type = "_type_" })'
                        window.location.href = url.replace("_type_", 3);
                    }





                </script>
    </form>
</body>
</html>
