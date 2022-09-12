using MainMode;
using MainMode.LoadScene;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UserIntaface.MainMenu;

public class ConfigCreator : MonoBehaviour
{
    [SerializeField] private BaseLoader _sceneLoader;
    [SerializeField] private ItemScroller _consumableItemScroller;
    [SerializeField] private ItemScroller _artifactItemScroller;

    [SerializeField] private CharacterScroller _characterScroller;

    private Animator animator;
    private PlayerConfig _config;
    private void Awake()
    {
        animator = GetComponent<Animator>();
        _sceneLoader = FindObjectOfType<MainLoader>();
    }
    public async void CreateNewConfig()
    {
        _config = new PlayerConfig(_consumableItemScroller, _artifactItemScroller, _characterScroller);
        animator.SetBool("ShiftPanels", true);
       await _sceneLoader.LoadAsync(_config);
    }
}
