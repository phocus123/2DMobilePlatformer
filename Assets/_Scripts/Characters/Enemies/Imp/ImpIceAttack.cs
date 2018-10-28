using PHOCUS.Character;
using UnityEngine;

public class ImpIceAttack : MonoBehaviour
{
    public GameObject ProjectilePrefab;
    public GameObject ExitPoint;

    public void Fire()
    {
        var go = Instantiate(ProjectilePrefab, ExitPoint.transform.position, Quaternion.identity);
        var projectileAttack = go.GetComponent<ProjectileAttack>();

        projectileAttack.Enemy = GetComponentInParent<Enemy>();
    }
}
