//==========================================================
// Student Number : S10270619
// Student Name : Dhushyanth
// Partner Name : Hafiz
//==========================================================

using System.Collections.Generic;

public class Airline
{
    public string Name { get; set; }
    public string Code { get; set; }
    public Dictionary<string, Flight> Flights { get; set; }

    public Airline(string name, string code)
    {
        Name = name;
        Code = code;
        Flights = new Dictionary<string, Flight>();
    }

    public bool AddFlight(Flight flight)
    {
        if (!Flights.ContainsKey(flight.FlightNumber))
        {
            Flights[flight.FlightNumber] = flight;
            return true;
        }
        return false;
    }
    
    public double CalculateFees()
    {
        double totalFees = 0;

        foreach (var flight in Flights.Values)
        {
            totalFees += flight.CalculateFees();
            if (flight.Destination == "Singapore (SIN)") // Arriving flight
            {
                totalFees += 500;
            }
            if (flight.Origin == "Singapore (SIN)") // Departing flight
            {
                totalFees += 800;
            }
        }

        return ApplyPromotions(totalFees);
    }
    
    // Apply promotional discounts based on various conditions
    private double ApplyPromotions(double totalFees)
    {
        int flightCount = Flights.Count;
        double discount = 0;

        // Promotion: Every 3 flights arriving/departing
        discount += (flightCount / 3) * 350;

        // Promotion: For each flight before 11am or after 9pm
        discount += Flights.Values.Count(f => f.ExpectedTime.Hour < 11 || f.ExpectedTime.Hour > 21) * 110;

        // Promotion: For flights originating from Dubai, Bangkok, Tokyo
        discount += Flights.Values.Count(f => f.Origin == "Dubai (DXB)" || f.Origin == "Bangkok (BKK)" || f.Origin == "Tokyo (NRT)") * 25;

        // Promotion: For flights without special requests
        discount += Flights.Values.Count(f => f is NORMFlight) * 50;

        // Promotion: Additional 3% discount if more than 5 flights
        if (flightCount > 5)
        {
            totalFees *= 0.97;  // Apply 3% off
        }

        return totalFees - discount;
    }
    
    public bool RemoveFlight(Flight flight)
    {
        return Flights.Remove(flight.FlightNumber);
    }

    public override string ToString()
    {
        return $"{Code} - {Name}";
    }
}﻿
