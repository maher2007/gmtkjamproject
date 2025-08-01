using UnityEngine;
using UnityEngine.UI;
public class HealthBar : MonoBehaviour
{
    [SerializeField] Slider slider;
    private GameObject Player;
    private void Awake()
    {
        Player = GameObject.FindGameObjectWithTag("player");
    }

    private void Update()
    {
        slider.value = Player.GetComponent<playercontroller>().health;
    }
}
