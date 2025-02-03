using FID;
using System;
using System.Collections.Generic;
using System.IO;

class Program
{
    static Terminal terminal = new Terminal("Changi Airport Terminal 5");

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
                    //Hafiz Feature 3 //
                    ListAllFlights();
                    break;
                case 2:
                    //Dhush Feature 4 //
                    ListBoardingGates();
                    break;
                case 3:
                    //Hafiz Feature 5 //
                    AssignBoardingGateToFlight();
                    break;
                case 4:
                    //Hafiz Feature 6 //
                    CreateFlight();
                    break;
                case 5:
                    //Dhush Feature 7 //
                    DisplayFullFlightDetailsFromAirline();
                    break;
                case 6:
                    //Dhush Feature 8 //
                    ModifyFlightDetails();
                    break;
                case 7:
                    //Hafiz Feature 9 //
                    DisplayScheduledFlights();
                    break;
                case 8:
                    //Dhush Advanced Feature//
                    ProcessUnassignedFlights();
                    break;
                case 9:
                    //Hafiz Advanced Feature//
                    DisplayTotalFeePerAirline();
                    break;
                case 10:
                    //Dhush Additional Feature//
                    SearchAndFilterFlights();
                    break;
                case 11:
                    //Dhush Additional Feature//
                    GenerateFlightDelayReport(); 
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

    //Collab//
    static int GetUserInput()
    {
        if (int.TryParse(Console.ReadLine(), out int option))
        {
            return option;
        }
        return -1;
    }
    //Collab//
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
        Console.WriteLine("8. Process Unassigned Flights");
        Console.WriteLine("9. Display Total Fee Per Airline");
        Console.WriteLine("10. Search And Filter Flights");
        Console.WriteLine("11. Generate Flight Delay Report"); // New option
        Console.WriteLine("0. Exit");
        Console.Write("\nPlease select your option: ");
    }

    //Dhush Feature 1 ////
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
                string name = data[0].Trim();  // Trim to remove any extra spaces
                string code = data[1].Trim();  // Trim to remove any extra spaces
                // Add the name and code into the dictionary
                terminal.Airlines[code] = new Airline(name, code);
                // increase the counter 
                count++;
            }
        }

        //  Total number of airlines that were loaded from the file
        Console.WriteLine($"{count} Airlines Loaded!");
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
                sr.ReadLine(); string line;
                while ((line = sr.ReadLine()) != null)
                {
                    // Split the line by commas to extract flight details
                    var data = line.Split(',');

                    // Check if the data has at least 5 elements and if the 4th element can be parsed as a DateTime
                    if (data.Length >= 5 && DateTime.TryParse(data[3].Trim(), out DateTime expectedTime))
                    {
                        string flightNumber = data[0].Trim();
                        string originFull = data[1].Trim();  // e.g. "Tokyo (NRT)"
                        string destinationFull = data[2].Trim();  // e.g. "Singapore (SIN)"

                        // This function extracts any IATA code inside parentheses:
                        string ExtractIata(string full)
                        {
                            // If the string has parentheses, split or find them
                            int openParen = full.IndexOf('(');
                            int closeParen = full.IndexOf(')');

                            // If found, extract what's inside; else return the full string
                            if (openParen >= 0 && closeParen > openParen)
                            {
                                // E.g. "Tokyo (NRT)" => we substring from openParen+1 to closeParen-1 => "NRT"
                                return full.Substring(openParen + 1, closeParen - (openParen + 1)).Trim();
                            }
                            else
                            {
                                // No parentheses, just return the entire string
                                return full;
                            }
                        }

                        string origin = ExtractIata(originFull);
                        string destination = ExtractIata(destinationFull);
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

                        terminal.Flights[flightNumber] = flight;

                        string airlineCode = flightNumber.Substring(0, 2);
                        if (terminal.Airlines.ContainsKey(airlineCode))
                        {
                            terminal.Airlines[airlineCode].AddFlight(flight);
                        }

                        count++;
                    }
                    else
                    {

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
        // Display header for the list of flights
        Console.WriteLine("=============================================");
        Console.WriteLine("List of Flights for Changi Airport Terminal 5");
        Console.WriteLine("=============================================");
        Console.WriteLine($"{"Flight Number",-15} {"Airline Name",-25} {"Origin",-20} {"Destination",-20} {"Expected Departure/Arrival Time",-30}");

        // Create a list of flights and sort them
        List<Flight> flightList = new List<Flight>(terminal.Flights.Values);
        flightList.Sort();

        // Iterate through each flight and display its details
        foreach (var flight in flightList)
        {
            // Extract the airline code from the flight number
            string airlineCode = flight.FlightNumber.Substring(0, 2).Trim();
            string airlineName = "Unknown Airline";

            // Check if the airline code exists in the dictionary and get the airline name
            if (terminal.Airlines.ContainsKey(airlineCode))
            {
                airlineName = terminal.Airlines[airlineCode].Name;
            }

            // Display the flight details with time in 12-hour format
            Console.WriteLine($"{flight.FlightNumber,-15} {airlineName,-25} {flight.Origin,-20} {flight.Destination,-20} {flight.ExpectedTime.ToString("dd/M/yyyy hh:mm tt"),-30}");
        }
    }

    //Dhush Feature 4//
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
                    bool supportsDDJB = bool.Parse(data[1].Trim()); // Parse as boolean (true/false)
                    bool supportsCFFT = bool.Parse(data[2].Trim()); // Parse as boolean (true/false)
                    bool supportsLWTT = bool.Parse(data[3].Trim()); // Parse as boolean (true/false)
                    // Add the Boarding Gate name and special request into the dictionary
                    terminal.BoardingGates[gateName] = new BoardingGate(gateName, supportsCFFT, supportsDDJB, supportsLWTT);
                    count++;
                }
            }
        }
        Console.WriteLine($"{count} Boarding Gates Loaded!");
    }

    //Dhush Feature 4 //
    static void ListBoardingGates()
    {
        // Display header for the list of boarding gates
        Console.WriteLine("=============================================");
        Console.WriteLine("List of Boarding Gates for Changi Airport Terminal 5");
        Console.WriteLine("=============================================");
        Console.WriteLine($"{"Gate Name",-19} {"DDJB",-24} {"CFFT",-19} {"LWTT",-20} ");

        // Iterate through each boarding gate and display its details
        foreach (var gate in terminal.BoardingGates.Values)
        {
            Console.WriteLine($"{gate.GateName,-20}{gate.SupportsDDJB,-25}{gate.SupportsCFFT,-20}{gate.SupportsLWTT,-20}");
        }
    }

    //Hafiz Feature 5 //
    static void AssignBoardingGateToFlight()
    {
        Console.WriteLine("=============================================");
        Console.WriteLine("Assign a Boarding Gate to a Flight");
        Console.WriteLine("=============================================");

        // Prompt the user for the Flight Number
        Console.Write("Enter Flight Number: ");
        string flightNumber = Console.ReadLine().Trim().ToUpper();

        // Check if the flight exists
        if (!terminal.Flights.ContainsKey(flightNumber))
        {
            Console.WriteLine("Invalid Flight Number. Please try again.");
            return;
        }

        Flight flight = terminal.Flights[flightNumber];

        // Prompt the user for the Boarding Gate Name
        Console.Write("Enter Boarding Gate Name: ");
        string gateName = Console.ReadLine().Trim().ToUpper();

        // Check if the boarding gate exists
        if (!terminal.BoardingGates.ContainsKey(gateName))
        {
            Console.WriteLine("Invalid Boarding Gate Name. Please try again.");
            return;
        }

        BoardingGate gate = terminal.BoardingGates[gateName];

        // Check if the boarding gate supports the flight's special request
        if ((flight is CFFTFlight && !gate.SupportsCFFT) ||
            (flight is DDJBFlight && !gate.SupportsDDJB) ||
            (flight is LWTTFlight && !gate.SupportsLWTT))
        {
            Console.WriteLine($"Boarding Gate {gateName} does not support the flight's special request. Please choose a different gate.");
            return;
        }

        // Check if the boarding gate is already assigned to another flight
        if (gate.Flight != null)
        {
            Console.WriteLine($"Boarding Gate {gateName} is already assigned to another flight. Please choose a different gate.");
            return;
        }

        // Display the flight and gate details
        Console.WriteLine($"Flight Number: {flight.FlightNumber}");
        Console.WriteLine($"Origin: {flight.Origin}");
        Console.WriteLine($"Destination: {flight.Destination}");
        Console.WriteLine($"Expected Time: {flight.ExpectedTime:dd/M/yyyy h:mm:ss tt}");
        Console.WriteLine($"Special Request Code: {(flight is CFFTFlight ? "CFFT" : flight is DDJBFlight ? "DDJB" : flight is LWTTFlight ? "LWTT" : "None")}");
        Console.WriteLine($"Boarding Gate Name: {gate.GateName}");
        Console.WriteLine($"Supports DDJB: {gate.SupportsDDJB}");
        Console.WriteLine($"Supports CFFT: {gate.SupportsCFFT}");
        Console.WriteLine($"Supports LWTT: {gate.SupportsLWTT}");

        // Prompt the user if they would like to update the status of the flight
        Console.Write("Would you like to update the status of the flight? (Y/N): ");
        string updateStatus = Console.ReadLine().Trim().ToUpper();

        if (updateStatus == "Y")
        {
            Console.WriteLine("1. Delayed");
            Console.WriteLine("2. Boarding");
            Console.WriteLine("3. On Time");
            Console.Write("Please select the new status of the flight: ");
            if (int.TryParse(Console.ReadLine().Trim(), out int statusOption))
            {
                switch (statusOption)
                {
                    case 1:
                        flight.Status = "Delayed";
                        break;
                    case 2:
                        flight.Status = "Boarding";
                        break;
                    case 3:
                        flight.Status = "On Time";
                        break;
                    default:
                        Console.WriteLine("Invalid option. Status not updated.");
                        break;
                }
            }
            else
            {
                Console.WriteLine("Invalid input. Status not updated.");
            }
        }

        // Assign the boarding gate to the flight
        gate.Flight = flight;
        Console.WriteLine($"Flight {flight.FlightNumber} has been assigned to Boarding Gate {gate.GateName}!");
    }

    //Hafiz Feature 6  ////
    static void CreateFlight()
    {   
        while (true)
        {
            string flightNumber;
            while (true)
            {
                Console.Write("Enter Flight Number: ");
                flightNumber = Console.ReadLine().Trim().ToUpper();

                // Validate flight number format (e.g., TR 123)
                if (string.IsNullOrEmpty(flightNumber))
                {
                    Console.WriteLine("Flight number cannot be empty. Please enter again.");
                }
                else if (flightNumber.Length >= 4 && flightNumber.Length <= 7 &&
                         char.IsLetter(flightNumber[0]) && char.IsLetter(flightNumber[1]) &&
                         flightNumber[2] == ' ' &&
                         int.TryParse(flightNumber.Substring(3), out _))
                {
                    if (terminal.Flights.ContainsKey(flightNumber))
                    {
                        Console.WriteLine("Flight number already exists. Please enter a different flight number.");
                    }
                    else
                    {
                        break;
                    }
                }
                else
                {
                    Console.WriteLine("Invalid flight number format. Please enter again (e.g., TR 123).");
                }
            }

            string origin;
            while (true)
            {
                Console.Write("Enter Origin: ");
                origin = Console.ReadLine().Trim();

                // Validate that origin does not contain numbers
                if (string.IsNullOrEmpty(origin))
                {
                    Console.WriteLine("Origin cannot be empty. Please enter again.");
                }
                else if (!ContainsDigit(origin))
                {
                    break;
                }
                else
                {
                    Console.WriteLine("Invalid origin. It should not contain numbers. Please enter again.");
                }
            }

            string destination;
            while (true)
            {
                Console.Write("Enter Destination: ");
                destination = Console.ReadLine().Trim();

                // Validate that destination does not contain numbers
                if (string.IsNullOrEmpty(destination))
                {
                    Console.WriteLine("Destination cannot be empty. Please enter again.");
                }
                else if (!ContainsDigit(destination))
                {
                    break;
                }
                else
                {
                    Console.WriteLine("Invalid destination. It should not contain numbers. Please enter again.");
                }
            }

            DateTime expectedTime;
            while (true)
            {
                Console.Write("Enter Expected Departure/Arrival Time (dd/MM/yyyy HH:mm): ");
                string input = Console.ReadLine().Trim();
                // Try to parse the input string to a DateTime object using the specified format
                if (string.IsNullOrEmpty(input))
                {
                    Console.WriteLine("Expected time cannot be empty. Please enter again.");
                }
                else if (DateTime.TryParseExact(input, "dd/MM/yyyy HH:mm", null, System.Globalization.DateTimeStyles.None, out expectedTime))
                {
                    break;
                }
                else
                {
                    Console.WriteLine("Invalid format. Please enter again in dd/MM/yyyy HH:mm format.");
                }
            }

            string requestType;
            while (true)
            {
                Console.Write("Enter Special Request Code (CFFT/DDJB/LWTT/None): ");
                requestType = Console.ReadLine().Trim().ToUpper();
                if (string.IsNullOrEmpty(requestType))
                {
                    Console.WriteLine("Special request code cannot be empty. Please enter again.");
                }
                else if (new[] { "CFFT", "DDJB", "LWTT", "NONE" }.Contains(requestType))
                {
                    break;
                }
                else
                {
                    Console.WriteLine("Invalid special request code. Please enter again (CFFT/DDJB/LWTT/None).");
                }
            }

            string status = "Scheduled";
            Flight newFlight = requestType switch
            {
                "CFFT" => new CFFTFlight(flightNumber, origin, destination, expectedTime, status, 150),
                "DDJB" => new DDJBFlight(flightNumber, origin, destination, expectedTime, status, 300),
                "LWTT" => new LWTTFlight(flightNumber, origin, destination, expectedTime, status, 500),
                _ => new NORMFlight(flightNumber, origin, destination, expectedTime, status)
            };

            // Add to Dictionary
            terminal.Flights[flightNumber] = newFlight;

            // Assign flight to airline if exists
            string airlineCode = flightNumber.Substring(0, 2);
            if (terminal.Airlines.ContainsKey(airlineCode))
            {
                terminal.Airlines[airlineCode].AddFlight(newFlight);
            }
            else
            {
                Console.WriteLine($"Warning: Airline '{airlineCode}' not found. Flight stored only in the terminal.");
            }

            // Append to flights.csv with correct format
            using (StreamWriter sw = new StreamWriter("flights.csv", true))
            {
                sw.WriteLine($"{flightNumber},{origin},{destination},{expectedTime:hh:mm:tt},{requestType}");
            }

            Console.WriteLine($"Flight {flightNumber} has been added!");

            Console.Write("Would you like to add another flight? (Y/N): ");
            string addMore = Console.ReadLine().Trim().ToUpper();
            if (addMore != "Y")
            {
                Console.WriteLine("Flight(s) have been successfully added!");
                break;
            }
        }
    }

    static bool ContainsDigit(string input)
    {
        foreach (char c in input)
        {
            if (char.IsDigit(c))
            {
                return true;
            }
        }
        return false;
    }

    //Dhush Feature 7 //
    static void DisplayFullFlightDetailsFromAirline()
    {
        Console.WriteLine("=============================================");
        Console.WriteLine("List of Airlines for Changi Airport Terminal 5");
        Console.WriteLine("=============================================");
        Console.WriteLine($"{"Airline Code",-15} {"Airline Name"}");

        // Ensure airlines are loaded
        if (terminal.Airlines.Count == 0)
        {
            Console.WriteLine("No airlines found. Please check if airlines.csv is loaded.");
            return;
        }

        foreach (var airlineEntry in terminal.Airlines.Values)
        {
            Console.WriteLine($"{airlineEntry.Code,-15} {airlineEntry.Name}");
        }

        Console.Write("\nEnter Airline Code: ");
        string airlineCode = Console.ReadLine().Trim().ToUpper();

        if (!terminal.Airlines.ContainsKey(airlineCode))
        {
            Console.WriteLine("Invalid Airline Code. Please enter a valid code from the list.");
            return;
        }

        var selectedAirline = terminal.Airlines[airlineCode];

        Console.WriteLine("\n=============================================");
        Console.WriteLine($"List of Flights for {selectedAirline.Name}");
        Console.WriteLine("=============================================");
        Console.WriteLine($"{"Flight Number",-15} {"Airline Name",-25} {"Origin",-25} {"Destination",-25} {"Expected Departure/Arrival Time",-30}");

        bool hasFlights = false;

        foreach (var flight in terminal.Flights.Values)
        {
            if (flight.FlightNumber.Replace(" ", "").StartsWith(airlineCode, StringComparison.OrdinalIgnoreCase))
            {
                hasFlights = true;

                // Retrieve Airline Name
                string airlineName = terminal.Airlines.ContainsKey(airlineCode) ? terminal.Airlines[airlineCode].Name : "Unknown Airline";

                // Format the date correctly
                string formattedTime = flight.ExpectedTime.ToString("d/M/yyyy h:mm:ss tt");

                Console.WriteLine($"{flight.FlightNumber,-15} {airlineName,-25} {flight.Origin,-25} {flight.Destination,-25} {formattedTime,-30}");
            }
        }

        if (!hasFlights)
        {
            Console.WriteLine("No flights found for this airline.");
        }

        // Prevent immediate menu return
        Console.WriteLine("\nPress any key to return to the main menu...");
        Console.ReadKey();
    }

    //Dhush Feature 8 //   
    static void ModifyFlightDetails()
    {
        Console.WriteLine("\n=============================================");
        Console.WriteLine("List of Airlines for Changi Airport Terminal 5");
        Console.WriteLine("=============================================");
        Console.WriteLine($"{"Airline Code",-15} {"Airline Name"}");

        // Display all airlines
        foreach (var airlineEntry in terminal.Airlines.Values)
        {
            Console.WriteLine($"{airlineEntry.Code,-15} {airlineEntry.Name}");
        }

        // Prompt user for Airline Code
        Console.Write("\nEnter Airline Code: ");
        string airlineCode = Console.ReadLine()?.Trim().ToUpper();

        // Validate airline code
        if (string.IsNullOrEmpty(airlineCode) || !terminal.Airlines.TryGetValue(airlineCode, out var airline))
        {
            Console.WriteLine("Invalid Airline Code. Please try again.");
            return;
        }

        // Display Flights under selected airline
        Console.WriteLine($"\nList of Flights for {airline.Name}");
        Console.WriteLine("=============================================");
        Console.WriteLine($"{"Flight Number",-15} {"Airline Name",-25} {"Origin",-25} {"Destination",-25} {"Expected Departure/Arrival Time",-30}");

        if (airline.Flights.Count == 0)
        {
            Console.WriteLine("No flights available for this airline.");
            return;
        }

        // Display each flight's details
        foreach (var flight in airline.Flights.Values)
        {
            Console.WriteLine($"{flight.FlightNumber,-15} {airline.Name,-25} {flight.Origin,-25} {flight.Destination,-25} {flight.ExpectedTime:dd/MM/yyyy h:mm:ss tt}");
        }

        Console.WriteLine();
        Console.Write("Choose an existing Flight to modify or delete: ");
        string flightNumber = Console.ReadLine()?.Trim().ToUpper();

        // Validate flight number
        if (string.IsNullOrEmpty(flightNumber) || !airline.Flights.TryGetValue(flightNumber, out var selectedFlight))
        {
            Console.WriteLine("Invalid Flight Number. Please try again.");
            return;
        }

        // Ask whether to modify or delete
        Console.WriteLine("1. Modify Flight\n2. Delete Flight");
        Console.WriteLine();
        Console.WriteLine("Choose an option: ");
        if (!int.TryParse(Console.ReadLine()?.Trim(), out int choice) || (choice != 1 && choice != 2))
        {
            Console.WriteLine("Invalid choice. Please enter 1 or 2.");
            return;
        }

        if (choice == 1) // Modify Flight
        {
            Console.WriteLine("\n1. Modify Basic Information");
            Console.WriteLine("2. Modify Status");
            Console.WriteLine("3. Modify Special Request Code");
            Console.WriteLine("4. Modify Boarding Gate");
            Console.WriteLine();
            Console.WriteLine("Choose an option: ");

            if (!int.TryParse(Console.ReadLine()?.Trim(), out int detailChoice) || detailChoice < 1 || detailChoice > 4)
            {
                Console.WriteLine("Invalid choice.");
                return;
            }

            switch (detailChoice)
            {
                case 1: // Modify Basic Information
                    Console.Write("Enter new Origin: ");
                    string newOrigin = Console.ReadLine()?.Trim();
                    if (string.IsNullOrEmpty(newOrigin) || ContainsDigit(newOrigin))
                    {
                        Console.WriteLine("Invalid origin. It should not be empty or contain numbers. Please try again.");
                        return;
                    }

                    Console.Write("Enter new Destination: ");
                    string newDestination = Console.ReadLine()?.Trim();
                    if (string.IsNullOrEmpty(newDestination) || ContainsDigit(newDestination))
                    {
                        Console.WriteLine("Invalid destination. It should not be empty or contain numbers. Please try again.");
                        return;
                    }

                    Console.Write("Enter new Expected Departure/Arrival Time (dd/MM/yyyy HH:mm): ");
                    if (!DateTime.TryParseExact(Console.ReadLine()?.Trim(), "dd/MM/yyyy HH:mm", null, System.Globalization.DateTimeStyles.None, out var newTime))
                    {
                        Console.WriteLine("Invalid date format. Please enter again in dd/MM/yyyy HH:mm format.");
                        return;
                    }

                    // Update flight details
                    selectedFlight.Origin = newOrigin;
                    selectedFlight.Destination = newDestination;
                    selectedFlight.ExpectedTime = newTime;
                    break;

                case 2: // Modify Status
                    Console.WriteLine("1. Delayed");
                    Console.WriteLine("2. Boarding");
                    Console.WriteLine("3. On Time");
                    Console.Write("Please select the new status of the flight: ");
                    if (int.TryParse(Console.ReadLine().Trim(), out int statusOption))
                    {
                        switch (statusOption)
                        {
                            case 1:
                                selectedFlight.Status = "Delayed";
                                break;
                            case 2:
                                selectedFlight.Status = "Boarding";
                                break;
                            case 3:
                                selectedFlight.Status = "On Time";
                                break;
                            default:
                                Console.WriteLine("Invalid option. Status not updated.");
                                break;
                        }
                    }
                    else
                    {
                        Console.WriteLine("Invalid input. Status not updated.");
                    }
                    Console.WriteLine("\nStatus updated successfully.");
                    break;

                case 3: // Modify Special Request Code
                    Console.Write("Enter new Special Request Code (CFFT, DDJB, LWTT, None): ");
                    string newRequestCode = Console.ReadLine()?.Trim().ToUpper();
                    if (string.IsNullOrEmpty(newRequestCode) || !new[] { "CFFT", "DDJB", "LWTT", "NONE" }.Contains(newRequestCode))
                    {
                        Console.WriteLine("Invalid special request code. Please enter one of the following: CFFT, DDJB, LWTT, None.");
                        return;
                    }

                    // Update flight special request code
                    selectedFlight = newRequestCode switch
                    {
                        "CFFT" => new CFFTFlight(selectedFlight.FlightNumber, selectedFlight.Origin, selectedFlight.Destination, selectedFlight.ExpectedTime, selectedFlight.Status, 150),
                        "DDJB" => new DDJBFlight(selectedFlight.FlightNumber, selectedFlight.Origin, selectedFlight.Destination, selectedFlight.ExpectedTime, selectedFlight.Status, 300),
                        "LWTT" => new LWTTFlight(selectedFlight.FlightNumber, selectedFlight.Origin, selectedFlight.Destination, selectedFlight.ExpectedTime, selectedFlight.Status, 500),
                        _ => new NORMFlight(selectedFlight.FlightNumber, selectedFlight.Origin, selectedFlight.Destination, selectedFlight.ExpectedTime, selectedFlight.Status)
                    };

                    airline.Flights[flightNumber] = selectedFlight;
                    terminal.Flights[flightNumber] = selectedFlight;
                    Console.WriteLine("\nSpecial Request Code updated successfully.");
                    break;

                case 4: // Modify Boarding Gate
                    Console.Write("Enter new Boarding Gate: ");
                    string newGateName = Console.ReadLine()?.Trim().ToUpper();

                    if (string.IsNullOrEmpty(newGateName) || !terminal.BoardingGates.ContainsKey(newGateName))
                    {
                        Console.WriteLine("Invalid Boarding Gate. Please try again.");
                        return;
                    }

                    BoardingGate newGate = terminal.BoardingGates[newGateName];

                    // Remove flight from any previously assigned gate
                    foreach (var gate in terminal.BoardingGates.Values)
                    {
                        if (gate.Flight == selectedFlight)
                        {
                            gate.Flight = null;
                            break;
                        }
                    }

                    // Assign flight to new gate if available
                    if (newGate.Flight == null)
                    {
                        newGate.Flight = selectedFlight;
                        Console.WriteLine("\nBoarding Gate updated successfully.");
                    }
                    else
                    {
                        Console.WriteLine("\nThe selected Boarding Gate is already occupied.");
                    }
                    break;
            }
            // Display updated flight details
            Console.WriteLine("\nFlight updated!");
            Console.WriteLine($"Flight Number: {selectedFlight.FlightNumber}");
            Console.WriteLine($"Airline Name: {airline.Name}");
            Console.WriteLine($"Origin: {selectedFlight.Origin}");
            Console.WriteLine($"Destination: {selectedFlight.Destination}");
            Console.WriteLine($"Expected Departure/Arrival Time: {selectedFlight.ExpectedTime:dd/MM/yyyy h:mm:ss tt}");
            Console.WriteLine($"Status: {selectedFlight.Status}");
            Console.WriteLine($"Special Request Code: {(selectedFlight is CFFTFlight ? "CFFT" : selectedFlight is DDJBFlight ? "DDJB" : selectedFlight is LWTTFlight ? "LWTT" : "None")}");
            Console.WriteLine($"Boarding Gate: {terminal.BoardingGates.Values.FirstOrDefault(g => g.Flight == selectedFlight)?.GateName ?? "Unassigned"}");
        }
        else if (choice == 2) // Delete Flight
        {
            Console.Write("\nAre you sure you want to delete this flight? [Y/N]: ");
            if (Console.ReadLine()?.Trim().ToUpper() == "Y")
            {
                airline.Flights.Remove(flightNumber);
                terminal.Flights.Remove(flightNumber);

                // Remove flight from any boarding gate
                foreach (var gate in terminal.BoardingGates.Values)
                {
                    if (gate.Flight?.FlightNumber == flightNumber)
                    {
                        gate.Flight = null;
                        break;
                    }
                }

                Console.WriteLine("\nFlight deleted successfully.");
            }
            else
            {
                Console.WriteLine("\nFlight deletion cancelled.");
            }
        }
    }

    //Hafiz Feature 9 //
    static void DisplayScheduledFlights()
    {
        // Display header for the flight schedule
        Console.WriteLine("=============================================");
        Console.WriteLine("Flight Schedule for Changi Airport Terminal 5");
        Console.WriteLine("=============================================");
        Console.WriteLine($"{"Flight Number",-15} {"Airline Name",-25} {"Origin",-25} {"Destination",-25} {"Expected Departure/Arrival Time",-30}   {"Status",-18} {"Boarding Gate",-15}");

        // Create a list of flights and sort them by expected time
        List<Flight> flightList = new List<Flight>(terminal.Flights.Values);
        flightList.Sort();

        // Iterate through each flight and display its details
        foreach (var flight in flightList)
        {
            // Extract the airline code from the flight number
            string airlineCode = flight.FlightNumber.Substring(0, 2).Trim();
            // Get the airline name from the dictionary, default to "Unknown Airline" if not found
            string airlineName = terminal.Airlines.ContainsKey(airlineCode) ? terminal.Airlines[airlineCode].Name : "Unknown Airline";
            // Default boarding gate to "Unassigned"
            string boardingGate = "Unassigned";

            // Check if the flight is assigned to a boarding gate
            foreach (var gate in terminal.BoardingGates.Values)
            {
                if (gate.Flight != null && gate.Flight.FlightNumber == flight.FlightNumber)
                {
                    boardingGate = gate.GateName;
                    break;
                }
            }

            // Format the expected time for display
            string expectedTimeFormatted = flight.ExpectedTime.ToString("dd/M/yyyy h:mm:ss tt").PadRight(30);
            // Display the flight details
            Console.WriteLine($"{flight.FlightNumber,-15} {airlineName,-25} {flight.Origin,-25} {flight.Destination,-25} {expectedTimeFormatted}    {flight.Status,-18} {boardingGate,-16}");
        }
    }

    //Dhush Advanced Feature (a)  //// 
    static void ProcessUnassignedFlights()
    {
        Console.WriteLine("=============================================");
        Console.WriteLine("Processing Unassigned Flights in Bulk");
        Console.WriteLine("=============================================");

        Queue<Flight> unassignedFlights = new Queue<Flight>();
        int alreadyAssignedFlights = 0, assignedFlights = 0;
        int alreadyAssignedGates = 0, assignedGates = 0;

        // Identify unassigned flights and count already assigned flights
        foreach (var flight in terminal.Flights.Values)
        {
            bool isAssigned = terminal.BoardingGates.Values.Any(gate => gate.Flight?.FlightNumber == flight.FlightNumber);
            if (!isAssigned)
            {
                unassignedFlights.Enqueue(flight);
            }
            else
            {
                alreadyAssignedFlights++;
            }
        }

        Console.WriteLine($"Total Unassigned Flights: {unassignedFlights.Count}");

        // Identify unassigned boarding gates and count already assigned gates
        foreach (var gate in terminal.BoardingGates.Values)
        {
            if (gate.Flight == null)
            {
                assignedGates++;
            }
            else
            {
                alreadyAssignedGates++;
            }
        }

        Console.WriteLine($"Total Unassigned Boarding Gates: {assignedGates}");

        // Process and assign flights to boarding gates
        while (unassignedFlights.Count > 0 && assignedGates > 0)
        {
            Flight flight = unassignedFlights.Dequeue();
            BoardingGate assignedGate = null;

            // Step 1: Try to assign a gate that matches the flight's special request
            foreach (var gate in terminal.BoardingGates.Values)
            {
                if (gate.Flight == null &&
                    ((flight is CFFTFlight && gate.SupportsCFFT) ||
                     (flight is DDJBFlight && gate.SupportsDDJB) ||
                     (flight is LWTTFlight && gate.SupportsLWTT)))
                {
                    assignedGate = gate;
                    break;
                }
            }

            // Step 2: If no special request gate is found, assign any available gate
            if (assignedGate == null)
            {
                foreach (var gate in terminal.BoardingGates.Values)
                {
                    if (gate.Flight == null && !gate.SupportsCFFT && !gate.SupportsDDJB && !gate.SupportsLWTT)
                    {
                        assignedGate = gate;
                        break;
                    }
                }
            }

            // Assign the gate if one was found
            if (assignedGate != null)
            {
                assignedGate.Flight = flight;
                flight.Status = "On Time"; // Default status update after assigning
                assignedFlights++;
                assignedGates--;

                // Retrieve airline name
                string airlineName = terminal.Airlines.Values.FirstOrDefault(a => flight.FlightNumber.StartsWith(a.Code, StringComparison.OrdinalIgnoreCase))?.Name ?? "Unknown";

                // Display Flight Assignment Details
                Console.WriteLine("\nFlight Assigned:");
                Console.WriteLine($"Flight Number: {flight.FlightNumber}");
                Console.WriteLine($"Airline Name: {airlineName}");
                Console.WriteLine($"Origin: {flight.Origin}");
                Console.WriteLine($"Destination: {flight.Destination}");
                Console.WriteLine($"Expected Departure/Arrival Time: {flight.ExpectedTime:dd/M/yyyy h:mm:ss tt}");
                Console.WriteLine($"Special Request Code: {(flight is CFFTFlight ? "CFFT" : flight is DDJBFlight ? "DDJB" : flight is LWTTFlight ? "LWTT" : "None")}");
                Console.WriteLine($"Assigned Boarding Gate: {assignedGate.GateName}");
            }
            else
            {
                Console.WriteLine($"No available gate for Flight {flight.FlightNumber}.");
            }
        }

        // Calculate percentages
        double flightProcessingRate = (alreadyAssignedFlights + assignedFlights) > 0
            ? (double)assignedFlights / (alreadyAssignedFlights + assignedFlights) * 100
            : 0;

        double gateProcessingRate = (alreadyAssignedGates + assignedGates) > 0
            ? (double)assignedGates / (alreadyAssignedGates + assignedGates) * 100
            : 0;

        // Final Summary
        Console.WriteLine("\n=============================================");
        Console.WriteLine($"Total Flights Processed & Assigned: {assignedFlights}");
        Console.WriteLine($"Total Boarding Gates Processed & Assigned: {assignedGates}");
        Console.WriteLine($"Flights Processed vs Already Assigned: {flightProcessingRate:F2}%");
        Console.WriteLine($"Boarding Gates Processed vs Already Assigned: {gateProcessingRate:F2}%");
        Console.WriteLine("Processing Complete.");
    }

    //Hafiz Advanced Feature (b)  ////
    static void DisplayTotalFeePerAirline()
    {
        Console.WriteLine("=============================================");
        Console.WriteLine("Displaying Total Fees Per Airline");
        Console.WriteLine("=============================================");

        // 1) Check if any flights are unassigned:
        bool anyUnassigned = terminal.Flights.Values
            .Any(f => !terminal.BoardingGates.Values.Any(g => g.Flight == f));
        if (anyUnassigned)
        {
            Console.WriteLine("Some flights have not been assigned a Boarding Gate.");
            Console.WriteLine("Please ensure that all unassigned Flights have their Boarding Gates assigned");
            Console.WriteLine("before running this feature again.\n");
            return;
        }

        // Grand totals
        double grandSubtotal = 0.0;
        double grandDiscountTotal = 0.0;
        double grandFinalTotal = 0.0;

        Console.WriteLine();
        Console.WriteLine(
          $"{"Airline Name",-23} {"Subtotal Before Discount",-28} {"Total Discounts",-18} {"Final Fees",-12}"
        );

        // 2) Process each airline
        foreach (var kvp in terminal.Airlines)
        {
            Airline airline = kvp.Value;

            double airlineSubtotal = 0.0;
            double airlineDiscounts = 0.0;

            // 3) Go through each flight
            foreach (Flight flight in airline.Flights.Values)
            {
                // -- A) Fees from base + origin/dest + special request
                double flightFee = flight.CalculateFees();
                airlineSubtotal += flightFee;

                // -- B) Flight-level discounts
                if (flight.ExpectedTime.TimeOfDay < new TimeSpan(11, 0, 0) ||
                    flight.ExpectedTime.TimeOfDay > new TimeSpan(21, 0, 0))
                {
                    airlineDiscounts += 110;
                }

                if (flight.Origin == "DXB" || flight.Origin == "BKK" || flight.Origin == "NRT")
                {
                    airlineDiscounts += 25;
                }

                if (flight is NORMFlight)
                {
                    airlineDiscounts += 50;
                }
            }

            // -- C) Airline-level discounts
            // For every 3 flights, discount $350
            airlineDiscounts += (Math.Floor(airline.Flights.Count / 3.0) * 350);

            // If airline has more than 5 flights, discount extra 3% of the airlineSubtotal
            if (airline.Flights.Count > 5)
            {
                airlineDiscounts += (airlineSubtotal * 0.03);
            }

            double finalAirlineFee = airlineSubtotal - airlineDiscounts;

            // Accumulate into grand totals
            grandSubtotal += airlineSubtotal;
            grandDiscountTotal += airlineDiscounts;
            grandFinalTotal += finalAirlineFee;

            // Display per airline
            Console.WriteLine($"{airline.Name,-23} ${airlineSubtotal,-26:F2} ${airlineDiscounts,-16:F2} ${finalAirlineFee,-10:F2}");
        }

        // 4) Show final summary
        double discountPercent = (grandFinalTotal > 0)
             ? (grandDiscountTotal / grandFinalTotal) * 100
             : 0.0;

        Console.WriteLine("\n=============================================");
        Console.WriteLine($"Subtotal of ALL Airline Fees (before discounts): ${grandSubtotal:F2}");
        Console.WriteLine($"Subtotal of ALL Airline Discounts:               ${grandDiscountTotal:F2}");
        Console.WriteLine($"Final Total of ALL Airline Fees (after discounts): ${grandFinalTotal:F2}");
        Console.WriteLine($"Percentage of Discounts Over Final Total of Fees: {discountPercent:F2}%");
    }

    //DHUSH Additional Feature    //// 
    static void SearchAndFilterFlights()
    {
        Console.WriteLine("=============================================");
        Console.WriteLine("Search and Filter Flights");
        Console.WriteLine("=============================================");

        // Display search and filter options
        Console.WriteLine("1. Search by Flight Number");
        Console.WriteLine("2. Filter by Origin");
        Console.WriteLine("3. Filter by Destination");
        Console.WriteLine("4. Filter by Airline");
        Console.WriteLine("5. Filter by Status");
        Console.WriteLine("6. Filter by Boarding Gate");
        Console.WriteLine("7. Display All Flights");
        Console.Write("\nPlease select your option: ");

        // Get user input for the filter option
        int option = GetUserInput();
        if (option < 1 || option > 7)
        {
            Console.WriteLine("Invalid option. Please try again.");
            return;
        }

        // Initialize the list of flights to be filtered
        List<Flight> filteredFlights = new List<Flight>(terminal.Flights.Values);

        // Apply the selected filter
        switch (option)
        {
            case 1: // Search by Flight Number
                Console.Write("Enter Flight Number: ");
                string flightNumber = Console.ReadLine().Trim().ToUpper();
                if (string.IsNullOrEmpty(flightNumber))
                {
                    Console.WriteLine("Flight number cannot be empty. Please try again.");
                    return;
                }
                filteredFlights = filteredFlights.Where(f => f.FlightNumber.Equals(flightNumber, StringComparison.OrdinalIgnoreCase)).ToList();
                break;

            case 2: // Filter by Origin
                Console.Write("Enter Origin Airport Code: ");
                string originCode = Console.ReadLine().Trim().ToUpper();
                if (string.IsNullOrEmpty(originCode))
                {
                    Console.WriteLine("Origin airport code cannot be empty. Please try again.");
                    return;
                }
                filteredFlights = filteredFlights.Where(f => f.Origin.Equals(originCode, StringComparison.OrdinalIgnoreCase)).ToList();
                break;

            case 3: // Filter by Destination
                Console.Write("Enter Destination: ");
                string destination = Console.ReadLine().Trim().ToUpper();
                if (string.IsNullOrEmpty(destination))
                {
                    Console.WriteLine("Destination cannot be empty. Please try again.");
                    return;
                }
                filteredFlights = filteredFlights.Where(f => f.Destination.Equals(destination, StringComparison.OrdinalIgnoreCase)).ToList();
                break;

            case 4: // Filter by Airline
                Console.Write("Enter Airline Code: ");
                string airlineCode = Console.ReadLine().Trim().ToUpper();
                if (string.IsNullOrEmpty(airlineCode))
                {
                    Console.WriteLine("Airline code cannot be empty. Please try again.");
                    return;
                }
                filteredFlights = filteredFlights.Where(f => f.FlightNumber.StartsWith(airlineCode, StringComparison.OrdinalIgnoreCase)).ToList();
                break;

            case 5: // Filter by Status
                Console.Write("Enter Status (Scheduled, Delayed, Boarding, On Time): ");
                string status = Console.ReadLine().Trim();
                if (string.IsNullOrEmpty(status) || !new[] { "Scheduled", "Delayed", "Boarding", "On Time" }.Contains(status))
                {
                    Console.WriteLine("Invalid status. Please enter one of the following: Scheduled, Delayed, Boarding, On Time.");
                    return;
                }
                filteredFlights = filteredFlights.Where(f => f.Status.Equals(status, StringComparison.OrdinalIgnoreCase)).ToList();
                break;

            case 6: // Filter by Boarding Gate
                Console.Write("Enter Boarding Gate: ");
                string gateName = Console.ReadLine().Trim().ToUpper();
                if (string.IsNullOrEmpty(gateName))
                {
                    Console.WriteLine("Boarding gate cannot be empty. Please try again.");
                    return;
                }
                filteredFlights = filteredFlights.Where(f => terminal.BoardingGates.Values.Any(g => g.GateName.Equals(gateName, StringComparison.OrdinalIgnoreCase) && g.Flight?.FlightNumber == f.FlightNumber)).ToList();
                break;

            case 7: // Display All Flights
                break; // No filtering needed
        }

        // Check if any flights match the filter criteria
        if (filteredFlights.Count == 0)
        {
            Console.WriteLine("No flights found matching the criteria.");
            return;
        }

        // Display the filtered flights
        Console.WriteLine("\n=============================================");
        Console.WriteLine("Filtered Flights");
        Console.WriteLine("=============================================");
        Console.WriteLine($"{"Flight Number",-15} {"Airline Name",-25} {"Origin",-20} {"Destination",-20} {"Expected Time",-30} {"Status",-15} {"Boarding Gate",-15}");

        // Iterate through the filtered flights and display their details
        foreach (var flight in filteredFlights)
        {
            string airlineCode = flight.FlightNumber.Substring(0, 2).Trim();
            string airlineName = terminal.Airlines.ContainsKey(airlineCode) ? terminal.Airlines[airlineCode].Name : "Unknown Airline";
            string boardingGate = "Unassigned";

            // Check if the flight is assigned to a boarding gate
            foreach (var gate in terminal.BoardingGates.Values)
            {
                if (gate.Flight != null && gate.Flight.FlightNumber == flight.FlightNumber)
                {
                    boardingGate = gate.GateName;
                    break;
                }
            }
            // Display the flight details
            Console.WriteLine($"{flight.FlightNumber,-15} {airlineName,-25} {flight.Origin,-20} {flight.Destination,-20} {flight.ExpectedTime.ToString("dd/M/yyyy HH:mm"),-30} {flight.Status,-15} {boardingGate,-15}");
        }
    }

    //HAFIZ Additional Feature    ////
    static void GenerateFlightDelayReport()
    {
        Console.WriteLine("Generating Flight Delay Report...");

        var delayedFlights = terminal.Flights.Values.Where(f => f.Status == "Delayed").ToList();

        if (delayedFlights.Count == 0)
        {
            Console.WriteLine("No delayed flights found.");
            return;
        }

        Console.WriteLine("=============================================");
        Console.WriteLine("Flight Delay Report for Changi Airport Terminal 5");
        Console.WriteLine("=============================================");
        Console.WriteLine($"{"Flight Number",-15} {"Airline Name",-25} {"Origin",-20} {"Destination",-20} {"Expected Time",-30} {"Delay Duration",-20}");

        foreach (var flight in delayedFlights)
        {
            string airlineCode = flight.FlightNumber.Substring(0, 2).Trim();
            string airlineName = terminal.Airlines.ContainsKey(airlineCode) ? terminal.Airlines[airlineCode].Name : "Unknown Airline";
            TimeSpan delayDuration = DateTime.Now - flight.ExpectedTime;

            Console.WriteLine($"{flight.FlightNumber,-15} {airlineName,-25} {flight.Origin,-20} {flight.Destination,-20} {flight.ExpectedTime.ToString("dd/M/yyyy hh:mm tt"),-30} {delayDuration.Hours} hours {delayDuration.Minutes} minutes");
        }
    }

}
