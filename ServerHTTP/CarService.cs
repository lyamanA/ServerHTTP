using System.Text.Json;

namespace ServerHTTP
{
    public  class CarService
    {
        private readonly string _filePath = "cars.json"; 
        private List<Car> _cars;
        private int _nextId;

        public CarService()
        {
            _cars = LoadCarsFromFile();
            _nextId = _cars.Any() ? _cars.Max(c => c.Id) + 1 : 1; 
        }

        public string GetCars()
        {
            var cars = LoadCarsFromFile();
            return JsonSerializer.Serialize(cars);
        }

        public string AddCar(Car car)
        {
            if (car != null)
            {
                car.Id = _nextId++;
                _cars.Add(car);
                SaveCarsToFile(); 
                return $"Car with ID {car.Id} added";
            }
            return "No car data provided";
        }

        public string UpdateCar(Car car)
        {
            var existingCar = _cars.FirstOrDefault(c => c.Id == car.Id);
            if (existingCar != null)
            {
                existingCar.Brand = car.Brand;
                existingCar.Model = car.Model;
                SaveCarsToFile(); 
                return "Car updated";
            }
            return "Car not found";
        }

        public string DeleteCar(int id)
        {
            var carToRemove = _cars.FirstOrDefault(c => c.Id == id);
            if (carToRemove != null)
            {
                _cars.Remove(carToRemove);
                SaveCarsToFile(); 
                return "Car deleted";
            }
            return "Car not found";
        }

        private void SaveCarsToFile()
        {
            var json = JsonSerializer.Serialize(_cars, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(_filePath, json);
        }

        private List<Car> LoadCarsFromFile()
        {
            if (File.Exists(_filePath))
            {
                var json = File.ReadAllText(_filePath);
                return JsonSerializer.Deserialize<List<Car>>(json) ?? new List<Car>();
            }
            return new List<Car>();
        }
    }
}

