using PHOCUS.Character;
using PHOCUS.Utilities;
using UnityEngine;

public class ProjectileAttack : MonoBehaviour
{
    public Enemy Enemy;
    public float MoveSpeed;

    Rigidbody2D rigid;
    Player player;
    Vector3 horizontalDirection;
    int damage;

    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        player = GameManager.Instance.Player;
        damage = Enemy.Damage;

        var direction = player.transform.position - transform.position;
        horizontalDirection = new Vector3(direction.x, 0, 0);

        Destroy(gameObject, 5f);
    }

    private void Update()
    {
        rigid.velocity = horizontalDirection.normalized * MoveSpeed;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            var damageable = collision.GetComponent<IDamageable>();
            damageable.DealDamage(damage);
            Destroy(gameObject);
        }
      
    }

}
