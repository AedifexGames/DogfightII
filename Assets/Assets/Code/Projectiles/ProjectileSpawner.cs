using UnityEngine;

public class ProjectileSpawner : MonoBehaviour
{
    [SerializeField] private ProjectileSO _projectileData;
    [SerializeField] private Transform _firePoint;
    public void Fire()
    {
        GameObject.Instantiate(_projectileData.GetProjectile(), _firePoint.position, _firePoint.rotation);
        //_projectileData.GetFireAudio()?.GetAudio();
    }
}
