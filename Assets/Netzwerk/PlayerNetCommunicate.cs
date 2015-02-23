using UnityEngine;
using System;
using System.Collections;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.IO;

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

	void Start()
	{
		//Debug.Log (LocalIPAddress());
		InitializeListener ();
	}

	void InitializeListener() {
		tcpListenerLeft = CreateListener (PORT);
		tcpListenerLeft.Start();
		
		tcpListenerRight = CreateListener (PORT);
		tcpListenerRight.Start();
		
		StartToListen ();
	}

	TcpListener CreateListener(int port) {
		TcpListener listen = new TcpListener (IPAddress.Parse (LocalIPAddress ()), port);
		PORT++;
		return listen;
	}

	void StartToListen() {

		UnityThreadHelper.CreateThread(() =>
		                               {
			NetworkStream streamLeft = tcpListenerLeft.AcceptTcpClient().GetStream();
			Debug.Log("Left Connection accepted.");
			
			streamReaderLeft = new StreamReader(streamLeft);
			left = true;
			HandOverStreamReader();
			
		});
		
		UnityThreadHelper.CreateThread(() =>
		                               {
			NetworkStream streamRight = tcpListenerRight.AcceptTcpClient().GetStream();
			Debug.Log("Right Connection accepted.");
			
			streamReaderRight = new StreamReader(streamRight);
			right = true;
			HandOverStreamReader();

		});

	}
	
	void HandOverStreamReader () {

		if (left && right) {
			playerManager.AddPhonePlayer(streamReaderLeft, streamReaderRight);
			right = false;
			left = false;
			InitializeListener ();
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
			


/**gameObject.transform.rotation = Quaternion.AngleAxis(angleRight, Vector3.up);
		
		Vector3 newPosition = gameObject.transform.position;
		
		newPosition.x += (float)(Math.Cos (DegreeToRadian (angleLeft)) * distanceLeft * 0.001);
		newPosition.z += (float)(Math.Sin (DegreeToRadian (angleLeft)) * distanceLeft * 0.001);
		
		gameObject.transform.position = newPosition;**/

//string responseString = "You have successfully connected to me";

//Forms and sends a response string to the connected client.
//Byte[] sendBytes = Encoding.ASCII.GetBytes(responseString);
//stream.Write(sendBytes, 0, sendBytes.Length);
//Debug.Log("Message Sent /> : " + responseString);
