//==========================================================
// Student Number : S10270619
// Student Name : Dhushyanth
// Partner Name : Hafiz
//==========================================================
//==========================================================
// Student Number : S10270619
// Student Name : Dhushyanth
// Partner Name : Hafiz
//==========================================================
using System.Collections.Generic;
public abstract class Flight : IComparable<Flight>
{
    public string FlightNumber { get; set; }
    public string Origin { get; set; }
    public string Destination { get; set; }
    public DateTime ExpectedTime { get; set; }
    public string Status { get; set; }

    public Flight(string flightNumber, string origin, string destination, DateTime expectedTime, string status = "Scheduled")
    {
        FlightNumber = flightNumber;
        Origin = origin;
        Destination = destination;
        ExpectedTime = expectedTime;
        Status = status;
    }

    public virtual double CalculateFees()
    {
        double BaseFee = 300;  // Boarding Gate Base Fee

        if (Destination == "SIN")
            BaseFee += 500;  // Arriving flight fee
        else if (Origin == "SIN")
            BaseFee += 800;  // Departing flight fee
        return BaseFee;
    }

    public int CompareTo(Flight f)
    {
        return ExpectedTime.CompareTo(f.ExpectedTime);
    }

    public override string ToString()
    {
        return $"{FlightNumber}: {Origin} -> {Destination}, Time: {ExpectedTime}, Status: {Status}";
    }

}
