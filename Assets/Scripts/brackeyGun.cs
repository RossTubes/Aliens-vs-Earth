using UnityEngine;
using TMPro;

public class brackeyGun : MonoBehaviour
{
    public float damage = 10f;
    public float range = 100f;

    //Gun stats
    public float timeBetweenShooting, spread, reloadTime, timeBetweenShots;
    public int magazineSize, bulletsPerTap;
    public bool allowButtonHold;
    int bulletsLeft, bulletsShot;
    public TextMeshProUGUI text;
    public RaycastHit rayHit;
    public Transform ShotPointAug;

    //Graphics
    public GameObject muzzleFlash, bulletHoleGraphic;

    //bools 
    bool shooting, readyToShoot, reloading;

    public Camera fpsCam;
    // Update is called once per frame

    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Fire();
        }
        //SetText
        text.SetText(bulletsLeft + " / " + magazineSize);

    }

    void Fire()
    {
        RaycastHit hit;
        if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range))
        {
            Debug.Log(hit.transform.name);
        }
    }
    private void Awake()
    {
        MyInput();
        bulletsLeft = magazineSize;
        readyToShoot = true;

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
         void Shoot()
        {
            readyToShoot = false;

            //Spread
            float x = Random.Range(-spread, spread);
            float y = Random.Range(-spread, spread);

            //Graphics
            Instantiate(bulletHoleGraphic, rayHit.point, Quaternion.Euler(0, 180, 0));
            Instantiate(muzzleFlash, rayHit.point, Quaternion.identity);

            bulletsLeft--;
            bulletsShot--;

            Invoke("ResetShot", timeBetweenShooting);

            if (bulletsShot > 0 && bulletsLeft > 0)
                Invoke("Shoot", timeBetweenShots);
        }
         void ResetShot()
        {
            readyToShoot = true;
        }
         void Reload()
        {
            reloading = true;
            Invoke("ReloadFinished", reloadTime);
        }
         void ReloadFinished()
        {
            bulletsLeft = magazineSize;
            reloading = false;
        }
    }
}
