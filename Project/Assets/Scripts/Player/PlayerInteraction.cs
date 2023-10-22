using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    [SerializeField] private float range = 3;
    [SerializeField] private LayerMask layer = 3;
    [SerializeField] private bool canInteract = true;

    bool CanInteract()
    {
        if (Time.timeScale <= 0 || !canInteract)
            return false;
        else
            return true;
    }


    void Update()
    {
        bool target = Physics.Raycast(transform.position, transform.forward, out RaycastHit _hit, range, layer, QueryTriggerInteraction.Ignore) && CanInteract();
        HudManager.Instance.ActiveAimCircule(target);

        if (Input.GetKey(KeyCode.Mouse0))
        {
            if (!CanInteract())
                return;

            if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, range, layer, QueryTriggerInteraction.Ignore))
            {
                if (hit.transform.TryGetComponent<IInteract>(out var obj))
                {
                    obj.Interact();
                }
            }
        }
    }
}
