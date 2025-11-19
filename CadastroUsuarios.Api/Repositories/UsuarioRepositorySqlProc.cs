using CadastroUsuarios.Shared.Interfaces;
using CadastroUsuarios.Shared.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace CadastroUsuarios.Api.Repositories
{
    public class UsuarioRepositorySqlProc : IUsuarioRepository
    {
        private readonly string _connString;

        public UsuarioRepositorySqlProc()
        {
            _connString = ConfigurationManager
                .ConnectionStrings["TestDb"]
                .ConnectionString;
        }

        public IEnumerable<Usuario> Listar()
        {
            var lista = new List<Usuario>();

            using (var conn = new SqlConnection(_connString))
            using (var cmd = new SqlCommand("usp_Usuarios_Listar", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                conn.Open();
                using (var rd = cmd.ExecuteReader())
                {
                    while (rd.Read())
                    {
                        lista.Add(new Usuario
                        {
                            Id       = rd.GetInt32(0),
                            Nome     = rd.GetString(1),
                            Cpf      = rd.GetString(2),
                            Telefone = rd.IsDBNull(3) ? string.Empty : rd.GetString(3)
                        });
                    }
                }
            }

            return lista;
        }

        public Usuario Obter(int id)
        {
            using (var conn = new SqlConnection(_connString))
            using (var cmd = new SqlCommand("usp_Usuarios_Obter", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("@Id", SqlDbType.Int).Value = id;

                conn.Open();
                using (var rd = cmd.ExecuteReader())
                {
                    if (rd.Read())
                    {
                        return new Usuario
                        {
                            Id       = rd.GetInt32(0),
                            Nome     = rd.GetString(1),
                            Cpf      = rd.GetString(2),
                            Telefone = rd.IsDBNull(3) ? string.Empty : rd.GetString(3)
                        };
                    }
                }
            }

            return null;
        }

        public void Adicionar(Usuario usuario)
        {
            using (var conn = new SqlConnection(_connString))
            using (var cmd = new SqlCommand("usp_Usuarios_Adicionar", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("@Nome", SqlDbType.NVarChar, 100).Value = usuario.Nome;
                cmd.Parameters.Add("@Cpf", SqlDbType.Char, 11).Value = usuario.Cpf;
                cmd.Parameters.Add("@Telefone", SqlDbType.VarChar, 20).Value =
                    (object)usuario.Telefone ?? DBNull.Value;

                conn.Open();
                var idGerado = Convert.ToInt32(cmd.ExecuteScalar());
                usuario.Id = idGerado;
            }
        }

        public void Atualizar(Usuario usuario)
        {
            using (var conn = new SqlConnection(_connString))
            using (var cmd = new SqlCommand("usp_Usuarios_Atualizar", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("@Id", SqlDbType.Int).Value = usuario.Id;
                cmd.Parameters.Add("@Nome", SqlDbType.NVarChar, 100).Value = usuario.Nome;
                cmd.Parameters.Add("@Cpf", SqlDbType.Char, 11).Value = usuario.Cpf;
                cmd.Parameters.Add("@Telefone", SqlDbType.VarChar, 20).Value =
                    (object)usuario.Telefone ?? DBNull.Value;

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void Excluir(int id)
        {
            using (var conn = new SqlConnection(_connString))
            using (var cmd = new SqlCommand("usp_Usuarios_Excluir", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("@Id", SqlDbType.Int).Value = id;

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }
    }
}