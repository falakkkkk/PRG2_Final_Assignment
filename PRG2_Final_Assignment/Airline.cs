using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRG2_Final_Assignment
{
    public class Airline
    {
        public string Name { get; set; }
        public string Code { get; set; }

        public Dictionary<string, Flight> Flights { get; set; } = new Dictionary<string, Flight>();

        public Airline() { }

        public Airline(string name, string code)
        {
            Name = name;
            Code = code;
        }
        public bool AddFlight(Flight flight)
        {
            if (Flights.ContainsKey(flight.FlightNumber))
            {
                return false;
            }
            else
            {
                Flights.Add(flight.FlightNumber, flight);
                return true;
            }

        }

      public double CalculateFees()
      {
            double totalFee = 0;
            foreach(var flight in Flights.Values)
            {
                totalFee += flight.CalculateFees(flight.Origin, flight.Destination);
            }
            double discount = 0;
            foreach(var flight in Flights.Values)

            return totalFee;
      }

        public bool RemoveFlight(Flight flight)
        {
            if (Flights.ContainsKey(flight.FlightNumber))
            {
                Flights.Remove(flight.FlightNumber);
                return true;
            }   
            return false;
        }

        public override string ToString()
        {
            return "Name" + Name + "\tCode: " + Code;
        }

    }
}






