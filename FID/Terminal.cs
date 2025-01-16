



using System.Collections.Generic;

//DHUSH//
public class Terminal
{
    public string TerminalName { get; set; }
    public Dictionary<string, Airline> Airlines { get; set; }
    public Dictionary<string, Flight> Flights { get; set; }
    public Dictionary<string, BoardingGate> BoardingGates { get; set; }
    public Dictionary<string, double> GateFees { get; set; }

    public Terminal(string terminalName)
    {
        TerminalName = terminalName;
        Airlines = new Dictionary<string, Airline>();
        Flights = new Dictionary<string, Flight>();
        BoardingGates = new Dictionary<string, BoardingGate>();
        GateFees = new Dictionary<string, double>();
    }

    public bool AddAirline(Airline airline)
    {
        if (!Airlines.ContainsKey(airline.Code))
        {
            Airlines[airline.Code] = airline;
            return true;
        }
        return false;
    }

    public bool AddBoardingGate(BoardingGate gate)
    {
        if (!BoardingGates.ContainsKey(gate.GateName))
        {
            BoardingGates[gate.GateName] = gate;
            return true;
        }
        return false;
    }

    public Airline GetAirlineFromFlight(Flight flight)
    {
        return Airlines.TryGetValue(flight.FlightNumber.Substring(0, 2), out var airline) ? airline : null;
    }

    public void PrintAirlineFees()
    {
        foreach (var airline in Airlines.Values)
        {
            Console.WriteLine($"{airline.Name}: {airline.CalculateFees()}");
        }
    }

    public override string ToString()
    {
        return $"Terminal: {TerminalName}, Airlines: {Airlines.Count}, Flights: {Flights.Count}, Gates: {BoardingGates.Count}";
    }
}
