using UnityEngine;

public class CarAnimationManager : MonoBehaviour
{

    Animator animator;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public bool  PlayDriverDoorOpen()
    {
        animator.SetTrigger("OpenDriverDoor");
        return true;
    }

    public bool PlayDriverDoorClose()
    {
        animator.SetTrigger("CloseDriverDoor");
        return false;
    }

}
