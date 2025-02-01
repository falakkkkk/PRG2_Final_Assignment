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


        Flight flight = CreateFlightObject(flightNumber, origin, destination, expectedTime, specialRequest);


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



Dictionary<string, Dictionary<string, object>> flightGateDict = new Dictionary<string, Dictionary<string, object>>();

Dictionary<string, string> flightStatusDict = new Dictionary<string, string>();


// Display Menu

void DisplayMenu()
{
    Console.WriteLine("=============================================\r\nWelcome to Changi Airport Terminal 5\r\n=============================================\r\n1. List All Flights\r\n2. List Boarding Gates\r\n3. Assign a Boarding Gate to a Flight\r\n4. Create Flight\r\n5. Display Airline Flights\r\n6. Modify Flight Details\r\n7. Display Flight Schedule\r\n7. Assign Boarding Gate Automatically To Remaining Flights\r\n0. Exit");
}

while (true)
{
    DisplayMenu();
    Console.Write("Please select your option: ");
    string option = Console.ReadLine();

    if (option == "1")
    {
        Console.WriteLine("=============================================\r\nList of Flights for Changi Airport Terminal 5\r\n=============================================\r\n");
        displayFlights(Terminal5);
    }
    else if (option == "2")
    {
        Console.WriteLine("=======================================================\r\nList of Boarding Gates for Changi Airport Terminal 5\r\n=======================================================");
        DisplayBoardingGates();
    }
    else if (option == "3")
    {
        Console.WriteLine("=============================================\r\nAssign a Boarding Gate to a Flight\r\n=============================================\r\n");
        AssignGate(Terminal5);
    }
    else if (option == "4")
    {
        createFlight(Terminal5);
    }
    else if (option =="5")
    {
        DisplayAirlineFlightDetails(Terminal5.Airlines, flightGateDict);
    }
    else if (option == "6")
    {
        ModifyOrDeleteFlight(Terminal5.Airlines, flightGateDict);
    }
    else if (option == "7")
    {
        DisplayScheduledFlights(Terminal5.Airlines, flightGateDict);
    }
    else if (option == "8")
    {
        ProcessUnassignedFlightsBulk(Terminal5);
    }
    else if (option == "0")
    {
        Console.WriteLine("Good Bye.");
        break;
    }
    else
    {
        Console.WriteLine("Please enter a vaild option.");
    }
}


string GetSpecialRequestCode(Flight flight)
{
    if (flight is DDJBFlight) return "DDJB";
    if (flight is CFFTFlight) return "CFFT";
    if (flight is LWTTFlight) return "LWTT";
    return "";
}


Flight CreateFlightObject(string flightNumber, string origin, string destination, DateTime expectedTime, string specialRequest)
{
    if (specialRequest == "DDJB")
    {
        return new DDJBFlight(flightNumber, origin, destination, expectedTime, "Unassigned", 300);
    }
    if (specialRequest == "LWTT")
    {
        return new LWTTFlight(flightNumber, origin, destination, expectedTime, "Unassigned", 500);
    }
    if (specialRequest == "CFFT")
    {
        return new CFFTFlight(flightNumber, origin, destination, expectedTime, "Unassigned", 150);
    }

    return new NORMFlight(flightNumber, origin, destination, expectedTime, "Unassigned");
}



// Basic Feature (3) Display flight
void displayFlights(Terminal terminal)
{
    string csvFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\flights.csv");
    
    if (!File.Exists(csvFilePath))
    {
        Console.WriteLine("Error: The flights CSV file does not exist.");
        return;
    }

    Console.WriteLine();
    Console.WriteLine($"{"Flight Number",-15} {"Airline",-20} {"Origin",-20} {"Destination",-20} {"Expected Time",-20} {"Special Request",-15}");
    Console.WriteLine(new string('-', 115));
    try
    {
        string[] lines = File.ReadAllLines(csvFilePath);
        for (int i = 1; i < lines.Length; i++)
        {
            string line = lines[i];
            string[] parts = line.Split(',');
            if (parts.Length >= 4)
            {
                string flightNumber = parts[0];
                string origin = parts[1];
                string destination = parts[2];
                string expectedTime = parts[3];
                string specialRequest = parts.Length > 4 ? parts[4] : "None";

   
                if (DateTime.TryParse(expectedTime, out DateTime parsedTime))
                {
                    expectedTime = parsedTime.ToString("dd/MM/yyyy HH:mm"); 
                }
                else
                {
                    expectedTime = "Invalid Time";
                }

                string airlineName = "Unknown Airline";
                string[] flightParts = flightNumber.Split(' ');
                if (terminal.Airlines.ContainsKey(flightParts[0]))
                {
                    airlineName = terminal.Airlines[flightParts[0]].Name;
                }

                Console.WriteLine($"{flightNumber,-15} {airlineName,-20} {origin,-20} {destination,-20} {expectedTime,-20} {specialRequest,-15}");
            }
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error: Failed to read from '{csvFilePath}'. {ex.Message}");
    }
}



// List all boarding gates method - Basic  Feature (4)
void DisplayBoardingGates()
{
    Console.WriteLine($"{"Gate Name",-15} {"DDJB",-12} {"CFFT",-12} {"LWTT",-12}");
    Console.WriteLine(new string('-', 55));

    foreach (var boardingGate in Terminal5.BoardingGates)
    {
        Console.WriteLine($"{boardingGate.Value.GateName,-15} {boardingGate.Value.SupportsDDJB,-12} {boardingGate.Value.SupportsCFFT,-12} {boardingGate.Value.SupportsLWTT,-12}");
    }
}



// Basic feature (5) Assign gates to flights.
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

        if (flightGateDict.ContainsKey(selectedFlight.FlightNumber))
        {
            Console.WriteLine($"Error: Flight '{selectedFlight.FlightNumber}' is already assigned to gate '{flightGateDict[selectedFlight.FlightNumber]["GateNumber"]}'. A flight can only be assigned one gate.");
            return;
        }

        break;
    }

    while (true)
    {
        Console.Write("Enter Boarding Gate Name: ");
        string gateNum = Console.ReadLine().Trim().ToUpper();

        if (!Terminal5.BoardingGates.ContainsKey(gateNum))
        {
            Console.WriteLine($"Error: Boarding Gate '{gateNum}' does not exist.");
            continue;
        }

        selectedGate = terminal.BoardingGates[gateNum];

        foreach (var flightEntry in flightGateDict)
        {
            if (flightEntry.Value["GateNumber"].ToString() == gateNum)
            {
                Console.WriteLine($"Error: Boarding Gate '{gateNum}' is already assigned to flight '{flightEntry.Key}'. A gate can only be assigned to one flight at a time.");
                return;
            }
        }

        break;
    }

    selectedGate.Flight = selectedFlight;

    string specialRequestCode = GetSpecialRequestCode(selectedFlight);

    Console.WriteLine($"Boarding Gate Assignment Successful:");
    Console.WriteLine($"Flight Number: {selectedFlight.FlightNumber}");
    Console.WriteLine($"Origin: {selectedFlight.Origin}");
    Console.WriteLine($"Destination: {selectedFlight.Destination}");
    Console.WriteLine($"Expected Time: {selectedFlight.ExpectedTime}");
    Console.WriteLine($"Special Request Code: {specialRequestCode}");
    Console.WriteLine($"Boarding Gate Name: {selectedGate.GateName}");
    Console.WriteLine($"Supports DDJB: {selectedGate.SupportsDDJB}");
    Console.WriteLine($"Supports CFFT: {selectedGate.SupportsCFFT}");
    Console.WriteLine($"Supports LWTT: {selectedGate.SupportsLWTT}");

    Dictionary<string, object> boardingGateDetails = new Dictionary<string, object>
    {
        { "GateNumber", selectedGate.GateName },
        { "SupportsDDJB", selectedGate.SupportsDDJB },
        { "SupportsCFFT", selectedGate.SupportsCFFT },
        { "SupportsLWTT", selectedGate.SupportsLWTT }
    };

    flightGateDict[selectedFlight.FlightNumber] = boardingGateDetails;

    Console.WriteLine("\nBoarding gate assignment has been recorded.\n");

    while (true)
    {
        Console.WriteLine();
        Console.WriteLine("Would you like to update the status of the flight? (Y/N)");
        string input = Console.ReadLine().Trim().ToUpper();

        if (input == "Y")
        {
            while (true)
            {
                Console.WriteLine("1. Delayed");
                Console.WriteLine("2. Boarding");
                Console.WriteLine("3. On Time");
                Console.Write("Please select the new status of the flight: ");
                string UpdateStatus = Console.ReadLine().Trim();

                if (UpdateStatus == "1")
                {
                    selectedFlight.Status = "Delayed";
                    Console.WriteLine($"Flight {selectedFlight.FlightNumber} has been assigned to Boarding Gate {selectedGate.GateName} and marked as Delayed!");

                    flightStatusDict[selectedFlight.FlightNumber] = "Delayed";
                    Console.WriteLine();
                    return;
                }
                else if (UpdateStatus == "2")
                {
                    selectedFlight.Status = "Boarding";
                    Console.WriteLine($"Flight {selectedFlight.FlightNumber} has been assigned to Boarding Gate {selectedGate.GateName} and is Boarding!");

                    flightStatusDict[selectedFlight.FlightNumber] = "Boarding";
                    Console.WriteLine();
                    return;
                }
                else if (UpdateStatus == "3")
                {
                    selectedFlight.Status = "On-Time";
                    Console.WriteLine($"Flight {selectedFlight.FlightNumber} has been assigned to Boarding Gate {selectedGate.GateName} and is On Time!");

                    flightStatusDict[selectedFlight.FlightNumber] = "On-Time";
                    Console.WriteLine();
                    return;
                }
                else
                {
                    Console.WriteLine("Invalid selection. Please enter 1, 2, or 3.");
                }
            }
        }
        else if (input == "N")
        {
            Console.WriteLine("Thank you.");
            Console.WriteLine();
            return;
        }
        else
        {
            Console.WriteLine("Invalid response. Please enter 'Y' or 'N'.");
        }
    }
}



//Basic feature (6) create flights.
void createFlight(Terminal terminal)
{
    bool addAnother = true;
    int flightsAdded = 0;

    string csvFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\flights.csv");

    if (!File.Exists(csvFilePath))
    {
        File.WriteAllText(csvFilePath, "Flight Number,Origin,Destination,Expected Departure/Arrival,Special Request Code\n");
    }


    while (addAnother)
    {
        Console.WriteLine("=============================================");
        Console.WriteLine("Create a New Flight");
        Console.WriteLine("=============================================\n");

        string flightNum;
        Airline inputAirline = null;
        while (true)
        {
            Console.Write("Enter Flight Number: ");
            flightNum = Console.ReadLine().Trim().ToUpper();

            string[] flightParts = flightNum.Split(' ');
            string airlineCode = flightParts[0];

            if (!terminal.Airlines.ContainsKey(airlineCode))
            {
                Console.WriteLine($"Error: Airline code '{airlineCode}' does not exist.\n");
                continue;
            }

            inputAirline = terminal.Airlines[airlineCode];

            if (inputAirline.Flights.ContainsKey(flightNum))
            {
                Console.WriteLine($"Error: Flight number '{flightNum}' already exists under Airline '{inputAirline.Name}'.\n");
                continue;
            }

            break;
        }

        string origin;
        while (true)
        {
            Console.Write("Enter Origin: ");
            origin = Console.ReadLine().Trim();
            if (string.IsNullOrEmpty(origin))
            {
                Console.WriteLine("Error: Origin cannot be empty.\n");
                continue;
            }
            break;
        }

        string destination;
        while (true)
        {
            Console.Write("Enter Destination: ");
            destination = Console.ReadLine().Trim();
            if (string.IsNullOrEmpty(destination))
            {
                Console.WriteLine("Error: Destination cannot be empty.\n");
                continue;
            }
            break;
        }

        DateTime expectedTime;
        while (true)
        {
            Console.Write("Enter Expected Departure/Arrival Time (dd/MM/yyyy HH:mm): ");
            string timeInput = Console.ReadLine().Trim();

            if (!DateTime.TryParse(timeInput, out expectedTime))
            {
                Console.WriteLine("Error: Invalid date/time format. Please enter a valid date and time.\n");
                continue;
            }

            if (expectedTime <= DateTime.Now)
            {
                Console.WriteLine("Error: Expected time cannot be in the past. Please enter a future date and time.\n");
                continue;
            }

            break;
        }


        string specialRequestCode = null;
        while (true)
        {
            Console.Write("Enter Special Request Code (CFFT/DDJB/LWTT/None): ");
            string inputCode = Console.ReadLine().Trim().ToUpper();

            if (inputCode == "NONE")
            {
                specialRequestCode = null;
                break;
            }
            else if (inputCode == "DDJB" || inputCode == "CFFT" || inputCode == "LWTT")
            {
                specialRequestCode = inputCode;
                break;
            }
            else
            {
                Console.WriteLine("Error: Invalid Special Request Code. Please enter DDJB, CFFT, LWTT, or None.\n");
            }
        }


        Flight flight = CreateFlightObject(flightNum, origin, destination, expectedTime, specialRequestCode);


        inputAirline.AddFlight(flight);

        if (!flightStatusDict.ContainsKey(flightNum))
        {
            flightStatusDict.Add(flightNum, "Unassigned");
            Console.WriteLine($"Flight '{flightNum}' status initialized as 'Unassigned'.");
        }
        else
        {
            Console.WriteLine($"[Warning] Flight number '{flightNum}' already exists in flightStatusDict. Skipping addition.");
        }
        try
        {
            string csvLine = $"{flight.FlightNumber},{flight.Origin},{flight.Destination},{flight.ExpectedTime:dd/MM/yyyy HH:mm},{specialRequestCode ?? "None"}";


            using (StreamWriter sw = new StreamWriter(csvFilePath, true))
            {
                sw.WriteLine(csvLine);
            }

            Console.WriteLine($"Flight information successfully written.\n");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: Failed to write to 'flights.csv'. {ex.Message}\n");
            inputAirline.Flights.Remove(flightNum);
            continue;
        }

        flightsAdded++;
        Console.WriteLine("Flight successfully added.\n");

        while (true)
        {
            Console.Write("Would you like to add another Flight? (Y/N): ");
            string response = Console.ReadLine().Trim().ToUpper();

            if (response == "Y")
            {
                Console.WriteLine();
                break;
            }
            else if (response == "N")
            {
                addAnother = false;
                break;
            }
            else
            {
                Console.WriteLine("Error: Please enter a valid response (Y/N).\n");
                continue;
            }
        }
    }

    if (flightsAdded > 0)
    {
        Console.WriteLine($"\nSuccess: {flightsAdded} Flight(s) have been successfully added.\n");
    }
    else
    {
        Console.WriteLine("\nNo Flights were added.\n");
    }
}



// Display full flight details from an airline - Basic Feature (7)
void DisplayAirlineFlightDetails(Dictionary<string, Airline> Airlines, Dictionary<string, Dictionary<string, object>> flightGateDict)
{
    Console.WriteLine("=============================================");
    Console.WriteLine("Flight Schedule for Changi Airport Terminal 5");
    Console.WriteLine("=============================================");


    Console.WriteLine($"\n{"Airline Name",-20} {"Airline Code",-12}");
    foreach (var airline in Airlines)
    {
        Console.WriteLine($"{airline.Value.Name,-20} {airline.Key,-12}");
    }


    string airlineCode;
    while (true)
    {
        Console.Write("\nEnter the 2-Letter Airline Code: ");
        airlineCode = Console.ReadLine().ToUpper();


        if (Airlines.ContainsKey(airlineCode))
        {
            break;
        }
        else
        {
            Console.WriteLine("Invalid airline code. Please try again.");
        }
    }

    Airline selectedAirline = Airlines[airlineCode];
    Console.WriteLine($"\nFlights for {selectedAirline.Name} ({selectedAirline.Code}):");


    if (selectedAirline.Flights.Count > 0)
    {
        foreach (var flight in selectedAirline.Flights.Values)
        {
            Console.WriteLine($"Flight Number: {flight.FlightNumber}, Origin: {flight.Origin}, Destination: {flight.Destination}");
        }

        string flightNumber;
        while (true)
        {
            Console.Write("\nEnter the Flight Number: ");
            flightNumber = Console.ReadLine().ToUpper();


            if (selectedAirline.Flights.ContainsKey(flightNumber))
            {
                break;
            }
            else
            {
                Console.WriteLine("No flight found with that number. Please try again.");
            }
        }

        Flight selectedFlight = selectedAirline.Flights[flightNumber];
        Console.WriteLine($"Flight Details for {selectedFlight.FlightNumber}");
        Console.WriteLine(new string('-', 25));
        Console.WriteLine($"Flight Number  : {selectedFlight.FlightNumber}");
        Console.WriteLine($"Airline Name   : {selectedAirline.Name}");
        Console.WriteLine($"Origin         : {selectedFlight.Origin}");
        Console.WriteLine($"Destination    : {selectedFlight.Destination}");
        Console.WriteLine($"Expected Time  : {selectedFlight.ExpectedTime}");
        Console.WriteLine($"Status          : {selectedFlight.Status}");
        Console.WriteLine($"Special Request Code: {GetSpecialRequestCode(selectedFlight)}");
        string GetSpecialRequestCode(Flight flight)
        {
            if (flight is DDJBFlight)
            {
                return "DDJB";
            }
            else if (flight is LWTTFlight)
            {
                return "LWTT";
            }
            else if (flight is CFFTFlight)
            {
                return "CFFT";
            }
            else
            {
                return "None";
            }
        }

        if (flightGateDict.ContainsKey(selectedFlight.FlightNumber))
        {
            string gateNumber = flightGateDict[selectedFlight.FlightNumber]["GateNumber"].ToString();
            Console.WriteLine($"Boarding Gate  : {gateNumber}");
        }
        else
        {
            Console.WriteLine($"Boarding Gate  : Not Assigned");
        }
    }
    else
    {
        Console.WriteLine("No flights available for this airline.");
    }
}



// Method to modify flight details - Basic Feature (8)
void ModifyOrDeleteFlight(Dictionary<string, Airline> Airlines, Dictionary<string, Dictionary<string, object>> flightGateDict)
    {
        Console.WriteLine("=============================================");
        Console.WriteLine("Modify or Delete Flight");
        Console.WriteLine("=============================================");

        Console.WriteLine($"\n{"Airline Name",-20} {"Airline Code",-12}");
        foreach (var airline in Airlines)
        {
            Console.WriteLine($"{airline.Value.Name,-20} {airline.Key,-12}");
        }


        string airlineCode;
        while (true)
        {
            Console.Write("\nEnter the 2-Letter Airline Code: ");
            airlineCode = Console.ReadLine().ToUpper();

            if (Airlines.ContainsKey(airlineCode))
            {
                break;
            }
            else
            {
                Console.WriteLine("Invalid airline code. Please try again.");
            }
        }

        Airline selectedAirline = Airlines[airlineCode];
        Console.WriteLine($"\nFlights for {selectedAirline.Name} ({selectedAirline.Code}):");

        if (selectedAirline.Flights.Count > 0)
        {

            foreach (var flight in selectedAirline.Flights.Values)
            {
                string gateInfo = flightGateDict.ContainsKey(flight.FlightNumber) ? flightGateDict[flight.FlightNumber]["GateNumber"].ToString() : "Unassigned";
                Console.WriteLine($"Flight Number: {flight.FlightNumber}, Origin: {flight.Origin}, Destination: {flight.Destination}, Boarding Gate: {gateInfo}");
            }


            string choice;
            while (true)
            {
                Console.WriteLine("\nOptions:");
                Console.WriteLine("[1] Modify an existing flight");
                Console.WriteLine("[2] Delete an existing flight");
                Console.Write("Enter your choice: ");
                choice = Console.ReadLine();

                if (choice == "1" || choice == "2")
                {
                    break;
                }
                else
                {
                    Console.WriteLine("Invalid choice. Please try again.");
                }
            }

            if (choice == "1")
            {

                string flightNumber;
                while (true)
                {
                    Console.Write("\nEnter the Flight Number to modify: ");
                    flightNumber = Console.ReadLine().ToUpper();

                    if (selectedAirline.Flights.ContainsKey(flightNumber))
                    {
                        break;
                    }
                    else
                    {
                        Console.WriteLine("No flight found with that number. Please try again.");
                    }
                }

                Flight flightToModify = selectedAirline.Flights[flightNumber];


                string attributeChoice;
                while (true)
                {
                    Console.WriteLine("Select attribute to modify:");
                    Console.WriteLine("[1] Basic Information (Origin, Destination, Expected Time)");
                    Console.WriteLine("[2] Status");
                    Console.WriteLine("[3] Special Request Code");
                    Console.WriteLine("[4] Boarding Gate");
                    Console.Write("Enter your choice: ");
                    attributeChoice = Console.ReadLine();

                    if (attributeChoice == "1" || attributeChoice == "2" || attributeChoice == "3" || attributeChoice == "4")
                    {
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Invalid choice. Please try again.");
                    }
                }

                if (attributeChoice == "1")
                {

                    Console.Write("\nEnter new Origin: ");
                    flightToModify.Origin = Console.ReadLine();

                    Console.Write("Enter new Destination: ");
                    flightToModify.Destination = Console.ReadLine();

                    DateTime newExpectedTime;
                    while (true)
                    {
                        Console.Write("Enter new Expected Time (yyyy-MM-dd HH:mm): ");
                        if (DateTime.TryParse(Console.ReadLine(), out newExpectedTime))
                        {
                            flightToModify.ExpectedTime = newExpectedTime;
                            break;
                        }
                        else
                        {
                            Console.WriteLine("Invalid time format. Please try again.");
                        }
                    }
                }
                else if (attributeChoice == "2")
                {

                    string statusChoice;
                    while (true)
                    {
                        Console.WriteLine("Select Status:");
                        Console.WriteLine("[1] On Time");
                        Console.WriteLine("[2] Delayed");
                        Console.WriteLine("[3] Cancelled");
                        Console.Write("Enter your choice: ");
                        statusChoice = Console.ReadLine();

                        if (statusChoice == "1" || statusChoice == "2" || statusChoice == "3")
                        {
                            break;
                        }
                        else
                        {
                            Console.WriteLine("Invalid choice. Please try again.");
                        }
                    }

                    if (statusChoice == "1")
                    {
                        flightToModify.Status = "On Time";
                    }
                    else if (statusChoice == "2")
                    {
                        flightToModify.Status = "Delayed";
                    }
                    else if (statusChoice == "3")
                    {
                        flightToModify.Status = "Cancelled";
                    }
                }
                else if (attributeChoice == "3")
                {

                    string inputCode;
                    while (true)
                    {
                        Console.Write("\nEnter new Special Request Code (1: DDJB, 2: CFFT, 3: LWTT, 4: None): ");
                        inputCode = Console.ReadLine().Trim();

                        if (inputCode == "1" || inputCode == "2" || inputCode == "3" || inputCode == "4")
                        {
                            break;
                        }
                        else
                        {
                            Console.WriteLine("Invalid Special Request Code. Please try again.");
                        }
                    }

                    Flight newFlight = null;
                    string specialRequestCode = "";

                    if (inputCode == "1")
                    {
                        newFlight = new DDJBFlight(flightToModify.FlightNumber, flightToModify.Origin, flightToModify.Destination, flightToModify.ExpectedTime, flightToModify.Status, 300);
                        specialRequestCode = "DDJB";
                    }
                    else if (inputCode == "2")
                    {
                        newFlight = new CFFTFlight(flightToModify.FlightNumber, flightToModify.Origin, flightToModify.Destination, flightToModify.ExpectedTime, flightToModify.Status, 150);
                        specialRequestCode = "CFFT";
                    }
                    else if (inputCode == "3")
                    {
                        newFlight = new LWTTFlight(flightToModify.FlightNumber, flightToModify.Origin, flightToModify.Destination, flightToModify.ExpectedTime, flightToModify.Status, 500);
                        specialRequestCode = "LWTT";
                    }
                    else if (inputCode == "4")
                    {
                        newFlight = new NORMFlight(flightToModify.FlightNumber, flightToModify.Origin, flightToModify.Destination, flightToModify.ExpectedTime, flightToModify.Status);
                        specialRequestCode = "None";
                    }

                    selectedAirline.Flights[flightToModify.FlightNumber] = newFlight;


                    Console.WriteLine("\nUpdated Flight Details:");
                    Console.WriteLine($"Flight Number  : {newFlight.FlightNumber}");
                    Console.WriteLine($"Airline Name   : {selectedAirline.Name}");
                    Console.WriteLine($"Origin         : {newFlight.Origin}");
                    Console.WriteLine($"Destination    : {newFlight.Destination}");
                    Console.WriteLine($"Expected Time  : {newFlight.ExpectedTime}");
                    Console.WriteLine($"Status         : {newFlight.Status}");
                    Console.WriteLine($"Special Request Code: {specialRequestCode}");
                }

                else if (attributeChoice == "4")
                {

                    if (!flightGateDict.ContainsKey(flightToModify.FlightNumber))
                    {
                        Console.WriteLine("Boarding Gate is not assigned. Please assign a boarding gate first.");
                        return;
                    }


                    Console.Write("\nEnter new Boarding Gate: ");
                    string newGate = Console.ReadLine();


                    flightGateDict[flightToModify.FlightNumber]["GateNumber"] = newGate;

                    Console.WriteLine($"Boarding Gate updated to: {newGate}");


                    Console.WriteLine($"Flight Number  : {flightToModify.FlightNumber}");
                    Console.WriteLine($"Airline Name   : {selectedAirline.Name}");
                    Console.WriteLine($"Origin         : {flightToModify.Origin}");
                    Console.WriteLine($"Destination    : {flightToModify.Destination}");
                    Console.WriteLine($"Expected Time  : {flightToModify.ExpectedTime}");
                    Console.WriteLine($"Status         : {flightToModify.Status}");
                    Console.WriteLine($"Boarding Gate  : {newGate}");
                }
            }
            else if (choice == "2")
            {

                string flightNumberToDelete;
                while (true)
                {
                    Console.Write("\nEnter the Flight Number to delete: ");
                    flightNumberToDelete = Console.ReadLine().ToUpper();

                    if (selectedAirline.Flights.ContainsKey(flightNumberToDelete))
                    {
                        Flight flightToDelete = selectedAirline.Flights[flightNumberToDelete];
                        if (selectedAirline.RemoveFlight(flightToDelete))
                        {
                            Console.WriteLine($"Flight {flightNumberToDelete} has been deleted.");
                            if (flightGateDict.ContainsKey(flightNumberToDelete))
                            {
                                flightGateDict.Remove(flightNumberToDelete);
                                Console.WriteLine($"Boarding Gate information for {flightNumberToDelete} has been removed.");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Error deleting the flight.");
                        }
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Flight not found. Please enter a valid flight number.");
                    }
                }
            }
        }
        else
        {
            Console.WriteLine("\nNo flights available for this airline.");
        }
    }



//Basic feature (9) filter by earlist flight to latest flight

void DisplayScheduledFlights(Dictionary<string, Airline> Airlines, Dictionary<string, Dictionary<string, object>> flightGateDict)
{
    Console.WriteLine("=============================================");
    Console.WriteLine("Scheduled Flights for Today");
    Console.WriteLine("=============================================\n");

    DateTime today = DateTime.Today;
    List<Flight> allFlights = new List<Flight>();

    foreach (var airline in Airlines.Values)
    {
        foreach (var flight in airline.Flights.Values)
        {
            allFlights.Add(flight);
        }
    }

    if (allFlights.Count == 0)
    {
        Console.WriteLine("No scheduled flights for today.\n");
        return;
    }

    allFlights.Sort((x, y) => x.ExpectedTime.CompareTo(y.ExpectedTime));

    Console.WriteLine($"{"Flight Number",-15} {"Airline",-20} {"Origin",-20} {"Destination",-20} {"Expected Time",-25} {"Status",-15} {"Special Request",-20} {"Boarding Gate",-20}");
    Console.WriteLine(new string('-', 155));

    foreach (var flight in allFlights)
    {

        string specialRequestCode = GetSpecialRequestCode(flight);


        string boardingGate = "Not Assigned";
        if (flightGateDict.ContainsKey(flight.FlightNumber) && flightGateDict[flight.FlightNumber].ContainsKey("GateNumber"))
        {
            boardingGate = flightGateDict[flight.FlightNumber]["GateNumber"].ToString();
        }
        string airlineName = "Unknown Airline";
        string[] flightParts = flight.FlightNumber.Split(' ');
        if (flightParts.Length > 0 && Airlines.ContainsKey(flightParts[0]))
        {
            airlineName = Airlines[flightParts[0]].Name;
        }

        Console.WriteLine($"{flight.FlightNumber,-15} {airlineName,-20} {flight.Origin,-20} {flight.Destination,-20} {flight.ExpectedTime.ToString("dd/MM/yyyy HH:mm"),-25} {flight.Status,-15} {specialRequestCode,-20} {boardingGate,-20}");
    }

}



void ProcessUnassignedFlightsBulk(Terminal terminal)
{

    Queue<Flight> unassignedFlights = new Queue<Flight>();


    int totalUnassignedFlights = 0;
    foreach (var airline in terminal.Airlines.Values)
    {
        foreach (var flight in airline.Flights.Values)
        {
            if (!flightGateDict.ContainsKey(flight.FlightNumber))
            {
                unassignedFlights.Enqueue(flight);
                totalUnassignedFlights++;
            }
        }
    }
    Console.WriteLine($"Total number of Flights without Boarding Gate assignment: {totalUnassignedFlights}");

    int totalUnassignedGates = 0;
    foreach (var gate in terminal.BoardingGates.Values)
    {
        if (gate.Flight == null)
        {
            totalUnassignedGates++;
        }
    }
    Console.WriteLine($"Total number of Boarding Gates without a Flight assigned: {totalUnassignedGates}");


    int autoFlightAssignments = 0;
    int autoGateAssignments = 0;


    while (unassignedFlights.Count > 0)
    {
        Flight flight = unassignedFlights.Dequeue();
        string specialRequest = GetSpecialRequestCode(flight);
        BoardingGate assignedGate = null;


        if (!string.IsNullOrEmpty(specialRequest) && specialRequest != "None")
        {
            foreach (var gate in terminal.BoardingGates.Values)
            {
                if (gate.Flight == null) 
                {
                    if ((specialRequest == "DDJB" && gate.SupportsDDJB) ||
                        (specialRequest == "CFFT" && gate.SupportsCFFT) ||
                        (specialRequest == "LWTT" && gate.SupportsLWTT))
                    {
                        assignedGate = gate;
                        break;
                    }
                }
            }
        }
        else
        {

            foreach (var gate in terminal.BoardingGates.Values)
            {
                if (gate.Flight == null && !gate.SupportsDDJB && !gate.SupportsCFFT && !gate.SupportsLWTT)
                {
                    assignedGate = gate;
                    break;
                }
            }
            if (assignedGate == null)
            {
                foreach (var gate in terminal.BoardingGates.Values)
                {
                    if (gate.Flight == null)
                    {
                        assignedGate = gate;
                        break;
                    }
                }
            }
        }

        if (assignedGate != null)
        {
            assignedGate.Flight = flight;
            flightGateDict[flight.FlightNumber] = new Dictionary<string, object>
            {
                { "GateNumber", assignedGate.GateName },
                { "SupportsDDJB", assignedGate.SupportsDDJB },
                { "SupportsCFFT", assignedGate.SupportsCFFT },
                { "SupportsLWTT", assignedGate.SupportsLWTT }
            };
            autoFlightAssignments++;
            autoGateAssignments++;


            string airlineName = "Unknown Airline";
            string[] flightParts = flight.FlightNumber.Split(' ');
            if (flightParts.Length > 0 && terminal.Airlines.ContainsKey(flightParts[0]))
            {
                airlineName = terminal.Airlines[flightParts[0]].Name;
            }

            Console.WriteLine("\nAssigned Flight Details:");
            Console.WriteLine($"Flight Number       : {flight.FlightNumber}");
            Console.WriteLine($"Airline Name        : {airlineName}");
            Console.WriteLine($"Origin              : {flight.Origin}");
            Console.WriteLine($"Destination         : {flight.Destination}");
            Console.WriteLine($"Expected Time       : {flight.ExpectedTime}");
            Console.WriteLine($"Special Request Code: {specialRequest}");
            Console.WriteLine($"Assigned Boarding Gate: {assignedGate.GateName}");
            Console.WriteLine("---------------------------------------------");
        }
        else
        {
            Console.WriteLine($"\nNo available boarding gate found for flight {flight.FlightNumber} with special request '{specialRequest}'.");
        }
    }

    Console.WriteLine("\nBulk Processing Complete.");
    Console.WriteLine($"Total Flights Assigned Automatically: {autoFlightAssignments}");
    Console.WriteLine($"Total Boarding Gates Assigned Automatically: {autoGateAssignments}");

    int totalAssigned = flightGateDict.Count;
    double flightAutoPercentage = totalAssigned > 0 ? ((double)autoFlightAssignments / totalAssigned) * 100 : 0;
    double gateAutoPercentage = totalAssigned > 0 ? ((double)autoGateAssignments / totalAssigned) * 100 : 0;

    Console.WriteLine($"Percentage of Flights Processed Automatically: {flightAutoPercentage:F2}%");
    Console.WriteLine($"Percentage of Boarding Gates Processed Automatically: {gateAutoPercentage:F2}%");
}

