
/*
 
    REQUIRE LICENSE GENERATOR 1.073 OR GREATER https://github.com/cTrader-Guru/License-Generator
 
 */


using System;
using System.IO;
using cAlgo.API;
using System.Text;
using System.Text.Json;
using System.Security.Cryptography;

namespace cAlgo.Robots
{

    public class License : Robot
    {

        #region Editable Params

        private readonly byte[] IV = new byte[16] { 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x75, 0x69, 0x0, 0x73, 0x0, 0x0, 0x0, 0x0 };
        private readonly string Password = "Edit with your password";

        private readonly string LocalFileName = "{0}\\cAlgo\\cTraderGuru\\License-{1}.ctg";

        #endregion
        
        #region Class

        private class LicenseInfo
        {

            public long UserID { get; set; } = 0;

            public string Product { get; set; } = "";          // <-- (Product Name)

            public string Expire { get; set; } = "";           // <-- (*|2022.12.31 23:59:00)

            public bool AllowBackTest { get; set; } = true;

        }

        #endregion
        
        #region Property

        private LicenseInfo Info = null;
        private bool Checked = false;

        #endregion

        #region Public Methods

        public void CheckLicense(string cBotName)
        {

            try
            {

                if (Info == null)
                {

                    Print($"Loading license for {Account.UserId}...");

                    string HashName = GetMD5(cBotName.ToUpper() + Account.UserId);

                    string LocalLicense;

                    try
                    {

                        LocalLicense = LoadSecureFile(string.Format(LocalFileName, Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), HashName));

                    }
                    catch
                    {

                        throw new Exception("Problems on load license");

                    }

                    string Decrypted;

                    try
                    {
                        Decrypted = Decrypt(LocalLicense);
                    }
                    catch
                    {

                        throw new Exception("Decryption problems");

                    }

                    Info = JsonSerializer.Deserialize<LicenseInfo>(Decrypted);

                    if (Info == null) throw new Exception("Unexpected error in the license reading process");

                }

                if (RunningMode != RunningMode.RealTime)
                {

                    if (!Info.AllowBackTest)
                    {

                        Exit("This license does not provide for backtesting");
                        return;

                    }

                }
                else
                {

                    if (Info.UserID != Account.UserId)
                    {

                        Exit(string.Format("License not for this UserID ({0})", Account.UserId));
                        return;

                    }
                    else if (Info.Product.ToUpper().CompareTo(cBotName.ToUpper()) != 0)
                    {

                        Exit(string.Format("License not for this product '{0}'", cBotName));
                        return;

                    }
                    else if (Info.Expire.CompareTo("*") != 0 && Server.Time > Convert.ToDateTime(Info.Expire))
                    {

                        Exit(string.Format("License Expired for {0}", cBotName));
                        return;

                    }

                }

                if (!Checked)
                {
                    Checked = true;
                    Print("License for {0} expire : {1}", Info.Product, Info.Expire.CompareTo("*") != 0 ? Info.Expire : "Life Time");

                }

            }
            catch (Exception exc)
            {

                Exit(string.Format("Error process license : {0}", exc.Message));
                return;

            }

        }

        #endregion
        
        #region Private Methods

        private string LoadSecureFile(string FileToOpen, double Waiting = 5000)
        {

            string response = null;

            using (FileStream fs = File.Open(FileToOpen, FileMode.Open, FileAccess.Read, FileShare.None))
            {

                var started = DateTime.UtcNow;
                while ((DateTime.UtcNow - started).TotalMilliseconds < Waiting)
                {

                    try
                    {

                        using (StreamReader sr = new StreamReader(fs))
                        {
                            response = sr.ReadToEnd();

                            sr.Dispose();
                            fs.Dispose();
                            return response;

                        }

                    }
                    catch { }

                }

                fs.Dispose();

            }

            // --> string enc = File.ReadAllText(_config.fileLicense);

            return null;

        }

        private string GetMD5(string input)
        { // <-- https://stackoverflow.com/questions/11454004/calculate-a-md5-hash-from-a-string

            // Use input string to calculate MD5 hash
            using (MD5 md5 = MD5.Create())
            {
                byte[] inputBytes = Encoding.ASCII.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                // Convert the byte array to hexadecimal string
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("X2"));
                }
                return sb.ToString();
            }

        }

        private string Encrypt(string PlainText)
        {

            SHA256 mySHA256 = SHA256.Create();
            byte[] key = mySHA256.ComputeHash(Encoding.ASCII.GetBytes(Password));

            // Instantiate a new Aes object to perform string symmetric encryption
            Aes encryptor = Aes.Create();

            encryptor.Mode = CipherMode.CBC;

            // Set key and IV
            byte[] aesKey = new byte[32];
            Array.Copy(key, 0, aesKey, 0, 32);
            encryptor.Key = aesKey;
            encryptor.IV = IV;

            // Instantiate a new MemoryStream object to contain the encrypted bytes
            MemoryStream memoryStream = new MemoryStream();

            // Instantiate a new encryptor from our Aes object
            ICryptoTransform aesEncryptor = encryptor.CreateEncryptor();

            // Instantiate a new CryptoStream object to process the data and write it to the 
            // memory stream
            CryptoStream cryptoStream = new CryptoStream(memoryStream, aesEncryptor, CryptoStreamMode.Write);

            // Convert the plainText string into a byte array
            byte[] plainBytes = Encoding.ASCII.GetBytes(PlainText);

            // Encrypt the input plaintext string
            cryptoStream.Write(plainBytes, 0, plainBytes.Length);

            // Complete the encryption process
            cryptoStream.FlushFinalBlock();

            // Convert the encrypted data from a MemoryStream to a byte array
            byte[] cipherBytes = memoryStream.ToArray();

            // Close both the MemoryStream and the CryptoStream
            memoryStream.Close();
            cryptoStream.Close();

            // Convert the encrypted byte array to a base64 encoded string
            string cipherText = Convert.ToBase64String(cipherBytes, 0, cipherBytes.Length);

            // Return the encrypted data as a string
            return cipherText;

        }

        private string Decrypt(string EncText)
        {

            SHA256 mySHA256 = SHA256.Create();
            byte[] key = mySHA256.ComputeHash(Encoding.ASCII.GetBytes(Password));

            // Instantiate a new Aes object to perform string symmetric encryption
            Aes encryptor = Aes.Create();

            encryptor.Mode = CipherMode.CBC;

            // Set key and IV
            byte[] aesKey = new byte[32];
            Array.Copy(key, 0, aesKey, 0, 32);
            encryptor.Key = aesKey;
            encryptor.IV = IV;

            // Instantiate a new MemoryStream object to contain the encrypted bytes
            MemoryStream memoryStream = new MemoryStream();

            // Instantiate a new encryptor from our Aes object
            ICryptoTransform aesDecryptor = encryptor.CreateDecryptor();

            // Instantiate a new CryptoStream object to process the data and write it to the 
            // memory stream
            CryptoStream cryptoStream = new CryptoStream(memoryStream, aesDecryptor, CryptoStreamMode.Write);

            // Will contain decrypted plaintext
            string plainText = String.Empty;

            try
            {
                // Convert the ciphertext string into a byte array
                byte[] cipherBytes = Convert.FromBase64String(EncText);

                // Decrypt the input ciphertext string
                cryptoStream.Write(cipherBytes, 0, cipherBytes.Length);

                // Complete the decryption process
                cryptoStream.FlushFinalBlock();

                // Convert the decrypted data from a MemoryStream to a byte array
                byte[] plainBytes = memoryStream.ToArray();

                // Convert the decrypted byte array to string
                plainText = Encoding.ASCII.GetString(plainBytes, 0, plainBytes.Length);
            }
            finally
            {
                // Close both the MemoryStream and the CryptoStream
                memoryStream.Close();
                cryptoStream.Close();
            }

            // Return the decrypted data as a string
            return plainText;

        }

        private void Exit(string Message)
        {

            Print(Message);
            Stop();

        }

        #endregion

    }

}
