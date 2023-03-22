using System;
using System.IO;
using OpenQA.Selenium;

namespace CsharpWebDriverLib.DriverBase
{
    internal class LogWriter
    {
        private ILogs DriverLogs { get; set; }
        private DirectoryInfo BaseLogFolder { get; set; }
        private DirectoryInfo CurLogFolder  { get; set; }
        private string CurrentTestName { get; set; }
        private string CurrentFileName { get; set; }

        private void Constructor(ILogs driverLogs, DirectoryInfo baseLogFolder, string currentTestName, string currentFileName)
        {
            DriverLogs = driverLogs;
            BaseLogFolder = baseLogFolder;
            CurrentTestName = currentTestName;
            CurrentFileName = currentFileName;

            CurLogFolder = Directory.CreateDirectory(Path.Combine(BaseLogFolder.FullName, CurrentTestName));
        }

        internal LogWriter(ILogs driverLogs, DirectoryInfo curLogFolder, string currentTestName)
        {
            Constructor(driverLogs, curLogFolder, currentTestName, currentTestName);
        }
        internal LogWriter(ILogs driverLogs, DirectoryInfo curLogFolder, string currentTestName, string currentFileName)
        {
            Constructor(driverLogs, curLogFolder, currentTestName, currentFileName);
        }

        internal void LogWrite(string eventName, string logMessage)
        {
            try
            {
                var logFilePathName = Path.Combine(CurLogFolder.FullName, CurrentFileName + ".txt");
                using (StreamWriter w = File.AppendText(logFilePathName))
                {
                    Log(eventName, logMessage, w);
                }
            }
#pragma warning disable CS0168
            catch (Exception ex)
            {
            }
#pragma warning restore CS0168
        }

        internal void Log(string eventName, string logMessage, TextWriter txtWriter)
        {
            try
            {
                txtWriter.Write("\r\nLog Entry : ");
                txtWriter.WriteLine("{0} {1}", DateTime.Now.ToLongTimeString(), DateTime.Now.ToLongDateString());
                txtWriter.WriteLine("event: {0}", eventName);
                txtWriter.WriteLine("     : {0}", logMessage);
                txtWriter.WriteLine("-------------------------------------------------------");
            }
#pragma warning disable CS0168
            catch (Exception ex)
            {
            }
#pragma warning restore CS0168
        }

        internal void FinalLogWrite()
        {
            LogWrite("================================================",
                     "================================================");
        }

        internal void saveCurLogs(string logType)
        {
            try
            {
                if (DriverLogs != null)
                {
                    var browserLogs = DriverLogs.GetLog(logType);
                    if (browserLogs.Count > 0)
                    {
                        foreach (var log in browserLogs)
                        {
                            LogWrite(logType, log.Message);
                        }
                    }
                }
            }
            catch
            {
                //There are no log types present
            }
        }

        internal void saveAllCurLogs()
        {
            saveCurLogs(LogType.Server);
            saveCurLogs(LogType.Browser);
            saveCurLogs(LogType.Client);
            saveCurLogs(LogType.Driver);
            saveCurLogs(LogType.Profiler);
        }
    }

}
