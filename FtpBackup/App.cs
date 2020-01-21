using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using FluentFTP;

namespace FtpBackup {

  public class App {
    private CommandOptions Options;
    private FtpClient client;
    private Statistic statistic;

    public App(CommandOptions options) {
      this.Options = options;
      this.statistic = new Statistic();

      CheckDirectory(Options.BackupFolder);
    }


    public void Run() {
      if( !ConnectFtp() ) {
        Logger.Fatal("Ftp not connected!!!");
        return;
      }

      if( !client.DirectoryExists(Options.RemoteFolder) ) {
        Logger.Fatal("Remote folder not found in ftp!!!");
        return;
      }

      client.SetWorkingDirectory(Options.RemoteFolder);
      client.RetryAttempts = Options.CountRetry;

      var files = GetFiles(Options.WorkFolder);
      statistic.TotalFiles = files.Count();

      Logger.Debug("Find {0} files", statistic.TotalFiles);
      Logger.Info("> Start backup");

      var backupFiles = new List<string>();
      foreach( var file in files ) {
        var backupFile = BackupFile(file);
        if( !string.IsNullOrWhiteSpace(backupFile) ) {
          backupFiles.Add(backupFile);
        }
        Logger.EmptyLine();
      }

      Logger.Info("Backup {0} files", backupFiles.Count());
      Logger.Info("</ Endbackup");

      statistic.ToConsole();
      CloseFtp();
    }

    public bool ConnectFtp() {
      client = new FtpClient(Options.FtpHost);
      client.Credentials = new NetworkCredential(Options.FtpUser, Options.FtpPassword);
      client.Connect();
      return client.IsConnected;
    }

    public void CloseFtp() {
      if( client != null && client.IsConnected ) {
        client.Disconnect();
      }
    }


    private List<string> GetFiles(string dirPath) {
      return Directory.GetFiles(dirPath, "*", SearchOption.AllDirectories).ToList();
    }

    private string BackupFile(string filePath) {
      string relativFilePath = GetRelativFilePath(filePath);
      string ftpFilePath = FtpExtensions.GetFtpPath(Options.RemoteFolder, relativFilePath);
      Logger.Debug("fileToBackup: '{0}'", ftpFilePath);

      if( !client.FileExists(ftpFilePath) ) {
        statistic.NotFoundFiles++;
        Logger.Debug("file not found on ftp");
        return null;
      }

      string backupFilePath = Path.Combine(Options.BackupFolder, relativFilePath.TrimStart(Path.DirectorySeparatorChar));
      var ftpLocalExists = Options.OverwriteFile ? FtpLocalExists.Overwrite : FtpLocalExists.Skip;
      if( client.DownloadFile(backupFilePath, ftpFilePath, ftpLocalExists, FtpVerify.Retry | FtpVerify.Throw) ) {
        statistic.BackupFiles++;
        Logger.Debug("backupFilePath: '{0}'", backupFilePath);
      } else {
        statistic.SkipFiles++;
        Logger.Debug("file exist in '{0}'", backupFilePath);
      }

      return filePath;
    }

    private string GetRelativFilePath(string filePath) {
      return filePath.Replace(Options.WorkFolder, "");
    }

    private void CheckDirectory(string dirPath) {
      if( !Directory.Exists(dirPath) ) {
        Directory.CreateDirectory(dirPath);
      }
    }


    ~App() {
      CloseFtp();
    }
  }

  class Statistic {
    public int TotalFiles { get; set; }
    public int BackupFiles { get; set; }
    public int SkipFiles { get; set; }
    public int NotFoundFiles { get; set; }

    public void ToConsole() {
      Logger.EmptyLine();
      Logger.Trace("********************************");
      Logger.Trace("** Statistic");

      foreach( var prop in this.GetType().GetProperties() ) {
        Logger.Trace("* {0}: {1}", prop.Name, prop.GetValue(this, null));
      }

      Logger.Trace("********************************");
      Logger.EmptyLine();
    }
  }
}
