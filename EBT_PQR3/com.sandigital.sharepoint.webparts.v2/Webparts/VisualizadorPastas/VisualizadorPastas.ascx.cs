using com.sandigital.sharepoint.webparts.v2.Componentes;
using Microsoft.SharePoint;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Web.UI.WebControls.WebParts;
using System.Linq;

namespace com.sandigital.sharepoint.webparts.v2.Webparts.VisualizadorPastas
{
    [ToolboxItemAttribute(false)]
    public partial class VisualizadorPastas : WebPart
    {
        #region Propriedades
        private string _strLista;

        [Category("Configurações")]
        [Personalizable(PersonalizationScope.Shared), WebBrowsable(true), WebDisplayName("Biblioteca"), WebDescription("Caminho Completo da Biblioteca de documentos")]
        [Browsable(true)]
        public string Lista
        {
            get { return _strLista; }
            set { _strLista = value; }
        }

        #endregion

        // Uncomment the following SecurityPermission attribute only when doing Performance Profiling on a farm solution
        // using the Instrumentation method, and then remove the SecurityPermission attribute when the code is ready
        // for production. Because the SecurityPermission attribute bypasses the security check for callers of
        // your constructor, it's not recommended for production purposes.
        // [System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.Assert, UnmanagedCode = true)]
        public VisualizadorPastas()
        {
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            InitializeControl();
        }

        string strAddLink = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(Lista))
                {
                    this.lblErro.Text = "Biblioteca não configurada. Edite a lista, configure a biblioteca";
                }

                if (!Page.IsPostBack)
                {
                    CarregaControle();
                }
            }
            catch (Exception ex)
            {
                lblErro.Text = "Erro ao carregar a webpart. o erro foi:  " + ex.Message;
            }

        }      

        protected void CarregaControle()
        {
            try
            {




                spsWrapper objWrapper = new spsWrapper();

                string strPasta = ViewState["pasta"] == null ?
                    SPContext.Current.Web.ServerRelativeUrl + "/" + Lista + "/"
                    : ViewState["pasta"].ToString();


                
              



                SPFolder objPasta = SPContext.Current.Web.GetFolder(strPasta);

                //Hack p o titulo da wp
                  this.lblHack.Text = string.Format("<div class='ag_hackTitulo' titulo='{0}' url='{1}'></div>",
                    this.Title,
                    objPasta.DocumentLibrary.DefaultViewUrl
                    );



                strAddLink = string.Format("{0}/_layouts/Upload.aspx?List={1}&amp;RootFolder={2}",
                    SPContext.Current.Web.Url,
                objPasta.DocumentLibrary.ID,
                objPasta.Url
                    );


                //Carrega o breadcrumb
                List<VisualizadorPastaBreadcrumbModel> objBreadCrumb = new List<VisualizadorPastaBreadcrumbModel>();

                SPFolder objPastaAtual = objPasta;
                while (objPastaAtual.ParentFolder.Name != "")
                {
                    objBreadCrumb.Add(new VisualizadorPastaBreadcrumbModel
                    {
                        name = objPastaAtual.Name,
                        url = objPastaAtual.Url
                    });

                    objPastaAtual = objPastaAtual.ParentFolder;

                }

                if (objBreadCrumb.Count > 0)
                {
                    objBreadCrumb.Insert(0, new VisualizadorPastaBreadcrumbModel { name = objPasta.DocumentLibrary.Title, url = objPasta.DocumentLibrary.RootFolder.Url });
                }

                this.rptCaminho.DataSource = objBreadCrumb.OrderBy(f => f.url);
                this.rptCaminho.DataBind();



                //Carrega a árvore
                List<VisualizadorPastasModel> objArvore = new List<VisualizadorPastasModel>();



                foreach (SPFolder pst in objPasta.SubFolders)
                {
                    if (pst.Name != "Forms")
                        objArvore.Add(new VisualizadorPastasModel
                        {
                            id = pst.UniqueId,
                            name = pst.Name,
                            docIcon = "/_layouts/images/folder.gif",
                            folder = pst.Name,
                            url = pst.Url,
                            tp = 0
                        });

                }

                foreach (SPFile arq in objPasta.Files)
                {
                    objArvore.Add(new VisualizadorPastasModel
                    {
                        id = arq.UniqueId,
                        name = arq.Name,
                        docIcon = "/_layouts/images/" + arq.IconUrl,
                        folder = arq.ParentFolder.Name,
                        url = arq.Url,
                        tp = 1
                    });

                }

                this.rptArquivos.DataSource = objArvore.OrderBy(f => f.tp.ToString() + f.name);
                this.rptArquivos.DataBind();

            }
            catch (Exception ex)
            {
                this.lblErro.Text= "Erro ao carregar a webpart. o erro foi: " + ex.Message;
            }
        }
        
        protected void lnkClick_Command(object sender, System.Web.UI.WebControls.CommandEventArgs e)
        {
            if (e.CommandName.ToString() == "1")//redireciona pro arquivo
            {
                Page.Response.Redirect(e.CommandArgument.ToString());
            }
            else //troca  a pasta
            {
                ViewState["pasta"] = e.CommandArgument;
                CarregaControle();

            }
        }





        #region inicio customização Toolbar


      


//            Protected Overrides Function CreateWebPartChrome() As 
//    System.Web.UI.WebControls.WebParts.WebPartChrome
//    Return New CustomisedWebPartChrome(Me, 
//        WebPartManager.GetCurrentWebPartManager(Me.Page))
//End Function

        
        
        
        #endregion












    }
}
