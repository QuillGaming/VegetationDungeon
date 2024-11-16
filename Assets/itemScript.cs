using UnityEngine;

public class itemScript : MonoBehaviour
{
    public GameObject swordFloor;
    public GameObject swordHeld;
    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        swordHeld.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }
}