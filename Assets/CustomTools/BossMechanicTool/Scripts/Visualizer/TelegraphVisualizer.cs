using Unity.VisualScripting;
using UnityEngine;

public class TelegraphVisualizer : MonoBehaviour
{
    #if UNITY_EDITOR
    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(transform.position, transform.localScale.x);
    }
    
    #endif
}
