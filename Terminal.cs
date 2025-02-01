//==========================================================
// Student Number : S10267107C
// Student Name : Toh Keng Siong
// Partner Name : Ryan Tan Zong Hong
//==========================================================
using PRG2_FinalAssignment;
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
            double airlineFee = 0;
            double totalFees = 0;
            double totalDiscount = 0;
            foreach (Airline airlines in Airlines.Values)
            {
                double discount = 0;
                int flightCount = 0;
                airlineFee = airlines.CalculateFees();
                flightCount = 0; //re-initialize flightcount to 0 for each airline.
                foreach(Flight flight in airlines.Flights.Values)
                {
                    flightCount++; //counts the number of flight from the airline

                    discount += (flightCount / 3) * 350; // Discount for every 3 flights

                    if (flight.ExpectedTime.Hour < 11 || flight.ExpectedTime.Hour > 21) //Discount for flights before 11am and after 9pm
                    {
                        discount += 110;
                    }
                    if (flight.Origin.ToUpper() == "Dubai (DXB)" || flight.Origin.ToUpper() == "Bangkok (BKK)" || flight.Origin.ToUpper() == "Tokyo (NRT)") // Discount for flights from Dubai Bangkok ir Tokyo
                    {
                        discount += 25;
                    }
                    if (flight is NORMFlight) // Discount if flight doesnt have any special request codes.
                    {
                        discount += 50;
                    }
                }
                if (flightCount > 5)
                {
                    airlineFee *= 0.97; //apply 3 percent off to the airline fees before any discount
                }
                totalFees += airlineFee;
                totalDiscount += discount;
                Console.WriteLine($"Airline: {airlines.Code}, Total Fee: ${airlineFee:F2}, Discount Applied: ${discount:F2}"); 
            }
            double discountedTotalFees = totalFees - totalDiscount;
            double discountPercentage = (totalDiscount / discountedTotalFees) * 100;

            Console.WriteLine($"Subtotal Fees: ${totalFees:F2}");
            Console.WriteLine($"Total Discounts: ${totalDiscount:F2}");
            Console.WriteLine($"Final Total Fees Collected: ${discountedTotalFees:F2}");
            Console.WriteLine($"Discount Percentage: {discountPercentage:F2}%");

        }

        public override string ToString()
        {
            return $"Terminal name: {TerminalName}";
        }

    }
}
