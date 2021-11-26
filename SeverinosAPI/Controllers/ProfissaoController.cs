using Microsoft.AspNetCore.Mvc;
using System;
using SeverinoConexao;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SeverinosAPI.Models;

namespace SeverinosAPI.Controllers
{
    [Route("api/[controller]")]
    [Controller]
    public class ProfissaoController
    {
        // GET Profissao
        [HttpGet()]
        public ActionResult<List<string>> GetProfissoes()
        {
            try
            { 
                var Profissoes = DBModel.GetReader($"select * from tb_profissao");
                Profissoes.Read();

                List<string> Lista = new List<string>();            

                while (Profissoes.Read())
                {               
                    Lista.Add(Profissoes["NomeProfissao"].ToString());
                }

                return Lista;
            }
            finally
            {
                DBModel.Conexao.Close();
            }
        }
    }
}
