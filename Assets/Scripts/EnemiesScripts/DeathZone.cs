using UnityEngine;

public class DeathZone : MonoBehaviour
{
    IHealthable player;
    public bool shield = true;
    [SerializeField] int damage = 1; 

    private void Start() => player = GameObject.FindGameObjectWithTag("Player").GetComponent<IHealthable>();

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player" && shield) player.ChangeHp(-damage);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.collider.tag == "Player" && shield) player.ChangeHp(-damage);
    }
}
