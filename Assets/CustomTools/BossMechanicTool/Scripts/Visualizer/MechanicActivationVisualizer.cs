using UnityEngine;

public class MeschanicActivationVisualizer : MonoBehaviour
{
    #if UNITY_EDITOR
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position, transform.localScale.x);
    }
    
    #endif
}
