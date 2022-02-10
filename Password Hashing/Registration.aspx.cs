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

using System.Text.RegularExpressions;
using System.Drawing;
using System.IO;

namespace Password_Hashing
{
    public partial class Registration : System.Web.UI.Page
    {
        string MYDBConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["MYDBConnection"].ConnectionString;
        static string finalHash;
        static string salt;
        byte[] Key;
        byte[] IV;

        static string line = "\r";

        string strFileName;
        string strFilePath;
        string strFolder;

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btn_Submit_Click(object sender, EventArgs e)
        {

            SqlConnection connection = new SqlConnection(MYDBConnectionString);
            string sql = "select * from Account where email=@userid";
            SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@userid", tb_userid.Text.Trim());
            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    lb_error1.Text = "User already available";
                    lbl_pwdchecker.Text = "";
                }
                else
                {
                    int scores = checkPassword(tb_pwd.Text);
                    if (scores <= 3)
                    {
                        lbl_pwdchecker.Text = "Status: Weak";
                        lbl_pwdchecker.ForeColor = Color.Red;
                    }
                    else if (scores == 4)
                    {
                        lbl_pwdchecker.Text = "Status: Strong, not strong enough for account creation";
                        lbl_pwdchecker.ForeColor = Color.Green;
                    }

                    else if (scores == 5)
                    {
                        lbl_pwdchecker.Text = "Status: Very Strong";
                        lbl_pwdchecker.ForeColor = Color.Green;
                        if (tb_pwd.Text.ToString().Trim() == tb_cfpwd.Text.ToString().Trim())
                        {
                            // photo upload

                            strFolder = Server.MapPath("./Photos/");
                            // Retrieve the name of the file that is posted.
                            strFileName = photoUpload.PostedFile.FileName;
                            strFileName = Path.GetFileName(strFileName);
                            if (photoUpload.Value != "")
                            {
                                if (!Directory.Exists(strFolder))
                                {
                                    Directory.CreateDirectory(strFolder);
                                }
                                strFilePath = strFolder + strFileName;
                                photoUpload.PostedFile.SaveAs(strFilePath);
                                string pwd = tb_pwd.Text.ToString().Trim();

                                //Generate random "salt" 
                                RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
                                byte[] saltByte = new byte[8];

                                //Fills array of bytes with a cryptographically strong sequence of random values.
                                rng.GetBytes(saltByte);
                                salt = Convert.ToBase64String(saltByte);

                                SHA512Managed hashing = new SHA512Managed();

                                string pwdWithSalt = pwd + salt;
                                byte[] plainHash = hashing.ComputeHash(Encoding.UTF8.GetBytes(pwd));
                                byte[] hashWithSalt = hashing.ComputeHash(Encoding.UTF8.GetBytes(pwdWithSalt));

                                finalHash = Convert.ToBase64String(hashWithSalt);

                                RijndaelManaged cipher = new RijndaelManaged();
                                cipher.GenerateKey();
                                Key = cipher.Key;
                                IV = cipher.IV;

                                createAccount();
                                Response.Redirect("Login.aspx", false);
                            }
                            else
                            {
                                lb_error1.Text = "Click 'Choose File' to select the file to upload.";
                            }

                        }
                        else
                        {
                            lb_error1.Text = "Password does not match";
                        }
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


        protected void createAccount()
        {

            try
            {
                using (SqlConnection con = new SqlConnection(MYDBConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("INSERT INTO Account VALUES(@FirstName, @LastName, @CreditCard, @CreditCV, @CreditExpiry, @Email, @PasswordHash, @PasswordSalt, @DateOfBirth ,@IV,@Key, @PhotoPath, @Locked)"))
                    {
                        using (SqlDataAdapter sda = new SqlDataAdapter())
                        {
                            cmd.CommandType = CommandType.Text;
                            cmd.Parameters.AddWithValue("@FirstName", tb_fname.Text.Trim());
                            cmd.Parameters.AddWithValue("@LastName", tb_lname.Text.Trim());
                            cmd.Parameters.AddWithValue("@CreditCard", Convert.ToBase64String(encryptData(tb_creditcard.Text.Trim())));
                            cmd.Parameters.AddWithValue("@CreditCV", Convert.ToBase64String(encryptData(tb_creditcv.Text.Trim())));
                            cmd.Parameters.AddWithValue("@CreditExpiry", Convert.ToBase64String(encryptData(tb_creditexpiry.Text.Trim())));
                            cmd.Parameters.AddWithValue("@Email", tb_userid.Text.Trim());
                            cmd.Parameters.AddWithValue("@PasswordHash", finalHash);
                            cmd.Parameters.AddWithValue("@PasswordSalt", salt);
                            cmd.Parameters.AddWithValue("@DateOfBirth", tb_dob.Text.Trim());
                            cmd.Parameters.AddWithValue("@IV", Convert.ToBase64String(IV));
                            cmd.Parameters.AddWithValue("@Key", Convert.ToBase64String(Key));
                            cmd.Parameters.AddWithValue("@PhotoPath", strFilePath);
                            cmd.Parameters.AddWithValue("@Locked", false);
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

        protected byte[] encryptData(string data)
        {
            byte[] cipherText = null;
            try
            {
                RijndaelManaged cipher = new RijndaelManaged();
                cipher.IV = IV;
                cipher.Key = Key;
                ICryptoTransform encryptTransform = cipher.CreateEncryptor();
                //ICryptoTransform decryptTransform = cipher.CreateDecryptor();
                byte[] plainText = Encoding.UTF8.GetBytes(data);
                cipherText = encryptTransform.TransformFinalBlock(plainText, 0, plainText.Length);

            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }

            finally { }
            return cipherText;
        }

        protected void btn_Login_Click(object sender, EventArgs e)
        {
            Response.Redirect("Login.aspx", false);
        }
    }
}