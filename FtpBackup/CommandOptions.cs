using System;
using System.Collections.Generic;
using System.Linq;
using CommandLine;

namespace FtpBackup {

  public class CommandOptions {

    [Option('w', "workfolder", Required = true, HelpText = "Set work folder.")]
    public string WorkFolder { get; set; }

    [Option('b', "backupfolder", Required = true, HelpText = "Set backup folder.")]
    public string BackupFolder { get; set; }

    [Option('r', "remotefolder", Required = true, HelpText = "Set ftp remote folder.")]
    public string RemoteFolder { get; set; }

    [Option('h', "ftphost", Required = true, HelpText = "Set ftp host.")]
    public string FtpHost { get; set; }

    [Option('u', "ftpuser", Required = true, HelpText = "Set ftp user.")]
    public string FtpUser { get; set; }

    [Option('p', "ftppassword", Required = true, HelpText = "Set ftp password.")]
    public string FtpPassword { get; set; }

    [Option('c', "countretry", Default = 1, HelpText = "Set count retry checksum backup file")]
    public int CountRetry { get; set; }

    [Option('o', "overwrite", Default = false, HelpText = "Overwrite file?")]
    public bool OverwriteFile { get; set; }

    [Option('f', "format", Default = "yyyy.MM.dd_HH.mm", HelpText = "Set format date to {now}")]
    public string FormatDate { get; set; }


    public void ToConsole() {
      Logger.EmptyLine();
      Logger.Trace("********************************");

      Logger.Trace("* WorkFolder: {0}", WorkFolder);
      Logger.Trace("* BackupFolder: {0}", BackupFolder);
      Logger.Trace("* RemoteFolder: {0}", RemoteFolder);
      Logger.Trace("* CountRetry: {0}", CountRetry);
      Logger.Trace("* OverwriteFile: {0}", OverwriteFile);
      Logger.Trace("* FormatDate: {0}", FormatDate);

      Logger.Trace("********************************");
      Logger.EmptyLine();
    }
  }
}
