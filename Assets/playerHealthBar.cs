using UnityEngine;
using UnityEngine.UI;

public class playerHealthBar : MonoBehaviour
{
    [SerializeField] private Slider slider;
    public void UpdateHealthBar(float curVal, float maxVal)
    {
        slider.value = curVal / maxVal;
    }
}
