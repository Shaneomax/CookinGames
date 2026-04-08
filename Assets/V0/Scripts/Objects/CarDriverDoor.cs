using System;
using UnityEngine;

public class CarDriverDoor : MonoBehaviour, Iinteractable
{
    [SerializeField] CarAnimationManager carAnimationManager;
    [SerializeField] Car car;
    public void Interact()
    {
        if (car != null)
        {
            if(car.isDriverDoorOpen)
            {
                car.isDriverDoorOpen = carAnimationManager.PlayDriverDoorClose();
            }
            else
            {
                car.isDriverDoorOpen = carAnimationManager.PlayDriverDoorOpen();
            }
        }
    }

   
}
