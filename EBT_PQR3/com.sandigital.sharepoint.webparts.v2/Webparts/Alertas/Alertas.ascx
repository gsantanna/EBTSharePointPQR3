<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Alertas.ascx.cs" Inherits="com.sandigital.sharepoint.webparts.v2.Webparts.Alertas.Alertas" %>


<asp:Label ID="lblErro" runat="server"></asp:Label>




<asp:Panel ID="pnlMain" runat="server"  Visible="false">


    <asp:Repeater ID="rptMain" runat="server">
        <HeaderTemplate>
            <style type="text/css">
    .AlertaHeader {
        width: 100%;
        background-color: #c54343;
        min-height: 30px;
        height: 30px;
        color: white;
        font-family: Verdana, Geneva, Tahoma, sans-serif;
        font-size: 12px;
        font-weight: bolder;
        padding-top: 6px;
        padding-left: 16px;
        border-bottom-left-radius: 15px;
        border-top-left-radius: 15px;
    }

    .AlertaContainer {
        
        width: 100%;
        background-color: #efefef;
        padding:20px 20px 20px 20px;
              
        border-left: 1px solid #949494;
        border-bottom: 1px solid #949494;
        border-right: 1px solid #949494;
        
        font-family: Verdana, Geneva, Tahoma, sans-serif;
        

    }

   
    .ItemAlertaTB
    {
       /* width:100%;*/
    }

    .ItemAlertaTitulo
    {        
        font-size: 12px;
        font-weight: bolder;
        color:black;
    }
    .ItemAlertaTexto
    {
        font-size:12px;

    }


</style>
            
            <div class="AlertaHeader">ALERTAS</div>
            <div style="padding-left:12px;width:100%;">
            <div class="AlertaContainer">

        </HeaderTemplate>

        <ItemTemplate>
            <table class="ItemAlertaTB">
                <tr>
                    <td style="width:25px;"><img src="/_layouts/Images/com.sandigital.sharepoint.webparts.v2/RedArrow.png"   alt="" style="width:20px;height:26px;"/>  </td><td style="vertical-align:middle">  <div class="ItemAlertaTitulo"><%# Eval("Title") %></div></td>           
                </tr>
                <tr>
                <td></td><td style="vertical-align:top"><p class="ItemAlertaTexto"><%#Eval("Body") %>  </p></td>
                </tr>
            </table>
        </ItemTemplate>

        <FooterTemplate>

</div>
    </div>
        </FooterTemplate>

    </asp:Repeater>



    




    
    
    
    

      
        
    


</asp:Panel>