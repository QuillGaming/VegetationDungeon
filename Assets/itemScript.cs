using UnityEngine;

public class itemScript : MonoBehaviour
{
    public GameObject swordFloor;
    public GameObject swordHeld;
    public GameObject swordBack;

    public GameObject halberdFloor;
    public GameObject halberdHeld;
    public GameObject halberdBack;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        swordHeld.SetActive(false);
        halberdHeld.SetActive(false);

        swordBack.SetActive(false);
        halberdBack.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }
}