﻿using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;
using UnityEngine.UI;


public class PhPHandler : MonoBehaviour {

    private string secretKey = "mySecretKey";
    public string AddUserURL = "https://explosethething.000webhostapp.com/AddUser.php?";
    public string AddScoresURL = "https://explosethething.000webhostapp.com/AddScore.php?";
    public string ShowLevelScoresURL = "https://explosethething000webhostapp.com/ShowLevelScores.php?";
    public string ShowUsersURL = "https://explosethething.000webhostapp.com/ShowUsers.php?";

    public Text Output;

    public InputField User;
    public InputField Score;
    public InputField Level;

    public static string CreateMD5(string input)
    {
        MD5 md5 = MD5.Create();
        byte[] inputBytes = Encoding.ASCII.GetBytes(input);
        byte[] hashBytes = md5.ComputeHash(inputBytes);

        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < hashBytes.Length; i++)
        {
            sb.Append(hashBytes[i].ToString("X2"));
        }

        return sb.ToString();
    }

    public void OnShowUsersButtonClick()
    {
        Output.text = GetAllUsers();
    }

    public void OnShowScoresButtonClick()
    {
        Output.text = GetLevelScores(Level.text);
    }

    public void OnAddUserButtonClick()
    {
        AddUser(User.text);
    }

    public string GetLevelScores(string level)
    {
        WWW GetScores = new WWW("https://explosethething.000webhostapp.com/ShowLevelScores.php?" + "Level=" + WWW.EscapeURL(Level.text));


        while (!GetScores.isDone) { }

        if (GetScores.error != null)
        {
            print("There was an error getting Scores: " + GetScores.error);
        }

        return GetScores.text;
    }

    public string GetAllUsers()
    {
        //Works only with string literal for some reason
        WWW GetUsers = new WWW("https://explosethething.000webhostapp.com/ShowUsers.php?"); 
        
        

        while (!GetUsers.isDone) {}

        if (GetUsers.error != null)
        {
            print("There was an error getting users: " + GetUsers.error);
        }

        return GetUsers.text;

    }

    public void AddUser(string User)
    {
        string hash = CreateMD5(User + secretKey);
        string post_url = AddUserURL + "Name=" + WWW.EscapeURL(User) + "&hash=" + hash;

        WWW post = new WWW("https://explosethething.000webhostapp.com/AddUser.php?" + "Name=" + WWW.EscapeURL(User) + "&hash=" + hash);
        while (!post.isDone) {}
        if (post.error != null)
        {
            print("There was an error posting user data: " + post.error);
        }

    }

    public void AddScore(string User, string Score, string Level)
    {
        string hash = CreateMD5(User + secretKey);
        string post_url = AddUserURL + "Name=" + WWW.EscapeURL(User) + "&hash=" + hash;

        WWW post = new WWW("https://explosethething.000webhostapp.com/AddScore.php?" + "Name=" + WWW.EscapeURL(User) + "Score=" + WWW.EscapeURL(Score) + "Level=" + WWW.EscapeURL(Level) + "&hash=" + hash);
        while (!post.isDone) { }
        if (post.error != null)
        {
            print("There was an error posting score data: " + post.error);
        }

    }
}

