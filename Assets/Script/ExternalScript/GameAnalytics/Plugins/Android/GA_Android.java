package com.yourcompany.yourgame;

import com.unity3d.player.UnityPlayerActivity;

import android.app.Activity;
import android.content.Context;
import android.provider.Settings.Secure;
import android.telephony.TelephonyManager;
import android.util.Log;
import java.io.File;
import java.security.MessageDigest;
import java.security.NoSuchAlgorithmException;
 
import android.os.Bundle;

public class GA_Android extends UnityPlayerActivity
{
    private static android.content.ContentResolver contentResolver;
     
    @Override
    protected void onCreate(Bundle savedInstanceState)
    {
        // call UnityPlayerActivity.onCreate()
        super.onCreate(savedInstanceState);

        contentResolver = getContentResolver();
    }
     
    public static String GetDeviceId()
    {
        // Get the device ID
        String auid = "android_id " + Secure.getString(contentResolver, Secure.ANDROID_ID);
        //String auid = "android_id " + android.provider.Settings.Secure.ANDROID_ID;
        
        /*
        TelephonyManager tm = (TelephonyManager)context.getSystemService(Context.TELEPHONY_SERVICE);
        if(tm != null)
        {
             try{
             auid = tm.getDeviceId();
             }
             catch (SecurityException e){
                  e.printStackTrace();
             }
             tm = null;
        }
        if(((auid == null) || (auid.length() == 0)) && (context != null))
             auid = Secure.getString(context.getContentResolver(), Secure.ANDROID_ID);
        if((auid == null) || (auid.length() == 0))
             auid = null;
        */
         
        return auid;    
    }
}