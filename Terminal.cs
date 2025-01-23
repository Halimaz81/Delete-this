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

        public Terminal() { }

        public Terminal(string terminalName)
        {
            TerminalName = terminalName;
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
            string code = "";
            for (int i = 0; i < 2; i++)
            {
                code += flight.FlightNumber[i];
            }
            if (Airlines.ContainsKey(code))
            {
                return Airlines[code];
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
