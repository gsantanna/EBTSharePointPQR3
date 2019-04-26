using com.sandigital.sharepoint.webparts.v2.Componentes;
using Microsoft.SharePoint;
using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Linq;


namespace com.sandigital.sharepoint.webparts.v2.ControlTemplates.com.sandigital.sharepoint.webparts.v2
{
    public partial class SubmenuComunidades : UserControl
    {        
        public string Lista { get; set; }
        
        protected void Page_Load(object sender, EventArgs e)
        {

            //carrega o menu superior
            if (string.IsNullOrEmpty(Lista))
            {
                this.lblErro.Visible = true;
                this.lblErro.Text = "Erro, lista não configurada!";
                return;
            }
            else
            {

                if (!IsPostBack)
                {
                    CarregaMenu();

                }

            }


        }
        
        private string getCssClass(string Link)
        {
            if (
                Request.Url.LocalPath.ToUpper() == Link.ToUpper())
            {
                return "active";
            }
            else
            {
                return "inativo";
            }
        }

        public void CarregaMenu()
        {
            //tenta carregar a lista configurada.
            spsWrapper objSps = new spsWrapper(true);

            try
            {
                var objDs = objSps.RetornaListaPorUrl(Lista, "<Orderby><FieldRef Name='Ordem'/></Orderby>").Select(sp =>
                    new
                    {
                        Link = Convert.ToString(sp["Link"]),
                        Texto = Convert.ToString(sp["Texto"]),
                        Cor =  (sp["COR"]),
                        Classe = getCssClass(Convert.ToString(sp["Link"]))
                    });

                this.rptMain.DataSource = objDs;
                this.rptMain.DataBind();

            }
            catch (Exception ex)
            {
                this.lblErro.Text = "<!-- Erro ao carregar este controle, o erro foi: " + ex.Message + "-->";

            }


        }


    }
}
