using UnityEngine;

public class itemScript : MonoBehaviour
{
    public GameObject swordFloor;
    public GameObject swordHeld;

    public GameObject halberdFloor;
    public GameObject halberdHeld;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        swordHeld.SetActive(false);
        halberdHeld.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }
}