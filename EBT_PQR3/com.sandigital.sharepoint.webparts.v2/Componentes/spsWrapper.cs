using Microsoft.SharePoint;
using Microsoft.SharePoint.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.IO;
using Microsoft.SharePoint.Administration;
using System.Web;
using System.Net;


namespace com.sandigital.sharepoint.webparts.v2.Componentes
{
    public class spsWrapper
    {
        public string Erro { get; set; }

        public SPSite Site { get; private set; }

        public spsWrapper(bool DontUseSystemAccount)
        {
            if (DontUseSystemAccount)
            {
                Site = SPContext.Current.Site;
            }
            else
            {

                //Cria o contexto com privilégios elevados
                SPSecurity.RunWithElevatedPrivileges(delegate
                {
                    Site = new SPSite(SPContext.Current.Site.ID);

                });
            }
        }

        public spsWrapper()
        {
            //Cria o contexto com privilégios elevados
            SPSecurity.RunWithElevatedPrivileges(delegate
          {
              Site = new SPSite(SPContext.Current.Site.ID);

          });
        }


        public spsWrapper(string strUrlSite)
        {
            SPSecurity.RunWithElevatedPrivileges(delegate
            {
                Site = new SPSite(strUrlSite);

            });

        }





        public SPListItem RetornaItem(string NomeLista, int id_ListItem, Guid _Site)
        {
            try
            {
                return Site.AllWebs[_Site].Lists[NomeLista].GetItemById(id_ListItem);
            }
            catch (Exception ex)
            {
                this.Erro = ex.Message;
                return null;
            }

        }


        public string RetornaUrlLista(Guid gdSite, Guid id)
        {
            SPList objLista = Site.AllWebs[gdSite].Lists[id];
            return objLista.DefaultViewUrl;
        }

        public SPListItemCollection RetornaLista(Guid gdSite, Guid id)
        {
            try
            {
                SPList objLista = Site.AllWebs[gdSite].Lists[id];


                return objLista.Items;
            }
            catch
            {
                return null;
            }
        }


        public List<SPListItem> RetornaLista(Guid id, string[] Campos, Guid gdSite)
        {
            SPList objLista = Site.AllWebs[gdSite].Lists[id];

            List<string> tmp = new List<string>();
            tmp.AddRange(Campos);
            tmp.Add("Modified");
            tmp.Add("Editor");
            tmp.Add("Created");
            tmp.Add("Author");

            return RetornaLista(objLista.Title, tmp.ToArray(), gdSite);

        }
        public List<SPListItem> RetornaLista(string Lista, string[] Campos, Guid gdSite)
        {
            string strViewFields = "";
            foreach (string campo in Campos)
            {
                strViewFields += "<FieldRef Name=\"" + campo + "\" />";
            }

            return RetornaLista(Lista, string.Empty, strViewFields, gdSite);
        }
        public List<SPListItem> RetornaLista(string Lista, string Query, string ViewFields, Guid gdSite)
        {

            return RetornaLista(Lista, Query, ViewFields, 0, gdSite);


        }
        public List<SPListItem> RetornaLista(string Lista, string Query, string ViewFields, uint Limite, Guid gdSite)
        {
            SPList objLista = Site.AllWebs[gdSite].Lists[Lista];


            string sQuery = Query;
            string sViewFields = ViewFields;
            string sViewAttrs = @"Scope=""Recursive""";

            var oQuery = new SPQuery();
            oQuery.ExpandRecurrence = true;
            oQuery.IncludeAttachmentUrls = true;
            oQuery.Query = sQuery;
            oQuery.ViewFields = sViewFields;
            oQuery.ViewAttributes = sViewAttrs;
            oQuery.RowLimit = Limite;
            oQuery.IncludeMandatoryColumns = true;

            SPListItemCollection collListItems = objLista.GetItems(oQuery);

            List<SPListItem> objSaida = new List<SPListItem>();

            foreach (SPListItem oListItem in collListItems)
            {
                objSaida.Add(oListItem);
            }

            return objSaida;
        }
       
        public List<SPListItem> RetornaListaPorUrl(string urlLista)
        {

            List<SPListItem> objSaida = new List<SPListItem>();

            SPList objLista=
            Site.RootWeb.GetListFromUrl(urlLista);


            foreach (SPListItem item in objLista.Items)
            {
                objSaida.Add(item);
            }

            return objSaida;



        }
       
        

        public List<SPListItem> RetornaListaPorUrl(string urlLista, string strQuery)
        {

            List<SPListItem> objSaida = new List<SPListItem>();

            SPList objLista =
            Site.RootWeb.GetList(urlLista);

            SPQuery objQuery = new SPQuery { Query = strQuery };


            foreach (SPListItem item in objLista.GetItems(objQuery))
            {
                objSaida.Add(item);
            }

            return objSaida;



        }





        public List<SPListItem> RetornaEventosRecorrentes(string Lista, string ViewFields, Guid gdSite)
        {
            throw new NotImplementedException();

        }







        public bool AdicionaAnexo(string NomeLista, int id_ListItem, string NomeArquivo, byte[] Arquivo, Guid gdSite)
        {
            try
            {
                //Site
                SPWeb objSite = Site.AllWebs[gdSite];
                if (objSite == null) throw new Exception("Site não encontrdo!");

                objSite.AllowUnsafeUpdates = true;

                //Lista
                SPList Lista = objSite.Lists[NomeLista];
                if (Lista == null) throw new Exception("Lista não encontrdo!");

                //Item 
                SPListItem item = Lista.Items.GetItemById(id_ListItem);
                if (item == null) throw new Exception("Item não encontrdo!");

                //Adiciona o anexo 
                item.Attachments.AddNow(NomeArquivo, Arquivo);

                return true;
            }
            catch (Exception ex)
            {
                this.Erro = ex.Message;
                return false;
            }


        }

        public bool LimpaAnexos(string NomeLista, int id_ListItem, Guid gdSite)
        {
            try
            {
                //Site
                SPWeb objSite = Site.AllWebs[gdSite];
                if (objSite == null) throw new Exception("Site não encontrdo!");

                objSite.AllowUnsafeUpdates = true;

                //Lista
                SPList Lista = objSite.Lists[NomeLista];
                if (Lista == null) throw new Exception("Lista não encontrdo!");

                //Item 
                SPListItem item = Lista.Items.GetItemById(id_ListItem);
                if (item == null) throw new Exception("Item não encontrdo!");

                //Para cada attachament deleta 
                while (item.Attachments.Count > 0)
                {
                    item.Attachments.DeleteNow(item.Attachments[0]);
                }






                return true;
            }
            catch (Exception ex)
            {
                this.Erro = ex.Message;
                return false;
            }


        }


        public SPListItem AdicionaItemLista(string NomeLista, Dictionary<string, object> Valores)
        {

            SPWeb objSite = Site.AllWebs[SPContext.Current.Web.ID];

            SPListItemCollection objLista = objSite.Lists[NomeLista].Items;

            //monta o Item da lista 
            SPListItem objItem = objLista.Add();

            //para item no dicionario 
            foreach (var i in Valores)
            {
                objItem[i.Key] = i.Value;

            }

            objItem.Web.AllowUnsafeUpdates = true;

            objItem.Update();

            return objItem;




        }

        public void AtualizaItemLista(string NomeLista, int idListItem, Dictionary<string, object> Valores)
        {
            SPWeb objSite = Site.AllWebs[SPContext.Current.Web.ID];
            SPListItemCollection objLista = objSite.Lists[NomeLista].Items;
            SPListItem objItem = objLista.GetItemById(idListItem);

            //para item no dicionario 
            foreach (var i in Valores)
            {
                objItem[i.Key] = i.Value;

            }

            objItem.Web.AllowUnsafeUpdates = true;
            objItem.Update();





        }


        public bool DeletaItem(string NomeLista, int id)
        {
            try
            {

                //SPListItem objItem = SPContext.Current.Web.Lists[NomeLista].GetItemById(id);
                SPListItem objItem = Site.AllWebs[SPContext.Current.Web.ID].Lists[NomeLista].GetItemById(id);


                objItem.Web.AllowUnsafeUpdates = true;
                objItem.Delete();
                objItem.Update();

                return true;
            }
            catch (Exception ex)
            {
                Erro = ex.Message;
                return false;

            }


        }

        public Dictionary<Guid, String> GetCampos(Guid gdLista)
        {

            var ret = new Dictionary<Guid, string>();

            foreach (SPList campo in (Site.AllWebs[SPContext.Current.Web.ID].Lists[gdLista].Fields))
            {
                ret.Add(campo.ID, campo.Title);
            }
            return ret;

        }
        public Dictionary<Guid, string> GetLists(Guid gdSite)
        {
            var ret = new Dictionary<Guid, string>();

            foreach (SPList lst in (Site.AllWebs[SPContext.Current.Web.ID].Lists))
            {
                ret.Add(lst.ID, lst.Title);
            }
            return ret;

        }

        public SPList GetList(Guid gdLista, Guid gdSite)
        {
            return Site.AllWebs[SPContext.Current.Web.ID].Lists[gdLista];
        }


        public SPDocumentLibrary GetLib(string NomeBiblioteca, Guid gdSite)
        {
            SPWeb objSite = Site.AllWebs[SPContext.Current.Web.ID];
            SPDocumentLibrary objRet = (SPDocumentLibrary)objSite.Lists[NomeBiblioteca];
            return objRet;
        }

        




        #region Mapping
        #endregion
    }



    public static class Utilidades
    {

        public static bool ArquivoExiste(string url)
        {
            bool retorno = false;

            try
            {
                HttpWebResponse response = null;
                var request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "HEAD";
                response = (HttpWebResponse)request.GetResponse();
                retorno = (response.StatusCode == HttpStatusCode.OK);

            }
            catch  
            {
                retorno = false;
            }


            return retorno;


        }

        public static string NomeUsuario(string strIdUsuario)
        {
            try
            {

                SPUser usr = SPContext.Current.Web.EnsureUser(strIdUsuario);

                if (usr.ID == SPContext.Current.Web.Site.SystemAccount.ID)   //web.Site.SystemAccount.ID)
                {
                    return "Conta de Sistema";
                }
                else
                {
                    return usr.Name;
                }
            }
            catch
            {
                return strIdUsuario;
            }


        }

        public static string MontaLinksAnexo(SPAttachmentCollection anexos)
        {

            if (anexos == null)
            {
                return string.Empty;
            }
            else
            {
                StringBuilder strSaida = new StringBuilder();

                foreach (var anexo in anexos)
                {
                    strSaida.AppendFormat("<a href=\"{0}{1}\">{1}</a><br/>", anexos.UrlPrefix, anexo);

                }

                return strSaida.ToString();


            }




        }



        public static string FormataDataSPSISO(DateTime dt)
        {
            return SPUtility.CreateISO8601DateTimeFromSystemDateTime(dt);
        }

        public static string NomeSite()
        {
            return SPContext.Current.Web.Title;

        }

        public static string UrlBase()
        {
            return SPContext.Current.Web.Url;
        }


        public static void EnviaEmail(string strPara, string strAssunto, string strCorpo, Dictionary<string, byte[]> Anexos)
        {

            //Envia o e-mail 
            string strSMTP = SPAdministrationWebApplication.Local.OutboundMailServiceInstance.Server.Address;
            string strDE = SPAdministrationWebApplication.Local.OutboundMailSenderAddress;


            MailMessage objMensagem = new MailMessage(strDE, strPara);
            objMensagem.IsBodyHtml = true;
            objMensagem.Subject = strAssunto;
            objMensagem.Body = strCorpo;


            foreach (var anexo in Anexos)
            {
                objMensagem.Attachments.Add(new Attachment(new MemoryStream(anexo.Value), anexo.Key));
            }

            SmtpClient objSmtp = new SmtpClient(strSMTP);
            objSmtp.Send(objMensagem);


        }



    }



}
