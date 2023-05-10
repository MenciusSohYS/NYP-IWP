using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using System.IO;


public static class TxtHandler
{
    public static string TextToWrite = "Default Text for TextToWrite";
    public static string CurrencyToWrite = "C123";
    public static string path = @"PlayerStats.txt"; //set path

    public static void CreateTextFile()
    {
        File.WriteAllText(path, CurrencyToWrite + Environment.NewLine); //write if it doesnt exist

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
        int currency = 0;

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
            for (int j = 0; j < filedata[i].Length; ++i) //search the row of each line
            {
                if (filedata[i][j] == WhatToFind) //look for whattofind which will mean currency
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

        string CurrencyAsString = "";

        for (int i = whichlocation; i < filedata[whichline].Length; ++i)
        {
            if (filedata[whichline][i] < 48 || filedata[whichline][i] > 57) //use ascii to find out if its a valid number
            {
                Debug.Log("Non-number detected, should only have numbers: " + filedata[whichline][i]);
                return 0;
            }
            CurrencyAsString += filedata[whichline][i]; //if its valid, add it to the back of the string
        }

        currency = int.Parse(CurrencyAsString); //convert the string to int and return it

        return currency;
    }
}
