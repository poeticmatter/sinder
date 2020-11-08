using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class TransactionRecords
{
    public List<Transaction> transactions;

    public TransactionRecords()
	{
        transactions = new List<Transaction>();
	}

    public void RecordTransaction(string win, string loss)
	{
        transactions.Add(new Transaction(win, loss));
	}

    public void Reset()
	{
        transactions.Clear();
	}

}

[Serializable]
public class Transaction
{
    public string card_win_id;
    public string card_lose_id;

    public Transaction (string win, string loss)
	{
        this.card_win_id = win;
        this.card_lose_id = loss;
	}

}
