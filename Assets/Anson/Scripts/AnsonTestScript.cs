﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class AnsonTestScript : MonoBehaviour
{
    [SerializeField] PlayerMasterController playerMasterController;
    [SerializeField] BoardManager boardManager;
    [SerializeField] Dice dice;
    [SerializeField] TurnController turnController;
    [SerializeField] CardManager cardManager;
    bool diceRolled = false;

    private void Awake()
    {
        AssignAllComponents();
    }
    private void FixedUpdate()
    {
        if (dice.GetValue() > 0 && diceRolled)
        {
            diceRolled = false;
            playerMasterController = turnController.GetCurrentPlayer();
            if (!boardManager.ShowMovable(playerMasterController.GetTile(), dice.GetValue()))
            {
                if (!boardManager.ShowMovable(playerMasterController.GetCurrentRoom(), dice.GetValue()))
                {
                    Debug.LogError("Failed to show boardManager movable");
                }
            }
            //playerMasterController.DisplayBoardMovableTiles(dice.GetValue());
            dice.ResetDice();
        }
    }

    public void RollDie(InputAction.CallbackContext callbackContext)
    {
        if (callbackContext.performed)
        {
            RollDie();
        }
    }
    public void RollDie()
    {
        //Need to be replaced with results of die
        dice.RollDice();
        //playerMasterController.PlayerSelectionScript.MoveAmount = dice.GetValue();
        diceRolled = true;
        StartCoroutine(DelayResetDice(5f));
    }


    public void FindCards(InputAction.CallbackContext callbackContext)
    {
        if (callbackContext.performed)
        {
            List<Card> cardsToFind = cardManager.playableCards.GetRange(0, 3);
            var temp = turnController.GetCurrentPlayer().FindCard(cardsToFind);
            if (temp == null)
            {
                print("Player: " + turnController.GetCurrentPlayer() + " does not have: \n" + cardsToFind.ToString());
            }
            else
            {
                print("Player: " + temp.Item1 + " does have: \n" + temp.Item2.ToString());
            }
        }
    }


    public void AssignAllComponents(InputAction.CallbackContext callbackContext)
    {
        AssignAllComponents();
    }


    public void AssignAllComponents()
    {
        if (playerMasterController == null)
        {

            playerMasterController = FindObjectOfType<PlayerMasterController>();
        }
        if (boardManager == null)
        {
            boardManager = FindObjectOfType<BoardManager>();
        }
        if (dice == null)
        {
            dice = FindObjectOfType<Dice>();
        }
        if (!turnController)
        {
            turnController = FindObjectOfType<TurnController>();
        }
        if (!cardManager)
        {
            cardManager = FindObjectOfType<CardManager>();
        }
    }

    IEnumerator DelayResetDice(float t)
    {
        yield return new WaitForSeconds(t);
        dice.ResetDice();
    }

}
