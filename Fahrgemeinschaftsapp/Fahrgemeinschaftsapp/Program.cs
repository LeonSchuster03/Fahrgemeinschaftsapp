using DocumentFormat.OpenXml.Math;
using Microsoft.SqlServer.Server;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Fahrgemeinschaftsapp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            List<User> user_list = new List<User>();
            List<Driver> driver_list = new List<Driver>();
            LogIn(user_list, driver_list);

            Console.ReadLine();
        
        }

        public static void LogIn(List<User> user_list, List<Driver> driver_list)
        {
            Console.WriteLine("Do you already have an account? (y/n)");
            string AccountYN = Console.ReadLine();
            if(AccountYN == "y")
            {
                Console.WriteLine("Please enter your username");
                string username = Console.ReadLine();

                Console.WriteLine("klappt");

            }
            else
            {
                Registration(user_list, driver_list);
            }
        }
        public static void Registration(List<User>user_list, List<Driver>driver_list)  //Creating .csv File and setting username and password 
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
                        Console.WriteLine(fi);
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
            int age = Convert.ToInt32(Console.ReadLine()) ;

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
            if(hascarstring == "y")
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
        }
        public static void PrintUserInfo()
        {
            Console.WriteLine("Here are all the information we saved about you:");
            //Console.WriteLine($"Username: {user_list.User}");
        }
    }

    
}
