using System.Collections.Generic;
using UnityEngine;

public enum PowerupTarget
{
    Enemies,
    Player,
    Ball,
    Special
}

public enum SpawnConditions
{
    None,
    NotInCircle,
    NeedsMultipleBalls
}

public abstract class PowerupBase : MonoBehaviour
{
    [field: HideInInspector] public PowerupTarget Target { get; set; }

    public Powerups PowerupType => powerupType;
    [SerializeField] protected Powerups powerupType;

    public SpawnConditions Conditions => spawnConditions;
    [SerializeField] protected SpawnConditions spawnConditions;

    public PowerupTarget[] ValidTargets => validTargets;
    [SerializeField] private PowerupTarget[] validTargets;

    public int SpawnLimit => spawnLimit;
    [SerializeField] private int spawnLimit;

    public float DespawnTime => despawnTime;
    [SerializeField] private float despawnTime;

    public float Duration => duration;
    [SerializeField] private float duration;

    protected virtual void EnemyActivate(List<Player> enemies)
    {
    }

    protected virtual void PlayerActivate(Player lastPlayerHit)
    {
    }

    protected virtual void BallActivate(GameObject ball)
    {
    }

    protected virtual void SpecialActivate()
    {
    }

    protected virtual void Revert()
    {
    }

    private List<Player> GetAllEnemies(Player lastPlayerHit)
    {
        var enemies = new List<Player>();

        foreach (var player in PlayerManager.players)
        {
            if (player != lastPlayerHit)
            {
                enemies.Add(player);
            }
        }

        return enemies;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // If powerup collides with ball call appropriate activation method and pass relevant arguments based on powerup type
        if (other.gameObject.tag == "Ball")
        {
            switch (Target)
            {
                case PowerupTarget.Enemies:
                    EnemyActivate(GetAllEnemies(other.GetComponent<Ball>().lastPlayerHit));
                    break;

                case PowerupTarget.Player:
                    if (other.GetComponent<Ball>().lastPlayerHit)
                    {
                        PlayerActivate(other.GetComponent<Ball>().lastPlayerHit);
                    }

                    break;

                case PowerupTarget.Ball:
                    BallActivate(other.gameObject);
                    break;

                case PowerupTarget.Special:
                    SpecialActivate();
                    break;
            }

            Invoke("Revert", duration);
            Destroy(gameObject, duration + 0.1f);

            // Powerup seems destroyed, but is still able to call Revert()
            GetComponent<Collider2D>().enabled = false;
            GetComponent<SpriteRenderer>().enabled = false;
        }
    }
}