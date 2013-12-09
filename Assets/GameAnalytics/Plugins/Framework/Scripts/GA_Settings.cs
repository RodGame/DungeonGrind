//#define IOS_ID
//#define ANDROID_ID

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;

#if UNITY_METRO && !UNITY_EDITOR
using GA_Compatibility.Collections;
#endif

/// <summary>
/// GA test. This should be instances as an asset and 
/// </summary>
/// 
public class GA_Settings : ScriptableObject
{
	#if UNITY_IPHONE && !UNITY_EDITOR && IOS_ID
	[DllImport ("__Internal")]
	private static extern string GetUserID ();
	#endif
	
	#if UNITY_ANDROID && !UNITY_EDITOR && ANDROID_ID
	private const string ANDROID_CLASS_NAME = "com.yourcompany.yourgame";
	#endif
	
	/// <summary>
	/// Types of help given in the help box of the GA inspector
	/// </summary>
	public enum HelpTypes { None, FpsCriticalAndTrackTargetHelp, GuiAndTrackTargetHelp, IncludeSystemSpecsHelp, ProvideCustomUserID };
	public enum MessageTypes { None, Error, Info, Warning };
	
	/// <summary>
	/// A message and message type for the help box displayed on the GUI inspector
	/// </summary>
	public struct HelpInfo
	{
		public string Message;
		public MessageTypes MsgType;
		public HelpTypes HelpType;
	}
	
	#region public static values
	
	/// <summary>
	/// The version of the GA Unity Wrapper plugin
	/// </summary>
	[HideInInspector]
	public static string VERSION = "0.5.1";
	
	#endregion
	
	#region public values
	
	public int TotalMessagesSubmitted;
	public int TotalMessagesFailed;
	
	public int DesignMessagesSubmitted;
	public int DesignMessagesFailed;
	public int QualityMessagesSubmitted;
	public int QualityMessagesFailed;
	public int BusinessMessagesSubmitted;
	public int BusinessMessagesFailed;
	public int UserMessagesSubmitted;
	public int UserMessagesFailed;
	
	public string CustomArea = string.Empty;
	
	//Set the track target to use for predefined events, such as CriticalFPS (position of track target is sent with these events).
	public Transform TrackTarget;
	
	[SerializeField]
	public string GameKey = "";
	[SerializeField]
	public string SecretKey = "";
	[SerializeField]
	public string ApiKey = "";
	[SerializeField]
	public string Build = "0.1";
	public bool DebugMode = true;
	public bool SendExampleGameDataToMyGame = false;
	public bool RunInEditorPlayMode = true;
	
	public bool AllowRoaming = false;
	public bool ArchiveData = false;
	public bool NewSessionOnResume = true;
	public Vector3 HeatmapGridSize = Vector3.one;

	//bytes
	public long ArchiveMaxFileSize = 2000;
	public bool CustomUserID;
	public float SubmitInterval = 10;
	
	public bool InternetConnectivity;
	
	//These values are used for the GA_Inspector only
	public enum InspectorStates { Basic, QA, Debugging, Data, Pref }
	public InspectorStates CurrentInspectorState;
	public List<HelpTypes> ClosedHints = new List<HelpTypes>();
	public bool DisplayHints;
	public Vector2 DisplayHintsScrollState;
	public Texture2D Logo;

	#endregion
	
	#region public methods
	
	/// <summary>
	/// Help/hints/tips messages used for the GA inspector hints box. This function decides which hint to display.
	/// Garbos: Depricated because: Was duplicated to return list of messages instead. Use GetHelpMessageList
	/// </summary>
	/// <returns>
	/// The help message.
	/// </returns>
	public List<HelpInfo> GetHelpMessageList()
	{
		List<HelpInfo> result = new List<HelpInfo>();
		
		if (GameKey.Equals("") || SecretKey.Equals(""))
		{
			result.Add( new HelpInfo() { Message = "Please fill in your Game Key and Secret Key, obtained from the GameAnalytics website where you created your game.", MsgType = MessageTypes.Warning, HelpType = HelpTypes.None });
		}
		
		if (Build.Equals(""))
		{
			result.Add( new HelpInfo() { Message = "Please fill in a name for your build, representing the current version of the game. Updating the build name for each version of the game will allow you to filter by build when viewing your data on the GA website.", MsgType = MessageTypes.Warning, HelpType = HelpTypes.None });
		}
		
		if (CustomUserID && !ClosedHints.Contains(HelpTypes.ProvideCustomUserID))
		{
			result.Add( new HelpInfo() { Message = "You have indicated that you will provide a custom user ID - no data will be submitted until it is provided. This should be defined from code through: GA.Settings.SetCustomUserID", MsgType = MessageTypes.Info, HelpType = HelpTypes.ProvideCustomUserID });
		}
		
		return result;
	}
	
	/// <summary>
	/// Help/hints/tips messages used for the GA inspector hints box. This function decides which hint to display.
	/// Garbos: Depricated because: Was duplicated to return list of messages instead. Use GetHelpMessageList
	/// </summary>
	/// <returns>
	/// The help message.
	/// </returns>
	public HelpInfo GetHelpMessage()
	{
		if (GameKey.Equals("") || SecretKey.Equals(""))
		{
			return new HelpInfo() { Message = "Please fill in your Game Key and Secret Key, obtained from the GameAnalytics website where you created your game.", MsgType = MessageTypes.Warning, HelpType = HelpTypes.None };
		}
		else if (Build.Equals(""))
		{
			return new HelpInfo() { Message = "Please fill in a name for your build, representing the current version of the game. Updating the build name for each version of the game will allow you to filter by build when viewing your data on the GA website.", MsgType = MessageTypes.Warning, HelpType = HelpTypes.None };
		}
		
		else if (CustomUserID && !ClosedHints.Contains(HelpTypes.ProvideCustomUserID))
		{
			return new HelpInfo() { Message = "You have indicated that you will provide a custom user ID - no data will be submitted until it is provided. This should be defined from code through: GA.Settings.SetCustomUserID", MsgType = MessageTypes.Info, HelpType = HelpTypes.ProvideCustomUserID };
		}
		
		return new HelpInfo() { Message = "No hints to display. The \"Reset Hints\" button resets closed hints.", MsgType = MessageTypes.None, HelpType = HelpTypes.None };
	}
	
	/// <summary>
	/// Checks the internet connectivity, and sets INTERNETCONNECTIVITY
	/// </summary>
	public IEnumerator CheckInternetConnectivity(bool startQueue)
	{
		// Application.internetReachability returns the type of Internet reachability currently possible on the device, but does not check if the there is an actual route to the network. So we can check this instantly.
		if (Application.internetReachability == NetworkReachability.ReachableViaCarrierDataNetwork && !GA.SettingsGA.AllowRoaming)
		{
			InternetConnectivity = false;
		}
		else
		{
			//Try to ping the server
			WWW www = new WWW(GA.API.Submit.GetBaseURL(true) + "/ping");
				
			//Wait for response
			yield return www;
			
			try
			{
				if (GA.API.Submit.CheckServerReply(www))
				{
					InternetConnectivity = true;
				}
				else if (www.error != null)
				{
					InternetConnectivity = false;
				}
				else
				{
					//Get the JSON object from the response
					Hashtable returnParam = (Hashtable)GA_MiniJSON.JsonDecode(www.text);
					
					//If the response contains the key "status" with the value "ok" we know that we are connected
					if (returnParam != null && returnParam.ContainsKey("status") && returnParam["status"].ToString().Equals("ok"))
					{
						InternetConnectivity = true;
					}
					else
					{
						InternetConnectivity = false;
					}
				}
			}
			catch
			{
				InternetConnectivity = false;
			}
		}
		
		if (startQueue)
		{
			if (InternetConnectivity)
				GA.Log("GA initialized, waiting for events..");
			else
				GA.Log("GA detects no internet connection..");
			
			//Add additional IDs
			AddUniqueIDs();
			
			//Start the submit queue for sending messages to the server
			GA.RunCoroutine(GA_Queue.SubmitQueue());
			GA.Log("GameAnalytics: Submission queue started.");
			
			#if UNITY_EDITOR
			
			GameObject gaTracking = new GameObject("GA Trackeing");
			gaTracking.AddComponent<GA_Tracking>();
			
			#endif
		}
	}
	
	private void AddUniqueIDs()
	{
		#if !UNITY_EDITOR
		
		string os = "";
		string[] osSplit = SystemInfo.operatingSystem.Split(' ');
		if (osSplit.Length > 0)
			os = osSplit[0];
		
		#endif
		
		#if UNITY_IPHONE && !UNITY_EDITOR && IOS_ID
		
		try
		{
			string iOSid = GetUniqueIDiOS();
			if (iOSid != null && iOSid != string.Empty)
			{
				GA.API.User.NewUser(GA_User.Gender.Unknown, null, null, iOSid, null, GA.API.GenericInfo.GetSystem(), SystemInfo.deviceModel, os, SystemInfo.operatingSystem, "GA Unity SDK " + VERSION);
			}
		}
		catch
		{ }
		
		#elif UNITY_ANDROID && !UNITY_EDITOR && ANDROID_ID
		
		try
		{
			AndroidJNI.AttachCurrentThread();
			
			using (AndroidJavaClass cls_UnityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer")) {
				
				using (AndroidJavaObject obj_Activity = cls_UnityPlayer.GetStatic<AndroidJavaObject>("currentActivity")) {
					
					AndroidJavaClass cls_AndroidID = new AndroidJavaClass(ANDROID_CLASS_NAME + ".GA_Android");
					
					string androidID = cls_AndroidID.CallStatic<string>("GetDeviceId");
					GA.API.User.NewUser(GA_User.Gender.Unknown, null, null, null, androidID, GA.API.GenericInfo.GetSystem(), SystemInfo.deviceModel, os, SystemInfo.operatingSystem, "GA Unity SDK " + VERSION);
				}
			}
		}
		catch
		{ }
		
		#elif UNITY_FLASH && !UNITY_EDITOR
		
		GA.API.User.NewUser(GA_User.Gender.Unknown, null, null, null, null, GA.API.GenericInfo.GetSystem(), "Flash", os, SystemInfo.operatingSystem, "GA Unity SDK " + VERSION);
		
		#elif !UNITY_EDITOR
		
		GA.API.User.NewUser(GA_User.Gender.Unknown, null, null, null, null, GA.API.GenericInfo.GetSystem(), SystemInfo.deviceModel, os, SystemInfo.operatingSystem, "GA Unity SDK " + VERSION);
		
		#endif
	}
	
	public string GetUniqueIDiOS ()
	{
		string uid = "";
		
		#if UNITY_IPHONE && !UNITY_EDITOR && IOS_ID
		uid = GetUserID();
		#endif
		
		#if UNITY_IPHONE && UNITY_EDITOR && !IOS_ID
		GA.LogWarning("GA Warning: Remember to read the iOS_Readme in the GameAnalytics > Plugins > iOS folder, for information on how to setup advertiser ID for iOS. GA will not work on iOS if you do not follow these steps.");
		#endif
		
		return uid;
	}
		
	/// <summary>
	/// Sets a custom user ID.
	/// Make sure each user has a unique user ID. This is useful if you have your own log-in system with unique user IDs.
	/// NOTE: Only use this method if you have enabled "Custom User ID" on the GA inspector!
	/// </summary>
	/// <param name="customID">
	/// The custom user ID - this should be unique for each user
	/// </param>
	public void SetCustomUserID(string customID)
	{
		if (customID != string.Empty)
		{
			GA.API.GenericInfo.SetCustomUserID(customID);
		}
	}
	
		
	/// <summary>
	/// Sets a custom area string. An area is often just a level, but you can set it to whatever makes sense for your game. F.x. in a big open world game you will probably need custom areas to identify regions etc.
	/// By default, if no custom area is set, the Application.loadedLevelName string is used.
	/// </summary>
	/// <param name="customID">
	/// The custom area.
	/// </param>
	public void SetCustomArea(string customArea)
	{
		CustomArea = customArea;
	}
	
	#endregion
}
