using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class NpcInteract : MonoBehaviour
{
    public Canvas dialogueCanvas;
    
    public FirstPersonController cameraController;
    
    public TMP_InputField inputField;
    
    public bool canPlayerMove;
    public bool canCameraMove;
    
    // Start is called before the first frame update
    void Start()
    {
        dialogueCanvas.gameObject.SetActive(false);
        cameraController.SetCameraCanMove(true);
        canCameraMove = true;
        canPlayerMove = true;
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

        //enable or disable camera movement and dialogue canvas
        switch (countNPCs)
        {
            case 0 when !canCameraMove:
                canCameraMove = true;
                dialogueCanvas.gameObject.SetActive(false);
                cameraController.SetCameraCanMove(true);
                break;
            case > 0 when canCameraMove:
                canCameraMove = false;
                dialogueCanvas.gameObject.SetActive(true);
                cameraController.SetCameraCanMove(false);
                break;
        }

        //enable or disable movement
        switch (inputField.isFocused)
        {
            case true when canPlayerMove:
                canPlayerMove = false;
                cameraController.SetPlayerCanMove(false);
                break;
            case false when !canPlayerMove:
                canPlayerMove = true;
                cameraController.SetPlayerCanMove(true);
                break;
        }
    }
    
}
