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
    class NORMFlight : Flight
    {
        public NORMFlight(string flightNumber, string origin, string destination, DateTime expectedTime) : base(flightNumber, origin, destination, expectedTime)
        {

        }
        public NORMFlight() : base() { }

        public override double CalculateFees()
        {
            return base.CalculateFees();
        }
        public override string ToString()
        {
            return base.ToString();
        }
    }
}

//im assuming you dont need to consider the special codes cus this is for normal flights


/////////////////////////////////////////////////////////////////////////////

//double discounts = 50; //defaults to 50 since this is a normal flight, no special req, hence discount is 50


//string[] flights = File.ReadAllLines("flights.csv");
//for (int i = 1; i < flights.Length; i++)
//{

//}

//TimeSpan eleven = new TimeSpan(11, 0, 0); // 11:00 AM
//TimeSpan nine = new TimeSpan(21, 0, 0); // 9:00 PM
//if (ExpectedTime.TimeOfDay < eleven) 
//{
//    discounts += 110;
//}

//else if (ExpectedTime.TimeOfDay > nine)
//{
//    discounts += 110;
//}






//if (Origin == "Dubai (DXB)")
//{
//    discounts += 25;
//}
//else if (Origin == "Bangkok(BKK)")
//{
//    discounts += 25;
//}
//else if (Origin == "Tokyo (NRT)")
//{
//    discounts += 25;
//}


//return totalfees - discounts;
//}//ending method brack

//    }//ending class brack
//}//ending ns brack