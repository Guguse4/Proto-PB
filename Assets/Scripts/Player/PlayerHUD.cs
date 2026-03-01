using Combat;
using TMPro;
using UnityEngine;

public class PlayerHUD : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI healthText;

    private void Update()
    {
        // healthText.text = $"Health : {GameManager.Instance.GetHealth()}";
    }
}
