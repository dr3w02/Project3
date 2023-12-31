using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using static UnityEditor.Experimental.GraphView.GraphView;

public class PlayerConfigurationManager : MonoBehaviour
{
    private List<PlayerConfiguration> playerConfigs;
    public static PlayerControls playerControls;
    public static SoulFireInputSystem soulFireInputSystem;

    [SerializeField]
    private int MaxPlayers = 2;

  

    public static PlayerConfigurationManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.Log("Trying to create another insance");

        }

        else
        {
            Instance = this;
            DontDestroyOnLoad(Instance);
            playerConfigs = new List<PlayerConfiguration>();

        }

    }

    public void SetPlayerColor(int index, Material color)
    {
        playerConfigs[index].PlayerMaterial = color;
    }

    public void ReadyPlayer(int index)
    {
        playerConfigs[index].IsReady = true;
        if (playerConfigs.Count == MaxPlayers && playerConfigs.TrueForAll(p => p.IsReady == true))
        {
            SceneManager.LoadScene("Game");
        }
           
    }

   

    public void Join(PlayerInput pi)
    {
        Debug.Log("PlayerJoined"+pi.playerIndex);

        

        if (!playerConfigs.Any(p => p.PlayerIndex == pi.playerIndex)) 
        {
            pi.transform.SetParent(transform);
            playerConfigs.Add(new PlayerConfiguration(pi));
        
        
        }
    }
}


public class PlayerConfiguration
{
    public PlayerConfiguration(PlayerInput pi)
    {
        PlayerIndex = pi.playerIndex;
        Input = pi;
    }


    public PlayerInput Input { get; set; }

    public int PlayerIndex { get; set; }

    public bool IsReady { get; set; }

    public Material PlayerMaterial { get; set;  }

}



