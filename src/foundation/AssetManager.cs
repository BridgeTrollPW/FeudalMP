using System;
using System.Xml.Schema;
using Godot;

namespace FeudalMP.src.foundation
{
    class AssetManager : Node
    {
        public const string PATH_BASE = "res://assets";
        public const string PATH_UI = PATH_BASE + "/ui";
        public const string PATH_MAPS = PATH_BASE + "/maps";
        public override void _Ready()
        {

        }

        public static T Load<T>(string path) where T : Node
        {
            return (T)((PackedScene)GD.Load(path)).Instance();
        }

    }


}