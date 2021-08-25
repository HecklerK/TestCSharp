using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace TestCSharp
{
    public class GetD_F
    {

        public class file
        {
            public string Path { get; set; }
            public string Name { get; set; }
            public string MimeType { get; set; }
            public long Size { get; set; }

            public file(string Path)
            {
                this.Path = Path;
                Name = new FileInfo(Path).Name;
                MimeType = MimeMapping.MimeUtility.GetMimeMapping(Name);
                Size = new FileInfo(Path).Length;
            }
        }

        public class directory
        {
            public string Path { get; set; }
            public string Name { get; set; }
            public long Size = 0;
            public List<directory> directories = new List<directory>();
            public List<file> files = new List<file>();


            public int amount = 0;
            public Dictionary<string, int> MimeTypeAmount = new Dictionary<string, int>();
            public Dictionary<string, long> MimeTypeSize = new Dictionary<string, long>();


            public directory (string Path)
            {
                this.Path = Path;
                Name = new DirectoryInfo(Path).Name;
                foreach (var dir in Directory.GetDirectories(Path))
                {
                    var item = new directory(dir);
                    directories.Add(item);
                    amount += item.amount;
                    foreach (var mime in item.MimeTypeAmount)
                    {
                        if (MimeTypeAmount.ContainsKey(mime.Key))
                        {
                            MimeTypeAmount[mime.Key] += mime.Value;
                            MimeTypeSize[mime.Key] += item.MimeTypeSize[mime.Key];
                        }
                        else
                        {
                            MimeTypeAmount.Add(mime.Key, mime.Value);
                            MimeTypeSize.Add(mime.Key, item.MimeTypeSize[mime.Key]);
                        }
                    }

                }
                foreach (var f in Directory.GetFiles(Path))
                {
                    var item = new file(f);
                    files.Add(item);
                    amount++;
                    if (MimeTypeAmount.ContainsKey(item.MimeType))
                    {
                        MimeTypeAmount[item.MimeType]++;
                        MimeTypeSize[item.MimeType] += item.Size;
                    }
                    else
                    {
                        MimeTypeAmount.Add(item.MimeType, 1);
                        MimeTypeSize.Add(item.MimeType, item.Size);
                    }
                }
                foreach (var dir in directories)
                {
                    Size += dir.Size;
                }
                foreach (var f in files)
                {
                    Size += f.Size;
                }
            }

            public string GetD_F_String(string space)
            {
                string D = "";
                D += space + Name + "/   " + Size + " бит\n";
                foreach (var dir in directories)
                {
                    D += dir.GetD_F_String(space + "   ");
                }

                foreach (var f in files)
                {
                    D += space + "   " + f.Name + "   MineType: " + f.MimeType + "   " + f.Size + " бит\n";
                }

                return D;
            }

            public string CreatHTML(int space)
            {
                string D = "";
                D += "<h3 style=\"transform: translate(" + space.ToString() + "px,0);\">" + Name + "/   " + Size + " бит </h3>";
                foreach (var dir in directories)
                {
                    D += dir.CreatHTML(space + 10);
                }

                foreach (var f in files)
                {
                    D += "<h4 style=\"transform: translate(" + (space + 10).ToString() + "px,0);\">" + f.Name + "   MineType: " + f.MimeType + "   " + f.Size + " бит </h4>";
                }

                return D;
            }

            public string GetMimeTypeAmount()
            {
                string D = "";
                D += "Всего файлов: " + amount.ToString() + "\n";
                foreach (var mime in MimeTypeAmount)
                {
                    D += "количество типа " + mime.Key + ": " + mime.Value.ToString() + "(" + String.Format( "{0:F2}", mime.Value * 100.0 / amount) + " %)\n";
                }
                D += "------------------------------------------------------------\n";
                return D;
            }

            public string GetMimeTypeSize()
            {
                string D = "";
                D += "Средний размер файла для каждого типа\n";
                foreach (var mime in MimeTypeSize)
                {
                    D += "средний размер для типа " + mime.Key + ": " + (mime.Value / MimeTypeAmount[mime.Key]).ToString() + " бит\n";
                }
                D += "------------------------------------------------------------\n";
                return D;
            }
        }
    }
}
