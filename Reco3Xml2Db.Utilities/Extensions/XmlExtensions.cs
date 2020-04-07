using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Reco3Xml2Db.Utilities.Extensions {
  public static class XmlExtensions {
    public static string UnformatXml(this string xml) {
      string result = "";

      StringBuilder sb = new StringBuilder(xml);


      using (MemoryStream stream = new MemoryStream()) {
        using (XmlTextWriter writer = new XmlTextWriter(stream, Encoding.Unicode)) {
          XmlDocument doc = new XmlDocument();

          try {
            // Load the XmlDocument with the XML.
            doc.LoadXml(xml);

            writer.Formatting = Formatting.None;

            // Write the XML into a formatting XmlTextWriter
            doc.WriteContentTo(writer);
            writer.Flush();
            stream.Flush();

            // Have to rewind the MemoryStream in order to read
            // its contents.
            stream.Position = 0;

            // Read MemoryStream contents into a StreamReader.
            StreamReader sReader = new StreamReader(stream);

            // Extract the text from the StreamReader.
            string formattedXml = sReader.ReadToEnd();

            result = formattedXml;
          }
          catch (XmlException ex) {
            // Handle the exception
            throw new XmlException(ex.Message);
          }
        }
      }
      //stream.Close();
      //writer.Close();

      return result;
    }
  }
}
