using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldBender : MonoBehaviour
{
    public Material worldBendingMaterial;

    void Update()
    {
        worldBendingMaterial.SetFloat("_Vertical_Amount", NumberAnimator.Number2);
        worldBendingMaterial.SetFloat("_Horizontal_Amount", NumberAnimator.Number1);
    }
}
