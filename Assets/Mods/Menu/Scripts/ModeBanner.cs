using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class ModeBanner : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private int _sceneId;
    [SerializeField] private Player _player;
    [Header("Reference")]
    [SerializeField] private Animator _animator;

    public System.Action<Vector3> OnSelect;

    public void OnPointerEnter(PointerEventData eventData)
    {
        OnSelect?.Invoke(_player.transform.position);
        _animator.SetBool("select", true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _animator.SetBool("select", false);
    }

    public void LoadScene()
    {
        SceneManager.LoadScene(_sceneId);
    }
}
