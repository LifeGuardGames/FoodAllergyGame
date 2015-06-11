using UnityEngine;
using System;
using System.Collections;
using System.Reflection;
using System.Collections.Generic;


//---------------------------------------------------
// StringUtils
// Utility functions for formatting/localizing strings.
// Jason: A better way to do this is by using System.String.Format ....too late to change now
//---------------------------------------------------

public class StringUtils {

	public static string FormatStringPossession(string oldString){
		if(string.IsNullOrEmpty(oldString)){
			Debug.LogWarning("Formatting an empty string");
			return "";
		}

		string subString = oldString.Substring(oldString.Length - 1);
		string newString = "";

		if(subString == "s")
			newString = oldString + "'";
		else
			newString = oldString + "'s";

		return newString;
	}

	public static Vector3 ParseVector3(string vectorString){
		Vector3 vector = new Vector3(0, 0, 0);
		String[] arrayVector3;
		
		try{
			arrayVector3 = vectorString.Split(","[0]);
			if(arrayVector3.Length == 3){
				vector = new Vector3(
					float.Parse(arrayVector3[0].Trim(new char[]{'(' })), 
					float.Parse(arrayVector3[1]), 
					float.Parse(arrayVector3[2].Trim(new char[]{')' })));
			}
			else
				Debug.LogError("Illegal vector3 parsing, reverting to 0,0,0");
		}
		catch(NullReferenceException e){
			Debug.LogError("Vector3 parsing. string cannot be null. error message: " + e.Message);
		}
		return vector;
	}

	/// <summary>
	/// Formats the int to double digit string.
	/// Useful for making integer counts into string for XML use
	/// </summary>
	/// <returns>Double digit string representation of the number</returns>
	/// <param name="number">Number to convert</param>
	public static string FormatIntToDoubleDigitString(int number){
		if(number > 99 || number < 0){
			Debug.LogError("Unsupported input for number detected " + number);
			return "00";
		}
		else{
			if(number > 9){
				return number.ToString();
			}
			else{
				return "0" + number.ToString();
			}
		}
	}

	/// <summary>
	/// Formats a TimeSpan into a xH:xM:xS format
	/// </summary>
	/// <returns>The time left.</returns>
	/// <param name="timeLeft">Time left</param>
	public static string FormatTimeLeft(TimeSpan timeLeft){
		string displayTime = "";
		if(timeLeft.Hours > 0){
			displayTime = string.Format("{0}[FFFF33]h[-] {1}[FFFF33]m[-] {2}[FFFF33]s[-]", 
			                            timeLeft.Hours, timeLeft.Minutes, timeLeft.Seconds);
		}
		else if(timeLeft.Minutes > 0){
			displayTime = string.Format("{0}[FFFF33]m[-] {1}[FFFF33]s[-]", timeLeft.Minutes, timeLeft.Seconds);
		}
		else{
			displayTime = string.Format("{0}[FFFF33]s[-]", timeLeft.Seconds);
		}
		return displayTime;
	}
}
