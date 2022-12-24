using System;
using MainMode;
using Underworld;
using UnityEngine;
using UnityEngine.SceneManagement;
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
    private Banner rememberBanner;

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
            banner = raycastHit2D.transform.gameObject.GetComponent<Banner>();
            if (Input.GetMouseButtonDown(0))
            {
                SceneManager.LoadScene(banner.SceneName);
            }
            if (!bannerChoosed || rememberBanner != banner)
            {
                bannerChoosed = true;
                camera.transform.position = banner.BackGroundPosition - Vector3.forward * 10;
                
            }

            rememberBanner = banner;
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
