using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using eApi;
using ApiSerialComm;

namespace eApi.Filesystem
{
    class filesystem
    {
        public struct FileFormat_t
        {
            public string id;
            public string name;
            public ApiSerialComm.SerialComm.Packet_t packet;
        }

        static public void saveToFile(SerialComm.Packet_t product, string name, string id)
        {
            string save = "";
            save += name + "|";
            save += System.Convert.ToString((byte)product.type) + "|";
            save += System.Convert.ToString((byte)product.message.command) + "|";
            save += System.Convert.ToString((byte)product.message.parameter) + "|";
            save += System.Convert.ToString((byte)product.dataLength) + "|";
            save += System.Convert.ToBase64String(product.data) + "|";

            System.IO.File.WriteAllText("C:/Users/Public/eApi/" + id + ".settings", save);
        }

        static public void deleteFile(string id)
        {
            FileFormat_t[] files = loadFromFiles();

            for (int i = 0; i < files.Length; i++)
            {
                if (id == files[i].id)
                {
                    System.IO.File.Delete("C:/Users/Public/eApi/" + id + ".settings");
                }
            }
        }

        static public FileFormat_t[] loadFromFiles()
        {
            FileFormat_t[] product = new FileFormat_t[0];

            ApiSerialComm.SerialComm.Packet_t p = new ApiSerialComm.SerialComm.Packet_t();
            string save = "";
            string id = "";
            string name = "";
            string type = "";
            string command = "";
            string parameter = "";
            string dataLength = "";
            string data = "";
            int endOfParam = 0;

            if (!System.IO.Directory.Exists("C:/Users/Public/eApi"))
            {
                System.IO.Directory.CreateDirectory("C:/Users/Public/eApi");
            }
            string[] filenames = System.IO.Directory.GetFiles("C:/Users/Public/eApi", "*.settings");

            for (int i = 0; i < filenames.Length; i++)
            {
                try
                {
                    System.Array.Resize(ref product, i + 1);
                    save = System.IO.File.ReadAllText(filenames[i]);

                    id = System.IO.Path.GetFileNameWithoutExtension(filenames[i]);
                    product[i].id = id;

                    endOfParam = save.IndexOf('|');
                    name = save.Substring(0, endOfParam);
                    save = save.Substring(endOfParam + 1);
                    product[i].name = name;

                    endOfParam = save.IndexOf('|');
                    type = save.Substring(0, endOfParam);
                    save = save.Substring(endOfParam + 1);
                    p.type = (ApiSerialComm.SerialComm.PacketType_t)System.Convert.ToByte(type);

                    endOfParam = save.IndexOf('|');
                    command = save.Substring(0, endOfParam);
                    save = save.Substring(endOfParam + 1);
                    p.message.command = System.Convert.ToByte(command);

                    endOfParam = save.IndexOf('|');
                    parameter = save.Substring(0, endOfParam);
                    save = save.Substring(endOfParam + 1);
                    p.message.parameter = System.Convert.ToByte(parameter);

                    endOfParam = save.IndexOf('|');
                    dataLength = save.Substring(0, endOfParam);
                    save = save.Substring(endOfParam + 1);
                    p.dataLength = System.Convert.ToByte(dataLength);

                    endOfParam = save.IndexOf('|');
                    data = save.Substring(0, endOfParam);
                    save = save.Substring(endOfParam + 1);
                    p.data = System.Convert.FromBase64String(data);

                    product[i].packet = p;
                }
                catch { }
            }

            return product;
        }

        static public FileFormat_t getDataFromFile(string path)
        {
            FileFormat_t product = new FileFormat_t();

            ApiSerialComm.SerialComm.Packet_t p = new ApiSerialComm.SerialComm.Packet_t();
            string save = "";
            string id = "";
            string name = "";
            string type = "";
            string command = "";
            string parameter = "";
            string dataLength = "";
            string data = "";
            int endOfParam = 0;

            if (System.IO.Path.GetFileNameWithoutExtension(path).Contains("config_"))
            {
                id = System.IO.Path.GetFileNameWithoutExtension(path);
                product.id = id;
            }
            else
                product.id = "custom_" + System.IO.Directory.GetFiles("C:/Users/Public/eApi", "*.settings").Length.ToString();

            save = System.IO.File.ReadAllText(path);

            endOfParam = save.IndexOf('|');
            name = save.Substring(0, endOfParam);
            save = save.Substring(endOfParam + 1);
            product.name = name;

            endOfParam = save.IndexOf('|');
            type = save.Substring(0, endOfParam);
            save = save.Substring(endOfParam + 1);
            p.type = (ApiSerialComm.SerialComm.PacketType_t)System.Convert.ToByte(type);

            endOfParam = save.IndexOf('|');
            command = save.Substring(0, endOfParam);
            save = save.Substring(endOfParam + 1);
            p.message.command = System.Convert.ToByte(command);

            endOfParam = save.IndexOf('|');
            parameter = save.Substring(0, endOfParam);
            save = save.Substring(endOfParam + 1);
            p.message.parameter = System.Convert.ToByte(parameter);

            endOfParam = save.IndexOf('|');
            dataLength = save.Substring(0, endOfParam);
            save = save.Substring(endOfParam + 1);
            p.dataLength = System.Convert.ToByte(dataLength);

            endOfParam = save.IndexOf('|');
            data = save.Substring(0, endOfParam);
            save = save.Substring(endOfParam + 1);
            p.data = System.Convert.FromBase64String(data);

            product.packet = p;

            return product;
        }
    }
}
