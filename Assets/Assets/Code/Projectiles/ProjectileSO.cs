using UnityEngine;

[CreateAssetMenu(fileName = "ProjectileSO", menuName = "Scriptable Objects/ProjectileSO")]
public class ProjectileSO : ScriptableObject
{
    [SerializeField] private GameObject _projectile;
    [SerializeField] private AudioSO _fireAudio;
    public GameObject GetProjectile() { return _projectile; }
    public AudioSO GetFireAudio() { return _fireAudio; }
}
