using System;
using UnityEngine;

public class Menu : MonoBehaviour
{
    [SerializeField] private GameObject backgorund;
    [SerializeField] private GameObject button;
    [SerializeField] private GameObject title;
    [SerializeField] private GameObject rules;

    private Color backgorundColor;

    public Action play;
    public void OnPlay()
    {
        button.SetActive(false);
        backgorund.SetActive(false);
        title.SetActive(false);
        play?.Invoke();
    }
}
