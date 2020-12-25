using Godot;

namespace FeudalMP.src.ui
{
    public class NodeTreeManager : Node
    {
        private Node hudLayer;
        private Node guiLayer;
        private Node sceneLayer;

        public Node HUDLayer { get => hudLayer;}
        public Node GUILayer { get => guiLayer;}
        public Node SceneLayer { get => sceneLayer;}

        public override void _Ready()
        {
            hudLayer = new Node
            {
                Name = "HUDLayer"
            };
            GetTree().Root.AddChild(hudLayer);

            guiLayer = new Node
            {
                Name = "GUILayer"
            };
            GetTree().Root.AddChild(guiLayer);

            sceneLayer = new Node
            {
                Name = "SceneLayer"
            };
            GetTree().Root.AddChild(sceneLayer);
        }

    }
}