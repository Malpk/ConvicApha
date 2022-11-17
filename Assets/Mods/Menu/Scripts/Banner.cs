using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Banner : MonoBehaviour
{
    public Sprite backGroundSprite;
    [SerializeField] private Transform ownSceneStartPos;

    public Vector3 BackGroundPosition => ownSceneStartPos.position;
}
