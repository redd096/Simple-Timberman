using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using redd096;

[AddComponentMenu("TimberMan/Player/Player")]
public class Player : MonoBehaviour
{
    public System.Action<bool> onTap;
    public System.Action<bool> onDie;

    bool dead;

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

        //chop tree
        GameManager.instance.levelManager.PlayerChop(rightTap);
    }

    public void Die(bool killedByBranch)
    {
        //be sure player is not already dead
        if (dead)
            return;

        //set player dead
        dead = true;

        onDie?.Invoke(killedByBranch);
    }
}
