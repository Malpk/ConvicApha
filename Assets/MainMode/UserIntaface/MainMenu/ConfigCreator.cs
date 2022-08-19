using MainMode;
using MainMode.LoadScene;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UserIntaface.MainMenu;

public class ConfigCreator : MonoBehaviour
{
    public MainLoader Loader;
    [SerializeField] private ItemScroller _consumableItemScroller;
    [SerializeField] private ItemScroller _artifactItemScroller;

    [SerializeField] private CharacterScroller _characterScroller;

    private Animator animator;
    private PlayerConfig _config;
    private void Awake()
    {
        animator = GetComponent<Animator>();
        Loader = FindObjectOfType<MainLoader>();
    }
    public async void CreateNewConfig()
    {
        _config = new PlayerConfig(_consumableItemScroller, _artifactItemScroller, _characterScroller);
        animator.SetBool("ShiftPanels", true);
       await Loader.LoadAsync(_config);
    }
}
