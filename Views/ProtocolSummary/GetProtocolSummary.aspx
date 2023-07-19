<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<!DOCTYPE html>

<html>
<head runat="server">
    <meta name="viewport" content="width=device-width" />
    <title>GetProtocolSummary</title>
    <script src="../../Documents/JS/jquery-1.8.0.min.js"></script>
    <script src="../../Documents/JS/jquery.form.js"></script>
    <%-- <script src="../../Documents/JS/jquery-ui-1.10.3.custom.js"></script>--%>
    <link href="../../Documents/CSS/jquery-ui-1.10.3.custom.css" rel="stylesheet" />
    <script src="../../Documents/JS/jquery.validationEngine-en.js"></script>
    <script src="../../Documents/JS/jquery.validationEngine.js"></script>
    <script src="../../Documents/JS/jquery.unobtrusive-ajax.js"></script>
    <link href="../../Documents/CSS/jquery-ui.css" rel="stylesheet" />
    <script src="../../Documents/JS/jquery-ui-1.11.2.js"></script>
    <link href="../../Documents/CSS/validationEngine.jquery.css" rel="stylesheet" />
    <link href="../../Documents/CSS/style.css" rel="stylesheet" />
    <script type="text/javascript">
        $(function () {
            $('#FillProtocol').load('../ProtocolSummary/Index/');
        });
    </script>
</head>
    
<body>

    <div id="FillProtocol">
        
    </div>
</body>
</html>
