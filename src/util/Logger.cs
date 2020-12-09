using System;
using Godot;
using Godot.Collections;

namespace FeudalMP.src.util
{
    public class Logger : Godot.Object
    {
        private string ClazzName;
        public Logger(string Clazz)
        {
            ClazzName = Clazz;
            if (ClazzName == null || ClazzName == "")
            {
                ClazzName = "Log";
            }
        }

        private string GetTimeFormat()
        {
            Dictionary dateTimeDict = OS.GetDatetime();
            return string.Format("{0}.{1}.{2} - {3}:{4}:{5}", dateTimeDict["day"], dateTimeDict["month"], dateTimeDict["year"], dateTimeDict["hour"], dateTimeDict["minute"], dateTimeDict["second"]);
        }

        private void Write(string text, string level)
        {
            GD.Print(string.Format("[{0}] {1}.{2} :: {3}", GetTimeFormat(), ClazzName, level, text));
        }

        public void Info(string text) { Write(text, "INFO"); }
        public void Warn(string text) { Write(text, "WARN"); }
        public void Error(string text) { Write(text, "ERROR"); }
    }
}