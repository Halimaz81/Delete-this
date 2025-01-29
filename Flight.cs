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
    abstract class Flight : IComparable<Flight>
    {
        public string FlightNumber { get; set; }
        public string Origin { get; set; }
        public string Destination { get; set; }
        public DateTime ExpectedTime { get; set; }
        public string Status { get; set; } = "Scheduled";

        public Flight() { }

        public Flight(string flightNumber, string origin, string destination, DateTime expectedTime)
        {
            FlightNumber = flightNumber;
            Origin = origin;
            Destination = destination;
            ExpectedTime = expectedTime;
        }

        public int CompareTo(Flight other)
        {
            return this.ExpectedTime.CompareTo(other.ExpectedTime);
        }
        public abstract double CalculateFees();

        public override string ToString()
        {
            return $"{FlightNumber} from {Origin} to {Destination} at {ExpectedTime} is {Status}";
        }
    }
}
