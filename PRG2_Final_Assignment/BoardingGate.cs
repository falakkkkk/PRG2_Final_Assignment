using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRG2_Final_Assignment
{
    public class BoardingGate
    {
        public string GateName { get; set; }
        public bool SupportsDDJB { get; set; }
        public bool SupportsCFFT { get; set; }
        public bool SupportsLWTT { get; set; }
        public Flight Flight { get; set; }

        public BoardingGate() { }

        public BoardingGate (string gateName, bool supportsDDJB, bool supportsCFFT, bool supportsLWTT)
        {
            GateName = gateName;
            SupportsDDJB = supportsDDJB;
            SupportsCFFT = supportsCFFT;
            SupportsLWTT = supportsLWTT;
            Flight = null; 
        }

        public double CalculateFees()
        {
            if (Flight == null)
            {
                return 0;
            }
            return Flight.CalculateFees(Flight.Origin,Flight.Destination);

        }
        public override string ToString()
        {
            if (Flight != null)
            {
                return $"Gate Name: {GateName}, Supports DDJB: {SupportsDDJB}, Supports CFFT: {SupportsCFFT}, Supports LWTT: {SupportsLWTT}, Flight: {Flight.ToString()}";
            }
            return $"Gate Name: {GateName}, Supports DDJB: {SupportsDDJB}, Supports CFFT: {SupportsCFFT}, Supports LWTT: {SupportsLWTT}, Flight: No flight assigned";
        }


    }
}
