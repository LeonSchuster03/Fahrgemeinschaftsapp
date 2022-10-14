using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Fahrgemeinschaftsapp
{
    public class Carpool
    {
        public int Id { get; set; }
        public int PassengerCount { get; set; }
        public string Destination { get; set; }
        public string StartLocation { get; set; }
        public DateTime Departure { get; set; }
        public string Password { get; set; }

        public Carpool(int id, int passengercount, string destination,string startlocation, DateTime departure, string password)
        {
            Id = id;
            PassengerCount = passengercount;
            Destination = destination;
            StartLocation = startlocation;
            Departure = departure;
            Password = password;
        }
        
        /// <summary>
        /// the user can create a carpool and add other users
        /// </summary>
        /// <param name="carpool"></param>
        /// <param name="userName"></param>
        public static void CreateCarpool(List<Carpool> carpool, string userName)
        {
            DirectoryInfo di = new DirectoryInfo($@"C:\010Projects\019 Fahrgemeinschaft\Fahrgemeinschaftsapp\Carpools");
            //Collecting the neccessary info
            Console.Clear();
            int passengerCount = InputCheck.NumberCheck("How many seats does the car have?");
            Console.WriteLine(" ");
            string destination = InputCheck.StringCheck("What is the destination?");
            Console.WriteLine(" ");
            string startLoc = InputCheck.StringCheck("Where do you plan to start?");
            Console.WriteLine("");
            Console.WriteLine("When do you plan to start?");
            DateTime departure = InputCheck.ValidTimeCheck("When do you plan to start?");
            int carpoolID = di.GetFiles().Length;
            string departureString = departure.ToShortTimeString();
            do
            {
                FileInfo fi = new FileInfo($@"C:\010Projects\019 Fahrgemeinschaft\Fahrgemeinschaftsapp\Carpools\\\\/{carpoolID}.csv\");
                if (fi.Exists)
                {
                    carpoolID++;
                }
                else
                {
                    break;
                }
            } while (true);

            File.Create(Convert.ToString(carpoolID));
            List<string> passengers = new List<string>();
            for (int j = 1; j < passengerCount; j++)
            {
                Console.WriteLine("");
                Console.WriteLine($"Please give us the username of the {j}. passenger");
                string dude = Console.ReadLine();
                FileInfo f1 = new FileInfo($@"C:\010Projects\019 Fahrgemeinschaft\Fahrgemeinschaftsapp\Userlist\\\\/{dude}.csv\");
                if (!f1.Exists)
                {
                    Console.WriteLine("");
                    Console.WriteLine("This user doesnt exist. Please choose another one");
                    j--;
                }
                else if (dude == userName)
                {
                    Console.WriteLine(" ");
                    Console.WriteLine("You cant add yourself to the carpool. Choose another one");
                    j--;
                }
                else
                {
                    passengers.Add(dude);
                }
            }
            passengers.Add(userName);
            Console.WriteLine("Please enter a password for the carpool");
            string carpoolPassword = Program.HashPassword(Program.HidePassword());
            carpool.Add(new Carpool(carpoolID, passengerCount, destination, startLoc, departure, carpoolPassword));
            using (StreamWriter writer = new StreamWriter($@"C:\010Projects\019 Fahrgemeinschaft\Fahrgemeinschaftsapp\Carpools\{Convert.ToString(carpoolID)}.csv"))
            {
                var newLine = $"{carpoolID};{startLoc};{destination};{departureString};{carpoolPassword};{passengerCount}";

                foreach (string passenger in passengers)
                {
                    newLine += $";,{passenger},";
                }
                writer.Write(newLine);
            }
            Console.Clear();
            Console.WriteLine($"A carpool with the ID {carpoolID} got created!");
            Console.WriteLine($"The passengers are:");
            foreach (string passenger in passengers)
            {
                Console.WriteLine(passenger);
            }
            Console.WriteLine($"You will start at {departureString} in {startLoc}");
            Console.WriteLine(" ");
            Console.WriteLine("[1] View your carpools");
            Console.WriteLine("[2] Go back to menu");
            string input = Console.ReadLine();
            if (input == "1")
            {
                ViewYourCarpools(carpool, userName);
            }
        }

        /// <summary>
        /// the user gets to see all carpools, where he/she is a member of
        /// </summary>
        /// <param name="carpool"></param>
        /// <param name="userName"></param>
        public static void ViewYourCarpools(List<Carpool> carpool, string userName)
        {
            Console.Clear();
            Console.WriteLine("Here you can see all the carpools, you are a part of:");
            Console.WriteLine("------------------------------------------------- ");
            Console.WriteLine(" ");

            DirectoryInfo di = new DirectoryInfo($@"C:\010Projects\019 Fahrgemeinschaft\Fahrgemeinschaftsapp\Carpools\");
            FileInfo[] Files = di.GetFiles();
            foreach (FileInfo file in Files)
            {
                string content = File.ReadAllText($@"C:\010Projects\019 Fahrgemeinschaft\Fahrgemeinschaftsapp\Carpools\{file}");
                if (content.Contains($",{userName},"))
                {
                    string[] values = content.Split(';');
                    using (StreamReader sr = new StreamReader($@"C:\010Projects\019 Fahrgemeinschaft\Fahrgemeinschaftsapp\Carpools\{file}"))
                    {
                        Console.WriteLine($"The Carpool with the ID {values[0].Trim(',')} will start at {values[3]} in {values[1]}");
                        Console.WriteLine($"It's destination is {values[2]}");
                        Console.WriteLine("The passengers are:");
                        for (int i = 6; i < 6 + Convert.ToInt32(values[5]); i++)
                        {
                            if (values[i].Equals($",{userName},"))
                            {
                                values[i] = "You";;
                            }
                            Console.WriteLine($"{values[i].Replace("\r\n", string.Empty).Trim(',')}");
                        }
                    }
                }
                Console.WriteLine("");
            }

            Console.WriteLine("------------------------------------------------- ");
            Console.WriteLine("[1] Create a carpool");
            Console.WriteLine("[2] Join a carpool");
            Console.WriteLine("[3] Leave a carpool");
            Console.WriteLine("[4] Go back to menu");
            string input = Console.ReadLine();
            switch (input)
            {
                case "1":
                    CreateCarpool(carpool, userName);
                    break;
                case "2":
                    JoinCarpool(carpool, userName);
                    break;
                case "3":
                    LeaveCarpool(carpool, userName);
                    break;
                default:
                    break;                       
            }
        }

        /// <summary>
        /// the user can enter a id of a carpool and join it with the correct password
        /// </summary>
        /// <param name="carpool"></param>
        /// <param name="userName"></param>
        public static void JoinCarpool(List<Carpool> carpool, string userName)
        {
            Console.Clear();
            Console.WriteLine("Enter the ID of the Carpool you want to join");
            string fileName = Console.ReadLine();
            Console.WriteLine("Please enter the password of this carpool");
            string carpoolPassword = Program.HidePassword();
            FileInfo fi = new FileInfo($@"C:\010Projects\019 Fahrgemeinschaft\Fahrgemeinschaftsapp\Carpools\{fileName}.csv");
            if (fi.Exists)
            {
                if (ValidatePassword(fileName, carpoolPassword))
                {
                    string[] values = FileHandling.Read($@"C:\010Projects\019 Fahrgemeinschaft\Fahrgemeinschaftsapp\Carpools\{fileName}.csv");
                    List<string> passengers = new List<string>();
                    for (int i = 6; i < values.Length; i++)
                    {
                        if (values[i] != "")
                        {
                            passengers.Add(values[i].Replace("\r\n", string.Empty));
                        }
                    }
                    if (values.Length - 6 < Convert.ToInt32(values[5]))
                    {
                        using (StreamWriter writer = new StreamWriter($@"C:\010Projects\019 Fahrgemeinschaft\Fahrgemeinschaftsapp\Carpools\{fileName}.csv"))
                        {
                            var newLine = $"{values[0]};{values[1]};{values[2]};{values[3]};{values[4]};{values[5]}";
                            foreach (string passenger in passengers)
                            {
                                newLine += $";{passenger}";
                            }
                            newLine += $";,{userName},";

                            writer.Write(newLine.Replace("\r\n", string.Empty));
                        }
                        Console.Clear();
                        Console.WriteLine($"You successfully joined the carpool with the ID {values[0]}");
                        Console.WriteLine(" ");
                    }
                    else
                    {
                        Console.WriteLine($"You cant join the carpool with the ID {values[0]}, there are no free seats");
                        Console.WriteLine(" ");
                    }
                }
                else
                {
                    Console.WriteLine("The password is wrong.");
                    Console.WriteLine("Redirecting to menu ...");
                    Thread.Sleep(1000);
                }
            }
            else
            {
                Console.Clear();
                Console.WriteLine($"There is no carpool with the ID {fileName}");
                Console.WriteLine(" ");
            }
            Console.WriteLine("------------------------------------------------- ");
            Console.WriteLine("[1] Go back");
            Console.WriteLine("[2] Go to menu");
            string input = Console.ReadLine();
            if (input == "1")
            {
                ViewYourCarpools(carpool, userName);
            }
        }

        /// <summary>
        /// the user can choose, which carpool he/she wants to leave
        /// </summary>
        /// <param name="carpool"></param>
        /// <param name="userName"></param>
        public static void LeaveCarpool(List<Carpool> carpool, string userName)
        {
            Console.Clear();
            Console.WriteLine("Enter the ID of the Carpool you want to leave");
            string fileName = Console.ReadLine();
            Console.WriteLine("Please enter the carpool password");
            string userPw = Program.HidePassword();

            FileInfo fi = new FileInfo($@"C:\010Projects\019 Fahrgemeinschaft\Fahrgemeinschaftsapp\Carpools\{fileName}.csv");
            if (fi.Exists)
            {
                if (ValidatePassword(fileName, userPw))
                {
                    string text = File.ReadAllText($@"C:\010Projects\019 Fahrgemeinschaft\Fahrgemeinschaftsapp\Carpools\{fileName}.csv");
                    if (text.Contains($",{userName},"))
                    {
                        string test = File.ReadAllText($@"C:\010Projects\019 Fahrgemeinschaft\Fahrgemeinschaftsapp\Carpools\{fileName}.csv");
                        string[] values = text.Split(';');
                        var newLine = string.Empty;

                        foreach (string value in values)
                        {
                            if (value.Replace("\r\n", string.Empty) != $",{userName},")
                            {
                                if (value != string.Empty)
                                {
                                    newLine += $"{value};";
                                }
                            }
                        }
                        newLine = newLine.TrimEnd(';');
                        using (StreamWriter writer = new StreamWriter($@"C:\010Projects\019 Fahrgemeinschaft\Fahrgemeinschaftsapp\Carpools\{fileName}.csv"))
                        {
                            writer.Write(newLine);
                        }
                        Console.Clear();
                        Console.WriteLine($"You got successfully removed from the carpool with the ID {fileName}");
                    }
                    else
                    {
                        Console.Clear();
                        Console.WriteLine("You're not a part of this carpool, therefore you can't leave it");
                    }
                    string content = File.ReadAllText($@"C:\010Projects\019 Fahrgemeinschaft\Fahrgemeinschaftsapp\Carpools\{fileName}.csv");
                    string[] csv = content.Split(';');
                    if (csv.Length <= 6)
                    {
                        File.Delete($@"C:\010Projects\019 Fahrgemeinschaft\Fahrgemeinschaftsapp\Carpools\{fileName}.csv");
                    }
                }
                else
                {
                    Console.WriteLine("The password is incorrect");
                    Console.WriteLine("");
                }
            }
            else
            {
                Console.Clear();
                Console.WriteLine("This carpool does not exist");
            }
            Console.WriteLine("------------------------------------------------- ");
            Console.WriteLine("[1] Go back");
            Console.WriteLine("[2] Go to menu");
            string input = Console.ReadLine();
            if (input == "1")
            {
                ViewYourCarpools(carpool, userName);
            }
        }

        /// <summary>
        /// by entering the ID, the user can delete an existing carpool
        /// </summary>
        /// <param name="userName"></param>
        public static void DeleteCarpool(string userName)
        {
            Console.Clear();
            Console.WriteLine("Please enter the ID of the carpool you want to delete");
            string cpId = Console.ReadLine();
            FileInfo fi = new FileInfo($@"C:\010Projects\019 Fahrgemeinschaft\Fahrgemeinschaftsapp\Carpools\{cpId}.csv");
            string content = string.Empty;
            if (fi.Exists)
            {
                using (StreamReader reader = new StreamReader($@"C:\010Projects\019 Fahrgemeinschaft\Fahrgemeinschaftsapp\Carpools\{cpId}.csv"))
                {
                    content = reader.ReadToEnd();

                }
                if (content.Contains($",{userName},"))
                {
                    Console.WriteLine("Do you really want to delete this carpool? (y/n)");
                    string confirmation = Console.ReadLine();
                    if (confirmation == "y")
                    {
                        Console.Clear();
                        File.Delete($@"C:\010Projects\019 Fahrgemeinschaft\Fahrgemeinschaftsapp\Carpools\{cpId}.csv");
                        Console.WriteLine($"The carpool with the ID {cpId} got deleted");
                        Thread.Sleep(1500);
                        Console.WriteLine("Redirecting to menu ...");
                        Thread.Sleep(1000);
                    }
                    else
                    {
                        Console.WriteLine("No carpool got deleted");
                        Console.WriteLine(" ");
                        Console.WriteLine("Redirecting to menu ...");
                        Thread.Sleep(2000);
                    }
                }
                else
                {
                    Console.WriteLine($"You're not a part of the carpool with the ID {cpId}");
                    Console.WriteLine(" ");
                    Console.WriteLine("Redirecting to menu ...");
                    Thread.Sleep(2000);
                }
            }
            else
            {
                Console.WriteLine($"A carpool with the ID {cpId} does not exist.");
                Console.WriteLine(" ");
                Console.WriteLine("Redirecting to menu ...");
                Thread.Sleep(2000);
            }
        }

        /// <summary>
        /// checks if user input equals the password of th carpool
        /// </summary>
        /// <param name="cpID"></param>
        /// <param name="inputPw"></param>
        /// <returns></returns>
        public static bool ValidatePassword(string cpID, string inputPw)
        {
            string tmpNewHash = Program.HashPassword(inputPw);
            bool bEqual = false;
            string[] values = FileHandling.Read($@"C:\010Projects\019 Fahrgemeinschaft\Fahrgemeinschaftsapp\Carpools\{cpID}.csv");
            string tmpHash = values[4].Replace("\r\n", string.Empty);
            if (tmpNewHash.Length == tmpHash.Length)
            {
                int i = 0;
                while ((i < tmpNewHash.Length) && (tmpNewHash[i] == tmpHash[i]))
                {
                    i += 1;
                }
                if (i == tmpNewHash.Length)
                {
                    bEqual = true;
                }
            }
            return bEqual;
        }

    }
}
