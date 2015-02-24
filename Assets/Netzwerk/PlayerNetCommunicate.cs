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

	private System.Diagnostics.Process process;

	private StreamReader streamReadersLeft;
	private StreamReader streamReadersRight;

	public int angleLeft;
	public int distanceLeft;
	public int angleRight;
	public int buttonPressed;

	//private StreamReader[] streamReadersLeft;
	//private StreamReader[] streamReadersRight;

	//public int[] angleLeft;
	//public int[] distanceLeft;
	//public int[] angleRight;
	//public Boolean[] buttonPressed;

	void Start()
	{
		//Debug.Log (LocalIPAddress());

		/**streamReadersLeft = new StreamReader[4];
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
		}**/

		process = System.Diagnostics.Process.Start((Application.dataPath) + "/Netzwerk/ServiceAnnouncer.jar");

		angleLeft = 0;
		distanceLeft = 0;
		angleRight = 0;
		buttonPressed = 0;

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
			
			streamReadersLeft = new StreamReader(streamLeft);
			left = true;

			while (true) {
				
				String data = streamReadersLeft.ReadLine();
				String[] tokens = data.Split(' ');
				
				if (tokens.Length == 2) {
					if (tokens[0] != "") {
						angleLeft = int.Parse(tokens[0].Substring(1));
					}
					
					if (tokens[1] != "") {
						distanceLeft = int.Parse(tokens[1].Substring(1));
					}
					
				} else if (tokens.Length == 1) {
					
					if (tokens[0] != "") {
						if (tokens[0].Substring(0,1).Equals("0")) {
							angleLeft = int.Parse(tokens[0].Substring(1));
						}
						
						if (tokens[0].Substring(0,1).Equals("1")) {
							distanceLeft = int.Parse(tokens[0].Substring(1));
						}
					}
				}
				
			};
			
		});
		
		UnityThreadHelper.CreateThread(() =>
		                               {
			NetworkStream streamRight = tcpListenerRight.AcceptTcpClient().GetStream();
			Debug.Log("Right Connection accepted.");
			
			streamReadersRight = new StreamReader(streamRight);
			right = true;

			while (true) {
				
				String data = streamReadersRight.ReadLine();

				Debug.Log (data);
				
				if (data.Substring(0,1).Equals("a")) {
					if (data.Substring(1).Equals("0")) {
						buttonPressed = 0;
					} else {
						buttonPressed = 1;
					}
				} else {
					angleRight = int.Parse(data);
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
	
		foreach (System.Diagnostics.Process p in System.Diagnostics.Process.GetProcessesByName("java")) {
			p.CloseMainWindow();
		}

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
