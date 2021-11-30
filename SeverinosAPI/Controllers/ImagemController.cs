using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SeverinoConexao;
using Newtonsoft.Json;

namespace SeverinosAPI.Controllers
{
    [Route("api/[controller]")]
    [Controller]
    public class ImagemController
    {
        // GET Imagem
        [HttpGet("{idPessoa}")]
        public ActionResult<string> Get(int idPessoa)
        {
            try
            {
                var Imagem = DBModel.GetReader($" select Coalesce(imgLogo, 'Sem imagem') imgLogo from tb_pessoa where seqpessoa = {idPessoa} ");
                Imagem.Read();

                if (Imagem.HasRows)
                {
                    return Imagem["imgLogo"].ToString();
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

        // PUT Imagem
        [HttpPut("{idPessoa}")]
        public ActionResult<Boolean> AlteraImagem(int idPessoa, [FromBody] string jsonString)
        {
            try
            {
                var JsonObj = JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(jsonString);

                string UpdateSenha =
                    $"update tb_pessoa set imgLogo = '{JsonObj["imagem"]}' where SeqPessoa = {idPessoa}";

                return DBModel.RunSqlNonQuery(UpdateSenha) > 0;
            }
            finally
            {
                DBModel.Conexao.Close();
            }
        }
    }
}
