using System;
using System.Linq;
using Godot;

namespace FeudalMP.src.foundation
{
    public sealed class NodeTreeManager : Node
    {

        private static readonly Lazy<NodeTreeManager> lazy = new Lazy<NodeTreeManager>(() => new NodeTreeManager());
        public static NodeTreeManager Instance { get { return lazy.Value; } }

        private NodeTreeManager() { }
        private TreeComponentWrapper hudLayer;
        private TreeComponentWrapper guiLayer;
        private TreeComponentWrapper sceneLayer;
        private TreeComponentWrapper serviceLayer;

        public TreeComponentWrapper HUDLayer { get => hudLayer; }
        public TreeComponentWrapper GUILayer { get => guiLayer; }
        public TreeComponentWrapper SceneLayer { get => sceneLayer; }
        public TreeComponentWrapper ServiceLayer { get => serviceLayer; }

        public override void _Ready()
        {
            Name = "NodeTreeManager";
            hudLayer = new TreeComponentWrapper
            {
                Name = "HUDLayer"
            };
            GetTree().Root.AddChild(hudLayer);

            guiLayer = new TreeComponentWrapper
            {
                Name = "GUILayer"
            };
            GetTree().Root.AddChild(guiLayer);

            sceneLayer = new TreeComponentWrapper
            {
                Name = "SceneLayer"
            };
            GetTree().Root.AddChild(sceneLayer);

            serviceLayer = new TreeComponentWrapper
            {
                Name = "ServiceLayer"
            };
            GetTree().Root.AddChild(serviceLayer);
        }
    }

    public class TreeComponentWrapper : Node
    {
        public void ChangeScene<T>(string path) where T : Node
        {
            ChangeScene(AssetManager.Load<T>(path));
        }
        public void ChangeScene(Node tscn)
        {
            this.Clear();
            this.AddChild(tscn);
        }
        public void Clear()
        {
            foreach (Node node in this.GetChildren())
            {
                node.QueueFree();
            }
        }
    }
}