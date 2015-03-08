using UnityEngine;
using System;
using System.Collections;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.IO;
using UnityThreading;

public class PlayerNetCommunicate : MonoBehaviour {

	private PlayerManager playerManager;

	private UdpClient[] udpListenerLeft;
	private UdpClient[] udpListenerRight;
	private UdpClient socket;

	UnityThreading.ActionThread myThread;
	
	public IPEndPoint[] IPLeft;
	public IPEndPoint[] IPRight;

	public int[] angleLeft;
	public int[] distanceLeft;
	public int[] angleRight;
	public int[] distanceRight;
	public int[] buttonPressed;
	
	Boolean createdRight = true;
	Boolean createdLeft = true;
	
	Boolean left = false;
	Boolean right = false;

	int PLAYER = 0;
	int PLAYER_ACTIVE = 0;
	int PORT = 4443;
	
	void Start()
	{

		BroadcastIP ();
	
		playerManager = GameObject.FindObjectOfType<PlayerManager>();

		udpListenerLeft = new UdpClient[4];
		udpListenerRight = new UdpClient[4];

		IPLeft = new IPEndPoint[4];
		IPRight = new IPEndPoint[4];
		
		angleLeft = new int[4];
		distanceLeft = new int[4];
		angleRight = new int[4];
		distanceRight = new int[4];
		buttonPressed = new int[4];

		for (int i = 0; i < 1; i++) {

			angleLeft[i] = 0;
			distanceLeft[i] = 0;
			angleRight[i] = 0;
			distanceRight[i] = 0;
			buttonPressed[i] = 0;

		}

		int counter = 0;
	
		while (counter < 4) {

			if (createdLeft && createdRight) {
				createdLeft = false;
				createdRight = false;
				
				udpListenerLeft[counter] = new UdpClient(PORT);
				IPLeft[counter] = new IPEndPoint(IPAddress.Parse(LocalIPAddress ()), PORT);
				
				PORT++;
				
				udpListenerRight[counter] = new UdpClient(PORT);
				IPRight[counter] = new IPEndPoint(IPAddress.Parse(LocalIPAddress ()), PORT);
				
				PORT++;
				
				PLAYER = counter;
				
				InitializeListenerUdp ();

				counter++;
			}
					
		}

	}

	void BroadcastIP () {

		myThread = UnityThreadHelper.CreateThread (() =>
		{

			try {

				socket = new UdpClient (8888);
				
				while (true) {

					IPEndPoint endpoint = new IPEndPoint (IPAddress.Parse (LocalIPAddress ()), PORT);
					byte[] recvBuf = socket.Receive (ref endpoint);

					String message = System.Text.Encoding.Default.GetString (recvBuf);
					message = message.Trim ();

					if (message.Equals ("IPREQUEST")) {
						byte[] send = Encoding.ASCII.GetBytes ("IPANSWER");
						socket.Send (send, send.Length, endpoint);						
					}
				}
			} catch (Exception e) {
				//Debug.Log (e.Message);
			}});
	}

	void InitializeListenerUdp() {

		UnityThreadHelper.CreateThread(() =>
		                               {
			int slot = PLAYER;
			//Debug.Log ("Creating left listener for player " + slot + " on port: " + IPLeft[slot].Port);
			Boolean taken = false;
			createdLeft = true;

			while (true) {
				byte[] answerByte = udpListenerLeft[slot].Receive (ref IPLeft[slot]);
				String data = Encoding.ASCII.GetString(answerByte, 0, answerByte.Length);

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
						if (tokens[0].Equals("start")) {
							if (taken == false) {
								byte[] send = Encoding.ASCII.GetBytes("hello");
								udpListenerLeft[slot].Send(send, send.Length, IPLeft[slot]);
								UnityThreadHelper.Dispatcher.Dispatch(() => playerManager.HandlePhonePlayerJoin(slot));
								PLAYER_ACTIVE++;
								taken = true;
							} else {
								byte[] send = Encoding.ASCII.GetBytes("sorry");
								udpListenerLeft[slot].Send(send, send.Length, IPLeft[slot]);
							}
						}
					}
				}

			}

		});

		UnityThreadHelper.CreateThread(() =>
		                               {
			int slot = PLAYER;
			//Debug.Log ("Creating right listener for player " + slot + " on port: " + IPRight[slot].Port);
			createdRight = true;

			while (true) {
				byte[] byteBuffer = udpListenerRight[slot].Receive (ref IPRight[slot]);
				String data = System.Text.Encoding.Default.GetString(byteBuffer);

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

			}
			
		});


	}

	void OnApplicationQuit()
	{

		myThread.Exit ();

		try
		{
			socket.Close();
			for (int i = 0; i < PLAYER + 1; i++) {
				udpListenerLeft[i].Close();
				udpListenerRight[i].Close ();
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