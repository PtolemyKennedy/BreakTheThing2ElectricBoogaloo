using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System;
using UnityEngine;
using UnityEngine.UI;


public class PhPHandler : MonoBehaviour {

    private string secretKey = "mySecretKey";
    public Uri AddUserURL = new Uri("https://explosethething.000webhostapp.com/AddUser.php?");
    public Uri AddScoresURL = new Uri("https://explosethething.000webhostapp.com/AddScore.php?");
    public Uri ShowLevelScoresURL = new Uri("https://explosethething.000webhostapp.com/ShowLevelScores.php?");
    public Uri ShowUsersURL = new Uri("https://explosethething.000webhostapp.com/ShowUsers.php?");
    public Uri FindUserURL = new Uri("https://explosethething.000webhostapp.com/FindUser.php?");
    public Uri FindLevelURL = new Uri("https://explosethething.000webhostapp.com/FindLevel.php?");

    Uri showlevelscoresurl = new Uri("https://explosethething.000webhostapp.com/ShowLevelScores.php?");
    public string Output;
    public string[] MultiOutput;
    public string User;
    public string Score;
    public string Level;

    private static string CreateMD5(string input)
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

    /// <summary>
    /// Populates a textfield 'Output' with the users
    /// </summary>
    public void OnShowUsersButtonClick()
    {
        MultiOutput = GetAllUsers();
    }

    /// <summary>
    /// Populates a textfield 'Output' with names and scores for a level, the level is defined by a number given in an inputfield 'Level'
    /// </summary>
    public void OnShowScoresButtonClick()
    {
        Output = GetLevelScores(Level);
    }

    /// <summary>
    /// Adds to the USER table based on the content of 'User' Inputfield
    /// </summary>
    public void OnAddUserButtonClick()
    {
        AddUser(User);
    }

    /// <summary>
    /// Adds the the SCORES table based on the contents of the 'User', 'Score' and 'Level' Inputfields
    /// </summary>
    public void OnAddScoreButtonClick()
    {
        AddScore(User, Score, Level);
    }

    public void ConvertNamesToIDs()
    {
        User = ConvertUserToId(User);
        print("Getting id of level " + Level);
        Level = ConvertLevelToId(Level);
        print("Id is " + Level);
    }

    

    private string GetLevelScores(string level)
    {
        WWW GetScores = new WWW(showlevelscoresurl.OriginalString + "Level=" + WWW.EscapeURL(Level));

        while (!GetScores.isDone) { }

        if (GetScores.error != null)
        {
            Output = ("There was an error getting Scores: " + GetScores.error);
        }

        return GetScores.text;
    }

    private string[] GetAllUsers()
    {
        //Works only with string literal for some reason
        WWW GetUsers = new WWW(ShowUsersURL.OriginalString);               

        while (!GetUsers.isDone) {}

        if (GetUsers.error != null)
        {
            print("There was an error getting users: " + GetUsers.error);
        }

        string[] returnval = GetUsers.text.Split(' ');
        return returnval;
    }

    private string ConvertUserToId(string user)
    {
        WWW FindUser = new WWW(FindUserURL.OriginalString + "Name="+WWW.EscapeURL(user));                 

        while (!FindUser.isDone) {}

        if (FindUser.error != null)
        {
            print("There was an error getting users: " + FindUser.error);
        }

        string[] returnval = FindUser.text.Split(' ');
        return returnval[0];
    }

    private string ConvertLevelToId(string level)
    {
        WWW FindLevel = new WWW(FindLevelURL.OriginalString + "Level=" + WWW.EscapeURL(level));
        while (!FindLevel.isDone) { }

        print("full return is " + FindLevel.text);

        if (FindLevel.error != null)
        {
            print("There was an error getting users: " + FindLevel.error);
        }

        string[] returnval = FindLevel.text.Split(' ');
        return returnval[0];
    }

    private void AddUser(string User)
    {
        string hash = CreateMD5(User + secretKey);
        string post_url = AddUserURL + "Name=" + WWW.EscapeURL(User) + "&hash=" + hash;

        WWW post = new WWW(AddUserURL.OriginalString + "Name=" + WWW.EscapeURL(User) + "&hash=" + hash);
        while (!post.isDone) {}
        if (post.error != null)
        {
            print("There was an error posting user data: " + post.error);
        }
    }


    private void AddScore(string User, string Score, string Level)
    {
        print("User = " + User + " Score = " + Score + " Level = " + Level);
        string hash = CreateMD5(User + secretKey);
        Uri post_url = new Uri(AddScoresURL +"Name=" + WWW.EscapeURL(User) + "&Score=" + WWW.EscapeURL(Score) + "&Level=" + WWW.EscapeURL(Level) + "&hash=" + hash);
        print(post_url.OriginalString);
        WWW post = new WWW(post_url.OriginalString);

        while (!post.isDone) { print("sending"); }
        if (post.error != null)
        {
            print("There was an error posting score data: " + post.error);
        }
    }
}

