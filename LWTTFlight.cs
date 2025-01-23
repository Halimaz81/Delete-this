using PRG2_Assignment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRG2_FinalAssignment
{
    class LWTTFlight : Flight
    {
        public double RequestFee { get; set; }
        public override double CalculateFees()
        {
            double extrafees = 0;
            if (Origin == "Singapore (SIN)")//departing flights
            {
                extrafees += 800;
            }
            else if (Destination == "Singapore (SIN)") //arriving flights
            {
                extrafees += 500;
            }

            double totalfees = 300 + RequestFee + extrafees; //300 is the Boarding gate base fee
            return totalfees;
        }


        public string Status = "On time";
        public LWTTFlight(string flightNumber, string origin, string destination, DateTime expectedTime, double requestfee) : base(flightNumber, origin, destination, expectedTime)
        {
            RequestFee = requestfee;
        }

        public LWTTFlight() : base() { }
        public override string ToString()
        {
            return base.ToString() + $"Request fee: {RequestFee}";
        }


    }
}
