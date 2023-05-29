using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "List of SkillCards")]
public class ListOfCards : ScriptableObject
{
    public List<SkillCard_SO> ListCards = new List<SkillCard_SO>();
}
