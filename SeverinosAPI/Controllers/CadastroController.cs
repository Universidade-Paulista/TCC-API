using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SeverinoConexao;
using System;
using System.Text.Json;
using System.Collections.Generic;
using SeverinosAPI.Models;

namespace SeverinosAPI.Controllers
{   
    [Route("api/[controller]")]
    [Controller]
    public class CadastroController
    {        
        // GET api/Cadastro
        [HttpGet("{idPessoa}")]
        public ActionResult<string> Get(int idPessoa)
        {
            DBModel.GetConexao();          
            var Pessoa = DBModel.GetReader($"select * from tb_pessoa tp where tp.seqpessoa = {idPessoa}");
            Pessoa.Read();

            var Cadastro = new Cadastro
            {   
                SeqPessoa = Int32.Parse(Pessoa["SeqPessoa"].ToString()),
                Nome = Pessoa["Nome"].ToString(),
                NroCPF = Pessoa["NroCPF"].ToString(),
                Email = Pessoa["Email"].ToString(),
                IndSeverino = Boolean.Parse(Pessoa["IndSeverino"].ToString()),
                Celular = Pessoa["Celular"].ToString()                           
            };

            if (Cadastro.IndSeverino == true)
            {

            }

            return System.Text.Json.JsonSerializer.Serialize(Cadastro);
        }

        // POST Cadastro/
        [HttpPost]
        public ActionResult<Boolean> Post([FromBody] string jsonString)
        {               
            var JsonObj = JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(jsonString);

            var CadastroPessoa = new Cadastro
            {
                //tb_pessoa
                Nome = JsonObj["nome"],
                NroCPF = JsonObj["cpf"],
                Email = JsonObj["email"],
                Celular = JsonObj["celular"],
                IndSeverino = Boolean.Parse(JsonObj["indseverino"]),
                Senha = JsonObj["senha"]
            };

            string InsertPessoa = 
                "insert into tb_pessoa(nome, nrocpf, email, telefone, indseverino, senha) " +
               $"values('{CadastroPessoa.Nome}', '{CadastroPessoa.NroCPF}', '{CadastroPessoa.Email}', " +
               $"'{CadastroPessoa.Celular}', {CadastroPessoa.IndSeverino}, '{CadastroPessoa.Senha}')";

            Boolean incluiu = DBModel.RunSqlNonQuery(InsertPessoa) > 0;

            if (incluiu) {
                DBModel.GetConexao();
                var Pessoa = DBModel.GetReader($"select seqpessoa from tb_pessoa tp where tp.nrocpf = '{CadastroPessoa.NroCPF}'");
                Pessoa.Read();

                int SeqPessoa = Int32.Parse(Pessoa["SeqPessoa"].ToString());

                var CadastroEndereco = new Cadastro
                {                   
                    //tb_endereco
                    Logradouro = JsonObj["logradouro"],
                    Complemento = JsonObj["complemento"],
                    Numero = Int32.Parse(JsonObj["numero"]),                    
                    Bairro = JsonObj["bairro"],
                    Cep = JsonObj["cep"],
                    Estado = JsonObj["estado"],
                    Cidade = JsonObj["cidade"]
                };

                string InsertEndereco = 
                    $"insert into tb_endereco(seqpessoa, logradouro, complemento, numero, bairro, cep, estado, cidade) " +
                    $"values({SeqPessoa}, '{CadastroEndereco.Logradouro}', '{CadastroEndereco.Complemento}', " +
                    $"{CadastroEndereco.Numero}, '{CadastroEndereco.Bairro}', '{CadastroEndereco.Cep}', " +
                    $"'{CadastroEndereco.Estado}', '{CadastroEndereco.Cidade}')";
                DBModel.RunSqlNonQuery(InsertEndereco);

                if (CadastroPessoa.IndSeverino == true)
                {
                    var CadastroColaborador = new Cadastro
                    {
                        //tb_colaborador
                        RazaoSocial = JsonObj["razaosocial"],
                        NroCpfCnpj = JsonObj["nrocpfcnpj"],
                        LinkWhatsapp = JsonObj["linkwhatsapp"],
                        NroTelComercial = JsonObj["nrotelcomercial"]
                    };

                    string InsertColaborador = 
                        $"insert into tb_colaborador(seqpessoa, status, razaosocial, nrocpfcnpj, linkwhatsapp, nrotelcomercial) " +
                        $"values({SeqPessoa}, 'I', '{CadastroColaborador.RazaoSocial}', '{CadastroColaborador.NroCpfCnpj}', " +
                        $"'{CadastroColaborador.LinkWhatsapp}', '{CadastroColaborador.NroTelComercial}')";
                    DBModel.RunSqlNonQuery(InsertColaborador);
                }
            }

            return incluiu;
        }

        // PUT Cadastro/
        [HttpPut("{idPessoa}")]
        public void Put(int idPessoa, [FromBody] Byte imgagem)
        {          
            
        }

        // DELETE Cadastro/
        [HttpDelete("{idPessoa}")]
        public void Delete(int idPessoa)
        {

        }
    }    
}
