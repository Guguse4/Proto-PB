using UnityEngine;

public class MechanicActivationVisualizer : MonoBehaviour
{
    #if UNITY_EDITOR
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position, transform.localScale.x);
    }
    
    #endif
}
