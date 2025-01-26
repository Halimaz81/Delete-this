using PRG2_Assignment;
using PRG2_FinalAssignment;

Terminal terminal = new Terminal("Terminal 5");
void Loadfiles()// task 1
{
    string[] airlines = File.ReadAllLines("airlines.csv");
    Console.WriteLine("Loading Airlines...");
    for (int i = 1; i < airlines.Length; i++)
    {
        string[] airlinedeets = airlines[i].Split(',');
        Airline airlinex = new Airline(airlinedeets[0], airlinedeets[1]);
        terminal.AddAirline(airlinex);
    }
    Console.WriteLine($"{airlines.Length} Airlines Loaded! ");

    Console.WriteLine("Loading Boarding Gates...");
    string[] boardinggates = File.ReadAllLines("boardinggates.csv");
    for (int i = 1; i < boardinggates.Length; i++)
    {
        string[] boardingdeets = boardinggates[i].Split(",");
        BoardingGate boardinggatex = new BoardingGate(boardingdeets[0], Convert.ToBoolean(boardingdeets[2]), Convert.ToBoolean(boardingdeets[1]), Convert.ToBoolean(boardingdeets[3])); //Boarding Gate,DDJB,CFFT,LWTT ,bool _supportscfft, bool _supportsddjb, bool _supportslwtt, Flight _flight)
        terminal.AddBoardingGate(boardinggatex);
    }
    Console.WriteLine($"{boardinggates.Length} Boarding gates Loaded! ");

    Console.WriteLine("Done");
}


void LoadFlights(string flightscsv) //Task 2
{
    Console.WriteLine("Loading Flights...");
    if (!File.Exists(flightscsv))
    {
        Console.WriteLine("File does not exists!");
    }

    try
    {
        using (StreamReader sr = new StreamReader(flightscsv))
        {
            sr.ReadLine();
            string? data;
            while ((data = sr.ReadLine()) != null)
            {
                string[] linedata = data.Split(",");

                if (linedata.Length < 4)
                {
                    Console.WriteLine("Invalid flight data found, skipping..");
                    continue;
                }

                string flightNumber = linedata[0];
                string origin = linedata[1];
                string destination = linedata[2];
                DateTime expectedDepartureArrival = DateTime.ParseExact(linedata[3], "h:mm tt", null);
                string? specialRequestCode = null;

                if (linedata.Length == 5)
                {
                    specialRequestCode = linedata[4];
                }

                Flight flight;

                if (specialRequestCode == "DDJB")
                {
                    flight = new DDJBFlight(flightNumber, origin, destination, expectedDepartureArrival, 300);
                }
                else if (specialRequestCode == "CFFT")
                {
                    flight = new CFFTFlight(flightNumber, origin, destination, expectedDepartureArrival, 150);
                }
                else if (specialRequestCode == "LWTT")
                {
                    flight = new LWTTFlight(flightNumber, origin, destination, expectedDepartureArrival, 500);
                }
                else
                {
                    flight = new NORMFlight(flightNumber, origin, destination, expectedDepartureArrival);
                }

                terminal.Flights[flight.FlightNumber] = flight;
            }
        }
        Console.WriteLine($"{terminal.Flights.Count} Flights Loaded!");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error loading flights: {ex.Message}");
    }
}



void ListAllFlights(Dictionary<string, Flight> flightDict, Dictionary<string, Airline> airlineDict)  //Task 3
{
    string airlineName = "";
    string code = "";

    if (flightDict.Keys.Count == 0)
    {
        Console.WriteLine("There are no available flights.");
    }
    Console.WriteLine("=============================================\r\nList of Flights for Changi Airport Terminal 5\r\n=============================================\r\n");
    Console.WriteLine($"{"Flight Number",-18}{"Airline Name",-20}{"Orgin",-25}{"Destination",-25}{"Expected Departure/Arrival Time",-40}");




    foreach (Flight flight in flightDict.Values)
    {
        Airline airline = terminal.GetAirlineFromFlight(flight);
        if (airline != null)
        {
            airlineName = airline.Name;
            Console.WriteLine($"{flight.FlightNumber,-18}{airlineName,-20}{flight.Origin,-25}{flight.Destination,-25}{flight.ExpectedTime,-40}");
        }
        else
        {
            continue;
        }
    }
}


void listAllBoardingGates() // task 4
{
    Console.WriteLine("=============================================");
    Console.WriteLine("List of Boarding Gates for Changi Airport Terminal 5");
    Console.WriteLine("=============================================");
    Console.WriteLine($"{"Gate Name",-10} {"DDJB",-10} {"CFFT",-10} {"LWTT",-10}");
    string[] boardinggates = File.ReadAllLines("boardinggates.csv");
    for (int i = 1; i < boardinggates.Length; i++)
    {
        string[] boardingdeets = boardinggates[i].Split(",");
        Console.WriteLine($"{boardingdeets[0],-10} {boardingdeets[1],-10} {boardingdeets[2],-10} {boardingdeets[3],-10}");
    }
}


void AssignBoardingGate(Dictionary<string, Flight> flightsDict, Dictionary<string, BoardingGate> boardingGates)  //Task 5
{
    Console.WriteLine("=============================================\r\nAssign a Boarding Gate to a Flight\r\n=============================================\r\n");
    bool approvedFlightNumber = false;
    bool approvedBoardingGateName = false;
    Flight selectedFlight = null;
    BoardingGate selectedBoardingGate = null;

    while (true)
    {
        approvedFlightNumber = false;
        approvedBoardingGateName = false;
        selectedFlight = null;
        selectedBoardingGate = null;

        Console.WriteLine("Enter the Flight Number: ");
        string flightNumber = Console.ReadLine();
        Console.WriteLine("Enter Boarding Gate Name: ");
        string boardingGateName = Console.ReadLine();

        if (flightsDict.ContainsKey(flightNumber))
        {
            selectedFlight = flightsDict[flightNumber];
            approvedFlightNumber = true;
        }

        if (boardingGates.ContainsKey(boardingGateName))
        {
            selectedBoardingGate = boardingGates[boardingGateName];
            approvedBoardingGateName = true;

        }

        if (!approvedFlightNumber)
        {
            Console.WriteLine("Invalid Flight number. Please try again.");
            continue;
        }
        else if (!approvedBoardingGateName)
        {
            Console.WriteLine("Invalid Boarding Gate Name. Please try again.");
            continue;
        }
        else if (selectedBoardingGate.Flight != null)
        {
            Console.WriteLine($"Boarding Gate {boardingGateName} is already assigned to Flight {selectedBoardingGate.Flight.FlightNumber}. Please choose a different gate.");
            continue;

        }
        break;
    }
    Console.WriteLine($"Flight Number: {selectedFlight.FlightNumber}");
    Console.WriteLine($"Origin: {selectedFlight.Origin}");
    Console.WriteLine($"Destination: {selectedFlight.Destination}");
    Console.WriteLine($"Expected Departure/Arrival: {selectedFlight.ExpectedTime}");
    if (selectedFlight is DDJBFlight)
    {
        Console.WriteLine($"Special Request Code: DDJB");
    }
    else if (selectedFlight is CFFTFlight)
    {
        Console.WriteLine($"Special Request Code: CFFT");
    }
    else if (selectedFlight is LWTTFlight)
    {
        Console.WriteLine($"Special Request Code: LWTT");
    }
    else
    {
        Console.WriteLine($"Special Request Code: None");
    }

    Console.WriteLine($"Boarding Gate Name: {selectedBoardingGate.GateName}");
    Console.WriteLine($"Supports DDJB: {selectedBoardingGate.SupportsDDJB}");
    Console.WriteLine($"Supports CFFT: {selectedBoardingGate.SupportsCFFT}");
    Console.WriteLine($"Supports LWTT: {selectedBoardingGate.SupportsLWTT}");
    while (true)
    {
        Console.WriteLine("Would you like to update the status of the flight? (Y/N): ");
        string? updateStatusChoice = Console.ReadLine().ToUpper();

        if (updateStatusChoice == "Y")
        {
            Console.WriteLine("1. Delayed");
            Console.WriteLine("2. Boarding");
            Console.WriteLine("3. On Time");
            Console.WriteLine("Please select the new status of the flight:");
            string? statusChoice = Console.ReadLine();
            if (statusChoice == "1")
            {
                selectedFlight.Status = "Delayed";
            }
            else if (statusChoice == "2")
            {
                selectedFlight.Status = "Boarding";
            }
            else if (statusChoice == "3")
            {
                selectedFlight.Status = "On Time";
            }
            else
            {
                Console.WriteLine("Invalid choice. Setting status to 'On Time' by default.");
                selectedFlight.Status = "On Time";
            }
            break;
        }
        else if (updateStatusChoice == "N")
        {
            selectedFlight.Status = "On Time";
            break;
        }
        else
        {
            Console.WriteLine("Invalid Input, input can only be (Y/N). Please try again.");
            continue;
        }
    }
    Console.WriteLine("Updated Flight Details:");
    Console.WriteLine($"Flight {selectedFlight.FlightNumber} has been assigned to Boarding Gate {selectedBoardingGate.GateName}");
    Console.WriteLine($"Status: {selectedFlight.Status}");
}
void CreateNewFlight(Dictionary<string, Flight> flights) //Task 6
{
    while (true)
    {
        string flightNumber;
        string origin;
        string destination;
        DateTime expectedTime;
        Flight newFlight;
        string specialRequestCode = "None";

        Console.Write("Enter Flight Number: ");
        flightNumber = Console.ReadLine().Trim();
        Console.Write("Enter Origin: ");
        origin = Console.ReadLine().Trim();
        Console.Write("Enter Destination: ");
        destination = Console.ReadLine().Trim();

        while (true)
        {
            Console.Write("Enter Expected Departure/Arrival Time (dd/mm/yyyy hh:mm): ");
            string timeInput = Console.ReadLine().Trim();
            if (DateTime.TryParseExact(timeInput, "dd/MM/yyyy HH:mm", null, System.Globalization.DateTimeStyles.None, out expectedTime))
            {
                break;
            }
            Console.WriteLine("Invalid date format. Please try again.");
        }

        Console.Write("Do you have any additional information to add (Y/N): ");
        string choice = Console.ReadLine().ToUpper().Trim();
        if (choice == "Y")
        {
            Console.Write("Enter the special request code (DDJB/CFFT/LWTT/None): ");
            specialRequestCode = Console.ReadLine().ToUpper().Trim();
            if (specialRequestCode == "DDJB")
            {
                newFlight = new DDJBFlight(flightNumber, origin, destination, expectedTime, 300);
            }
            else if (specialRequestCode == "CFFT")
            {
                newFlight = new CFFTFlight(flightNumber, origin, destination, expectedTime, 150);
            }
            else if (specialRequestCode == "LWTT")
            {
                newFlight = new LWTTFlight(flightNumber, origin, destination, expectedTime, 500);
            }
            else if (specialRequestCode == "NONE" || specialRequestCode == "")
            {
                newFlight = new NORMFlight(flightNumber, origin, destination, expectedTime);
            }
            else
            {
                Console.WriteLine("Invalid special request code.. exiting.");
                return;
            }
        }
        else if (choice == "N")
        {
            newFlight = new NORMFlight(flightNumber, origin, destination, expectedTime);
        }
        else
        {
            Console.WriteLine("Invalid input.. exiting.");
            return;
        }

        terminal.Flights[newFlight.FlightNumber] = newFlight;

        Console.WriteLine($"Flight {flightNumber} has been successfully added to the system.");

        string filePath = "flights.csv";
        try
        {
            using (StreamWriter sw = new StreamWriter(filePath, true))
            {
                sw.WriteLine($"{flightNumber},{origin},{destination},{expectedTime.ToString("t")},{specialRequestCode.ToUpper()}");
            }
            Console.WriteLine("Flight information has been saved to flights.csv.");
        }
        catch (IOException ex)
        {
            Console.WriteLine($"Error writing to file: {ex.Message}");
        }

        Console.Write("Would you like to add another flight? (Y/N): ");
        string anotherFlight = Console.ReadLine().ToUpper();
        if (anotherFlight != "Y")
        {
            break;
        }
    }
}

/////////////////
///
void displayFlights() //task 7 
{
    Console.WriteLine("=============================================");
    Console.WriteLine("List of Airlines for Changi Airport Terminal 5");
    Console.WriteLine("=============================================");
    Console.WriteLine($"{"Airline Code",-15} {"Airline Name"}");
    string[] airlines = File.ReadAllLines("airlines.csv");
    for (int i = 1; i < airlines.Length; i++)
    {
        string[] airlinedeets = airlines[i].Split(',');
        Console.WriteLine($"{airlinedeets[1],-15} {airlinedeets[0]}");
    }
    Console.Write("Enter Airline Code: ");
    string code = Console.ReadLine();
    if (terminal.Airlines.ContainsKey(code))
    {
        Console.WriteLine("=============================================");
        Console.WriteLine($"{"Flight Number",-15} {"Airline Name",-20} {"Origin",-20} {"Destination",-20} {"Expected",-20}");
        Airline retrievedAirline = terminal.Airlines[code];
        //Console.WriteLine($"Number of flights for airline {retrievedAirline.Name}: {retrievedAirline.Flights.Count}"); //check to see if dict is populated  
        foreach (KeyValuePair<string, Flight> lol in retrievedAirline.Flights)  // the airline dict already has the appropriate flights assigned to it. 
        {
            Console.WriteLine($"{lol.Value.FlightNumber,-15} {code,-20} {lol.Value.Origin,-20} {lol.Value.Destination,-20} {lol.Value.ExpectedTime,20}");
        }
    }
    else
    {
        Console.WriteLine($"Airline with code '{code}' not found.");
    }
}

////////////////
///

while (true)
{
    Loadfiles();
    LoadFlights("flights.csv");
    Console.WriteLine("=============================================\r\nWelcome to Changi Airport Terminal 5\r\n=============================================\r\n");
    Console.WriteLine("1. List All Flights");
    Console.WriteLine("2. List Boarding Gates");
    Console.WriteLine("3. Assign a Boarding Gate to a Flight");
    Console.WriteLine("4. Create Flight");
    Console.WriteLine("5. Display Airline Flights");
    Console.WriteLine("6. Modify Flight Details");
    Console.WriteLine("7. Display Flight Schedule");
    Console.WriteLine("0. Exit");
    Console.WriteLine("Please select your option:");
    string? choice = Console.ReadLine();
    if (choice != null)
    {
        if (choice == "1")
        {
            ListAllFlights(terminal.Flights, terminal.Airlines);
        }
        else if (choice == "2")
        {
            listAllBoardingGates();
        }
        else if (choice == "3")
        {
            AssignBoardingGate(terminal.Flights, terminal.BoardingGates);
        }
        else if (choice == "4")
        {
            CreateNewFlight(terminal.Flights);
        }
        else if (choice == "5")
        {
            displayFlights();
        }
        else if (choice == "6")
        {

        }
        else if (choice == "7")
        {

        }
        else if (choice == "0")
        {
            Console.WriteLine("Goodbye!");
            break;
        }
    }
    else
    {
        Console.WriteLine("Please enter an option, if not 0 to exit.");
        continue;
    }

}
