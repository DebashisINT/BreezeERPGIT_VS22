<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ExpenseHistory.ascx.cs" Inherits="DashBoard.DashBoard.Accounts.UserControl.ExpenseHistory" %>

<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Data.Linq" TagPrefix="dx" %>


<script>
    

    function Expensekeydown(e) {

        var OtherDetails = {}

        $.ajax({
            type: "POST",
            url: "../Service/general.asmx/TopExpenseThisMonth",
            data: JSON.stringify(OtherDetails),
            async: true,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {


            },
            error: function (response) {
                jAlert("Please try again later");
                LoadingPanel.Hide();
            }
        });



        

    }

    
</script>

<div>
    <aside class="colWraper">
        <div class="diverh">
            

        </div>

    </aside>

</div>

