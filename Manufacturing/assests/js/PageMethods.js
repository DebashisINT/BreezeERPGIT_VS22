// PageMethods.js
var msg_count;
var element_msg;

// Initializes global variables and session state.
    function pageLoad()
       {       
            if ($get("MessageId")) 
            {    
            element_msg= $get("MessageId");          
            }
            if ($get("H_msgcount"))
            {    
            msg_count= $get("H_msgcount").value;    
            }    
       }

    // Gets the session state value.
    function GetSessionValue(key) 
    {
     PageMethods.GetSessionValue(key,OnSucceeded,OnFailed);    
    }

    function GetAttachment(message) 
    {
     PageMethods.GetAttachment(message,OnSucceeded,OnFailed); 
    }

    function GetRates(key)
    {
     PageMethods.GetRates(key,OnSucceeded,OnFailed);
    }

    function Getlogout(user)
    {
     PageMethods.Getlogout(user,OnSucceeded,OnFailed);
    }

    //Sets the session state value.
    function SetSessionValue(key, value) 
    {
      PageMethods.SetSessionValue(key, value,OnSucceeded,OnFailed);       
    }

    function SetRates()
    {
       PageMethods.SetRates(key, value,OnSucceeded,OnFailed);
    }
    function SetAttachment(key, value) 
    {   
       PageMethods.SetAttachment(key,value,OnSucceeded,OnFailed);
    }
        
    function Setlogout(user, value) 
    {   
      PageMethods.Setlogout(user,value,OnSucceeded,OnFailed);
    }    
            
    // Callback function invoked on successful 
    // completion of the page method.
    function OnSucceeded(result, userContext, methodName) 
    {
        if (methodName == "GetAttachment")    
        {    
            if (result.charAt()==0)
            {    
             result=result.split("|")
             alert(result[1]);
            }
        }     
         
        //----------------------
       if (methodName == "Getlogout")
       {
        if (result==0)
         {
          alert('Someone else has logged in using this id... ');                      
           window.parent.location.href='../login.aspx'; 
          //window.parent.close();
          //window.open("../login.aspx")          
         }
        }    
        
        //----------------------
        if (methodName == "GetSessionValue")
        { 
            if (typeof(element_msg) !== "undefined")         
            {
            if (result==msg_count)
            {    
           
        }
        else
        {
            msg_count=result;
            element_msg.innerHTML='';
            var st = "hi";    
            element_msg.innerHTML ="<img src='../Images/IMG/New_messag1.gif' style='border-width:0px; cursor: hand;'  alt='You Have "+result+" New Messages !!'    onclick='javaScript:frmOpen()' />";    
            if (result!=0)
            {
             alert('You Have ' + result + ' New Messages!')                    
             frmOpen();                  
             _sound.beep(1);
            }
        
        }
        if (result==0)
        {    
            msg_count=0;
            element_msg.innerHTML = '';
        }
      }
     }  
    }

    // Callback function invoked on failure 
    // of the page method.
    function OnFailed(error, userContext, methodName) 
    {       
        if(error !== null) 
        {          
          // window.location.href='./login.aspx';        
        }
    }

    if (typeof(Sys) !== "undefined") Sys.Application.notifyScriptLoaded();  
       
    function frmOpen()
       {
       window.open("frmmessage_popup.aspx","Search_Conformation_Box","height=700,width=850,right=0,top=0location=no,directories=no,menubar=no,toolbar=no,status=no,scrollbars=no,resizable=no,dependent=no'");       
       }