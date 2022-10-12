using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Fahrgemeinschaftsapp
{
    public class User
    {
            public string Username { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public int Age { get; set; }
            public string Gender { get; set; }
            public string StartPlace { get; set; }
            public string EndPlace { get; set; }
            public bool HasCar { get; set; }

        public User(string username, string   firstname, string lastname, int age, string gender, string startplace, string endplace, bool hascar)
        {
            Username = username;
            FirstName = firstname;
            LastName = lastname;
            Age = age;
            Gender = gender;
            StartPlace = startplace;
            EndPlace = endplace;
            HasCar = hascar;
        }

        /// <summary>
        /// by entering his/her username and password the user gains access to the menu
        /// </summary>
        /// <param name="userList"></param>
        /// <returns></returns>
        public static string LogIn(List<User> userList) 
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
                    string inputPassword = Program.HidePassword();

                    if (!ValidatePassword(userName, inputPassword))
                    {
                        Console.WriteLine("Username or password is incorrect. Please the again!");
                        Thread.Sleep(750);
                        Console.Clear();
                        goto LoggingIn;
                    }
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

        /// <summary>
        /// Creating .csv File and setting username and password 
        /// </summary>
        /// <param name="userList"></param>
        /// <returns></returns>
        public static string Registration(List<User> userList) 
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
                    string pw = Program.HashPassword(Program.HidePassword());
                    Console.WriteLine("Confirm the password");
                    string pwConfirm = Program.HashPassword(Program.HidePassword());
                    do
                    {
                        if (pw == pwConfirm)
                        {
                            using (StreamWriter writer = new StreamWriter($@"C:\010Projects\019 Fahrgemeinschaft\Fahrgemeinschaftsapp\Userlist\{userName}.csv"))
                                writer.WriteLine(Program.HashPassword(pw));
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

            CollectInfo(userName, userList);
            return userName;
        }

        /// <summary>
        /// Adding info to userspecific file
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="userList"></param>
        public static void CollectInfo(string userName, List<User> userList)
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
            string hasCarString;// = string.Empty;
            if ((firstName != "Marcello") && (lastName != "Greulich"))
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

        /// <summary>
        /// by entering the old and new password, the user can change his/her password
        /// </summary>
        /// <param name="userName"></param>
        public static void ChangePassword(string userName)
        {
        ChangePW:
            Console.Clear();
            Console.WriteLine("Please enter your old password");
            string inputPw = Program.HidePassword();

            if (ValidatePassword(userName, inputPw))
            {
                Console.WriteLine("Please enter your new password");
                string newPw = Program.HidePassword();
                if (ValidatePassword(userName, newPw))
                {
                    Console.WriteLine("Your new password cant be your old password!");
                    Thread.Sleep(1000);
                    goto ChangePW;
                }
                Console.WriteLine("Please confirm your new password");
                string confirmNewPw = Program.HidePassword();
                if (confirmNewPw == newPw)
                {
                    string[] values = FileHandling.Read($@"C:\010Projects\019 Fahrgemeinschaft\Fahrgemeinschaftsapp\Userlist\{userName}.csv");
                    values[7] = Program.HashPassword(newPw);
                    
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
            else if (!ValidatePassword(userName, inputPw))
            {
                Console.WriteLine("The password was incorrect, please try again");
                Thread.Sleep(1500);
                Console.Clear();
                goto ChangePW;
            }
        }

        /// <summary>
        /// the user can change his username by entering his password
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public static string ChangeUserName(string userName)
        {
            string[] values = FileHandling.Read($@"C:\010Projects\019 Fahrgemeinschaft\Fahrgemeinschaftsapp\Userlist\{userName}.csv");
            
            Console.WriteLine("Please enter your password");
            string inputPw = Program.HidePassword();

            if (ValidatePassword(userName, inputPw))
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
                            using (StreamWriter writer = new StreamWriter($@"C:\010Projects\019 Fahrgemeinschaft\Fahrgemeinschaftsapp\Carpools\{file}"))
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

        /// <summary>
        /// all registered users are shown in a list
        /// </summary>
        /// <param name="userName"></param>
        public static void PrintAll(string userName)
        {
            Console.Clear();
            Console.WriteLine("[1] Show all users");
            Console.WriteLine("[2] Only show users with the same destination");
            string input = Console.ReadLine();

            if (input == "1")
            {
                Console.Clear();
                Console.WriteLine("Here are all users, who are currently looking for a carpool");
                Console.WriteLine("------------------------------------------------- ");

                foreach (string file in Directory.EnumerateFiles($@"C:\010Projects\019 Fahrgemeinschaft\Fahrgemeinschaftsapp\Userlist\\\\"))
                {
                    string[] values = FileHandling.Read(file);
                    Console.WriteLine($"{values[1]} ({values[4]}/{values[3]}), Start: {values[5]}, Destination: {values[6]}");
                    
                }
            }
            else if (input == "2")
            {
                string text = File.ReadAllText($@"C:\010Projects\019 Fahrgemeinschaft\Fahrgemeinschaftsapp\Userlist\{userName}.csv");
                string[] info = text.Split(';');
                Console.Clear();
                Console.WriteLine($"Here are all users, who have {info[6]} as destination");
                Console.WriteLine("------------------------------------------------- ");

                foreach (string file in Directory.EnumerateFiles($@"C:\010Projects\019 Fahrgemeinschaft\Fahrgemeinschaftsapp\Userlist\\\\"))
                {
                    string[] values = FileHandling.Read(file);
                    
                    if ((info[6] == values[6]) && (info[0] != values[0]))
                    {
                        Console.WriteLine($"{values[1]} ({values[4]}/{values[3]}), Start: {values[5]}, Destination: {values[6]}");
                    }
                }
            }
            Console.WriteLine("------------------------------------------------- ");
            Console.WriteLine("[1] Go back to menu");
            Console.ReadLine();
        }

        /// <summary>
        /// all registered users, who are allowed to drive are shown in a list
        /// </summary>
        /// <param name="userName"></param>
        public static void PrintDrivers(string userName)
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
                    string[] values = FileHandling.Read(fileUser);

                        if (values[8] == "TRUE")
                        {
                            Console.WriteLine($"{values[1]} ({values[4]}/{values[3]}), Start: {values[5]}, Destination: {values[6]}");
                        }
                    
                }
            }
            else if (input == "2")
            {
                string[] info = FileHandling.Read($@"C:\010Projects\019 Fahrgemeinschaft\Fahrgemeinschaftsapp\Userlist\{userName}.csv");
                Console.Clear();
                Console.WriteLine($"Here are all drivers, who have {info[6]} as destination");
                Console.WriteLine("------------------------------------------------- ");
                foreach (string fileUser in Directory.EnumerateFiles($@"C:\010Projects\019 Fahrgemeinschaft\Fahrgemeinschaftsapp\Userlist\\\\"))
                {

                        string[] values = FileHandling.Read(fileUser);
                        if ((info[6] == values[6]) && (info[0] != values[0]) && (values[8] == "TRUE"))
                        {
                            Console.WriteLine($"{values[1]} ({values[4]}/{values[3]}), Start: {values[5]}, Destination: {values[6]}");
                        }
                    
                }
            }
            Console.WriteLine("------------------------------------------------- ");
            Console.WriteLine("[1] Go back to menu");
            Console.ReadLine();
        }

        /// <summary>
        /// all the information about the user is shown in a list
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public static string PrintInfo(string userName)
        {
            Console.Clear();
            Console.WriteLine("Here are all the information we saved about you:");
            Console.WriteLine("------------------------------------------------- ");

        string[] values = FileHandling.Read($@"C:\010Projects\019 Fahrgemeinschaft\Fahrgemeinschaftsapp\Userlist\{userName}.csv");

                Console.WriteLine($"Username:       {values[0]}");
                Console.WriteLine($"First name:     {values[1]}");
                Console.WriteLine($"Last name:      {values[2]}");
                Console.WriteLine($"Age:            {values[3]}");
                Console.WriteLine($"Gender:         {values[4]}");
                Console.WriteLine($"Start Place     {values[5]}");
                Console.WriteLine($"Destination:    {values[6]}");
                Console.WriteLine($"Is a driver:    {values[8]}");
            
            Console.WriteLine("------------------------------------------------- ");
            Console.WriteLine("[1] Edit the Information");
            Console.WriteLine("[2] Change your password");
            Console.WriteLine("[3] Go back to menu");
            string input = Console.ReadLine();
            if (input == "1")
            {
                userName = EditInfo(userName);
            }
            else if (input == "2")
            {
                ChangePassword(userName);
            }
            else
            {
                Console.Clear();
            }
            return userName;
        }

        /// <summary>
        /// Allows the user to edit his/her information
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public static string EditInfo(string userName)
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

            string[] values = FileHandling.Read($@"C:\010Projects\019 Fahrgemeinschaft\Fahrgemeinschaftsapp\Userlist\{userName}.csv");
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
                userName = PrintInfo(userName);
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

        /// <summary>
        /// the user can delete his account and all carpools he/she is a part of
        /// </summary>
        /// <param name="userName"></param>
        public static void DeleteAccount(string userName)
        {
            //bool loggenIn = true;    
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Clear();
            Console.WriteLine("Please enter your password");
            string inputPw = Program.HidePassword();
            string[] values = FileHandling.Read($@"C:\010Projects\019 Fahrgemeinschaft\Fahrgemeinschaftsapp\Userlist\{userName}.csv");
            string userPw = values[7].Replace("\r\n", string.Empty);

            if (ValidatePassword(userName, inputPw))
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

        /// <summary>
        /// checks if user input equals the password of the account
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="inputPw"></param>
        /// <returns></returns>
        public static bool ValidatePassword(string userName, string inputPw)
        {
            string tmpNewHash = Program.HashPassword(inputPw);
            bool bEqual = false;
            string tmpHash;

           
                string[] values = FileHandling.Read($@"C:\010Projects\019 Fahrgemeinschaft\Fahrgemeinschaftsapp\Userlist\{userName}.csv");
                tmpHash = values[7];
                tmpHash = tmpHash.Replace("\r\n", string.Empty);
           
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
