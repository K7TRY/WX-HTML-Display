using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace WeatherUpdate
{
   public static class SerializeObject
   {
      public static XmlSerializerNamespaces GetNamespaces()
      {
         XmlSerializerNamespaces ns;
         ns = new XmlSerializerNamespaces();
         ns.Add("xs", "http://www.w3.org/2001/XMLSchema");
         ns.Add("xsi", "http://www.w3.org/2001/XMLSchema-instance");
         return ns;
      }

      public static string TargetNamespace
      {
         get
         {
            return "http://www.w3.org/2001/XMLSchema";
         }
      }

      /// <summary>
      /// Returns the local executable path.
      /// </summary>
      /// <returns></returns>
      public static string XmlFilePath()
      {
         AppDomain Ad = AppDomain.CurrentDomain;
         return Ad.BaseDirectory;
      }

      public static string GetXmlFilenamePath()
      {
         AppDomain Ad = AppDomain.CurrentDomain;
         string AppName = Ad.FriendlyName.Substring(0, Ad.FriendlyName.IndexOf("."));
         return Ad.BaseDirectory + AppName + "Settings.xml";
      }

      public static void SaveToXmlFile(object Obj)
      {
         SaveToXmlFile(Obj, GetXmlFilenamePath());
      }

      public static void SaveToXmlFile(object Obj, string filePathAndName)
      {
         XmlSerializer Serializer = new XmlSerializer(Obj.GetType());
         try
         {
            using (XmlTextWriter xmlWriter = new XmlTextWriter(filePathAndName, Encoding.Unicode))
            {
               xmlWriter.Formatting = Formatting.Indented;
               xmlWriter.Namespaces = true;
               Serializer.Serialize(xmlWriter, Obj);
               xmlWriter.Close();
            }
         }
         catch (IOException IoEx)
         {
            throw new Exception("The file could not be saved.", IoEx.InnerException);
         }
      }

      public static string ToXmlString(object Obj)
      {
         XmlSerializer ser;
         ser = new XmlSerializer(Obj.GetType(), SerializeObject.TargetNamespace);
         MemoryStream memStream;
         memStream = new MemoryStream();
         XmlTextWriter xmlWriter;
         xmlWriter = new XmlTextWriter(memStream, Encoding.UTF8);
         xmlWriter.Namespaces = true;
         ser.Serialize(xmlWriter, Obj, SerializeObject.GetNamespaces());
         xmlWriter.Close();
         memStream.Close();
         string xml;
         xml = Encoding.UTF8.GetString(memStream.GetBuffer());
         xml = xml.Substring(xml.IndexOf(Convert.ToChar(60)));
         xml = xml.Substring(0, (xml.LastIndexOf(Convert.ToChar(62)) + 1));
         return xml;
      }

      public static object LoadFromXmlFile(System.Type ObjType)
      {
         return LoadFromXmlFile(ObjType, GetXmlFilenamePath());
      }

      public static object LoadFromXmlFile(System.Type ObjType, string filePathAndName)
      {
         XmlSerializer Serializer = new XmlSerializer(ObjType);
         try
         {
            using (FileStream XmlFileStream = new FileStream(filePathAndName, FileMode.Open))
            {
               using (XmlReader XmlDataReader = new XmlTextReader(XmlFileStream))
               {
                  object ReturnObject = (object)Serializer.Deserialize(XmlDataReader);
                  XmlDataReader.Close();
                  XmlFileStream.Close();
                  return ReturnObject;
               }
            }
         }
         catch (FileNotFoundException FNF)
         {
            throw new Exception("The file could not be found.", FNF.InnerException);
         }
      }

      public static object FromXmlString(string Xml, System.Type ObjType)
      {
         XmlSerializer ser;
         ser = new XmlSerializer(ObjType);
         StringReader stringReader;
         stringReader = new StringReader(Xml);
         XmlTextReader xmlReader;
         xmlReader = new XmlTextReader(stringReader);
         object obj;
         obj = ser.Deserialize(xmlReader);
         xmlReader.Close();
         stringReader.Close();
         return obj;
      }
   }
}