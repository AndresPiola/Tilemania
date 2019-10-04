using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[System.Serializable]
public class ItemEntry
{
    public int id;
    public float playerScore;
    public string name;

}
public class RankingManager  {
    public static void SortWinners(ref List<Pawn> _players, ref List<ItemEntry> orderList)
    {
        List<int> rank;
        List<ItemEntry> tmpRankList;


        rank = new List<int>();
        tmpRankList = new List<ItemEntry>();
        orderList = new List<ItemEntry>();

        for (int i = 0; i < _players.Count; i++)
        {
            ItemEntry itm = new ItemEntry();

            itm.id = i;
            itm.playerScore = _players[i].percent;
            itm.name = _players[i].GetOnlineName();
            tmpRankList.Add(itm);


        }
        orderList = tmpRankList.OrderByDescending(x => x.playerScore).ToList();

        for (int i = 0; i < orderList.Count; i++)
        {
            _players[orderList[i].id].rankPosition = i;
        }
    }


}
