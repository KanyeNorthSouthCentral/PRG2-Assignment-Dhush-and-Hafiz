//==========================================================
// Student Number : S10270619
// Student Name : Dhushyanth
// Partner Name : Hafiz
//==========================================================
﻿using System.Collections.Generic;
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

    public double CalculateFees()
    {
        double totalFee = 0;

        // Flight type fee
        if (Destination == "SIN")
        {
            totalFee += 500;  // Arriving Flight Fee
        }
        if (Origin == "SIN")
        {
            totalFee += 800;  // Departing Flight Fee
        }

        // Boarding gate base fee
        totalFee += 300;

        // Special request code fee
        switch (SpecialRequestCode)
        {
            case "DDJB":
                totalFee += 300;
                break;
            case "CFFT":
                totalFee += 150;
                break;
            case "LWTT":
                totalFee += 500;
                break;
            default:
                break;
        }

        // Apply discounts
        if (ExpectedTime.Hour < 11 || ExpectedTime.Hour > 21)
        {
            totalFee -= 110;  // Discount for early or late flights
        }

        if (Origin == "DXB" || Origin == "BKK" || Origin == "NRT")
        {
            totalFee -= 25;  // Discount for specific origins
        }

        if (string.IsNullOrEmpty(SpecialRequestCode))
        {
            totalFee -= 50;  // Discount for flights without special requests
        }

        return totalFee;
    }

    public override string ToString()
    {
        return $"{FlightNumber}: {Origin} -> {Destination}, Time: {ExpectedTime}, Status: {Status}";
    }

    public int CompareTo(Flight f)
    {
        return ExpectedTime.CompareTo(f.ExpectedTime);
    }
}
