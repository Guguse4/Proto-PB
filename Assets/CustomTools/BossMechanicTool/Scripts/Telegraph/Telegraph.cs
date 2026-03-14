using System;
using BossMechanicTool.Action;
using UnityEngine;

namespace BossMechanicTool.Telegraph
{
    public class Telegraph: MonoBehaviour
    {
        private MaterialPropertyBlock _propBlock;

        public void SetSizeInformations(SizeInformation sizeInformation)
        {
            transform.localScale = sizeInformation.Scale;
            Renderer rend = GetComponentInChildren<Renderer>();
            _propBlock = new MaterialPropertyBlock();
            rend.GetPropertyBlock(_propBlock);
            _propBlock.SetFloat("_Inner_Radius", sizeInformation.InnerRadius);
            _propBlock.SetFloat("_Degrees", sizeInformation.Angle);
            rend.SetPropertyBlock(_propBlock);
        }
    }
}