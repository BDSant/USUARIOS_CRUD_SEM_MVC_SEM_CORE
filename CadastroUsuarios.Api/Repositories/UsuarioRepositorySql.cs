using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using CadastroUsuarios.Shared.Interfaces;
using CadastroUsuarios.Shared.Models;

namespace CadastroUsuarios.Api.Repositories
{
    public class UsuarioRepositorySql : IUsuarioRepository
    {
        private readonly string _connString;

        public UsuarioRepositorySql()
        {
            _connString = ConfigurationManager
                .ConnectionStrings["TestDb"]
                .ConnectionString;
        }

        public IEnumerable<Usuario> Listar()
        {
            var lista = new List<Usuario>();

            using (var conn = new SqlConnection(_connString))
            using (var cmd = new SqlCommand(
                "SELECT Id, Nome, Cpf, Telefone FROM dbo.Usuarios ORDER BY Id", conn))
            {
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
            using (var cmd = new SqlCommand(
                "SELECT Id, Nome, Cpf, Telefone FROM dbo.Usuarios WHERE Id = @Id", conn))
            {
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
            using (var cmd = new SqlCommand(
                @"INSERT INTO dbo.Usuarios (Nome, Cpf, Telefone)
                  VALUES (@Nome, @Cpf, @Telefone);
                  SELECT SCOPE_IDENTITY();", conn))
            {
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
            using (var cmd = new SqlCommand(
                @"UPDATE dbo.Usuarios
                  SET Nome = @Nome,
                      Cpf = @Cpf,
                      Telefone = @Telefone
                  WHERE Id = @Id", conn))
            {
                cmd.Parameters.Add("@Nome", SqlDbType.NVarChar, 100).Value = usuario.Nome;
                cmd.Parameters.Add("@Cpf", SqlDbType.Char, 11).Value = usuario.Cpf;
                cmd.Parameters.Add("@Telefone", SqlDbType.VarChar, 20).Value = (object)usuario.Telefone ?? DBNull.Value;
                cmd.Parameters.Add("@Id", SqlDbType.Int).Value = usuario.Id;

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void Excluir(int id)
        {
            using (var conn = new SqlConnection(_connString))
            using (var cmd = new SqlCommand(
                "DELETE FROM dbo.Usuarios WHERE Id = @Id", conn))
            {
                cmd.Parameters.Add("@Id", SqlDbType.Int).Value = id;

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }
    }
}
