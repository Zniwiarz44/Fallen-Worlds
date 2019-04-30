using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointerPlacer : MonoBehaviour {

    public Transform objectToPlace;
    public Camera gameCamera;
    public LayerMask mask;
    public float verticalOffset = 0.1f;

    Transform tempPointer = null;

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Mouse0))
        {
            if(tempPointer)
            {
                Destroy(tempPointer.gameObject);
            }
            Ray ray = gameCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;
            
            if (Physics.Raycast(ray, out hitInfo, 500, mask))
            {
                // Instantiate position marker to visually show player's destination
                objectToPlace.position = new Vector3(hitInfo.point.x, hitInfo.point.y + verticalOffset, hitInfo.point.z);
                objectToPlace.rotation = Quaternion.FromToRotation(Vector3.up, hitInfo.normal);
                tempPointer = Instantiate(objectToPlace, objectToPlace.position, objectToPlace.rotation);
            }
        }
    }
}
