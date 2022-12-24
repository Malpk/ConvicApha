using UnityEngine;

public class Banner : MonoBehaviour
{
    [SerializeField] private Transform sceneStartPos;
    public string SceneName;
    public Vector3 BackGroundPosition => sceneStartPos.position;
    
}
