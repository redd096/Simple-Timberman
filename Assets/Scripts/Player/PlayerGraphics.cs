using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[AddComponentMenu("TimberMan/Player/Player Graphics")]
public class PlayerGraphics : MonoBehaviour
{
    [Header("Important")]
    [SerializeField] bool spriteLookRight = false;
    [Tooltip("Position in scene (right, negative is used for left)")] [SerializeField] float xPosition = 1.5f;

    Player player;
    SpriteRenderer spriteRenderer;
    Animator anim;

    private void Awake()
    {
        //get references
        player = GetComponent<Player>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        anim = GetComponentInChildren<Animator>();

        AddEvents();
    }

    private void OnDestroy()
    {
        RemoveEvents();
    }

    #region events

    void AddEvents()
    {
        player.onTap += OnTap;
        player.onDie += OnDie;
    }

    void RemoveEvents()
    {
        player.onTap -= OnTap;
        player.onDie -= OnDie;
    }

    void OnTap(bool rightTap)
    {
        //set x position
        transform.position = new Vector3(rightTap ? xPosition : -xPosition, transform.position.y, transform.position.z);

        //set sprite flip x
        spriteRenderer.flipX = rightTap ? spriteLookRight : !spriteLookRight;

        //start animation
        anim.SetTrigger("OnTap");
    }

    void OnDie(bool killedByBranch)
    {
        if (killedByBranch)
        {
            //flip y
            spriteRenderer.flipY = true;
        }
    }

    #endregion
}
