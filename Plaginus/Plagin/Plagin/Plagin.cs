using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;

namespace Plagin
{
    public class Plagin : IPlagin
    {
        public void Compress()
        {
            List<string> jsonFiles = JoinJson();
            foreach (string jsonefiles in jsonFiles)
            {
                using (FileStream sourceStream = new FileStream(jsonefiles + ".json", FileMode.Open))
                {
                    // поток для записи сжатого файла
                    using (FileStream targetStream = File.Create(jsonefiles + ".zip"))
                    {
                        // поток архивации
                        using (GZipStream compressionStream = new GZipStream(targetStream, CompressionMode.Compress))
                        {
                            sourceStream.CopyTo(compressionStream); // копируем байты из одного потока в другой
                        }
                    }
                }
            }
        }

        private List<string> JoinJson()
        {
            const string dirName = "C:\\Users\\evdan\\My_Project\\Projects\\OOPHS\\Lab2OOP\\Lab2OOP\\bin\\Debug";
            string[] files = Directory.GetFiles(dirName);
            List<string> jsonFiles = new List<string>();
            foreach (string file in files)
            {
                if (file.IndexOf(".json") > 0)
                {
                    FileInfo fileInf = new FileInfo(file);
                    var indesJson = fileInf.FullName.LastIndexOf('.');
                    jsonFiles.Add(fileInf.FullName.Remove(indesJson));
                }
            }
            return jsonFiles;
        }

        public void Decompress()
        {
            List<string> jsonFiles = JoinJson();
            foreach (string jsonefiles in jsonFiles)
            {
                using (FileStream sourceStream = new FileStream(jsonefiles + ".zip", FileMode.Open))
                {
                    // поток для записи восстановленного файла
                    using (FileStream targetStream = File.Create(jsonefiles + ".json"))
                    {
                        // поток разархивации
                        using (GZipStream decompressionStream = new GZipStream(sourceStream, CompressionMode.Decompress))
                        {
                            decompressionStream.CopyTo(targetStream);
                        }
                    }
                }
            }
        }
    }
}
