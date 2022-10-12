using System;
using System.Collections.Generic;
using System.IO.Pipes;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Fahrgemeinschaftsapp
{
    public class InputCheck
    {

        /// <summary>
        /// Checks if input is only letters
        /// </summary>
        /// <param name="question"></param>
        /// <returns></returns>
        public static string StringCheck(string question)
        {
            string answer;
            do
            {
                Console.WriteLine(question);
                answer = Console.ReadLine();
            } while (answer.Any(c => !char.IsLetter(c)) || answer == "");
            return answer;
        }

        /// <summary>
        /// Checks if input is only digits
        /// </summary>
        /// <param name="question"></param>
        /// <returns></returns>
        public static int NumberCheck(string question)
        {
            string answer;
            do
            {
                Console.WriteLine(question);
                answer = Console.ReadLine();
            } while (answer.Any(c => !char.IsDigit(c)) || answer == "");
            return Convert.ToInt32(answer);
        }

        /// <summary>
        /// Checks if input is m,f or d
        /// </summary>
        /// <returns></returns>
        public static string GenderCheck()
        {
            string answer;
            do
            {
                Console.WriteLine("Whats your gender? (m/f/d)");
                answer = Console.ReadLine();
                if (answer == "m" || answer == "f" || answer == "f")
                {
                    break;
                }
            } while (true);
            return answer;
        }

        /// <summary>
        /// Checks if input is y or n
        /// </summary>
        /// <returns></returns>
        public static string YesOrNoCheck(string question)
        {
            string answer;
            do
            {
                Console.WriteLine(question);
                answer = Console.ReadLine();
                if (answer == "y" || answer == "n")
                {
                    break;
                }
            } while (true);
            return answer;
        }

        /// <summary>
        /// Checks if input is a valid time
        /// </summary>
        /// <param name="question"></param>
        /// <returns></returns>
        public static DateTime ValidTimeCheck(string question)
        {
            string answer;
            Regex checktime =
                    new Regex(@"^(?: 0?[0 - 9] | 1[0 - 9] | 2[0 - 3]):[0 - 5][0 - 9]$");
            do
            {
                Console.WriteLine(question);
                answer= Console.ReadLine();                
            }while (!checktime.IsMatch(answer));
            return Convert.ToDateTime(answer);
        }
    }
}
