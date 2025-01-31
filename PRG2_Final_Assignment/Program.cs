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



// Basic Feature (2) Load flights file and add to dictionary // STATUS IS WRONG!!!


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
            flight = new DDJBFlight(flightNumber, origin, destination, expectedTime, "Unassigned", 300);

        }
        else if (specialRequest == "LWTT")
        {
            flight = new LWTTFlight(flightNumber, origin, destination, expectedTime, "Unassigned", 500);
            
        }
        else if (specialRequest == "CFFT")
        {
            flight = new CFFTFlight(flightNumber, origin, destination, expectedTime, "Unassigned", 150);
            
        }
        else
        {
            flight = new NORMFlight(flightNumber, origin, destination, expectedTime, "Unassigned");
            
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



Dictionary<string, Dictionary<string, object>> flightGateDict = new Dictionary<string, Dictionary<string, object>>();

Dictionary<string, string> flightStatusDict = new Dictionary<string, string>();


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
    else if (option == "4")
    {
        createFlight(Terminal5);
    }

}

// Basic Feature (3) Display flight
void displayFlights(Terminal terminal)
{
    string csvFilePath = "flights.csv";
    if (!File.Exists(csvFilePath))
    {
        Console.WriteLine("Error: The flights CSV file does not exist.");
        return;
    }
    Console.WriteLine();
    Console.WriteLine($"{"Flight Number",-15} {"Origin",-20} {"Destination",-20} {"Expected Time",-15} {"Special Request",-15}");
    Console.WriteLine(new string('-', 85));
    try
    {
        string[] lines = File.ReadAllLines(csvFilePath);
        foreach (string line in lines)
        {
            string[] parts = line.Split(',');
            if (parts.Length >= 4) // Ensure the line has at least FlightNumber, Origin, Destination, ExpectedTime
            {
                string flightNumber = parts[0];
                string origin = parts[1];
                string destination = parts[2];
                string expectedTime = parts[3];
                string specialRequest = parts.Length > 4 ? parts[4] : "None";

                Console.WriteLine($"{flightNumber,-15} {origin,-20} {destination,-20} {expectedTime,-15} {specialRequest,-15}");
            }
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error: Failed to read from '{csvFilePath}'. {ex.Message}");
    }


    //Console.WriteLine();
    //Console.WriteLine($"{"Flight Number",-15} {"Airline Name",-25} {"Origin",-20} {"Destination",-20} {"Expected Time",-15}");
    //Console.WriteLine(new string('-', 110));

    //foreach (var airline in terminal.Airlines.Values)
    //{

    //    foreach (var flight in airline.Flights.Values)
    //    {
    //        string flightNumber = flight.FlightNumber;
    //        string airlineName = airline.Name;
    //        string origin = flight.Origin;
    //        string destination = flight.Destination;
    //        string time = flight.ExpectedTime.ToString("h:mm tt");

    //        Console.WriteLine($"{flightNumber,-15} {airlineName,-25} {flight.Origin,-20} {flight.Destination,-20} {time,-15}");
    //    }
    //}
}


// List all boarding gates method - Basic  Feature (4)
void DisplayBoardingGates()
{
    Console.WriteLine($"{"Gate Name",-15} {"DDJB",-12} {"CFFT",-12} {"LWTT",-12}");

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

    Dictionary<string, object> boardingGateDetails = new Dictionary<string, object>
    {
        { "GateNumber", selectedGate.GateName },
        { "SupportsDDJB", selectedGate.SupportsDDJB },
        { "SupportsCFFT", selectedGate.SupportsCFFT },
        { "SupportsLWTT", selectedGate.SupportsLWTT }
    };

    // Add or Update the flight's boarding gate details in the nested dictionary
    if (flightGateDict.ContainsKey(selectedFlight.FlightNumber))
    {
        flightGateDict[selectedFlight.FlightNumber] = boardingGateDetails;
    }
    else
    {
        flightGateDict.Add(selectedFlight.FlightNumber, boardingGateDetails);
    }

    // **Optional:** Display confirmation
    Console.WriteLine("\nBoarding gate assignment has been recorded.\n");

    Flight flight;
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
                Console.WriteLine($"Flight {selectedFlight.FlightNumber} has been assigned to Boarding Gate {selectedGate.GateName} and marked as Delayed!");

                // Update flightStatusDict
                if (flightStatusDict.ContainsKey(selectedFlight.FlightNumber))
                {
                    flightStatusDict[selectedFlight.FlightNumber] = "Delayed";
                }
                else
                {
                    flightStatusDict.Add(selectedFlight.FlightNumber, "Delayed");
                }

                Console.WriteLine();
                DisplayMenu();
                break;
            }
            else if (UpdateStatus == "2")
            {
                selectedFlight.Status = "Boarding";
                Console.WriteLine($"Flight {selectedFlight.FlightNumber} has been assigned to Boarding Gate {selectedGate.GateName} and is Boarding!");

                // Update flightStatusDict
                if (flightStatusDict.ContainsKey(selectedFlight.FlightNumber))
                {
                    flightStatusDict[selectedFlight.FlightNumber] = "Boarding";
                }
                else
                {
                    flightStatusDict.Add(selectedFlight.FlightNumber, "Boarding");
                }

                Console.WriteLine();
                DisplayMenu();
                break;
            }
            else if (UpdateStatus == "3")
            {
                selectedFlight.Status = "Unassigned";
                Console.WriteLine($"Flight {selectedFlight.FlightNumber} has been assigned to Boarding Gate {selectedGate.GateName} and is On Time!");

                
                if (flightStatusDict.ContainsKey(selectedFlight.FlightNumber))
                {
                    flightStatusDict[selectedFlight.FlightNumber] = "On-Time";
                }
                else
                {
                    flightStatusDict.Add(selectedFlight.FlightNumber, "On-Time");
                }

                Console.WriteLine();
                DisplayMenu();
                break;
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
            break;
        }
        else
        {
            Console.WriteLine("Please enter a vail response.");
        }
    }
}



// Basic feature (6) create flights
void createFlight(Terminal terminal)
{

    bool addAnother = true;
    int flightsAdded = 0;
    string csvFilePath = "flights.csv";

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

        // 4. Prompt for Expected Departure/Arrival Time
        Console.Write("Enter Expected Departure/Arrival Time (dd/MM/yyyy hh:mm): ");
        DateTime expectedTime = Convert.ToDateTime(Console.ReadLine());


        // 5. Prompt for Additional Information (Special Request Code)
        string specialRequestCode = null;
        while (true)
        {
            Console.Write("Enter Special Request Code (CFFT/DDJB/LWTT/None): ");
            string inputCode = Console.ReadLine().Trim().ToUpper();

            if (inputCode == "NONE")
            {
                specialRequestCode = null; // No special request
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



    // 6. Create the Appropriate Flight Object
        Flight flight;
        if (specialRequestCode == "DDJB")
        {
            flight = new DDJBFlight(flightNum, origin, destination, expectedTime, "Unassigned", 300);

        }
        else if (specialRequestCode == "LWTT")
        {
            flight = new LWTTFlight(flightNum, origin, destination, expectedTime, "Unassigned", 500);

        }
        else if (specialRequestCode == "CFFT")
        {
            flight = new CFFTFlight(flightNum, origin, destination, expectedTime, "Unassigned", 150);

        }
        else
        {
            flight = new NORMFlight(flightNum, origin, destination, expectedTime, "Unassigned");

        }

        // 7. Add the Flight Object to the Airline's Dictionary
        inputAirline.AddFlight(flight);

        if (!flightStatusDict.ContainsKey(flightNum))
        {
            flightStatusDict.Add(flightNum, "Unassigned");
            Console.WriteLine($"[Info] Flight '{flightNum}' status initialized as 'Unassigned'.");
        }
        else
        {
            Console.WriteLine($"[Warning] Flight number '{flightNum}' already exists in flightStatusDict. Skipping addition.");
        }

        // 8. Append the New Flight Information to flights.csv
        try
        {
            // Prepare CSV line
            // Format: FlightNumber,Origin,Destination,ExpectedTime,SpecialRequest
            //string csvLine = $"{flight.FlightNumber},{flight.Origin},{flight.Destination},{flight.ExpectedTime.ToString("yyyy-MM-dd HH:mm")}";
            //if (specialRequestCode != null)
            //{
            //    csvLine += $",{specialRequestCode}";
            //}
            string csvLine = $"{flight.FlightNumber},{flight.Origin},{flight.Destination},{flight.ExpectedTime.ToString("yyyy-MM-dd HH:mm")}";
            if (specialRequestCode != null)
            {
                csvLine += $",{specialRequestCode}";
            }
            // Append the line to the CSV file using File.AppendAllText
            //File.AppendAllText("flights.csv", csvLine + Environment.NewLine);

            //    Console.WriteLine("[Info] Flight information successfully written to 'flights.csv'.\n");
            //}

            File.AppendAllText(csvFilePath, csvLine + Environment.NewLine);

            Console.WriteLine($"[Info] Flight information successfully written to '{csvFilePath}'.\n");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: Failed to write to 'flights.csv'. {ex.Message}\n");
            // Optionally, remove the flight from the dictionary if CSV write fails
            inputAirline.Flights.Remove(flightNum);
            continue;
        }

        flightsAdded++;
        Console.WriteLine("Flight successfully added.\n");

        // 9. Prompt to Add Another Flight
        while (true)
        {
            Console.Write("Would you like to add another Flight? (Y/N): ");
            string response = Console.ReadLine().Trim().ToUpper();

            if (response == "Y")
            {
                Console.WriteLine(); // Add a blank line for readability
                break; // Continue the outer loop to add another flight
            }
            else if (response == "N")
            {
                addAnother = false;
                break; // Exit the outer loop
            }
            else
            {
                Console.WriteLine("Error: Please enter a valid response (Y/N).\n");
                continue; // Prompt again
            }
        }
    }

    // 10. Display Success Message
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

void ModifyOrDeleteFlight(Dictionary<string, Airline> Airlines, Dictionary<string, Dictionary<string, object>> flightGateDict)
{
    Console.WriteLine("=============================================");
    Console.WriteLine("Modify or Delete Flight");
    Console.WriteLine("=============================================");

    // Display all airlines
    Console.WriteLine($"\n{"Airline Name",-20} {"Airline Code",-12}");
    foreach (var airline in Airlines)
    {
        Console.WriteLine($"{airline.Value.Name,-20} {airline.Key,-12}");
    }

    // Prompt for airline selection
    Console.Write("\nEnter the 2-Letter Airline Code: ");
    string airlineCode = Console.ReadLine().ToUpper();

    if (Airlines.ContainsKey(airlineCode))
    {
        Airline selectedAirline = Airlines[airlineCode];
        Console.WriteLine($"\nFlights for {selectedAirline.Name} ({selectedAirline.Code}):");

        if (selectedAirline.Flights.Count > 0)
        {
            // Display flight details
            foreach (var flight in selectedAirline.Flights.Values)
            {
                Console.WriteLine($"Flight Number: {flight.FlightNumber}, Origin: {flight.Origin}, Destination: {flight.Destination}");
            }

            Console.WriteLine("\nOptions:");
            Console.WriteLine("[1] Modify an existing flight");
            Console.WriteLine("[2] Delete an existing flight");
            Console.Write("Enter your choice: ");
            string choice = Console.ReadLine();

            if (choice == "1")
            {
                // Modify flight details
                Console.Write("\nEnter the Flight Number to modify: ");
                string flightNumber = Console.ReadLine().ToUpper();

                if (selectedAirline.Flights.ContainsKey(flightNumber))
                {
                    Flight flightToModify = selectedAirline.Flights[flightNumber];

                    Console.WriteLine("Select attribute to modify:");
                    Console.WriteLine("[1] Origin");
                    Console.WriteLine("[2] Destination");
                    Console.WriteLine("[3] Expected Time");
                    Console.WriteLine("[4] Boarding Gate");
                    Console.Write("Enter your choice: ");
                    string attributeChoice = Console.ReadLine();

                    switch (attributeChoice)
                    {
                        case "1":
                            Console.Write("Enter new Origin: ");
                            flightToModify.Origin = Console.ReadLine();
                            break;
                        case "2":
                            Console.Write("Enter new Destination: ");
                            flightToModify.Destination = Console.ReadLine();
                            break;
                        case "3":
                            Console.Write("Enter new Expected Time (yyyy-MM-dd HH:mm): ");
                            if (DateTime.TryParse(Console.ReadLine(), out DateTime newTime))
                            {
                                flightToModify.ExpectedTime = newTime;
                            }
                            else
                            {
                                Console.WriteLine("Invalid time format.");
                            }
                            break;
                        case "4":
                            Console.Write("Enter new Boarding Gate: ");
                            if (flightGateDict.ContainsKey(flightNumber))
                            {
                                flightGateDict[flightNumber]["GateNumber"] = Console.ReadLine();
                            }
                            else
                            {
                                flightGateDict[flightNumber] = new Dictionary<string, object>
                                {
                                    { "GateNumber", Console.ReadLine() }
                                };
                            }
                            break;
                        default:
                            Console.WriteLine("Invalid choice.");
                            break;
                    }

                    Console.WriteLine("Flight details updated successfully.");
                }
                else
                {
                    Console.WriteLine("No flight found with that number.");
                }
            }
            else if (choice == "2")
            {
                // Delete flight
                Console.Write("\nEnter the Flight Number to delete: ");
                string flightNumber = Console.ReadLine().ToUpper();

                if (selectedAirline.Flights.ContainsKey(flightNumber))
                {
                    Console.Write("Are you sure you want to delete this flight? [Y/N]: ");
                    string confirmation = Console.ReadLine().ToUpper();

                    if (confirmation == "Y")
                    {
                        selectedAirline.Flights.Remove(flightNumber);
                        if (flightGateDict.ContainsKey(flightNumber))
                        {
                            flightGateDict.Remove(flightNumber);
                        }
                        Console.WriteLine("Flight deleted successfully.");
                    }
                    else
                    {
                        Console.WriteLine("Flight deletion canceled.");
                    }
                }
                else
                {
                    Console.WriteLine("No flight found with that number.");
                }
            }
            else
            {
                Console.WriteLine("Invalid choice.");
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




