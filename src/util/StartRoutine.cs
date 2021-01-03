using System;
using System.Runtime.CompilerServices;
using Godot;

namespace FeudalMP.src.util
{
    public class StartRoutine
    {
        private static readonly Logger logger = new Logger(nameof(StartRoutine));
        public static Error Check()
        {
            bool firstRun = false;
            File f = new File();
            if (!f.FileExists("user://settings.cfg"))
            {
                logger.Info("Created settings file at" + OS.GetUserDataDir() + "/settings.cfg");
                f.Open("user://settings.cfg", File.ModeFlags.Write);
                f.Close();
                firstRun = true;
            }
            ConfigFile cf = new ConfigFile();
            Error cf_error = cf.Load("user://settings.cfg");
            if (cf_error != Error.Ok)
            {
                logger.Error("Something went wrong while loading the settings.cfg from " + OS.GetUserDataDir());
                OS.Alert(string.Format(TranslationServer.Translate("ERR_START_SETTINGS_CFG"), OS.GetUserDataDir()), "Failure on startup");
                return Error.CantOpen;
            }
            if (firstRun)
            {
                cf.SetValue(CFG.Application.SECTION, CFG.Application.FIRST_TIME_RUN, 1);
                cf.Save("user://settings.cfg");
                writeDefaults(cf);
            }

            Directory dir = new Directory();
            if (!dir.DirExists("user://maps"))
            {
                dir.MakeDirRecursive("user://maps");
                logger.Info("Map directory did not exist and was therefore created");
            }

            logger.Info("StartRoutine Check was successful");
            return Error.Ok;
        }


        private static void writeDefaults(ConfigFile cf)
        {
            //
            //Server
            //
            cf.SetValue(CFG.Server.SECTION, CFG.Server.PORT, 9913);
            //
            //Application
            //

            //
            //Sound
            //

            //
            //Grafik
            //


            cf.Save("user://settings.cfg");
        }
    }
}