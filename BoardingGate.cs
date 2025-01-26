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
    class BoardingGate
    {
        public string GateName { get; set; }
        public bool SupportsCFFT { get; set; }
        public bool SupportsDDJB { get; set; }
        public bool SupportsLWTT { get; set; }
        public Flight Flight { get; set; }

        public BoardingGate() { }
        public BoardingGate(string gateName, bool supportsCFFT, bool supportsDDJB, bool supportsLWTT)
        {
            GateName = gateName;
            SupportsCFFT = supportsCFFT;
            SupportsDDJB = supportsDDJB;
            SupportsLWTT = supportsLWTT;
        }

        public double CalculateFees()
        {
            double baseFee = 300;
            if (SupportsCFFT == true)
                baseFee += 150;
            if (SupportsDDJB == true)
                baseFee += 300;
            if (SupportsLWTT == true)
                baseFee += 500;

            return baseFee;
        }

        public override string ToString()
        {
            return $"Gate {GateName}\nSupports CFFT: {SupportsCFFT}\nSupports DDJB: {SupportsDDJB}\nSupports LWTT: {SupportsLWTT}";
        }
    }
}
