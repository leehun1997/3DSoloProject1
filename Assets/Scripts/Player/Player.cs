using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public PlayerInputSystem InputSystem;
    public PlayerGage PlayerGage;

    public ItemSO itemData;
    public Action addItem;

    public Transform dropItemTransform;

    // Start is called before the first frame update
    void Awake()
    {
        GameManger.Instance.Player = this;
        InputSystem = GetComponent<PlayerInputSystem>();
        PlayerGage = GetComponent<PlayerGage>();
    }
}
