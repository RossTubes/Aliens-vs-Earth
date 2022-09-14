using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Gun : MonoBehaviour
{
    //Gun stats
    public int damage;
    public float timeBetweenShooting, spread, range, reloadTime, timeBetweenShots;
    public int magazineSize, bulletsPerTap;
    public bool allowButtonHold;
    int bulletsLeft, bulletsShot;
    public TextMeshProUGUI text;

    //bools 
    bool shooting, readyToShoot, reloading;

    //Reference
    public Camera fpsCam;
    public Transform shootPoint;
    public RaycastHit rayHit;
    public int layerNumber = 2;
    private LayerMask lm = 1;

    //Graphics
    public GameObject muzzleFlash, bulletHoleGraphic;
    //public CamShake camShake;
    //public float camShakeMagnitude, camShakeDuration;
    //public TextMeshProUGUI text;

    private void Awake()
    {
        bulletsLeft = magazineSize;
        readyToShoot = true;

        lm = (1 << layerNumber);

    }
    private void Update()
    {
        MyInput();

        //SetText
        text.SetText(bulletsLeft + " / " + magazineSize);
    }
    private void MyInput()
    {
        if (allowButtonHold) shooting = Input.GetKey(KeyCode.Mouse0);
        else shooting = Input.GetKeyDown(KeyCode.Mouse0);

        if (Input.GetKeyDown(KeyCode.R) && bulletsLeft < magazineSize && !reloading) Reload();

        //Shoot
        if (readyToShoot && shooting && !reloading && bulletsLeft > 0)
        {
            bulletsShot = bulletsPerTap;
            Shoot();
        }
    }
    private void Shoot()
    {
        readyToShoot = false;
        
        //Spread
        float x = Random.Range(-spread, spread);
        float y = Random.Range(-spread, spread);
        //Calculate Direction with Spread
        //Vector3 direction = shootPoint.transform.forward; //+ new Vector3(x, y, 0);
        //Vector3 worldPoint = shootPoint.TransformPoint(shootPoint.transform.position);
        bool raySuccess = Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out rayHit, range, gameObject.layer);
        Debug.Log("rayhit "+ rayHit.point);

       // Debug.Log("direction"+direction);

       Debug.DrawRay(fpsCam.transform.position, fpsCam.transform.forward);
        if (raySuccess)        
        {
            //RayCast
            //Debug.Log(rayHit.collider.name);
          

            if (rayHit.collider.CompareTag("Enemy"))
                rayHit.collider.GetComponent<Enemy>().TakeDamage(damage);
        }
        //Debug.Log("hit what?" + rayHit.collider);
        //ShakeCamera
        //camShake.Shake(camShakeDuration, camShakeMagnitude);

        //Graphics
        Instantiate(bulletHoleGraphic, rayHit.point, Quaternion.Euler(0, 180, 0));
        Instantiate(muzzleFlash, rayHit.point, Quaternion.identity);

        bulletsLeft--;
        bulletsShot--;

        Invoke("ResetShot", timeBetweenShooting);

        if (bulletsShot > 0 && bulletsLeft > 0)
            Invoke("Shoot", timeBetweenShots);
    }
    private void ResetShot()
    {
        readyToShoot = true;
    }
    private void Reload()
    {
        reloading = true;
        Invoke("ReloadFinished", reloadTime);
    }
    private void ReloadFinished()
    {
        bulletsLeft = magazineSize;
        reloading = false;
    }
}
