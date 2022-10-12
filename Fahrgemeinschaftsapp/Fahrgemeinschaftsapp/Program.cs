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
using System.Security.Cryptography;
using System.Runtime.Remoting.Messaging;

namespace Fahrgemeinschaftsapp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "Carpool App by Leon";
            List<User> userList = new List<User>();
            List<Carpool> carPoolList = new List<Carpool>();
            while (true)
            {
                Console.WriteLine("Welcome to the carpool app");
                Console.WriteLine(" ");
                string userName = LogIn(userList);
                

               Home:
                bool loggedIn = true;
                string userInput = Menu(userName);
                switch (userInput)
                {
                    case "1":
                        userName = PrintUserInfo(userName);
                        goto Home;
                    case "2":
                        PrintAllUsers(userName);
                        goto Home;
                    case "3":
                        PrintAllDrivers(userName);
                        goto Home;
                    case "4":
                        CreateCarpool(carPoolList, userName);
                        goto Home;
                    case "5":
                        ViewYourCarpools(carPoolList, userName); //TODO Add carpool password, so you only can join carpools by entering it
                        goto Home;
                    case "6":
                        DeleteCarpool(userName);
                        goto Home;
                    case "7":
                        userName = Settings(userName); //TODO fix delete account log off issue
                        if (loggedIn)
                        {
                            goto Home;
                        }
                        else
                        {
                            break;
                        }
                    case "8":
                        loggedIn = false;
                        break;
                }
               
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Logging out ...");
                Console.ForegroundColor = ConsoleColor.White;
                Thread.Sleep(1000);
                Console.Clear();
            }
        }
        public static string LogIn(List<User> userList) //by entering his/her username and password the user gains access to the menu
        {
        Home:
            Console.WriteLine("Do you already have an account? (y/n)");
            string hasAccount = Console.ReadLine();
            string userName = string.Empty;
            Console.Clear();
            if (hasAccount == "y")
            {
            LoggingIn:
                Console.WriteLine("Please enter your username");
                userName = Console.ReadLine();
                FileInfo fi = new FileInfo($@"C:\010Projects\019 Fahrgemeinschaft\Fahrgemeinschaftsapp\Userlist\\\\/{userName}.csv\");
                if (fi.Exists)
                {

                    Console.WriteLine("Please enter your password");
                    string inputPassword = HidePassword();
                    
                    //using (StreamReader sr = new StreamReader(Path.Combine($@"C:\010Projects\019 Fahrgemeinschaft\Fahrgemeinschaftsapp\Userlist\\\\{username}.csv")))
                    //{
                    //    string[] values = new string[7];
                    //    var line = sr.ReadLine();
                    //    values = line.Split(';');

                        if (!ValidateUserPassword(userName,inputPassword))
                        {
                            Console.WriteLine("Username or password is incorrect. Please the again!");
                            Thread.Sleep(750);
                            Console.Clear();
                            goto LoggingIn;
                           }                        
                    //}
                }
                else
                {
                    Console.WriteLine("This user doesnt exist. You get redirected to Registration");
                    Thread.Sleep(1500);
                    Console.Clear();
                    userName = Registration(userList);
                }
            }
            else if (hasAccount == "n")
            {
                Registration(userList);
            }
            else
            {
                Console.WriteLine("Your input wasnt clear, please try again");
                Thread.Sleep(1250);
                Console.Clear();
                goto Home;
            }
            return userName;
        }

        public static string Registration(List<User> userList)  //Creating .csv File and setting username and password 
        {
            Console.WriteLine("What should be your username?");
            string userName = Console.ReadLine();
            do
            {
                FileInfo fi = new FileInfo($@"C:\010Projects\019 Fahrgemeinschaft\Fahrgemeinschaftsapp\Userlist\\\\/{userName}.csv\");
                if (!fi.Exists)
                {
                    using (File.Create(userName))
                        Console.WriteLine($"Your username is now {userName}");
                    Console.WriteLine("Choose a password");
                    string pw = HashPassword(HidePassword());
                    Console.WriteLine("Confirm the password");
                    string pwConfirm = HashPassword(HidePassword());
                    do
                    {
                        if (pw == pwConfirm)
                        {
                            using (StreamWriter writer = new StreamWriter($@"C:\010Projects\019 Fahrgemeinschaft\Fahrgemeinschaftsapp\Userlist\{userName}.csv"))
                                writer.WriteLine(HashPassword(pw));
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
                    userName = Console.ReadLine();
                }
            } while (true);
            
            CollectUserInfo(userName, userList);
            return userName;
        }

        public static void CollectUserInfo(string userName, List<User> userList) //Adding info to userspecific file
        {
            List<string> userInfo = new List<string>();
            Console.Clear();
            Console.WriteLine("Whats your first name?");
            string firstName = Console.ReadLine();
            Console.WriteLine("Whats your last name?");
            string lastName = Console.ReadLine();
            Console.WriteLine("Please enter your age");
            int age = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("Whats your gender (m/f/d)");
            string gender = Console.ReadLine();
            Console.WriteLine("Where is your starting location");
            string startLocation = Console.ReadLine();
            Console.WriteLine("Whats your destination?");
            string destination = Console.ReadLine();
            string hasCarString = string.Empty;
            if((firstName != "Marcello")&&(lastName != "Greulich"))
            {
                Console.WriteLine("Do you have a car/Are you able to drive? (y/n)");
                hasCarString = Console.ReadLine();
            }
            else
            {
                Console.WriteLine("You drive 70+ km/h instead of 50 km/h while driving through villages," +
                    "therefore you are not able to get registered as driver");
                Thread.Sleep(2500);
                hasCarString = "n";
            }
            
            userInfo.Add(firstName);
            userInfo.Add(lastName);
            userInfo.Add(Convert.ToString(age));
            userInfo.Add(gender);
            userInfo.Add(startLocation);
            userInfo.Add(destination);
            string pw = string.Empty;
            using (StreamReader reader = new StreamReader($@"C:\010Projects\019 Fahrgemeinschaft\Fahrgemeinschaftsapp\Userlist\{userName}.csv"))
            {
                pw = reader.ReadLine();
            }
            
            bool hascar = false;
            if (hasCarString == "y")
            {
                using (StreamWriter writer = new StreamWriter($@"C:\010Projects\019 Fahrgemeinschaft\Fahrgemeinschaftsapp\Userlist\{userName}.csv"))
                {
                    var newLine = $"{userName};{firstName};{lastName};{Convert.ToString(age)};{gender};{startLocation};{destination};{pw};{hascar}";
                    writer.WriteLine(newLine);
                }                
            }
            else if (hasCarString == "n")
            {
                using (StreamWriter writer = new StreamWriter($@"C:\010Projects\019 Fahrgemeinschaft\Fahrgemeinschaftsapp\Userlist\{userName}.csv"))
                {
                    var newLine = $"{userName};{firstName};{lastName};{Convert.ToString(age)};{gender};{startLocation};{destination};{pw};{hascar}";
                    writer.WriteLine(newLine);
                }
            }
            userList.Add(new User(userName, firstName, lastName, age, gender, startLocation, destination, hascar));

            Console.WriteLine(" ");
            Console.WriteLine("Saving the information in our database ...");
            Thread.Sleep(500);
            Console.Clear();
            Console.WriteLine("Redirecting to Menu ...");
            Thread.Sleep(500);
        }
           
        public static string Menu(string userName) //printing the menu, where the user can choose what he/she wants to do
        {
            Console.Clear();  
            Console.WriteLine($"Welcome {userName},");
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
            Console.WriteLine("[7] Settings");
            Console.WriteLine("[8] Log out");

            string navigateMenu = Console.ReadLine();
            return navigateMenu;
        }
        public static string Settings(string userName)
        {
            Console.Clear();
            Console.WriteLine("Settings");
            Console.WriteLine("--------------------------------------------------- ");
            Console.WriteLine("[1] Change username");
            Console.WriteLine("[2] Change password");
            Console.WriteLine("[3] Delete your account");
            Console.WriteLine("--------------------------------------------------- ");
            Console.WriteLine("[4] Back to menu");
            string userInput = Console.ReadLine();
            switch (userInput)
            {
                case "1":
                    ChangeUserName(userName);
                    break;
                case "2":
                    ChangeUserPassword(userName);
                    break;
                case "3":
                    DeleteAccount(userName);
                    break;
                default:
                    break;
            }   
            return userName;
        }

        public static void ChangeUserPassword(string userName) //by entering the old and new password, the user can change his/her password
        {
        ChangePW:
            Console.Clear();
            Console.WriteLine("Please enter your old password");
            string inputPw = HidePassword();


            if (ValidateUserPassword(userName, inputPw))
            {
                Console.WriteLine("Please enter your new password");
                string newPw = HidePassword();
                if(ValidateUserPassword(userName, newPw))
                {  
                    Console.WriteLine("Your new password cant be your old password!");
                    Thread.Sleep(1000);
                    goto ChangePW;
                }
                Console.WriteLine("Please confirm your new password");
                string confirmNewPw = HidePassword() ;
                if (confirmNewPw == newPw)
                {
                    string[] values;
                    using (StreamReader reader = new StreamReader($@"C:\010Projects\019 Fahrgemeinschaft\Fahrgemeinschaftsapp\Userlist\{userName}.csv"))
                    {
                        string text = File.ReadAllText($@"C:\010Projects\019 Fahrgemeinschaft\Fahrgemeinschaftsapp\Userlist\{userName}.csv");
                        values = text.Split(';');
                        values[7] = HashPassword(newPw);
                    }
                    using (StreamWriter writer = new StreamWriter($@"C:\010Projects\019 Fahrgemeinschaft\Fahrgemeinschaftsapp\Userlist\{userName}.csv"))
                    {
                        writer.WriteLine($"{values[0]};{values[1]};{values[2]};{values[3]};{values[4]};{values[5]};{values[6]};{values[7]};{values[8]}");
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
            else if (!ValidateUserPassword(userName, inputPw))
            {
                Console.WriteLine("The password was incorrect, please try again");
                Thread.Sleep(1500);
                Console.Clear();
                goto ChangePW;
            }
        }
        public static string ChangeUserName(string userName)
        {
            string[] values = new string[8];
            using (var reader = new StreamReader($@"C:\010Projects\019 Fahrgemeinschaft\Fahrgemeinschaftsapp\Userlist\{userName}.csv"))
            {               
                var line = reader.ReadLine();
                values = line.Split(';');
            }
            Console.WriteLine("Please enter your password");
            string inputPw = HidePassword();

            if (ValidateUserPassword(userName, inputPw))
            {               
                Console.WriteLine("What should be your new Username?");
                TryAgain:
                string newUserName = Console.ReadLine();
                FileInfo fi = new FileInfo($@"C:\010Projects\019 Fahrgemeinschaft\Fahrgemeinschaftsapp\Userlist\{newUserName}.csv");
                if (!fi.Exists)
                {
                    values[0] = newUserName;
                    string newLine = $"{values[0]};{values[1]};{values[2]};{values[3]};{values[4]};{values[5]};{values[6]};{values[7]};{values[8]}";
                    using (var writer = new StreamWriter($@"C:\010Projects\019 Fahrgemeinschaft\Fahrgemeinschaftsapp\Userlist\{userName}.csv"))
                    {                       

                        writer.WriteLine(newLine);
                    }
                    DirectoryInfo di = new DirectoryInfo($@"C:\010Projects\019 Fahrgemeinschaft\Fahrgemeinschaftsapp\Carpools\");
                    FileInfo[] Files = di.GetFiles();

                    foreach (FileInfo file in Files)
                    {
                        string content = File.ReadAllText($@"C:\010Projects\019 Fahrgemeinschaft\Fahrgemeinschaftsapp\Carpools\{file}");
                        if (content.Contains($",{userName},"))
                        {
                            string[] carpoolInfos = content.Split(';');
                            for (int i = 0; i < carpoolInfos.Length; i++)
                            {
                                if (carpoolInfos[i] == $",{userName},")
                                {
                                    carpoolInfos[i] = $",{newUserName},";
                                }
                            }
                            string newInfo = string.Empty;
                            foreach (string info in carpoolInfos)
                            {
                                newInfo += $";{info}";
                            }
                            using(StreamWriter writer = new StreamWriter($@"C:\010Projects\019 Fahrgemeinschaft\Fahrgemeinschaftsapp\Carpools\{file}"))
                            {
                                writer.Write(newInfo);
                            }                            
                        }
                    }
                    string oldPath = $@"C:\010Projects\019 Fahrgemeinschaft\Fahrgemeinschaftsapp\Userlist\{userName}.csv";
                    string newPath = $@"C:\010Projects\019 Fahrgemeinschaft\Fahrgemeinschaftsapp\Userlist\{newUserName}.csv";

                    File.Copy(oldPath, newPath, true);
                    File.Delete(oldPath);                    
                    return newUserName;
                }
                else
                {
                    Console.WriteLine("This user already exists");
                    Console.WriteLine("Please choose another one");
                    goto TryAgain;
                }              
            }
            else
            {                
                Console.Clear();
                Console.WriteLine("The password was incorrect");
                Console.WriteLine("Redirecting to menu ...");
                Thread.Sleep(1000);
                return userName;
            }
        }

        public static void PrintAllUsers(string userName) //all registered users are shown in a list
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
                string text = File.ReadAllText($@"C:\010Projects\019 Fahrgemeinschaft\Fahrgemeinschaftsapp\Userlist\{userName}.csv");
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

        public static void PrintAllDrivers(string userName) //all registered users, who are allowed to drive are shown in a list
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
                foreach (string fileUser in Directory.EnumerateFiles($@"C:\010Projects\019 Fahrgemeinschaft\Fahrgemeinschaftsapp\Userlist\\\\"))
                {
                    using (var reader = new StreamReader(fileUser))
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
                string text = File.ReadAllText($@"C:\010Projects\019 Fahrgemeinschaft\Fahrgemeinschaftsapp\Userlist\{userName}.csv");
                string[] info = text.Split(';');
                Console.Clear();
                Console.WriteLine($"Here are all drivers, who have {info[6]} as destination");
                Console.WriteLine("------------------------------------------------- ");
                foreach (string fileUser in Directory.EnumerateFiles($@"C:\010Projects\019 Fahrgemeinschaft\Fahrgemeinschaftsapp\Userlist\\\\"))
                {                                       
                    using (var reader = new StreamReader(fileUser))
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

        public static string PrintUserInfo(string userName) //all the information about the user is shown in a list
        {
            Console.Clear();
            Console.WriteLine("Here are all the information we saved about you:");
            Console.WriteLine("------------------------------------------------- ");

            using (var reader = new StreamReader($@"C:\010Projects\019 Fahrgemeinschaft\Fahrgemeinschaftsapp\Userlist\{userName}.csv"))
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
                userName = EditUserInfo(userName);
            }
            else if (input == "2")
            {
                ChangeUserPassword(userName);
            }
            else
            {
                Console.Clear();
            }
            return userName;
        }

        public static string EditUserInfo(string userName) //Allows the user to edit his/her information
        {
        Edit:
            Console.Clear();
            Console.WriteLine("Which information do you want to edit?");
            Console.WriteLine("[1] Username");
            Console.WriteLine("[2] First name");
            Console.WriteLine("[3] Last name");
            Console.WriteLine("[4] Age");
            Console.WriteLine("[5] Gender");
            Console.WriteLine("[6] Start place");
            Console.WriteLine("[7] Destination");
            Console.WriteLine("[8] is driver?");
            Console.WriteLine("-------------------------------------------------");
            Console.WriteLine("[9] Go back");
            Console.WriteLine("[0] Go to menu");

            string input = Console.ReadLine();

            var fileText = File.ReadAllText($@"C:\010Projects\019 Fahrgemeinschaft\Fahrgemeinschaftsapp\Userlist\{userName}.csv");
            string[] values = fileText.Split(';');
            Console.Clear();
            bool goToMenu = false;
            bool goBack = false;
            switch (input)
            {
                case "1":
                    userName = ChangeUserName(userName);
                    values[0] = userName;
                    break;
                case "2":
                    Console.WriteLine("Please enter your new first name");
                    values[1] = Console.ReadLine();
                    break;
                case "3":
                    Console.WriteLine("Please enter your new last name");
                    values[2] = Console.ReadLine();
                    break;
                case "4":
                    Console.WriteLine("Please enter your new age");
                    values[3] = Console.ReadLine();
                    break;
                case "5":
                    Console.WriteLine("Please enter your new gender");
                    values[4] = Console.ReadLine();
                    break;
                case "6":
                    Console.WriteLine("Please enter your new start place");
                    values[5] = Console.ReadLine();
                    break;
                case "7":
                    Console.WriteLine("Please enter your new destination");
                    values[6] = Console.ReadLine();
                    break;
                case "8":
                    Console.WriteLine("Please enter your new driving status (y/n)");
                    string canDrive = Console.ReadLine();
                    if (canDrive == "y")
                    {
                        values[8] = "TRUE";                                             
                    }
                    else if (canDrive == "n")
                    {
                        values[8] = "FALSE";
                    }                   
                    break;
                case "9":
                    goBack = true;
                    break;
                default:
                    goToMenu = true;
                    break;
            }
            using (StreamWriter writer = new StreamWriter($@"C:\010Projects\019 Fahrgemeinschaft\Fahrgemeinschaftsapp\Userlist\{userName}.csv"))
            {
                writer.WriteLine($"{values[0]};{values[1]};{values[2]};{values[3]};{values[4]};{values[5]};{values[6]};{values[7]};{values[8]}");
            }
            if (goToMenu)
            {
                Console.Clear();
                Console.WriteLine("Redirecting to menu ...");
                Thread.Sleep(750);
            }
            else if (goBack)
            {
                userName = PrintUserInfo(userName);
            }
            else
            {
                Console.Clear();
                Console.WriteLine("The information got saved in our database");
                Thread.Sleep(750);
                goto Edit;
            }
            return userName;
        }

        public static void CreateCarpool(List<Carpool> carpool, string userName) //the user can create a carpool and add other users
        {
            DirectoryInfo di = new DirectoryInfo($@"C:\010Projects\019 Fahrgemeinschaft\Fahrgemeinschaftsapp\Carpools");
            //Collecting the neccessary info
            Console.Clear();
            Console.WriteLine("How many seats does the car have?");
            int passengerCount = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine(" ");
            Console.WriteLine("Whats the destination?");
            string destination = Console.ReadLine();
            Console.WriteLine(" ");
            Console.WriteLine("Where do you plan to start?");
            string startLoc = Console.ReadLine();
            Console.WriteLine("");
            Console.WriteLine("When do you plan to start?");
            DateTime departure = Convert.ToDateTime(Console.ReadLine());
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
                else if(dude == userName)
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
            string carpoolPassword = HashPassword(HidePassword());
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

        public static void ViewYourCarpools(List<Carpool> carpool, string userName) //the user gets to see all carpools, where he/she is a member of
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
                        for(int i = 6; i < 6 + Convert.ToInt32(values[5]); i++)
                        {
                            //values[i] = values[i].Trim(',');
                            
                            if (values[i].Equals($",{userName},"))
                            {
                                values[i] = "You";
                                values[i].Replace("\r\n", string.Empty);
                            }
                            values[i].Replace(",\r\n", string.Empty);
                            Console.WriteLine($"{values[i].Replace("\r\n", string.Empty).Trim(',')}");
                        }
                        
                    }
                }  
            }
            Console.WriteLine(" ");
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
                case"3":
                    LeaveCarpool(carpool, userName);
                    break;
                default:
                    break; //redirecting to menu                        
            }
        }

        public static void JoinCarpool(List<Carpool> carpool, string userName)
        {
            Console.Clear();
            Console.WriteLine("Enter the ID of the Carpool you want to join");
            string fileName = Console.ReadLine();
            Console.WriteLine("Please enter the password of this carpool");
            string carpoolPassword = HidePassword();
            FileInfo fi = new FileInfo($@"C:\010Projects\019 Fahrgemeinschaft\Fahrgemeinschaftsapp\Carpools\{fileName}.csv");
            if (fi.Exists)
            {
                if (ValidateCarpoolPassword(fileName, carpoolPassword))
                {
                    string text = File.ReadAllText($@"C:\010Projects\019 Fahrgemeinschaft\Fahrgemeinschaftsapp\Carpools\{fileName}.csv").Replace("\r\n", string.Empty);
                    string[] values = text.Split(';');
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

        public static void LeaveCarpool(List<Carpool> carpool, string userName) //the user can choose, which carpool he/she wants to leave
        {
            Console.Clear();
            Console.WriteLine("Enter the ID of the Carpool you want to leave");
            string fileName = Console.ReadLine();
            Console.WriteLine("Please enter the carpool password");
            string userPw = HidePassword();
            
            FileInfo fi = new FileInfo($@"C:\010Projects\019 Fahrgemeinschaft\Fahrgemeinschaftsapp\Carpools\{fileName}.csv");
            if (fi.Exists)
            {
                if (ValidateCarpoolPassword(fileName, userPw))
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
                                if(value != string.Empty)
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
            if(input == "1")
            {
                ViewYourCarpools(carpool, userName);
            }
        }

        public static void DeleteCarpool(string userName) //by entering the ID, the user can delete an existing carpool
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

        public static void DeleteAccount(string userName) //the user can delete his account and all carpools he/she is a part of
        {
            bool loggenIn = true;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Clear();
            Console.WriteLine("Please enter your password");
            string inputPw = HidePassword();
            var fileText = File.ReadAllText($@"C:\010Projects\019 Fahrgemeinschaft\Fahrgemeinschaftsapp\Userlist\{userName}.csv");
            string[] values = fileText.Split(';');
            string userPw = values[7].Replace("\r\n", string.Empty);

            if (ValidateUserPassword(userName, inputPw))
            {
                Console.WriteLine("Do you really want to delete your account? (y/n)");
                Console.WriteLine("This action is irreversible and all carpool you're a part of will get deleted");
                string confirmation = Console.ReadLine();
                if (confirmation == "y")
                {
                    File.Delete($@"C:\010Projects\019 Fahrgemeinschaft\Fahrgemeinschaftsapp\Userlist\{userName}.csv");

                    DirectoryInfo di = new DirectoryInfo($@"C:\010Projects\019 Fahrgemeinschaft\Fahrgemeinschaftsapp\Carpools\");
                    FileInfo[] Files = di.GetFiles();
                    string[] content = new string[8];
                    foreach (FileInfo file in Files)
                    {
                        string text = File.ReadAllText($@"C:\010Projects\019 Fahrgemeinschaft\Fahrgemeinschaftsapp\Carpools\{file}");
                        if (text.Contains($",{userName},"))
                        {
                            string test = File.ReadAllText($@"C:\010Projects\019 Fahrgemeinschaft\Fahrgemeinschaftsapp\Carpools\{file}.csv");
                            content = text.Split(';');
                            var newLine = string.Empty;

                            foreach (string info in content)
                            {
                                if (info != $",{userName},")
                                {
                                    newLine += $"{info};";
                                }
                            }
                            using (StreamWriter writer = new StreamWriter($@"C:\010Projects\019 Fahrgemeinschaft\Fahrgemeinschaftsapp\Carpools\{file}"))
                            {
                                writer.Write(newLine);
                            }
                        }
                    }
                    Console.Clear();
                    Console.WriteLine("Your account got deleted");
                    Console.ForegroundColor = ConsoleColor.White;
                    Thread.Sleep(2000);
                    Environment.Exit(0);
                    //return loggenIn;
                }
                else
                {
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine("Redirecting to menu ...");
                    Thread.Sleep(1000);
                    //return loggenIn;
                }
            }
            else
            {
                Console.Clear();
                Console.WriteLine("The password was not correct");
                Console.WriteLine("Redirecting to menu ...");
                //return loggenIn;
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
        public static string HashPassword(string password) //hashes the password so it is not stored in plain text
        {
            byte[] tmpSource = ASCIIEncoding.ASCII.GetBytes(password);
            byte[] tmpHash = new MD5CryptoServiceProvider().ComputeHash(tmpSource);

            int i;
            StringBuilder sOutput = new StringBuilder(tmpHash.Length);
            for (i = 0; i < tmpHash.Length; i++)
            {
                sOutput.Append(tmpHash[i].ToString("X2"));
            }
            return sOutput.ToString();
        }
        public static bool ValidateUserPassword(string userName,string inputPw) //checks if user input equals password
        {
            string tmpNewHash = HashPassword(inputPw);
            bool bEqual = false;
            string tmpHash;
            using (StreamReader sr = new StreamReader($@"C:\010Projects\019 Fahrgemeinschaft\Fahrgemeinschaftsapp\Userlist\{userName}.csv"))
            {
                var line = sr.ReadLine();
                var values = line.Split(';');
                tmpHash = values[7];
                tmpHash = tmpHash.Replace("\r\n", string.Empty);
            }
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
        public static bool ValidateCarpoolPassword(string cpID, string inputPw) //checks if user input equals password
        {
            string tmpNewHash = HashPassword(inputPw);
            bool bEqual = false;
            string tmpHash;
            using (StreamReader sr = new StreamReader($@"C:\010Projects\019 Fahrgemeinschaft\Fahrgemeinschaftsapp\Carpools\{cpID}.csv"))
            {
                var line = sr.ReadLine();
                var values = line.Split(';');
                tmpHash = values[4];
                tmpHash = tmpHash.Replace("\r\n", string.Empty);
            }
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