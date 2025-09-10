using UnityEngine;

public class BloodPower : BasePower
{
    [SerializeField]
    protected float healthGain;
    [SerializeField]
    protected float healthLoss;

    protected void LoseHealth()
    {
        player.stats.health += healthGain;
        player.stats.health = Mathf.Clamp(player.stats.health, 0, player.maxHealth);
    }

    protected void GainHealth()
    {
        player.stats.health -= healthLoss;
        player.stats.health = Mathf.Clamp(player.stats.health, 10, player.maxHealth);
    }
    
}
