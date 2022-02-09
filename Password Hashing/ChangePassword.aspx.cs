using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Password_Hashing
{
    public partial class ChangePassword : System.Web.UI.Page
    {
        string MYDBConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["MYDBConnection"].ConnectionString;
        static byte[] Key = null;
        static byte[] IV = null;
        static string finalHash;
        static string salt;
        static string userid = null;
        static string errorMsg = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserID"] != null && Session["AuthToken"] != null && Request.Cookies["AuthToken"] != null)
            {
                if (!Session["AuthToken"].ToString().Equals(Request.Cookies["AuthToken"].Value))
                {
                    Response.Redirect("Login.aspx", false);
                }
                else
                {
                    userid = Session["UserID"].ToString();
                }
            }
            else
            {
                Response.Redirect("HomePage.aspx", false);
            }
        }

        protected void btn_Submit_Click(object sender, EventArgs e)
        {
            string oldpwd = tb_oldpwd.Text.ToString().Trim();

            SHA512Managed hashing = new SHA512Managed();
            string dbHash = getDBHash(userid);
            string dbSalt = getDBSalt(userid);

            try
            {
                if (dbSalt != null && dbSalt.Length > 0 && dbHash != null && dbHash.Length > 0)
                {
                    string pwdWithSalt = oldpwd + dbSalt;
                    byte[] hashWithSalt = hashing.ComputeHash(Encoding.UTF8.GetBytes(pwdWithSalt));
                    string userHash = Convert.ToBase64String(hashWithSalt);

                    if (userHash.Equals(dbHash))
                    {
                        // sample code
                        SqlConnection connection = new SqlConnection(MYDBConnectionString);
                        string sql = "select * from Account where email=@userid";
                        SqlCommand command = new SqlCommand(sql, connection);
                        command.Parameters.AddWithValue("@userid", userid);
                        try
                        {
                            connection.Open();
                            SqlDataReader reader = command.ExecuteReader();
                            int scores = checkPassword(tb_newpwd.Text);
                            if (scores == 1)
                            {
                                lbl_pwdchecker.Text = "Status: Weak";
                                lbl_pwdchecker.ForeColor = Color.Red;
                            }
                            else if (scores == 2)
                            {
                                lbl_pwdchecker.Text = "Status: Weak";
                                lbl_pwdchecker.ForeColor = Color.Red;
                            }
                            else if (scores == 3)
                            {
                                lbl_pwdchecker.Text = "Status: Weak";
                                lbl_pwdchecker.ForeColor = Color.Red;
                            }
                            else if (scores == 4)
                            {
                                lbl_pwdchecker.Text = "Status: Strong, not strong enough to change";
                                lbl_pwdchecker.ForeColor = Color.Green;
                            }

                            else if (scores == 5)
                            {
                                lbl_pwdchecker.Text = "Status: Very Strong";
                                lbl_pwdchecker.ForeColor = Color.Green;
                                if (tb_newpwd.Text.ToString().Trim() == tb_cfpwd.Text.ToString().Trim())
                                {

                                    string newpwd = tb_newpwd.Text.ToString().Trim();

                                    //Generate random "salt" 
                                    RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
                                    byte[] saltByte = new byte[8];

                                    //Fills array of bytes with a cryptographically strong sequence of random values.
                                    rng.GetBytes(saltByte);
                                    salt = Convert.ToBase64String(saltByte);

                                    SHA512Managed newhashing = new SHA512Managed();

                                    string newpwdWithSalt = newpwd + salt;
                                    byte[] plainHash = hashing.ComputeHash(Encoding.UTF8.GetBytes(newpwd));
                                    byte[] newhashWithSalt = hashing.ComputeHash(Encoding.UTF8.GetBytes(pwdWithSalt));

                                    finalHash = Convert.ToBase64String(hashWithSalt);

                                    RijndaelManaged cipher = new RijndaelManaged();
                                    cipher.GenerateKey();
                                    Key = cipher.Key;
                                    IV = cipher.IV;

                                    lb_error1.Text = "Password Changed";

                                    changePassword();
                                }
                                else
                                {
                                    lb_error1.Text = "Password does not match";
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            throw new Exception(ex.ToString());
                        }
                        finally
                        {
                            connection.Close();
                        }
                    }
                    else
                    {
                        // invalid password
                        errorMsg = "Old password does not match";
                        lb_error1.Text = errorMsg;
                    }
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }

            finally { }
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

        private int checkPassword(string password)
        {
            int score = 0;
            if (password.Length < 12)
            {
                return 1;
            }
            else
            {
                score = 1;
            }

            if (Regex.IsMatch(password, "[a-z]"))
            {
                score++;
            }

            if (Regex.IsMatch(password, "[A-Z]"))
            {
                score++;
            }

            if (Regex.IsMatch(password, "[0-9]"))
            {
                score++;
            }

            if (Regex.IsMatch(password, "(?=.*[^a-zA-Z0-9])"))
            {
                score++;
            }


            return score;
        }

        protected void changePassword()
        {
            try
            {
                using (SqlConnection con = new SqlConnection(MYDBConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("UPDATE FROM Account SET PasswordHash=@PasswordHash, PasswordSalt=@PasswordSalt, IV=@IV, Key=@Key WHERE Email=@Email"))
                    {
                        using (SqlDataAdapter sda = new SqlDataAdapter())
                        {
                            cmd.CommandType = CommandType.Text;
                            cmd.Parameters.AddWithValue("@Email", userid);
                            cmd.Parameters.AddWithValue("@PasswordHash", finalHash);
                            cmd.Parameters.AddWithValue("@PasswordSalt", salt);
                            cmd.Parameters.AddWithValue("@IV", Convert.ToBase64String(IV));
                            cmd.Parameters.AddWithValue("@Key", Convert.ToBase64String(Key));
                            cmd.Connection = con;
                            try
                            {
                                con.Open();
                                cmd.ExecuteNonQuery();
                                //con.Close();
                            }
                            catch (Exception ex)
                            {
                                throw new Exception(ex.ToString());
                            }
                            finally
                            {
                                con.Close();
                            }
                        }
                    }
                }


            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }
    }
}