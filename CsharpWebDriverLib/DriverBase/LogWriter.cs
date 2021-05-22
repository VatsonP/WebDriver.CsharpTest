using System;
using System.IO;

namespace CsharpWebDriverLib
{
    internal class LogWriter
    {
        private DirectoryInfo BaseLogFolder { get; set; }
        private DirectoryInfo CurLogFolder  { get; set; }
        private string CurrentTestName { get; set; }
        private string CurrentFileName { get; set; }

        private void Constructor(DirectoryInfo baseLogFolder, string currentTestName, string currentFileName)
        {
            BaseLogFolder = baseLogFolder;
            CurrentTestName = currentTestName;
            CurrentFileName = currentFileName;

            CurLogFolder = Directory.CreateDirectory(Path.Combine(BaseLogFolder.FullName, CurrentTestName));
        }

        internal LogWriter(DirectoryInfo curLogFolder, string currentTestName)
        {
            Constructor(curLogFolder, currentTestName, currentTestName);
        }
        internal LogWriter(DirectoryInfo curLogFolder, string currentTestName, string currentFileName)
        {
            Constructor(curLogFolder, currentTestName, currentFileName);
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
    }

}
