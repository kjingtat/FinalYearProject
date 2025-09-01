using UnityEngine;

public class PlayerAnim : MonoBehaviour
{
    private Animator animator;

    void Awake()
    {
        animator = GetComponent<Animator>();

        if (animator == null)
        {
            Debug.LogError("[PlayerAnim] No Animator found on " + gameObject.name);
        }
    }

    void Update()
    {
        if (animator == null) return;

        bool isMoving = Input.GetKey(KeyCode.W) ||
                        Input.GetKey(KeyCode.A) ||
                        Input.GetKey(KeyCode.S) ||
                        Input.GetKey(KeyCode.D);

        animator.SetBool("IsMoving", isMoving);
    }
}
