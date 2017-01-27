using System;
using System.ComponentModel;
using System.IO;
using System.Reflection;

namespace WeatherUpdate
{
   public enum LogType
   {
      [Description("Info")]
      Info,

      [Description("Warning")]
      Warning,

      [Description("Error")]
      Error
   }

   public static class TextFileLogger
   {
      public static void LogThis(string LogMessage, LogType LogMessageType)
      {
         string LogFileName = GetLogFilename();
         Log(LogMessage, LogMessageType, LogFileName);
      }

      public static void LogThis(string LogMessage, LogType LogMessageType, string LogPath)
      {
         string LogFileName = GetLogFilename(LogPath);
         Log(LogMessage, LogMessageType, LogFileName);
      }

      private static void Log(string LogMessage, LogType LogMessageType, string LogFileName)
      {
         Console.WriteLine(LogMessage);

         string LogEntryMessage = CreateLogEntry(LogMessageType, LogMessage);
         StreamWriter LogStreamWriter = null;

         try
         {
            LogStreamWriter = File.AppendText(LogFileName);
            LogStreamWriter.WriteLine(LogEntryMessage);
         }
         catch (Exception)
         {
            // TODO figure out what I want to do when the log file cannot be written to.
         }
         finally
         {
            if (LogStreamWriter != null)
            {
               LogStreamWriter.Close();
               LogStreamWriter.Dispose();
            }
         }
      }

      private static string GetLogFilename()
      {
         AppDomain Ad = AppDomain.CurrentDomain;
         string AppName = Ad.FriendlyName.Substring(0, Ad.FriendlyName.IndexOf("."));
         return Ad.BaseDirectory + AppName + "-" + DateTime.Now.ToString("yyyy-MM-dd") + ".log";
      }

      private static string GetLogFilename(string path)
      {
         if (!path.EndsWith("\\"))
            path = path + "\\";

         try
         {
            CreateFolderIfNeeded(path);
            AppDomain Ad = AppDomain.CurrentDomain;
            string AppName = Ad.FriendlyName.Substring(0, Ad.FriendlyName.IndexOf("."));
            return path + AppName + "-" + DateTime.Now.ToString("yyyy-MM-dd") + ".log";
         }
         catch
         {
            return GetLogFilename();
         }
      }

      private static string CreateLogEntry(LogType logtype, string message)
      {
         string stub = DateTime.Now.ToString("yyyy-MM-dd\tHH:mm:ss\t");
         stub += GetEnumDescription(logtype) + ",\t";
         stub += message;
         return stub;
      }

      public static void CreateFolderIfNeeded(string folderPath)
      {
         try
         {
            if (!Directory.Exists(folderPath))
               Directory.CreateDirectory(folderPath);
         }
         catch (Exception ex)
         {
            TextFileLogger.LogThis("The folder could not be created. " + ex.Message, LogType.Error);
         }
      }

      public static string ReadLogFile(string folderPath)
      {
         StreamReader sr = new StreamReader(GetLogFilename(folderPath), true);
         return sr.ReadToEnd();
      }

      public static string GetEnumDescription(Enum value)
      {
         FieldInfo fi = value.GetType().GetField(value.ToString());

         DescriptionAttribute[] attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);

         if (attributes != null && attributes.Length > 0)
            return attributes[0].Description;
         else
            return value.ToString();
      }
   }
}