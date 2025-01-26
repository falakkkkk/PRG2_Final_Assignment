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
    public abstract class Flight
    {
        public string FlightNumber { get; set; }
        public string Origin { get; set; }
        public string Destination { get; set; }
        public DateTime ExpectedTime { get; set; }
        public string Status { get; set; }

        public Flight() { }
        public Flight (string flightNumber,  string origin, string destination, DateTime expectedTime, string status)
        {
            FlightNumber = flightNumber;
            Origin = origin;
            Destination = destination;
            ExpectedTime = expectedTime;
            Status = status;

        }

        public virtual double CalculateFees(string origin, string destination)
        {
            double fees = 0;
            double boarding_baseFee = 300;

            if (destination == "Singapore (SIN)")
            {
                fees = 500;
            }
            else if (origin == "Singapore (SIN)")
            {
                fees = 800;
            }

            return fees + boarding_baseFee;
        }

        public override string ToString()
        {
            return "Flight Number: " + FlightNumber + "\tOrigin: " + Origin + "\tDestination: " + Destination + "\tExpected Time: " + ExpectedTime + "\tStatus " + Status;
        } 

    }
}
