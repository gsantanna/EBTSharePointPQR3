using Microsoft.SharePoint;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Web.UI.WebControls.WebParts;

using System.Linq;
using Newtonsoft.Json;
using System.Text;

namespace com.sandigital.sharepoint.webparts.v2.Webparts.AgrupadoraPastas
{
    [ToolboxItemAttribute(false)]
    public partial class AgrupadoraPastas : WebPart
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


        string strGD = Guid.NewGuid().ToString().Replace("{", "").Replace("}", "");

        public string GetIdc()
        {
            return "dv_agrupadora_" +  strGD;

        }
        #endregion


        // Uncomment the following SecurityPermission attribute only when doing Performance Profiling on a farm solution
        // using the Instrumentation method, and then remove the SecurityPermission attribute when the code is ready
        // for production. Because the SecurityPermission attribute bypasses the security check for callers of
        // your constructor, it's not recommended for production purposes.
        // [System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.Assert, UnmanagedCode = true)]
        public AgrupadoraPastas()
        {
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            InitializeControl();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.lblErro.Text = "";

            try
            {
                if (string.IsNullOrEmpty(Lista))
                {
                    this.lblErro.Text = "Lista não configurada, edite a webpart e selecione o nome da lista";

                }
                else
                {
                    
                    if (!Page.IsPostBack) CarregaGrid();
                }
            }
            catch (Exception ex)
            {
                this.lblErro.Text = "Erro ao carregar a webpart, o erro foi: " + ex.Message ;
            }


        }

        List<AgrupadoraPastasPasta> Pastas = new List<AgrupadoraPastasPasta>();
        List<AgrupadoraPastasArquivo> Arquivos = new List<AgrupadoraPastasArquivo>();

        protected void CarregaGrid()
        {

            //abre a lista 
            SPList objLista = SPContext.Current.Web.Lists[Lista];

            this.ltDiv1.Text = GetIdc();
            this.ltDiv2.Text = "#"+GetIdc();


            //Hack p o titulo da wp
            this.lblHack.Text = string.Format("<div class='ag_hackTitulo' titulo='{0}' url='{1}'></div>",
              this.Title,
              objLista.DefaultViewUrl
              );




            foreach (SPListItem item in objLista.Items)
            {
                Arquivos.Add(new AgrupadoraPastasArquivo
                  {
                      id_arquivo = item.File.Url,
                      Icone = item.File.IconUrl,
                      Titulo = item.Name,
                      id_pasta = item.File.ParentFolder.Url,
                      url = item.File.ServerRelativeUrl
                  });

            }


            //Carrega as pastas
            foreach (SPListItem dir in objLista.Folders)
            {
                if(dir.DisplayName!="Forms")
                Pastas.Add(new AgrupadoraPastasPasta
                {
                    id_pasta = dir.Url,
                    Pai = dir.Folder.ParentFolder.Url,
                    Pasta = dir.DisplayName,
                    id_pai = dir.Folder.ParentFolder.Url,
                    Arquivos = new List<AgrupadoraPastasArquivo>(),
                    Pastas = new List<AgrupadoraPastasPasta>(),
                    nivel = dir.Folder.Url.Split('/').Length
                });
            }

            List<AgrupadoraPastasPasta> objSaida = new List<AgrupadoraPastasPasta>();

            int iP1 = 1;

            //Monta a 'arvore
            foreach (var objPasta in Pastas.Where(f => f.id_pai == objLista.RootFolder.Url))
            {

                AgrupadoraPastasPasta p1 = objPasta;
                p1.idx_pasta = iP1.ToString();
                p1.Arquivos = Arquivos.Where(f => f.id_pasta == p1.id_pasta).ToList();

                int iP2 = 1;


                foreach (var objPasta2 in Pastas.Where(f => f.id_pai == p1.id_pasta))
                {
                    AgrupadoraPastasPasta p2 = objPasta2;
                    p2.Arquivos = Arquivos.Where(f => f.id_pasta == p2.id_pasta).ToList();
                    p2.idx_pasta = p1.idx_pasta + "_" + iP2;
                    p2.idx_pai = p1.idx_pasta;

                    int iP3 = 1;




                    foreach (var objPasta3 in Pastas.Where(f => f.id_pai == p2.id_pasta))
                    {
                        AgrupadoraPastasPasta p3 = objPasta3;
                        p3.Arquivos = Arquivos.Where(f => f.id_pasta == p3.id_pasta).ToList();
                        p3.idx_pasta = p2.idx_pasta + "_" + iP3;
                        p3.idx_pai = p2.idx_pasta;
                        int iP4 = 1;


                        foreach (var objPasta4 in Pastas.Where(f => f.id_pai == p3.id_pasta))
                        {
                            AgrupadoraPastasPasta p4 = objPasta4;
                            p4.Arquivos = Arquivos.Where(f => f.id_pasta == p4.id_pasta).ToList();
                            p4.idx_pasta = p3.idx_pasta + "_" + iP4;
                            p4.idx_pai = p3.idx_pasta;
                            int iP5 = 1;


                            foreach (var objPasta5 in Pastas.Where(f => f.id_pai == p4.id_pasta))
                            {

                                AgrupadoraPastasPasta p5 = objPasta5;
                                p5.Arquivos = Arquivos.Where(f => f.id_pasta == p5.id_pasta).ToList();
                                p5.idx_pasta = p4.idx_pasta + "_" + iP5;
                                p5.idx_pai = p4.idx_pasta;
                                p4.Pastas.Add(p5);
                                iP5++;
                            }

                            p3.Pastas.Add(p4);
                            iP4++;
                        }

                        p2.Pastas.Add(p3);
                        iP3++;
                    }

                    p1.Pastas.Add(p2);
                    iP2++;
                }


                iP1++;
                objSaida.Add(p1);

            }



            StringBuilder strSaida = new StringBuilder();



            //Serializa a saida para json 
            string strJson =
            JsonConvert.SerializeObject(objSaida, Formatting.None);
            this.ltSaida.Text = strJson;






        }//CarregaGrid









    }
}
