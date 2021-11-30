using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SeverinoConexao;
using SeverinosAPI.Models;

namespace SeverinosAPI.Controllers
{
    [Route("api/[controller]")]
    [Controller]
    public class LoginController : ControllerBase
    {
        // GET Login
        [HttpGet("{email}/{senha}")]
        public ActionResult<string> Get(String email, String senha)
        {
            try
            {
                var Login = new Login();

                string SelectPessoa =
                    $"select indseverino, seqPessoa from tb_pessoa where upper(email) = '{email.ToUpper()}' and upper(senha) = '{senha.ToUpper()}'";

                var pessoa = DBModel.GetReader(SelectPessoa);
                pessoa.Read();                

                if (pessoa.HasRows)
                {
                    Login.IndSeverino = Convert.ToBoolean(pessoa["indseverino"]) ? "S" : "N";
                    Login.SeqPessoa = Convert.ToInt32(pessoa["seqPessoa"]);

                    return System.Text.Json.JsonSerializer.Serialize(Login);
                }
                else
                {
                    Login.IndSeverino = "E";
                    Login.SeqPessoa = 0;

                    return System.Text.Json.JsonSerializer.Serialize(Login);
                }
            }
            finally
            {
                DBModel.Conexao.Close();
            }
        }

        // PUT Cadastro
        [HttpPut("{cpf}/{senhaNova}")]
        public ActionResult<Boolean> RecuperaSenha(string cpf, string senhaNova)
        {            
            string UpdateSenha =
                $"update tb_pessoa set senha = '{senhaNova}' where nrocpf = '{cpf}'";

            return DBModel.RunSqlNonQuery(UpdateSenha) > 0;            
        }
    }
}
