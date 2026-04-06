using System;
using UnityEngine;

public class GunController : MonoBehaviour
{
    [SerializeField] private float shootingRange = 12f;
    [SerializeField] private LayerMask enemy;

    public void Shoot()
    {
   
        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, shootingRange, enemy))
        {
  
            Debug.Log("Enemy takes hit");
        }
        else
        {

            Debug.Log("It's not an enemy");
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawRay(transform.position, transform.forward * shootingRange);
    }
}
