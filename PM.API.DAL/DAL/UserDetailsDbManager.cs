using PM.Model.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace PM.API.DAL.DAL
{
    public class UserDetailsDbManager : IUserDetailsDbManager
    {
        SqlConnection con;
        SqlCommand cmd;
        SqlDataReader dr;
        private readonly string constr;

        public UserDetailsDbManager(IWebConfiguration Constr)
        {
            constr = Constr.DefaultConnection;
        }
        public bool Create(UserDetails userDetails)
        {
            con = new SqlConnection(constr);
            con.Open();
            cmd = new SqlCommand("PM.CreateUserDetails", con)
            {
                CommandType = CommandType.StoredProcedure
            };
            cmd.Parameters.AddWithValue("@UserName", userDetails.UserName);
            cmd.Parameters.AddWithValue("@UserPassword", userDetails.UserPassword);
            cmd.Parameters.AddWithValue("@RoleMatrixId", userDetails.RoleMatrixId);
            return cmd.ExecuteNonQuery() > 0 ? true : false;
        }

        public IEnumerable<UserDetails> GetAll()
        {
            con = new SqlConnection(constr);
            con.Open();
            cmd = new SqlCommand("PM.GetAllUserDetails ", con)
            {
                CommandType = CommandType.StoredProcedure
            };
            dr = cmd.ExecuteReader();
            var ud = new List<UserDetails>();
            while (dr.Read())
            {
                UserDetails u = new UserDetails
                {
                    UserName = dr["UserName"].ToString(),
                    UserPassword = dr["UserPassword"].ToString(),
                    RoleMatrixId = Convert.ToInt32(dr["RoleMatrixId"])
                };
                ud.Add(u);
            }
            return ud;
        }
    }
}
