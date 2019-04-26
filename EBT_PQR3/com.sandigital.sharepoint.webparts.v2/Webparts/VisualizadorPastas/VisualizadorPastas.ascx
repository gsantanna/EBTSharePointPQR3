<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Register TagPrefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="VisualizadorPastas.ascx.cs" Inherits="com.sandigital.sharepoint.webparts.v2.Webparts.VisualizadorPastas.VisualizadorPastas" %>
<asp:Label ID="lblHack" runat="server">

</asp:Label>

<asp:Label ID="lblErro" runat="server" />



    

    <asp:Repeater ID="rptCaminho" runat="server">
        <HeaderTemplate>
           
            <div style="width:100%;background-color:#ededed">


        </HeaderTemplate>
        <ItemTemplate>
            <asp:LinkButton ID="lnkGo" runat="server" CommandName='0' CommandArgument='<%#Eval("url") %>' OnCommand="lnkClick_Command">                        
                <%#Eval("name") %>
                </asp:LinkButton>

        </ItemTemplate>
        <SeparatorTemplate>
            
        </SeparatorTemplate>

        <FooterTemplate>

            </div>

        </FooterTemplate>
    </asp:Repeater>


























<asp:Repeater ID="rptArquivos" runat="server">

    <HeaderTemplate>


        <table summary="Biblioteca " width="100%" border="0" cellspacing="0" dir="none" onmouseover="EnsureSelectionHandler(event,this,9)" cellpadding="1" id="onetidDoclibViewTbl0" class="ms-listviewtable">

    <tbody>

        <tr valign="top" class="ms-viewheadertr ms-vhltr">


            <th nowrap="" scope="col" onmouseover="OnChildColumn(this)" class="ms-vh2">
                <div sortable="" sortdisable="" filterdisable="" filterable="" filterdisablemessage="" name="DocIcon" class="ms-vh-div" style="font-weight:normal;">
                    Tipo
                    <img src="/_layouts/images/blank.gif" class="ms-hidden" border="0" width="1" height="1">
                    <img src="/_layouts/images/blank.gif" alt="" border="0"><img src="/_layouts/images/blank.gif" border="0" alt="">
                </div>
            </th>


            <th nowrap="" scope="col" onmouseover="OnChildColumn(this)" class="ms-vh2">
                <div sortable="" sortdisable="" filterdisable="" filterable="FALSE" filterdisablemessage="" name="LinkFilename"  displayname="Nome"  resulttype="" class="ms-vh-div" style="font-weight:normal;">
                    Nome<img src="/_layouts/images/blank.gif" alt="" border="0"><img src="/_layouts/images/blank.gif" border="0" alt="">
                </div>

            </th>
        </tr>


    </HeaderTemplate>
    <ItemTemplate>


        <tr class="ms-itmhover" setedgeborder="true">


            <td class="ms-vb-icon">
                

                 <asp:LinkButton ID="LinkButton1" runat="server" CommandName='<%#Eval("tp") %>' CommandArgument='<%#Eval("url") %>' OnCommand="lnkClick_Command">                        
                   
                      <img border="0" alt='<%#  Eval("name","Item: {0}") %>' title='<%#Eval("name") %>' src='<%#Eval("docIcon") %>'>
                          
                        </asp:LinkButton>
            &nbsp;&nbsp;</td>
            <td height="100%" class="ms-vb-title ms-vb-lastCell">
                <div class="ms-vb itx" ">

                   
                    <asp:LinkButton ID="btnAcao" runat="server" CommandName='<%#Eval("tp") %>' CommandArgument='<%#Eval("url") %>'  OnCommand="lnkClick_Command"   Visible='<%#  Convert.ToInt32( Eval("tp")) == 0 %>'  >                        
                        <%#Eval("name") %>
                        </asp:LinkButton>                 


                    <asp:HyperLink ID="lnkAcao" runat="server" NavigateUrl='<%# Eval("url") %>'  Visible='<%#  Convert.ToInt32( Eval("tp")) == 1 %>' >
                          <%#Eval("name") %>
                    </asp:HyperLink>                      



                </div>
            </td>
        </tr>




    </ItemTemplate>
    <FooterTemplate>
        
            </tbody>
</table>




        <table id="" width="100%" cellpadding="0" cellspacing="0" border="0" style="" xmlns:x="http://www.w3.org/2001/XMLSchema" 
    xmlns:d="http://schemas.microsoft.com/sharepoint/dsp" xmlns:asp="http://schemas.microsoft.com/ASPNET/20"
     xmlns:__designer="http://schemas.microsoft.com/WebParts/v2/DataView/designer" 
    xmlns:sharepoint="Microsoft.SharePoint.WebControls" xmlns:ddwrt2="urn:frontpage:internal">
    
    <tbody><tr><td colspan="2" class="ms-partline">
        <img src="/_layouts/images/blank.gif" width="1" height="1" alt=""></td></tr><tr>
            
            <td class="ms-addnew" style="padding-bottom: 3px"><span style="height:10px;width:10px;position:relative;display:inline-block;overflow:hidden;" class="s4-clust"><img src="/_layouts/images/fgimg.png" alt="" style="left:-0px !important;top:-128px !important;position:absolute;"></span>&nbsp;<a class="ms-addnew" id="idHomePageNewDocument" 
                href="<%#strAddLink %>&amp;" 
                onclick="javascript:NewItem2(event, &quot;<%#strAddLink %>&quot;);javascript:return false;" target="_self">Adicionar documento</a></td></tr><tr><td><img src="/_layouts/images/blank.gif" width="1" height="5" alt=""></td></tr></tbody></table>









    </FooterTemplate>
</asp:Repeater>






<script type="text/javascript">

    $(document).ready(function () {

        var hack = $(".ag_hackTitulo");

        $(hack).each(function () {
            $($(this).closest("tbody")).find(".ms-WPHeaderTd h3 nobr").html("<a href='" + $(this).attr("url") + "'>" + $(this).attr("titulo") + "</a>")
        });
        //

    });


</script>


        

        


