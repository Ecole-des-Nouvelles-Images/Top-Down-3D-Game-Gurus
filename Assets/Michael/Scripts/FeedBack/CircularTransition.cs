using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;

namespace Michael.Scripts.FeedBack
{
    public class CircularTransition : Image
    {
        public override Material materialForRendering {
            get {
                Material material = new Material(base.materialForRendering);
                material.SetInt("_StencilComp", (int)CompareFunction.NotEqual);
                return material;
            }
        }
    }
}