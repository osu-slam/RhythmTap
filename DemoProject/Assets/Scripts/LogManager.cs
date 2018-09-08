using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class LogManager {
	
    private static LogManager instance;
    internal static LogManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new LogManager();
            }
            return instance;
        }
    }
    internal string path;
    private LogManager()
    {
		string directory = Path.Combine("", "Log");
        if (!Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }
		string filename =  WelcomeController.name + "_" + DateTime.Now.Month.ToString() + "_" + DateTime.Now.Day.ToString() + "_" + DateTime.Now.Hour.ToString() + "_" + DateTime.Now.Minute.ToString() + ".csv" ;
		path = Path.Combine(directory, filename);
    }
        
    internal void Log(float onsetTime, int index)
    {
		string contents = index.ToString() + "," + onsetTime.ToString() +  "\n";
        File.AppendAllText(path, contents);
    }

	internal void Log(float eOnsetTime, float aOnsetTime,/* float eDuration, float aDuration,*/ int index)
	{
		string contents = "";
		if (index > -1)
			contents += index.ToString ();

		contents += "," + 
			eOnsetTime.ToString() + "," +
			aOnsetTime.ToString() + "," + 
			(eOnsetTime - aOnsetTime).ToString() + "," +
			/*eDuration.ToString() + "," +
			aDuration.ToString() + "," +
			(eDuration - aDuration).ToString() + "," + */"\n";
		File.AppendAllText(path, contents);
	}

	internal void Log(string arg1, string arg2,string arg3)
	{
		string contents = arg1 + "," + arg2 + "," + arg3 + "," + "\n";
		File.AppendAllText(path, contents);
	}

	internal void Log(string arg1, string arg2)
	{
		string contents = arg1 + "," + arg2 + ","  + "\n";
		File.AppendAllText(path, contents);
	}


	/* log the start Date */
	internal void Log()
	{
		string contents = DateTime.Now +"\n";
		File.AppendAllText(path, contents);
	}


    internal void Log(string type, string[] args)
    {
        string contents = DateTime.Now + "," + type;
        for (int i = 0; i < args.Length; i++)
        {
            contents += "," + args[i];
        }
        contents += "," + DateTime.Now + "\n";
        File.AppendAllText(path, contents);
    }
    internal enum SongType
    {
        MP3PROVIDED,
        MP3PERSONAL,
        YOUTUBE,
    }
	internal void LogSessionStart(float bpm, int gameNum)
    {
        Log("Session", "Start", DateTime.Now.ToString());
		Log ("Game Number", gameNum.ToString());
		Log("BPM", bpm.ToString("f4"));
		Log ("Onset Error", '\u00B1' + " 0.1");
		Log ("Duration Threshold", "0.85");
		Log ("Response Lag", "0.15");
		Log ("Index", "Onset_Expected, Onset_Actual, Onset_Diff", "Dur_Expected, Dur_Actual, Dur_Diff");
        //Log("Session", new string[] { "Song", title, type.ToString()});
       // Log("Session", "Difficulty", difficulty.ToString());
        //Log("Session", "Drums", drums.ToString());
        //Log("Session", "TimeSignature", timeSignature);
    }
}
