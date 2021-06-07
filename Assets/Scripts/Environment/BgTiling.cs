using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(SpriteRenderer))]
public class BgTiling : MonoBehaviour
{
    public int offsetX = 2; // so dont get weird errors;

    //use for cheking if we need to instantiate stuff
    private bool hasRightBuddy = false;
    private bool hasLeftBuddy = false;

    private bool reverseScale = false;//used if object is not tilable

    private float _spriteWidth = 0f; // width of our background

    private Camera _cam;
    private Transform _transformWorkspace;

    private void Awake()
    {
        _cam = Camera.main;
        _transformWorkspace = transform;
    }
    private void Start()
    {
        SpriteRenderer sRenderer = GetComponent<SpriteRenderer>();
        _spriteWidth = sRenderer.sprite.bounds.size.x;

    }

    private void Update()
    {
        if (hasLeftBuddy == false || hasRightBuddy == false)
        {
            //calc the camera extend (HALF THE WIDTH) of what camera can see in world coordinates
            float camHorizontalExtend = _cam.orthographicSize * Screen.width / Screen.height;
    

            // calc the xPos where the cam can ssee the edge of the sprite
            float edgeVisiblePosRight = (_transformWorkspace.position.x + _spriteWidth / 2) - camHorizontalExtend;
            float edgeVisiblePosLeft = (_transformWorkspace.position.x - _spriteWidth / 2) + camHorizontalExtend;

            //checking if we can see the edge + calling new tile/buddy
            if(_cam.transform.position.x >= edgeVisiblePosRight - offsetX && hasRightBuddy == false)
            {
                MakeNewBuddy(1);
                hasRightBuddy = true;
            }
            else if(_cam.transform.position.x <= edgeVisiblePosLeft + offsetX && hasLeftBuddy == false)
            {
                MakeNewBuddy(-1);
                hasLeftBuddy = true;
            }
        }
    }

    //for repeating bg on required side
    private void MakeNewBuddy(int rightLeftBuddy) {
        // call new pos for new buddy
        Vector3 newPos = new Vector3(_transformWorkspace.position.x + _spriteWidth * rightLeftBuddy, _transformWorkspace.position.y, _transformWorkspace.position.z);
        Transform newBuddy = Instantiate(_transformWorkspace, newPos, _transformWorkspace.rotation) as Transform;

        if(reverseScale == true)
        {
            newBuddy.localScale = new Vector3(newBuddy.localScale.x * -1, newBuddy.localScale.y, newBuddy.localScale.z);
        }

        newBuddy.parent = _transformWorkspace.parent;

        if(rightLeftBuddy > 0)
        {
            newBuddy.GetComponent<BgTiling>().hasLeftBuddy = true;
        }
        else
        {
            newBuddy.GetComponent<BgTiling>().hasRightBuddy = true;
        }
    }
}
