using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FID
{
    class DDJBFlight : Flight
    {
    public double RequestFee { get; set; }
        public DDJBFlight() : base("DefaultFlightNumber", "DefaultOrigin", "DefaultDestination", DateTime.Now, "Scheduled")
        {
            RequestFee = 0.0;
        }
        public DDJBFlight(string flightNumber, string origin, string destination, DateTime expectedTime, string status, double requestFee) : base(flightNumber, origin, destination, expectedTime, status)
        {
            RequestFee = 300;
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
