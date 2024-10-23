using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;
using System.Threading;
using TMPro;
using UnityEngine.Windows;

public class ClientUDP : MonoBehaviour
{
    Socket socket;
    public GameObject UItextObj;
    TextMeshProUGUI UItext;
    string clientText;

    
    public TMP_InputField ipInputField;
    public TMP_InputField userNameInputField;
    string serverIP;


    // Start is called before the first frame update
    void Start()
    {
        UItext = UItextObj.GetComponent<TextMeshProUGUI>();

    }
    public void StartClient()
    {
        serverIP = ipInputField.text;

        if (IsValidIP(serverIP)){//IP es correcta?

            clientText = "IP correcta";

            IPEndPoint ipep = new IPEndPoint(IPAddress.Parse(serverIP), 9050);
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);


            Thread mainThread = new Thread(Send);
            mainThread.Start();
        }
        else
        {
            clientText += "\nIP no válida.";
        }
    }

    void Update()
    {
        UItext.text = clientText;
    }

    void Send()
    {
        //TO DO 2
        //Unlike with TCP, we don't "connect" first,
        //we are going to send a message to establish our communication so we need an endpoint
        //We need the server's IP and the port we've binded it to before
        //Again, initialize the socket
        //IPEndPoint ipep = new IPEndPoint(IPAddress.Parse("192.168.0.17"), 9050);//Poner aqui tu IP
        IPEndPoint ipep = new IPEndPoint(IPAddress.Parse(serverIP), 9050);//Poner aqui tu IP

        socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);


        //TO DO 2.1 
        //Send the Handshake to the server's endpoint.
        //This time, our UDP socket doesn't have it, so we have to pass it
        //as a parameter on it's SendTo() method

        string username = userNameInputField.text ;
        byte[] data = Encoding.ASCII.GetBytes(username);

        socket.SendTo(data, data.Length, SocketFlags.None, ipep);

        clientText += $"\nUsername '{username}' sent to the server.";

        //TO DO 5
        //We'll wait for a server response,
        //so you can already start the receive thread
        Thread receive = new Thread(Receive);
        receive.Start();

    }

    //TO DO 5
    //Same as in the server, in this case the remote is a bit useless
    //since we already know it's the server who's communicating with us
    void Receive()
    {
        IPEndPoint sender = new IPEndPoint(IPAddress.Any, 0);
        EndPoint Remote = (EndPoint)(sender);
        byte[] data = new byte[1024];

        while (true)
        {
            int recv = socket.ReceiveFrom(data, ref Remote);

            string message = Encoding.ASCII.GetString(data, 0, recv);
            clientText += "\nReceived from server: " + message; // Almacenar el mensaje recibido

            if (message == "MiServidorUDP")
            {
                clientText += "\nConnected to server: " + message;
            }

        }
        //clientText = ("Message received from {0}: " + Remote.ToString());
        //clientText = clientText += "\n" + Encoding.ASCII.GetString(data, 0, recv);

    }

    bool IsValidIP(string ip)
    {
        IPAddress ipAddr;
        return IPAddress.TryParse(ip, out ipAddr);
    }
}


