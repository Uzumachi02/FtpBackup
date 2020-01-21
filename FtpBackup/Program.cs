using System;
using System.Collections.Generic;
using System.Linq;
using CommandLine;

namespace FtpBackup {
  class Program {
    static void Main(string[] args) {
      Parser.Default.ParseArguments<CommandOptions>(args)
        .WithParsed(RunOptions);
    }

    static void RunOptions(CommandOptions options) {
      if( options.BackupFolder.Contains("{now}") ) {
        options.BackupFolder = options.BackupFolder.Replace("{now}", DateTime.Now.ToString(options.FormatDate));
      }

      options.ToConsole();

      var app = new App(options);
      try {
        app.Run();
      } catch( Exception e ) {
        Logger.Error(e);
      }

      string logPath = System.IO.Path.Combine(options.BackupFolder, "_logBackup.log");
      Logger.SaveToFile(logPath);
      Logger.Info("Log file is save to {0}", logPath);
    }
  }
}
