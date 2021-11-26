using Microsoft.AspNetCore.Mvc;
using SeverinoConexao;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SeverinosAPI.Controllers
{
    [Route("api/[controller]")]
    [Controller]
    public class ValidacoesCPFController
    {
        // GET ValidacoesCPF
        [HttpGet("{cpf}")]
        public ActionResult<string> GetCPFExistente(string cpf)
        {
            try
            { 
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
            finally
            {
                DBModel.Conexao.Close();
            }
        }
    }
}
