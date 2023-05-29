using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

using System.Linq;
using TMPro;


public class EnemyImprovementSingle : MonoBehaviour, IDamageable
{
    private Animator _animator;
    public float _NumberOfUses;
    //public Animation animation;
    public List<string> TargetTags = new List<string>();
    //public List<SkillCard_SO> SkillCards = new List<SkillCard_SO>();
    public string EnemyTag;
    public List<Transform> Enemies = new List<Transform>();
    public UnityEvent OnSelectSkillCard;
    //public bool saveToPlayerPrefs = false;
    //put the ref in the editor
    public Canvas cardCanvas;
    public SkillCard_SO[] AllSkillCards;
    public SkillCard_SO[] SkillCards;
    public TMP_Text[] Title_Text;
    public TMP_Text[] Bonus_Texts;
    public TMP_Text[] Malus_Texts;
    public string bonusPrefix = "Bonus";
    public string malusPrefix = "Malus";
    public string linePrefix = "> ";


    [Header("-- SOUND DEPENDENCIES --")]

    public FMODUnity.EventReference Spawn_ER;

    private void Awake()
    {
        UpdateListOfEnemies();
        _animator = GetComponent<Animator>();
        
        var cardList = AllSkillCards.ToList();
        if(DeckManager.instance)
        {
            for(int i = 0; i < SkillCards.Length; i++)
            {
                SkillCards[i] = DeckManager.instance.DrawOneCard();
            }
        }
        else
        {
            for ( int i = 0; i < SkillCards.Length; i++ )
            {
                var randomCard = cardList[Random.Range(0, cardList.Count)];
                cardList.Remove(randomCard);
                SkillCards[i] = randomCard;
            }
        }
    }



    private void UpdateListOfEnemies()
    {
        foreach ( GameObject enemy in GameObject.FindGameObjectsWithTag(EnemyTag) )
        {
            Enemies.Add(enemy.GetComponent<Transform>());
        }
    }

    

    private void OnEnable()
    {

        var buttons = cardCanvas.GetComponentsInChildren<UnityEngine.UI.Button>().ToList();
        buttons.ForEach(btn =>
        {
            var btnIndex = buttons.IndexOf(btn);
            btn.onClick.AddListener(() => SelectCard(btnIndex));
        });
    }

    private void DisplayCanvas()
    {
        //AssignNewSkillCard();

        //if ( saveToPlayerPrefs && PlayerPrefs.GetInt(SceneManager.GetActiveScene().name + "." + name + ".t_Activated") == 1 )
        //{
        //    TakeDamagesParriable(0, null, false);
        //}
        print("je me display !");
        GameObject pManager = GameObject.Find("PauseManager");
        if(pManager )
        {
            pManager.GetComponent<PauseManager>().b_IsChoosingCard = true;
        }
        cardCanvas.gameObject.SetActive(true);
        cardCanvas.GetComponentsInChildren<UnityEngine.UI.Selectable>().First().Select();
        Time.timeScale = 0;

        int cardIndex = 0;
        foreach ( SkillCard_SO skillCard in SkillCards )
        {
            UpdateCardMessageTitle(Title_Text[cardIndex], skillCard);
            UpdateCardMessages(Bonus_Texts[cardIndex], skillCard, true);
            UpdateCardMessages(Malus_Texts[cardIndex], skillCard, false);
            cardIndex++;
        }
    }

    public void TakeDamagesParriable( float damageTaken, Transform Striker, bool isAHook )
    {
        if ( _NumberOfUses <= 0 )
            return;

        _animator.SetTrigger("t_Activated");
        _NumberOfUses--;
        if ( Striker )
        {
            CharacterSpecs specs = Striker.GetComponent<CharacterSpecs>();
            specs.RegenerateLife(specs.MaxHealth);
        }
        print("je prend des dommages !");
        DisplayCanvas();
    }

    public void SelectCard(int index)
    {
        
        var skillCard = SkillCards[index];
        //print("Assign");
        DeckManager currentDeckInstance = DeckManager.instance;
        currentDeckInstance.AddCardToEnnemyDeck(skillCard);
        currentDeckInstance.RemoveCardFromRunDeck(SkillCards[0]);
        currentDeckInstance.RemoveCardFromRunDeck(SkillCards[1]);
        if ( currentDeckInstance )
        {
            currentDeckInstance.GainMoneyBet();
            currentDeckInstance.PlayerBetMoney = currentDeckInstance.CalculateCurrentCardsBet();
            print("Money Bet = " + currentDeckInstance.PlayerBetMoney);
        }
        //GameObject.FindGameObjectsWithTag("Gate").ToList().ForEach(o => o.gameObject.SetActive(false));
        ToggleOnRoomComplete.ToggleAll();
        OnSelectSkillCard?.Invoke();
        GameObject pManager = GameObject.Find("PauseManager");
        if ( pManager )
        {
            pManager.GetComponent<PauseManager>().b_IsChoosingCard = false;
        }
        Time.timeScale = 1;
        gameObject.SetActive(false);
        cardCanvas.gameObject.SetActive(false);
    }

    public void TakeDamagesNonParriable( float damageTaken, Transform Striker, float ForceOfProjection )
    {
    }


    private void UpdateCardMessages(TMP_Text textToUpdate, SkillCard_SO skillCard, bool b_BonusDisplay)
    {
        string newLine = System.Environment.NewLine;
        if (b_BonusDisplay)
        {
            if( skillCard.Bonus.Count > 0)
            {
                if ( !string.IsNullOrEmpty(bonusPrefix) ) textToUpdate.text = bonusPrefix + "\n";
                for ( int j = 0; j < skillCard.Bonus.Count; j++ )
                {
                    // first Line of Text
                    if ( j == 0 )
                    {
                        textToUpdate.text += linePrefix + skillCard.Bonus[j].cardMessage;
                    }
                    else
                    {
                        textToUpdate.text += newLine + linePrefix + skillCard.Bonus[j].cardMessage;
                    }
                }
            }
        }
        else
        {
            if( skillCard.Malus.Count > 0 )
            {
                if(!string.IsNullOrEmpty(malusPrefix)) textToUpdate.text = malusPrefix + "\n";
                for ( int j = 0; j < skillCard.Malus.Count; j++ )
                {
                    // first Line of Text
                    if ( j == 0 )
                    {
                        textToUpdate.text += linePrefix + skillCard.Malus[j].cardMessage;
                    }
                    else
                    {
                        textToUpdate.text += newLine + linePrefix + skillCard.Malus[j].cardMessage;
                    }
                }
            }
        }
        
    }


    private void UpdateCardMessageTitle( TMP_Text textToUpdate, SkillCard_SO skillCard)
    {
        textToUpdate.text = skillCard.cardName;

    }


    #region -- SOUNDS STUFFS --

    public void SpawnStele_SoundEvent()
    {
        FMODUnity.RuntimeManager.PlayOneShotAttached(Spawn_ER, gameObject);
    }

    #endregion


}
