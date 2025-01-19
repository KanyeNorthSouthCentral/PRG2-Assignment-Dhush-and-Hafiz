using FID;
using System;
using System.Collections.Generic;
using System.IO;

class Program
{
    static Dictionary<string, Airline> airlines = new Dictionary<string, Airline>();
    static Dictionary<string, BoardingGate> boardingGates = new Dictionary<string, BoardingGate>();
    static Dictionary<string, Flight> flights = new Dictionary<string, Flight>();

    static void Main(string[] args)
    {
        

        LoadAirlines();
        LoadBoardingGates();
        LoadFlights();

        while (true)
        {
            ShowMainMenu();
            int option = GetUserInput();

            switch (option)
            {
                case 1:
                    ListAllFlights();
                    break;
                case 2:
                    ListBoardingGates();
                    break;
                case 0:
                    Console.WriteLine("Exiting program. Goodbye!");
                    return;
                default:
                    Console.WriteLine("Invalid option. Please try again.");
                    break;
            }
        }
    }

    //COllab//
    static int GetUserInput()
    {
        if (int.TryParse(Console.ReadLine(), out int option))
        {
            return option;
        }
        return -1;
    }

    static void ShowMainMenu()
    {
        Console.WriteLine("\n=============================================");
        Console.WriteLine("Welcome to Changi Airport Terminal 5");
        Console.WriteLine("=============================================");
        Console.WriteLine("1. List All Flights");
        Console.WriteLine("2. List Boarding Gates");
        Console.WriteLine("3. Assign a Boarding Gate to a Flight");
        Console.WriteLine("4. Create Flight");
        Console.WriteLine("5. Display Airline Flights");
        Console.WriteLine("6. Modify Flight Details");
        Console.WriteLine("7. Display Flight Schedule");
        Console.WriteLine("0. Exit");
        Console.Write("\nPlease select your option: ");
    }

    //DHUSH Feature 1//
    static void LoadAirlines()
    {
        Console.WriteLine("Loading Airlines...");
        int count = 0;
        using (StreamReader sr = new StreamReader("airlines.csv"))
        {
            sr.ReadLine();
            string line;
            while ((line = sr.ReadLine()) != null)
            {
                var data = line.Split(',');
                if (data.Length >= 2)
                {
                    string name = data[0].Trim();
                    string code = data[1].Trim();
                    airlines[code] = new Airline(name,code);
                    count++;
                }
            }
        }
        Console.WriteLine($"{count} Airlines Loaded!");
    }

    static void LoadBoardingGates()
    {
        Console.WriteLine("Loading Boarding Gates...");
        int count = 0;
        using (StreamReader sr = new StreamReader("boardinggates.csv"))
        {
            sr.ReadLine();
            string line;
            while ((line = sr.ReadLine()) != null)
            {
                var data = line.Split(',');
                if (data.Length >= 4)
                {
                    string gateName = data[0].Trim();
                    bool supportsCFFT = bool.Parse(data[1].Trim());
                    bool supportsDDJB = bool.Parse(data[2].Trim());
                    bool supportsLWTT = bool.Parse(data[3].Trim());
                    boardingGates[gateName] = new BoardingGate(gateName, supportsCFFT, supportsDDJB, supportsLWTT);
                    count++;
                }
            }
        }
        Console.WriteLine($"{count} Boarding Gates Loaded!");
    }

    //Hafiz Feature 2 //
    static void LoadFlights()
    {
        Console.WriteLine("Loading Flights...");
        int count = 0;
        try
        {
            using (StreamReader sr = new StreamReader("flights.csv"))
            {
                sr.ReadLine();
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    var data = line.Split(',');
                    if (data.Length >= 5 && DateTime.TryParse(data[3].Trim(), out DateTime expectedTime))
                    {
                        string flightNumber = data[0].Trim();
                        string origin = data[1].Trim();
                        string destination = data[2].Trim();
                        string requestType = data[4].Trim(); // Special Request Code

                        // Default status for flights
                        string status = "Scheduled";

                        Flight flight = requestType switch
                        {
                            "CFFT" => new CFFTFlight(flightNumber, origin, destination, expectedTime, status, 150),
                            "DDJB" => new DDJBFlight(flightNumber, origin, destination, expectedTime, status, 300),
                            "LWTT" => new LWTTFlight(flightNumber, origin, destination, expectedTime, status, 500),
                            _ => new NORMFlight(flightNumber, origin, destination, expectedTime, status)
                        };

                        flights[flightNumber] = flight;

                        string airlineCode = flightNumber.Substring(0, 2);
                        if (airlines.ContainsKey(airlineCode))
                        {
                            airlines[airlineCode].AddFlight(flight);
                        }

                        count++;
                    }
                    else
                    {
                        Console.WriteLine($"Invalid row skipped: {line}");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading flights: {ex.Message}");
        }
        Console.WriteLine($"{count} Flights Loaded!");
    }

    //Hafiz Feature 3 //
    static void ListAllFlights()
    {
        Console.WriteLine("=============================================");
        Console.WriteLine("List of All Flights");
        Console.WriteLine("=============================================");
        Console.WriteLine($"{"Flight Number",-15} {"Airline Name",-25} {"Origin",-20} {"Destination",-20} {"Expected Departure/Arrival Time",-30}");

        foreach (var flight in flights.Values)
        {
            string airlineCode = flight.FlightNumber.Substring(0, 2).Trim();
            string airlineName = "Unknown Airline";
            
            if (airlines.ContainsKey(airlineCode))
            {
                airlineName = airlines[airlineCode].Name;
            }
            
            Console.WriteLine($"{flight.FlightNumber,-15} {airlineName,-25} {flight.Origin,-20} {flight.Destination,-20} {flight.ExpectedTime.ToString("dd/M/yyyy HH:mm"),-30}");
        }
    }




    //Dhush Feature 4 //
    static void ListBoardingGates()
    {
        Console.WriteLine("=============================================");
        Console.WriteLine("List of Boarding Gates for Changi Airport Terminal 5");
        Console.WriteLine("=============================================");
        Console.WriteLine("Gate Name       DDJB                   CFFT                   LWTT");

        foreach (var gate in boardingGates.Values)
        {
            Console.WriteLine($"{gate.GateName,-15}{gate.SupportsDDJB,-20}{gate.SupportsCFFT,-20}{gate.SupportsLWTT}");
        }
    }
}
