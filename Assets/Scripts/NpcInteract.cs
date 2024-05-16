using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class NpcInteract : MonoBehaviour
{
    public Canvas dialogueCanvas;
    
    public FirstPersonController cameraController;
    
    // Start is called before the first frame update
    void Start()
    {
        dialogueCanvas.gameObject.SetActive(false);
        cameraController.SetCameraCanMove(true);
    }

    // Update is called once per frame
    void Update()
    {
        float interactRange = 2.0f;
        Collider[] hitColliders = new Collider[10];
        int count = Physics.OverlapSphereNonAlloc(transform.position, interactRange, hitColliders);
        int countNPCs = 0;
        for (int i = 0; i < count; i++)
        {
            if (hitColliders[i].CompareTag("NPC"))
            {
                countNPCs++;
            }
        }
        if (countNPCs == 0 && dialogueCanvas.gameObject.activeSelf)
        {
            dialogueCanvas.gameObject.SetActive(false);
            cameraController.SetCameraCanMove(true);
        }
        else if (countNPCs > 0 && !dialogueCanvas.gameObject.activeSelf)
        {
            dialogueCanvas.gameObject.SetActive(true);
            cameraController.SetCameraCanMove(false);
        }
    }
    
}
