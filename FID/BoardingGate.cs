//==========================================================
// Student Number : S10270619
// Student Name : Dhushyanth
// Partner Name : Hafiz
//==========================================================

using FID;
using System.Collections.Generic;

public class BoardingGate
{
    public string GateName { get; set; }
    public bool SupportsCFFT { get; set; }
    public bool SupportsDDJB { get; set; }
    public bool SupportsLWTT { get; set; }
    public Flight Flight { get; set; }

    public BoardingGate(string gateName, bool supportsCFFT, bool supportsDDJB, bool supportsLWTT)
    {
        GateName = gateName;
        SupportsCFFT = supportsCFFT;
        SupportsDDJB = supportsDDJB;
        SupportsLWTT = supportsLWTT;
        Flight = null;
    }

    // Calculate fees based on assigned flight
    public double CalculateFees()
    {
        double fees = 300; // Base fee
        if (Flight is DDJBFlight)
            fees += 300;
        if (Flight is CFFTFlight)
            fees += 150;
        if (Flight is LWTTFlight)
            fees += 500;
        return fees;
    }

    public override string ToString()
    {
        return $"{GateName} - Flight: {(Flight != null ? Flight.FlightNumber : "Unassigned")}";
    }
}
