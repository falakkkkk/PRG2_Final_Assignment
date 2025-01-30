//==========================================================
// Student Number	: S10268096J
// Partner Number   : S10267014H
// Student Name	: Houshika Barathimogan
// Partner Name	: Sharma Falak 
//==========================================================


using PRG2_Final_Assignment;
using System.Data;
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
//Console.WriteLine($"{"Airline Name",-20} {"Airline Code",-12}");
//foreach (var airlines in Terminal5.Airlines)
//{

//    Console.WriteLine($"{airlines.Value.Name,-20} {airlines.Key,-12}");
//}
//Console.WriteLine();

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

//Console.WriteLine($"{"Boarding Gate",-15} {"DDJB",-12} {"CFFT",-12} {"LWTT",-12}");

//foreach (var boardingGate in Terminal5.BoardingGates)
//{
//    Console.WriteLine($"{boardingGate.Value.GateName,-15} {boardingGate.Value.SupportsDDJB,-12} {boardingGate.Value.SupportsCFFT,-12} {boardingGate.Value.SupportsLWTT,-12}");
//}

// Basic Feature (2) Load flights file and add to dictionary


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
            flight = new DDJBFlight(flightNumber, origin, destination, expectedTime, "On Time", 300);
           
        }
        else if (specialRequest == "LWTT")
        {
            flight = new LWTTFlight(flightNumber, origin, destination, expectedTime, "On Time", 500);
            
        }
        else if (specialRequest == "CFFT")
        {
            flight = new CFFTFlight(flightNumber, origin, destination, expectedTime, "On Time", 150);
            
        }
        else
        {
            flight = new NORMFlight(flightNumber, origin, destination, expectedTime, "On Time");
            
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

// Basic feature (5) Assign Flights to boarding gate

void AssignGate(Terminal terminal)
{

    Flight selectedFlight = null;
    BoardingGate selectedGate = null;

    while (true)
    {

        Console.Write("Enter Flight Number: ");
        string flightNum = Console.ReadLine().Trim().ToUpper();

        string[] flightPartsInput = flightNum.Split(' ');
        string inputAirlineCode = flightPartsInput[0].Trim();
        string inputFlightNumber = flightNum;

        if (!Terminal5.Airlines.ContainsKey(inputAirlineCode))
        {
            Console.WriteLine($"Error: Airline code '{inputAirlineCode}' does not exist.");
            continue;
        }

        Airline inputAirline = Terminal5.Airlines[inputAirlineCode];

        if (!inputAirline.Flights.ContainsKey(inputFlightNumber))
        {
            Console.WriteLine($"Error: Flight number '{inputFlightNumber}' does not exist under Airline '{inputAirline.Name}'.");
            continue;
        }
        selectedFlight = inputAirline.Flights[inputFlightNumber];
        break;
    }

    while (true)
    {
        Console.Write("Enter Boarding Gate Name: ");
        string gateNum = Console.ReadLine().Trim().ToUpper();


        // Validate Boarding Gate Name
        if (!Terminal5.BoardingGates.ContainsKey(gateNum))
        {
            Console.WriteLine($"Error: Boarding Gate '{gateNum}' does not exist.");
            continue;
        }
        selectedGate = terminal.BoardingGates[gateNum];
        break;
    }

    selectedGate.Flight = selectedFlight;



    //string selectedAirlineCode = flightNum.Split(' ')[0];
    //Flight selectedFlight = Terminal5.Airlines[selectedAirlineCode].Flights[flightNum];


    //BoardingGate selectedGate = Terminal5.BoardingGates[gateNum];
    //selectedGate.Flight = selectedFlight;

    string specialRequestCode = "None";
    if (selectedFlight is DDJBFlight)
    {
        specialRequestCode = "DDJB";
    }
    else if (selectedFlight is CFFTFlight)
    {
        specialRequestCode = "CFFT";
    }
    else if (selectedFlight is LWTTFlight)
    {
        specialRequestCode = "LWTT";
    }

    Console.WriteLine($"Flight Number: {selectedFlight.FlightNumber}");
    Console.WriteLine($"Origin: {selectedFlight.Origin}");
    Console.WriteLine($"Destination: {selectedFlight.Destination}");
    Console.WriteLine($"Expected Time: {selectedFlight.ExpectedTime}");
    Console.WriteLine($"Special Request Code: {specialRequestCode}");
    Console.WriteLine($"Boarding Gate Name: {selectedGate.GateName}");
    Console.WriteLine($"Supports DDJB: {selectedGate.SupportsDDJB}");
    Console.WriteLine($"Supports CFFT: {selectedGate.SupportsCFFT}");
    Console.WriteLine($"Supports LWTT: {selectedGate.SupportsLWTT}");
    while (true)
    {
        Console.WriteLine("Would you like to update the status of the flight? (Y/N)");
        string input = Console.ReadLine().ToUpper();
        if (input == "Y")
        {
            Console.WriteLine("1. Delayed\r\n2. Boarding\r\n3. On Time");
            Console.WriteLine("Please select the new status of the flight: ");
            string UpdateStatus = Console.ReadLine();
            if (UpdateStatus == "1")
            {
                selectedFlight.Status = "Delayed";
                Console.WriteLine($"Flight {selectedFlight.FlightNumber} has been assigned to Boarding Gate {selectedGate.GateName}!");
                Console.WriteLine();
                DisplayMenu();
            }
            else if (UpdateStatus == "2")
            {
                selectedFlight.Status = "Boarding";
                Console.WriteLine($"Flight {selectedFlight.FlightNumber} has been assigned to Boarding Gate {selectedGate.GateName}!");
                Console.WriteLine();
                DisplayMenu();
            }
            else if (UpdateStatus == "3")
            {
                selectedFlight.Status = "On Time";
                Console.WriteLine($"Flight {selectedFlight.FlightNumber} has been assigned to Boarding Gate {selectedGate.GateName}!");
                Console.WriteLine();
                DisplayMenu();
            }
            else
            {
                Console.WriteLine("Please enter a vaild number.");
                continue;
            }
        }
        else if (input == "N")
        {
            Console.WriteLine("Thank you.");
            Console.WriteLine();
            DisplayMenu();
        }
        else
        {
            Console.WriteLine("Please enter a vail response.");
        }
    }
}





// Display Menu

void DisplayMenu()
{
    Console.WriteLine("=============================================\r\nWelcome to Changi Airport Terminal 5\r\n=============================================\r\n1. List All Flights\r\n2. List Boarding Gates\r\n3. Assign a Boarding Gate to a Flight\r\n4. Create Flight\r\n5. Display Airline Flights\r\n6. Modify Flight Details\r\n7. Display Flight Schedule\r\n0. Exit");
}

while (true)
{
    DisplayMenu();
    Console.Write("Please select your option: ");
    string option = Console.ReadLine();

    if (option == "2")
    {
        Console.WriteLine("=============================================\r\nList of Boarding Gates for Changi Airport Terminal 5\r\n=============================================");
        DisplayBoardingGates();
    }
    else if (option == "3")
    {
        Console.WriteLine("=============================================\r\nAssign a Boarding Gate to a Flight\r\n=============================================\r\n");
        AssignGate(Terminal5);
    }
    else if (option == "7")
    {
        DisplayAirlineFlightDetails(Terminal5.Airlines);
    }
}

// List all boarding gates method - Basic             Feature (4)
void DisplayBoardingGates()
{
    Console.WriteLine($"{"Gate Name",-15} {"DDJB",-12} {"CFFT",-12} {"LWTT",-12}");

    foreach (var boardingGate in Terminal5.BoardingGates)
    {
        Console.WriteLine($"{boardingGate.Value.GateName,-15} {boardingGate.Value.SupportsDDJB,-12} {boardingGate.Value.SupportsCFFT,-12} {boardingGate.Value.SupportsLWTT,-12}");
    }
}

// Display full flight details from an airline - Basic Feature (7)
void DisplayAirlineFlightDetails(Dictionary<string, Airline> Airlines)
{
    Console.WriteLine("=============================================");
    Console.WriteLine("Flight Schedule for Changi Airport Terminal 5");
    Console.WriteLine("=============================================");


    Console.WriteLine($"\n{"Airline Name",-20} {"Airline Code",-12}");
    foreach (var airline in Airlines)
    {
        Console.WriteLine($"{airline.Value.Name,-20} {airline.Key,-12}");
    }

    Console.Write("\nEnter the 2-Letter Airline Code: ");
    string airlineCode = Console.ReadLine().ToUpper();


    if (Airlines.ContainsKey(airlineCode))
    {
        Airline selectedAirline = Airlines[airlineCode];
        Console.WriteLine($"\nFlights for {selectedAirline.Name} ({selectedAirline.Code}):");


        if (selectedAirline.Flights.Count > 0)
        {
            foreach (var flight in selectedAirline.Flights.Values)
            {
                Console.WriteLine($"Flight Number: {flight.FlightNumber}, Origin: {flight.Origin}, Destination: {flight.Destination}");
            }

            Console.Write("\nEnter the Flight Number: ");
            string flightNumber = Console.ReadLine().ToUpper();

            if (selectedAirline.Flights.ContainsKey(flightNumber))
            {
                Flight selectedFlight = selectedAirline.Flights[flightNumber];



                Console.WriteLine($"Flight Details for {selectedFlight.FlightNumber}");

                Console.WriteLine($"Flight Number  : {selectedFlight.FlightNumber}");
                Console.WriteLine($"Airline Name   : {selectedAirline.Name}");
                Console.WriteLine($"Origin         : {selectedFlight.Origin}");
                Console.WriteLine($"Destination    : {selectedFlight.Destination}");
                Console.WriteLine($"Expected Time  : {selectedFlight.ExpectedTime}");
                //Console.WriteLine($"Special Request: {selectedFlight.SpecialRequest ?? "None"}");
                Console.WriteLine($"Boarding Gate  : "); // place holder for now, wait for boarding gate to be assigned.

            }
            else
            {
                Console.WriteLine("No flight found with that number.");
            }
        }
        else
        {
            Console.WriteLine("No flights available for this airline.");
        }
    }
    else
    {
        Console.WriteLine("No airline found with that code.");
    }
}

// Method to modify flight details - Basic Feature (8)
void ModifyFlightDetails(Dictionary<string, Airline> Airlines)
{


    Console.WriteLine("List of Available Airlines:");


    foreach (var airline in Airlines)
    {
        Console.WriteLine($"{airline.Value.Name,-20} {airline.Key,-12}");
    }


    Console.Write("\nEnter the 2-Letter Airline Code: ");
    string airlineCode = Console.ReadLine().ToUpper();

    if (!Airlines.ContainsKey(airlineCode))
    {
        Console.WriteLine("No airline found with that code.");
        return;
    }


    Airline selectedAirline = Airlines[airlineCode];
    Console.WriteLine($"\nFlights for {selectedAirline.Name} ({selectedAirline.Code}):");

    foreach (var flight in selectedAirline.Flights.Values)
    {
        Console.WriteLine($"{flight.FlightNumber,-10} {flight.Origin,-15} {flight.Destination,-15}");
    }


    Console.WriteLine("\nSelect an option:");
    Console.WriteLine("[1] Modify Flight");
    Console.WriteLine("[2] Delete Flight");
    Console.Write("Please select your option: ");
    string option = Console.ReadLine();

    if (option == "1")
    {


        Console.Write("\nEnter the Flight Number you wish to modify: ");
        string flightNumberToModify = Console.ReadLine().ToUpper();

        if (!selectedAirline.Flights.ContainsKey(flightNumberToModify))
        {
            Console.WriteLine("No flight found with that number.");
            return;
        }

        Flight selectedFlight = selectedAirline.Flights[flightNumberToModify];

        Console.WriteLine("\nCurrent Flight Details:");
        Console.WriteLine($"Flight Number  : {selectedFlight.FlightNumber}");
        Console.WriteLine($"Origin         : {selectedFlight.Origin}");
        Console.WriteLine($"Destination    : {selectedFlight.Destination}");
        Console.WriteLine($"Expected Time  : {selectedFlight.ExpectedTime}");
        Console.WriteLine($"Status         : {selectedFlight.Status}");
        //Console.WriteLine($"Special Request: {selectedFlight.SpecialRequest ?? "None"}");
        //Console.WriteLine($"Boarding Gate  : {selectedFlight.BoardingGate?.GateName ?? "None"}");

        Console.WriteLine("\nSelect which flight specification to modify:");
        Console.WriteLine("[1] Basic Information (Origin, Destination, Expected Time)");
        Console.WriteLine("[2] Status");
        Console.WriteLine("[3] Special Request Code");
        Console.WriteLine("[4] Boarding Gate");
        Console.Write("Please select your option: ");
        string modifyOption = Console.ReadLine();

        if (modifyOption == "1")
        {

            Console.Write("Enter new Origin: ");
            selectedFlight.Origin = Console.ReadLine();

            Console.Write("Enter new Destination: ");
            selectedFlight.Destination = Console.ReadLine();

            Console.Write("Enter new Expected Time (yyyy-MM-dd HH:mm): ");
            selectedFlight.ExpectedTime = DateTime.Parse(Console.ReadLine());

            Console.WriteLine("\nFlight updated successfully.");
        }
        else if (modifyOption == "2")
        {

            Console.Write("Enter new Status: ");
            selectedFlight.Status = Console.ReadLine();

            Console.WriteLine("\nFlight updated successfully.");
        }
        else if (modifyOption == "3")
        {

            Console.Write("Enter new Special Request Code: ");
            //selectedFlight.SpecialRequest = Console.ReadLine();

            Console.WriteLine("\nFlight updated successfully.");
        }
        else if (modifyOption == "4")
        {

            Console.Write("Enter new Boarding Gate: ");
            string newGate = Console.ReadLine();

            BoardingGate boardingGate = Terminal5.BoardingGates.Values.FirstOrDefault(gate => gate.GateName == newGate);
            if (boardingGate != null)
            {
                //selectedFlight.BoardingGate = boardingGate;
                Console.WriteLine("\nFlight updated successfully.");
            }
            else
            {
                Console.WriteLine("Boarding Gate not found.");
            }
        }
        else
        {
            Console.WriteLine("Invalid option selected.");
        }
    }
    else if (option == "2")
    {

        Console.Write("\nEnter the Flight Number you wish to delete: ");
        string flightNumberToDelete = Console.ReadLine().ToUpper();

        if (!selectedAirline.Flights.ContainsKey(flightNumberToDelete))
        {
            Console.WriteLine("No flight found with that number.");
            return;
        }

        Flight selectedFlight = selectedAirline.Flights[flightNumberToDelete];

        Console.Write("Are you sure you want to delete this flight? (Y/N): ");
        string confirmDelete = Console.ReadLine().ToUpper();

        if (confirmDelete == "Y")
        {
            selectedAirline.Flights.Remove(flightNumberToDelete);
            Console.WriteLine("\nFlight deleted successfully.");
        }
        else
        {
            Console.WriteLine("Deletion canceled.");
        }
    }
    else
    {
        Console.WriteLine("Invalid option selected.");
    }

    Console.WriteLine("\nUpdated Flight Details:");
    foreach (var flight in selectedAirline.Flights.Values)
    {
        Console.WriteLine($"Flight Number: {flight.FlightNumber}");
        Console.WriteLine($"Origin: {flight.Origin}");
        Console.WriteLine($"Destination: {flight.Destination}");
        Console.WriteLine($"Expected Time: {flight.ExpectedTime}");
        Console.WriteLine($"Status: {flight.Status}");
        // Console.WriteLine($"Special Request: {flight.SpecialRequest ?? "None"}");
        //Console.WriteLine($"Boarding Gate: {flight.BoardingGate?.GateName ?? "None"}");

    }
}


