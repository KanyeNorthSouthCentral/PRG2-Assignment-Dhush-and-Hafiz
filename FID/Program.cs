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
        Console.WriteLine("\nPlease select your option:");
    }

    //DHUSH Feature 1//
    static void LoadAirlines()
    {
        Console.WriteLine("Loading Airlines...");    
        // Counter to track how many airlines loaded
        int count = 0;
        // Reading of the csv
        using (StreamReader sr = new StreamReader("airlines.csv"))
        {
            sr.ReadLine(); // Skip header
            // Variable to hold the line of the file as it is read
            string line;
            // Loop to read file till then end of the line 
            while ((line = sr.ReadLine()) != null)
            {
                var data = line.Split(','); 
                // Extracting the airline name and code
                string code = data[0].Trim();  // Trim to remove any extra spaces
                string name = data[1].Trim();  // Trim to remove any extra spaces
                // Add the name and code into the dictionary
                airlines[code] = new Airline(name, code);
                // increase the counter 
                count++;
            }
        }
        
        //  Total number of airlines that were loaded from the file
        Console.WriteLine($"{count} Airlines Loaded!");
    }

    static void LoadBoardingGates()
    {
        Console.WriteLine("Loading Boarding Gates...");
        // Counter to track how many airlines loaded
        int count = 0;
        // Reading of the csv
        using (StreamReader sr = new StreamReader("boardinggates.csv"))
        {
            sr.ReadLine(); // Skip header
            // Variable to hold the line of the file as it is read
            string line;
            // Loop to read file till then end of the line 
            while ((line = sr.ReadLine()) != null)
            {
                var data = line.Split(',');
                {
                    // Extracting the Boarding Gate name and special request
                    string gateName = data[0].Trim();
                    bool supportsCFFT = bool.Parse(data[1].Trim()); // Parse as boolean (true/false)
                    bool supportsDDJB = bool.Parse(data[2].Trim()); // Parse as boolean (true/false)
                    bool supportsLWTT = bool.Parse(data[3].Trim()); // Parse as boolean (true/false)
                    // Add the Boarding Gate name and special request into the dictionary
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
    }

    //Hafiz Feature 3 //
    static void ListAllFlights()
    {
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
    
    //Hafiz Feature 5 //

    //Hafiz Feature 6 //

    //Dhush Feature 7 //

    //Dhush Feature 8 //

    //Hafiz Feature 9 //
}
