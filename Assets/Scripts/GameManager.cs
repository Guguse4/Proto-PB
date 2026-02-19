using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; set; }
    private Player _player;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        
        _player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }
    
    public int GetHealth() => _player.CurrentHealth;
}