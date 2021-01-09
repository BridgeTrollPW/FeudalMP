using System.Collections.Generic;
using FeudalMP.src.foundation;
using FeudalMP.src.network.client;
using FeudalMP.src.network.messages;
using Godot;

public class PlayerList : Control
{
    [Signal]
    public delegate void ClosePlayerListUI();
    private const float iteration = 0.5f;
    private float elapsedTime = 0f;
    private VBoxContainer vBoxContainer;
    public override void _Ready()
    {
        vBoxContainer = GetNode<VBoxContainer>("HBoxContainer/VBoxContainer");
        Name = nameof(PlayerList);
        Visible = false;
    }

    public override void _Process(float delta)
    {
        if (Visible)
        {
            elapsedTime += delta;
            if (elapsedTime > iteration)
            {
                Client client = NodeTreeManager.Instance.ServiceLayer.GetNode<Client>("./Client");
                client.NetworkMessageDispatcher.Dispatch(new RequestPlayerList(), NetworkedMultiplayerPeer.TargetPeerServer);
                
                //Add Players that are not already part of the playerlist
                foreach (KeyValuePair<int, string> entry in client.PlayerList)
                {
                    if (vBoxContainer.HasNode(entry.Key.ToString()))
                    {
                        continue;
                    }
                    AddItem(entry.Value, entry.Key);
                }

                //Remove players that are no longer connected
                if (client.PlayerList.Count > 0)
                {
                    foreach (Node n in vBoxContainer.GetChildren())
                    {
                        if (client.PlayerList.ContainsKey(n.Name.ToInt()))
                        {
                            continue;
                        }
                        vBoxContainer.RemoveChild(n);
                        n.QueueFree();
                    }
                }
                elapsedTime = 0;
            }
        }
    }
    public void AddItem(string name, int id)
    {
        Label playerNameLabel = new Label
        {
            Text = name
        };

        Button disconnectButton = new Button
        {
            Text = "Disconnect"
        };
        disconnectButton.Connect("pressed", this, "OnDisconnectPressed", new Godot.Collections.Array() { id });

        HBoxContainer row = new HBoxContainer
        {
            Name = id.ToString()
        };
        row.AddChild(playerNameLabel);
        row.AddChild(disconnectButton);
        vBoxContainer.AddChild(row);
    }

    public void OnDisconnectPressed(int id)
    {
        GD.Print("Disconnecting peer with id=" + id);
        Client client = NodeTreeManager.Instance.ServiceLayer.GetNode<Client>("./Client");
        client.NetworkMessageDispatcher.Dispatch(new Disconnect()
        {
            DisconnectedPeer = id
        }, NetworkedMultiplayerPeer.TargetPeerServer);
    }

    public void OnBackButtonPressed()
    {
        EmitSignal(nameof(ClosePlayerListUI));
        Visible = false;
    }
}
