using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gagecontroller : MonoBehaviour
{
    public Gage HPGage;
    public Gage StaminaGage;

    // Start is called before the first frame update
    void Awake()
    {
        GameManger.Instance.Player.PlayerGage.controller = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
