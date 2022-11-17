using System;
using MainMode;
using Underworld;
using UnityEngine;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    [SerializeField] private MapSpawner mapSpawnerMain;
    [SerializeField] private Camera camera;
    [SerializeField] private UnderWorldGameBuilder underWorldGameBuilder;
    [SerializeField] private int translationSpeed;
    private Banner banner;
    private bool bannerChoosed;
    private Vector3 startPos;

    private void Start()
    {
        underWorldGameBuilder.Play();
        startPos = camera.transform.position;
        mapSpawnerMain.Play();
    }

    private void Update()
    {
        Vector2 mousePos = camera.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D raycastHit2D = Physics2D.Raycast(mousePos, transform.forward);
        if (raycastHit2D && raycastHit2D.transform.gameObject.GetComponent<Banner>())
        {
            if (!bannerChoosed)
            {
                bannerChoosed = true;
                banner = raycastHit2D.transform.gameObject.GetComponent<Banner>();
                camera.transform.position = banner.BackGroundPosition;
            }
        }
        else
        {
            if (Math.Abs(camera.transform.position.y - startPos.y) > 0.1f)
            {
                camera.transform.position = startPos;
                bannerChoosed = false;
            }
        }
        Translate();
    }

    private void Translate()
    {
        camera.transform.Translate(Vector3.right * Time.deltaTime * translationSpeed * 0.1f);
    }
}
