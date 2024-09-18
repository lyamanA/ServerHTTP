using System.Net.Sockets;
using System.Net;
using ServerHTTP;
using System.Text.Json;

var carService = new CarService(); 

var ip = IPAddress.Loopback;
var port = 27001;
var listener = new TcpListener(ip, port);

listener.Start();
Console.WriteLine("Server started. Listening for connections...");


while (true)
{
    var client = listener.AcceptTcpClient();
    var stream = client.GetStream();
    var br = new BinaryReader(stream);
    var bw = new BinaryWriter(stream);

    bool keepAlive = true;

    try
    {
        while (keepAlive)  
        {
            var input = br.ReadString();
            var command = JsonSerializer.Deserialize<Command>(input);
            Console.WriteLine($"Received command: {command.HttpMethod}");

            switch (command.HttpMethod.ToUpper())
            {
                case Command.GET:
                    var carListJson = carService.GetCars();
                    bw.Write(carListJson);
                    break;

                case Command.POST:
                    if (command.Value != null)
                    {
                        var response = carService.AddCar(command.Value);
                        bw.Write(response);
                    }
                    else
                    {
                        bw.Write("No car data provided");
                    }
                    break;

                case Command.PUT:
                    if (command.Value != null)
                    {
                        var response = carService.UpdateCar(command.Value);
                        bw.Write(response);
                    }
                    else
                    {
                        bw.Write("No car data provided");
                    }
                    break;

                case Command.DELETE:
                    if (command.Value != null)
                    {
                        var response = carService.DeleteCar(command.Value.Id);
                        bw.Write(response);
                    }
                    else
                    {
                        bw.Write("No car data provided");
                    }
                    break;

                case Command.EXIT:
                    keepAlive = false;
                    bw.Write("Connection closed");
                    client.Close();
                    break;

                default:
                    bw.Write("Unknown command");
                    break;
            }
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error: {ex.Message}");
        bw.Write("Error processing request");
    }
    finally
    {
        client.Close();
    }
}