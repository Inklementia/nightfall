using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeactivateAfterAnim : MonoBehaviour
{
    //called from animation
    public void Deactivate()
    {
        gameObject.SetActive(false);
    }
}
