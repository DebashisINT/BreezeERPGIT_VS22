<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/OMS/MasterPage/ERP.Master" 
     Inherits="ERP.OMS.Management.ToolsUtilities.management_utilities_frmrfamaster" Codebehind="frmrfamaster.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    
    
    <script language="javascript" type="text/javascript">
    function SignOff()
    {
        window.parent.SignOff();
    }
    function height()
    {
        if(document.body.scrollHeight>=500)
            window.frameElement.height = document.body.scrollHeight;
        else
            window.frameElement.height = '500px';
        window.frameElement.Width = document.body.scrollWidth;
    }
   </script>
        <script language="javascript" type="text/javascript">
            function OnGridFocusedRowChanged() {
                // Query the server for the Row ID "rfa_id" fields from the focused row 
                // The values will be returned to the OnGetRowValues() function 
                grid.GetRowValues(grid.GetFocusedRowIndex(), 'rfa_id', OnGetRowValues);
            }
            //Value array contains Row ID "rfa_id" field values returned from the server 
            function OnGetRowValues(a) {
                RowID1 = a;
            }
        
        </script>
        <script language="javascript" type="text/javascript">
             function frmOpenNewWindow_custom(location,v_height,v_weight,top,left)
               {   
                   window.open(location,"Search_Conformation_Box","height="+ v_height +",width="+ v_weight +",top="+ top +",left="+ left +",location=no,directories=no,menubar=no,toolbar=no,status=no,scrollbars=yes,resizable=no,dependent=no'");       
               } 
            function setRules123(str,obj1)
            {
               var obj = document.getElementById(str);    
               frmOpenNewWindow_custom('frmsetrule.aspx?id='+obj.value +'&Hcontrol='+obj1,'220','300','200','300');
            }
            
            function Page_Load()
            {
                document.getElementById("TdGrid").style.display = 'inline';
                document.getElementById("TdAll").style.display = 'none';
            }
            function btnAdd_Click()
            {
                document.getElementById("TdGrid").style.display = 'none';
                document.getElementById("TdAll").style.display = 'inline'; 
                document.getElementById("txt_templateno").disabled = false;
                var combo = document.getElementById("txt_templateno");
                combo.value='';
                combo = document.getElementById("txt_shortname");
                combo.value='';
                combo = document.getElementById("txt_description");
                combo.value='';                
                combo = document.getElementById("lst_targetuser1");
                combo.value='0';
                document.getElementById("lst_subtargetuser1").style.display = 'none';
                combo = document.getElementById("lst_targetuser2");
                combo.value='0';
                document.getElementById("lst_subtargetuser2").style.display = 'none';
                combo = document.getElementById("lst_targetuser3");
                combo.value='0';
                document.getElementById("lst_subtargetuser3").style.display = 'none';
                combo = document.getElementById("lst_targetuser4");
                combo.value='0';
                document.getElementById("lst_subtargetuser4").style.display = 'none';
                combo = document.getElementById("lst_targetuser5");
                combo.value='0';
                document.getElementById("lst_subtargetuser5").style.display = 'none';
                combo = document.getElementById("lst_targetuser6");
                combo.value='0';
                document.getElementById("lst_subtargetuser6").style.display = 'none';
                combo = document.getElementById("txt_hoursallowed");
                combo.value='';
                combo = document.getElementById("Hsetrule2");
                combo.value='';
                combo = document.getElementById("Hsetrule3");
                combo.value='';
                combo = document.getElementById("Hsetrule4");
                combo.value='';
                combo = document.getElementById("Hsetrule5");
                combo.value='';
                combo = document.getElementById("Hsetrule6");
                combo.value='';
                combo = document.getElementById("hdID");
                combo.value='';
            }
         
        </script>
        <script type="text/ecmascript">
            function btnCancel_Click()
            {
                document.getElementById("TdGrid").style.display = 'inline';
                document.getElementById("TdAll").style.display = 'none';
                document.location.href('frmrfamaster.aspx');
                //grid.PerformCallback();
            }
            function callDoc(obj)
            {
                var combo
                if(obj==1)
                {
                    combo = document.getElementById("lst_targetuser1");
                }
                if(obj==2)
                {
                    combo = document.getElementById("lst_targetuser2");
                }
                if(obj==3)
                {
                    combo = document.getElementById("lst_targetuser3");
                }
                if(obj==4)
                {
                    combo = document.getElementById("lst_targetuser4");
                }
                if(obj==5)
                {
                    combo = document.getElementById("lst_targetuser5");
                }
                if(obj==6)
                {
                    combo = document.getElementById("lst_targetuser6");
                }
                var data= 'Combo~' + combo.value + '~' +obj;
                CallServer(data,"");
            }
            function btnEdit_Click()
            {
                document.getElementById("TdGrid").style.display = 'none';
                document.getElementById("TdAll").style.display = 'inline'; 
                var data = 'Edit';
                data+='~'+RowID1;
                CallServer(data,"");
            }
            function btnDelete_Click()
            {
                var data = 'Delete';
                data+='~'+RowID1;
                CallServer(data,"");
                grid.PerformCallback();
                document.getElementById("TdGrid").style.display = 'inline';
                document.getElementById("TdAll").style.display = 'none'; 
            }
            function btnSave_Click()
            {
                var data = 'Save';
                var total_level=1;
                var cmb = document.getElementById("txt_templateno");
                if(cmb.value=='')
                {
                    alert('Template No. Required');
                    return false;
                }
                else
                {
                    data+='~'+cmb.value;
                }
                cmb = document.getElementById("txt_shortname");
                data+='~'+cmb.value;
                cmb = document.getElementById("txt_description");
                data+='~'+cmb.value;
                cmb = document.getElementById("lst_targetuser1");
                if(cmb.value!="0")
                {
                    total_level=total_level+1;
                    var cmbsub = document.getElementById("lst_subtargetuser1");  
                    if(cmb.value=="ODH")
                    {
                        data+='~'+cmbsub.value;
                    }
                    else if(cmb.value=="SBH")
                    {
                        data+='~'+cmbsub.value;
                    }
                    else if(cmb.value=="SU")
                    {
                        data+='~'+cmbsub.value;
                    }
                    else if(cmb.value=="RH")
                    {
                        data+='~'+'RH';
                    }
                    else if(cmb.value=="HOD")
                    {
                        data+='~'+'HOD';
                    }
                    else    
                    {
                        data+='~'+'OBH';
                    }
                }
                else    
                {
                    data+='~'+'0';
                }
                cmb = document.getElementById("lst_targetuser2");
                if(cmb.value!="0")
                {
                    total_level=total_level+1;
                    var cmbsub = document.getElementById("lst_subtargetuser2"); 
                    var cmbdata = document.getElementById("Hsetrule2");  
                    if(cmb.value=="ODH")
                    {
                        data+='~'+cmbsub.value;
                        data+='~'+cmbdata.value;
                    }
                    else if(cmb.value=="SBH")
                    {
                        data+='~'+cmbsub.value;
                        data+='~'+cmbdata.value;
                    }
                    else if(cmb.value=="SU")
                    {
                        data+='~'+cmbsub.value;
                        data+='~'+cmbdata.value;
                    }
                    else if(cmb.value=="RH")
                    {
                        data+='~'+'RH';
                        data+='~'+cmbdata.value;
                    }
                    else if(cmb.value=="HOD")
                    {
                        data+='~'+'HOD';
                        data+='~'+cmbdata.value;
                    }
                    else    
                    {
                        data+='~'+'OBH';
                        data+='~'+cmbdata.value;
                    }
                }
                else
                {
                    var cmbdata = document.getElementById("Hsetrule2");  
                    data+='~'+'0';
                    data+='~'+cmbdata.value;
                }
                cmb = document.getElementById("lst_targetuser3");
                if(cmb.value!="0")
                {
                    total_level=total_level+1;
                    var cmbsub = document.getElementById("lst_subtargetuser3");  
                    var cmbdata = document.getElementById("Hsetrule3");  
                    if(cmb.value=="ODH")
                    {
                        data+='~'+cmbsub.value;
                        data+='~'+cmbdata.value;
                    }
                    else if(cmb.value=="SBH")
                    {
                        data+='~'+cmbsub.value;
                        data+='~'+cmbdata.value;
                    }
                    else if(cmb.value=="SU")
                    {
                        data+='~'+cmbsub.value;
                        data+='~'+cmbdata.value;
                    }
                    else if(cmb.value=="RH")
                    {
                        data+='~'+'RH';
                        data+='~'+cmbdata.value;
                    }
                    else if(cmb.value=="HOD")
                    {
                        data+='~'+'HOD';
                        data+='~'+cmbdata.value;
                    }
                    else    
                    {
                        data+='~'+'OBH';
                        data+='~'+cmbdata.value;
                    }
                }
                else
                {
                    var cmbdata = document.getElementById("Hsetrule3");  
                    data+='~'+'0';
                    data+='~'+cmbdata.value;
                }
                cmb = document.getElementById("lst_targetuser4");
                if(cmb.value!="0")
                {
                    total_level=total_level+1;
                    var cmbsub = document.getElementById("lst_subtargetuser4");
                    var cmbdata = document.getElementById("Hsetrule4");    
                    if(cmb.value=="ODH")
                    {
                        data+='~'+cmbsub.value;
                        data+='~'+cmbdata.value;
                    }
                    else if(cmb.value=="SBH")
                    {
                        data+='~'+cmbsub.value;
                        data+='~'+cmbdata.value;
                    }
                    else if(cmb.value=="SU")
                    {
                        data+='~'+cmbsub.value;
                        data+='~'+cmbdata.value;
                    }
                    else if(cmb.value=="RH")
                    {
                        data+='~'+'RH';
                        data+='~'+cmbdata.value;
                    }
                    else if(cmb.value=="HOD")
                    {
                        data+='~'+'HOD';
                        data+='~'+cmbdata.value;
                    }
                    else    
                    {
                        data+='~'+'OBH';
                        data+='~'+cmbdata.value;
                    }
                }
                else
                {
                    var cmbdata = document.getElementById("Hsetrule4");  
                    data+='~'+'0';
                    data+='~'+cmbdata.value;
                }
                cmb = document.getElementById("lst_targetuser5");
                if(cmb.value!="0")
                {
                    total_level=total_level+1;
                    var cmbsub = document.getElementById("lst_subtargetuser5");
                    var cmbdata = document.getElementById("Hsetrule5");    
                    if(cmb.value=="ODH")
                    {
                        data+='~'+cmbsub.value;
                        data+='~'+cmbdata.value;
                    }
                    else if(cmb.value=="SBH")
                    {
                        data+='~'+cmbsub.value;
                        data+='~'+cmbdata.value;
                    }
                    else if(cmb.value=="SU")
                    {
                        data+='~'+cmbsub.value;
                        data+='~'+cmbdata.value;
                    }
                    else if(cmb.value=="RH")
                    {
                        data+='~'+'RH';
                        data+='~'+cmbdata.value;
                    }
                    else if(cmb.value=="HOD")
                    {
                        data+='~'+'HOD';
                        data+='~'+cmbdata.value;
                    }
                    else    
                    {
                        data+='~'+'OBH';
                        data+='~'+cmbdata.value;
                    }
                }
                else
                {
                    var cmbdata = document.getElementById("Hsetrule5");  
                    data+='~'+'0';
                    data+='~'+cmbdata.value;
                }
                cmb = document.getElementById("lst_targetuser6");
                if(cmb.value!="0")
                {
                    total_level=total_level+1;
                    var cmbsub = document.getElementById("lst_subtargetuser6");
                    var cmbdata = document.getElementById("Hsetrule6");    
                    if(cmb.value=="ODH")
                    {
                        data+='~'+cmbsub.value;
                        data+='~'+cmbdata.value;
                    }
                    else if(cmb.value=="SBH")
                    {
                        data+='~'+cmbsub.value;
                        data+='~'+cmbdata.value;
                    }
                    else if(cmb.value=="SU")
                    {
                        data+='~'+cmbsub.value;
                        data+='~'+cmbdata.value;
                    }
                    else if(cmb.value=="RH")
                    {
                        data+='~'+'RH';
                        data+='~'+cmbdata.value;
                    }
                    else if(cmb.value=="HOD")
                    {
                        data+='~'+'HOD';
                        data+='~'+cmbdata.value;
                    }
                    else    
                    {
                        data+='~'+'OBH';
                        data+='~'+cmbdata.value;
                    }
                }
                else
                {
                    var cmbdata = document.getElementById("Hsetrule6");  
                    data+='~'+'0';
                    data+='~'+cmbdata.value;
                }
                data+='~'+total_level;
                cmb = document.getElementById("txt_hoursallowed");
                if(cmb.value=='')
                {
                    alert('Hours Required');
                    return false;
                }
                else
                {
                    data+='~'+cmb.value;
                }
                cmb = document.getElementById("hdID");
                data+='~'+cmb.value;
                CallServer(data,"");
                grid.PerformCallback();
                document.getElementById("TdGrid").style.display = 'inline';
                document.getElementById("TdAll").style.display = 'none'; 
                document.location.href('frmrfamaster.aspx');
            }
            function ReceiveServerData(rValue)
            {
                var DATA = rValue.split('~'); 
                 //alert(rValue); 
                if(DATA[0]=="Combo")
                {   
                    var combo;
                    var datafield;
                    if(DATA[2]=="1")
                    {
                        combo = document.getElementById("lst_subtargetuser1");
                        if(DATA[3]=="1")
                        {
                            document.getElementById("lst_subtargetuser1").style.display = 'inline';
                            datafield="1";
                        }
                        else    
                        {
                            document.getElementById("lst_subtargetuser1").style.display = 'none';
                        }
                    }
                    if(DATA[2]=="2")
                    {
                        combo = document.getElementById("lst_subtargetuser2");
                        if(DATA[3]=="1")
                        {
                            document.getElementById("lst_subtargetuser2").style.display = 'inline';
                            datafield="1";
                        }
                        else    
                        {
                            document.getElementById("lst_subtargetuser2").style.display = 'none';
                        }
                    }
                    if(DATA[2]=="3")
                    {
                        combo = document.getElementById("lst_subtargetuser3");
                        if(DATA[3]=="1")
                        {
                            document.getElementById("lst_subtargetuser3").style.display = 'inline';
                            datafield="1";
                        }
                        else    
                        {
                            document.getElementById("lst_subtargetuser3").style.display = 'none';
                        }
                    }
                    if(DATA[2]=="4")
                    {
                        combo = document.getElementById("lst_subtargetuser4");
                        if(DATA[3]=="1")
                        {
                            document.getElementById("lst_subtargetuser4").style.display = 'inline';
                            datafield="1";
                        }
                        else    
                        {
                            document.getElementById("lst_subtargetuser4").style.display = 'none';
                        }
                    }
                    if(DATA[2]=="5")
                    {
                        combo = document.getElementById("lst_subtargetuser5");
                        if(DATA[3]=="1")
                        {
                            document.getElementById("lst_subtargetuser5").style.display = 'inline';
                            datafield="1";
                        }
                        else    
                        {
                            document.getElementById("lst_subtargetuser5").style.display = 'none';
                        }
                    }
                    if(DATA[2]=="6")
                    {
                        combo = document.getElementById("lst_subtargetuser6");
                        if(DATA[3]=="1")
                        {
                            document.getElementById("lst_subtargetuser6").style.display = 'inline';
                            datafield="1";
                        }
                        else    
                        {
                            document.getElementById("lst_subtargetuser6").style.display = 'none';
                        }
                    }
                    if(datafield=="1")
                    {
                        combo.length=0;
                        var NoItems=DATA[1].split(';');
                        var i;
                        for(i=0;i<NoItems.length;i++)
                        {
                            var items = NoItems[i].split(',');
                            var opt = document.createElement("option");
                            opt.text = items[1];
                            opt.value = items[0];
                            combo.options.add(opt);
                        }
                    }
                    
                }
                if(DATA[0]=="Edit")
                { 
                    var combo = document.getElementById("txt_templateno");
                    combo.value=DATA[34];
                    document.getElementById("txt_templateno").disabled = true;
                    combo = document.getElementById("txt_shortname");
                    combo.value=DATA[1];
                    combo = document.getElementById("txt_description");
                    combo.value=DATA[2];
                    combo = document.getElementById("txt_hoursallowed");
                    combo.value=DATA[4];
                    if(DATA[7]=="1")
                    {
                        var comboTarget = document.getElementById("lst_targetuser1");
                        var comboSubTarget = document.getElementById("lst_subtargetuser1");
                        comboSubTarget.length=0;
                        var NoItems=DATA[8].split(';');
                        var i;
                        for(i=0;i<NoItems.length;i++)
                        {
                            var items = NoItems[i].split(',');
                            var opt = document.createElement("option");
                            opt.text = items[1];
                            opt.value = items[0];
                            comboSubTarget.options.add(opt);
                        }
                        comboTarget.value = DATA[5];
                        comboSubTarget.value = DATA[6];
                        document.getElementById("lst_subtargetuser1").style.display = 'inline';
                    }
                    else    
                    {
                        var comboTarget = document.getElementById("lst_targetuser1");
                        comboTarget.value = DATA[5];
                        document.getElementById("lst_subtargetuser1").style.display = 'none';
                    }
                    if(DATA[11]=="1")
                    {
                        var comboTarget = document.getElementById("lst_targetuser2");
                        var comboSubTarget = document.getElementById("lst_subtargetuser2");
                        comboSubTarget.length=0;
                        var NoItems=DATA[12].split(';');
                        var i;
                        for(i=0;i<NoItems.length;i++)
                        {
                            var items = NoItems[i].split(',');
                            var opt = document.createElement("option");
                            opt.text = items[1];
                            opt.value = items[0];
                            comboSubTarget.options.add(opt);
                        }
                        comboTarget.value = DATA[9];
                        comboSubTarget.value = DATA[10];
                        document.getElementById("lst_subtargetuser2").style.display = 'inline';
                    }
                    else    
                    {
                        var comboTarget = document.getElementById("lst_targetuser2");
                        comboTarget.value = DATA[9];
                        document.getElementById("lst_subtargetuser2").style.display = 'none';
                    }
                    if(DATA[15]=="1")
                    {
                        var comboTarget = document.getElementById("lst_targetuser3");
                        var comboSubTarget = document.getElementById("lst_subtargetuser3");
                        comboSubTarget.length=0;
                        var NoItems=DATA[16].split(';');
                        var i;
                        for(i=0;i<NoItems.length;i++)
                        {
                            var items = NoItems[i].split(',');
                            var opt = document.createElement("option");
                            opt.text = items[1];
                            opt.value = items[0];
                            comboSubTarget.options.add(opt);
                        }
                        comboTarget.value = DATA[13];
                        comboSubTarget.value = DATA[14];
                        document.getElementById("lst_subtargetuser3").style.display = 'inline';
                    }
                    else    
                    {
                        var comboTarget = document.getElementById("lst_targetuser3");
                        comboTarget.value = DATA[13];
                        document.getElementById("lst_subtargetuser3").style.display = 'none';
                    }
                    if(DATA[19]=="1")
                    {
                        var comboTarget = document.getElementById("lst_targetuser4");
                        var comboSubTarget = document.getElementById("lst_subtargetuser4");
                        comboSubTarget.length=0;
                        var NoItems=DATA[20].split(';');
                        var i;
                        for(i=0;i<NoItems.length;i++)
                        {
                            var items = NoItems[i].split(',');
                            var opt = document.createElement("option");
                            opt.text = items[1];
                            opt.value = items[0];
                            comboSubTarget.options.add(opt);
                        }
                        comboTarget.value = DATA[17];
                        comboSubTarget.value = DATA[18];
                        document.getElementById("lst_subtargetuser4").style.display = 'inline';
                    }
                    else    
                    {
                        var comboTarget = document.getElementById("lst_targetuser4");
                        comboTarget.value = DATA[17];
                        document.getElementById("lst_subtargetuser4").style.display = 'none';
                    }
                    if(DATA[23]=="1")
                    {
                        var comboTarget = document.getElementById("lst_targetuser5");
                        var comboSubTarget = document.getElementById("lst_subtargetuser5");
                        comboSubTarget.length=0;
                        var NoItems=DATA[24].split(';');
                        var i;
                        for(i=0;i<NoItems.length;i++)
                        {
                            var items = NoItems[i].split(',');
                            var opt = document.createElement("option");
                            opt.text = items[1];
                            opt.value = items[0];
                            comboSubTarget.options.add(opt);
                        }
                        comboTarget.value = DATA[21];
                        comboSubTarget.value = DATA[22];
                        document.getElementById("lst_subtargetuser5").style.display = 'inline';
                    }
                    else    
                    {
                        var comboTarget = document.getElementById("lst_targetuser5");
                        comboTarget.value = DATA[21];
                        document.getElementById("lst_subtargetuser5").style.display = 'none';
                    }
                    if(DATA[27]=="1")
                    {
                        var comboTarget = document.getElementById("lst_targetuser6");
                        var comboSubTarget = document.getElementById("lst_subtargetuser6");
                        comboSubTarget.length=0;
                        var NoItems=DATA[28].split(';');
                        var i;
                        for(i=0;i<NoItems.length;i++)
                        {
                            var items = NoItems[i].split(',');
                            var opt = document.createElement("option");
                            opt.text = items[1];
                            opt.value = items[0];
                            comboSubTarget.options.add(opt);
                        }
                        comboTarget.value = DATA[25];
                        comboSubTarget.value = DATA[26];
                        document.getElementById("lst_subtargetuser6").style.display = 'inline';
                    }
                    else    
                    {
                        var comboTarget = document.getElementById("lst_targetuser6");
                        comboTarget.value = DATA[25];
                        document.getElementById("lst_subtargetuser6").style.display = 'none';
                    }
                    combo = document.getElementById("Hsetrule2");
                    combo.value=DATA[29];
                    combo = document.getElementById("Hsetrule3");
                    combo.value=DATA[30];
                    combo = document.getElementById("Hsetrule4");
                    combo.value=DATA[31];
                    combo = document.getElementById("Hsetrule5");
                    combo.value=DATA[32];
                    combo = document.getElementById("Hsetrule6");
                    combo.value=DATA[33];
                    combo = document.getElementById("hdID");
                    combo.value=DATA[34];
                }
                if(DATA[0]=="Save")
                {
                    if(DATA[1]=="Y")
                    {
                        if(DATA[2]=="1")
                        {
                            alert('Update Successfully!');
                        }
                        else
                        {
                            alert('Insert Successfully!');
                        }
                    }
                }
                if(DATA[0]=="Delete")
                {
                    if(DATA[1]=="Y")
                    {
                        alert('Deleted Successfully!');
                    }
                }
            }
        </script>
</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div>
     
        <table class="TableMain100">
            <tr>
                <td class="EHEADER" colspan="2" style="text-align:center;">Request For Approval</td>                
            </tr>
            <tr>
                <td style="text-align:left; vertical-align:top; width:10%">
                    <table>
                        <tr>
                            <td>
                                <input id="btnAdd" type="button" value="Add" class="btnUpdate" style="width: 60px" onclick="btnAdd_Click()" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <input id="btnEdit" type="button" value="Modify" class="btnUpdate" style="width: 60px" onclick="btnEdit_Click()"/>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <input id="btnDelete" type="button" value="Delete" class="btnUpdate" style="width: 60px" onclick="btnDelete_Click()"/>
                            </td>
                        </tr>
                    </table>
                </td>
                <td>
                    <table class="TableMain100">
                        <tr>
                            <td id="TdGrid">
                                <dxe:ASPxGridView ID="grdrfa" ClientInstanceName="grid" runat="server" KeyFieldName="rfa_id" Width="100%" OnCustomCallback="grdrfa_CustomCallback">
                                    <Styles>
                                        <Header ImageSpacing="5px" SortingImageSpacing="5px">
                                        </Header>
                                        <LoadingPanel ImageSpacing="10px">
                                        </LoadingPanel>
                                    </Styles>
                                   
                                    <Columns>
                                        <dxe:GridViewDataTextColumn FieldName="rfa_id" Visible="False" VisibleIndex="0">
                                             <CellStyle CssClass="gridcellleft">
                                             </CellStyle>
                                        </dxe:GridViewDataTextColumn>
                                        <dxe:GridViewDataTextColumn FieldName="Title" VisibleIndex="0">
                                             <CellStyle CssClass="gridcellleft">
                                             </CellStyle>
                                        </dxe:GridViewDataTextColumn>
                                    </Columns>
                                    <StylesEditors>
                                        <ProgressBar Height="25px">
                                        </ProgressBar>
                                    </StylesEditors>
                                    <SettingsBehavior AllowFocusedRow="True" AllowSort="False" />
                                    <ClientSideEvents FocusedRowChanged="function(s, e) { OnGridFocusedRowChanged(); }"/>
                                    <SettingsPager ShowSeparators="True">
                                        <FirstPageButton Visible="True">
                                        </FirstPageButton>
                                        <LastPageButton Visible="True">
                                        </LastPageButton>
                                    </SettingsPager>
                                </dxe:ASPxGridView>
                            </td>
                        </tr>
                        <tr>
                            <td id="TdAll">
                                <table class="TableMain100">
                                    <tr>
                                        <td style="width: 197px; text-align: right;">
                                            <span class="Ecoheadtxt" >Template No :</span>
                                        </td>
                                        <td class="gridcellleft" style="width: 190px">
                                            <asp:TextBox ID="txt_templateno" runat="server" Width="186px"></asp:TextBox>
                                        </td>
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                    </tr>
                                    <tr>
                                        <td style="width: 197px; text-align: right;">
                                           <span class="Ecoheadtxt" > Short Name :</span>
                                        </td>
                                        <td class="gridcellleft" style="width: 190px">
                                            <asp:TextBox ID="txt_shortname" runat="server" Width="186px"></asp:TextBox>
                                        </td>
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                    </tr>
                                    <tr>
                                        <td style="width: 197px; text-align: right;">
                                            <span class="Ecoheadtxt" >Description :</span>
                                        </td>
                                        <td colspan="2" class="gridcellleft">
                                            <asp:TextBox ID="txt_description" runat="server" TextMode="MultiLine" Width="348px" Height="59px"></asp:TextBox>
                                        </td>
                                        <td></td>
                                        <td></td>
                                    </tr>
                                    <tr>
                                        <td style="width: 197px; text-align: right;">
                                            <span class="Ecoheadtxt" >Target User [1st Level ] :</span>
                                        </td>
                                        <td class="gridcellleft" style="width: 190px">
                                            <asp:DropDownList ID="lst_targetuser1" runat="server" Width="190px">
                                                <asp:ListItem Value="0">None</asp:ListItem>
                                                <asp:ListItem Value="RH">Own Reporting Head</asp:ListItem>
                                                <asp:ListItem Value="HOD">Own Department Head</asp:ListItem>
                                                <asp:ListItem Value="ODH">Other Department Head</asp:ListItem>
                                                <asp:ListItem Value="SBH">Specific Branch Head</asp:ListItem>
                                                <asp:ListItem Value="OBH">Own Branch Head</asp:ListItem>
                                                <asp:ListItem Value="SU">Specific User</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                        <td class="gridcellleft" id="subtarget1" style="width:160px">
                                            <asp:DropDownList ID="lst_subtargetuser1" runat="server" Width="160px">
                                            </asp:DropDownList>
                                        </td>
                                        <td></td>
                                        <td></td>
                                    </tr>
                                    <tr>
                                        <td style="width: 197px; text-align: right;">
                                            <span class="Ecoheadtxt" >Target User [2nd Level ] :</span>
                                        </td>
                                        <td class="gridcellleft" style="width: 190px">
                                            <asp:DropDownList ID="lst_targetuser2" runat="server" Width="190px">
                                                <asp:ListItem Value="0">None</asp:ListItem>
                                                <asp:ListItem Value="RH">Own Reporting Head</asp:ListItem>
                                                <asp:ListItem Value="HOD">Own Department Head</asp:ListItem>
                                                <asp:ListItem Value="ODH">Other Department Head</asp:ListItem>
                                                <asp:ListItem Value="SBH">Specific Branch Head</asp:ListItem>
                                                <asp:ListItem Value="OBH">Own Branch Head</asp:ListItem>
                                                <asp:ListItem Value="SU">Specific User</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                        <td class="gridcellleft" id="subtarget2" style="width:160px">
                                            <asp:DropDownList ID="lst_subtargetuser2" runat="server" Width="160px">
                                            </asp:DropDownList>
                                        </td>
                                        <td style="text-align:left">
                                        <a href="#" onclick="setRules123('Hsetrule2','Hsetrule2')">
                                            <span style="color: #000099; text-decoration: underline">Set Rule</span></a>
                                        </td>
                                        <td><asp:HiddenField ID="Hsetrule2" runat="server" Value="" /></td>
                                    </tr>
                                    <tr>
                                        <td style="width: 197px; text-align: right;">
                                            <span class="Ecoheadtxt" >Target User [3rd Level ] :</span>
                                        </td>
                                        <td class="gridcellleft" style="width: 190px">
                                            <asp:DropDownList ID="lst_targetuser3" runat="server" Width="190px">
                                                <asp:ListItem Value="0">None</asp:ListItem>
                                                <asp:ListItem Value="RH">Own Reporting Head</asp:ListItem>
                                                <asp:ListItem Value="HOD">Own Department Head</asp:ListItem>
                                                <asp:ListItem Value="ODH">Other Department Head</asp:ListItem>
                                                <asp:ListItem Value="SBH">Specific Branch Head</asp:ListItem>
                                                <asp:ListItem Value="OBH">Own Branch Head</asp:ListItem>
                                                <asp:ListItem Value="SU">Specific User</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                        <td class="gridcellleft" id="subtarget3" style="width:160px">
                                            <asp:DropDownList ID="lst_subtargetuser3" runat="server" Width="160px">
                                            </asp:DropDownList>
                                        </td>
                                        <td style="text-align:left">
                                        <a href="#" onclick="setRules123('Hsetrule3','Hsetrule3')">
                                            <span style="color: #000099; text-decoration: underline">Set Rule</span></a>
                                        </td>
                                        <td><asp:HiddenField ID="Hsetrule3" runat="server" Value="" /></td>
                                    </tr>
                                    <tr>
                                        <td style="width: 197px; text-align: right;">
                                            <span class="Ecoheadtxt" >Target User [4th Level ] :</span>
                                        </td>
                                        <td class="gridcellleft" style="width: 190px">
                                            <asp:DropDownList ID="lst_targetuser4" runat="server" Width="190px">
                                                <asp:ListItem Value="0">None</asp:ListItem>
                                                <asp:ListItem Value="RH">Own Reporting Head</asp:ListItem>
                                                <asp:ListItem Value="HOD">Own Department Head</asp:ListItem>
                                                <asp:ListItem Value="ODH">Other Department Head</asp:ListItem>
                                                <asp:ListItem Value="SBH">Specific Branch Head</asp:ListItem>
                                                <asp:ListItem Value="OBH">Own Branch Head</asp:ListItem>
                                                <asp:ListItem Value="SU">Specific User</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                        <td class="gridcellleft" id="subtarget4" style="width:160px">
                                            <asp:DropDownList ID="lst_subtargetuser4" runat="server" Width="160px">
                                            </asp:DropDownList>
                                        </td>
                                        <td style="text-align:left">
                                        <a href="#" onclick="setRules123('Hsetrule4','Hsetrule4')">
                                            <span style="color: #000099; text-decoration: underline">Set Rule</span></a>
                                        </td>
                                        <td>
                                            <asp:HiddenField ID="Hsetrule4" runat="server" Value="" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 197px; text-align: right;">
                                            <span class="Ecoheadtxt" >Target User [5th Level ] :</span>
                                        </td>
                                        <td class="gridcellleft" style="width: 190px">
                                            <asp:DropDownList ID="lst_targetuser5" runat="server" Width="190px">
                                                <asp:ListItem Value="0">None</asp:ListItem>
                                                <asp:ListItem Value="RH">Own Reporting Head</asp:ListItem>
                                                <asp:ListItem Value="HOD">Own Department Head</asp:ListItem>
                                                <asp:ListItem Value="ODH">Other Department Head</asp:ListItem>
                                                <asp:ListItem Value="SBH">Specific Branch Head</asp:ListItem>
                                                <asp:ListItem Value="OBH">Own Branch Head</asp:ListItem>
                                                <asp:ListItem Value="SU">Specific User</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                        <td class="gridcellleft" id="subtarget5" style="width:160px">
                                            <asp:DropDownList ID="lst_subtargetuser5" runat="server" Width="160px">
                                            </asp:DropDownList>
                                        </td>
                                        <td style="text-align:left">
                                        <a href="#" onclick="setRules123('Hsetrule5','Hsetrule5')">
                                            <span style="color: #000099; text-decoration: underline">Set Rule</span></a>
                                        </td>
                                        <td><asp:HiddenField ID="Hsetrule5" runat="server" Value="" /></td>
                                    </tr>
                                    <tr>
                                        <td style="width: 197px; text-align: right;">
                                            <span class="Ecoheadtxt" >Target User [6th Level ] :</span>
                                        </td>
                                        <td class="gridcellleft" style="width: 190px">
                                            <asp:DropDownList ID="lst_targetuser6" runat="server" Width="190px">
                                                <asp:ListItem Value="0">None</asp:ListItem>
                                                <asp:ListItem Value="RH">Own Reporting Head</asp:ListItem>
                                                <asp:ListItem Value="HOD">Own Department Head</asp:ListItem>
                                                <asp:ListItem Value="ODH">Other Department Head</asp:ListItem>
                                                <asp:ListItem Value="SBH">Specific Branch Head</asp:ListItem>
                                                <asp:ListItem Value="OBH">Own Branch Head</asp:ListItem>
                                                <asp:ListItem Value="SU">Specific User</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                        <td class="gridcellleft" id="subtarget6" style="width:160px">
                                            <asp:DropDownList ID="lst_subtargetuser6" runat="server" Width="160px">
                                            </asp:DropDownList>
                                        </td>
                                        <td style="text-align:left">
                                        <a href="#" onclick="setRules123('Hsetrule6','Hsetrule6')">
                                            <span style="color: #000099; text-decoration: underline">Set Rule</span></a>
                                        </td>
                                        <td><asp:HiddenField ID="Hsetrule6" runat="server" Value="" /></td>
                                    </tr>
                                    <tr>
                                        <td style="width: 197px; text-align: right;">
                                            <span class="Ecoheadtxt" >Hours Allowed : </span>
                                        </td>
                                        <td class="gridcellleft" style="width: 190px">
                                            <asp:TextBox ID="txt_hoursallowed" runat="server" Width="186px"></asp:TextBox>
                                        </td>
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                    </tr>
                                    <tr>
                                        <td style="width: 197px"></td>
                                        <td style="width: 190px">
                                            <input id="btnSave" type="button" value="Save" class="btnUpdate" onclick="btnSave_Click()" style="width: 54px"/>
                                            <input id="btnCancel" type="button" value="Cancel" class="btnUpdate" onclick="btnCancel_Click()"/>
                                            <input id="hdID" type="hidden" style="width: 151px; height: 7px" />
                                        </td>
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
