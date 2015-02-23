using UnityEngine;
using System;
using System.Collections;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.IO;
using UnityThreading;

public class PlayerNetCommunicate : MonoBehaviour
{

	public PlayerManager playerManager;

	TcpListener tcpListenerLeft;
	TcpListener tcpListenerRight;
	StreamReader streamReaderRight;
	StreamReader streamReaderLeft;
	Boolean left;
	Boolean right;
	int PORT = 4444;
	int PLAYER = 0;

	private StreamReader[] streamReadersLeft;
	private StreamReader[] streamReadersRight;

	public int[] angleLeft;
	public int[] distanceLeft;
	public int[] angleRight;
	public Boolean[] buttonPressed;

	void Start()
	{
		//Debug.Log (LocalIPAddress());

		streamReadersLeft = new StreamReader[4];
		streamReadersRight = new StreamReader[4];

		angleLeft = new int[4];
		distanceLeft = new int[4];
		angleRight = new int[4];
		buttonPressed = new bool[4];

		for (int i = 0; i < 4; i++) {
			angleLeft[i] = 0;
			distanceLeft[i] = 0;
			angleRight[i] = 0;
			buttonPressed[i] = false;
		}

		InitializeListener ();
	}

	void InitializeListener() {

		tcpListenerLeft = new TcpListener (IPAddress.Parse (LocalIPAddress ()), PORT);
		Debug.Log("Created Listener on Port: " + PORT);
		tcpListenerLeft.Start();
		PORT++;
		
		tcpListenerRight = new TcpListener (IPAddress.Parse (LocalIPAddress ()), PORT);
		Debug.Log("Created Listener on Port: " + PORT);
		tcpListenerRight.Start();
		PORT++;
		
		UnityThreadHelper.CreateThread(() =>
		                               {
			NetworkStream streamLeft = tcpListenerLeft.AcceptTcpClient().GetStream();
			Debug.Log("Left Connection accepted.");
			
			streamReadersLeft[PLAYER] = new StreamReader(streamLeft);
			left = true;

			while (true) {
				
				String data = streamReadersLeft[PLAYER].ReadLine();
				String[] tokens = data.Split(' ');
				
				if (tokens.Length == 2) {
					if (tokens[0] != "") {
						angleLeft[PLAYER] = int.Parse(tokens[0].Substring(1));
					}
					
					if (tokens[1] != "") {
						distanceLeft[PLAYER] = int.Parse(tokens[1].Substring(1));
					}
					
				} else if (tokens.Length == 1) {
					
					if (tokens[0] != "") {
						if (tokens[0].Substring(0,1).Equals("0")) {
							angleLeft[PLAYER] = int.Parse(tokens[0].Substring(1));
						}
						
						if (tokens[0].Substring(0,1).Equals("1")) {
							distanceLeft[PLAYER] = int.Parse(tokens[0].Substring(1));
						}
					}
				}
				
			};
			
		});
		
		UnityThreadHelper.CreateThread(() =>
		                               {
			NetworkStream streamRight = tcpListenerRight.AcceptTcpClient().GetStream();
			Debug.Log("Right Connection accepted.");
			
			streamReadersRight[PLAYER] = new StreamReader(streamRight);
			right = true;

			while (true) {
				
				String data = streamReadersRight[PLAYER].ReadLine();
				
				if (data.Substring(0,1).Equals("a")) {
					if (data.Substring(1,2).Equals("0")) {
						buttonPressed[PLAYER] = false;
					} else {
						buttonPressed[PLAYER] = true;
					}
				} else {
					angleRight[PLAYER] = int.Parse(data);
				}
				
			};
			
		});

	}

	void Update () {
		if (left && right) {
			right = false;
			left = false;
			playerManager.AddPhonePlayer(PLAYER);
			//PLAYER++;
			//InitializeListener ();
		}
	}

	void OnApplicationQuit()
	{
		
		// You must close the tcp listener
		try
		{
			tcpListenerLeft.Stop();
			tcpListenerRight.Stop ();
		}
		catch(Exception e)
		{
			Debug.Log(e.Message);
		}
	}
	
	public string LocalIPAddress()
	{
		IPHostEntry host;
		string localIP = "";
		host = Dns.GetHostEntry(Dns.GetHostName());
		foreach (IPAddress ip in host.AddressList)
		{
			if (ip.AddressFamily == AddressFamily.InterNetwork)
			{
				localIP = ip.ToString();
				break;
			}
		}
		return localIP;
	}

}

//string responseString = "You have successfully connected to me";

//Forms and sends a response string to the connected client.
//Byte[] sendBytes = Encoding.ASCII.GetBytes(responseString);
//stream.Write(sendBytes, 0, sendBytes.Length);
//Debug.Log("Message Sent /> : " + responseString);
