using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;

public class LeaderBoardScript : MonoBehaviour
{
    public GameObject LeaderboadContentArea;
    public GameObject LeaderboadItemPrefab;
    public GameObject LeaderboadGO;
    public List<GameObject> ListOfLeaderboardEntries = new List<GameObject>();

    private void Start()
    {
        LeaderboadGO.SetActive(false);
    }
    public void OpenPanel()
    {
        LeaderboadGO.SetActive(true);
        GetLeaderboard();
    }

    public void GetLeaderboard()
    {
        for (int i = 0; i < ListOfLeaderboardEntries.Count; ++i)
        {
            Destroy(ListOfLeaderboardEntries[i]); //remove all entries
        }
        ListOfLeaderboardEntries = new List<GameObject>(); //redo list

        CreateNewLeaderboardEntry("Position:", "Name:", "Score:");

        var Ibreq = new GetLeaderboardRequest
        {
            StatisticName = "HighScore",
            StartPosition = 0,
            MaxResultsCount = 10
        };
        PlayFabClientAPI.GetLeaderboard(Ibreq, OnLeaderboardGet,
            error =>
            {
                Debug.Log("Cannot get leaderboard");
            }
            );
    }
    void OnLeaderboardGet(GetLeaderboardResult r)
    {
        Debug.Log(r.ToString());
        foreach (var item in r.Leaderboard)
        {
            CreateNewLeaderboardEntry((item.Position + 1).ToString(), item.DisplayName, item.StatValue.ToString());
        }
        //UpdateLeaderboard(Positioning, Name, Score);
    }

    void CreateNewLeaderboardEntry(string Positioning, string Name, string Score)
    {
        GameObject LeaderboardEntry = Instantiate(LeaderboadItemPrefab, transform.position, Quaternion.identity);

        LeaderboardEntry.transform.SetParent(LeaderboadContentArea.transform, false);

        LeaderboardEntry.GetComponent<LeaderBoardPanelScript>().PositionText.text = Positioning;
        LeaderboardEntry.GetComponent<LeaderBoardPanelScript>().NameText.text = Name;
        LeaderboardEntry.GetComponent<LeaderBoardPanelScript>().ScoreText.text = Score;

        ListOfLeaderboardEntries.Add(LeaderboardEntry);
    }
}
