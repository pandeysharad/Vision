﻿<%@ Page Language="C#" AutoEventWireup="true" CodeFile="StaffListReport.aspx.cs" Inherits="Pages_StaffListReport" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
<style type="text/css">
#customers {
    font-family: "Trebuchet MS", Arial, Helvetica, sans-serif;
    border-collapse: collapse;
    width: 100%;
}

#customers td, #customers th {
    border: 1px solid #ddd;
    padding: 8px;
}

#customers tr:nth-child(even){background-color: #f2f2f2;}

#customers th {
    padding-top: 12px;
    padding-bottom: 12px;
    text-align: left;
    background-color: #4CAF50;
    color: white;
}
</style>
</head>
<body>
    <form id="form1" runat="server">
     <table width="280px" style="font-weight: bold">
     <tr>
     <td colspan="2">STAFF LIST SHEET:-</td>
     </tr>
    </table>
    <table width="100%" style="font-weight: bold">
        <tr>
            <td>
                <%=print%>
            </td>
        </tr>
    </table>
    <br />
    <hr />
     <table width="280px" style="font-weight: bold">
     <tr>
     <td colspan="2">OTHER INFORMATION:-</td>
      </tr>
    </table>
     <table width="100%" style="font-weight: bold">
        <tr>
            <td>
                <%=print1%>
            </td>
        </tr>
    </table>
    <br />
    <hr />
       <table width="280px" style="font-weight: bold">
     <tr>
     <td colspan="2">ACADMIC INFORMATION:-</td>
      </tr>
    </table>
     <table width="100%" style="font-weight: bold">
        <tr>
            <td>
                <%=print2%>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
