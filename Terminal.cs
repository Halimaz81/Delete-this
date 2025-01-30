//==========================================================
// Student Number : S10267107C
// Student Name : Toh Keng Siong
// Partner Name : Ryan Tan Zong Hong
//==========================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRG2_Assignment
{
    class Terminal
    {
        public string TerminalName { get; set; }
        public Dictionary<string, Airline> Airlines = new Dictionary<string, Airline>();

        public Dictionary<string, Flight> Flights = new Dictionary<string, Flight>();

        public Dictionary<string, BoardingGate> BoardingGates = new Dictionary<string, BoardingGate>();

        public Dictionary<string, double> GateFees = new Dictionary<string, double>();
        public Queue<Flight> UnassignedFlights { get; private set; } = new Queue<Flight>();

        public List<BoardingGate> UnassignedGates { get; private set; } = new List<BoardingGate> { };



        public Terminal() { }

        public Terminal(string terminalName)
        {
            TerminalName = terminalName;
        }


        public BoardingGate GetAssignedGate(Flight flight) // extra method assigned only for the advanced feature.
        {
            foreach (BoardingGate gate in BoardingGates.Values)
            {
                if (gate.Flight == flight)
                {
                    return gate;
                }
            }
            return null; //returns null if theres no gate found for the flight. 
        }


        public bool AddAirline(Airline airline)
        {
            if (Airlines.ContainsKey(airline.Code))
            {
                return false;
            }
            Airlines[airline.Code] = airline;
            return true;
        }

        public bool AddBoardingGate(BoardingGate gate)
        {
            if (BoardingGates.ContainsKey(gate.GateName))
            {
                return false;
            }
            BoardingGates[gate.GateName] = gate;
            return true;
        }

        public Airline GetAirlineFromFlight(Flight flight)
        {
            foreach (Airline airline in Airlines.Values)
            {
                if (airline.Flights.ContainsKey(flight.FlightNumber))
                {
                    return airline;
                }
            }
            return null;
        }

        public void PrintAirlineFees() //method to calculate the amount of money the airline has to pay at the boardingGate based on the services / arrival and departing fees...
        {
            foreach (Airline airlines in Airlines.Values)
            {
                Console.WriteLine($"{airlines} fee: {airlines.CalculateFees()}");
            }
        }

        public override string ToString()
        {
            return $"Terminal name: {TerminalName}";
        }

    }
}
