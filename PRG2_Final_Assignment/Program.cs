//==========================================================
// Student Number	: S10268096J
// Partner Number   : S10267014H
// Student Name	: Houshika Barathimogan
// Partner Name	: Sharma Falak 
//==========================================================


using PRG2_Final_Assignment;
using System.Globalization;

//Basic Feature (1) Load Airlines file and add to dictionary

Terminal Terminal5 = new Terminal("Terminal 5");
string[] csvLines = File.ReadAllLines("airlines.csv");
for (int i = 1; i < csvLines.Length; i++)
{
    string[] data = csvLines[i].Split(',');
    string aName = data[0];
    string aCode = data[1];

    Terminal5.AddAirline(new Airline(aName, aCode));

}
Console.WriteLine($"{"Airline Name",-20} {"Airline Code",-12}");
foreach (var airlines in Terminal5.Airlines)
{

    Console.WriteLine($"{airlines.Value.Name,-20} {airlines.Key,-12}");
}
Console.WriteLine();

//Basic Feature (1) Load BoardingGate file and add to dictionary

string[] csvLines1 = File.ReadAllLines("boardinggates.csv");
for (int i = 1; i < csvLines1.Length; i++)
{
    string[] data = csvLines1[i].Split(',');
    string gateName = data[0];
    bool supportsDDJB = bool.Parse(data[1].ToLower());
    bool supportsCFFT = bool.Parse(data[2].ToLower());
    bool supportsLWTT = bool.Parse(data[3].ToLower());

    Terminal5.AddBoardingGate(new BoardingGate(gateName, supportsDDJB, supportsCFFT, supportsLWTT));
}

Console.WriteLine($"{"Boarding Gate",-15} {"DDJB",-12} {"CFFT",-12} {"LWTT",-12}");

foreach (var boardingGate in Terminal5.BoardingGates)
{
    Console.WriteLine($"{boardingGate.Value.GateName,-15} {boardingGate.Value.SupportsDDJB,-12} {boardingGate.Value.SupportsCFFT,-12} {boardingGate.Value.SupportsLWTT,-12}");
}





// Basic Feature (2) Load flights file and add to dictionary


Dictionary<string, Flight> Flights = new Dictionary<string, Flight>();


try
{
    string[] flightLines = File.ReadAllLines("flights.csv");
    for (int i = 1; i < flightLines.Length; i++)
    {
        string[] data = flightLines[i].Split(',');
        string flightNumber = data[0];
        string origin = data[1];
        string destination = data[2];
        DateTime expectedTime = Convert.ToDateTime(data[3]);

        string specialRequest = "";
        if (data.Length > 4)
        {
            specialRequest = data[4].Trim();
        }
        else
        {
            specialRequest = null;
        }

        Flight flight;
        if (specialRequest == "DDJB")
        {
            flight = new DDJBFlight(flightNumber, origin, destination, expectedTime, specialRequest, 300);
        }
        else if (specialRequest == "LWTT")
        {
            flight = new LWTTFlight(flightNumber, origin, destination, expectedTime, specialRequest, 500);
        }
        else if (specialRequest == "CFFT")
        {
            flight = new CFFTFlight(flightNumber, origin, destination, expectedTime, specialRequest, 150);
        }
        else
        {
            flight = new NORMFlight(flightNumber, origin, destination, expectedTime, specialRequest);
        }

        string[] flightParts = flightNumber.Split(' ');


        string airlineCode = flightParts[0].Trim();


        if (Terminal5.Airlines.ContainsKey(airlineCode))
        {

            Airline airline = Terminal5.Airlines[airlineCode];
            if (!airline.Flights.ContainsKey(flightNumber))
            {
                airline.AddFlight(flight);
            }
            else
            {
                Console.WriteLine($"[Warning] Flight number '{flightNumber}' already exists under Airline '{airline.Name}'. Skipping addition.");
            }
        }
        else
        {
            Console.WriteLine($"[Warning] Airline code '{airlineCode}' not found for flight '{flightNumber}'. Skipping this flight.");
        }
    }
}
catch (FileNotFoundException ex)
{
    Console.WriteLine("The file was not found. Please check.");
}
catch (IOException ex)
{
    Console.WriteLine("Error reading the file. Please check.");
}
catch (Exception ex)
{
    Console.WriteLine("There's an unexpected error. Please check.");
}

// Basic Feature (3) Display flight
void displayFlights(Terminal terminal)
{
    Console.WriteLine();
    Console.WriteLine($"{"Flight Number",-15} {"Airline Name",-25} {"Origin",-20} {"Destination",-20} {"Expected Time",-15}");
    Console.WriteLine(new string('-', 110));

    foreach (var airline in terminal.Airlines.Values)
    {
       
        foreach (var flight in airline.Flights.Values)
        {
            string flightNumber = flight.FlightNumber;
            string airlineName = airline.Name;
            string origin = flight.Origin;
            string destination = flight.Destination;
            string time = flight.ExpectedTime.ToString("h:mm tt");

            Console.WriteLine($"{flightNumber,-15} {airlineName,-25} {flight.Origin,-20} {flight.Destination,-20} {time,-15}");
        }
    }
}

displayFlights(Terminal5); 


// Display Menu

//void DisplayMenu()
//{
//    Console.WriteLine("=============================================\r\nWelcome to Changi Airport Terminal 5\r\n=============================================\r\n1. List All Flights\r\n2. List Boarding Gates\r\n3. Assign a Boarding Gate to a Flight\r\n4. Create Flight\r\n5. Display Airline Flights\r\n6. Modify Flight Details\r\n7. Display Flight Schedule\r\n0. Exit");
//}

//while (true)
//{
//    DisplayMenu();
//    Console.Write("Please select your option: ");
//    string option = Console.ReadLine();

//    if (option == "2")
//    {
//        Console.WriteLine("=============================================\r\nList of Boarding Gates for Changi Airport Terminal 5\r\n=============================================");
//        DisplayBoardingGates();
//    }
//    else if (option == "7")
//    {
//        DisplayAirlineFlightDetails(Terminal5.Airlines);
//    }
//}

//// List all boarding gates method - Basic Feature (4)
//void DisplayBoardingGates()
//{
//    Console.WriteLine($"{"Gate Name",-15} {"DDJB",-12} {"CFFT",-12} {"LWTT",-12}");

//    foreach (var boardingGate in Terminal5.BoardingGates)
//    {
//        Console.WriteLine($"{boardingGate.Value.GateName,-15} {boardingGate.Value.SupportsDDJB,-12} {boardingGate.Value.SupportsCFFT,-12} {boardingGate.Value.SupportsLWTT,-12}");
//    }
//}

//// Display full flight details from an airline - Basic Feature (7)
//void DisplayAirlineFlightDetails(Dictionary<string, Airline> Airlines)
//{
//    Console.WriteLine("=============================================\r\nFlight Schedule for Changi Airport Terminal 5\r\n=============================================");

//    Console.WriteLine($"{"Airline Name",-20} {"Airline Code",-12}");
//    foreach (var airlines in Terminal5.Airlines)
//    {

//        Console.WriteLine($"{airlines.Value.Name,-20} {airlines.Key,-12}");
//    }
//    Console.Write("Enter the 2-Letter Airline Code: ");
//    string airlineCode = Console.ReadLine();
//    if (Airlines.ContainsKey(airlineCode))
//    {
//        Airline  selectedAirline = Terminal5.Airlines[airlineCode];
//        Console.WriteLine($"\nFlights for {selectedAirline.Name} ({selectedAirline.Code}):"); // use to string method to display once hardcoded airline is fixed.
//    }


//}
