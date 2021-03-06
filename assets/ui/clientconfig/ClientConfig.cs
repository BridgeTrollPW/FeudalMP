using FeudalMP.assets.ui.mainmenu;
using FeudalMP.src.foundation;
using FeudalMP.src.network.client;
using FeudalMP.src.network.messages;
using FeudalMP.src.network.service;
using Godot;
using System;

public class ClientConfig : Control
{
    private TextEdit addressTextField;
    private TextEdit portTextField;
    private TextEdit nameTextField;
    private Client client;
    public override void _Ready()
    {
        portTextField = GetNode<TextEdit>("VBoxContainer/Port/TextEdit");
        addressTextField = GetNode<TextEdit>("VBoxContainer/Host/TextEdit");
        nameTextField = GetNode<TextEdit>("VBoxContainer/Name/TextEdit");
    }

    public void OnConnectPressed()
    {
        client = new Client();
        NodeTreeManager.Instance.ServiceLayer.AddChild(client);
        string ip = addressTextField.Text;
        int port = portTextField.Text.ToInt();
        client.Connect(ip, port);
    }
    public void OnBackPressed()
    {
        Client clientService = NodeTreeManager.Instance.ServiceLayer.GetNodeOrNull<Client>("./Client");
        if (clientService != null)
        {
            clientService.Terminate();
        }
        NodeTreeManager.Instance.GUILayer.ChangeScene<MainMenu>("res://assets/ui/mainmenu/MainMenu.tscn");
    }
    public void OnMessageConnectPressed()
    {
        ConnectClient connect = new ConnectClient
        {
            name = (nameTextField.Text != null || nameTextField.Text != "") ? nameTextField.Text : "Unknown Leyben"
        };
        NodeTreeManager.Instance.ServiceLayer.GetNode<NetworkMessageDispatcher>("./NetworkMessageDispatcher").Dispatch(connect, 1, NetworkedMultiplayerPeer.TransferModeEnum.Reliable);
    }

}
