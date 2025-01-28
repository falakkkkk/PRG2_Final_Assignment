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
    public class CFFTFlight : Flight
    {
        public double RequestFee { get; set; }

        public CFFTFlight() : base() { }
        public CFFTFlight(string flightNumber, string origin, string destination, DateTime expectedTime, string status) : base(flightNumber, origin, destination, expectedTime, status)
        {
            
        }

        public override double CalculateFees(string origin, string destination)
        {
            double fees = 0;
            double boarding_baseFee = 300;
            double requestFee = 150;
       

            if (destination == "Singapore (SIN)")
            {
                fees = 500 + requestFee;  
            }
            else if (origin == "Singapore (SIN)")
            {
                fees = 800 + requestFee;  
            }

            return fees + boarding_baseFee;
        }


        public override string ToString()
        {
            return base.ToString() + "\tRequest Fee: " +150;
        }
    }
}
