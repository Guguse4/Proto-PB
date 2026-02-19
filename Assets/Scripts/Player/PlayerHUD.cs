using System;
using TMPro;
using UnityEditor.Build.Content;
using UnityEngine;

public class PlayerHUD : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI healthText;

    private void Update()
    {
        healthText.text = $"Health : {GameManager.Instance.GetHealth()}";
    }
}
