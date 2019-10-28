using System.Collections.Generic;
using UnityEngine;

public class SpeedPowerup : PowerupBase
{
    [SerializeField] private float ballSpeedIncrease, playerSpeedIncrease, enemySpeedDecrease;

    private Ball affectedBall;

    private List<Player> affectedEnemies;
    private Player affectedPlayer;

    protected override void BallActivate(GameObject ball)
    {
        affectedBall = ball.GetComponent<Ball>();
        affectedBall.ballSpeed += ballSpeedIncrease;
    }

    protected override void PlayerActivate(Player lastPlayerHit)
    {
        affectedPlayer = lastPlayerHit;
        lastPlayerHit.playerSpeed += playerSpeedIncrease;
    }

    protected override void EnemyActivate(List<Player> enemies)
    {
        affectedEnemies = enemies;

        foreach (Player enemy in enemies)
        {
            enemy.playerSpeed -= enemySpeedDecrease;
        }
    }

    protected override void Revert()
    {
        switch (Target)
        {
            case PowerupTarget.Ball:
                affectedBall.ballSpeed -= ballSpeedIncrease;
                break;

            case PowerupTarget.Player:
                affectedPlayer.playerSpeed -= playerSpeedIncrease;
                break;

            case PowerupTarget.Enemies:
                foreach (Player enemy in affectedEnemies)
                {
                    enemy.playerSpeed += enemySpeedDecrease;
                }
                break;
        }
    }
}