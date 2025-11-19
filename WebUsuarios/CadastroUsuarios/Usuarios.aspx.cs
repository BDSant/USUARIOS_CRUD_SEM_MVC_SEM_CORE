using CadastroUsuarios.Shared.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net.Http;
using System.Text;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;

namespace WebUsuarios.CadastroUsuarios
{
    public partial class Usuarios : Page
    {
        private static readonly HttpClient httpClient = new HttpClient();

        private static string ApiBaseUrl
        {
            get
            {
                var url = ConfigurationManager.AppSettings["ApiBaseUrl"] ?? "";
                return url.TrimEnd('/');
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CarregarUsuarios();
            }
        }

        private void CarregarUsuarios()
        {
            try
            {
                var response = httpClient.GetAsync(ApiBaseUrl + "/api/usuarios").Result;

                if (response.IsSuccessStatusCode)
                {
                    var json = response.Content.ReadAsStringAsync().Result;
                    var lista = JsonConvert.DeserializeObject<List<Usuario>>(json);

                    rptUsuarios.DataSource = lista;
                    rptUsuarios.DataBind();
                }
                else
                {
                    lblErroModal.Text = "Erro ao carregar usuários: " + response.ReasonPhrase;
                    ScriptManager.RegisterStartupScript(this, GetType(), "ShowErro", "$('#modalErro').modal('show');", true);
                    return;
                }
            }
            catch (Exception ex)
            {
                // logar / tratar ex
            }
        }

        protected void btnSalvar_Click(object sender, EventArgs e)
        {
            try
            {
                int id;
                int.TryParse(hfId.Value, out id);

                var usuario = new Usuario
                {
                    Id = id,
                    Nome = txtNome.Text,
                    Cpf = txtCpf.Text,
                    Telefone = txtTelefone.Text
                };

                var json = JsonConvert.SerializeObject(usuario);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                HttpResponseMessage response;

                if (id == 0)
                {
                    // Inserir
                    response = httpClient.PostAsync(ApiBaseUrl + "/api/usuarios", content).Result;
                }
                else
                {
                    // Atualizar
                    response = httpClient.PutAsync(ApiBaseUrl + "/api/usuarios/" + id, content).Result;
                }

                if (response.IsSuccessStatusCode)
                {
                    // Atualiza a lista após salvar
                    CarregarUsuarios();

                    lblSucessoModal.Text = "Usuário salvo com sucesso!";
                    ScriptManager.RegisterStartupScript(this, GetType(), "ShowSucesso", "$('#modalSucesso').modal('show');", true);
                }
                else
                {
                    lblErroModal.Text = "Erro ao salvar usuários: " + response.ReasonPhrase;
                    ScriptManager.RegisterStartupScript(this, GetType(), "ShowErro", "$('#modalErro').modal('show');", true);
                }
            }
            catch (Exception ex)
            {
                // logar / tratar ex
            }
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static string ExcluirUsuario(int id)
        {
            try
            {
                var response = httpClient.DeleteAsync(ApiBaseUrl + "/api/usuarios/" + id).Result;

                if (response.IsSuccessStatusCode)
                    return "OK";

                var body = response.Content.ReadAsStringAsync().Result;
                return $"Erro ao excluir. Status: {(int)response.StatusCode} - {response.ReasonPhrase}. Detalhes: {body}";
            }
            catch (Exception ex)
            {
                return "Erro: " + ex.Message;
            }
        }



    }
}