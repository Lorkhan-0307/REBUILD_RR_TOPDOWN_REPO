using UnityEngine;
using UnityEngine.UI;
using CodeMonkey;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Health playerHealth;
    [SerializeField] private Image totalHealthBar;
    [SerializeField] private Image currentHealthBar;

    private void Start()
    {
        totalHealthBar.fillAmount = playerHealth.currentHealth / 5;

        //CMDebug.ButtonUI(new Vector2(50, -100), "Add Health", () => totalHealthBar.fillAmount = (totalHealthBar.fillAmount * 5) / 3);

    }
    private void Update()
    {
        currentHealthBar.fillAmount = playerHealth.currentHealth / 5;
    }

    public void UpgradeHealthBar()
    {
        totalHealthBar.fillAmount = (totalHealthBar.fillAmount * 5) / 3;
    }
}
