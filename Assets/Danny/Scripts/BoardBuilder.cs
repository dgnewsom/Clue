﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
/// <summary>
/// Method to create the game board from a .csv file
/// </summary>
public class BoardBuilder : MonoBehaviour
{

    //Number of bonus tiles to add
    [SerializeField] private int numberOfFreeRollTiles = 3;
    [SerializeField] private int numberOfFreeSuggestionTiles = 3;

    //.csv file containing board layout
    private TextAsset boardCSV;
    
    //Prefabs
    //Player Token
    private GameObject playerPiecePrefab;

    //Board Tiles
    private GameObject generalTilePrefab;
    private GameObject startingTilePrefab;
    private GameObject roomEntryTilePrefab;
    private GameObject shortcutTilePrefab;
    private GameObject freeRollTilePrefab;
    private GameObject freeSuggestionTilePrefab;


    //Rooms
    private GameObject BallroomPrefab;
    private GameObject BilliardRoomPrefab;
    private GameObject CentrePrefab;
    private GameObject ConservatoryPrefab;
    private GameObject DiningRoomPrefab;
    private GameObject HallPrefab;
    private GameObject KitchenPrefab;
    private GameObject LibraryPrefab;
    private GameObject LoungePrefab;
    private GameObject StudyPrefab;
    
    //Gameobjects to organise board when created
    private BoardManager boardManager;
    private string[][] boardStringArray;
    private GameObject players;
    private GameObject startTiles;
    private GameObject roomEntryTiles;
    private GameObject rooms;
    private GameObject generalTiles;
    private GameObject shortcutTiles;
    private GameObject weaponTokens;
    private GameObject freeRollTiles;
    private GameObject freeSuggestionTiles;
    int boardWidth;
    int boardHeight;

    void Awake()
    {
        Initialise();
    }

    /// <summary>
    /// Method to create the board, initialise and place all items and pass references to board manager
    /// </summary>
    private void Initialise()
    {
        boardManager = GetComponentInParent<BoardManager>();
        LoadResources();
        GenerateBoardArrayFromCSV();
        boardHeight = boardStringArray.Length;
        boardWidth = boardStringArray[0].Length;
        BuildBoard();
        CreateWeapons();
        PlaceBonusTiles();
        boardManager.CreateBoardArray(GetComponentsInChildren<BoardTileScript>(), boardHeight, boardWidth);
        //Transfer script arrays to board manager.
        boardManager.SetObjectArrays(players.GetComponentsInChildren<PlayerTokenScript>(),
                                     rooms.GetComponentsInChildren<RoomScript>(),
                                     roomEntryTiles.GetComponentsInChildren<RoomEntryBoardTileScript>(),
                                     shortcutTiles.GetComponentsInChildren<ShortcutBoardTileScript>(),
                                     startTiles.GetComponentsInChildren<StartTileScript>(),
                                     weaponTokens.GetComponentsInChildren<WeaponTokenScript>(),
                                     freeRollTiles.GetComponentsInChildren<FreeRollBoardTileScript>(),
                                     freeSuggestionTiles.GetComponentsInChildren<FreeSuggestionTileScript>());
        boardManager.PlaceWeapons();
    }

    /// <summary>
    /// Load all prefabs from resources folder
    /// </summary>
    private void LoadResources()
    {
        //Load board csv file
        boardCSV = Resources.Load("Danny/BoardLayout") as TextAsset;

        //Load prefabs from resources
        //Player token
        playerPiecePrefab = Resources.Load("Danny/Prefabs/PlayerToken/PlayerToken") as GameObject;

        //Tiles
        generalTilePrefab = Resources.Load("Danny/Prefabs/Tiles/(General)BoardTile") as GameObject;
        startingTilePrefab = Resources.Load("Danny/Prefabs/Tiles/(Start)BoardTile") as GameObject;
        roomEntryTilePrefab = Resources.Load("Danny/Prefabs/Tiles/(RoomEntry)BoardTile") as GameObject;
        shortcutTilePrefab = Resources.Load("Danny/Prefabs/Tiles/(Shortcut)BoardTile") as GameObject;
        freeRollTilePrefab = Resources.Load("Danny/Prefabs/Tiles/(FreeRoll)BoardTile") as GameObject;
        freeSuggestionTilePrefab = Resources.Load("Danny/Prefabs/Tiles/(FreeSuggestion)BoardTile") as GameObject;

        //Rooms
        BallroomPrefab = Resources.Load("Danny/Prefabs/Rooms/Ballroom") as GameObject;
        BilliardRoomPrefab = Resources.Load("Danny/Prefabs/Rooms/BilliardRoom") as GameObject;
        CentrePrefab = Resources.Load("Danny/Prefabs/Rooms/Centre") as GameObject;
        ConservatoryPrefab = Resources.Load("Danny/Prefabs/Rooms/Conservatory") as GameObject;
        DiningRoomPrefab = Resources.Load("Danny/Prefabs/Rooms/DiningRoom") as GameObject;
        HallPrefab = Resources.Load("Danny/Prefabs/Rooms/Hall") as GameObject;
        KitchenPrefab = Resources.Load("Danny/Prefabs/Rooms/Kitchen") as GameObject;
        LibraryPrefab = Resources.Load("Danny/Prefabs/Rooms/Library") as GameObject;
        LoungePrefab = Resources.Load("Danny/Prefabs/Rooms/Lounge") as GameObject;
        StudyPrefab = Resources.Load("Danny/Prefabs/Rooms/Study") as GameObject;
    }

    /// <summary>
    /// Create a 2d array from the .csv file to build the board from
    /// </summary>
    private void GenerateBoardArrayFromCSV()
    {
        //Create string array from .csv file
        string[] boardRows = boardCSV.text.TrimEnd().Split('\n');
        List<string[]> boardList = new List<string[]>();
        for (int i = 0; i < boardRows.Length; i++)
        {
            boardList.Add(boardRows[i].Split(','));
        }

        boardStringArray = boardList.ToArray();
        /*
        //Print Array for testing
        for (int i = 0; i < boardArray.Length; i++)
        {
            for (int j = 0; j < boardArray[i].Length; j++)
            {
                print("row - " + i + " col - " + j + " - " + boardArray[i][j].ToString());
            }
        }
        */
    }

    /// <summary>
    /// Build the board using various helper methods
    /// </summary>
    private void BuildBoard()
    {
        //Initialize gameobjects to organize board objects
        players = new GameObject("Player Tokens");
        players.transform.parent = this.transform;
        rooms = new GameObject("Rooms");
        rooms.transform.parent = this.transform;
        roomEntryTiles = new GameObject("Room Entry Tiles");
        roomEntryTiles.transform.parent = this.transform;
        shortcutTiles = new GameObject("Shortcut Tiles");
        shortcutTiles.transform.parent = this.transform;
        startTiles = new GameObject("Start Tiles");
        startTiles.transform.parent = this.transform;
        generalTiles = new GameObject("General Tiles");
        generalTiles.transform.parent = this.transform;
        weaponTokens = new GameObject("Weapon Tokens");
        weaponTokens.transform.parent = this.transform;
        freeRollTiles = new GameObject("Free Roll Tiles");
        freeRollTiles.transform.parent = this.transform;
        freeSuggestionTiles = new GameObject("Free Accusation Tiles");
        freeSuggestionTiles.transform.parent = this.transform;


        // Create board from string array
        int x;
        int z = -1;

        for (int row = boardStringArray.Length - 1; row >= 0; row--)
        {
            GameObject rowObject = new GameObject("Row - " + (boardStringArray.Length - row - 1));
            rowObject.transform.parent = generalTiles.transform;
            z++;
            x = -1;
            for (int col = 0; col < boardStringArray[row].Length; col++)
            {
                x++;
                string[] square = boardStringArray[row][col].Split(':');
                switch (square[0])
                {
                    case "X":
                        CreateBoardTile(x, z, rowObject);
                        break;
                    case "S":
                        CreateStartTile(x, z, square[1], rowObject);
                        break;
                    case "E":
                        CreateRoomEntranceTile(x, z, square[1], square[2], rowObject);
                        break;
                    case "SC":
                        CreateShortcutTile(x, z, square[1], square[2]);
                        break;
                    case "R":
                        CreateRoom(x, z, square[1]);
                        break;
                    default:
                        break;
                }
            }

            //Delete row if empty
            if (rowObject.transform.childCount.Equals(0))
            {
                GameObject.Destroy(rowObject);
            }
        }
    }

    /// <summary>
    /// Randomly place Free roll / accusation tiles.
    /// </summary>
    private void PlaceBonusTiles()
    {
        CreateFreeRollTiles();
        CreateFreeSuggestionTiles();
    }

    /// <summary>
    /// Instantiate a shortcut tile and set its parameters
    /// </summary>
    /// <param name="x">X position to create the tile</param>
    /// <param name="z">Z position to create the tile</param>
    /// <param name="shortcutRooms">Rooms to add to Shortcut tile</param>
    /// <param name="rotation">Rotation to set the tile</param>
    private void CreateShortcutTile(int x, int z, string shortcutRooms, string rotation)
    {
        //Create shortcut tile
        int arrowRotation = int.Parse(rotation);
        GameObject shortcutTile = GameObject.Instantiate(this.shortcutTilePrefab, new Vector3(x, 0, z), Quaternion.Euler(0,arrowRotation,0) , shortcutTiles.transform);

        //Set coordinates and tile type
        ShortcutBoardTileScript tileScript = shortcutTile.GetComponent<ShortcutBoardTileScript>();
        tileScript.GridPosition = new Vector2(x, z);
        tileScript.TileType = TileTypeEnum.Shortcut;

        //Set shortcut rooms
        string[] rooms = shortcutRooms.Split('/');
        RoomEnum shortcutFrom = RoomScript.GetRoomFromString(rooms[0]);
        RoomEnum shortcutTo = RoomScript.GetRoomFromString(rooms[1]);
        tileScript.SetShortcutRooms(shortcutFrom, shortcutTo);

        //Name tile object
        shortcutTile.name = $"Shortcut {rooms[0]} ==> {rooms[1]} ( {x} : {z} )";
    }


    /// <summary>
    /// Instantiate a roomEntrance tile and set its parameters
    /// </summary>
    /// <param name="x">X position to create the tile</param>
    /// <param name="z">Z position to create the tile</param>
    /// <param name="room">Room that tile is entrance to</param>
    /// <param name="rotation">Rotation to set tile</param>
    /// <param name="parent">GameObject to parent tile to</param>
    private void CreateRoomEntranceTile(int x, int z, string room, string rotation, GameObject parent)
    {
        //Create room entry tile 
        int arrowRotation = int.Parse(rotation);
        GameObject roomEntryTile = GameObject.Instantiate(roomEntryTilePrefab, new Vector3(x, 0, z), Quaternion.Euler(0,arrowRotation,0) , roomEntryTiles.transform);
        RoomEntryBoardTileScript tileScript = roomEntryTile.GetComponent<RoomEntryBoardTileScript>();

        //Set room, tile type and vector
        tileScript.GridPosition = new Vector2(x, z);
        tileScript.Room = RoomScript.GetRoomFromString(room);
        tileScript.TileType = TileTypeEnum.RoomEntry;

        //Name tile object
        roomEntryTile.name = $"Room Entry Tile - {room} - ( {x} : {z} )";
    }


    /// <summary>
    /// Instantiate a start tile and set its parameters
    /// </summary>
    /// <param name="x">X position to create the tile</param>
    /// <param name="z">Z position to create the tile</param>
    /// <param name="characterName">Character to assign to the Tile</param>
    /// <param name="parent">GameObject to parent tile to</param>
    private void CreateStartTile(int x, int z, string characterName, GameObject parent)
    {
        //Get camera and character enum from string
        CameraTarget playerCameraTarget;
        CharacterEnum character;
        switch (characterName)
        {
            case "Miss Scarlett":
                playerCameraTarget = CameraTarget.MissScarlett;
                character = CharacterEnum.MissScarlett;
                break;
            case "Prof Plum":
                playerCameraTarget = CameraTarget.ProfPlum;
                character = CharacterEnum.ProfPlum;
                break;
            case "Col Mustard":
                playerCameraTarget = CameraTarget.ColMustard;
                character = CharacterEnum.ColMustard;
                break;
            case "Mrs Peacock":
                playerCameraTarget = CameraTarget.MrsPeacock;
                character = CharacterEnum.MrsPeacock;
                break;
            case "Rev Green":
                playerCameraTarget = CameraTarget.RevGreen;
                character = CharacterEnum.RevGreen;
                break;
            case "Mrs White":
                playerCameraTarget = CameraTarget.MrsWhite;
                character = CharacterEnum.MrsWhite;
                break;
            default:
                character = CharacterEnum.Initial;
                playerCameraTarget = CameraTarget.Initial;
                break;
        }
        
        //Create start tile and player token
        GameObject startingTile = GameObject.Instantiate(startingTilePrefab, new Vector3(x, 0, z), transform.rotation, startTiles.transform);
        GameObject playerToken = GameObject.Instantiate(playerPiecePrefab, new Vector3(x, 0, z), transform.rotation, players.transform);
        //Name start tile and player token
        startingTile.name = $"{characterName} - Start Tile ( {x} : {z} )";
        playerToken.name = $"{characterName} - Player Token";
        
        //Set tile character, type and coordinates
        StartTileScript tilescript = startingTile.GetComponent<StartTileScript>();
        tilescript.SetCharacter(character, characterName);
        tilescript.GridPosition = new Vector2(x, z);
        tilescript.TileType = TileTypeEnum.Start;
        tilescript.SetToken(playerToken);
        
        //Set player token character, start tile and close up point
        playerToken.GetComponentInChildren<PlayerTokenScript>().SetCharacter(character,tilescript);
        playerToken.GetComponentInChildren<CloseUpPointScript>().SetCloseUpPointName(playerCameraTarget);
        playerToken.GetComponentInChildren<PlayerTokenScript>().CurrentTile = tilescript;
        
    }

    /// <summary>
    /// Instantiate a General tile and set its parameters
    /// </summary>
    /// <param name="x">X position to create the tile</param>
    /// <param name="z">Z position to create the tile</param>
    /// <param name="parent">GameObject to parent tile to</param>
    private void CreateBoardTile(int x, int z, GameObject parent)
    {
        //Create board tile, set name, type and coordinates
        GameObject boardTile = GameObject.Instantiate(generalTilePrefab, new Vector3(x, 0, z), transform.rotation, parent.transform);
        boardTile.name = $"Board Tile - ( {x} : {z} )";
        BoardTileScript tileScript = boardTile.GetComponent<BoardTileScript>();
        tileScript.GridPosition = new Vector2(x, z);
        tileScript.TileType = TileTypeEnum.General;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="x">X position to create the tile</param>
    /// <param name="z">Z position to create the tile</param>
    /// <param name="roomName">Room to instantiate</param>
    private void CreateRoom(int x, int z, string roomName)
    {
        //Create room from roomName string and set enum
        GameObject room;
        switch (roomName)
        {
            case "Study":
                room = GameObject.Instantiate(StudyPrefab, new Vector3(x, 0, z), transform.rotation, rooms.transform);
                room.GetComponent<RoomScript>().Room = RoomEnum.Study;
                break;
            case "Hall":
                room = GameObject.Instantiate(HallPrefab, new Vector3(x, 0, z), transform.rotation, rooms.transform);
                room.GetComponent<RoomScript>().Room = RoomEnum.Hall;
                break;
            case "Lounge":
                room = GameObject.Instantiate(LoungePrefab, new Vector3(x, 0, z), transform.rotation, rooms.transform);
                room.GetComponent<RoomScript>().Room = RoomEnum.Lounge;
                break;
            case "Library":
                room = GameObject.Instantiate(LibraryPrefab, new Vector3(x, 0, z), transform.rotation, rooms.transform);
                room.GetComponent<RoomScript>().Room = RoomEnum.Library;
                break;
            case "Centre":
                room = GameObject.Instantiate(CentrePrefab, new Vector3(x, 0, z), transform.rotation, rooms.transform);
                room.GetComponent<RoomScript>().Room = RoomEnum.Centre;
                break;
            case "Dining Room":
                room = GameObject.Instantiate(DiningRoomPrefab, new Vector3(x, 0, z), transform.rotation, rooms.transform);
                room.GetComponent<RoomScript>().Room = RoomEnum.DiningRoom;
                break;
            case "Billiard Room":
                room = GameObject.Instantiate(BilliardRoomPrefab, new Vector3(x, 0, z), transform.rotation, rooms.transform);
                room.GetComponent<RoomScript>().Room = RoomEnum.BilliardRoom;
                break;
            case "Conservatory":
                room = GameObject.Instantiate(ConservatoryPrefab, new Vector3(x, 0, z), transform.rotation, rooms.transform);
                room.GetComponent<RoomScript>().Room = RoomEnum.Conservatory;
                break;
            case "Ballroom":
                room = GameObject.Instantiate(BallroomPrefab, new Vector3(x, 0, z), transform.rotation, rooms.transform);
                room.GetComponent<RoomScript>().Room = RoomEnum.Ballroom;
                break;
            case "Kitchen":
                room = GameObject.Instantiate(KitchenPrefab, new Vector3(x, 0, z), transform.rotation, rooms.transform);
                room.GetComponent<RoomScript>().Room = RoomEnum.Kitchen;
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// Instantiate all weapon tokens
    /// </summary>
    private void CreateWeapons()
    {
        foreach (WeaponEnum weapon in Enum.GetValues(typeof(WeaponEnum))){
            
            GameObject prefab = Resources.Load("Danny/Prefabs/WeaponTokens/" + weapon.ToString()) as GameObject;
            GameObject weaponToken = GameObject.Instantiate(prefab);
            weaponToken.transform.parent = weaponTokens.transform;
            weaponToken.name = String.Format("Weapon Token ({0})", weapon.ToString());

        }
    }

    /// <summary>
    /// Randomly replace required number of general tiles with FreeAccusation Tiles
    /// </summary>
    private void CreateFreeSuggestionTiles()
    {
        int tilesPlaced = 0;
        while (tilesPlaced < numberOfFreeSuggestionTiles)
        {
            boardManager.CreateBoardArray(GetComponentsInChildren<BoardTileScript>(), boardHeight, boardWidth);
            BoardTileScript[] boardTiles = FindObjectsOfType<BoardTileScript>();
            BoardTileScript tile = boardTiles[Random.Range(0, boardTiles.Length)];
            if (tile.TileType.Equals(TileTypeEnum.General))
            {
                if (CheckNearbyTiles(tile))
                {
                    float x = tile.GridPosition.x;
                    float z = tile.GridPosition.y;
                    GameObject newTile = GameObject.Instantiate(freeSuggestionTilePrefab, new Vector3(x, 0, z), transform.rotation, freeSuggestionTiles.transform);
                    newTile.name = $"Free Suggestion Board Tile - ( {x} : {z} )";
                    BoardTileScript tileScript = newTile.GetComponent<BoardTileScript>();
                    tileScript.GridPosition = new Vector2(x, z);
                    tileScript.TileType = TileTypeEnum.FreeSuggestion;
                    tilesPlaced++;
                    GameObject.Destroy(tile.gameObject);

                }
                
            }
        }
    }

    /// <summary>
    /// Randomly replace required number of general tiles with FreeRoll Tiles
    /// </summary>
    private void CreateFreeRollTiles()
    {
        int tilesPlaced = 0;
        while(tilesPlaced < numberOfFreeRollTiles)
        {
            boardManager.CreateBoardArray(GetComponentsInChildren<BoardTileScript>(), boardHeight, boardWidth);
            BoardTileScript[] boardTiles = FindObjectsOfType<BoardTileScript>();
            BoardTileScript tile = boardTiles[Random.Range(0, boardTiles.Length)];
            if (tile.TileType.Equals(TileTypeEnum.General))
            {
                if (CheckNearbyTiles(tile))
                {
                    float x = tile.GridPosition.x;
                    float z = tile.GridPosition.y;
                    GameObject newTile = GameObject.Instantiate(freeRollTilePrefab, new Vector3(x, 0, z), transform.rotation, freeRollTiles.transform);
                    newTile.name = $"Free Roll Board Tile - ( {x} : {z} )";
                    BoardTileScript tileScript = newTile.GetComponent<BoardTileScript>();
                    tileScript.GridPosition = new Vector2(x, z);
                    tileScript.TileType = TileTypeEnum.FreeRoll;
                    tilesPlaced++;
                    GameObject.Destroy(tile.gameObject);

                }
                
            }
        }
    }

    /// <summary>
    /// Check for nearby bonus / start tiles in order to space out around the board
    /// </summary>
    /// <param name="tile">Tile to check for nearby free roll or free suggestion tiles</param>
    /// <returns></returns>
    private bool CheckNearbyTiles(BoardTileScript tile)
    {
        List<BoardTileScript> nearbyTiles = boardManager.bfs(tile,6);
        //Debug.Log(nearbyTiles.Count);
        foreach (BoardTileScript nearbyTile in nearbyTiles)
        {
            if (nearbyTile.TileType.Equals(TileTypeEnum.FreeSuggestion) || nearbyTile.TileType.Equals(TileTypeEnum.FreeRoll) || nearbyTile.TileType.Equals(TileTypeEnum.Start))
            {
                return false;
            }
        }
        return true;
    }
}
