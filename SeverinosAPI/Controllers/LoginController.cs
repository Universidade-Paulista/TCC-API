using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SeverinoConexao;
using SeverinosAPI.Models;
using Npgsql;

namespace SeverinosAPI.Controllers
{
    [Route("api/[controller]")]
    [Controller]
    public class LoginController : ControllerBase
    {
        private NpgsqlConnection Conexao;

        // GET Login
        [HttpGet("{email}/{senha}")]
        public ActionResult<string> Get(String email, String senha)
        {
            try 
            {
                Conexao = DBModel.GetConexao();

                string SelectPessoa =
                    $"select indseverino from tb_pessoa where upper(email) = '{email.ToUpper()}' and upper(senha) = '{senha.ToUpper()}'";

                var pessoa = DBModel.GetReader(SelectPessoa);
                pessoa.Read();

                if (pessoa.HasRows)
                {
                    return Convert.ToBoolean(pessoa["indseverino"]) ? "S" : "N";
                }
                else
                {
                    return "E";
                }
            }
            finally
            {
                Conexao.Close();
            }                        
        }

        // PUT Cadastro
        [HttpPut("{cpf}/{senhaNova}")]
        public ActionResult<Boolean> RecuperaSenha(string cpf, string senhaNova)
        {
            try
            {
                Conexao = DBModel.GetConexao();

                string UpdateSenha =
                    $"update tb_pessoa set senha = '{senhaNova}' where nrocpf = '{cpf}'";

                return DBModel.RunSqlNonQuery(UpdateSenha) > 0;
            }
            finally
            {
                Conexao.Close();
            }            
        }
    }
}
