using UnityEngine;

public class ProjectileBody : MonoBehaviour
{
    internal ProjectileHandler handler;

    private void Start()
    {
        handler = transform.parent.GetComponent<ProjectileHandler>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        BaseController controller = collision.gameObject.GetComponent<BaseController>();
        if (controller) controller.ProjectileHit(handler);
        else if (handler.IsSameSender(collision.gameObject)) return;
        handler.ToDestroy();
    }
}
