using System;
using UnityEngine;

public class CarDriverDoor : MonoBehaviour, Iinteractable
{
    [SerializeField] CarAnimationManager carAnimationManager;
    [SerializeField] Car car;
    [SerializeField] bool isDriverDoor = true;
    public void Interact()
    {
        if (car == null || carAnimationManager == null) return;

        if (isDriverDoor)
        {
            // Toggle driver door
            car.isDriverDoorOpen = car.isDriverDoorOpen ?
                carAnimationManager.PlayDriverDoorClose() :
                carAnimationManager.PlayDriverDoorOpen();
        }
        else
        {
            // Toggle passenger door
            car.isPassengerDoorOpen = car.isPassengerDoorOpen ?
                carAnimationManager.PlayPassengerDoorClose() :
                carAnimationManager.PlayPassengerDoorOpen();
        }
    }
}
