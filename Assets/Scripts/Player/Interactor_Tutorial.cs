using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface IInteractableTut
{
    public void Interact();
}

public class Interactor_Tutorial : MonoBehaviour
{
    public Transform interactorSource;
    public float interactRange = 10;
    public GameObject drone;
    public GameObject remote;
    public GameObject pickupRemote;
    public bool pickedup = false;
    public GameObject pedestal;
    private Animator pedestalAnim;
    [SerializeField] GameObject spotlight;

    public GameObject playerButton;
    public Animator playerButtonAnim;
    public PlayerButtonPressed playerButtonPressed;

    void Start()
    {
        pedestalAnim = pedestal.GetComponent<Animator>();
        playerButtonAnim = playerButton.GetComponent<Animator>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            RaycastHit hitInfo;
            Ray r = new Ray(interactorSource.position, interactorSource.forward);
            bool hit = Physics.Raycast(r, out hitInfo, interactRange);
            if (hit)
            {
                if (hitInfo.transform.gameObject.tag == "Remote")
                {
                    pickedup = true;
                    pedestalAnim.SetBool("isEmpty", true);
                    spotlight.SetActive(false);
                }

                if (hitInfo.transform.gameObject == playerButton)
                {
                    if (playerButtonPressed.isOpen == false)
                    {
                        playerButtonPressed.isOpen = true;
                        playerButtonPressed.x = 2;
                    }

                    else if (playerButtonPressed.isOpen == true)
                    {
                        playerButtonPressed.isOpen = false;
                        playerButtonPressed.x = 1;
                    }
                }
            }
        }

        if (pickedup == true)
        {
            drone.SetActive(true);
            remote.SetActive(true);
            if (pickupRemote.activeSelf == true)
            {
                GameObject.FindWithTag("Remote").SetActive(false);
            }

        }
    }
}
