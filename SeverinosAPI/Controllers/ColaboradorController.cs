using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SeverinoConexao;

namespace SeverinosAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ColaboradorController
    {
        // GET api/values
        [HttpGet]
        [HttpGet("{idcolabnomeprof}/{Metodo}")]
        public ActionResult<dynamic> Get(string idcolabnomeprof, string Metodo)
        {
            if (Metodo.ToUpper() == "whatsapp".ToUpper()) {
                return GetLinkWhatsappSeverino(idcolabnomeprof);
            }
            else
            {
                return GetListaSeverinos(idcolabnomeprof);
            }
        }

        public List<string> GetListaSeverinos(string idcolabnomeprof)
        {
            try
            {
                var Profisssao = DBModel.GetReader($"select seqprofissao from tb_profissao where upper(nomeprofissao) = '{idcolabnomeprof.ToUpper()}'");
                Profisssao.Read();

                List<string> Colaboradores = new List<string>();

                if (Profisssao.HasRows)
                {
                    string SelectColaboradoes =
                        " select tc.razaosocial                        " +
                        "   from tb_profissaocolaborador tp            " +
                        "  inner join tb_colaborador tc                " +
                        "     on tp.seqcolaborador = tc.seqcolaborador " +
                       $"  where tp.seqprofissao = {Int32.Parse(Profisssao["seqprofissao"].ToString())} ";
                    DBModel.Conexao.Close();

                    var Colaborador = DBModel.GetReader(SelectColaboradoes);

                    while (Colaborador.Read())
                    {
                        Colaboradores.Add(Colaborador["razaosocial"].ToString());
                    }

                    return Colaboradores;
                }
                else
                {
                    return Colaboradores;
                }
            }
            finally
            {
                DBModel.Conexao.Close();
            }
        }

        public string GetLinkWhatsappSeverino(string idcolabnomeprof)
        {
            try
            {
                var Severino = DBModel.GetReader($"select linkwhatsapp from tb_colaborador where seqpessoa = {idcolabnomeprof}");
                Severino.Read();

                if (Severino.HasRows)
                {
                    return Severino["linkwhatsapp"].ToString();
                }
                else
                {
                    return "E";
                }
            }
            finally
            {
                DBModel.Conexao.Close();
            }            
        }
    }
}
