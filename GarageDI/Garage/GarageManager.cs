using GarageDI.Enums;

namespace GarageDI.Garage;

internal class GarageManager
{
    private readonly IUI ui;
    private readonly IGarageHandler handler;
    private readonly IUtil util;
    private Dictionary<int, Action> menyOptions;
    private string parkMenyOptions;

    public GarageManager(IUI ui, IGarageHandler handler, IUtil util)
    {
        this.ui = ui;
        this.handler = handler;
        this.util = util;
        menyOptions = GetMenyOptions();
        parkMenyOptions = GetParkMenyOptions();

        Vehicle.Check = RegNoExists;
        Vehicle.Callback = Run;
    }

    internal void Run()
    {
        var menuItems = GetMainMenuItems();
        int selectedIndex = 0;

        do
        {
            ui.Clear();
            ShowMeny(menuItems, selectedIndex);
            var key = ui.GetKeyInfo();

            if (key.Key == ConsoleKey.UpArrow)
            {
                if (selectedIndex > 0)
                    selectedIndex--;
            }
            else if (key.Key == ConsoleKey.DownArrow)
            {
                if (selectedIndex < menuItems.Length - 1)
                    selectedIndex++;
            }
            else if (key.Key == ConsoleKey.Enter)
            {
                if (menyOptions.TryGetValue(selectedIndex, out Action? value))
                {
                    value?.Invoke();
                    Pause();
                }
            }

        } while (true);
    }

    private void ShowMeny(string[] menyItems, int selectedIndex)
    {
        for (int i = 0; i < menyItems.Length; i++)
        {
            if (i == selectedIndex)
            {
                Console.BackgroundColor = ConsoleColor.Green;
                Console.ForegroundColor = ConsoleColor.Black;
                ui.Print($"> {menyItems[i]}");
                Console.ResetColor();
            }
            else
            {
                ui.Print($"  {menyItems[i]}");
            }
        }
    }

    private void Seed()
    {
        handler.Park(new Buss() { RegNo = "A", Seats = 1, Color = "RED" });
        handler.Park(new Buss() { RegNo = "AA", Seats = 2, Color = "RED" });
        handler.Park(new Buss() { RegNo = "AAA", Seats = 3, Color = "BLUE" });
        handler.Park(new Buss() { RegNo = "B", Seats = 1, Color = "BLUE" });
        handler.Park(new Buss() { RegNo = "BB", Seats = 2, Color = "RED" });
        handler.Park(new Buss() { RegNo = "BBB", Seats = 3, Color = "BLUE" });
        handler.Park(new Car() { RegNo = "BBBB", Color = "BLUE" });
        handler.Park(new Car() { RegNo = "BBBBB", Color = "RED" });
    }

    private Dictionary<int, Action> GetMenyOptions()
    {
        return new Dictionary<int, Action>
        {
            {MenyConstants.Park, Park },
            {MenyConstants.ListAll, ListParked },
            {MenyConstants.ListByType, ListByType },
            {MenyConstants.Unpark, UnPark},
            {MenyConstants.Search, Search },
            {MenyConstants.Seed, Seed },
            {MenyConstants.Load, Load },
            {MenyConstants.Quit, Quit }
        };
    }

    private void Load()
    {
        var answer = util.AskForString("Change the current garage? Y for yes, N for no");
        if (answer.Equals("Y", StringComparison.CurrentCultureIgnoreCase)) handler.Load();
    }

    private void Search()
    {
        ui.Clear();
        ui.Meny(isFull: false, GetParkMenyOptions(), "Search meny!" +
                                           "\nSkip search criteria with x" +
                                           "\n0, For all vehicles");
        ui.Print("");
        const int all = 0;
        var search = ChooseVehicle(search: true);

        if (search.Equals(all))
        {
            var vehicleProperties = new Vehicle().GetPropertiesWithIncludedAttribute();
            var vehicles = handler.SearchVehicle(new VehicleDTO(vehicleProperties, null!));
            PrintAll(vehicles);
        }
        else
        {
            var vehicleType = (VehicleType)search;
            var vehicleProp = vehicleType.GetInstanceAndPropsForType();
            var vehicles = handler.SearchVehicle(vehicleProp);
            PrintAll(vehicles);
        }
    }

    private void PrintAll(IEnumerable<IVehicle> vehicles)
    {
        if (!vehicles.Any()) ui.Print("No result");
        else
        {
            foreach (var v in vehicles)
            {
                ui.Print(v.GetInfo());
            }
        }
    }

    private void UnPark()
    {
        var regNo = util.AskForString("Enter reg number").ToUpper();
        ui.Print(handler.Leave(regNo) ?
            $"Vehicle: [{regNo}] unparked" :
            $"Can´t find vehicle with registration number {regNo}");
    }

    private void ListParked()
    {
        PrintAll(handler.GetAll());
    }

    private void ListByType()
    {
        ui.Print("List by type");
        handler.GetByType().ForEach(r => ui.Print($"Type: {r.Name} Count: {r.Count}"));
    }

    private void Quit()
    {
        Environment.Exit(0);
    }

    private void Park()
    {
        ui.Clear();
        ui.Meny(handler.IsGarageFull, GetParkMenyOptions(), "Park meny");
        if (handler.IsGarageFull) return;

        VehicleType vehicleType = (VehicleType)ChooseVehicle(search: false);
        VehicleDTO vehicleDTO = vehicleType.GetInstanceAndPropsForType();
        var vehicle = handler.GetVehicle(vehicleDTO);

        ui.Print(handler.Park(vehicle) ?
            $"[{vehicleType}] with registration number:{vehicle.RegNo} parked" :
            $"Something failed");
    }

    private int ChooseVehicle(bool search)
    {
        int input;
        bool cont;
        do
        {
            input = util.AskForKey("");

            cont = search ?
                input <= Enum.GetValues(typeof(VehicleType)).Length && input >= 0 :
                input <= Enum.GetValues(typeof(VehicleType)).Length && input > 0;
        }
        while (cont is false);

        return input;
    }

    private bool RegNoExists(string regNo)
    {
        if (handler.Get(regNo) != null)
        {
            ui.Print($"Reg number:{regNo} is already in the garage!");
            Pause(message: false);
            return false;
        }
        return true;
    }

    private string GetParkMenyOptions()
    {
        if (!string.IsNullOrEmpty(parkMenyOptions))
            return parkMenyOptions;

        else
        {
            var values = Enum.GetValues(typeof(VehicleType));
            StringBuilder builder = new StringBuilder();
            foreach (var value in values)
            {
                builder.AppendLine($"{(int)value}, {value}");
            }
            return parkMenyOptions = builder.ToString();
        }
    }

    public string[] GetMainMenuItems()
    {
        return new string[]
        {
            $"Park",
            $"List Parked",
            $"List By Type",
            $"UnPark",
            $"Search",
            $"Seed Vehicles",
            $"Load",
            $"Quit"
        };
    }

    private void Pause(bool message = true)
    {
        if(message)  Console.WriteLine("\nPress any key to go back to main meny");
        Console.ReadKey(intercept: true);
    }
}
