using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    [SerializeField] private Slider hpSlider;

    public void SetHP(int current, int max)
    {
        hpSlider.maxValue = max;
        hpSlider.value = current;
    }
}
