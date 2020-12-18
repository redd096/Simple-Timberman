using UnityEngine;

[AddComponentMenu("TimberMan/Player/Player")]
public class Player : MonoBehaviour
{
    bool dead;

    public System.Action<bool> onTap;
    public System.Action<bool> onDie;

    /// <summary>
    /// Called by canvas
    /// </summary>
    /// <param name="rightTap">tap left or right button?</param>
    public void OnTap(bool rightTap)
    {
        //be sure player is not dead
        if (dead)
            return;

        onTap?.Invoke(rightTap);

        //check if player is died after chop
        if (redd096.GameManager.instance.levelManager.PlayerDieOnChop(rightTap))
        {
            Die(true);
        }
    }

    void Die(bool killedByBranch)
    {
        //be sure player is not already dead
        if (dead)
            return;

        //set player dead
        dead = true;

        onDie?.Invoke(killedByBranch);
    }
}
