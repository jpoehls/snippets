﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="TreeViewPopulateOnDemand._Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:TreeView runat="server" ID="uxTree" OnTreeNodePopulate="uxTree_TreeNodePopulate" />
    </div>
    <div>
        <iframe id="myframe" name="myframe" style="width: 400px; height: 400px;"></iframe>
    </div>
    </form>
</body>
</html>
