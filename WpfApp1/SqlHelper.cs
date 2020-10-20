using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;

namespace WpfApp1
{
   public static class SqlHelper
    {
        //定义一个连接字符串
        private static readonly string conStr = ConfigurationManager.ConnectionStrings["mssqlserver"].ConnectionString;
        private static readonly string conStr1 = ConfigurationManager.AppSettings["mssqlserver1"];
        //1.执行增(insert)、删(delete)、改(update)方法
        //ExecuteNonQuery()
        public static int ExecuteNonQuery(string sql, CommandType commandType,params SqlParameter[] pms)
        {
            using (SqlConnection con=new SqlConnection(conStr1))
            {
                using (SqlCommand cmd=new SqlCommand(sql,con))
                {
                    cmd.CommandType = commandType;
                    if (pms!=null)
                    {
                        cmd.Parameters.AddRange(pms);
                    }
                    con.Open();
                    return cmd.ExecuteNonQuery();
                }
            }
        }
        //2.执行查询、返回单个值的方法
        //ExecuteScalar()
        public static object ExecuteScalar(string sql, CommandType commandType, params SqlParameter[] pms)
        {
            using (SqlConnection con=new SqlConnection(conStr))
            {
                using (SqlCommand cmd=new SqlCommand(sql,con))
                {
                    cmd.CommandType = commandType;
                    if (pms!=null)
                    {
                        cmd.Parameters.AddRange(pms);
                    }
                    con.Open();
                    return cmd.ExecuteScalar();
                }
            }
        }
        //3.执行查询、返回多行、多列方法
        //ExecuteReader()
        public static SqlDataReader ExecuteReader(string sql, CommandType commandType, params SqlParameter[] pms)
        {
            SqlConnection con = new SqlConnection(conStr1);
            using (SqlCommand cmd=new SqlCommand(sql,con))
            {
                cmd.CommandType = commandType;
                if (pms!=null)
                {
                    cmd.Parameters.AddRange(pms);
                }
                try
                {
                    con.Open();
                    return cmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
                }
                catch 
                {
                    con.Close();
                    con.Dispose();
                    throw;
                }                
            }
        }
        //查询数据，返回datatable
        public static DataTable ExecuteDataTable(string sql, params SqlParameter[] pms)
        {
            DataTable dt = new DataTable();
            using (SqlDataAdapter adapter=new SqlDataAdapter(sql,conStr))
            {
                if(pms!=null)
                {
                    adapter.SelectCommand.Parameters.AddRange(pms);
                }
                adapter.Fill(dt);
            }
            return dt;
        }
    }
}
