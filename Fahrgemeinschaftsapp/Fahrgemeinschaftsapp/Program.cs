using DocumentFormat.OpenXml.Bibliography;
using DocumentFormat.OpenXml.Drawing.Charts;
using DocumentFormat.OpenXml.Math;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.SqlServer.Server;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
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
            List<User> user_list = new List<User>();
            List<Carpool> carpool_list = new List<Carpool>();
            while (true)
            {
                Console.WriteLine("Welcome to the carpool app");
                Console.WriteLine(" ");
                string username = LogIn(user_list);
                bool loggedin = true;

                do
                {
                    switch (Menu(username))
                    {
                        case "1":
                            PrintUserInfo(username);
                            break;
                        case "2":
                            PrintAllUsers(username); 
                            break;
                        case "3":
                            PrintAllDrivers(username);
                            break;
                        case "4":
                            CreateCarpool(carpool_list, username);
                            break;
                        case "5":
                            ViewYourCarpools(carpool_list, username);
                            break;
                        case "6":
                            DeleteCarpool(username);
                            break;
                        case "7":
                            ChangeUserPassword(username);
                            break;
                        case "8":
                            loggedin = DeleteAccount(username);
                            break;
                        case "9":
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
        public static string LogIn(List<User> user_list) //by entering his/her username and password the user gains access to the menu
        {
        Home:
            Console.WriteLine("Do you already have an account? (y/n)");
            string AccountYN = Console.ReadLine();
            string username = string.Empty;
            Console.Clear();
            if (AccountYN == "y")
            {
            LoggingIn:
                Console.WriteLine("Please enter your username");
                username = Console.ReadLine();
                FileInfo fi = new FileInfo($@"C:\010Projects\019 Fahrgemeinschaft\Fahrgemeinschaftsapp\Userlist\\\\/{username}.csv\");
                if (fi.Exists)
                {
                    string input_pw = null;
                    Console.WriteLine("Please enter your password");
                    input_pw = HidePassword();
                    
                    using (StreamReader sr = new StreamReader(Path.Combine($@"C:\010Projects\019 Fahrgemeinschaft\Fahrgemeinschaftsapp\Userlist\\\\{username}.csv")))
                    {
                        string[] values = new string[7];
                        var line = sr.ReadLine();
                        values = line.Split(';');

                        if (values[7] != input_pw)
                        {
                            Console.WriteLine("Username or password is incorrect. Please the again!");
                            Thread.Sleep(750);
                            Console.Clear();
                            goto LoggingIn;
                           }                        
                    }
                }
                else
                {
                    Console.WriteLine("This user doesnt exist. You get redirected to Registration");
                    Thread.Sleep(1500);
                    Console.Clear();
                    username = Registration(user_list);
                }
            }
            else if (AccountYN == "n")
            {
                Registration(user_list);
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

        public static string Registration(List<User> user_list)  //Creating .csv File and setting username and password 
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
                    Console.WriteLine("Choose a password");
                    string pw = HidePassword();
                    Console.WriteLine("Confirm the password");
                    string pw_confirm = HidePassword();
                    do
                    {
                        if (pw == pw_confirm)
                        {
                            using (StreamWriter writer = new StreamWriter($@"C:\010Projects\019 Fahrgemeinschaft\Fahrgemeinschaftsapp\Userlist\{username}.csv"))
                                writer.WriteLine(pw);
                            break;
                        }
                        else
                        {
                            Console.WriteLine("The passwords dont match. Please try again!");
                        }
                    } while (true);
                    break;
                }
                else
                {
                    Console.WriteLine("This username already exists, please choose another");
                    username = Console.ReadLine();
                }
            } while (true);
            
            CollectUserInfo(username, user_list);
            return username;
        }

        public static void CollectUserInfo(string username, List<User> user_list) //Adding info to userspecific file
        {
            List<string> userinfo = new List<string>();
            List<string> driverinfo = new List<string>();
            Console.Clear();
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
            string hascarstring = string.Empty;
            if(firstname != "Marcello")
            {
                Console.WriteLine("Do you have a car/Are you able to drive? (y/n)");
                hascarstring = Console.ReadLine();
            }
            else
            {
                Console.WriteLine("Because you drive 70+ km/h instead of 50 km/h while driving through villages," +
                    " you are not able to get registered as driver");
                Thread.Sleep(2500);
                hascarstring = "n";
            }
            
            userinfo.Add(firstname);
            userinfo.Add(lastname);
            userinfo.Add(Convert.ToString(age));
            userinfo.Add(gender);
            userinfo.Add(startlocation);
            userinfo.Add(destination);
            string pw = string.Empty;
            using (StreamReader reader = new StreamReader($@"C:\010Projects\019 Fahrgemeinschaft\Fahrgemeinschaftsapp\Userlist\{username}.csv"))
            {
                pw = reader.ReadLine();
            }
            
            bool hascar = false;
            if (hascarstring == "y")
            {
                hascar = true;
                FileInfo fi = new FileInfo($@"C:\010Projects\019 Fahrgemeinschaft\Fahrgemeinschaftsapp\Driverlist\\\\/{username}.csv\");
                File.Create(username);

                using (StreamWriter writer = new StreamWriter($@"C:\010Projects\019 Fahrgemeinschaft\Fahrgemeinschaftsapp\Userlist\{username}.csv"))
                {
                    var newLine = $"{username};{firstname};{lastname};{Convert.ToString(age)};{gender};{startlocation};{destination};{pw};{hascar}";
                    writer.WriteLine(newLine);
                }                
            }
            else if (hascarstring == "n")
            {
                using (StreamWriter writer = new StreamWriter($@"C:\010Projects\019 Fahrgemeinschaft\Fahrgemeinschaftsapp\Userlist\{username}.csv"))
                {
                    var newLine = $"{username};{firstname};{lastname};{Convert.ToString(age)};{gender};{startlocation};{destination};{pw};{hascar}";
                    writer.WriteLine(newLine);
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

        public static string Menu(string username) //printing the menu, where the user can choose what he/she wants to do
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
            Console.WriteLine("[7] Change password");
            Console.WriteLine("[8] Delete your account");
            Console.WriteLine("[9] Log out");

            string navigatemenu = Console.ReadLine();
            return navigatemenu;
        }

        public static void ChangeUserPassword(string username) //by entering the old and new password, the user can change his/her password
        {
        ChangePW:
            Console.Clear();
            Console.WriteLine("Please enter your old password");
            string input_pw = HidePassword();
            var filetext = File.ReadAllText($@"C:\010Projects\019 Fahrgemeinschaft\Fahrgemeinschaftsapp\Userlist\{username}.csv");
            string[] values = filetext.Split(';');
            string user_pw = values[7];
            user_pw = user_pw.Replace("\r\n", string.Empty);

            if (input_pw == user_pw)
            {
                Console.WriteLine("Please enter your new password");
                string new_pw = HidePassword();
                if(new_pw == user_pw)
                {  
                    Console.WriteLine("Your new password cant be your old password!");
                    Thread.Sleep(1000);
                    goto ChangePW;
                }
                Console.WriteLine("Please confirm your new password");
                string confirm_new_pw = HidePassword();
                if (confirm_new_pw == new_pw)
                {
                    values[7] = new_pw;

                    using (StreamWriter writer = new StreamWriter($@"C:\010Projects\019 Fahrgemeinschaft\Fahrgemeinschaftsapp\Userlist\{username}.csv"))
                    {
                        writer.WriteLine($"{values[0]};{values[1]};{values[2]};{values[3]};{values[4]};{values[5]};{values[6]};{values[7]}");
                    }
                    Console.Clear();
                    Console.WriteLine("Your password got changed");
                    Console.WriteLine("Redirecting to menu ...");
                    Thread.Sleep(2000);
                }
                else
                {
                    Console.WriteLine("The passwords dont match. Please try again!");
                    Thread.Sleep(1500);
                    Console.Clear();
                    goto ChangePW;
                }
            }
            else if (input_pw != user_pw)
            {
                Console.WriteLine("The password was incorrect, please try again");
                Thread.Sleep(1500);
                Console.Clear();
                goto ChangePW;
            }
        }

        public static void PrintAllUsers(string username) //all registered users are shown in a list
        {
            Console.Clear();
            Console.WriteLine("[1] Show all users");
            Console.WriteLine("[2] Only show users with the same destination");
            string input = Console.ReadLine();
            
            if(input == "1")
            {
                Console.Clear();
                Console.WriteLine("Here are all users, who are currently looking for a carpool");
                Console.WriteLine("------------------------------------------------- ");

                foreach (string file in Directory.EnumerateFiles($@"C:\010Projects\019 Fahrgemeinschaft\Fahrgemeinschaftsapp\Userlist\\\\"))
                {
                    using (var reader = new StreamReader(file))
                    {
                        string[] values = new string[8];
                        var line = reader.ReadLine();
                        values = line.Split(';');
                        Console.WriteLine($"{values[1]} ({values[4]}/{values[3]}), Start: {values[5]}, Destination: {values[6]}");
                    }
                }                
            }
            else if(input == "2")
            {
                string text = File.ReadAllText($@"C:\010Projects\019 Fahrgemeinschaft\Fahrgemeinschaftsapp\Userlist\{username}.csv");
                string[] info = text.Split(';');
                Console.Clear();
                Console.WriteLine($"Here are all users, who have {info[6]} as destination");
                Console.WriteLine("------------------------------------------------- ");
                
                foreach (string file in Directory.EnumerateFiles($@"C:\010Projects\019 Fahrgemeinschaft\Fahrgemeinschaftsapp\Userlist\\\\"))
                {
                    using (var reader = new StreamReader(file))
                    {
                        string[] values = new string[8];
                        var line = reader.ReadLine();
                        values = line.Split(';');
                        if ((info[6] == values[6]) && (info[0] != values[0]))
                        {
                            Console.WriteLine($"{values[1]} ({values[4]}/{values[3]}), Start: {values[5]}, Destination: {values[6]}");
                        }
                        
                    }
                }
            }
            Console.WriteLine("------------------------------------------------- ");
            Console.WriteLine("[1] Go back to menu");
            Console.ReadLine();
        }

        public static void PrintAllDrivers(string username) //all registered users, who are allowed to drive are shown in a list
        {           
            Console.Clear();
            Console.WriteLine("[1] Show all drivers");
            Console.WriteLine("[2] Only show drivers with the same destination");
            string input = Console.ReadLine();

            if (input == "1")
            {
                Console.Clear();
                Console.WriteLine("Here are all drivers, who are currently looking for a carpool");
                Console.WriteLine("------------------------------------------------- ");
                foreach (string fileuser in Directory.EnumerateFiles($@"C:\010Projects\019 Fahrgemeinschaft\Fahrgemeinschaftsapp\Userlist\\\\"))
                {
                    using (var reader = new StreamReader(fileuser))
                    {
                        var line = reader.ReadLine();
                        string[]values = line.Split(';');
                        if (values[8] == "TRUE")
                        {
                            Console.WriteLine($"{values[1]} ({values[4]}/{values[3]}), Start: {values[5]}, Destination: {values[6]}");
                        }
                    }
                }
            }
            else if (input == "2")
            {
                string text = File.ReadAllText($@"C:\010Projects\019 Fahrgemeinschaft\Fahrgemeinschaftsapp\Userlist\{username}.csv");
                string[] info = text.Split(';');
                Console.Clear();
                Console.WriteLine($"Here are all drivers, who have {info[6]} as destination");
                Console.WriteLine("------------------------------------------------- ");
                foreach (string fileuser in Directory.EnumerateFiles($@"C:\010Projects\019 Fahrgemeinschaft\Fahrgemeinschaftsapp\Userlist\\\\"))
                {                                       
                    using (var reader = new StreamReader(fileuser))
                    {                           
                        var line = reader.ReadLine();
                        string[]values = line.Split(';');
                        if ((info[6] == values[6]) && (info[0] != values[0]) && (values[8] == "TRUE"))
                        {
                            Console.WriteLine($"{values[1]} ({values[4]}/{values[3]}), Start: {values[5]}, Destination: {values[6]}");
                        }
                    }                   
                }
            }
            Console.WriteLine("------------------------------------------------- ");
            Console.WriteLine("[1] Go back to menu");
            Console.ReadLine();           
        }

        public static void PrintUserInfo(string username) //all the information about the user is shown in a list
        {
            Console.Clear();
            Console.WriteLine("Here are all the information we saved about you:");
            Console.WriteLine("------------------------------------------------- ");

            using (var reader = new StreamReader($@"C:\010Projects\019 Fahrgemeinschaft\Fahrgemeinschaftsapp\Userlist\{username}.csv"))
            {
                var line = reader.ReadLine();
                var values = line.Split(';');

                Console.WriteLine($"Username:       {values[0]}");
                Console.WriteLine($"First name:     {values[1]}");
                Console.WriteLine($"Last name:      {values[2]}");
                Console.WriteLine($"Age:            {values[3]}");
                Console.WriteLine($"Gender:         {values[4]}");
                Console.WriteLine($"Start Place     {values[5]}");
                Console.WriteLine($"Destination:    {values[6]}");
                Console.WriteLine($"Is a driver:    {values[8]}");
            }
            Console.WriteLine("------------------------------------------------- ");
            Console.WriteLine("[1] Edit the Information");
            Console.WriteLine("[2] Change your password");
            Console.WriteLine("[3] Go back to menu");
            string input = Console.ReadLine();
            if (input == "1")
            {
                EditUserInfo(username);
            }
            else if (input == "2")
            {
                ChangeUserPassword(username);
            }
            else
            {
                Console.Clear();
            }
        }

        public static void EditUserInfo(string username) //Allows the user to edit his/her information
        {
        Edit:
            Console.Clear();
            Console.WriteLine("Which information do you want to edit?");
            Console.WriteLine("[1] First name");
            Console.WriteLine("[2] Last name");
            Console.WriteLine("[3] Age");
            Console.WriteLine("[4] Gender");
            Console.WriteLine("[5] Start place");
            Console.WriteLine("[6] Destination");
            Console.WriteLine("[7] is driver?");
            Console.WriteLine("");
            Console.WriteLine("[8] Go back");
            Console.WriteLine("[9] Go to menu");

            string input = Console.ReadLine();

            var filetext = File.ReadAllText($@"C:\010Projects\019 Fahrgemeinschaft\Fahrgemeinschaftsapp\Userlist\{username}.csv");
            string[] values = filetext.Split(';');
            Console.Clear();
            bool gotomenu = false;
            bool goback = false;
            switch (input)
            {
                case "1":
                    Console.WriteLine("Please enter your new first name");
                    values[1] = Console.ReadLine();
                    break;
                case "2":
                    Console.WriteLine("Please enter your new last name");
                    values[2] = Console.ReadLine();
                    break;
                case "3":
                    Console.WriteLine("Please enter your new age");
                    values[3] = Console.ReadLine();
                    break;
                case "4":
                    Console.WriteLine("Please enter your new gender");
                    values[4] = Console.ReadLine();
                    break;
                case "5":
                    Console.WriteLine("Please enter your new start place");
                    values[5] = Console.ReadLine();
                    break;
                case "6":
                    Console.WriteLine("Please enter your new destination");
                    values[6] = Console.ReadLine();
                    break;
                case "7":
                    Console.WriteLine("Please enter your new driving status (y/n)");
                    string candrive = Console.ReadLine();
                    if (candrive == "y")
                    {
                        values[8] = "TRUE";                                             
                    }
                    else if (candrive == "n")
                    {
                        values[8] = "FALSE";
                    }
                    using (StreamWriter writer = new StreamWriter($@"C:\010Projects\019 Fahrgemeinschaft\Fahrgemeinschaftsapp\Userlist\{username}.csv"))
                    {
                        writer.WriteLine($"{values[0]};{values[1]};{values[2]};{values[4]};{values[5]};{values[6]};{values[7]};{values[8]}");
                    }
                    break;
                case "8":
                    goback = true;
                    break;
                default:
                    gotomenu = true;
                    break;
            }
            using (StreamWriter writer = new StreamWriter($@"C:\010Projects\019 Fahrgemeinschaft\Fahrgemeinschaftsapp\Userlist\{username}.csv"))
            {
                writer.WriteLine($"{values[0]};{values[1]};{values[2]};{values[3]};{values[4]};{values[5]};{values[6]};{values[7]}");
            }
            if (gotomenu)
            {
                Console.Clear();
                Console.WriteLine("Redirecting to menu ...");
                Thread.Sleep(750);
            }
            else if (goback)
            {
                PrintUserInfo(username);
            }
            else
            {
                Console.Clear();
                Console.WriteLine("The information got saved in our database");
                Thread.Sleep(750);
                goto Edit;
            }
        }

        public static void CreateCarpool(List<Carpool> carpool, string username) //the user can create a carpool and add other users
        {
            DirectoryInfo di = new DirectoryInfo($@"C:\010Projects\019 Fahrgemeinschaft\Fahrgemeinschaftsapp\Carpools");
            //Collecting the neccessary info
            Console.Clear();
            Console.WriteLine("How many seats does the car have?");
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
            carpool.Add(new Carpool(carpoolID, passengercount, destination, startloc, departure));

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
            for (int j = 1; j < passengercount; j++)
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
                else if(dude == username)
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
            passengers.Add(username);

            using (StreamWriter writer = new StreamWriter($@"C:\010Projects\019 Fahrgemeinschaft\Fahrgemeinschaftsapp\Carpools\{Convert.ToString(carpoolID)}.csv"))
            {
                var newLine = $"{carpoolID};{startloc};{destination};{s_departure};{passengercount}";

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
            Console.WriteLine($"You will start at {s_departure} in {startloc}");
            Console.WriteLine(" ");
            Console.WriteLine("[1] View your carpools");
            Console.WriteLine("[2] Go back to menu");
            string input = Console.ReadLine();
            if (input == "1")
            {
                ViewYourCarpools(carpool, username);
            }
        }

        public static void ViewYourCarpools(List<Carpool> carpool, string username) //the user gets to see all carpools, where he/she is a member of
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
                if (content.Contains($",{username},"))
                {
                    string[] values = content.Split(';');
                    using (StreamReader sr = new StreamReader($@"C:\010Projects\019 Fahrgemeinschaft\Fahrgemeinschaftsapp\Carpools\{file}"))
                    {
                        Console.WriteLine($"The Carpool with the ID {values[0].Trim(',')} will start at {values[3]} in {values[1]}");
                        Console.WriteLine($"It's destination is {values[2]}");
                        Console.WriteLine("The passengers are:");
                        for(int i = 5; i < values.Length; i++)
                        {
                            values[i] = values[i].Trim(',');
                            if (values[i] == username)
                            {
                                values[i] = "You";
                                values[i].Replace("\r\n", string.Empty);
                            }
                            Console.WriteLine(values[i]);
                        }
                    }
                }
            }
            Console.WriteLine("");
            Console.WriteLine("------------------------------------------------- ");
            Console.WriteLine("[1] Create a carpool");
            Console.WriteLine("[2] Join a carpool");
            Console.WriteLine("[3] Leave a carpool");
            Console.WriteLine("[4] Go back to menu");
            string input = Console.ReadLine();
            switch (input)
            {
                case "1":
                    CreateCarpool(carpool, username);
                    break;
                case "2":
                    JoinCarpool(carpool, username);
                    break;
                case"3":
                    LeaveCarpool(carpool, username);
                    break;
                default:
                    break; //redirecting to menu                        
            }
        }

        public static void JoinCarpool(List<Carpool> carpool, string username)
        {
            Console.Clear();
            Console.WriteLine("Enter the ID of the Carpool you want to join");
            string filename = Console.ReadLine();
            FileInfo fi = new FileInfo($@"C:\010Projects\019 Fahrgemeinschaft\Fahrgemeinschaftsapp\Carpools\{filename}.csv");
            if (fi.Exists)
            {
                string text = File.ReadAllText($@"C:\010Projects\019 Fahrgemeinschaft\Fahrgemeinschaftsapp\Carpools\{filename}.csv").Replace("\r\n", string.Empty);
                string[] values = text.Split(';');
                List<string> passengers = new List<string>();
                for(int i = 5; i < values.Length; i++)
                {
                    if (values[i] != "")
                    {
                        passengers.Add(values[i].Replace("\r\n", string.Empty));
                    }                   
                }
                if(values.Length-5 < Convert.ToInt32(values[4]))
                {
                    using (StreamWriter writer = new StreamWriter($@"C:\010Projects\019 Fahrgemeinschaft\Fahrgemeinschaftsapp\Carpools\{filename}.csv"))
                    {
                        var newLine = $"{values[0]};{values[1]};{values[2]};{values[3]};{values[4]}";

                        foreach (string passenger in passengers)
                        {
                            newLine += $";{passenger}";
                        }
                        newLine += $";,{username},";
                        
                        writer.Write(newLine.Replace("\r\n", string.Empty));
                    }
                    Console.Clear();
                    Console.WriteLine($"You successfully join the carpool with the ID {values[0]}");
                    Console.WriteLine(" ");
                }
                Console.WriteLine($"You cant join the carpool with the ID {values[0]}, there are no free seats");
                Console.WriteLine(" ");                
            }
            else
            {
                Console.Clear();
                Console.WriteLine($"There is no carpool with the ID {filename}");
                Console.WriteLine(" ");
            }
            Console.WriteLine("------------------------------------------------- ");
            Console.WriteLine("[1] Go back");
            Console.WriteLine("[2] Go to menu");
            string input = Console.ReadLine();
            if (input == "1")
            {
                ViewYourCarpools(carpool, username);
            }
        }

        public static void LeaveCarpool(List<Carpool> carpool, string username) //the user can choose, which carpool he/she wants to leave
        {
            Console.Clear();
            Console.WriteLine("Enter the ID of the Carpool you want to leave");
            string filename = Console.ReadLine();
            FileInfo fi = new FileInfo($@"C:\010Projects\019 Fahrgemeinschaft\Fahrgemeinschaftsapp\Carpools\{filename}.csv");
            if (fi.Exists)
            {
                string text = File.ReadAllText($@"C:\010Projects\019 Fahrgemeinschaft\Fahrgemeinschaftsapp\Carpools\{filename}.csv");
                if (text.Contains($",{username},"))
                {
                    string test = File.ReadAllText($@"C:\010Projects\019 Fahrgemeinschaft\Fahrgemeinschaftsapp\Carpools\{filename}.csv");
                    string[] values = text.Split(';');
                    var newline = string.Empty;

                    foreach (string value in values)
                    {
                        if(value != $",{username},")
                        {
                            newline += $"{value};";
                        }
                    }                                          
                    using (StreamWriter writer = new StreamWriter($@"C:\010Projects\019 Fahrgemeinschaft\Fahrgemeinschaftsapp\Carpools\{filename}.csv"))
                    {
                        writer.Write(newline);
                    }
                    Console.Clear();
                    Console.WriteLine($"You got removed successfully from the carpool with the ID {filename}");
                }
                else
                {
                    Console.Clear();
                    Console.WriteLine("You're not a part of this carpool, therefore you can't leave it");
                }
                string content = File.ReadAllText($@"C:\010Projects\019 Fahrgemeinschaft\Fahrgemeinschaftsapp\Carpools\{filename}.csv");
                string[] csv = content.Split(';');
                if (csv.Length <= 6)
                {
                    File.Delete($@"C:\010Projects\019 Fahrgemeinschaft\Fahrgemeinschaftsapp\Carpools\{filename}.csv");
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
            if(input == "1")
            {
                ViewYourCarpools(carpool, username);
            }
        }

        public static void DeleteCarpool(string username) //by entering the ID, the user can delete an existing carpool
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
                        Console.WriteLine(" ");
                        Console.WriteLine("Redirecting to menu ...");
                        Thread.Sleep(2000);
                    }
                }
                else
                {
                    Console.WriteLine($"You're not a part of the carpool with the ID {cp_id}");
                    Console.WriteLine(" ");
                    Console.WriteLine("Redirecting to menu ...");
                    Thread.Sleep(2000);
                }
            }
            else
            {
                Console.WriteLine($"A carpool with the ID {cp_id} does not exist.");
                Console.WriteLine(" ");
                Console.WriteLine("Redirecting to menu ...");
                Thread.Sleep(2000);
            }
        }

        public static bool DeleteAccount(string username) //the user can delete his account and all carpools he/she is a part of
        {
            bool loggenin = true;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Clear();
            Console.WriteLine("Please enter your password");
            string input_pw = HidePassword();
            var filetext = File.ReadAllText($@"C:\010Projects\019 Fahrgemeinschaft\Fahrgemeinschaftsapp\Userlist\{username}.csv");
            string[] values = filetext.Split(';');
            string user_pw = values[7];
            user_pw = user_pw.Replace("\r\n", string.Empty);

            if (user_pw == input_pw)
            {
                Console.WriteLine("Do you really want to delete your account? (y/n)");
                Console.WriteLine("This action is irreversible and all carpool you're a part of will get deleted");
                string confirmation = Console.ReadLine();
                if (confirmation == "y")
                {
                    File.Delete($@"C:\010Projects\019 Fahrgemeinschaft\Fahrgemeinschaftsapp\Userlist\{username}.csv");

                    DirectoryInfo di = new DirectoryInfo($@"C:\010Projects\019 Fahrgemeinschaft\Fahrgemeinschaftsapp\Carpools\");
                    FileInfo[] Files = di.GetFiles();
                    string[] content = new string[8];
                    foreach (FileInfo file in Files)
                    {
                        string text = File.ReadAllText($@"C:\010Projects\019 Fahrgemeinschaft\Fahrgemeinschaftsapp\Carpools\{file}");
                        if (text.Contains($",{username},"))
                        {
                            string test = File.ReadAllText($@"C:\010Projects\019 Fahrgemeinschaft\Fahrgemeinschaftsapp\Carpools\{file}.csv");
                            content = text.Split(';');
                            var newline = string.Empty;

                            foreach (string info in content)
                            {
                                if (info != $",{username},")
                                {
                                    newline += $"{info};";
                                }
                            }
                            using (StreamWriter writer = new StreamWriter($@"C:\010Projects\019 Fahrgemeinschaft\Fahrgemeinschaftsapp\Carpools\{file}"))
                            {
                                writer.Write(newline);
                            }
                        }
                    }
                    Console.Clear();
                    Console.WriteLine("Your account got deleted");
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
            else
            {
                Console.Clear();
                Console.WriteLine("The password was not correct");
                Console.WriteLine("Redirecting to menu ...");
                return loggenin;
            }
        }

        public static string HidePassword() //replaces the input of passwords with '*'
        {
            StringBuilder sb = new StringBuilder();
            while (true)
            {
                var key = Console.ReadKey(true);
                if (key.Key == ConsoleKey.Enter)
                    break;                              
                if(key.Key == ConsoleKey.Backspace)
                {
                    if(sb.Length > 0)
                    {
                        sb.Length--;
                        Console.Write("\b\0\b");
                    }                    
                    continue;
                }
                Console.Write("*");
                sb.Append(key.KeyChar);
            }
            Console.WriteLine(" ");
            return sb.ToString();
        }
    }   
}