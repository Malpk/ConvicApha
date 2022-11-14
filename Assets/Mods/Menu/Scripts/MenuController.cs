using System;
using UnityEngine;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{ 
    [SerializeField] private MainMenu mainMenu;
    [SerializeField] private Camera camera;
    [SerializeField] private Image backGround;
    [SerializeField] private Sprite defaultBgSprite;
    [SerializeField] private int translationSpeed;
    private Banner banner;
    private bool bannerChoosed;
    private Vector3 startPos;

    private void Start()
    {
        startPos = camera.transform.position;
        mainMenu.CreateNewConfig();
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
                backGround.gameObject.SetActive(false);
                banner = raycastHit2D.transform.gameObject.GetComponent<Banner>();
                camera.transform.position = banner.ownSceneStartPos;
            }
            Translate();
        }
        else
        {
            camera.transform.position = startPos;
            bannerChoosed = false;
            backGround.gameObject.SetActive(true);
            backGround.sprite = defaultBgSprite;
        }
    }

    private void Translate()
    {
        camera.transform.Translate(Vector3.right * Time.deltaTime * translationSpeed * 0.1f);
    }
}
