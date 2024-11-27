using UnityEngine;
using UnityEngine.UI;

public class enemyHealthBar : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private Camera cam;
    public void UpdateHealthBar(float curVal, float maxVal)
    { 
        slider.value = curVal / maxVal;
    }
    // Update is called once per frame
    void Update()
    {
        transform.rotation = cam.transform.rotation;
    }
}
