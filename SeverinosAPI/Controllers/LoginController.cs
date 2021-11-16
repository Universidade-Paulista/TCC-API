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
        // GET api/Login
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            DBModel.GetConexao();

            var pessoa = DBModel.GetReader("select nome from tb_pessoa ");

            pessoa.Read();
            var tes = pessoa["nome"].ToString();

            return new string[] { pessoa["nome"].ToString() };            
        }

        // GET api/Login
        [HttpGet("{email}/{senha}/{indseverino}")]
        public ActionResult<string> Gettest(String email, String senha, bool indseverino)
        {
            DBModel.GetConexao();
            var test = DBModel.GetReader($"select nome from tb_pessoa where email = '{email.ToUpper()}' and senha = '{senha.ToUpper()}' and indseverino = '{indseverino}' ");
            test.Read();

            var Login = new Cadastro
            {
                Email = test["Email"].ToString(),
                Senha = test["Senha"].ToString(),
                IndSeverino = Boolean.Parse(test["IndSeverino"].ToString()),

            };

            if (Login.IndSeverino == true)
            {

            }

            return System.Text.Json.JsonSerializer.Serialize(Login);
        }

        // GET login/Login/5
        [HttpGet("{email}/{senha}")]
        public ActionResult<string> Get(String email, String senha)
        {
            DBModel.GetConexao();

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

        // POST login/Login
        [HttpPost]
        public void Post([FromBody] string jsonString)
        {                          
            var myJsonObj = JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(jsonString.ToUpper());

            int codigo = Int32.Parse(myJsonObj["CODIGO"]);
            var login  = myJsonObj["LOGIN"];
            var senha  = myJsonObj["SENHA"];

            string sql = $"insert into tb_login(seqpessoa, login, senha) values({codigo}, '{login}', '{senha}')";

            DBModel.RunSqlNonQuery(sql);
        }

        // PUT login/Login/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE login/Login/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
