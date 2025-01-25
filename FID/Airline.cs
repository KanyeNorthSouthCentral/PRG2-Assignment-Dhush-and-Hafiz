//==========================================================
// Student Number : S10270619
// Student Name : Dhushyanth
// Partner Name : Hafiz
//==========================================================

using System;
using System.Collections.Generic;
using System.Linq;

public class Airline
{
    // Properties to store airline details
    public string Name { get; set; }  // Airline name
    public string Code { get; set; }  // Unique airline code
    public Dictionary<string, Flight> Flights { get; private set; }  // Collection of flights indexed by flight number

    // Constructor to initialize airline object with name and code
    public Airline(string name, string code)
    {
        Name = name;
        Code = code;
        Flights = new Dictionary<string, Flight>();
    }

    // Method to add a flight to the airline's collection
    // Returns true if the flight is successfully added; false if the flight number already exists
    public bool AddFlight(Flight flight)
    {
        if (!Flights.ContainsKey(flight.FlightNumber))
        {
            Flights[flight.FlightNumber] = flight;
            return true;
        }
        return false;
    }

    // Method to remove a flight from the airline's collection
    // Returns true if the flight was found and removed; false otherwise
    public bool RemoveFlight(Flight flight)
    {
        if (Flights.ContainsKey(flight.FlightNumber))
        {
            Flights.Remove(flight.FlightNumber);
            return true;
        }
        return false;
    }

    // Method to calculate the total fees for all flights operated by the airline
    // Includes base fees and discounts based on promotional conditions
    public double CalculateTotalFees()
    {
        // Sum up the base fees for all flights
        double totalFees = Flights.Values.Sum(f => f.CalculateFees());
        int flightCount = Flights.Count;
        
        // Apply discount: $350 for every 3 flights arriving/departing
        totalFees -= 350 * (flightCount / 3);

        // Apply discount: $110 for flights departing before 11am or after 9pm
        totalFees -= Flights.Values.Count(f => f.ExpectedTime.Hour < 11 || f.ExpectedTime.Hour > 21) * 110;

        // Apply discount: $25 for flights originating from Dubai (DXB), Bangkok (BKK), or Tokyo (NRT)
        totalFees -= Flights.Values.Count(f => new[] { "DXB", "BKK", "NRT" }.Contains(f.Origin)) * 25;

        // Apply discount: $50 for flights without any special request codes
        totalFees -= Flights.Values.Count(f => string.IsNullOrEmpty(f.SpecialRequestCode)) * 50;

        // Apply 3% discount if the airline has more than 5 flights
        if (flightCount > 5)
            totalFees *= 0.97;

        return totalFees;
    }

    // Override ToString() method to provide formatted airline details
    public override string ToString()
    {
        return $"{Code} - {Name}, Flights Count: {Flights.Count}";
    }
}
