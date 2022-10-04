using DocumentFormat.OpenXml.Math;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.SqlServer.Server;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Xml.Serialization;
using static System.Net.WebRequestMethods;
using File = System.IO.File;

namespace Fahrgemeinschaftsapp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "Carpool App by Leon";
            List <User> user_list = new List<User>();
            List<Driver> driver_list = new List<Driver>();
            List<Carpool> carpool_list = new List<Carpool>();
            while (true)
            {
                Console.WriteLine("Welcome to the carpool app"); 
                Console.WriteLine(" ");
                string username = LogIn(user_list, driver_list); //TODO implement password into login-process
                bool loggedin = true;

                do
                {
                    switch (Menu(username))
                    {
                        case "1":
                            PrintUserInfo(username); //TODO make returned list look cleaner + make Info editable
                            break;
                        case "2":
                            
                            PrintAllUsers(); //TODO make returned list look cleaner
                            break; 
                        case "3":                           
                            PrintAllDrivers(); //TODO make returned list look cleaner
                            break; 
                            
                        case "4":
                            CreateCarpool(carpool_list, username);
                            break;
                        case "5":
                            ViewYourCarpools(username); //TODO make returned list look cleaner

                            break;
                        case "6":
                            DeleteCarpool(username);
                            break;
                        case "7":
                            loggedin = DeleteAccount(username);
                            break;
                        case "8":
                            loggedin = false;
                            break;
                    }
                } while (loggedin == true);  
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Logging out ...");
                Console.ForegroundColor = ConsoleColor.White;
                Thread.Sleep(1000);
                Console.Clear();
            }
        }       
        public static string LogIn(List<User> user_list, List<Driver> driver_list)
        {
        Home:
            Console.WriteLine("Do you already have an account? (y/n)");
            string AccountYN = Console.ReadLine();
            string username = "";
            Console.Clear();
            if (AccountYN == "y")
            {
                Console.WriteLine("Please enter your username");
                username = Console.ReadLine();
                FileInfo fi = new FileInfo($@"C:\010Projects\019 Fahrgemeinschaft\Fahrgemeinschaftsapp\Userlist\\\\/{username}.csv\");
                if (fi.Exists)
                {
                    Console.WriteLine("klappt");
                }
                else
                {
                    Console.WriteLine("This user doesnt exist. You get redirected to Registration");
                    Thread.Sleep(1500);
                    Console.Clear();
                    Registration(user_list, driver_list);
                }
            }
            else if(AccountYN == "n")
            {
                Registration(user_list, driver_list);
            }
            else
            {
                Console.WriteLine("Your input wasnt clear, please try again");
                Thread.Sleep(1250);
                Console.Clear();
                goto Home;
            }
            return username;            
        }
        public static void Registration(List<User> user_list, List<Driver> driver_list)  //Creating .csv File and setting username and password 
        {
            Console.WriteLine("What should be your username?");
            string username = Console.ReadLine();
            do
            {
                FileInfo fi = new FileInfo($@"C:\010Projects\019 Fahrgemeinschaft\Fahrgemeinschaftsapp\Userlist\\\\/{username}.csv\");
                if (!fi.Exists)
                {
                    using (File.Create(username))
                        Console.WriteLine($"Your username is now {username}");
                    break;
                }
                else
                {
                    Console.WriteLine("This username already exists, please choose another");                   
                    username = Console.ReadLine();
                }
            } while (true);
            CollectUserInfo(username, user_list, driver_list);
        }
        public static void CollectUserInfo(string username, List<User> user_list, List<Driver> driver_list) //Adding info to userspecific file
        {
            List<string> userinfo = new List<string>();
            List<string> driverinfo = new List<string>();

            Console.WriteLine("Whats your first name?");
            string firstname = Console.ReadLine();
            Console.WriteLine("Whats your last name?");
            string lastname = Console.ReadLine();
            Console.WriteLine("Please enter your age");
            int age = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("Whats your gender (m/f/d)");
            string gender = Console.ReadLine();
            Console.WriteLine("Where is your starting location");
            string startlocation = Console.ReadLine();
            Console.WriteLine("Whats your destination?");
            string destination = Console.ReadLine();
            Console.WriteLine("Do you have a car/Are you able to drive? (y/n)");
            string hascarstring = Console.ReadLine();

            userinfo.Add(firstname);
            userinfo.Add(lastname);
            userinfo.Add(Convert.ToString(age));
            userinfo.Add(gender);
            userinfo.Add  (startlocation); 
            userinfo.Add(destination);          
            using (StreamWriter writer = new StreamWriter($@"C:\010Projects\019 Fahrgemeinschaft\Fahrgemeinschaftsapp\Userlist\{username}.csv"))
            {
                for (int i = 0; i < userinfo.Count; i++)
                {
                    writer.Write(string.Join(";", userinfo[i]));
                    writer.Write(",");
                }
            }
            bool hascar = false;
            if (hascarstring == "y")
            {
                hascar = true;
                Console.WriteLine("How many seats does your car have?");
                string seatst = Console.ReadLine();
                Console.WriteLine("How many seats are free?");
                string seatsf = Console.ReadLine();
                Console.WriteLine("What time do you want to start?");
                DateTime starttime = Convert.ToDateTime(Console.ReadLine());

                driverinfo.Add(seatst);
                driverinfo.Add(seatsf);
                driverinfo.Add(starttime.ToShortTimeString());

                var temp = username + ".driver";
                driver_list.Add(new Driver(temp, firstname, lastname, age, gender, startlocation, destination, hascar, Convert.ToInt32(seatst), Convert.ToInt32(seatsf), starttime));
 
                using (StreamWriter writer = new StreamWriter($@"C:\010Projects\019 Fahrgemeinschaft\Fahrgemeinschaftsapp\Driverlist\{username}.csv"))
                {
                    for (int i = 0; i < driverinfo.Count; i++)
                    {
                        writer.Write(string.Join(";", driverinfo[i]));
                        writer.Write(",");
                    }
                }
            }            
            user_list.Add(new User(username, firstname, lastname, age, gender, startlocation, destination, hascar));

            Console.WriteLine(" ");  
            Console.WriteLine("Saving the information in our database ...");
            Thread.Sleep(500);
            Console.Clear();
            Console.WriteLine("Redirecting to Menu ...");
            Thread.Sleep(500);            
        }
        public static string Menu(string username)
        {
            Console.Clear();
            Console.WriteLine($"Welcome {username},");
            Console.WriteLine("--------------------------------------------------- ");
            Console.WriteLine("Please choose, what you want to do:");
            Console.WriteLine("--------------------------------------------------- ");
            Console.WriteLine("[1] List information about you");
            Console.WriteLine("[2] Look for possible passengers");
            Console.WriteLine("[3] Look for possible drivers");
            Console.WriteLine("--------------------------------------------------- ");
            Console.WriteLine("[4] Create a carpool");
            Console.WriteLine("[5] View your carpools");
            Console.WriteLine("[6] Delete a carpool");
            Console.WriteLine("--------------------------------------------------- ");
            Console.WriteLine("[7] Delete your account");
            Console.WriteLine("[8] Log out");

            string navigatemenu = Console.ReadLine();
            return navigatemenu;
        }
        public static void PrintAllUsers()
        {
            Console.Clear();
            Console.WriteLine("Here are all users, who are currently looking for a carpool");
            Console.WriteLine("------------------------------------------------- ");
            foreach (string file in Directory.EnumerateFiles($@"C:\010Projects\019 Fahrgemeinschaft\Fahrgemeinschaftsapp\Userlist\\\\"))
            {
                using (StreamReader sr = new StreamReader(file))
                    Console.WriteLine(sr.ReadLine());
            }
            Console.WriteLine("------------------------------------------------- ");
            Console.WriteLine("[1] Go back to menu");
            Console.ReadLine();
        }
        public static void PrintAllDrivers()
        {
            Console.Clear();  
            Console.WriteLine("Here are all users, who are currently looking for a carpool and are able to drive");
            Console.WriteLine("------------------------------------------------- ");
            foreach (string file in Directory.EnumerateFiles($@"C:\010Projects\019 Fahrgemeinschaft\Fahrgemeinschaftsapp\Driverlist\\\\"))
            {
                string userfilename = Path.GetFileName(file);
                using (StreamReader sr = new StreamReader(Path.Combine($@"C:\010Projects\019 Fahrgemeinschaft\Fahrgemeinschaftsapp\Userlist\\\\", userfilename)))
                    Console.WriteLine(sr.ReadLine());
                using (StreamReader sr2 = new StreamReader(file))
                    Console.WriteLine(sr2.ReadLine());
                Console.WriteLine(" ");
            }
            Console.WriteLine("------------------------------------------------- ");
            Console.WriteLine("[1] Go back to menu");
            Console.ReadLine();
        }
        public static void PrintUserInfo(string username)
        {
            Console.Clear();
            Console.WriteLine("Here are all the information we saved about you:");
            Console.WriteLine("------------------------------------------------- ");

            FileInfo fi = new FileInfo($@"C:\010Projects\019 Fahrgemeinschaft\Fahrgemeinschaftsapp\Driverlist\\\\/{username}.csv\");
            
            using (StreamReader sr = new StreamReader($@"C:\010Projects\019 Fahrgemeinschaft\Fahrgemeinschaftsapp\Userlist\{username}.csv"))
                Console.WriteLine(sr.ReadLine());
           
            if (fi.Exists)
            {
                using (StreamReader sr = new StreamReader($@"C:\010Projects\019 Fahrgemeinschaft\Fahrgemeinschaftsapp\Driverlist\{username}.csv"))
                    Console.WriteLine(sr.ReadLine());
            }
            Console.WriteLine("------------------------------------------------- ");
            Console.WriteLine("[1] Go back to menu");
            Console.ReadLine();           
        }
        public static void CreateCarpool(List<Carpool> carpool, string username)
        {
            DirectoryInfo di = new DirectoryInfo($@"C:\010Projects\019 Fahrgemeinschaft\Fahrgemeinschaftsapp\Carpools");

            Console.Clear();
            Console.WriteLine("How many passengers fit in the car (incl. driver)?");
            int passengercount = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine(" ");
            Console.WriteLine("Whats the destination?");
            string destination = Console.ReadLine();
            Console.WriteLine(" ");
            Console.WriteLine("Where do you plan to start?");
            string startloc = Console.ReadLine();
            Console.WriteLine("");
            Console.WriteLine("When do you plan to start?");
            DateTime departure = Convert.ToDateTime(Console.ReadLine());
            int carpoolID = di.GetFiles().Length;
            string s_departure = departure.ToShortTimeString();
            carpool.Add(new Carpool(carpoolID, passengercount, destination,startloc, departure));
            
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
            for(int j = 1; j < passengercount; j++) 
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
                else
                {
                    passengers.Add(dude);
                }
            }
            passengers.Add(username);
            using (StreamWriter writer = new StreamWriter($@"C:\010Projects\019 Fahrgemeinschaft\Fahrgemeinschaftsapp\Carpools\{Convert.ToString(carpoolID)}.csv"))
            {
                
                foreach(string passenger in passengers)
                {
                    writer.Write(passenger);
                    writer.Write(",");
                }
                writer.Write($"{carpoolID},{startloc},{destination},{s_departure}");
            }
            Console.Clear();
            Console.WriteLine($"A carpool with the ID {carpoolID} got created!");
            Console.WriteLine($"The passengers are:");
            foreach(string passenger in passengers)
            {
                Console.WriteLine(passenger);
            }
            Console.WriteLine($"You will start at {s_departure} in {startloc}");
            Console.WriteLine(" ");
            Console.WriteLine("[1] Go back to menu");
            Console.ReadLine();
        }
        public static void ViewYourCarpools(string username)
        {
            Console.Clear();
            Console.WriteLine("Here you can see all the carpools, you are a part of:");
            Console.WriteLine("------------------------------------------------- ");
            Console.WriteLine(" ");

            DirectoryInfo di = new DirectoryInfo($@"C:\010Projects\019 Fahrgemeinschaft\Fahrgemeinschaftsapp\Carpools\");
            FileInfo[] Files = di.GetFiles();

            foreach (FileInfo file in Files)
            {
                string Content = string.Empty;
                using (StreamReader reader = new StreamReader($@"C:\010Projects\019 Fahrgemeinschaft\Fahrgemeinschaftsapp\Carpools\{file}"))
                {
                    Content = reader.ReadToEnd();
                }

                    if (Content.Contains($",{username},"))
                    {
                        using (StreamReader sr = new StreamReader($@"C:\010Projects\019 Fahrgemeinschaft\Fahrgemeinschaftsapp\Carpools\{file}"))
                        {
                            Console.WriteLine(sr.ReadLine());
                            Console.WriteLine(" ");
                        }
                    }                
            }          
            Console.WriteLine("------------------------------------------------- ");
            Console.WriteLine("[1] Go back to menu");
            Console.ReadLine();
        }
        public static void DeleteCarpool(string username)
        {
            Console.Clear();
            Console.WriteLine("Please enter the ID of the carpool you want to delete");
            string cp_id = Console.ReadLine();
            FileInfo fi = new FileInfo($@"C:\010Projects\019 Fahrgemeinschaft\Fahrgemeinschaftsapp\Carpools\{cp_id}.csv");
            string Content = string.Empty;
            if (fi.Exists)
            {
                using (StreamReader reader = new StreamReader($@"C:\010Projects\019 Fahrgemeinschaft\Fahrgemeinschaftsapp\Carpools\{cp_id}.csv"))
                {
                    Content = reader.ReadToEnd();
                }

                if (Content.Contains($",{username},"))
                {
                    Console.WriteLine("Do you really want to delete this carpool? (y/n)");
                    string confirmation = Console.ReadLine();
                    if (confirmation == "y")
                    {
                        Console.Clear();
                        File.Delete($@"C:\010Projects\019 Fahrgemeinschaft\Fahrgemeinschaftsapp\Carpools\{cp_id}.csv");
                        Console.WriteLine($"The carpool with the ID {cp_id} got deleted");
                        Thread.Sleep(1500);
                        Console.WriteLine("Redirecting to menu ...");
                        Thread.Sleep(1000);
                    }
                    else
                    {
                        Console.WriteLine("No carpool got deleted");
                        Console.WriteLine("Redirecting to menu ...");
                        Thread.Sleep(2000);
                    }
                }
                else
                {
                    Console.WriteLine($"You're not a part of the carpool with the ID {cp_id}");
                    Console.WriteLine("Redirecting to menu ...");
                    Thread.Sleep(2000);
                }
            }
            else
            {
                Console.WriteLine($"A carpool with the ID {cp_id} does not exist.");
                Console.WriteLine("Redirecting to menu ...");
                Thread.Sleep(2000);
            }
        }
        public static bool DeleteAccount(string username)
        {
            bool loggenin = true;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Clear();
            Console.WriteLine("Do you really want to delete your account? (y/n)");
            Console.WriteLine("This action is irreversible and all carpool you're a part of will get deleted");
            string confirmation = Console.ReadLine();
            if(confirmation == "y")
            {
                File.Delete($@"C:\010Projects\019 Fahrgemeinschaft\Fahrgemeinschaftsapp\Userlist\{username}.csv");
                File.Delete($@"C:\010Projects\019 Fahrgemeinschaft\Fahrgemeinschaftsapp\Driverlist\{username}.csv");

                DirectoryInfo di = new DirectoryInfo($@"C:\010Projects\019 Fahrgemeinschaft\Fahrgemeinschaftsapp\Carpools\");
                FileInfo[] Files = di.GetFiles();

                foreach (FileInfo file in Files)
                {
                    string Content = string.Empty;
                    using (StreamReader reader = new StreamReader($@"C:\010Projects\019 Fahrgemeinschaft\Fahrgemeinschaftsapp\Carpools\{file}"))
                    {
                        Content = reader.ReadToEnd();
                    }

                    if (Content.Contains($",{username},"))
                    {
                        File.Delete($@"C:\010Projects\019 Fahrgemeinschaft\Fahrgemeinschaftsapp\Carpools\{file}");
                    }
                }
                Console.Clear();
                Console.WriteLine("Your account and all carpools you are involved got deleted");
                Console.ForegroundColor = ConsoleColor.White;
                Thread.Sleep(2000);
                loggenin = false;
                return loggenin;
            }
            else
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("Redirecting to menu ...");
                Thread.Sleep(1000);
                return loggenin;
            }                        
        }                
    }
}
