using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Reflection;
using Newtonsoft.Json;

namespace MermaidNetHtmlBuilder
{
    public class MermaidHtmlBuilder
    {
        private Stream LoadResource(string resourceName)
        {
            var assembly = Assembly.GetExecutingAssembly();

            return assembly.GetManifestResourceStream(resourceName);
        }

        private string LoadResourceString(string resourceName)
        {
            using (Stream stream = LoadResource(resourceName))
            using (StreamReader reader = new StreamReader(stream))
            {
                return reader.ReadToEnd();
            }
        }

        private ZipArchiveEntry CreateFromResource(ZipArchive zipArchive, string entryName, string resourceName)
        {
            var entry = zipArchive.CreateEntry(entryName);
            using (var str = entry.Open())
            using (var resourceStream = LoadResource(resourceName))
            {
                resourceStream.CopyTo(str);
            }

            return entry;
        }

        public byte[] BuildZip(string graph, IDictionary<string, string> descriptions)
        {

            byte[] bytes;

            using (var memoryStream = new MemoryStream())
            {
                using (var zipArchive = new ZipArchive(memoryStream, ZipArchiveMode.Create))
                {
                    CreateFromResource(zipArchive, "mermaid.min.js", "MermaidNetHtmlBuilder.Data.mermaid.min.js");
                    CreateFromResource(zipArchive, "popper.min.js", "MermaidNetHtmlBuilder.Data.popper.min.js");
                    CreateFromResource(zipArchive, "tippy.min.js", "MermaidNetHtmlBuilder.Data.tippy.min.js");

                    var template = LoadResourceString("MermaidNetHtmlBuilder.Data.template.html");

                    var descriptionsJson = JsonConvert.SerializeObject(descriptions, Formatting.None);

                    var resultHtml = template.Replace("##GRAPH##", graph).Replace("##DESCRIPTIONS##", descriptionsJson);

                    var entry = zipArchive.CreateEntry("index.html");
                    using (var str = entry.Open())
                    using (var writer = new StreamWriter(str))
                    {
                        writer.Write(resultHtml);
                    }
                }

                bytes = memoryStream.ToArray();



            }

            return bytes;
        }

        public void SaveZip(string zipPath, string graph, IDictionary<string, string> descriptions)
        {
            var bytes = BuildZip(graph, descriptions);
            using (var str1 = File.OpenWrite(zipPath))
            {
                str1.Write(bytes, 0, bytes.Length);
            }
        }
    }
}
