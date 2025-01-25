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
    // Properties
    public string FlightNumber { get; set; }
    public string Origin { get; set; }
    public string Destination { get; set; }
    public DateTime ExpectedTime { get; set; }
    public string Status { get; set; }

    // Constructor
    public Flight(string flightNumber, string origin, string destination, DateTime expectedTime, string status = "Scheduled")
    {
        FlightNumber = flightNumber;
        Origin = origin;
        Destination = destination;
        ExpectedTime = expectedTime;
        Status = status;
    }

    // Method to calculate fees
    public virtual double CalculateFees()
    {
        double totalFees = 300;  // Boarding Gate Base Fee
        if (Destination == "SIN")
            totalFees += 500;  // Arriving flight fee
        else if (Origin == "SIN")
            totalFees += 800;  // Departing flight fee
        return totalFees;
    }

    // Method to compare flights based on expected time
    public int CompareTo(Flight f)
    {
        return ExpectedTime.CompareTo(f.ExpectedTime);
    }

    // Method to return flight details as a string
    public override string ToString()
    {
        return $"{FlightNumber}: {Origin} -> {Destination}, Time: {ExpectedTime}, Status: {Status}";
    }
}
