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

	private TcpListener[] tcpListenerLeft;
	private TcpListener[] tcpListenerRight;

	private StreamReader[] streamReadersLeft;
	private StreamReader[] streamReadersRight;
	
	public int[] angleLeft;
	public int[] distanceLeft;
	public int[] angleRight;
	public int[] distanceRight;
	public int[] buttonPressed;
	
	Boolean left;
	Boolean right;

	int PLAYER = 0;
	int PLAYER_ACTIVE = 0;
	int PORT = 4444;

	private System.Diagnostics.Process process;

	void Start()
	{

		process = System.Diagnostics.Process.Start((Application.dataPath) + "/Netzwerk/ServiceAnnouncer.jar");

		tcpListenerLeft = new TcpListener[4];
		tcpListenerRight = new TcpListener[4];

		streamReadersLeft = new StreamReader[4];
		streamReadersRight = new StreamReader[4];

		angleLeft = new int[4];
		distanceLeft = new int[4];
		angleRight = new int[4];
		distanceRight = new int[4];
		buttonPressed = new int[4];

		for (int i = 0; i < 4; i++) {
			angleLeft[i] = 0;
			distanceLeft[i] = 0;
			angleRight[i] = 0;
			distanceRight[i] = 0;
			buttonPressed[i] = 0;
		}

		InitializeListenerTcp ();
	}

	void InitializeListenerTcp() {

		tcpListenerLeft[PLAYER] = new TcpListener (IPAddress.Parse (LocalIPAddress ()), PORT);
		Debug.Log("Created Listener on Port: " + PORT);
		tcpListenerLeft[PLAYER].Start();
		PORT++;
		
		tcpListenerRight[PLAYER] = new TcpListener (IPAddress.Parse (LocalIPAddress ()), PORT);
		Debug.Log("Created Listener on Port: " + PORT);
		tcpListenerRight[PLAYER].Start();
		PORT++;
		
		UnityThreadHelper.CreateThread(() =>
		                               {
			int slot = PLAYER;
			NetworkStream streamLeft = tcpListenerLeft[slot].AcceptTcpClient().GetStream();
			Debug.Log("Left Connection accepted.");
			PLAYER_ACTIVE++;
			
			streamReadersLeft[slot] = new StreamReader(streamLeft);
			left = true;

			while (true) {
				
				String data = streamReadersLeft[slot].ReadLine();
				String[] tokens = data.Split(' ');
				
				if (tokens.Length == 2) {
					if (tokens[0] != "") {
						angleLeft[slot] = int.Parse(tokens[0].Substring(1));
					}
					
					if (tokens[1] != "") {
						distanceLeft[slot] = int.Parse(tokens[1].Substring(1));
					}
					
				} else if (tokens.Length == 1) {
					
					if (tokens[0] != "") {
						if (tokens[0].Substring(0,1).Equals("0")) {
							angleLeft[slot] = int.Parse(tokens[0].Substring(1));
						}
						
						if (tokens[0].Substring(0,1).Equals("1")) {
							distanceLeft[slot] = int.Parse(tokens[0].Substring(1));
						}
					}
				}
				
			};
			
		});
		
		UnityThreadHelper.CreateThread(() =>
		                               {

			int slot = PLAYER;
			NetworkStream streamRight = tcpListenerRight[slot].AcceptTcpClient().GetStream();
			Debug.Log("Right Connection accepted.");
			
			streamReadersRight[slot] = new StreamReader(streamRight);
			right = true;

			while (true) {
				
				String data = streamReadersRight[slot].ReadLine();
				String[] tokens = data.Split(' ');

				if (tokens.Length == 2) {
					if (tokens[0] != "") {
						angleRight[slot] = int.Parse(tokens[0].Substring(1));
					}
					
					if (tokens[1] != "") {
						distanceRight[slot] = int.Parse(tokens[1].Substring(1));
					}
					
				} else if (tokens.Length == 1) {
					
					if (tokens[0] != "") {
						if (tokens[0].Substring(0,1).Equals("0")) {
							angleRight[slot] = int.Parse(tokens[0].Substring(1));
						}
						
						if (tokens[0].Substring(0,1).Equals("1")) {
							distanceRight[slot] = int.Parse(tokens[0].Substring(1));
						}

						if (tokens[0].Substring(0,1).Equals("a")) {
							if (data.Substring(1).Equals("0")) {
								buttonPressed[slot] = 0;
							} else {
								buttonPressed[slot] = 1;
							}
						}
					}
				}
			};
		});

	}

	void Update () {
		if (left && right) {
			right = false;
			left = false;
			playerManager.HandlePhonePlayerJoin(PLAYER);
			PLAYER++;
			InitializeListenerTcp ();
		}
	}

	void OnApplicationQuit()
	{
	
		foreach (System.Diagnostics.Process p in System.Diagnostics.Process.GetProcessesByName("java")) {
			p.CloseMainWindow();
		}

		try
		{
			for (int i = 0; i < PLAYER; i++) {
				tcpListenerLeft[i].Stop();
				tcpListenerRight[i].Stop ();
			}
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