using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// Muhammad Hafiz Bin Mohamed Noor S10270616F
namespace FID
{
    class LWTTFlight : Flight
    {
		public double RequestFee { get; set; }

        public LWTTFlight(string flightNumber, string origin, string destination, DateTime expectedTime, string status, double requestFee) : base(flightNumber, origin, destination, expectedTime, status)
        {
            RequestFee = 500;
        }

        public override double CalculateFees()
        { 
            return base.CalculateFees() + RequestFee;
        }



    }
}
