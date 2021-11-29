using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SeverinoConexao;

namespace SeverinosAPI.Controllers
{
    [Route("api/[controller]")]
    [Controller]
    public class StatusController
    {
        // GET Status
        [HttpGet("{idPessoa}")]
        public ActionResult<string> GetStatus(int idPessoa)
        {
            try
            {
                var Status = DBModel.GetReader($" select status from tb_colaborador where seqpessoa = {idPessoa} ");
                Status.Read();

                if (Status.HasRows)
                {
                    return Status["Status"].ToString();
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

        // POST Status
        [HttpPost("{idPessoa}/{Status}")]
        public ActionResult<Boolean> AlteraStatus(string idPessoa, string Status)
        {
            try
            {
                if (Status == "A" || Status == "I")
                {
                    string UpdateStatus =
                    " update tb_colaborador         " +
                   $"    set status = '{Status}'    " +
                   $"  Where seqpessoa = {idPessoa} ";

                    return DBModel.RunSqlNonQuery(UpdateStatus) > 0;
                }
                else
                {
                    return false;
                }                
            }
            finally
            {
                DBModel.Conexao.Close();
            }
        }
    }
}
