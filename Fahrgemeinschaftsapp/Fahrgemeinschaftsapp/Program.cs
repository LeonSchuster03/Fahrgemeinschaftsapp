using DocumentFormat.OpenXml.Math;
using Microsoft.SqlServer.Server;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Fahrgemeinschaftsapp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            List<User> user_list = new List<User>();
            List<Driver> driver_list = new List<Driver>();
            while (true)
            {
                Console.WriteLine("Welcome to the carpool app");
                Console.WriteLine(" ");
                string username = LogIn(user_list, driver_list);
                bool loggedin = true;
                do
                {
                    switch (Menu())
                    {
                        case "1":
                            break; //TODO List passengers
                        case "2":
                            break; //TODO List drivers
                        case "3":
                            PrintUserInfo(username); //TODO make returned list look cleaner
                            break;
                        case "4":
                            loggedin = DeleteAccount(username);
                            
                            break;
                        case "5":
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
                    Thread.Sleep(1000);
                    Console.Clear();
                    Registration(user_list, driver_list);
                }
            }
            else
            {
                Registration(user_list, driver_list);
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
            userinfo.Add(startlocation);
            userinfo.Add(destination);
            userinfo.Add(hascarstring);
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

                userinfo.Add(seatst);
                userinfo.Add(seatsf);
                userinfo.Add(starttime.ToShortTimeString());

                var temp = username + ".driver";

                driver_list.Add(new Driver(temp, firstname, lastname, age, gender, startlocation, destination, hascar, Convert.ToInt32(seatst), Convert.ToInt32(seatsf), starttime));

            }


            user_list.Add(new User(username, firstname, lastname, age, gender, startlocation, destination, hascar));



            user_list.Add(new User(username, firstname, lastname, age, gender, startlocation, destination, hascar));

            using (StreamWriter writer = new StreamWriter($@"C:\010Projects\019 Fahrgemeinschaft\Fahrgemeinschaftsapp\Userlist\{username}.csv"))
            {
                for (int i = 0; i < userinfo.Count; i++)
                {
                    writer.Write(string.Join(";", userinfo[i]));
                    writer.Write(",");
                }

            }

            Console.WriteLine("Saved the information in our database");
            Thread.Sleep(500);
            Console.Clear();
            Console.WriteLine("Redirecting to Menu ...");
            Thread.Sleep(500);
            
        }
        public static void PrintUserInfo(string username)
        {
            Console.Clear();
            Console.WriteLine("Here are all the information we saved about you:");
            Console.WriteLine("------------------------------------------------- ");
            //Console.WriteLine("Username, first name, last name, age, gender, StartLocation, EndLocation, is driver?");
            
            using (StreamReader sr = new StreamReader($@"C:\010Projects\019 Fahrgemeinschaft\Fahrgemeinschaftsapp\Userlist\{username}.csv"))
                Console.WriteLine(sr.ReadLine());
            //Console.WriteLine($"Username: {user_list.User}");


            Console.WriteLine("------------------------------------------------- ");
            Console.WriteLine("[1] Go back to menu");
            Console.ReadLine();
            
        }
    
        public static string Menu()
        {
            Console.Clear();
            Console.WriteLine($"Welcome ");
            Console.WriteLine("--------------------------------------------------- ");
            Console.WriteLine("Please choose, what you want to do:");
            Console.WriteLine("--------------------------------------------------- ");
            Console.WriteLine("[1] Look for possible passengers");
            Console.WriteLine("[2] Look for a possible driver");
            Console.WriteLine("[3] List information about you");
            Console.WriteLine("--------------------------------------------------- ");
            Console.WriteLine("[4] Delete your account");
            Console.WriteLine("[5] Log out");

            string navigatemenu = Console.ReadLine();

            return navigatemenu;

        }
        public static bool DeleteAccount(string username)
        {
            bool loggenin = true;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Do you really want to delete your account? (y/n)");
            string confirmation = Console.ReadLine();
            if(confirmation == "y")
            {
                File.Delete($@"C:\010Projects\019 Fahrgemeinschaft\Fahrgemeinschaftsapp\Userlist\{username}.csv");
                Console.Clear();
                Console.WriteLine("Your account got deleted");
                Console.ForegroundColor = ConsoleColor.White;
                Thread.Sleep(1500);
                loggenin = false;
                return loggenin;


            }
            else
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("Redirecting to menu ...");
                Thread.Sleep(500);
                return loggenin;
            }
            
            
        }
    }

}
