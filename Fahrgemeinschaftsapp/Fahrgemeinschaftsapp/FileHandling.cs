using DocumentFormat.OpenXml.Drawing.Charts;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TecAlliance.Carpool.Models
{
    public class FileHandling
    {

        /// <summary>
        /// reads the content of a file and returns an array
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public   static string[] ReadCreateArray(string path)
        {
            string[] values = null;
            using (var reader = new StreamReader(path))
            {
                var line = reader.ReadLine().Replace("\r\n", string.Empty);
                values = line.Split(';');
            }
            return values;
        }
    }    
}