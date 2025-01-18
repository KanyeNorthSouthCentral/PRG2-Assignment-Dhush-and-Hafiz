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

    public bool RemoveFlight(Flight flight)
    {
        return Flights.Remove(flight.FlightNumber);
    }

    public double CalculateFees()
    {
        double total = 0;
        foreach (var flight in Flights.Values)
        {
            total += flight.CalculateFees();
        }
        return total;
    }

    public override string ToString()
    {
        return $"{Code} - {Name}";
    }
}﻿
