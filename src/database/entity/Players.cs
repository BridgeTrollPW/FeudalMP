using FeudalMP.src.database.boundary;
using Godot;

namespace FeudalMP.src.database.entity
{
    public class Players
    {
        public int Id { get; set; }
        public string SteamId { get; set; }
        public long Gold { get; set; }
        public string Name { get; set; }
        public Vector3 Position { get; set; }
        private Vector3 Rotation { get; set; }
        private Faction Faction { get; set; }
        private Profession Profession { get; set; }
    }
}