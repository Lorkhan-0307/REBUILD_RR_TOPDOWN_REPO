using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BulletUI : MonoBehaviour
{
    [SerializeField] private RangeAttack rangeAttack;
    [SerializeField] private Image totalBullet;
    [SerializeField] private Image currentBullet;

    private void Start()
    {
        totalBullet.fillAmount = rangeAttack.bulletCount / 5;

        //CMDebug.ButtonUI(new Vector2(50, -100), "Add Health", () => totalHealthBar.fillAmount = (totalHealthBar.fillAmount * 5) / 3);

    }
    private void Update()
    {
        currentBullet.fillAmount = rangeAttack.bulletCount / 5;
    }

    /*public void UpdateBulletCount()
    {
        currentBullet.fillAmount = rangeAttack.bulletCount / 5;
    }*/

    /*public void UpgradeBulletMax()
    {
        totalBullet.fillAmount = (totalBullet.fillAmount * 5) / 3;
    }*/
}
