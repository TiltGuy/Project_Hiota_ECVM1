using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PuzzleTrigger : MonoBehaviour
{
	//L'objet sur lequel vous placez ce script doit etre un trigger
	public string[] tagsAndNamesToCheck;

	public UnityEvent eventOnEnter;
	public UnityEvent eventOnExit;

	private int numberInTrigger = 0;
	private int i;

	private void OnTriggerEnter(Collider other)
	{
		if(CheckTagAndName(other.gameObject))
		{
			numberInTrigger += 1;

			if (numberInTrigger == 1)
			{
				eventOnEnter.Invoke();
			}
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (CheckTagAndName(other.gameObject))
		{
			numberInTrigger -= 1;

			if (numberInTrigger == 0)
			{
				eventOnExit.Invoke();
			}
		}
	}

	bool CheckTagAndName(GameObject toBeChecked)
	{
		if(tagsAndNamesToCheck.Length == 0)
		{
			return true;
		}

		for(i = 0; i<tagsAndNamesToCheck.Length; i++)
		{
			if(toBeChecked.name == tagsAndNamesToCheck[i] || toBeChecked.tag == tagsAndNamesToCheck[i])
			{
				return true;
			}
		}

		return false;
	}
}
