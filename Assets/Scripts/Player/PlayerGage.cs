using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable
{
    void TakePhysicalDamage(int damage);
}

public class PlayerGage : MonoBehaviour,IDamageable
{
    public Gagecontroller controller;

    Gage hp { get { return controller.HPGage; } }
    Gage stamina { get { return controller.StaminaGage; } }

    public Action onTakeDamage;

    // Update is called once per frame
    void Update()
    {
        hp.ChangeGage(hp.passiveGage * Time.deltaTime);
        stamina.ChangeGage(stamina.passiveGage * Time.deltaTime);

        if(stamina.curGage == 0)
        {
            hp.ChangeGage(-10);
        }
        if (hp.curGage == 0) GameOver();
    }

    public void GameOver()
    {
        Debug.Log("GameOver.");
    }

    public void TakePhysicalDamage(int damage)
    {
        hp.ChangeGage((int)damage);
        onTakeDamage?.Invoke();
    }
}
