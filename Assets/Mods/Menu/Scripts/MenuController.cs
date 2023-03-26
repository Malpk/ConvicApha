using System;
using MainMode;
using Underworld;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    [SerializeField] private MapSpawner mapSpawnerMain;
    [SerializeField] private UnderWorldGameBuilder underWorldGameBuilder;
    [SerializeField] private int translationSpeed;
    private Banner banner;
    private bool bannerChoosed;
    private Vector3 startPos;
    private Banner rememberBanner;



}
