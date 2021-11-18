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
        // GET Cadastro
        [HttpGet("{idPessoa}")]
        public ActionResult<string> GetCadastro(int idPessoa)
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
                Telefone = Pessoa["telefone"].ToString()                           
            };

            if (Cadastro.IndSeverino == true)
            {

            }

            return System.Text.Json.JsonSerializer.Serialize(Cadastro);
        }

        // GET Cadastro
        [HttpGet("{email}/{senha}")]
        public ActionResult<String> GetNomeSeverino(String email, String senha)
        {
            DBModel.GetConexao();
            var pessoa = DBModel.GetReader($"select nome from tb_pessoa where upper(email) = '{email.ToUpper()}' and upper(senha) = '{senha.ToUpper()}' and indseverino = true ");
            pessoa.Read();

            return pessoa["nome"].ToString();
		}
		
        // GET Cadastro
        [HttpGet("{cpf}")]
        public ActionResult<string> GetCPFExistente(string cpf)
        {
            DBModel.GetConexao();
            var Pessoa = DBModel.GetReader($"select * from tb_pessoa tp where tp.nrocpf = '{cpf}'");
            Pessoa.Read();

            if (Pessoa.HasRows)     
            {
                return "S";
            }
            else
            {
                return "N";
            }            
        }

        // POST Cadastro
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
                Telefone = JsonObj["telefone"],
                IndSeverino = Boolean.Parse(JsonObj["indseverino"]),
                Senha = JsonObj["senha"]
            };

            string InsertPessoa = 
                "insert into tb_pessoa(nome, nrocpf, email, telefone, indseverino, senha) " +
               $"values('{CadastroPessoa.Nome}', '{CadastroPessoa.NroCPF}', '{CadastroPessoa.Email}', " +
               $"'{CadastroPessoa.Telefone}', {CadastroPessoa.IndSeverino}, '{CadastroPessoa.Senha}')";

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

        // PUT Cadastro
        [HttpPut("{idPessoa}")]
        public ActionResult<string> AlteraCadastro(int idPessoa, [FromBody] string jsonString)
        {
            return "S";
        }

        // PUT Cadastro
        [HttpPut("{idPessoa}/{imagem}")]
        public ActionResult<string> AlteraImagem(int idPessoa, string imagem)
        {
            return "S";
        }        

        // DELETE Cadastro
        [HttpDelete("{idPessoa}")]
        public ActionResult<string> DeletaCadastro(int idPessoa)
        {
            return "S";
        }
    }    
}
