using BossMechanicTool.Action;
using UnityEngine;

namespace BossMechanicTool.Telegraph
{
    public class Telegraph: MonoBehaviour
    {
        public void SetSizeInformations(SizeInformation sizeInformation)
        {
            transform.localScale = sizeInformation.Scale;
            Renderer rend = GetComponentInChildren<Renderer>();
            rend.sharedMaterial.SetFloat("_Inner_Radius", sizeInformation.InnerRadius);
            rend.sharedMaterial.SetFloat("_Degrees", sizeInformation.Angle);
        }
    }
}