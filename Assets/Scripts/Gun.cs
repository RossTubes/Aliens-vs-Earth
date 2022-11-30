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
    private PickUpController pickUpController;

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
        pickUpController = GetComponent<PickUpController>();
        bulletsLeft = magazineSize;
        readyToShoot = true;

        lm = (1 << layerNumber);

    }
    private void Update()
    {
        MyInput();

        //SetText
        if (!reloading)
        text.SetText(bulletsLeft + " / " + magazineSize);
    }
    private void MyInput()
    {
        if (!pickUpController.equipped)
            return;
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
 
        bool raySuccess = Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out rayHit, range);
        Debug.Log("rayhit "+ rayHit.point);

       // Debug.Log("direction"+direction);

        if (raySuccess)        
        {
            if (rayHit.collider.CompareTag("Enemy"))
                rayHit.collider.GetComponent<Enemy>().TakeDamage(damage);
        }
        RaycastHit hit;

        if (Physics.Raycast(shootPoint.transform.position, shootPoint.transform.forward, out hit))
        {
            GameObject tempBullet = Instantiate(bulletHoleGraphic, hit.point,Quaternion.LookRotation(hit.normal));
            GameObject tempMuzzleFlash = Instantiate(muzzleFlash, shootPoint.forward, Quaternion.LookRotation(hit.normal));
            Destroy(tempBullet, 1f);
            Destroy(tempMuzzleFlash, 1f);
        }
        //Graphics

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
        text.SetText("Reloading...");
    }
    private void ReloadFinished()
    {
        bulletsLeft = magazineSize;
        reloading = false;
    }
}
