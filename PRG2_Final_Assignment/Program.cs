//==========================================================
// Student Number	: S10268096J
// Partner Number   : S10267014H
// Student Name	: Houshika Barathimogan
// Partner Name	: Sharma Falak 
//==========================================================


using PRG2_Final_Assignment;

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

Airline airline = new Airline("Singapore Airlines", "SQ");

try
{
    string[] lines = File.ReadAllLines("flights.csv");
    for (int i = 1;i < lines.Length;i++)
    {
        string[] data = lines[i].Split(',');
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

        if (specialRequest == "DDJB")
        {
            airline.AddFlight(new DDJBFlight(flightNumber, origin, destination, expectedTime, specialRequest));
        }
        else if (specialRequest == "LWTT")
        {
            airline.AddFlight(new LWTTFlight(flightNumber, origin, destination, expectedTime, specialRequest));
        }
        else if(specialRequest == "CFFT")
        {
            airline.AddFlight(new CFFTFlight(flightNumber, origin, destination, expectedTime, specialRequest));
        }
        else
        {
            airline.AddFlight(new NORMFlight(flightNumber, origin, destination, expectedTime, specialRequest));
        }
       
    }
}
catch(FileNotFoundException ex)
{
    Console.WriteLine("The file was not found. Please check.");
}
catch(IOException ex)
{
    Console.WriteLine("Error reading the file. Please check.");
}
catch (Exception ex)
{
    Console.WriteLine("There's an unexpected error. Please check.");
}

foreach (var flight in airline.Flights.Values)
{
    Console.WriteLine(flight.ToString());
}
  