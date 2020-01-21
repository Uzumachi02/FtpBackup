using System;

namespace FtpBackup {

  static class ConsoleMessage {

    public static void Info(object obj) {
      Console.ForegroundColor = ConsoleColor.Cyan;
      Console.WriteLine(obj);
      Console.ResetColor();
    }

    public static void Success(object obj) {
      Console.ForegroundColor = ConsoleColor.Green;
      Console.WriteLine(obj);
      Console.ResetColor();
    }

    public static void Warn(object obj) {
      Console.ForegroundColor = ConsoleColor.Yellow;
      Console.WriteLine(obj);
      Console.ResetColor();
    }

    public static void Error(object obj) {
      Console.ForegroundColor = ConsoleColor.Red;
      Console.WriteLine(obj);
      Console.ResetColor();
    }
  }
}
