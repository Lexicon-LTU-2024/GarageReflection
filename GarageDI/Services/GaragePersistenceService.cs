using Newtonsoft.Json;

namespace GarageDI.Services
{

    public class GaragePersistenceService : IGaragePersistenceService
    {

        private readonly JsonSerializerSettings _jsonSettings;

        public GaragePersistenceService()
        {
            _jsonSettings = new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.All,
                Formatting = Formatting.Indented
            };
        }

        public void SaveGarage(string garageName, int capacity, IEnumerable<IVehicle> vehicles)
        {
            string filePath = GetFilePath(garageName);

            var garageDto = new GarageDTO
            {
                Capacity = capacity,
                Name = garageName,
                Vehicles = vehicles
            };

            try
            {
                using (StreamWriter streamWriter = File.CreateText(filePath))
                {
                    var serializer = JsonSerializer.Create(_jsonSettings);
                    serializer.Serialize(streamWriter, garageDto);

                }
                //För att visa hur man kan använda debug writeline under utveckling kompileras inte in in release mode!
                Debug.WriteLine($"JSON data successfully written to {garageName}.json");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error writing JSON to {garageName}.json: {ex.Message}");
            }

        }

        public GarageDTO LoadGarage(string garageName)
        {
            string filePath = GetFilePath(garageName);

            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException($"The file for the garage '{garageName}' does not exist.");
            }

            using (StreamReader streamReader = File.OpenText(filePath))
            {
                var serializer = JsonSerializer.Create(_jsonSettings);
                var garageDto = (GarageDTO?)serializer.Deserialize(streamReader, typeof(GarageDTO));
                ArgumentNullException.ThrowIfNull(garageDto, nameof(garageDto));
                return garageDto!;
            }
        }

        private string GetFilePath(string garageName)
        {
            return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"{garageName}.json");
        }

    }
}

