using GABackend;
using System;
using System.Collections.Generic;
using UnityEngine;
using static GABackend.MKRLeaderboard;

[CreateAssetMenu(fileName = "Database CRUD", menuName = "Scriptable Objects / database CRUD")]
public class SODatabaseCRUD : ScriptableObject
{
    List<LeaderBoardItem> players = new List<LeaderBoardItem>();
    string errorMessage = string.Empty;

    public Tuple<List<LeaderBoardItem>, string> GetTodayDailyLeaderboard()
    {
        MKRLeaderboard.instance.GetTodayDailyLeaderboard(OnLeaderboardSuccess, OnLeaderboardError);
        return Tuple.Create(players, errorMessage);
    }

    public Tuple<List<LeaderBoardItem>, string> GetTodayWeeklyLeaderboard()
    {
        MKRLeaderboard.instance.GetTodayWeeklyLeaderboard(OnLeaderboardSuccess, OnLeaderboardError);
        return Tuple.Create(players, errorMessage);
    }

    public Tuple<List<LeaderBoardItem>, string> GetTodayMonthlyLeaderboard()
    {
        MKRLeaderboard.instance.GetTodayMonthlyLeaderboard(OnLeaderboardSuccess, OnLeaderboardError);
        return Tuple.Create(players, errorMessage);
    }

    void OnLeaderboardSuccess(LeaderBoardData data)
    {
        players.Clear();
        foreach (var item in data.leaderboard)
            players.Add(item);
    }

    void OnLeaderboardError(string errorMessage)
    {
        errorMessage = string.Empty;
        players.Clear();
        Debug.LogError($"Error retrieving leaderboard data: {errorMessage}");
        errorMessage = errorMessage.Trim();
    }
}
