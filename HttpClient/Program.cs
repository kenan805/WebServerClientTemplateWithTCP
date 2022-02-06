using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text.Json;

namespace HttpClient
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.ReadLine();
            Console.WriteLine(">>> Client app 1 <<<");
            var ipAddress = IPAddress.Parse("127.0.0.1");
            var port = 45001;
            var client = new TcpClient();
            client.Connect(ipAddress, port);

            var stream = client.GetStream();
            var br = new BinaryReader(stream);
            var bw = new BinaryWriter(stream);

            while (true)
            {
                string inputCmd = Console.ReadLine().ToUpper();
                Command cmd = null;
                string response = null;
                switch (inputCmd)
                {
                    case Command.HttpGet:
                        cmd = new Command { CmdName = inputCmd };
                        bw.Write(JsonSerializer.Serialize(cmd));
                        response = br.ReadString();

                        var cars = JsonSerializer.Deserialize<List<Car>>(response);
                        foreach (var c in cars)
                            Console.WriteLine($"Car id: {c.Id} Car vendor: {c.Vendor} Car year: {c.Year} Car volume: {c.Volume}");
                        break;
                    case Command.HttpPost:
                        cmd = new Command { CmdName = inputCmd };
                        bw.Write(JsonSerializer.Serialize(cmd));
                        var newCar = new Car() { Id = 7, Vendor = "Porsche", Volume = 0.0, Year = 2022 };
                        bw.Write(JsonSerializer.Serialize(newCar));
                        response = br.ReadString();
                        Console.WriteLine(response);
                        break;
                    case Command.HttpPut:
                        cmd = new Command { CmdName = inputCmd };
                        bw.Write(JsonSerializer.Serialize(cmd));
                        var putCar = new Car() { Id = 1, Vendor = "Kia", Volume = 2.7, Year = 2022 };
                        bw.Write(JsonSerializer.Serialize(putCar));
                        response = br.ReadString();
                        Console.WriteLine(response);
                        break;
                    case Command.HttpDelete:
                        cmd = new Command { CmdName = inputCmd };
                        bw.Write(JsonSerializer.Serialize(cmd));
                        var deleteCar = new Car() { Id = 7 };
                        bw.Write(JsonSerializer.Serialize(deleteCar));
                        response = br.ReadString();
                        Console.WriteLine(response);
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
