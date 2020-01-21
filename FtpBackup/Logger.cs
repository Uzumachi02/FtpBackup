using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace FtpBackup {

  public static class Logger {
    private const string FILE_EXT = ".log";
    private static bool toConsole = true;
    private static bool toFile = true;
    private static List<string> logs = new List<string>();
    private static string datetimeFormat = "yyyy-MM-dd HH:mm:ss.fff";

    public static string LogFileName { get; set; }


    public static void ConsoleOn() {
      toConsole = true;
    }

    public static void ConsoleOff() {
      toConsole = false;
    }

    public static void FileOn() {
      toFile = true;
    }

    public static void FileOff() {
      toFile = false;
    }

    public static void Log(string text) {
      Info(text);
    }

    public static void Info(string text) {
      WriteFormattedLog(LogLevel.INFO, text);
    }

    public static void Info(string template, params object[] args) {
      WriteFormattedLog(LogLevel.INFO, string.Format(template, args));
    }

    public static void Debug(string text) {
      WriteFormattedLog(LogLevel.DEBUG, text);
    }

    public static void Debug(string template, params object[] args) {
      WriteFormattedLog(LogLevel.DEBUG, string.Format(template, args));
    }

    public static void Warning(string text) {
      WriteFormattedLog(LogLevel.WARNING, text);
    }

    public static void Warning(string template, params object[] args) {
      WriteFormattedLog(LogLevel.WARNING, string.Format(template, args));
    }

    public static void Error(string text) {
      WriteFormattedLog(LogLevel.ERROR, text);
    }

    public static void Error(string template, params object[] args) {
      WriteFormattedLog(LogLevel.ERROR, string.Format(template, args));
    }

    public static void Error(Exception exception) {
      WriteFormattedLog(LogLevel.ERROR, exception.ToString());
    }

    public static void Fatal(string text) {
      WriteFormattedLog(LogLevel.FATAL, text);
    }

    public static void Fatal(string template, params object[] args) {
      WriteFormattedLog(LogLevel.FATAL, string.Format(template, args));
    }

    public static void Trace(string text) {
      WriteFormattedLog(LogLevel.TRACE, text);
    }

    public static void Trace(string template, params object[] args) {
      WriteFormattedLog(LogLevel.TRACE, string.Format(template, args));
    }

    public static void EmptyLine() {
      if( toFile ) {
        logs.Add("");
      }

      if( toConsole ) {
        Console.WriteLine();
      }
    }

    public static void SaveToFile() {
      SaveToFile(LogFileName);
    }

    public static void SaveToFile(string filePath) {
      if( string.IsNullOrWhiteSpace(filePath) ) {
        filePath = string.Format("_{0}{1}", System.Reflection.Assembly.GetExecutingAssembly().GetName().Name, FILE_EXT);
      }

      try {
        using( StreamWriter file = new StreamWriter(filePath) ) {
          foreach( var line in logs ) {
            file.WriteLine(line);
          }
        }
      } catch( Exception ex ) {
        Console.WriteLine(ex);
      }
    }


    private static void WriteFormattedLog(LogLevel level, string text) {
      string pretext = string.Format("[{0}] [{1}]: ", System.DateTime.Now.ToString(datetimeFormat), level);

      string log = pretext + text;
      if( toFile ) {
        logs.Add(log);
      }

      if( toConsole ) {
        switch( level ) {
          case LogLevel.INFO:
            ConsoleMessage.Info(log);
            break;
          case LogLevel.WARNING:
            ConsoleMessage.Warn(log);
            break;
          case LogLevel.ERROR:
            ConsoleMessage.Error(log);
            break;
          case LogLevel.FATAL:
            ConsoleMessage.Error(log);
            break;
          default:
            Console.WriteLine(log);
            break;
        }
      }
    }

    [System.Flags]
    private enum LogLevel {
      TRACE,
      INFO,
      DEBUG,
      WARNING,
      ERROR,
      FATAL
    }
  }
}
