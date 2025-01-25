using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRG2_Final_Assignment
{
    public class LWTTFlight : Flight
    {
        public double RequestFee { get; set; }

        public LWTTFlight() : base() { }
        public LWTTFlight(string flightNumber, string origin, string destination, DateTime expectedTime, string status, double requestFee) : base(flightNumber, origin, destination, expectedTime, status)
        {
            RequestFee = requestFee;
        }

        public override double CalculateFees(string origin, string destination)
        {
            double fees = 0;
            double boarding_baseFee = 300;

            if (destination == "Singapore (SIN)")
            {
                fees = 500 + 500;
            }
            else if (origin == "Singapore (SIN)")
            {
                fees = 800 + 500;
            }

            return fees + boarding_baseFee;
        }


        public override string ToString()
        {
            return base.ToString() + "\tRequest Fee: " + RequestFee;
        }
    }
}
