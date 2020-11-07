using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class MatchRecords
{
    public List<Match> records;

    public MatchRecords()
	{
        records = new List<Match>();
	}

    public void RecordMatch(string win, string loss)
	{
        records.Add(new Match(win, loss));
	}

    public void Reset()
	{
        records.Clear();
	}

}

[Serializable]
public class Match
{
    public string win;
    public string loss;

    public Match (string win, string loss)
	{
        this.win = win;
        this.loss = loss;
	}

}
