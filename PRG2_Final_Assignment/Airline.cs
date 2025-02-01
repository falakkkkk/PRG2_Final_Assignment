using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//==========================================================
// Student Number	: S10268096J
// Partner Number   : S10267014H
// Student Name	: Houshika Barathimogan
// Partner Name	: Sharma Falak 
//==========================================================


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
            double discount = 0;
            int flightCount = Flights.Count;

            foreach (var flight in Flights.Values)
            {
                totalFee += flight.CalculateFees(flight.Origin, flight.Destination);

                if (flight.ExpectedTime.Hour < 11 || flight.ExpectedTime.Hour >= 21)
                {
                    discount += 110;
                }


                if (flight.Origin == "Dubai (DXB)" || flight.Origin == "Bangkok (BKK)" || flight.Origin == "Tokyo (NRT)")
                {
                    discount += 25;
                }

                if (flight is NORMFlight )
                {
                    discount += 50;
                }
                    
            }

            
            if (flightCount >= 3 )
            {
                discount += (flightCount / 3) * 350;  

            }

            if (flightCount > 5)
            {
                discount += totalFee * 0.03;
            }


            totalFee -= discount;
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
            return "Name: " + Name + "\tCode: " + Code;
        }

    }
}






