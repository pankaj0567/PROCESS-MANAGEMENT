using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Net;
using System.Net.Mail;
using System.Text;
using PM.Model.Model;

namespace PM.API.DAL.DAL
{
    public class UserProfileDbManager : IUserProfileDbManager
    {
        SqlConnection con;
        SqlCommand cmd;
        SqlDataReader dr;
        private readonly string constr;
        private readonly IMailNetworkCredential mailNetworkCredential;

        public UserProfileDbManager(IWebConfiguration Constr, IMailNetworkCredential mailNetworkCredential)
        {
            constr = Constr.DefaultConnection;
            this.mailNetworkCredential = mailNetworkCredential;
        }
        public bool Create(UserProfile userProfile,int? id=0)
        {
            con = new SqlConnection(constr);
            con.Open();
            cmd = new SqlCommand("PM.CreateUserProfile", con)
            {
                CommandType = CommandType.StoredProcedure
            };
            cmd.Parameters.AddWithValue("@FullName", userProfile.FullName);
            cmd.Parameters.AddWithValue("@Email", userProfile.Email);
            cmd.Parameters.AddWithValue("@Phone", userProfile.Phone);
            cmd.Parameters.AddWithValue("@CommunicationAddress", userProfile.CommunicationAddress);
            cmd.Parameters.AddWithValue("@Status", userProfile.Status);
            if (id == 1)
            {
                cmd.Parameters.AddWithValue("@UserName", "1");
                dr = cmd.ExecuteReader();
                dr.Read();
                UserDetails u = new UserDetails
                {
                    UserName = dr["UserName"].ToString(),
                    UserPassword = dr["UserPassword"].ToString(),
                    RoleMatrixId = Convert.ToInt32(dr["RoleMatrixId"])
                };
                UserProfile userP = new UserProfile()
                {
                    FullName = dr["FullName"].ToString(),
                    Email = dr["Email"].ToString(),
                    Phone = Convert.ToInt64(dr["Phone"]),
                    CommunicationAddress = dr["CommunicationAddress"].ToString(),
                    Status = Convert.ToBoolean(dr["Status"])
                };
                string Message = $"You are registered successfully. Your user name is {u.UserName} and password.";
                if (SendEmail(userP.FullName, userP.Email, Message) == "Email Sent Successfully!") 
                    return true;

                return false;
            }
            else
            {
                cmd.Parameters.AddWithValue("@UserName", userProfile.UserName);
                return cmd.ExecuteNonQuery() > 0 ? true : false;
            }



            
        }


        public string SendEmail(string Name, string Email, string Message)
        {
            try
            {
                // Credentials
                var credentials = new NetworkCredential(mailNetworkCredential.UserName, mailNetworkCredential.Password);
                // Mail message
                var mail = new MailMessage()
                {
                    From = new MailAddress("noreply@gmail.com"),
                    Subject = "Login Credential",
                    Body = Message
                };
                mail.IsBodyHtml = true;
                mail.To.Add(new MailAddress(Email));
                // Smtp client
                var client = new SmtpClient()
                {
                    Port = 587,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Host = "smtp.gmail.com",
                    EnableSsl = true,
                    Credentials = credentials
                };
                client.Send(mail);
                return "Email Sent Successfully!";
            }
            catch (System.Exception e)
            {
                return e.Message;
            }

        }
    }
}
