using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnCardManager : MonoBehaviour
{
    public int numberofCardstoDraw = 0;
    public List<EnemyImprovement> SpawnSkilCards = new List<EnemyImprovement>();

    private void Start()
    {
        DrawCardsForSpawner();
    }

    private void DrawCardsForSpawner()
    {
        if(SpawnSkilCards.Count <= 0)
            return;
        //List<SkillCard_SO> skillCard_SOs = new List<SkillCard_SO>();
        //skillCard_SOs = DeckManager.instance.DrawCards(SpawnSkilCards.Count - 1);
        for ( int i = 0; i < SpawnSkilCards.Count; i++ )
        {
            if(SpawnSkilCards[i] != null)
            {
                SpawnSkilCards[i].SkillCards[0] = DeckManager.instance.DrawFuckingCard();
            }
        }
    }
}
