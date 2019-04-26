using Microsoft.SharePoint;
using System;
using System.ComponentModel;
using System.Web.UI.WebControls.WebParts;
using System.Linq;


namespace com.sandigital.sharepoint.webparts.v2.Webparts.Alertas
{
    [ToolboxItemAttribute(false)]
    public partial class Alertas : WebPart
    {

        #region Propriedades
        private string _strLista;

        [Category("Configurações")]
        [Personalizable(PersonalizationScope.Shared), WebBrowsable(true),
WebDisplayName("Caminho Completo da Lista"),
WebDescription("Caminho Completo da Lista")]
        [DefaultValue("/Lists/Alertas")]
        [Browsable(true)]
        public string Lista
        {
            get { return string.IsNullOrEmpty(_strLista) ? "/Lists/Alertas" : _strLista; }
            set { _strLista = value; }
        }

        #endregion


        public Alertas()
        {
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            InitializeControl();
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            this.lblErro.Visible = false;

            if (string.IsNullOrEmpty(Lista))
            {
                this.lblErro.Visible = true;
                this.lblErro.Text = "Lista não configurada";
                return;
            }


            CarregAlertas();




        }

        private void CarregAlertas()
        {

            //Carrega a lista de alertas 
            string strQuery = string.Format(@"
            <Orderby><FieldRef Name='Ordem'/></Orderby>
            <Where>
                <And>
                    <Contains>
                        <FieldRef Name='Comunidades' />
                        <Value Type='Text'>{0}</Value>
                    </Contains>
                <And>
                <Leq>
                    <FieldRef Name='Inicio' /><Value Type='DateTime' IncludeTimeValue='TRUE'><Today/></Value>
                </Leq>
                <Geq>
                    <FieldRef Name='Expires' /> <Value Type='DateTime' IncludeTimeValue='TRUE'><Today/></Value>
                </Geq> 
                </And>
                </And>
            </Where>", SPContext.Current.Web.Description);


            //Carrega a lista
            Componentes.spsWrapper objWrapper = new Componentes.spsWrapper();

            try
            {
                var ds =
                objWrapper.RetornaListaPorUrl(this.Lista, strQuery).Select(f => new { Title = f.Title, Body = f["Body"] });
                this.rptMain.DataSource = ds;
                this.rptMain.DataBind();

                this.pnlMain.Visible = ds.Count() > 0;


            }
            catch (Exception ex)
            {
                this.lblErro.Text = "Erro ao carregar esta webpart. o erro foi: " + ex.Message;
                this.lblErro.Visible = true;
            }






        }
    }
}
