using PRG2_Assignment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//==========================================================
// Student Number : S10268158
// Student Name : Ryan Tan Zong Hong
// Partner Name : Toh Keng Siong
//==========================================================

namespace PRG2_FinalAssignment
{
    class DDJBFlight : Flight
    {
        public double RequestFee { get; set; }
        public DDJBFlight(string flightNumber, string origin, string destination, DateTime expectedTime, double requestfee) : base(flightNumber, origin, destination, expectedTime)
        {
            RequestFee = requestfee;
        }
        public DDJBFlight() { }

        public override double CalculateFees()
        {
            return RequestFee + base.CalculateFees();
        }
        public override string ToString()
        {
            return base.ToString() + $"Request fee: {RequestFee}";
        }
    }
}
