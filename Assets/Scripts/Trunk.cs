using UnityEngine;

[AddComponentMenu("TimberMan/Trunk")]
public class Trunk : MonoBehaviour
{
    [Header("Important")]
    [SerializeField] bool canKill = false;
    [SerializeField] bool killToRight = true;

    public bool KillToRight => canKill && killToRight;
    public bool KillToLeft => canKill && !killToRight;
}
