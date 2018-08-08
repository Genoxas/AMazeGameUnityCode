using System;
using System.Runtime.Serialization.Formatters;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;
using System.Xml;
using System.Collections;
using System.Collections.Generic;

public class AccountRetriever : MonoBehaviour
{

  private string ip = "http://amazegame-project.ddns.net:47225/";
    private string data;
    public Account account;
    private string InputUsername;
    private string InputPassword;
    public string username="";

    //Get Account Data from Web API using user and password
    IEnumerator GetAccount()
    {
      string url = ip + "api/accounts/user=" + InputUsername + "|pass=" + InputPassword;
      //Debug.Log("Getting Account Info from " + url);
      WWW playerAccount = new WWW(url);
      while (!playerAccount.isDone)
      {

      }
      //Debug.Log(playerAccount.text);
      //Debug.Log("Done");
      string playerData = playerAccount.text;
      playerData = playerData.Substring(1, (playerData.Length - 2)); ;
      string[] fieldData = playerData.Split(',');
      //Debug.Log("Data in String: " + playerData);

      List<string> info = new List<string>();
      foreach (string field in fieldData)
      {
          string [] temp = field.Split(':');
          info.Add(temp[1]);
      }
      account = new Account();
      account.Id = Convert.ToInt32(info[0]);
      account.Username = info[1].Trim('"');
      account.Password = info[2].Trim('"'); ;
      account.GamesPlayed = Convert.ToInt32(info[3]);
      account.GamesWon = Convert.ToInt32(info[4]);
      account.Kills = Convert.ToInt32(info[5]);
      account.Deaths = Convert.ToInt32(info[6]);
      account.ItemsUsed = Convert.ToInt32(info[7]);
      account.PuzzlesCompleted = Convert.ToInt32(info[8]);
      username = account.Username;
      yield return playerAccount;
    }

    public bool GetData(string user, string password)
    {
       InputUsername = user;
       InputPassword = password;
       StartCoroutine("GetAccount");
       if (username.Equals(""))
        return false;
       return true;
       
    }

    private IEnumerator GetAccountWithUser()
    {
      string url = ip + "api/accounts/user=" + InputUsername;
      //Debug.Log("Getting Account Info from " + url);
      WWW playerAccount = new WWW(url);
      while (!playerAccount.isDone)
      {

      }
      //Debug.Log(playerAccount.text);
      //Debug.Log("Done");
      string playerData = playerAccount.text;
      playerData = playerData.Substring(1, (playerData.Length - 2)); ;
      string[] fieldData = playerData.Split(',');
      //Debug.Log("Data in String: " + playerData);

      List<string> info = new List<string>();
      foreach (string field in fieldData)
      {
        string[] temp = field.Split(':');
        info.Add(temp[1]);
      }
      username = info[1].Trim('"');
      yield return playerAccount;
    }

    public bool checkIfAccountExist(string user)
    {
      InputUsername = user;
      StartCoroutine("GetAccountWithUser");
      if (username.Equals(""))
      { 
        return false;
      }
      username = "";
      return true;
    }

    public void createAccount(string user, string password)
    {
      InputUsername = user;
      InputPassword = password;
      StartCoroutine("CreateAccountRequest");
    }

    private IEnumerator CreateAccountRequest()
    {
      string url = ip + "api/accounts/";
      WWWForm form = new WWWForm();
      form.AddField("Username", InputUsername);
      form.AddField("Password", InputPassword);
      WWW playerAccount = new WWW(url,form);
      while (!playerAccount.isDone)
      {

      }
      yield return playerAccount;
    }


    public void updateStats()
    {
      StartCoroutine("UpdateAccountRequest");
    }

    private IEnumerator UpdateAccountRequest()
    {
      string url = ip + "api/accounts/" + account.Id + "/update";
      WWWForm form = new WWWForm();
      account.GamesPlayed++;
      form.AddField("Id",account.Id);
      form.AddField("GamesPlayed", account.GamesPlayed);
      form.AddField("GamesWon", account.GamesWon);
      form.AddField("Kills", account.Kills);
      form.AddField("Deaths", account.Deaths);
      form.AddField("ItemsUsed", account.ItemsUsed);
      form.AddField("PuzzlesCompleted", account.PuzzlesCompleted);
      WWW playerAccount = new WWW(url, form);
      while (!playerAccount.isDone)
      {

      }
      Debug.Log("Update Completed");
      yield return playerAccount;
    }

   


}

public class Account
{
  public int Id { get; set; }
  public string Username { get; set; }
  public string Password { get; set; }
  public int GamesPlayed { get; set; }
  public int GamesWon { get; set; }
  public int Kills { get; set; }
  public int Deaths { get; set; }
  public int ItemsUsed { get; set; }
  public int PuzzlesCompleted { get; set; }
}

