using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRG2_Final_Assignment
{
    public class Terminal
    {
        public string TerminalName { get; set; }

        public Dictionary<string, Airline> Airlines { get; set; } = new Dictionary<string, Airline>();
        public Dictionary<string, Flight> Flights { get; set; } = new Dictionary<string, Flight>();
        public Dictionary<string, BoardingGate> BoardingGates { get; set; } = new Dictionary<string, BoardingGate>();
        public Dictionary<string, double> GateFees { get; set; } = new Dictionary<string, double>();

        public Terminal() { }
        public Terminal(string terminalName)
        {
            TerminalName = terminalName;
        }
        
        public bool AddAirline(Airline airline)
        {
            if (Airlines.ContainsKey(airline.Code))
            {
                return false;
            }
            else
            {
                Airlines.Add(airline.Code, airline);
                return true;
            }
        }
        public bool AddBoardingGate(BoardingGate boardingGate)
        {
            if (BoardingGates.ContainsKey(boardingGate.GateName))
            {
                return false;
            }
            else
            {
                BoardingGates.Add(boardingGate.GateName, boardingGate);
                return true;
            }
        }
        public Airline GetAirline(Airline airline)
        {
            foreach(var airline  in Airlines.Values)
            {
                return airline;
            }
            return null;
        }
        public void PrintAirLineFees(Airline airline)
        {
            Console.WriteLine("Airline Fees for Terminal" + TerminalName);
            foreach (var a in Airlines.Values)
            {
                Console.WriteLine($"{a.Name} ({a.Code})    Fees: ${a.CalculateFees()}");
            }
        }
    }
}
