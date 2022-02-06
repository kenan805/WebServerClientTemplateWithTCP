using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Text.Json;
using System.Threading.Tasks;

namespace HttpServer
{
    class Program
    {
        public static List<Car> Cars { get; set; }
        static void Main(string[] args)
        {
            Cars = new List<Car>()
            {
                new Car(){ Id=1, Vendor="BMW", Year=2020, Volume=3.5},
                new Car(){ Id=2, Vendor="Lada", Year=2014, Volume=1.6},
                new Car(){ Id=3, Vendor="Mercedes", Year=2018, Volume=2.4},
                new Car(){ Id=4, Vendor="Hyundai", Year=2016, Volume=1.8}
            };

            Console.WriteLine(">>> Server app <<<");
            Console.WriteLine("Listener...");
            var ipAddress = IPAddress.Parse("127.0.0.1");
            var listener = new TcpListener(ipAddress, 45001);
            listener.Start(100);

            while (true)
            {
                var client = listener.AcceptTcpClient();
                Console.WriteLine($"{client.Client.RemoteEndPoint} connected.");

                var stream = client.GetStream();
                var br = new BinaryReader(stream);
                var bw = new BinaryWriter(stream);

                //Task.Run(() =>
                //{
                while (true)
                {
                    var input = br.ReadString();
                    var cmd = JsonSerializer.Deserialize<Command>(input);
                    if (cmd == null) continue;
                    Console.WriteLine("Command: " + cmd.CmdName);

                    
                    switch (cmd.CmdName)
                    {
                        case Command.HttpGet:
                            bw.Write(Get(Cars));
                            break;
                        case Command.HttpPost:
                            var carJson = br.ReadString();
                            cmd.Value = JsonSerializer.Deserialize<Car>(carJson);
                            bw.Write(Post(cmd.Value, Cars));
                            break;
                        case Command.HttpPut:
                            carJson = br.ReadString();
                            cmd.Value = JsonSerializer.Deserialize<Car>(carJson);
                            bw.Write(Put(cmd.Value, Cars));
                            break;
                        case Command.HttpDelete:
                            carJson = br.ReadString();
                            cmd.Value = JsonSerializer.Deserialize<Car>(carJson);
                            bw.Write(Delete(cmd.Value, Cars));
                            break;
                        default:
                            break;
                    }
                }
                //});
            }
        }

        public static string Get(List<Car> cars)
        {
            string carsJson = JsonSerializer.Serialize(cars);
            return carsJson;
        }

        public static string Post(Car car, List<Car> cars)
        {
            cars.Add(car);
            return "Car ugurla add olundu!";
        }

        public static string Put(Car car, List<Car> cars)
        {
            if (cars.Exists(c => c.Id == car.Id))
            {
                foreach (var c in cars)
                {
                    if (c.Id == car.Id)
                    {
                        int index = cars.IndexOf(c);
                        cars[index] = car;
                        break;
                    }
                }
                return "Car ugurla update olundu!";
            }
            else
            {
                return "Bu id-de car tapilmadi!";
            }
        }

        public static string Delete(Car car, List<Car> cars)
        {
            if (cars.Exists(c => c.Id == car.Id))
            {
                foreach (var c in cars)
                {
                    if (c.Id == car.Id)
                    {
                        cars.Remove(c);
                        break;
                    }
                }
                return "Car ugurla delete olundu!";
            }
            else
            {
                return "Bu id-de car tapilmadi!";
            }
        }
    }

}
