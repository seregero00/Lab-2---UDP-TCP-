using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;
using System.Threading;
using TMPro;
//using UnityEngine.tvOS;

public class ClientTCP : MonoBehaviour
{
    public GameObject UItextObj;
    TextMeshProUGUI UItext;
    string clientText;
    Socket server;

    public TMP_InputField ipInputField;
    string serverIP;

    // Start is called before the first frame update
    void Start()
    {
        UItext = UItextObj.GetComponent<TextMeshProUGUI>();

    }

    // Update is called once per frame
    void Update()
    {
        UItext.text = clientText;

    }

    public void StartClient()
    {
        serverIP = ipInputField.text;

        if (IsValidIP(serverIP))
        {

            // Si la IP es válida, mostrar el mensaje "IP correcta" en la UI
            clientText = "IP correcta";

            // Continuar con el inicio del cliente
            IPEndPoint ipep = new IPEndPoint(IPAddress.Parse(serverIP), 9050);
            server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);


            Thread mainThread = new Thread(Connect);
            mainThread.Start();
        }
        else
        {
            clientText += "\nIP no válida.";
        }


    }
    void Connect()
    {
        //TO DO 2
        //Create the server endpoint so we can try to connect to it.
        //You'll need the server's IP and the port we binded it to before
        //Also, initialize our server socket.
        //When calling connect and succeeding, our server socket will create a
        //connection between this endpoint and the server's endpoint

        IPEndPoint ipep = new IPEndPoint(IPAddress.Parse(serverIP), 9050); //Poner aqui tu IP
        server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        server.Connect(ipep);

        //TO DO 4
        //With an established connection, we want to send a message so the server aacknowledges us
        //Start the Send Thread
        Thread sendThread = new Thread(Send);
        sendThread.Start();

        //TO DO 7
        //If the client wants to receive messages, it will have to start another thread. Call Receive()
        Thread receiveThread = new Thread(Receive);
        receiveThread.Start();

    }
    void Send()
    {
        //TO DO 4
        //Using the socket that stores the connection between the 2 endpoints, call the TCP send function with
        //an encoded message
        string message = "Hello from client!";
        byte[] msg = Encoding.ASCII.GetBytes(message);
        server.Send(msg);

    }

    //TO DO 7
    //Similar to what we already did with the server, we have to call the Receive() method from the socket.
    void Receive()
    {
        byte[] data = new byte[1024];
        int recv = 0;


        while (true)
        {
            data = new byte[1024];
            recv = server.Receive(data);
            if (recv == 0)
                break;
            else
            {
                string receivedMessage = Encoding.ASCII.GetString(data, 0, recv);
                clientText += "\nReceived: " + receivedMessage;
            }


        }

    }
    bool IsValidIP(string ip)
    {
        IPAddress ipAddr;
        return IPAddress.TryParse(ip, out ipAddr);
    }

}
