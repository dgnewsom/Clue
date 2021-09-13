﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Master controller for the player (character)
/// it acts as a wrapper for the player to communicate with other controllers or managers and vis versa 
/// to avoid super spaghetii code
/// </summary>
public class PlayerMasterController : MonoBehaviour
{
    public bool isAI = false;

    [Space]
    [Header("Player Components")]
    [SerializeField] PlayerTokenScript playerTokenScript;
    [SerializeField] PlayerStatsScript playerStatsScript;

    [Space]
    [Header("Other Components")]
    [SerializeField] BoardManager boardManager;

    public PlayerTokenScript PlayerTokenScript { get => playerTokenScript; set => playerTokenScript = value; }

    private void Awake()
    {

        //playerTokenScript = GetComponent<PlayerTokenScript>();

        playerStatsScript = GetComponent<PlayerStatsScript>();

        boardManager = FindObjectOfType<BoardManager>();

        //test

        //PlayerMasterController playerMasterController = FindObjectOfType<PlayerMasterController>();
        //print((int )playerMasterController.GetCharacter());

    }
    /// <summary>
    /// for initialising the player
    /// </summary>
    public void StartBehaviour()
    {
        playerStatsScript.InititaliseToGuessList();

    }
    /// <summary>
    /// Setting the notebook value
    /// </summary>
    /// <param name="entryKey"></param>
    /// <param name="value"></param>
    internal void SetNotebookValue(Enum entryKey, bool value)
    {
        playerStatsScript.SetNotebookValue(entryKey, value);
    }

    /// <summary>
    /// Getting the notebook value
    /// </summary>
    /// <param name="entryKey"></param>
    /// <returns></returns>
    internal bool GetNotebookValue(Enum entryKey)
    {
        return playerStatsScript.GetNotebookValue(entryKey);
    }

    /// <summary>
    /// check if the player can take a short cut 
    /// </summary>
    /// <returns></returns>
    public bool CanTakeShortcut()
    {
        return playerTokenScript.CanTakeShortcut();
    }

    /// <summary>
    /// Set the player's character
    /// </summary>
    /// <param name="ce">Character Enum</param>
    public void SetCharacter(CharacterEnum ce)
    {
        playerStatsScript.SetCharacter(ce);
    }


    public CharacterEnum GetCharacter()
    {
        return playerStatsScript.Character;
    }

    public override string ToString()
    {
        return playerStatsScript.Character.ToString()+" ("+name+")";
    }
    /// <summary>
    /// Get current grip position
    /// </summary>
    /// <returns></returns>
    public Vector2 GetGridPosition()
    {
        return playerTokenScript.GetGridPosition();
    }
    /// <summary>
    /// Get the current tile the token is on
    /// </summary>
    /// <returns></returns>
    public BoardTileScript GetTile()
    {
        return playerTokenScript.CurrentTile;
    }

    internal void TakeShortcut()
    {
        playerTokenScript.TakeShortcut();
    }

    internal bool IsInRoom()
    {
        return playerTokenScript.IsInRoom();
    }


    /// <summary>
    /// Add a list of card to the player's deck
    /// </summary>
    /// <param name="cs">list of cards </param>
    /// <returns>if a card from cs is in the deck already</returns>
    public bool AddCard(List<Card> cs)
    {
        return playerStatsScript.AddCard(cs);
    }

    public List<Card> GetDeck()
    {
        return playerStatsScript.Deck;
    }
    /// <summary>
    /// Initialise the detective notebook
    /// </summary>
    internal void initializeNotebook()
    {
        playerStatsScript.InitializeNotebook();
    }


    /// <summary>
    /// check if the player has a card in his deck from the given list of cards
    /// it will return the cards and the PlayerMasterController that has the card
    /// it will return null if none found
    /// 
    /// </summary>
    /// <param name="cards"> cards to be found</param>
    /// <returns> The current player controller and a list of cards that matched the cards to be found</returns>
    public Tuple<PlayerMasterController,List<Card>> FindCard(List<Card> cards)
    {
        List<Card> foundCards = playerStatsScript.FindCard(cards);
        if (foundCards.Count != 0)
        {
            return new Tuple<PlayerMasterController, List<Card>>( this,foundCards);
        }
        else
        {
            return null;
        }
    }


    /// <summary>
    /// calls the boardManager to display all the tiles that it can move to according to the range of it's movement
    /// </summary>
    /// <param name="range">how far can the player move (normally the dice roll)</param>
    public void DisplayBoardMovableTiles(int range)
    {
        if (boardManager == null)
        {
            boardManager = FindObjectOfType<BoardManager>();
        }
        boardManager.ShowMovable(GetTile(),range);

    }
    /// <summary>
    /// check if the player can move to a certain tile
    /// </summary>
    /// <param name="t">tile to be move towards</param>
    /// <returns>if the player can move</returns>
    public bool CanMove(BoardTileScript t)
    {
        return boardManager.CanMove(t);
    }

    /// <summary>
    /// moving the player to a certain tile
    /// </summary>
    /// <param name="b">if the player can move to that tile</param>
    public void MovePlayer(BoardTileScript b)
    {
        print("Moving Player");
        playerTokenScript.MoveToken(b);
    }

    /// <summary>
    /// for entering in to a room
    /// </summary>
    /// <param name="entryPoint">tile for the room entry</param>
    internal void EnterRoom(RoomEntryPoint entryPoint)
    {
        playerTokenScript.EnterRoom(entryPoint);
    }
    /// <summary>
    /// setting the tile that the player is currently on
    /// </summary>
    /// <param name="tileToSet">tile that the player is currently on</param>
    internal void SetCurrentTile(BoardTileScript tileToSet)
    {
        playerTokenScript.CurrentTile = tileToSet;
    }


    
    public RoomScript GetCurrentRoom()
    {
        return playerTokenScript.CurrentRoom;
    }

    public void SetCurrentRoom(RoomScript room)
    {
        playerTokenScript.CurrentRoom = room;
    }

    /// <summary>
    /// moving the player to a certain position instead of tile
    /// </summary>
    /// <param name="v">vector3 position</param>
    public void MovePlayer(Vector3 v)
    {
        print("Moving Player");
        playerTokenScript.MoveToken(v);
    }

    /// <summary>
    /// clear the tile the token was on
    /// </summary>
    public void ClearTokenTile()
    {
        playerTokenScript.ClearTokenTile();
    }

    internal void ExitRoom(RoomEntryBoardTileScript roomEntryBoardTileScript, BoardTileScript targetTile)
    {
        playerTokenScript.ExitRoom(roomEntryBoardTileScript, targetTile);
    }

    internal void SetPosition(Vector3 newPosition)
    {
        playerTokenScript.transform.position = newPosition; 
    }
    /// <summary>
    /// check if the player is eleminated
    /// </summary>
    /// <returns>is the player eliminated</returns>
    public bool IsEliminated()
    {
        return playerStatsScript.IsEliminated;
    }
    /// <summary>
    /// eliminate player
    /// </summary>
    public void EliminatePlayer()
    {
        playerStatsScript.IsEliminated = true;
    }

    /// <summary>
    /// check if the player token is still moving.
    /// returns true if the token is moving to a new tile or if the token is moving in to a room
    /// 
    /// </summary>
    /// <returns>true if it the token is moving</returns>
    public bool IsMoving()
    {
        return playerTokenScript.IsMove || playerTokenScript.IsMovingRoom();
    }

    /// <summary>
    /// Remove a card from ToGuessCard
    /// </summary>
    /// <param name="c">card to be removed</param>
    /// <returns>if the list contains the card to be removed</returns>
    public bool RemoveToGuessCard(Card c)
    {
        return playerStatsScript.RemoveToGessCard(c);
    }

    public List<Card> GetToGuessCards()
    {
        return playerStatsScript.ToGuessList;

    }

}
