using System;
using SharpDX.Direct3D9;
using System.Management;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Collections.Generic;
using DiscordRPC;
// making comments is fun

namespace AeroCL
{

    class Program
    {

        public DiscordRpcClient client;
        public static string currentDir = "";

        void Initialize()
        {
            try // some discord rich presence stuff probably, doesnt work
            {
                client = new DiscordRpcClient("925429119255216139");

                client.Initialize();

                client.SetPresence(new DiscordRPC.RichPresence()
                {
                    Details = $"AeroCL",
                    State = $"AeroCL",
                    Timestamps = Timestamps.Now,
                    Assets = new Assets()
                    {
                        LargeImageKey = $"image_large",
                        LargeImageText = "AeroCL",
                        SmallImageKey = $"image_small"
                    }
                });
            }
            catch
            {
                Console.WriteLine("Failed to intialize discord rpc");
            }
        }

        static void Main(string[] args)
        {

            Console.BackgroundColor = ConsoleColor.DarkCyan;
            Console.Clear();


            try
            {

                welcomeScreen();

                void welcomeScreen()
                {


                    string currentDir;

                    // some random shit for the specs tree
                    var graphicsCardName = new Direct3D().Adapters[0].Details.Description;
                    ManagementObjectSearcher mos =
                    new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_Processor");
                    string userName = Environment.UserName;



                    Console.WriteLine("==================================================");
                    Console.WriteLine(" AeroCL 1.0 | " + userName);
                    Console.WriteLine("==================================================");
                    Console.WriteLine(" |OS; " + Environment.OSVersion);
                    Console.WriteLine(" |PC; " + Environment.MachineName);
                    Console.WriteLine(" |GPU; " + graphicsCardName);
                    foreach (ManagementObject mo in mos.Get())
                    {
                        Console.WriteLine(" |CPU; " + mo["Name"]);
                    }
                    Console.WriteLine("==================================================");
                    Console.WriteLine("'HELP' For command list");
                    Console.WriteLine("==================================================");
                    cmdvoid();

                    // the welcome screen
                }



                void cmdvoid()
                {
                    // the whole cmd thing, needs to be called after every command to not show the command doesnt exist thing
                    Console.Write("@" + Environment.MachineName + ">");
                    String CMD = Console.ReadLine();

                    if (CMD == "HELP") // help command
                    {
                        Console.WriteLine("");
                        Console.WriteLine("==================================================");
                        Console.WriteLine("Command List");
                        Console.WriteLine("==================================================");
                        Console.WriteLine(" Basics == ");
                        Console.WriteLine("   HELP - this command");
                        Console.WriteLine("   EXIT - exit AeroCL");
                        Console.WriteLine("   SHTDWN - mashine shutdown");
                        Console.WriteLine("   REBOOT - mashine reboot");
                        Console.WriteLine("   ECHO - returns given text");
                        Console.WriteLine("   FETCHINFO - welcome screen");
                        Console.WriteLine("   SETDIR - change current directory");
                        Console.WriteLine(" File Management == ");
                        Console.WriteLine("   CRDIR - create directory at current");
                        Console.WriteLine("   CRFILE - create file at current");
                        Console.WriteLine("   DLDIR - deletes directory at current");
                        Console.WriteLine("   DLFILE - deletes file at current");
                        Console.WriteLine("   LISTDIR - lists contents of current");
                        Console.WriteLine(" Drive utilities == ");
                        Console.WriteLine("   LISTDRIVE - list of logical drives");
                        Console.WriteLine("   LISTUSB - list of removeable usb devices");
                        Console.WriteLine(" Networking == ");
                        Console.WriteLine("   GITCLONE - clones a github repository (https)");
                        Console.WriteLine("==================================================");
                        cmdvoid();
                    }

                    if (CMD == "EXIT") // exits aerocl 
                    {
                        Environment.Exit(0);
                    }

                    if (CMD == "ECHO") // echoes text
                    {
                        string echoText = Console.ReadLine();
                        Console.WriteLine();
                        Console.WriteLine(echoText);
                        cmdvoid();

                    }

                    if (CMD == "BEEP") // beep beep boop boop
                    {

                        Console.Beep();
                        Console.WriteLine("A sound has been sent");
                        cmdvoid();
                    }

                    if (CMD == "SHTDWN") // force shutdowns your pc
                    {
                        Process.Start("shutdown", "/s /t 0");
                    }

                    if (CMD == "REBOOT") // force reboots your pc
                    {
                        Process.Start("shutdown", "/r /t 0");
                    }

                    if (CMD == "FETCHINFO") // welcome screen
                    {
                        welcomeScreen();
                    }

                    if (CMD == "GITCLONE") // clones a github repository to set current directory
                    {
                        if (currentDir == "") // if the current directory hasnt been set display this
                        {
                            Console.WriteLine();
                            Console.WriteLine("Current directory hasnt been set");
                            Console.WriteLine("set it by executing SETDIR");
                            cmdvoid();
                        }

                        Console.WriteLine();
                        Console.WriteLine("Enter .git url to clone");

                        String gitUrl = Console.ReadLine();

                        
                        Console.WriteLine();
                        Console.WriteLine("The repository will be cloned to the set current directory");
                        Console.WriteLine("are you sure? - [y/n]");
                        Console.WriteLine("");
                        String warnDir = Console.ReadLine();

                        if (warnDir == "y")
                        {
                            Console.WriteLine();
                            Console.WriteLine("   ========================================================");
                            Console.WriteLine();
                            Console.WriteLine("    GITCLONE Started");
                            Console.WriteLine();
                            Console.WriteLine("    " + gitUrl + " ..");
                            Console.WriteLine("    if the url is invalid will might fail");
                            Console.WriteLine();
                            Console.Write("    [");
                            Console.Write("=");
                            Thread.Sleep(500);
                            Console.Write("===");

                            try
                            {
                                Console.Write("=======");

                                LibGit2Sharp.Repository.Clone(gitUrl, currentDir);

                                Console.Write("=========================]"); // i didnt know how to make an actual progress bar
                                Console.WriteLine();
                                Console.WriteLine();
                                Console.WriteLine("    Cloning succeded");
                                Console.WriteLine();
                                Console.WriteLine("    Cloned at ; " + currentDir);
                                Console.WriteLine("");
                                Console.WriteLine("   ========================================================");
                                Console.WriteLine();
                                cmdvoid();
                            }
                            catch
                            {
                                Console.WriteLine("   ========================================================");
                                Console.WriteLine("GITCLONE failed");
                                Console.WriteLine("the url is invalid or pc is offline");
                                Console.WriteLine();
                                cmdvoid();
                            }
                        }
                        if (warnDir == "n")
                        {
                            Console.WriteLine("");
                            cmdvoid();
                        }
                        else
                        {
                            Console.WriteLine("");
                            cmdvoid();
                        }

                    }

                    if (CMD == "LISTDRIVE") // lists all logical drives
                    {
                        try
                        {
                            string[] drives = System.IO.Directory.GetLogicalDrives();

                            Console.WriteLine("=============================================================");
                            Console.WriteLine("Logical drives");
                            Console.WriteLine("=============================================================");

                            foreach (string str in drives)
                            {
                                System.Console.WriteLine("| " + str);
                            }
                            Console.WriteLine("=============================================================");
                            cmdvoid();
                        }
                        catch
                        {
                            Console.WriteLine("Unable to get drives");
                            cmdvoid();
                        }
                    }

                    if (CMD == "LISTUSB") // lists usb devices
                    {
                        Console.WriteLine("=============================================================");
                        Console.WriteLine("Removeable drives");
                        Console.WriteLine("=============================================================");

                        foreach (DriveInfo drive in DriveInfo.GetDrives())
                        {
                            if (drive.DriveType == DriveType.Removable)
                            {
                                Console.WriteLine(string.Format("({0}) {1}", drive.Name.Replace("\\", ""), drive.VolumeLabel));
                            }
                            else
                            {
                                Console.WriteLine("No drives found");
                            }
                        }

                        Console.WriteLine("=============================================================");
                        cmdvoid();
                    }

                    if (CMD == "SETDIR") // sets current directory
                    {
                        Console.WriteLine();
                        Console.Write("Set current directory to? > ");

                        string setcurrentDir = Console.ReadLine();

                        if (setcurrentDir.EndsWith(@"\"))
                        {
                            if (Directory.Exists(setcurrentDir))
                            {
                                Console.WriteLine();
                                Console.WriteLine("The current directory is now set to > " + setcurrentDir);
                                currentDir = setcurrentDir;
                                cmdvoid();
                            }

                            if (File.Exists(setcurrentDir))
                            {
                                Console.WriteLine("Given path is a file not a accessable directory");
                                Console.WriteLine();
                                cmdvoid();
                            }

                        }

                        if (setcurrentDir.EndsWith(""))
                        {
                            Console.WriteLine("No backslash at the end of path");
                            Console.WriteLine();
                            cmdvoid();
                        }

                        else
                        {
                            Console.WriteLine("Cant access given directory");
                            Console.WriteLine();
                            cmdvoid();
                        }
                    }

                    if (CMD == "CRDIR") // create a directory
                    {
                        if (currentDir == "")
                        {
                            Console.WriteLine();
                            Console.WriteLine("Directory hasnt been set");
                            Console.WriteLine("Set it by executing SETDIR");
                            cmdvoid();
                        }


                        Console.WriteLine("Directory name?");
                        Console.WriteLine();

                        try
                        {
                            Console.Write(currentDir);
                            string crDir = Console.ReadLine();

                            Directory.CreateDirectory(currentDir + crDir);
                            Console.WriteLine();
                            Console.WriteLine("Directory created at ; " + currentDir + crDir);

                            cmdvoid();
                        }
                        catch
                        {
                            Console.WriteLine();
                            Console.WriteLine("Cant create directory");
                            Console.WriteLine("invalid name or already exists");
                            cmdvoid();
                        }

                    }

                    if (CMD == "CRFILE") // create a file
                    {
                        if (currentDir == "")
                        {
                            Console.WriteLine();
                            Console.WriteLine("Directory hasnt been set");
                            Console.WriteLine("Set it by executing SETDIR");
                            cmdvoid();
                        }

                        Console.WriteLine("File name? (file extension included)");
                        Console.WriteLine();

                        try
                        {
                            Console.Write(currentDir);
                            string crFile = Console.ReadLine();

                            File.Create(currentDir + crFile);
                            Console.WriteLine();
                            Console.WriteLine("File created at ; " + currentDir + crFile);

                            cmdvoid();
                        }
                        catch
                        {
                            Console.WriteLine();
                            Console.WriteLine("Cant create file");
                            Console.WriteLine("invalid name or already exists");
                            cmdvoid();
                        }

                    }

                    if (CMD == "DLFILE") // delete a file
                    {
                        if (currentDir == "")
                        {
                            Console.WriteLine();
                            Console.WriteLine("Directory hasnt been set");
                            Console.WriteLine("Set it by executing SETDIR");
                            cmdvoid();
                        }

                        Console.WriteLine("File name? (file extension included)");
                        Console.WriteLine();

                        try
                        {
                            Console.Write(currentDir);
                            string delFile = Console.ReadLine();

                            File.Delete(currentDir + delFile);
                            Console.WriteLine();
                            Console.WriteLine("File deleted");

                            cmdvoid();
                        }
                        catch
                        {
                            Console.WriteLine();
                            Console.WriteLine("Cant delete file");
                            Console.WriteLine("doesnt exist or file protected/critical");
                            cmdvoid();
                        }
                    }

                    if (CMD == "DLDIR") // delete a directory
                    {
                        if (currentDir == "")
                        {
                            Console.WriteLine();
                            Console.WriteLine("Directory hasnt been set");
                            Console.WriteLine("Set it by executing SETDIR");
                            cmdvoid();
                        }

                        Console.WriteLine("Directory name?");
                        Console.WriteLine();

                        try
                        {
                            Console.Write(currentDir);
                            string delDir = Console.ReadLine();

                            Directory.Delete(currentDir + delDir);
                            Console.WriteLine();
                            Console.WriteLine("Directory deleted");

                            cmdvoid();
                        }
                        catch
                        {
                            Console.WriteLine();
                            Console.WriteLine("Cant delete directory");
                            Console.WriteLine("directory protected or critical");
                            cmdvoid();
                        }
                    }

                    if (CMD == "LISTDIR") // list whatever is in the set current dir
                    {
                        if (currentDir == "")
                        {
                            Console.WriteLine();
                            Console.WriteLine("Directory hasnt been set");
                            Console.WriteLine("Set it by executing SETDIR");
                            cmdvoid();
                        }

                        string rootdir = currentDir;

                        Console.WriteLine();
                        Console.WriteLine("=============================================================");
                        Console.WriteLine("Contents of ; " + currentDir);
                        Console.WriteLine("=============================================================");


                        // stole this shit from stackoverflow because didnt know how to implement it myself
                        string[] files = Directory.GetFiles(rootdir);
                        Console.WriteLine(String.Join(Environment.NewLine, files));

                        string[] dirs = Directory.GetDirectories(rootdir);
                        Console.WriteLine(String.Join(Environment.NewLine, dirs));
                        Console.WriteLine("=============================================================");

                        cmdvoid();
                    }


                    else
                    {
                        Console.WriteLine("'" + CMD + "' is not a executeable command");
                        cmdvoid();
                    }
                }
            }
            catch // if stuff goes very wrong this happens
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;

                Console.WriteLine();
                Console.WriteLine("==================================================");
                Console.WriteLine(" AeroCL | CRASH HANDLER");
                Console.WriteLine("==================================================");
                Console.WriteLine("AeroCL has catched a unknown exception");
                Console.WriteLine("press any key to exit");
                Console.WriteLine("==================================================");
                Console.ReadKey();


            }
        }

        private static IList<ManagementBaseObject> GetUsbDevices() // i dont even know what the hell is this
        {
            throw new NotImplementedException();
        }
    }
}
