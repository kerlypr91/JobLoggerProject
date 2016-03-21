using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JLogger
{
    public class JobLogger
    {
        private static bool _logToFile;
        private static bool _logToConsole;
        private static bool _logToDatabase;

        private static bool _logMessage;
        private static bool _logWarning;
        private static bool _logError;

        private static int _tipoLog = 0;

        private enum TipoLog : int { Message = 1, Error = 2, Warning = 3 };

        private bool _initialized;

        public JobLogger(bool logToFile, bool logToConsole, bool logToDatabase, bool logMessage, bool logWarning, bool logError)
        {
            _logError = logError;
            _logMessage = logMessage;
            _logWarning = logWarning;
            _logToDatabase = logToDatabase;
            _logToFile = logToFile;
            _logToConsole = logToConsole;
        }

        public static void LogMessage(string sMessage, int tipoLog)
        {
            _tipoLog = tipoLog;

            if (Convert.ToString(sMessage).Trim().Length == 0)
            {
                throw new Exception("No message set");
            }
            if (!_logToConsole && !_logToFile && !_logToDatabase)
            {
                throw new Exception("Invalid configuration");
            }
            if (!_logError && !_logMessage && !_logWarning)
            {
                throw new Exception("Error or Warning or Message Configuration must be set");
            }
            if (!Enum.IsDefined(typeof(TipoLog), _tipoLog))
            {
                throw new Exception("Error or Warning or Message must be specified");
            }

            if (GetsLogged())
            {
                if (_logToDatabase)
                {
                    using (System.Data.SqlClient.SqlConnection connection = new System.Data.SqlClient.SqlConnection(System.Configuration.ConfigurationManager.AppSettings["ConnectionString"]))
                    {
                        connection.Open();
                        System.Data.SqlClient.SqlCommand command = new System.Data.SqlClient.SqlCommand("Insert into Log Values('" + sMessage + "', " + Convert.ToString(_tipoLog) + ")");
                        command.ExecuteNonQuery();
                        connection.Close();
                    }
                }

                if (_logToFile)
                {
                    string l = string.Empty;
                    if (!System.IO.File.Exists(System.Configuration.ConfigurationManager.AppSettings["LogFileDirectory"] + "LogFile" + DateTime.Now.ToShortDateString() + ".txt"))
                    {
                        l = System.IO.File.ReadAllText(System.Configuration.ConfigurationManager.AppSettings["LogFileDirectory"] + "LogFile" + DateTime.Now.ToShortDateString() + ".txt");
                    }
                    l += (DateTime.Now.ToShortDateString() + sMessage);
                    System.IO.File.WriteAllText(System.Configuration.ConfigurationManager.AppSettings["LogFileDirectory"] + "LogFile" + DateTime.Now.ToShortDateString() + ".txt", l);
                }

                if (_logToConsole)
                {
                    Console.ForegroundColor = GetColorConsole();
                    Console.WriteLine(DateTime.Now.ToShortDateString() + sMessage);
                }
            }
        }

        private static ConsoleColor GetColorConsole()
        {
            if (_tipoLog == (int)TipoLog.Message) return ConsoleColor.White;
            if (_tipoLog == (int)TipoLog.Error) return ConsoleColor.Red;
            if (_tipoLog == (int)TipoLog.Warning) return ConsoleColor.Yellow;
            return ConsoleColor.Black;
        }

        private static Boolean GetsLogged()
        {
            if (_tipoLog == (int)TipoLog.Message) return _logMessage;
            if (_tipoLog == (int)TipoLog.Error) return _logError;
            if (_tipoLog == (int)TipoLog.Warning) return _logWarning;
            return false;
        }

    }
}
