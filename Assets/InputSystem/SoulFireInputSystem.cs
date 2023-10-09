using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;


public class SoulFireInputSystem : MonoBehaviour
{

    public static SoulFireInputSystem current;
    public static PlayerControls playerControls;
    public List<PlayerControl> players = new List<PlayerControl>(); //List all of the players joining in - custom class
    
    private void Awake()
    {
        if(current == null)
        {
            current = this;
            DontDestroyOnLoad(gameObject);
            playerControls = new PlayerControls();
            playerControls.Controls.Join.performed += Join;
            playerControls.Enable();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Join(InputAction.CallbackContext ctx)
    {
        if (players.Count >= 2)
            return;

        //This for loop is checking the player aready exists
        foreach (PlayerControl player in players)
        {
            if (player.inputDevice == ctx.control.device)
            {
                Debug.Log("Player Already Joined");
                return;
            }
        }

        //Player doesn't exist so we need to create them
        PlayerControl newPlayer = new PlayerControl();
        newPlayer.SetDevice(ctx.control.device, players.Count); //Set up all the inputs in this class, look down below

        players.Add(newPlayer); //Add the new player to our list
    }
}

//This is are custom class for each player
[System.Serializable]
public class PlayerControl
{
    public InputDevice inputDevice; //This is the device they are using
    public int playerID; //The player index
    PlayerControls playerControls; //Controls we created, but are assigning it to only them
    InputUser inputUser; //Assigning them a input user

    public Vector2 movementDirection;

    public delegate void OnNorthPressed();
    public OnNorthPressed onNorthPressed;

    public delegate void OnSouthPressed();
    public OnSouthPressed onSouthPressed;

    public delegate void OnEastPressed();
    public OnEastPressed onEastPressed;

    public delegate void OnWestPressed();
    public OnWestPressed onWestPressed;

    public void SetDevice(InputDevice device, int id)
    {
        inputDevice = device;
        playerID = id;

        playerControls = new PlayerControls();
        inputUser = InputUser.PerformPairingWithDevice(inputDevice);
        inputUser.AssociateActionsWithUser(playerControls);
        playerControls.Enable();
        AssignEvents();
    }

    //Assign all the functions in the game manager
    void AssignEvents()
    {
        playerControls.Controls.Movement.performed += SetMovement;
        playerControls.Controls.Movement.canceled += CancelMovement;

        playerControls.Controls.North.performed += NorthPressed;
        playerControls.Controls.East.performed += EastPressed;
        playerControls.Controls.South.performed += SouthPressed;
        playerControls.Controls.West.performed += WestPressed;
    }

    private void NorthPressed(InputAction.CallbackContext obj)
    {
        if(onNorthPressed != null)
        {
            onNorthPressed.Invoke();
        }
    }
    private void EastPressed(InputAction.CallbackContext obj)
    {
        if (onEastPressed != null)
        {
            onEastPressed.Invoke();
        }
    }
    private void WestPressed(InputAction.CallbackContext obj)
    {
        if (onWestPressed != null)
        {
            onWestPressed.Invoke();
        }
    }
    private void SouthPressed(InputAction.CallbackContext obj)
    {
        if (onSouthPressed != null)
        {
            onSouthPressed.Invoke();
        }
    }

    private void CancelMovement(InputAction.CallbackContext obj)
    {
        movementDirection = Vector2.zero;
    }

    private void SetMovement(InputAction.CallbackContext ctx)
    {
        movementDirection = ctx.ReadValue<Vector2>();
    }

    
}
