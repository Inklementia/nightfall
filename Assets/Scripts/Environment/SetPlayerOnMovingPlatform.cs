using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetPlayerOnMovingPlatform : MonoBehaviour
{ 
    //this script is on Platform
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.transform.tag == "Player")
        collision.collider.transform.SetParent(transform);

    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        //if player already died -> dont do this
        if (collision.transform.tag == "Player" && collision.gameObject.activeSelf)
        collision.collider.transform.SetParent(null);

    }
}
