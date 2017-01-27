using Microsoft.Win32.TaskScheduler;
using System;
using System.Diagnostics;
using System.IO;
using System.Net;

namespace CreateScheduledTask
{
   class Program
   {
      static void Main(string[] args)
      {
         foreach (string arg in args)
         {
            switch (arg.Substring(0, 2).ToUpper())
            {
               case "/R":
                  removeScheduledTask();
                  break;

               default:
                  //installWebUi();
                  addScheduledTask();
                  break;
            }
         }

         if (args.Length == 0)
         {
            //installWebUi();
            addScheduledTask();
         }
      }

      private static void addScheduledTask()
      {
         Console.WriteLine("Adding a Windows Scheduled Task");
         Console.WriteLine("Please verify that the task is created with the timing you would like.");

         try
         {
            // using https://taskscheduler.codeplex.com/
            using (TaskService ts = new TaskService())
            {
               TaskDefinition td = ts.NewTask();
               td.RegistrationInfo.Description = Properties.Settings.Default.Description;

               TimeTrigger tt = new TimeTrigger(getMinutesUntilNextHalfHour());
               tt.Repetition.Interval = TimeSpan.FromMinutes(30);
               td.Triggers.Add(tt);

               AppDomain Ad = AppDomain.CurrentDomain;
               string AppName = Ad.FriendlyName.Substring(0, Ad.FriendlyName.IndexOf("."));

               td.RegistrationInfo.Source = "MAF CreateScheduledTask App";
               td.Actions.Add(new ExecAction(Path.Combine(Ad.BaseDirectory, Properties.Settings.Default.AppFileName), null, Ad.BaseDirectory));

               td.Principal.UserId = "SYSTEM";
               td.Principal.RunLevel = TaskRunLevel.LUA;

               td.Settings.MultipleInstances = TaskInstancesPolicy.IgnoreNew;
               td.Settings.ExecutionTimeLimit = TimeSpan.FromMinutes(5);

               ts.RootFolder.RegisterTaskDefinition(Properties.Settings.Default.AppFriendlyName, td);
            }
         }
         catch (Exception)
         {
            Console.WriteLine("Unable to create a scheduled task.");
         }

         try
         {
            Process p = new Process();
            p.StartInfo.FileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Windows), "system32", "taskschd.msc");
            p.StartInfo.Arguments = "s";
            p.Start();
         }
         catch (Exception)
         {
            Console.WriteLine("Unable to start the Task Scheduler Management Console.");
         }


         Console.WriteLine("Installation is now complete. Please press a key on the keyboard to continue.");
         Console.ReadKey();
      }

      private static void removeScheduledTask()
      {
         using (TaskService ts = new TaskService())
         {
            ts.RootFolder.DeleteTask(Properties.Settings.Default.AppFriendlyName, false);
         }
      }

      private static void EnumFolderTasks(TaskFolder fld)
      {
         foreach (Task task in fld.Tasks)
            Console.WriteLine(task);
      }

      private static DateTime getMinutesUntilNextHalfHour()
      {
         // I want the update to happen before the half hour so that the UI can update on the half hour and get the latest data.

         DateTime returnDt = DateTime.Now.AddMinutes(-DateTime.Now.Minute);

         if (DateTime.Now.Minute >= 28 && DateTime.Now.Minute <= 58)
            return returnDt.AddHours(1).AddMinutes(-2);
         else if (DateTime.Now.Minute < 28)
            return returnDt.AddMinutes(28);
         else
            return returnDt.AddHours(1).AddMinutes(28);
      }

      private static void installWebUi()
      {
         string sharepointUrl = Properties.Settings.Default.sharepointUrl; // @"https://sp2013.maf.org";
         string serverDataPath = Properties.Settings.Default.serverDataPath;

         while (true) // Loop indefinitely
         {
            Console.WriteLine("Please enter the URL for the SharePoint server [" + sharepointUrl + "]:");
            Console.WriteLine("");
            string line = Console.ReadLine();
            Uri sharePointUri = new Uri(sharepointUrl);

            if (line.Length > 0)
            {
               try
               {
                  sharePointUri = new Uri(line);
                  if (sharePointUri.IsAbsoluteUri)
                     sharepointUrl = sharePointUri.ToString();

                  break;
               }
               catch (System.UriFormatException ex)
               {
                  Console.WriteLine("");
                  Console.WriteLine("Your entry is not a valid URL.");
                  Console.WriteLine(ex);
               }
            }
            else
            {
               break;
            }

            string userInput = "";
            // TODO ask the user for the site collection name

            if (userInput.Length > 0)
               serverDataPath = string.Format(Properties.Settings.Default.siteDataPath, userInput);


         }

         // This is commented out because the install user does not need to change the path for the webUI or data files. 
         //Console.WriteLine("Please enter the path for the web script to be copied [" + serverDataPath + "]:");
         //string serverDataPathEntry = Console.ReadLine().Trim();

         //if (serverDataPathEntry.Length > 0)
         //{
         //   if (!serverDataPathEntry[0].Equals('/'))
         //      serverDataPathEntry = @"/" + serverDataPathEntry;

         //   if (!serverDataPathEntry[serverDataPathEntry.Length - 1].Equals('/'))
         //      serverDataPathEntry = serverDataPathEntry + @"/";

         //   serverDataPath = serverDataPathEntry;
         //}

         //Console.WriteLine("Copying Weather Clock web files to the SharePoint server.");
         //Console.WriteLine("");
         //copyFilesToSharePointServer(sharepointUrl, serverDataPath);
      }

      //private static void copyFilesToSharePointServer(string sharepointUrl, string serverDataPath)
      //{
      //   AppDomain Ad = AppDomain.CurrentDomain;

      //   try
      //   {
      //      // TODO Update this to use HTTP Put.
      //      var webFiles = Directory.GetFiles(Path.Combine(Ad.BaseDirectory, "webUi\\"), "*.*", SearchOption.AllDirectories);
      //      using (ClientContext clientContext = new ClientContext(sharepointUrl))
      //      {
      //         Web web = clientContext.Web;
      //         List documentsList = clientContext.Web.Lists.GetByTitle("Site Assets");
      //         var fileCreationInformation = new FileCreationInformation();

      //         foreach (var webFile in webFiles)
      //         {
      //            var spFilePath = serverDataPath + webFile.Replace(Ad.BaseDirectory, "").Replace("\\", @"/").Replace(@"webUi/", "");
      //            var spFileFolders = spFilePath.Substring(0, spFilePath.LastIndexOf(@"/") + 1);

      //            FolderCollection folderColl = web.RootFolder.Folders;
      //            Folder newFolder = folderColl.Add(spFileFolders);

      //            byte[] documentStream = System.IO.File.ReadAllBytes(webFile);
      //            fileCreationInformation.Content = documentStream;
      //            fileCreationInformation.Overwrite = true;

      //            fileCreationInformation.Url = sharepointUrl + spFilePath;
      //            ClientOM.File uploadFile = documentsList.RootFolder.Files.Add(fileCreationInformation);

      //            uploadFile.ListItemAllFields.Update();
      //            clientContext.ExecuteQuery();
      //         }
      //      }
      //   }
      //   catch (ClientOM.ClientRequestException cEx)
      //   {
      //      Console.WriteLine("");
      //      Console.WriteLine("There was an error communicating with the SharePoint server. Please make sure that the account you are running this application from has permission to write to the SharePoint server, and you are logged in to the server using IE.");
      //      Console.WriteLine(cEx.Message);
      //   }
      //   catch (WebException wEx)
      //   {
      //      Console.WriteLine("");
      //      Console.WriteLine("There was an error communicating with the SharePoint server. This is due to the web transfer not succeeding.");
      //      Console.WriteLine(wEx.Message);
      //   }
      //   catch (IOException ex)
      //   {
      //      Console.WriteLine("");
      //      Console.WriteLine(ex.Message);
      //   }
      //}

      //public static async void uploadFile(string serverDestination, string uploadFilePath)
      //{
      //   int baselineDelayMs = 10000;

      //   const int MaxAttempts = 4;
      //   Random random = new Random();
      //   int attempt = 0;
      //   int errorLevel = 0;

      //   FileInfo localFile = new FileInfo(uploadFilePath);

      //   Uri fullDestinationPath = getServerUrl(serverDestination, localFile.Name);

      //   string result = String.Empty;

      //   while (++attempt <= MaxAttempts)
      //   {

      //      HttpWebRequest req = (HttpWebRequest)WebRequest.Create(fullDestinationPath);
      //      req.Credentials = new NetworkCredential();

      //      req.PreAuthenticate = true;
      //      req.Method = @"PUT";
      //      req.Accept = @"text/xml";
      //      req.Headers.Add(@"Translate", @"f");
      //      req.Headers.Add(@"Overwrite", @"T");

      //      req.ContentLength = localFile.Length;
      //      req.SendChunked = false;
      //      req.AllowWriteStreamBuffering = true;
      //      req.AllowAutoRedirect = true;
      //      req.ContinueTimeout = putTimeoutMil;
      //      req.ReadWriteTimeout = putTimeoutMil;
      //      req.Timeout = putTimeoutMil;
      //      req.KeepAlive = false;

      //      try
      //      {
      //         uploadFileStream(req, localFile.FullName);

      //         using (WebResponse response = req.GetResponse())
      //         using (StreamReader reader = new StreamReader(response.GetResponseStream()))
      //         {
      //            result = reader.ReadToEnd();
      //         }

      //         if (result != String.Empty)
      //         {
      //            TextFileLogger.LogThis(result, LogType.Info, logLocation);
      //         }

      //         TextFileLogger.LogThis("WebDav upload complete.", LogType.Info, logLocation);

      //         break;
      //      }
      //      catch (WebException ex)
      //      {
      //         TextFileLogger.LogThis("Could not upload file.\r\n" + ex.Message, LogType.Error, logLocation);
      //         errorLevel = 1;
      //      }
      //      catch (ProtocolViolationException ProtEx)
      //      {
      //         TextFileLogger.LogThis("A protocol violation exception occurred. This may be due to another error.\r\n" + ProtEx.Message, LogType.Error, logLocation);
      //         errorLevel = 1;
      //      }
      //   }

      //   if (errorLevel > 0)
      //   {
      //      if (attempt == MaxAttempts)
      //         return;

      //      int delayMs = baselineDelayMs + random.Next((int)(baselineDelayMs * 0.5), baselineDelayMs);
      //      await Task.Delay(delayMs);

      //      baselineDelayMs *= 2;
      //   }

      //   if (result.Length == 0)
      //   {
      //      result = "Backup zip file uploaded to " + fullDestinationPath.ToString();
      //   }

      //   TextFileLogger.LogThis(result, LogType.Info, logLocation);
      //}

      //private static Uri getServerUrl(webDavSettings settings, string fileName)
      //{
      //   if (settings.destinationServerPort == 0)
      //   {
      //      settings.destinationServerPort = 80;

      //      if (settings.HttpProtocol == httpProtocol.https)
      //         settings.destinationServerPort = 443;
      //   }

      //   UriBuilder uriB = new UriBuilder(SharedStatic.GetEnumDescription(settings.HttpProtocol)
      //      , settings.destinationServerName
      //      , settings.destinationServerPort
      //      , settings.destinationPath + fileName);

      //   return uriB.Uri;
      //}

      //private static void uploadFileStream(HttpWebRequest request, string filePath)
      //{
      //   try
      //   {
      //      using (FileStream fileStream = File.OpenRead(filePath))
      //      using (Stream requestStream = request.GetRequestStream())
      //      {
      //         requestStream.WriteTimeout = putTimeoutMil;

      //         int bufferSize = 1024;
      //         byte[] buffer = new byte[bufferSize];
      //         int byteCount = 0;

      //         while ((byteCount = fileStream.Read(buffer, 0, bufferSize)) > 0)
      //         {
      //            requestStream.Write(buffer, 0, byteCount);
      //         }
      //      }
      //   }
      //   catch (WebException ex)
      //   {
      //      TextFileLogger.LogThis("Server connection not available.\r\n" + ex.Message, LogType.Error, SharedStatic.PathWithoutFilename(filePath).Replace("_backup.zip", ""));
      //      throw (ex);
      //   }
      //}


   }
}
