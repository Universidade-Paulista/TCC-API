using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SeverinoConexao;
using SeverinosAPI.Models;

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

        public List<Colaborador> GetListaSeverinos(string idcolabnomeprof)
        {
            try
            {
                var Profisssao = DBModel.GetReader($"select seqprofissao from tb_profissao where upper(nomeprofissao) = '{idcolabnomeprof.ToUpper()}'");
                Profisssao.Read();

                List<Colaborador> Colaboradores = new List<Colaborador>();               

                if (Profisssao.HasRows)
                {
                    string SelectColaboradoes =
                        " select tc.seqcolaborador, tc.razaosocial, cast(tps.imglogo as varchar) imglogo" +
                        "   from tb_profissaocolaborador tp                     " +
                        "  inner join tb_colaborador tc                         " +
                        "     on tp.seqcolaborador = tc.seqcolaborador          " +
                        "  inner join tb_pessoa tps                             " +
                        "     on tps.seqpessoa = tc.seqpessoa                   " +
                       $"  where tp.seqprofissao = {Int32.Parse(Profisssao["seqprofissao"].ToString())} "+
                        "    and tc.status = 'A'                                ";
                    DBModel.Conexao.Close();

                    var Colaborador = DBModel.GetReader(SelectColaboradoes);

                    while (Colaborador.Read())
                    {
                        var Colab = new Colaborador
                        {
                            SeqColaborador = Int32.Parse(Colaborador["seqcolaborador"].ToString()),
                            RazaoSocial = Colaborador["razaosocial"].ToString(),
                            ImgLogo = (string)Colaborador["imglogo"]
                        };

                        Colaboradores.Add(Colab);
                    }
                    DBModel.Conexao.Close();

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
