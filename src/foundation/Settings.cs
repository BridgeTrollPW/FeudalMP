using System;
using Godot;

namespace FeudalMP.src.foundation
{
    public class Settings : ConfigFile
    {
        private static readonly Lazy<Settings> lazy = new Lazy<Settings>(() => new Settings());
        public static Settings Instance { get { return lazy.Value; } }
        private Settings()
        {
            this.Load("user://settings.cfg");
        }
    }
}