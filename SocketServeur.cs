using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

internal class SocketServeur
{
    private TcpListener listener;
    private List<TcpClient> clients;

    public SocketServeur(int port)
    {
        listener = new TcpListener(IPAddress.Any, port);
        clients = new List<TcpClient>();
    }

    public void Start()
    {
        listener.Start();
        Console.WriteLine("Serveur démarré. En attente de connexion...");

        while (true)
        {
            TcpClient client = listener.AcceptTcpClient();
            Console.WriteLine("Client connecté.");

            // Ajouter le client à la liste des clients
            lock (clients)
            {
                clients.Add(client);
            }

            // Démarrer un thread pour gérer les communications avec ce client
            Thread clientThread = new Thread(new ParameterizedThreadStart(HandleClient));
            clientThread.Start(client);
        }
    }

    private void HandleClient(object obj)
    {
        TcpClient client = (TcpClient)obj;
        NetworkStream stream = client.GetStream();
        byte[] buffer = new byte[1024];
        int bytesRead;

        try
        {
            while ((bytesRead = stream.Read(buffer, 0, buffer.Length)) != 0)
            {
                string dataReceived = Encoding.ASCII.GetString(buffer, 0, bytesRead);
                Console.WriteLine($"Données reçues du client : {dataReceived}");

                // Envoyer le message à tous les clients
                SendMessageToAllClients(dataReceived);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erreur lors de la communication avec le client : {ex.Message}");
        }
        finally
        {
            // Supprimer le client de la liste des clients lorsqu'il se déconnecte
            lock (clients)
            {
                clients.Remove(client);
            }
            stream.Close();
            client.Close();
            Console.WriteLine("Client déconnecté.");
        }
    }

    private void SendMessageToAllClients(string message)
    {
        byte[] data = Encoding.ASCII.GetBytes(message);
        lock (clients)
        {
            foreach (TcpClient client in clients)
            {
                NetworkStream stream = client.GetStream();
                stream.Write(data, 0, data.Length);
            }
        }
    }
}
