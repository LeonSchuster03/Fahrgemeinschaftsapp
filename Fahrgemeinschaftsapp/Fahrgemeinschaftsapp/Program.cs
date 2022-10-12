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
                string userName = User.LogIn(userList);
                

               Home:
                bool loggedIn = true;
                string userInput = Menu(userName);
                switch (userInput)
                {
                    case "1":
                        userName = User.PrintInfo(userName);
                        goto Home;
                    case "2":
                        User.PrintAll(userName);
                        goto Home;
                    case "3":
                        User.PrintAllDrivers(userName);
                        goto Home;
                    case "4":
                        Carpool.CreateCarpool(carPoolList, userName);
                        goto Home;
                    case "5":
                        Carpool.ViewYourCarpools(carPoolList, userName);
                        goto Home;
                    case "6":
                        Carpool.DeleteCarpool(userName);
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

        public static string Settings(string userName) //opens a menu where you can change the username and password or delete the account
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
                    User.ChangeUserName(userName);
                    break;
                case "2":
                    User.ChangePassword(userName);
                    break;
                case "3":
                    User.DeleteAccount(userName);
                    break;
                default:
                    break;
            }   
            return userName;
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
        
    }   
}