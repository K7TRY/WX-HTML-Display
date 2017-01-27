using System;
using System.IO;
using System.Net;
using System.Text;

namespace WeatherUpdate
{
   public static class WebPut
   {
      private const int putTimeoutMil = 300000; // Five Minutes

      public static void uploadFile(string serverUrl, string fileContent)
      {
         int baselineDelayMs = 10000;

         const int MaxAttempts = 4;
         Random random = new Random();
         int attempt = 0;

         string result = String.Empty;

         while (++attempt <= MaxAttempts)
         {
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(serverUrl);

            userSettings user = new userSettings();

            req.Credentials = new NetworkCredential(user.UserName, user.Password, user.Domain);

            req.PreAuthenticate = true;
            req.Method = @"PUT";
            req.Accept = @"text/xml";
            req.Headers.Add(@"Translate", @"f");
            req.Headers.Add(@"Overwrite", @"T");

            req.ContentLength = fileContent.Length;
            req.SendChunked = false;
            req.AllowWriteStreamBuffering = true;
            req.AllowAutoRedirect = true;
            req.ContinueTimeout = putTimeoutMil;
            req.ReadWriteTimeout = putTimeoutMil;
            req.Timeout = putTimeoutMil;
            req.KeepAlive = false;

            try
            {
               using (StreamWriter writer = new StreamWriter(req.GetRequestStream(), Encoding.UTF8))
               {
                  writer.Write(fileContent);
               }

               using (WebResponse response = req.GetResponse())
               using (StreamReader reader = new StreamReader(response.GetResponseStream()))
               {
                  result = reader.ReadToEnd();
               }

               if (result != String.Empty)
               {
                  TextFileLogger.LogThis(result, LogType.Info);
               }

               break;
            }
            catch (ProtocolViolationException ProtEx)
            {
               TextFileLogger.LogThis("Attempt: " + attempt.ToString() + "\r\nA protocol violation exception occurred. This may be due to another error.\r\n" + ProtEx.Message, LogType.Error);

               System.Threading.Thread.Sleep(baselineDelayMs + random.Next((int)(baselineDelayMs * 0.5), baselineDelayMs));
               baselineDelayMs *= 2;
            }
            catch (WebException ex)
            {
               TextFileLogger.LogThis("Could not upload the data file to " + serverUrl + " using the username " + user.UserName + ".\r\n\r\n" + ex.Message, LogType.Error);

               System.Threading.Thread.Sleep(baselineDelayMs + random.Next((int)(baselineDelayMs * 0.5), baselineDelayMs));
               baselineDelayMs *= 2;
            }
         }
      }
   }

   public class userSettings : AuthenticationInfo
   {
      public userSettings()
      {
         bool saveSettings = false;

         if (Properties.Settings.Default.IV.Equals(""))
            createInitialVector();

         // To decrypt the password we need three things, the encrypted password, the key, and the IV.
         this.Key = @""; // Store the key in the exe.
         this.IV = Properties.Settings.Default.IV; // Store the IV in the settings.

         if (Properties.Settings.Default.Password.Length == 0){
            Properties.Settings.Default.Password = @"";
            saveSettings = true;
         }
         this.EncryptedPassword = Properties.Settings.Default.Password;

         if (Properties.Settings.Default.Domain.Length == 0)
         {
            Properties.Settings.Default.Domain = @"";
            saveSettings = true;
         }
         this.Domain = Properties.Settings.Default.Domain;

         if (Properties.Settings.Default.UserName.Length == 0) { 
            Properties.Settings.Default.UserName = @"";
            saveSettings = true;
         }
         this.UserName = Properties.Settings.Default.UserName;

         if(saveSettings)
            Properties.Settings.Default.Save();
      }

      public void createInitialVector()
      {
         Properties.Settings.Default.IV = this.GenerateIV();
         Properties.Settings.Default.Save();
      }
   }
}
