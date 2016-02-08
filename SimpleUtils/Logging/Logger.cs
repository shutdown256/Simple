using System;
using System.IO;
using System.Threading.Tasks;

namespace Logging {
    public class Logger {
        private static object monitor = new object();
        private static Task task = Task.Run(() => { });

        public static void Info(string msg) {
            AddMsg(msg, LogType.Info);
        }

        public static void Success(string msg) {
            AddMsg(msg, LogType.Success);
        }

        public static void Warning(string msg) {
            AddMsg(msg, LogType.Warning);
        }

        public static void Error(string msg) {
            AddMsg(msg, LogType.Error);
        }

        public static void Error(string msg, Exception ex) {
            AddMsg($"{msg}: {ex}", LogType.Error);
        }

        //public static void ToFile(string msg) {
        //    lock (monitor) task = task.ContinueWith(t => {
        //        //lock (monitor) {
        //        try {
        //            if (!Directory.Exists("Logs")) Directory.CreateDirectory("Logs");
        //            using (StreamWriter sw = File.AppendText($"Logs{Environment.NewLine}{DateTime.Now.ToString("dd-MM-yyyy")}.txt")) {
        //                sw.WriteLine($"[{DateTime.Now.ToString("HH:mm:ss")}]<f> {msg}");
        //            }
        //        } catch { }
        //        //}
        //    }, TaskContinuationOptions.ExecuteSynchronously);
        //}

        private static void AddMsg(string msg, LogType type) {

            //StackFrame frame = new StackFrame(2);
            //frame.ToString()
            //var method = frame.GetMethod();
            //cn = method.DeclaringType.Name;
            //msg = cn + " >> " + msg;
            lock (monitor) task = task.ContinueWith(t => {
                try {
                    Console.ForegroundColor = GetConsoleColor(type);
                    Console.WriteLine($"[{DateTime.Now.ToString("HH:mm:ss")}] {msg}");
                    Console.ResetColor();
                    if (!Directory.Exists("Logs")) Directory.CreateDirectory("Logs");
                    using (StreamWriter sw = File.AppendText($"Logs{Environment.NewLine}{DateTime.Now.ToString("dd-MM-yyyy")}.txt")) {
                        sw.WriteLine($"[{DateTime.Now.ToString("HH:mm:ss")}]<{GetTypeTag(type)}> {msg}");
                    }
                } catch { }
            }, TaskContinuationOptions.ExecuteSynchronously);
        }

        public static ConsoleColor GetConsoleColor(LogType t) {
            switch (t) {
                case LogType.Info: return ConsoleColor.White;
                case LogType.Success: return ConsoleColor.Green;
                case LogType.Warning: return ConsoleColor.Yellow;
                case LogType.Error: return ConsoleColor.Red;
                default: return ConsoleColor.Gray;
            }
        }

        public static string GetTypeTag(LogType t) {
            switch (t) {
                case LogType.Info: return "I";
                case LogType.Success: return "S";
                case LogType.Warning: return "W";
                case LogType.Error: return "E";
                default: return "-";
            }
        }

        public enum LogType { Info, Success, Warning, Error }
    }
}
