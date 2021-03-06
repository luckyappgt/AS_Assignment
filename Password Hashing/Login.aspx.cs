using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Security.Cryptography;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Net;
using System.IO;
using System.Web.Script.Serialization;

namespace Password_Hashing
{
    public partial class Login : System.Web.UI.Page
    {

        string MYDBConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["MYDBConnection"].ConnectionString;
        static string errorMsg = "";
        static int attempts = 0;
        static string lockedstatus;
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        public bool ValidateCaptcha()
        {
            bool result = true;
            string captchaResponse = Request.Form["g-recaptcha-response"];
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create
           (" https://www.google.com/recaptcha/api/siteverify?secret=6LezjmEeAAAAALJELK6ZmwAHqFHc8Wzyoy3sS4Ue &response=" + captchaResponse);


            try
            {
                using (WebResponse wResponse = req.GetResponse())
                {
                    using (StreamReader readStream = new StreamReader(wResponse.GetResponseStream()))
                    {
                        string jsonResponse = readStream.ReadToEnd();
                        JavaScriptSerializer js = new JavaScriptSerializer();
                        CaptchaObject jsonObject = js.Deserialize<CaptchaObject>(jsonResponse);
                        result = Convert.ToBoolean(jsonObject.success);

                    }
                }

                return result;
            }
            catch (WebException ex)
            {
                throw ex;
            }
        }


        protected void btn_Login_Click(object sender, EventArgs e)
        {        
            if (ValidateCaptcha())
            {
                string pwd = tb_pwd.Text.ToString().Trim();
                string userid = tb_userid.Text.ToString().Trim();

                SHA512Managed hashing = new SHA512Managed();
                string dbHash = getDBHash(userid);
                string dbSalt = getDBSalt(userid);

                try
                {
                    if (dbSalt != null && dbSalt.Length > 0 && dbHash != null && dbHash.Length > 0)
                    {
                        string pwdWithSalt = pwd + dbSalt;
                        byte[] hashWithSalt = hashing.ComputeHash(Encoding.UTF8.GetBytes(pwdWithSalt));
                        string userHash = Convert.ToBase64String(hashWithSalt);

                        SqlConnection connection = new SqlConnection(MYDBConnectionString);
                        string sql = "select Locked FROM ACCOUNT WHERE Email=@USERID";
                        SqlCommand command = new SqlCommand(sql, connection);
                        command.Parameters.AddWithValue("@USERID", userid);
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                if (reader["Locked"] != DBNull.Value)
                                {
                                    lockedstatus = reader["Locked"].ToString();
                                }
                            }
                        }

                        if (lockedstatus == "True")
                        {
                            lb_error.Text = "You have been locked out. Please contact website adminstrator";
                        }
                        else
                        {
                            if (userHash.Equals(dbHash))
                            {
                                Session["UserID"] = userid;

                                string guid = Guid.NewGuid().ToString();
                                Session["AuthToken"] = guid;

                                Response.Cookies.Add(new HttpCookie("AuthToken", guid));

                                Response.Redirect("HomePage.aspx", false);
                            }
                            else
                            {
                                // invalid password
                                if (attempts < 3)
                                {
                                    attempts += 1;
                                    errorMsg = "Userid or password is not valid. Please try again.";
                                    lb_error.Text = errorMsg;
                                }
                                else
                                {
                                    using (SqlConnection con = new SqlConnection(MYDBConnectionString))
                                    {
                                        using (SqlCommand cmd = new SqlCommand("UPDATE Account SET Locked=1 WHERE Email=@Email"))
                                        {
                                            using (SqlDataAdapter sda = new SqlDataAdapter())
                                            {
                                                cmd.CommandType = CommandType.Text;
                                                cmd.Parameters.AddWithValue("@Email", userid);
                                                cmd.Connection = con;
                                            }
                                            try
                                            {
                                                con.Open();
                                                cmd.ExecuteNonQuery();
                                                lb_error.Text = "You have been locked out. Please contact website adminstrator";
                                            }
                                            catch (Exception ex)
                                            {
                                                throw new Exception(ex.ToString());
                                            }
                                            finally
                                            {
                                                con.Close();
                                                // to allow other users to login
                                            }
                                        }
                                    }
                                }

                            }
                        }
                    }
                    else
                    {
                        // invalid user
                        errorMsg = "Userid or password is not valid. Please try again.";
                        lb_error.Text = errorMsg;
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.ToString());
                }

                finally { }
            }
            else
            {
                lb_error.Text = "Access Denied";
            }
        }


        protected string getDBSalt(string userid)
        {

            string s = null;

            SqlConnection connection = new SqlConnection(MYDBConnectionString);
            string sql = "select PASSWORDSALT FROM ACCOUNT WHERE Email=@USERID";
            SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@USERID", userid);

            try
            {
                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (reader["PASSWORDSALT"] != null)
                        {
                            if (reader["PASSWORDSALT"] != DBNull.Value)
                            {
                                s = reader["PASSWORDSALT"].ToString();
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }

            finally { connection.Close(); }
            return s;

        }

        protected string getDBHash(string userid)
        {

            string h = null;

            SqlConnection connection = new SqlConnection(MYDBConnectionString);
            string sql = "select PasswordHash FROM Account WHERE Email=@USERID";
            SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@USERID", userid);

            try
            {
                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {

                    while (reader.Read())
                    {
                        if (reader["PasswordHash"] != null)
                        {
                            if (reader["PasswordHash"] != DBNull.Value)
                            {
                                h = reader["PasswordHash"].ToString();
                            }
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }

            finally { connection.Close(); }
            return h;
        }

        protected string decryptData(byte[] cipherText)
        {

            string decryptedString = null;
            //byte[] cipherText = Convert.FromBase64String(cipherString);

            try
            {
                RijndaelManaged cipher = new RijndaelManaged();
                ICryptoTransform decryptTransform = cipher.CreateDecryptor();

                //Decrypt
                //byte[] decryptedText = decryptTransform.TransformFinalBlock(cipherText, 0, cipherText.Length);
                //decryptedString = Encoding.UTF8.GetString(decryptedText);


            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }

            finally { }
            return decryptedString;
        }

        protected void btn_register_Click(object sender, EventArgs e)
        {
            Response.Redirect("Registration.aspx", false);
        }
    }

    public class CaptchaObject
    {
        public string success { get; set; }
        public List<string> ErrorMessage { get; set; }
    }
}