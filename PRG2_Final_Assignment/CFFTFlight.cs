using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRG2_Final_Assignment
{
    public class CFFTFlight : Flight
    {
        public double RequestFee { get; set; }

        public CFFTFlight() : base() { }
        public CFFTFlight(string flightNumber, string origin, string destination, DateTime expectedTime, string status,double requestFee) : base(flightNumber, origin, destination, expectedTime, status)
        {
            RequestFee = requestFee;
        }

        public override double CalculateFees(string origin, string destination)
        {
            double fees = 0;

            if (destination == "Singapore (SIN)")
            {
                fees = 500 + 150;  
            }
            else if (origin == "Singapore (SIN)")
            {
                fees = 800 + 150;  
            }

            return fees + 300;
        }


        public override string ToString()
        {
            return base.ToString() + "\tRequest Fee: " + RequestFee;
        }
    }
}
