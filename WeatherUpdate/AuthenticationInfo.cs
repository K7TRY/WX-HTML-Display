using System;
using System.Security.Cryptography;
using System.Text;
using System.Xml.Serialization;

namespace WeatherUpdate
{
   [Serializable]
   public class AuthenticationInfo : IDisposable
   {
      public string UserName { get; set; }

      public string Domain { get; set; }

      public string EncryptedPassword;

      [XmlIgnore()]
      public string Password
      {
         get { return Decrypt(EncryptedPassword); }
         set { EncryptedPassword = Encrypt(value); }
      }

      [XmlIgnore()]
      private string _Key;

      [XmlIgnore()]
      protected string Key
      {
         get
         {
            if (_Key == null)
               _Key = GenerateKey();

            return _Key;
         }
         set { _Key = value; }
      }

      [XmlIgnore()]
      private string _IV;

      [XmlIgnore()]
      protected string IV
      {
         get
         {
            if (_IV == null)
               _IV = GenerateIV();

            return _IV;
         }
         set { _IV = value; }
      }

      public AuthenticationInfo()
      {
      }

      protected string Encrypt(string clearData)
      {
         byte[] buffer = Encoding.Unicode.GetBytes(clearData);

         using (RijndaelManaged rij = new RijndaelManaged())
         {
            rij.Key = Convert.FromBase64String(this.Key);
            rij.IV = Convert.FromBase64String(this.IV);

            return Convert.ToBase64String(rij.CreateEncryptor().TransformFinalBlock(buffer, 0, buffer.Length));
         }
      }

      protected string Decrypt(string cipherData)
      {
         byte[] buffer = Convert.FromBase64String(cipherData);

         using (RijndaelManaged rij = new RijndaelManaged())
         {
            rij.Key = Convert.FromBase64String(this.Key);
            rij.IV = Convert.FromBase64String(this.IV);

            return Encoding.Unicode.GetString(rij.CreateDecryptor().TransformFinalBlock(buffer, 0, buffer.Length));
         }
      }

      protected string GenerateKey()
      {
         using (RijndaelManaged rij = new RijndaelManaged())
         {
            rij.GenerateKey();
            return Convert.ToBase64String(rij.Key);
         }
      }

      protected string GenerateIV()
      {
         using (RijndaelManaged rij = new RijndaelManaged())
         {
            rij.GenerateIV();
            return Convert.ToBase64String(rij.IV);
         }
      }

      public bool TestPasswordEncryption(string PasswordToHide)
      {
         if (PasswordToHide == null || PasswordToHide.Length.Equals(0))
            throw new ArgumentException("The password must not be empty.");

         this.Key = GenerateKey();

         string AuthTest = this.Encrypt(PasswordToHide);
         string TestResults = this.Decrypt(AuthTest);

         return PasswordToHide.Equals(TestResults);
      }

      #region IDisposable Members

      private bool isDisposed = false;

      public void Dispose()
      {
         if (!isDisposed)
            Dispose(true);
      }

      private void Dispose(bool disposing)
      {
         isDisposed = true;
         if (disposing)
         {
            // Overwrite the memory with random number.
            // I do not know if this is needed, but removing this info from the memory is a little more secure.

            for (int i = 0; i < 5; i++)
            {
               Random random = new Random(i);
               int randomNum = random.Next(9);
               char randomNumChar = randomNum.ToString().ToCharArray()[0];

               string tempString = string.Empty;
               tempString = tempString.PadRight(this.Password.Length, randomNumChar);
               this.Password = tempString;
            }

            this.Password = string.Empty;
         }
      }

      #endregion IDisposable Members
   }
}