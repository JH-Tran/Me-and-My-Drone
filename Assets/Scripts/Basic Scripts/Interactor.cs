using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface IInteractable
{
    public void Interact();
}

public class Interactor : MonoBehaviour
{
    public Transform playerCameraTransform;
    public float interactRange = 20;
    public GameObject drone;
    public GameObject remote;
    public bool pickedup = false;

    void Update()
    {
        
    }

    public void playerInteraction()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Ray r = new Ray(playerCameraTransform.position, playerCameraTransform.forward);
            if (Physics.Raycast(r, out RaycastHit hitInfo, interactRange))
            {
                if (hitInfo.collider.gameObject.TryGetComponent(out IInteractable interactObject))
                {
                    interactObject.Interact();
                }
            }
        }
    }
}