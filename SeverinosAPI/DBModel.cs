using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Npgsql;

namespace SeverinoConexao
{
    public class DBModel
    {
        public static NpgsqlConnection Conexao = GetConexao();

        public static Npgsql.NpgsqlConnection GetConexao()
        {
            NpgsqlConnection conn;
            string myConnectionString;

            myConnectionString = DBModel.GetConnectionString();
            try
            {
                conn = new NpgsqlConnection();
                conn.ConnectionString = myConnectionString;
                conn.Open();                

                return conn;
            }
            catch (Npgsql.NpgsqlException Ex)
            {
                throw new Exception(Ex.Message);
            }
        }

        private static String GetConnectionString()
        {
            string sRetorno = "";

            sRetorno = "User ID=eyzsiosffqeroz;" +
                       "Password=75f2eec3886a903f63b330acae42b36d5eff5e6381c9232333bdc00e1639b200;" +
                       "Host=ec2-35-153-114-74.compute-1.amazonaws.com;" +
                       "Port=5432;" +
                       "Database=d13nbustk8eoiv;" +
                       "Pooling=true;" +
                       "SSL Mode=Require;" +
                       "TrustServerCertificate=True;";

            return sRetorno;
        }

        public static Npgsql.NpgsqlDataReader GetReader(string sSql)
        {
            NpgsqlDataReader drRetorno;
            NpgsqlCommand oCmd = new NpgsqlCommand();
            oCmd.Connection = Conexao;
            oCmd.CommandText = sSql;

            drRetorno = oCmd.ExecuteReader();

            return drRetorno;
        }

        public static int RunSqlNonQuery(string sSql)
        {
            NpgsqlCommand oCmd = new NpgsqlCommand();
            oCmd.Connection = Conexao;
            oCmd.CommandText = sSql;

            int iRetorno = oCmd.ExecuteNonQuery();            

            return iRetorno;
        }

        //public static void CleanUPDBReader(ref MySqlDataReader oDr)
        //{
        //    if (oDr != null)
        //    {
        //        oDr.Dispose();
        //    }
        //}

        //public static void CleanUPDBCmd(ref MySqlCommand oCmd)
        //{
        //    if (oCmd != null)
        //    {
        //        if (oCmd.Connection.State == ConnectionState.Open)
        //        {
        //            oCmd.Connection.Clone();
        //        }
        //        oCmd.Connection.Dispose();

        //        oCmd.Dispose();
        //    }
        //}
    }
}
