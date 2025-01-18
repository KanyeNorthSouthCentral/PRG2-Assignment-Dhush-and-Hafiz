using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//==========================================================
// Student Number : S10270616
// Student Name : Hafiz
// Partner Name : Dhushyanth
//==========================================================

namespace FID
{
    class CFFTFlight : Flight
    {
        public double RequestFee { get; set; }
        public CFFTFlight() : base("DefaultFlightNumber", "DefaultOrigin", "DefaultDestination", DateTime.Now, "Scheduled")
        {
            RequestFee = 0.0;
        }

        public CFFTFlight(string flightNumber, string origin, string destination, DateTime expectedTime, string status, double requestFee) : base(flightNumber, origin, destination, expectedTime, status)
        {
            RequestFee = 150;
        }

        public override double CalculateFees()
        {
            return base.CalculateFees() + RequestFee;
        }

        public override string ToString()
        {
            return $"{base.ToString()}, {CalculateFees()}";
        }
    }
}
