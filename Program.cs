//==========================================================
// Student Number : S10267107C
// Partner Number : Sxxxxxxxxx
// Student Name : Toh Keng Siong
// Partner Name : Ryan Tan Zong Hong
//==========================================================
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
    Console.WriteLine($"{airlines.Length - 1} Airlines Loaded! "); // -1 to compensate for the header

    Console.WriteLine("Loading Boarding Gates...");
    string[] boardinggates = File.ReadAllLines("boardinggates.csv");
    for (int i = 1; i < boardinggates.Length; i++)
    {
        string[] boardingdeets = boardinggates[i].Split(",");
        BoardingGate boardinggatex = new BoardingGate(boardingdeets[0], Convert.ToBoolean(boardingdeets[2]), Convert.ToBoolean(boardingdeets[1]), Convert.ToBoolean(boardingdeets[3])); //Boarding Gate,DDJB,CFFT,LWTT ,bool _supportscfft, bool _supportsddjb, bool _supportslwtt, Flight _flight)
        terminal.AddBoardingGate(boardinggatex);
    }
    Console.WriteLine($"{boardinggates.Length - 1} Boarding gates Loaded! ");

    Console.WriteLine("Done");
}


void LoadFlights(string flightscsv) //Task 2
{
    if (!File.Exists(flightscsv))
    {
        Console.WriteLine("File does not exists!");
    }

    try
    {
        using (StreamReader sr = new StreamReader(flightscsv))
        {
            sr.ReadLine(); // Skip header
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

                // Create the flight object
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

                // Add the flight to the global terminal.Flights dictionary
                terminal.Flights[flight.FlightNumber] = flight;

                // Extract airline code from flight number
                string airlineCode = flightNumber.Split(' ')[0];

                // Check if the airline exists in terminal.Airlines
                if (terminal.Airlines.ContainsKey(airlineCode))
                {
                    Airline airline = terminal.Airlines[airlineCode];
                    if (!airline.AddFlight(flight))
                    {
                        //Console.WriteLine($"Flight {flightNumber} already exists for airline {airlineCode}, skipping.");
                        return;
                    }
                }
                else
                {
                    Console.WriteLine($"Airline with code '{airlineCode}' not found, skipping flight {flightNumber}.");
                }
            }
        }

        Console.WriteLine($"{terminal.Flights.Count} Flights loaded successfully!");
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
    selectedBoardingGate.Flight = selectedFlight;
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
            selectedFlight.Status = "Scheduled";
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

        terminal.Flights[newFlight.FlightNumber] = newFlight; // add newly created flight into the terminal flight class
        terminal.GetAirlineFromFlight(newFlight).AddFlight(newFlight); // add newly created flight into the airline flight class

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
void modifyFlights() //task 8 
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
        foreach (KeyValuePair<string, Flight> lol in retrievedAirline.Flights)
        {
            Console.WriteLine($"{lol.Value.FlightNumber,-15} {code,-20} {lol.Value.Origin,-20} {lol.Value.Destination,-20} {lol.Value.ExpectedTime,-20}");
        }
    }
    else
    {
        Console.WriteLine($"Airline with code '{code}' not found.");
        return;
    }

    /////// end of task 7 

    Console.WriteLine("Choose an existing Flight to modify or delete.");
    string flightchosen = Console.ReadLine();
    Airline retrievedAirline2 = terminal.Airlines[code];
    if (!retrievedAirline2.Flights.ContainsKey(flightchosen)) //runs only if the flight is not in the airline flight dict.
    {
        Console.WriteLine("Sorry, but the flight you have chosen does not exist. Please try again.");
        return; //exits the method. 
    }
    Flight flightx = null; // establishes a flight object here first, im not sure if its the best thing to do. This will hold the flight that the user chooses. 
    foreach (KeyValuePair<string, Flight> flight in retrievedAirline2.Flights)
    {
        if (flight.Value.FlightNumber == flightchosen)
        {
            flightx = flight.Value;
        }
    }
    Console.WriteLine("1. Modify Flight");
    Console.WriteLine("2. Delete Flight");
    Console.WriteLine("Choose an option: ");
    string option = Console.ReadLine();
    if (option == "1")
    {
        Console.WriteLine("1. Modify Basic Information");
        Console.WriteLine("2. Modify Status");
        Console.WriteLine("3. Modify Special Request Code");
        Console.WriteLine("4. Modify Boarding Gate");
        string option2 = Console.ReadLine();
        if (option2 == "1")
        {
            Console.Write("Enter new Origin: ");
            string neworigin = Console.ReadLine();
            Console.Write("Enter new destination: ");
            string newdest = Console.ReadLine();
            Console.Write("Enter new Expected Departure/Arrival Time (dd/mm/yyyy hh:mm): ");
            string newtime = Console.ReadLine();
            try
            {
                DateTime.TryParseExact(newtime, "d/M/yyyy HH:mm", null, System.Globalization.DateTimeStyles.None, out DateTime validnewtime);
                flightx.Origin = neworigin;
                flightx.Destination = newdest;
                flightx.ExpectedTime = validnewtime;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Sorry, you've entered something wrong. Please try again. " + ex.Message);
            }


            Console.WriteLine("Flight updated!");
            Console.WriteLine($"Flight number: {flightx.FlightNumber}");
            Console.WriteLine($"Airline Name: {retrievedAirline2.Name}");
            Console.WriteLine($"Origin: {flightx.Origin}");
            Console.WriteLine($"Destination: {flightx.Destination}");
            Console.WriteLine($"Expected Departure/Arrival Time: {flightx.ExpectedTime:dd/MM/yyyy h:mm:ss tt}");
            Console.WriteLine($"Status: {flightx.Status ?? "Scheduled"}"); // Assuming Status is nullable, fallback to "Scheduled"
            if (flightx.GetType() == typeof(NORMFlight))
            {
                Console.WriteLine("Special Request Code: None");
            }
            else if (flightx is LWTTFlight)
            {
                Console.WriteLine("Special Request Code: LWTT");
            }
            else if (flightx is DDJBFlight)
            {
                Console.WriteLine("Special Request Code: DDJB");
            }
            else if (flightx is CFFTFlight)
            {
                Console.WriteLine("Special Request Code: CFFT");
            }
            else
            {
                Console.WriteLine("This flight has no special request code.");
            }

            BoardingGate currentGate = null; //creates a null gate to store the gate of the flight. 
            foreach (BoardingGate gate in terminal.BoardingGates.Values)
            {
                if (gate.Flight == flightx)
                {
                    currentGate = gate; //records the gate of the flight if any 
                    break;
                }
            }

            if (currentGate == null)
            {
                Console.WriteLine("Boarding Gate: Unassigned");
            }
            else
            {
                Console.WriteLine("Boarding Gate: " + currentGate.GateName);
            }



        }
        else if (option2 == "2")
        {
            Console.WriteLine("What would you like to change the flight status to?");
            string changestatus = Console.ReadLine();
            try
            {
                flightx.Status = changestatus;
                Console.WriteLine("Status updated!");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Sorry, something went wrong. Please try again." + ex.Message);
            }
        }
        else if (option2 == "3")
        {
            Console.WriteLine("What would you like to modify the special request code to?");
            string changereqcode = Console.ReadLine();
            if (changereqcode == "DDJB")
            {
                // create a new DDJBFlight object with the same properties as the original flight
                DDJBFlight newFlight = new DDJBFlight
                {
                    FlightNumber = flightx.FlightNumber,
                    Origin = flightx.Origin,
                    Destination = flightx.Destination,
                    ExpectedTime = flightx.ExpectedTime,
                    Status = flightx.Status
                };

                // replace the flight in the dictionary
                retrievedAirline2.Flights[flightx.FlightNumber] = newFlight;
                Console.WriteLine("Special Request Code successfully changed to DDJB!");
            }
            else if (changereqcode == "LWTT")
            {
                // create a new DDJBFlight object with the same properties as the original flight
                LWTTFlight newFlight = new LWTTFlight
                {
                    FlightNumber = flightx.FlightNumber,
                    Origin = flightx.Origin,
                    Destination = flightx.Destination,
                    ExpectedTime = flightx.ExpectedTime,
                    Status = flightx.Status
                };

                // replace the flight in the dictionary
                retrievedAirline2.Flights[flightx.FlightNumber] = newFlight;
                Console.WriteLine("Special Request Code successfully changed to LWTT!");
            }
            else if (changereqcode == "CFFT")
            {
                // create a new DDJBFlight object with the same properties as the original flight
                CFFTFlight newFlight = new CFFTFlight
                {
                    FlightNumber = flightx.FlightNumber,
                    Origin = flightx.Origin,
                    Destination = flightx.Destination,
                    ExpectedTime = flightx.ExpectedTime,
                    Status = flightx.Status
                };

                // replace the flight in the dictionary
                retrievedAirline2.Flights[flightx.FlightNumber] = newFlight;
                Console.WriteLine("Special Request Code successfully changed to CFFT!");
            }
            else
            {
                Console.WriteLine("Please try again, and enter a valid special request code. ");
            }
        }
        else if (option2 == "4") // option 4
        {
            Console.Write("Enter new Boarding Gate: ");
            string inputtedgate = Console.ReadLine();

            // checks if the gate entered by the user exists in terminal, if not it breaks. 
            if (!terminal.BoardingGates.ContainsKey(inputtedgate))
            {
                Console.WriteLine("Invalid boarding gate. Please enter a valid gate.");
                return;
            }

            // Retrieve the new gate
            BoardingGate newGate = terminal.BoardingGates[inputtedgate];

            // if any of the conditions are fufilled, then the boarding gate can be assigned to the flight and assignable becomes true. 
            bool assignable =
                (flightx is LWTTFlight && newGate.SupportsLWTT) ||
                (flightx is CFFTFlight && newGate.SupportsCFFT) ||
                (flightx is DDJBFlight && newGate.SupportsDDJB) ||
                (flightx is NORMFlight);

            if (!assignable)
            {
                Console.WriteLine($"Gate {newGate.GateName} does not support the flight type of Flight {flightx.FlightNumber}.");
                return;
            }

            // find the current gate of the flight if any 
            BoardingGate currentGate = null;
            foreach (BoardingGate gate in terminal.BoardingGates.Values)
            {
                if (gate.Flight == flightx)
                {
                    currentGate = gate; //records the gate of the flight if any 
                    break;
                }
            }

            // Remove the flight from its current gate
            if (currentGate != null)
            {
                currentGate.Flight = null;
                Console.WriteLine($"Flight {flightx.FlightNumber} removed from gate {currentGate.GateName}.");
            }

            // Check if the new gate is already occupied
            if (newGate.Flight != null)
            {
                Console.WriteLine($"Gate {newGate.GateName} is already occupied by Flight {newGate.Flight.FlightNumber}.");
                return;
            }

            // Assign the flight to the new gate
            newGate.Flight = flightx;
            Console.WriteLine($"Flight {flightx.FlightNumber} successfully assigned to gate {newGate.GateName}.");



        } //ending option2 = 4 brack
    } // ending option1 brack 
    else if (option == "2")
    {


        // check if flight exists in the airlines flight dict. 
        if (retrievedAirline2.Flights.ContainsKey(flightchosen))
        {
            // unassign flight from boarding gate if theres any 
            BoardingGate gateToRemove = null;

            foreach (BoardingGate gate in terminal.BoardingGates.Values)
            {
                if (gate.Flight != null && gate.Flight.FlightNumber == flightchosen)
                {
                    gateToRemove = gate; //records the gate that you want to remove. 
                    break;
                }
            }

            if (gateToRemove != null)
            {
                gateToRemove.Flight = null; // unassign the flight from the gate
                Console.WriteLine($"Flight {flightchosen} unassigned from boarding gate {gateToRemove.GateName}.");
            }

            // 3. remove the flight from the airlines flight dict and terminal flight dict
            retrievedAirline2.Flights.Remove(flightchosen);
            terminal.Flights.Remove(flightchosen);
            Console.WriteLine($"Flight {flightchosen} has been successfully deleted.");
        }
        else
        {
            Console.WriteLine($"Flight with number {flightx} not found.");
        }

    }
}
void DisplayFlightSchedule(Dictionary<string, Flight> flightDict, Dictionary<string, BoardingGate> boardingGates) //task 9
{
    // Check if there are any flights
    if (flightDict.Count == 0)
    {
        Console.WriteLine("There are no available flights.");
        return;
    }

    // Sort flights by ExpectedTime
    List<Flight> sortedFlights = new List<Flight>(flightDict.Values);
    sortedFlights.Sort();
    string airlineName = "";

    // Display header
    Console.WriteLine("=============================================");
    Console.WriteLine("Flight Schedule for Changi Airport Terminal 5");
    Console.WriteLine("=============================================");
    Console.WriteLine($"{"Flight Number",-18}{"Airline Name",-20}{"Origin",-25}{"Destination",-25}{"Expected Departure/Arrival Time",-40}{"Status",-20}{"Boarding Gate",-20}");
    Console.WriteLine(new string('-', 100));

    // Display each flight in sorted order
    foreach (Flight flight in sortedFlights)
    {
        Airline airline = terminal.GetAirlineFromFlight(flight);
        if (airline != null)
        {
            airlineName = airline.Name;
        }
        else
        {
            airlineName = "unavailable";
        }
        string gateName = "Unassigned";
        foreach (BoardingGate gate in boardingGates.Values)
        {
            if (gate.Flight == flight)
            {
                gateName = gate.GateName;
                break;
            }
        }

        Console.WriteLine($"{flight.FlightNumber,-18}{airlineName,-20}{flight.Origin,-25}{flight.Destination,-25}{flight.ExpectedTime,-40}{flight.Status,-20}{gateName,-10}");
    }
}


void processUnassignedFlights() // advanced feature A Ryan Tan 
{
    int initialAssignedFlights = 0;
    int initialUnassignedFlights = 0;
    int totalFlights = terminal.Flights.Count;
    terminal.UnassignedGates.Clear(); // Reset the list before processing flights again, to avoid multiple calls. 

    foreach (Airline airline in terminal.Airlines.Values)
    {
        foreach (Flight flight in airline.Flights.Values)
        {
            BoardingGate assignedGate = terminal.GetAssignedGate(flight);
            if (assignedGate != null) //if a gate is found, count it as an assigned flight.
            {
                initialAssignedFlights += 1;
            }
            else //else,count it as unassigned and add it to the terminal queue. 
            {
                initialUnassignedFlights += 1;
                terminal.UnassignedFlights.Enqueue(flight);
            }
        }//ending flight loop brack
    }// ending airline loop brack 

    int unassignedGates = 0;
    int totalGates = terminal.BoardingGates.Count;
    int assignedGates = 0;

    foreach (BoardingGate gate in terminal.BoardingGates.Values)
    {
        if (gate.Flight == null) // if gate has no flight assigned, count it as unassigned, assign it to the terminal list. 
        {
            unassignedGates += 1;
            terminal.UnassignedGates.Add(gate);
        }
        else // if gate has a flight assigned
        {
            assignedGates += 1;
        }
    }//ending gate loop brack 



    Console.WriteLine("Unassigned: " + initialUnassignedFlights); // initally, total unassigned flights = 30, which should be the case. 
    Console.WriteLine("Assigned: " + initialAssignedFlights);
    Console.WriteLine("Total flights: " + totalFlights);
    //Console.WriteLine(terminal.UnassignedFlights.Count);
    Console.WriteLine($"Unassigned Gates: {unassignedGates}");
    Console.WriteLine($"Total Gates: {totalGates}");
    //foreach (Flight f in terminal.UnassignedFlights) //delete this ltr
    //{
    //    Console.WriteLine(f);
    //}

    //Console.WriteLine("");
    //Console.WriteLine("GATE ASSIGNMENT START");
    //Console.WriteLine("");

    int flightsAndGatesAssigned = 0;
    while (terminal.UnassignedFlights.Count != 0)
    {
        Flight flightx = terminal.UnassignedFlights.Dequeue(); //stores the first flight in the queue as flightx
        string flightType = "";
        if (flightx is LWTTFlight)// assigns the flight type 
        {
            flightType = "LWTT";
        }
        else if (flightx is CFFTFlight)
        {
            flightType = "CFFT";
        }
        else if (flightx is DDJBFlight)
        {
            flightType = "DDJB";
        }
        else if (flightx is NORMFlight)
        {
            flightType = "Norm";
        }

        foreach (BoardingGate gate in new List<BoardingGate>(terminal.UnassignedGates)) //creates a copy of the terminal.unassignedgates to prevent index shifting
        {
            if (gate.SupportsDDJB && flightType == "DDJB" ||
                gate.SupportsLWTT && flightType == "LWTT" ||
                gate.SupportsCFFT && flightType == "CFFT" ||
                flightType == "Norm")
            {
                //Console.WriteLine($"Assigned flight {flightx.FlightNumber} to gate {gate.GateName}");
                flightsAndGatesAssigned += 1;
                gate.Flight = flightx; // Assign the flight to the gate
                terminal.UnassignedGates.Remove(gate); // Remove the gate from unassigned gates
                //Console.WriteLine($"Gate {gate.GateName} removed from unassigned gates. Remaining gates: {terminal.UnassignedGates.Count}");
                //if (terminal.UnassignedGates.Contains(gate))
                //{
                //    //Console.WriteLine($"Error: Gate {gate.GateName} not in unassigned gates but removed.");
                //} // debug if statement check 
                break; // Exit the loop after assigning the flight

            }
        }


    }// ending while brack 

    Console.WriteLine("Total assigned flights and gates:" + flightsAndGatesAssigned);
    Console.WriteLine($"Final count of unassigned gates: {terminal.UnassignedGates.Count}");
    Console.WriteLine($"Final count of unassigned flights: {terminal.UnassignedFlights.Count}");
    // Calculate percentages
    int automaticallyProcessedFlights = flightsAndGatesAssigned; // Automatically assigned flights
    int alreadyAssignedFlights = initialAssignedFlights; // Initially assigned flights

    int automaticallyProcessedGates = flightsAndGatesAssigned; // Automatically assigned gates
    int alreadyAssignedGates = totalGates - unassignedGates; // Initially assigned gates

    double percentageFlightsAutomaticallyProcessed = ((double)automaticallyProcessedFlights / totalFlights) * 100;
    double percentageGatesAutomaticallyProcessed = ((double)automaticallyProcessedGates / totalGates) * 100;

    // Display results
    Console.WriteLine($"Percentage of flights processed automatically: {percentageFlightsAutomaticallyProcessed:F2}%");
    Console.WriteLine($"Percentage of gates processed automatically: {percentageGatesAutomaticallyProcessed:F2}%");

    //	display the total number of Flights and Boarding Gates that were processed automatically over those that were already assigned as a percentage
    //total gates - unassigned gates will give u assigned gates. 
    Console.WriteLine();
    Console.WriteLine();
    Console.WriteLine("Flight details:");
    terminal.DisplayFlights();

}// ending method brack 

void CalculateTotalFees(Dictionary<string, Flight> flights) // Advanced feature B Toh Keng Siong
{
    Console.WriteLine("=============================================");
    Console.WriteLine("Total Fees Per Airline for Changi Airport Terminal 5");
    Console.WriteLine("=============================================");
    Dictionary<string, double> airlineFees = new Dictionary<string, double>();
    Dictionary<string, int> airlineFlightCount = new Dictionary<string, int>();
    double totalDiscount = 0;

    foreach (Flight flight in flights.Values)
    {
        BoardingGate assignedGate = terminal.GetAssignedGate(flight); //checking if all flights has been assigned a boardingGates
        if (assignedGate == null)
        {
            Console.WriteLine($"Flight {flight.FlightNumber} has no assigned boarding gate. Assign all gates before calculating fees.");
            return;
        }

        Airline airlineObj = terminal.GetAirlineFromFlight(flight);
        if (airlineObj == null)
        {
            Console.WriteLine($"Error: Unable to find airline for flight {flight.FlightNumber}.");
            continue;
        }
    }
    terminal.PrintAirlineFees();
}

Loadfiles();
LoadFlights("flights.csv");
while (true)
{
    Console.WriteLine("=============================================\r\nWelcome to Changi Airport Terminal 5\r\n=============================================\r\n");
    Console.WriteLine("1. List All Flights");
    Console.WriteLine("2. List Boarding Gates");
    Console.WriteLine("3. Assign a Boarding Gate to a Flight");
    Console.WriteLine("4. Create Flight");
    Console.WriteLine("5. Display Airline Flights");
    Console.WriteLine("6. Modify Flight Details");
    Console.WriteLine("7. Display Flight Schedule");
    Console.WriteLine("8. Display Calculated fees");
    Console.WriteLine("9: Assign boarding gates in bulk");
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
            modifyFlights();
        }
        else if (choice == "7")
        {
            DisplayFlightSchedule(terminal.Flights, terminal.BoardingGates);
        }
        else if (choice == "8")
        {
            CalculateTotalFees(terminal.Flights);
        }
        else if (choice == "9")
        {
            processUnassignedFlights();
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
