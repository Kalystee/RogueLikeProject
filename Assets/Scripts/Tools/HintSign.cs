using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HintSign : MonoBehaviour, IInteractable
{
    [SerializeField] TMP_Text textObject;
    [TextArea, SerializeField] string textDisplayed;

    private void Awake()
    {
        textObject.text = textDisplayed;
        textObject.enabled = false;
    }

    public void OnActivation(EntityController entity)
    {

    }

    public void OnRange(EntityController entity)
    {

    }

    public void OnEnterRange(EntityController entity)
    {
        textObject.enabled = true;
    }

    public void OnExitRange(EntityController entity)
    {
        textObject.enabled = false;
    }
}
