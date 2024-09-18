using System.Net;
using System.Net.Sockets;
using System.Text.Json;
using ClientHTTP;

var ip = IPAddress.Parse("127.0.0.1"); 
var port = 27001;
var client = new TcpClient();
client.Connect(ip, port);

var stream = client.GetStream();
var br = new BinaryReader(stream);
var bw = new BinaryWriter(stream);

Command command = null;

while (true)
{
    Console.WriteLine("Введите команду (GET, POST, PUT, DELETE) или 'exit' для выхода:");
    var input = Console.ReadLine()?.ToUpper();

    if (input == "EXIT")
    {
        command = new Command { HttpMethod = "EXIT" };
        bw.Write(JsonSerializer.Serialize(command));
        Console.WriteLine(br.ReadString());
        break;
    }

    switch (input)
    {
        case Command.GET:
            command = new Command { HttpMethod = Command.GET };
            bw.Write(JsonSerializer.Serialize(command));
            var carListJson = br.ReadString();
            var carList = JsonSerializer.Deserialize<List<Car>>(carListJson);
            Console.WriteLine("Список машин:");
            foreach (var car in carList)
            {
                Console.WriteLine($"ID: {car.Id}, Brand: {car.Brand}, Model: {car.Model}");
            }
            break;

        case Command.POST:
            Console.WriteLine("Введите марку машины:");
            var brand = Console.ReadLine();
            Console.WriteLine("Введите модель машины:");
            var model = Console.ReadLine();

            var newCar = new Car { Brand = brand, Model = model };
            command = new Command { HttpMethod = Command.POST, Value = newCar };
            bw.Write(JsonSerializer.Serialize(command));
            Console.WriteLine(br.ReadString());
            break;

        case Command.PUT:
            Console.WriteLine("Введите ID машины для изменения:");
            if (int.TryParse(Console.ReadLine(), out var id))
            {
                Console.WriteLine("Введите новую марку машины:");
                var newBrand = Console.ReadLine();
                Console.WriteLine("Введите новую модель машины:");
                var newModel = Console.ReadLine();

                var updatedCar = new Car { Id = id, Brand = newBrand, Model = newModel };
                command = new Command { HttpMethod = Command.PUT, Value = updatedCar };
                bw.Write(JsonSerializer.Serialize(command));
                Console.WriteLine(br.ReadString());
            }
            else
            {
                Console.WriteLine("Неверный формат ID.");
            }
            break;

        case Command.DELETE:
            Console.WriteLine("Введите ID машины для удаления:");
            if (int.TryParse(Console.ReadLine(), out var deleteId))
            {
                var carToDelete = new Car { Id = deleteId };
                command = new Command { HttpMethod = Command.DELETE, Value = carToDelete };
                bw.Write(JsonSerializer.Serialize(command));
                Console.WriteLine(br.ReadString());
            }
            else
            {
                Console.WriteLine("Неверный формат ID.");
            }
            break;

        default:
            Console.WriteLine("Неизвестная команда.");
            break;
    }

    Console.WriteLine("Нажмите любую клавишу для продолжения...");
    Console.ReadKey();
    Console.Clear();
}
