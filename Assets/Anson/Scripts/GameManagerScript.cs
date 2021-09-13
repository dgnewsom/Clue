﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// This controls the starting and exiting the game loop
/// </summary>
public class GameManagerScript : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] PlayerMasterController playerMasterController;
    [SerializeField] BoardManager boardManager;
    [SerializeField] Dice dice;
    [SerializeField] TurnController turnController;
    [SerializeField] RoundManager roundManager;
    [SerializeField] CardManager cardManager;
    [SerializeField] UserController userController;
    [SerializeField] UIHandler uIHandler;
    private void Start()
    {
        AssignAllComponents();
        Application.targetFrameRate = 300;

    }
    /// <summary>
    /// starting and initialing the game and it's components
    /// </summary>
    public void StartGame()
    {
        if (FindObjectOfType<GameSetUpScript>() != null)
        {
            FindObjectOfType<GameSetUpScript>().StartGame();
        }
        turnController.StartGame();
        cardManager.Initialise();
        foreach (PlayerMasterController p in FindObjectsOfType<PlayerMasterController>())
        {
            p.StartBehaviour();
        }
        uIHandler.StartBehaviour();
        roundManager.StartTurn();
    }
    /// <summary>
    /// returning to main menu
    /// </summary>
    public void LeaveToMenu()
    {
        SceneManager.LoadScene(0);
    }

    /// <summary>
    /// assigning all the components if not linked
    /// </summary>
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
        if (!roundManager)
        {
            roundManager = FindObjectOfType<RoundManager>();
        }
        if (!userController)
        {
            userController = FindObjectOfType<UserController>();
        }
        if (!uIHandler)
        {
            uIHandler = FindObjectOfType<UIHandler>();
        }
    }



}
