using UnityEngine;

public class SingleShotGun : Gun
{
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
    }
}
