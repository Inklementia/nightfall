using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenUrl : MonoBehaviour
{
    [TextArea(2,50)]
    [SerializeField] private string _myUrl;

    public void OpenMyUrl()
    {
        Application.OpenURL(_myUrl);
    }
}
