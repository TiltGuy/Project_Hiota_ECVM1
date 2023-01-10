using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnCardManager : MonoBehaviour
{
    //public int numberofCardstoDraw = 0;
    public List<EnemyImprovement> SteleSkilCards = new List<EnemyImprovement>();

    private void Start()
    {
        //DrawCardsForSpawner();
        //for(int i = 0; i < numberofCardstoDraw; i++)
        //{
        //    SkillCard_SO newcard = DeckManager.instance.DrawOneCard();
        //    Debug.Log(newcard);
        //}
    }

    public void DrawCardsForSpawner()
    {
        if ( SteleSkilCards.Count == 0 )
        {
            Debug.LogError("Il n'y a pas de stèle raccordées!!!!!", this);
            return;
        }
        List<SkillCard_SO> skillCard_SOsToAssign = new List<SkillCard_SO>();
        skillCard_SOsToAssign = DeckManager.instance.DrawCards(SteleSkilCards.Count);
        for ( int i = 0; i < SteleSkilCards.Count; i++ )
        {
            if ( SteleSkilCards[i] != null )
            {
                SteleSkilCards[i].SkillCard = skillCard_SOsToAssign[i];
                //Debug.Log(skillCard_SOsToAssign[i], SteleSkilCards[i]);
            }
        }
        //Debug.Log(skillCard_SOs);
        //DrawOneCardForEachSpawnCard();
    }

    private void DrawOneCardForEachSpawnCard()
    {
        for ( int i = 0; i < SteleSkilCards.Count; i++ )
        {
            if ( SteleSkilCards[i] != null )
            {
                SkillCard_SO newcard = DeckManager.instance.DrawOneCard();
                //Debug.Log(newcard, SpawnSkilCards[i]);
                SteleSkilCards[i].SkillCard = newcard;
            }
        }
    }
}
