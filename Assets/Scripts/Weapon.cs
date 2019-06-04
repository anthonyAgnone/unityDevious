using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
  // Start is called before the first frame update
  public Transform firePoint;
  public Animator animator;

  public bool isShooting = false;

  // Update is called once per frame
  void Update()
  {
    if (Input.GetButtonDown("Fire2"))
    {
      Shoot();
    }

    if (Input.GetButtonUp("Fire2"))
    {
      isShooting = false;
      animator.SetBool("isShooting", false);
    }
  }

  void FixedUpdate()
  {

  }

  void Shoot()
  {
    isShooting = true;
    animator.SetBool("isShooting", true);
    Debug.Log(isShooting);
  }
}
