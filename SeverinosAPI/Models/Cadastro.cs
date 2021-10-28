using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SeverinosAPI.Models
{
    public class Cadastro
    {
        //Tabela - tb_pessoa
        public int SeqPessoa { get; set; }
        public string Nome { get; set; }
        public string NroCPF { get; set; }
        public string Email { get; set; }
        public string Celular { get; set; }
        public Boolean IndSeverino { get; set; }
        public string Senha { get; set; }

        //Tabela - tb_endereco
        public string Logradouro { get; set; }
        public string Complemento { get; set; }
        public int Numero { get; set; }
        public string Bairro { get; set; }
        public string Cidade { get; set; }
        public string Cep { get; set; }
        public string Estado { get; set; }

        //Tabela - tb_colaborador
        public int SeqColaborador { get; set; }
        public string Status { get; set; }
        public string RazaoSocial { get; set; }
        public string NroCpfCnpj { get; set; }
        public string LinkWhatsapp { get; set; }
        public string NroTelComercial { get; set; }
        public string Instagram { get; set; }
        public string Facebook { get; set; }
        public float NotaAvaliacao { get; set; }

    }
}
