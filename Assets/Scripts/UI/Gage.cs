using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Gage : MonoBehaviour
{
    public Image gageImage;

    public float startGage;
    public float curGage;
    public float maxGage;
    public float passiveGage;

    // Start is called before the first frame update
    void Start()
    {
        curGage = startGage;
    }

    // Update is called once per frame
    void Update()
    {
        gageImage.fillAmount = GetPercentage();
    }

    float GetPercentage()
    {
        return curGage / maxGage;
    }

    public void ChangeGage(float amount)
    {
        if (amount >= 0)
        {
            curGage = Mathf.Min(curGage + amount, maxGage);
        }
        else if (amount < 0)
        {
            curGage = Mathf.Max(curGage + amount, 0);
        }
    }
}
