<%@ Page Language="C#" AutoEventWireup="true" Inherits="ErrorPages_ErrorPage" Codebehind="ErrorPage.aspx.cs" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Untitled Page</title>
    <link href="https://fonts.googleapis.com/css?family=Yanone+Kaffeesatz" rel="stylesheet">

    <%--<script type="text/javascript" language="javascript">

        function DisableBackButton() {
            alert('hi');
            window.history.forward()
        }
        DisableBackButton();
        window.onload = DisableBackButton;
        window.onpageshow = function(evt) { if (evt.persisted) DisableBackButton() }
        window.onunload = function() { void (0) }
    </script>
--%>
    <style>
        body,
html {
  margin: 0px;
  background-color: #f8f8f8;
  font-weight:normal;
  font-family: 'Yanone Kaffeesatz', sans-serif;
  font-size:16px;
  color: #979797;
  overflow-x: hidden;
  min-height: 100%;
  height: 100%;
}
a {
  text-decoration: none; 
}
h3 {
  -webkit-transform: rotate(-45deg) translateY(-30px);
          transform: rotate(-45deg) translateY(-30px); 
  color: white; 
  width: 100px;
  
}

.ghost {
  width: 125px;
  height: 200px;
  background-color: #f8f8f8;
  border: 3px solid #979797;
  position: relative;
  margin: auto;
  top: 140px;
  left: 0;
  right: 0;
  border-radius: 150px 150px 7px 7px;
  -webkit-animation: ghost 1.5s ease-in-out alternate infinite;
          animation: ghost 1.5s ease-in-out alternate infinite;
  z-index: 1; 
}

.corner {
  width: 21px; height: 21px;
  border-radius: 5px; 
  background-color: #f8f8f8;
  border: 3px solid #979797;
  position: absolute;
  margin-top: 188px; 
  -webkit-transform: rotate(45deg); 
          transform: rotate(45deg); 
  z-index: -1; 
}
.corner.two {
  margin-left: 33px; 
}
.corner.three {
  margin-left: 66px; 
}
.corner.four {
  margin-left: 98px; 
}

.over {
  width: 27px; height: 27px;
  border-radius: 3px; 
  background-color: #f8f8f8;
  position: absolute;
  margin-top: 188px; 
  -webkit-transform: rotate(45deg) translateX(3px) translateY(3px); 
          transform: rotate(45deg) translateX(3px) translateY(3px); 
}
.over.two {
  margin-left: 33px; 
}
.over.three {
  margin-left: 66px; 
}
.over.four {
  margin-left: 98px; 
}

.eye {
  width: 10px; height: 10px;
  border-radius: 10px; 
  background-color: #979797; 
  position: absolute; margin: 50px auto; 
  left: 0;right: 0;
  -webkit-transform: translateX(-25px);
          transform: translateX(-25px); 
}
.eye.two {
  -webkit-transform: translateX(25px);
          transform: translateX(25px); 
}

.mouth {
  width: 40px; height: 4px;
  border-radius: 10px; 
  background-color: #979797; 
  position: absolute; margin: 75px auto; 
  left: 0;right: 0;
}
.tonge {
  width: 4px; height: 10px;
  border-radius: 0px 0px 50px 50px; 
  background-color: #979797; 
  position: absolute; margin: 2px auto; 
  left: 0;right: 0;
  -webkit-transform: translateX(-6px);
          transform: translateX(-6px); 
  border-left: 5px solid rgba(0,0,0,.3);
  border-right: 5px solid rgba(0,0,0,.3);
  border-bottom: 5px solid rgba(0,0,0,.3);
}

.shadow {
  width: 120px;
  height: 30px;
  border-radius: 100%;
  background-color: rgba(0,0,0,.3);
  position: absolute;
  margin: 230px auto;
  left: 0;
  right: 0;
  -webkit-animation: shadow 1.5s ease-in-out alternate infinite;
          animation: shadow 1.5s ease-in-out alternate infinite;
}

.text{
  position: absolute;
  text-align: center;
  margin: auto;
  left: 0;
  right: 0;
  bottom: 20px;
  font-size: 3em;
}

@-webkit-keyframes shadow {
  0% {
    -webkit-transform: scale(1) translateY(0px);
            transform: scale(1) translateY(0px);
  }
  100% {
    -webkit-transform: scale(0.8) translateY(75px);
            transform: scale(0.8) translateY(75px);
  }
}

@keyframes shadow {
  0% {
    -webkit-transform: scale(1) translateY(0px);
            transform: scale(1) translateY(0px);
  }
  100% {
    -webkit-transform: scale(0.8) translateY(75px);
            transform: scale(0.8) translateY(75px);
  }
}
@-webkit-keyframes ghost {
  0% {
    -webkit-transform: scale(1) translateY(0px);
            transform: scale(1) translateY(0px)
  }
  100% {
    -webkit-transform: scale(1) translateY(-40px);
            transform: scale(1) translateY(-40px)
  }
}
@keyframes ghost {
  0% {
    -webkit-transform: scale(1) translateY(0px);
            transform: scale(1) translateY(0px)
  }
  100% {
    -webkit-transform: scale(1) translateY(-40px);
            transform: scale(1) translateY(-40px)
  }
}

.main {
  margin-top: 235px;
  text-align: center;
}
.main h2 {
  color: #666;
  font-size: 80px;
  margin: 15px 0;
}
.main h6 {
  font-size: 30px;
  line-height: 44px;
  margin: 28px 0;
  font-weight: 100;
}
.main h6 strong{
  font-size: 26px;
}
.main .error {
  position: absolute;
  margin: auto;
  left: 0;
  right: 0;
  top: -10vw;
  color: rgba(0,0,0,.03);
  font-size: 25vw;
  text-align: center;
  font-weight: 900;
  font-family: sans-serif;
}
.gobtn {
    background: #80c5c5;
    padding: 8px 27px;
    color: #fff;
    font-size: 18px;
    letter-spacing: 2px;
}
.gobtn:hover {
    background:#4cafaf;
}
    </style>
</head>
<body >
    <form id="form1" runat="server">
   <%-- 12-12-2016 by susanta error page design modification <div>
        <table style="width: 100%; height: 100%;">
            <tr>
                <td style="height: 100px;">
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td align="center" valign="middle">
                    <span style="font-size: 25px; color: Black; padding-bottom: 20px!important;"><strong>
                        <%=ErrorMessage %> </strong></span>
                    <br />
                    <br />
                    <span><%=ErrorDescription %></span>
                    <br />
                    <table>
                    <tr>
                    <td>
                  &nbsp;
                    </td>
                    <td>
                     &nbsp;
                    </td>
                    </tr>
                    </table>
                   
                </td>
            </tr>
        </table>
    </div>--%>
        <%--<div class="container">
            <div class="row">
                <div class="col-md-12">
                    <div class="error-template">
                        <h1>
                            Oops!</h1>
                        <h2>
                            404 Not Found</h2>
                        <div class="error-details">
                            Sorry, an error has occured, Requested page not found!
                        </div>
                        <div class="error-actions">
                            <a href="login.aspx" class="btn btn-primary btn-lg"><span class="glyphicon glyphicon-home"></span>
                                Back to Login </a>
                        </div>
                    </div>
                </div>
            </div>
        </div>--%>
        

          <div class="ghost">
            <span class="text">404</span>
            <div class="eye"></div>
            <div class="eye two"></div>
    
            <div class="mouth"></div>
    
            <div class="corner"></div>
            <div class="corner two"></div>
            <div class="corner three"></div>
            <div class="corner four"></div>
    
            <div class="over"></div>
            <div class="over two"></div>
            <div class="over three"></div>
            <div class="over four"></div>
    
            <div class="shadow"></div>
          </div>

          <div class="main">
            <div class="error">error</div>
            <h2>We've got a problem</h2>
            <h6><strong>There's a lot of reasons why this page is 404.</strong><br />
                Don't waste your time enjoying the look of it</h6>
            <%--  <a href="login.aspx" class="gobtn " >Go Ahead</a>--%>
              <asp:HyperLink ID="hdnGoAhead" runat="server" class="gobtn ">Go Ahead</asp:HyperLink>
          </div>
    </form>
</body>
</html>
