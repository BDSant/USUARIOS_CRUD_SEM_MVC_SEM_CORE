<%@ Page Language="C#" AutoEventWireup="true"
    CodeBehind="Usuarios.aspx.cs"
    Inherits="WebUsuarios.CadastroUsuarios.Usuarios" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Cadastro de Usuários</title>

    <!-- Bootstrap e jQuery (pode usar outras versões se preferir) -->
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap@4.6.2/dist/css/bootstrap.min.css" />
    <script src="https://code.jquery.com/jquery-3.6.4.min.js"></script>
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@4.6.2/dist/js/bootstrap.bundle.min.js"></script>

    <script type="text/javascript">
        // base da API vinda do Web.config
        //var apiBaseUrl = '<%= System.Configuration.ConfigurationManager.AppSettings["ApiBaseUrl"].TrimEnd('/') %>';

        function abrirPopupNovo() {
            // zera id e limpa campos
            $('#<%= hfId.ClientID %>').val('0');
            $('#<%= txtNome.ClientID %>').val('');
            $('#<%= txtCpf.ClientID %>').val('');
            $('#<%= txtTelefone.ClientID %>').val('');
            $('#tituloModal').text('Novo Usuário');
            $('#modalEditar').modal('show');
        }

        function abrirPopupEditar(id, nome, cpf, telefone) {
            $('#<%= hfId.ClientID %>').val(id);
            $('#<%= txtNome.ClientID %>').val(nome);
            $('#<%= txtCpf.ClientID %>').val(cpf);
            $('#<%= txtTelefone.ClientID %>').val(telefone);
            $('#tituloModal').text('Alterar Usuário');
            $('#modalEditar').modal('show');
        }

        function abrirPopupExcluir(id) {
            $('#idExcluir').val(id);
            $('#msgExcluir').text('Confirma exclusão do id ' + id + ' ?');
            $('#modalExcluir').modal('show');
        }


        function confirmarExclusao() {
            var id = $('#idExcluir').val();

            $.ajax({
                type: "POST",
                url: "Usuarios.aspx/ExcluirUsuario",
                data: JSON.stringify({ id: parseInt(id) }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (res) {
                    // ASP.NET WebMethod retorna algo do tipo { "d": "OK" }
                    var result = res.d;

                    if (result === "OK") {
                        $('#modalExcluir').modal('hide');
                        alert('Excluído com sucesso!');
                        location.reload();
                    } else {
                        alert("Erro retornado: " + result);
                    }
                },
                error: function (xhr, status, error) {
                    console.error("AJAX ERROR status:", xhr.status, "resp:", xhr.responseText);
                    alert("Falha ao excluir usuário. Status: " + xhr.status);
                }
            });
        }





    </script>
</head>
<body>
    <form id="form1" runat="server" class="container mt-4">
        <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true" />

        <h2>Cadastro de Usuários</h2>

        <!-- Botão Novo antes do Repeater -->
        <div class="mb-3">
            <button type="button" class="btn btn-success" onclick="abrirPopupNovo();">
                Novo Usuário
            </button>
        </div>

        <!-- Tabela + Repeater -->
        <table class="table table-bordered table-striped">
            <thead class="thead-dark">
                <tr>
                    <th>Id</th>
                    <th>Nome</th>
                    <th>CPF</th>
                    <th>Telefone</th>
                    <th style="width: 160px;">Ações</th>
                </tr>
            </thead>
            <tbody>
                <asp:Repeater ID="rptUsuarios" runat="server">
                    <ItemTemplate>
                        <tr>
                            <td><%# Eval("Id") %></td>
                            <td><%# Eval("Nome") %></td>
                            <td><%# Eval("Cpf") %></td>
                            <td><%# Eval("Telefone") %></td>
                            <td>
                                <!-- Botão Alterar -->
                                <button type="button"
                                    class="btn btn-sm btn-primary mr-1"
                                    onclick="abrirPopupEditar(
                                            '<%# Eval("Id") %>',
                                            '<%# System.Web.HttpUtility.JavaScriptStringEncode(Eval("Nome").ToString().Trim()) %>',
                                            '<%# System.Web.HttpUtility.JavaScriptStringEncode(Eval("Cpf").ToString().Trim()) %>',
                                            '<%# System.Web.HttpUtility.JavaScriptStringEncode(Eval("Telefone").ToString().Trim()) %>'
                                         );">
                                    Alterar
                                </button>

                                <!-- Botão Excluir -->
                                <button type="button"
                                    class="btn btn-sm btn-danger"
                                    onclick="abrirPopupExcluir('<%# Eval("Id") %>');">
                                    Excluir
                                </button>
                            </td>
                        </tr>
                    </ItemTemplate>
                </asp:Repeater>
            </tbody>
        </table>

        <!-- Modal Editar / Novo -->
        <div class="modal fade" id="modalEditar" tabindex="-1" role="dialog"
            aria-labelledby="tituloModal" aria-hidden="true">
            <div class="modal-dialog" role="document">
                <div class="modal-content">
                    <div class="modal-header">
                        <h5 class="modal-title" id="tituloModal">Editar Usuário</h5>
                        <button type="button" class="close" data-dismiss="modal"
                            aria-label="Close">
                            <span aria-hidden="true">&times;</span>
                        </button>
                    </div>
                    <div class="modal-body">

                        <asp:HiddenField ID="hfId" runat="server" />

                        <div class="form-group">
                            <label for="<%= txtNome.ClientID %>">Nome</label>
                            <asp:TextBox ID="txtNome" runat="server"
                                CssClass="form-control" />
                        </div>

                        <div class="form-group">
                            <label for="<%= txtCpf.ClientID %>">CPF</label>
                            <asp:TextBox ID="txtCpf" runat="server"
                                CssClass="form-control" MaxLength="11" />
                        </div>

                        <div class="form-group">
                            <label for="<%= txtTelefone.ClientID %>">Telefone</label>
                            <asp:TextBox ID="txtTelefone" runat="server"
                                CssClass="form-control" />
                        </div>
                    </div>
                    <div class="modal-footer">
                        <asp:Button ID="btnSalvar" runat="server"
                            Text="OK"
                            CssClass="btn btn-primary"
                            OnClick="btnSalvar_Click" />
                        <button type="button" class="btn btn-secondary"
                            data-dismiss="modal">
                            Cancelar</button>
                    </div>
                </div>
            </div>
        </div>

        <!-- Modal Excluir -->
        <div class="modal fade" id="modalExcluir" tabindex="-1" role="dialog"
            aria-labelledby="tituloExcluir" aria-hidden="true">
            <div class="modal-dialog" role="document">
                <div class="modal-content">
                    <div class="modal-header">
                        <h5 class="modal-title" id="tituloExcluir">Confirmar Exclusão</h5>
                        <button type="button" class="close" data-dismiss="modal"
                            aria-label="Close">
                            <span aria-hidden="true">&times;</span>
                        </button>
                    </div>
                    <div class="modal-body">
                        <p id="msgExcluir"></p>
                        <input type="hidden" id="idExcluir" />
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-danger"
                            onclick="confirmarExclusao();">
                            Sim
                        </button>
                        <button type="button" class="btn btn-secondary"
                            data-dismiss="modal">
                            Não
                        </button>
                    </div>
                </div>
            </div>
        </div>

        <!-- Modal Erro -->
        <div class="modal fade" id="modalErro" tabindex="-1">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header bg-danger text-white">
                        <h5 class="modal-title">Erro</h5>
                        <button type="button" class="close" data-dismiss="modal">
                            <span>&times;</span>
                        </button>
                    </div>
                    <div class="modal-body">
                        <asp:Label ID="lblErroModal" runat="server"></asp:Label>
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-danger" data-dismiss="modal">OK</button>
                    </div>
                </div>
            </div>
        </div>

        <!-- Modal Sucesso -->
        <div class="modal fade" id="modalSucesso" tabindex="-1">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header bg-success text-white">
                        <h5 class="modal-title">Sucesso</h5>
                        <button type="button" class="close" data-dismiss="modal">
                            <span>&times;</span>
                        </button>
                    </div>
                    <div class="modal-body">
                        <asp:Label ID="lblSucessoModal" runat="server"></asp:Label>
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-success" data-dismiss="modal">
                            OK
                        </button>
                    </div>
                </div>
            </div>
        </div>


    </form>
</body>
</html>

