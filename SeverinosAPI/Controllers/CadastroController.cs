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
            var CadastroPessoa = new Cadastro();

            DBModel.GetConexao();          
            var Pessoa = DBModel.GetReader($"select * from tb_pessoa tp where tp.seqpessoa = {idPessoa}");
            Pessoa.Read();
            
            CadastroPessoa.SeqPessoa = Int32.Parse(Pessoa["SeqPessoa"].ToString());
            CadastroPessoa.Nome = Pessoa["Nome"].ToString();
            CadastroPessoa.NroCPF = Pessoa["NroCPF"].ToString();
            CadastroPessoa.Email = Pessoa["Email"].ToString();
            CadastroPessoa.Telefone = Pessoa["Telefone"].ToString();
            CadastroPessoa.IndSeverino = Boolean.Parse(Pessoa["IndSeverino"].ToString());
            CadastroPessoa.Senha = Pessoa["Senha"].ToString();

            DBModel.GetConexao();
            var Endereco = DBModel.GetReader($"select * from tb_endereco te where te.seqpessoa = {idPessoa}");
            Endereco.Read();

            CadastroPessoa.Logradouro = Endereco["Logradouro"].ToString();
            CadastroPessoa.Complemento = Endereco["Complemento"].ToString();
            CadastroPessoa.Numero = Int32.Parse(Endereco["Numero"].ToString());
            CadastroPessoa.Bairro = Endereco["Bairro"].ToString();
            CadastroPessoa.Cidade = Endereco["Cidade"].ToString();
            CadastroPessoa.Cep = Endereco["Cep"].ToString();
            CadastroPessoa.Estado = Endereco["Estado"].ToString();           

            if (CadastroPessoa.IndSeverino == true)
            {
                DBModel.GetConexao();
                var Colaborador = DBModel.GetReader($"select * from tb_colaborador tc where tc.seqpessoa = {idPessoa}");
                Colaborador.Read();

                CadastroPessoa.SeqColaborador = Int32.Parse(Colaborador["SeqColaborador"].ToString());
                CadastroPessoa.Status = Colaborador["Status"].ToString();
                CadastroPessoa.RazaoSocial = Colaborador["RazaoSocial"].ToString();
                CadastroPessoa.NroCpfCnpj = Colaborador["NroCpfCnpj"].ToString();
                CadastroPessoa.LinkWhatsapp = Colaborador["LinkWhatsapp"].ToString();
                CadastroPessoa.NroTelComercial = Colaborador["NroTelComercial"].ToString();
                CadastroPessoa.Instagram = Colaborador["Instagram"].ToString();
                CadastroPessoa.Facebook = Colaborador["Facebook"].ToString();

                float NotaAvaliacao = 0;

                if (Colaborador["NotaAvaliacao"].ToString() != "")
                {
                    NotaAvaliacao = float.Parse(Colaborador["NotaAvaliacao"].ToString());
                }

                CadastroPessoa.NotaAvaliacao = NotaAvaliacao;

                DBModel.GetConexao();
                var Profissao = DBModel.GetReader(
                    $"select tp.nomeprofissao from tb_profissaocolaborador tpc " +
                    $"inner join tb_profissao tp on tp.seqprofissao = tpc.seqprofissao" +
                    $"where tpc.seqcolaborador = {CadastroPessoa.SeqColaborador}");
                Profissao.Read();

                CadastroPessoa.NomeProfissao = Profissao["NomeProfissao"].ToString();
            }

            return System.Text.Json.JsonSerializer.Serialize(CadastroPessoa);
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

                    DBModel.GetConexao();
                    var Colaborador = DBModel.GetReader($"select seqcolaborador from tb_colaborador tc where tc.seqpessoa = {SeqPessoa}");
                    Colaborador.Read();

                    DBModel.GetConexao();
                    var Profissao = DBModel.GetReader($"select seqprofissao from tb_profissao tp where tp.nomeprofissao = '{JsonObj["NomeProfissao"]}'");
                    Profissao.Read();

                    var ColaboradorProfissao = new ProfissaoColaborador
                    {
                        SeqColaborador = Int32.Parse(Colaborador["SeqColaborador"].ToString()),
                        SeqProfissao = Int32.Parse(Profissao["SeqProfissao"].ToString()),
                    };                    

                    string InsertProfissaoColaborador =
                        $"insert into tb_profissaocolaborador(seqcolaborador, seqprofissao) " +
                        $"values({ColaboradorProfissao.SeqColaborador}, {ColaboradorProfissao.SeqProfissao})";
                    DBModel.RunSqlNonQuery(InsertProfissaoColaborador);
                }
            }

            return incluiu;
        }

        // PUT Cadastro
        [HttpPut("{idPessoa}")]
        public ActionResult<Boolean> AlteraCadastro(int idPessoa, [FromBody] string jsonString)
        {
            var JsonObj = JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(jsonString);

            DBModel.GetConexao();
            var Colaborador = DBModel.GetReader($"select seqcolaborador from tb_colaborador tc where tc.seqpessoa = {idPessoa}");
            Colaborador.Read();

            var CadastroPessoa = new Cadastro
            {
                //tb_pessoa
                SeqPessoa = idPessoa,
                Nome = JsonObj["nome"],
                NroCPF = JsonObj["cpf"],
                Email = JsonObj["email"],
                Telefone = JsonObj["telefone"],
            
                //tb_endereco
                Logradouro = JsonObj["logradouro"],
                Complemento = JsonObj["complemento"],
                Numero = Int32.Parse(JsonObj["numero"]),
                Bairro = JsonObj["bairro"],
                Cep = JsonObj["cep"],
                Estado = JsonObj["estado"],
                Cidade = JsonObj["cidade"],

                //tb_colaborador
                SeqColaborador = Int32.Parse(Colaborador["seqcolaborador"].ToString()),
                RazaoSocial = JsonObj["razaosocial"],
                NroCpfCnpj = JsonObj["nrocpfcnpj"],
                LinkWhatsapp = JsonObj["linkwhatsapp"],
                Instagram = JsonObj["instagram"],
                Facebook = JsonObj["facebook"],
                NroTelComercial = JsonObj["nrotelcomercial"]
            };            

            string UpdatePessoa = 
                $"update tb_pesssoa set nome = '{CadastroPessoa.Nome}', " +
                $"nrocpf = '{CadastroPessoa.NroCPF}', email = '{CadastroPessoa.Email}', " +
                $"telefone = '{CadastroPessoa.Telefone}' where seqpessoa = {CadastroPessoa.SeqPessoa}";

            string UpdateEndereco = 
                $"update tb_endereco set logradouro = '{CadastroPessoa.Logradouro}', complemento = '{CadastroPessoa.Complemento}', " +
                $"numero = {CadastroPessoa.Numero}, bairro = '{CadastroPessoa.Bairro}', " +
                $"cep = '{CadastroPessoa.Cep}', estado '{CadastroPessoa.Estado}', cidade = '{CadastroPessoa.Cidade}'" +
                $"where seqpessoa = {CadastroPessoa.SeqPessoa}";

            string UpdateColaborador = 
                $"update tb_colaborador set razaosocial = '{CadastroPessoa.RazaoSocial}', nrocpfcnpj = '{CadastroPessoa.NroCpfCnpj}', " +
                $"linkwhatsapp = '{CadastroPessoa.LinkWhatsapp}', instagram = '{CadastroPessoa.Instagram}', " +
                $"facebook = '{CadastroPessoa.Facebook}', nrotelcomercial = '{CadastroPessoa.NroTelComercial}' " +
                $"where seqpessoa = {CadastroPessoa.SeqPessoa}";

            DBModel.GetConexao();
            var Profissao = DBModel.GetReader($"select seqprofissao from tb_profissao tp where tp.nomeprofissao = '{JsonObj["NomeProfissao"]}'");
            Profissao.Read();

            string UpdateProfissaoColaborador = 
                $"";


            return true;
        }

        // PUT Cadastro
        [HttpPut("{idPessoa}/{imagem}")]
        public ActionResult<Boolean> AlteraImagem(int idPessoa, string imagem)
        {
            DBModel.GetConexao();

            string UpdateSenha =
                $"update tb_pessoa set igmLogo = '{imagem}' where SeqPessoa = {idPessoa}";

            return DBModel.RunSqlNonQuery(UpdateSenha) > 0;
        }        

        // DELETE Cadastro
        [HttpDelete("{idPessoa}")]
        public ActionResult<string> DeletaCadastro(int idPessoa)
        {
            return "S";
        }
    }    
}
