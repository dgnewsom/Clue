﻿using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardSlot : MonoBehaviour
{
    public Card card;
    public Image img;
    public TextMeshProUGUI txt;
    public bool isVisible = false;

    private void Start()
    {
        Initialise();
        SetCardDetails();
    }

    private void Initialise()
    {
        img.sprite = null;
        txt.text = "";
    }

    public void SetCard(Card c)
    {
        card = c;
        SetCardDetails();
    }

    private void SetCardDetails()
    {
        if (!card)
        {
            return;
        }
        img.sprite = card.GetCardImage();
        txt.text = EnumToString.GetStringFromEnum(card.GetCardType());
    }
    /// <summary>
    /// Makes the card slot visible and its game object active
    /// </summary>
    /// <param name="isVisible">Makes it visible if true , hides it otherwise</param>
    public void SetVisible(bool isVisible = true)
    {
        this.isVisible = isVisible;
        gameObject.SetActive(isVisible);
    }


}
