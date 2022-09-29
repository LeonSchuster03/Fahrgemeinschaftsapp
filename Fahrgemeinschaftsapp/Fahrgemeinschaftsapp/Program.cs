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
            LogIn(user_list);
            Console.ReadLine();
        
        }

        public static void LogIn(List<User> user_list)
        {
            Console.WriteLine("Do you already have an account? (y/n)");
            string AccountYN = Console.ReadLine();
            if(AccountYN == "y")
            {
                Console.WriteLine("Please enter your username");
                string username = Console.ReadLine();
                string filepath = $@"C:\010Projects\019 Fahrgemeinschaft\Fahrgemeinschaftsapp\Userlist\\\\/{username}.csv\";
                Console.WriteLine("klappt");

            }
            else
            {
                Registration(user_list);
            }
        }
        public static void Registration(List<User>user_list)  //Creating .csv File and setting username and password 
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
                CollectUserInfo(username, user_list);
              

        }     

        public static void CollectUserInfo(string username, List<User> user_list) //Adding info to userspecific file
        {
            List<string> userinfo = new List<string>();
            
            Console.WriteLine("Whats your first name?");
            string firstname = Console.ReadLine();
            
            Console.WriteLine("Whats your last name?");
            string lastname = Console.ReadLine();

            Console.WriteLine("Please enter your age");
            string agestring = Console.ReadLine();

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
            userinfo.Add(agestring);
            userinfo.Add(gender);
            userinfo.Add(startlocation);
            userinfo.Add(destination);
            userinfo.Add(hascarstring);

            int age = Convert.ToInt32(agestring);
            bool hascar = false;
            if(hascarstring == "y")
            {
                hascar = true;
            }

            user_list.Add(new User(username, firstname, lastname, age, gender, startlocation, destination, hascar));

            using (StreamWriter writer = new StreamWriter($@"C:\010Projects\019 Fahrgemeinschaft\Fahrgemeinschaftsapp\Userlist\{username}.csv"))
            {
                for (int i = 0; i < userinfo.Count; i++)

                    writer.WriteLine(string.Join(";", userinfo[i]));
            }

        }
    }
    
}
