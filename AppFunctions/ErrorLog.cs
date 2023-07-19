using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Globalization;
using ClientDB.AppFunctions;

namespace ClientDB.AppFunctions
{
    public class errorLog
    {
        public static string strPath = AppDomain.CurrentDomain.BaseDirectory;
        public static string strLogFilePath = strPath + @".\log.html";


        public void WriteToLog(string msg)
        {
            try
            {
                if (!File.Exists(strLogFilePath))
                {
                    File.Create(strLogFilePath).Close();
                }
                using (StreamWriter w = File.AppendText(strLogFilePath))
                {
                    // w.WriteLine("\r<div classLog: ");
                    w.WriteLine("<div class='dateDiv'>{0}</div>", DateTime.Now.ToString(CultureInfo.InvariantCulture));
                    string err = "<div class='errorDiv'>" + msg + "</div>";
                    w.WriteLine(err);
                    w.Flush();
                    w.Close();
                }
            }
            catch
            {

            }

        }

        public string ReadLogFile()
        {
            try
            {
                string filePath = strLogFilePath;//string.Concat(Path.Combine(_templateDirectory, templateName), ".txt");

                StreamReader sr = new StreamReader(filePath);
                string body = sr.ReadToEnd();
                sr.Close();
                return body;
            }
            catch (Exception exp)
            {
                WriteToLog(exp.ToString());
                return "Error";
            }
        }



    }
}