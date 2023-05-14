using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using System.IO;


public static class TxtHandler
{
    public static string TextToWrite = "Default Text for TextToWrite";
    public static string CurrencyToWrite = "C0";
    public static string path = @"PlayerStats.txt"; //set path
    public static bool UnlockedMercenary = false;
    public static bool UnlockedBounty = true;

    public static void CreateTextFile()
    {
        string TempText = "";

        if (UnlockedMercenary)
            TempText += "M1" + Environment.NewLine;
        //add more to check for other characters

        TempText += CurrencyToWrite + Environment.NewLine;
        //add the currency to the string and push it

        File.WriteAllText(path, TempText);
        //write if it doesnt exist

        //dont need this right now
        {
            //if (!File.Exists(path))
            //{
            //    File.WriteAllText(path, TextToWrite); //write if it doesnt exist
            //}
            //else
            //{
            //    File.AppendAllText(path, CurrencyToWrite + Environment.NewLine); //add to if it does
            //}
        }
    }

    public static string[] ReadFile()
    {
        return File.ReadAllLines(path); //read and return from the file
    }

    public static int FindOneIntValue(char WhatToFind)
    {
        int IntToReturn = 0;

        string[] filedata = File.ReadAllLines(path);

        if (filedata.Length == 0) //check if file is empty
        {
            Debug.Log("Empty file");
            return 0;
        }

        int whichline = 0;
        int whichlocation = 0;

        for (int i = 0; i < filedata.Length; ++i) //search the lines
        {
            //Debug.Log(filedata[i]);
            for (int j = 0; j < filedata[i].Length; ++j) //search the row of each line
            {
                //Debug.Log("i: " + i + " j: " + j);
                if (filedata[i][j] == WhatToFind) //look for whattofind which will mean the letter that is associated with the line
                {
                    if (filedata[i].Length == j + 1)  //check if whattofind is the last element
                    {
                        Debug.Log(filedata[i].Length + "+" + j + 1 + " " + filedata[i]);
                        return 0;
                    }
                    whichline = i; //store the line
                    whichlocation = j + 1; //store the row of the line
                    i = filedata.Length + 1; //break only leaves this for loop, we make i bigger than the goal to also move it out of the bigger for loop
                    break;
                }
            }
        }

        string IntAsString = "";

        for (int i = whichlocation; i < filedata[whichline].Length; ++i)
        {
            if (filedata[whichline][i] < 48 || filedata[whichline][i] > 57) //use ascii to find out if its a valid number
            {
                Debug.Log("Non-number detected, should only have numbers: " + filedata[whichline][i]);
                return 0;
            }
            IntAsString += filedata[whichline][i]; //if its valid, add it to the back of the string
        }
        if (IntAsString == "")
            return 0;

        IntToReturn = int.Parse(IntAsString); //convert the string to int and return it

        return IntToReturn;
    }

    public static void CheckForCharacters()
    {
        if (FindOneIntValue('M') != 0)
        {
            UnlockedMercenary = true;
        }
    }
}
