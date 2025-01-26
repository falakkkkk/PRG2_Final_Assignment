using PRG2_Final_Assignment;
using static System.Runtime.InteropServices.JavaScript.JSType;

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
Console.WriteLine($"{"Airline Name",-15} {"Airline Code",-12}");
foreach (var airline in Terminal5.Airlines)
{

    Console.WriteLine($"{airline.Key,-15} {airline.Value.Name,-12}");
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
    
    Terminal5.AddBoardingGate(new BoardingGate(gateName, supportsDDJB,supportsCFFT,supportsLWTT));
}

Console.WriteLine($"{"Boarding Gate",-15} {"DDJB",-12} {"CFFT",-12} {"LWTT",-12}");

foreach (var boardingGate in Terminal5.BoardingGates)
{
    Console.WriteLine($"{boardingGate.Value.GateName,-15} {boardingGate.Value.SupportsDDJB,-12} {boardingGate.Value.SupportsCFFT,-12} {boardingGate.Value.SupportsLWTT,-12}");
}


