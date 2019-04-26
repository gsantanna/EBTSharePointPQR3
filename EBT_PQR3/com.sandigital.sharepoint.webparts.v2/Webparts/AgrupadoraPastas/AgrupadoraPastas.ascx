<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AgrupadoraPastas.ascx.cs" Inherits="com.sandigital.sharepoint.webparts.v2.Webparts.AgrupadoraPastas.AgrupadoraPastas" %>
<asp:Label ID="lblHack" runat="server"></asp:Label>
<asp:Label ID="lblErro" runat="server"></asp:Label>

<div id='<asp:Literal ID="ltDiv1" runat="server"></asp:Literal>'></div>

<script type="application/html-template" id="wpa-header">
<table style="width:100%">
</script>

<script type="application/html-template" id="wpa-pasta">

<tr class="grupo_{{idx_pai}}" style="{{vis}}" >
	<td style="padding-left:{{nivel}}px">
		<table style="width:100%">
			<tr>
				<td style="">
				<img border="0" alt="{{Nome}}" title="{{Nome}}" src="/_layouts/images/folder.gif"><a href="javascript:" onclick="expandir('{{idx_pasta}}');return false;" class="trigger_{{idx_pasta}}"><img src="/_layouts/images/plus.gif" border="0"></a> <strong>&nbsp; {{Nome}}</strong>
				</td>
			</tr>
			
				{{Pastas}}
				{{Arquivos}}
								
		</table>
	</td>
</tr>
</script>

<script type="application/html-template" id="wpa-arquivo">


<!-- inicio arquivo-->
<tr class="grupo_{{idx_pai}}" style="display:none;{{vis}}"  >
	<td style="padding-left:{{nivel}}px;" >
		<a href="{{url}}">
			<img src="/_layouts/images/{{Icone}}" alt="{{Titulo}}" /> &nbsp;  {{Titulo}}
		</a>
	</td>
</tr>
<!-- fim arquivo -->

</script>

<script type="application/html-template" id="wpa-footer">
</table>
</script>




<script type="text/javascript">

    (function () {

        var strJson = '<asp:Literal ID="ltSaida" runat="server"></asp:Literal>';
        var arrPastas = JSON.parse(strJson);



        function getHtmlPastas(arrPastas, inicio) {
            var htmlSaida = "";

            $(arrPastas).each(function (idx, obj) {

                htmlSaida += $("#wpa-pasta").html()
                .replace(/{{Nome}}/g, obj.Pasta)
                .replace(/{{idx_pasta}}/g, obj.idx_pasta)
                .replace(/{{idx_pai}}/g, obj.idx_pai)
                .replace(/{{Pastas}}/g, obj.Pastas.length == 0 ? "" : "" + getHtmlPastas(obj.Pastas, false) + "")
                .replace(/{{Arquivos}}/g, obj.Arquivos.length == 0 ? "" : "" + getHtmlArquivos(obj.Arquivos, obj.idx_pasta, inicio ) + "")
                .replace(/{{vis}}/g, inicio ? "" : "display:none")
                .replace(/{{nivel}}/g, obj.nivel * 5);


            });	//each

            return htmlSaida;
        }

        function getHtmlArquivos(arrArquivos, idx_pai, inicio ) {


            var htmlSaida = "";
            $(arrArquivos).each(function (idx, obj) {

                htmlSaida += $("#wpa-arquivo").html()
                .replace(/{{Titulo}}/g, obj.Titulo)
                .replace(/{{url}}/g, obj.url)
                .replace(/{{Icone}}/g, obj.Icone)
                .replace(/{{idx_pai}}/g, idx_pai)
                .replace(/{{vis}}/g, inicio ? "" : "display:none")

                ;

            });//arrArq

            return htmlSaida;
        }

        
        var dvSaida = $('<asp:Literal ID="ltDiv2" runat="server"></asp:Literal>');

         //arrPastas
        var htmlSaida = $("#wpa-header").html();

        $(arrPastas).each(function (idxP1, objP1) {

            //desenha a pasta 
            htmlSaida += getHtmlPastas(objP1, true);

        });//P1.Each

        htmlSaida += $("#wpa-footer").html();

        $(dvSaida).html(htmlSaida);

     })();





</script>