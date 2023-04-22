using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectChangerPlatform : MonoBehaviour
{
    [SerializeField] List<GameObject> objectList = new List<GameObject>();

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Object")) {
            if (!objectList.Contains(other.gameObject))
            {
                objectList.Add(other.gameObject);
            }
           
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (objectList.Contains(other.gameObject))
        {
            objectList.Remove(other.gameObject);
        }
    }

    public void changeObjectMaterials(Material material)
    {
        foreach (GameObject gameO in objectList)
        {
            gameO.gameObject.GetComponent<Renderer>().material = material;
        } 
    }
}
