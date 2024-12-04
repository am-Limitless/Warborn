using Photon.Pun;
using UnityEngine;

public class SingleShotGun : Gun
{
    PhotonView PV;

    private void Awake()
    {
        PV = GetComponent<PhotonView>();
    }

    public override void Use()
    {
        Shoot();
    }

    private void Shoot()
    {
        Camera cam = Camera.main;

        Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f));
        ray.origin = cam.transform.position;

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            hit.collider.gameObject.GetComponent<IDamageable>()?.TakeDamage(((GunInfo)itemInfo).damage);
        }

        PV.RPC("RPC_Shoot", RpcTarget.All, hit.point, hit.normal);
    }

    [PunRPC]
    void RPC_Shoot(Vector3 hitPosition, Vector3 hitNormal)
    {
        GameObject bulletImpactObj = Instantiate(bulletImpactMetalPrefab, hitPosition, Quaternion.LookRotation(hitNormal, Vector3.up));

        Destroy(bulletImpactObj, 1f);
    }
}
