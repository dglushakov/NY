using UnityEngine;
using UnityEngine.Networking;
using System;
using System.IO;
using System.Net;
using System.Threading;
using System.Collections.Generic;

public class UnityHttpListener : MonoBehaviour
{
	List<cameraRotationData> messageQueue = new List<cameraRotationData>();

	private HttpListener listener;
	private Thread listenerThread;

	Camera managedCamera;

	private int port = 4444;

	class cameraRotationData
	{
		public string cameraName;
		public int x;
		public int y;
		public int z;
	}

	void Start()
	{
		var mainThreadContext = System.Threading.SynchronizationContext.Current;

		listener = new HttpListener();
		listener.Prefixes.Add("http://localhost:4444/");
		listener.Prefixes.Add("http://127.0.0.1:4444/");
		listener.Prefixes.Add($"http://*:{port}/");
		listener.AuthenticationSchemes = AuthenticationSchemes.Anonymous;
		listener.Start();

		listenerThread = new Thread(startListener);
		listenerThread.Start();
		Debug.Log("Server Started");


	}

	void Update()
	{

		while (messageQueue.Count > 0)
		{
			cameraRotationData newData = messageQueue[0];
			Debug.Log("Rotate" + newData.cameraName + " to: " + newData.x + " " + newData.y + " " + newData.z);

			managedCamera = GameObject.FindWithTag(newData.cameraName).GetComponent<Camera>();
			managedCamera.transform.localEulerAngles = new Vector3(1, 2, 3);

			messageQueue.RemoveAt(0);
			managedCamera.transform.localEulerAngles = new Vector3(newData.x, newData.y, newData.z);
		}
	}

	private void startListener()
	{
		while (true)
		{
			var result = listener.BeginGetContext(ListenerCallback, listener);
			result.AsyncWaitHandle.WaitOne();


		}
	}

	private void ListenerCallback(IAsyncResult result)
	{
		var context = listener.EndGetContext(result);


		//Debug.Log("Method: " + context.Request.HttpMethod);
		//Debug.Log("LocalUrl: " + context.Request.Url.LocalPath);
		cameraRotationData data = new cameraRotationData();


		if (context.Request.QueryString.AllKeys.Length > 0)
			foreach (var key in context.Request.QueryString.AllKeys)
			{
				//Debug.Log("Key: " + key + ", Value: " + context.Request.QueryString.GetValues(key)[0]);
				if (key == "cameraName")
				{
					data.cameraName = context.Request.QueryString.GetValues(key)[0];
				}
				if (key == "x")
				{
					data.x = Int32.Parse(context.Request.QueryString.GetValues(key)[0]);
				}
				if (key == "y")
				{
					data.y = Int32.Parse(context.Request.QueryString.GetValues(key)[0]);
				}
				if (key == "z")
				{
					data.z = Int32.Parse(context.Request.QueryString.GetValues(key)[0]);
				}


			}

		if (context.Request.HttpMethod == "POST")
		{
			Thread.Sleep(1000);
			var data_text = new StreamReader(context.Request.InputStream,
								context.Request.ContentEncoding).ReadToEnd();
			//Debug.Log(data_text);
		}

		Debug.Log("Send");
		messageQueue.Add(data);
		context.Response.Close();
	}




}

