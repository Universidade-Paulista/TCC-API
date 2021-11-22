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
        public ActionResult<List<Profissao>> GetProfissoes()
        {
            DBModel.GetConexao();
            var Profissoes = DBModel.GetReader($"select * from tb_profissao");
            Profissoes.Read();

            List<Profissao> Lista = new List<Profissao>();            

            while (Profissoes.Read())
            {
                var Prof = new Profissao
                {
                    SeqProfissao = int.Parse(Profissoes["SeqProfissao"].ToString()),
                    NomeProfissao = Profissoes["NomeProfissao"].ToString()
                };

                Lista.Add(Prof);
            }

            return Lista;
        }
    }
}
