using PM.Model.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
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

        public IEnumerable<UserViewModel> GetAll(int role)
        {
            con = new SqlConnection(constr);
            con.Open();
            cmd = new SqlCommand("PM.GetAllUser", con)
            {
                CommandType = CommandType.StoredProcedure
            };
            cmd.Parameters.AddWithValue("@Role", role);
            dr = cmd.ExecuteReader();
            var ud = new List<UserViewModel>();
            while (dr.Read())
            {
                UserDetails u = new UserDetails
                {
                    UserName = dr["UserName"].ToString(),
                    UserPassword = dr["UserPassword"].ToString(),
                    RoleMatrixId = Convert.ToInt32(dr["RoleMatrixId"])
                };
                UserProfile userProfile = new UserProfile()
                {
                    FullName = dr["FullName"].ToString(),
                    Email = dr["Email"].ToString(),
                    Phone =Convert.ToInt64(dr["Phone"]),
                    CommunicationAddress = dr["CommunicationAddress"].ToString(),
                    Status = Convert.ToBoolean(dr["Status"])
                };
                ud.Add(new UserViewModel { UserDetails = u, UserProfile = userProfile });
            }
            return ud;
        }

        public UserDetails ValidateUser(UserDetails userDetails)
        {
            con = new SqlConnection(constr);
            con.Open();
            cmd = new SqlCommand("PM.ValidateUser", con)
            {
                CommandType = CommandType.StoredProcedure
            };
            cmd.Parameters.AddWithValue("@UserName", userDetails.UserName);
            cmd.Parameters.AddWithValue("@UserPassword", userDetails.UserPassword);
            dr = cmd.ExecuteReader();
            dr.Read();
                UserDetails u = new UserDetails
                {
                    UserName = dr["UserName"].ToString(),
                    UserPassword = dr["UserPassword"].ToString(),
                    RoleMatrixId = Convert.ToInt32(dr["RoleMatrixId"])
                };
              
            return u;
        }

        public bool BulkInsertFile(Model.Model.File file)
        {
            string filePath = string.Empty;

            if (file.FormFile != null)
            {

                var path = Path.Combine(
                        Directory.GetCurrentDirectory(), @"wwwroot\UploadedCsv",
                        file.FormFile.Name + ".csv");

                using (var stream = new FileStream(path, FileMode.Create))
                {
                    file.FormFile.CopyTo(stream);
                }


                //Creating object of datatable  
                DataTable tblcsv = new DataTable();
                //creating columns  
                tblcsv.Columns.Add("UserName");
                tblcsv.Columns.Add("FullName");
                tblcsv.Columns.Add("Email");
                tblcsv.Columns.Add("Phone");
                tblcsv.Columns.Add("CommunicationAddress");
                tblcsv.Columns.Add("Status");
                //getting full file path of Uploaded file  
                string CSVFilePath = path;
                //Reading All text  
                string ReadCSV = System.IO.File.ReadAllText(CSVFilePath);
                //spliting row after new line  
                foreach (string csvRow in ReadCSV.Split('\n'))
                {
                    if (!string.IsNullOrEmpty(csvRow))
                    {
                        //Adding each row into datatable  
                        tblcsv.Rows.Add();
                        int count = 0;
                        foreach (string FileRec in csvRow.Split(','))
                        {
                            tblcsv.Rows[tblcsv.Rows.Count - 1][count] = FileRec;
                            count++;
                        }
                    }


                }
                //Calling insert Functions  
                con = new SqlConnection(constr);
                SqlBulkCopy objbulk = new SqlBulkCopy(con);
                //assigning Destination table name    
                objbulk.DestinationTableName = "PM.UserProfile";
                //Mapping Table column    
                objbulk.ColumnMappings.Add("UserName", "UserName");
                objbulk.ColumnMappings.Add("FullName", "FullName");
                objbulk.ColumnMappings.Add("Email", "Email");
                objbulk.ColumnMappings.Add("Phone", "Phone");
                objbulk.ColumnMappings.Add("CommunicationAddress", "CommunicationAddress");
                //objbulk.ColumnMappings.Add("Status", "Status");
                //inserting Datatable Records to DataBase    
                con.Open();
                objbulk.WriteToServer(tblcsv);
                con.Close();
                return true;
            }

            return false;
        }
    }
}
