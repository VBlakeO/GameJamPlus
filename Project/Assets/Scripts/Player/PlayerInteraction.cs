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
        if (Input.GetKey(KeyCode.E))
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
