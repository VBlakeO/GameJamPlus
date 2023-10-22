using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour, IInteract
{
    public bool canInteract = true;

    public UnityEvent unityEvent;

    void IInteract.Interact()
    {
        if (canInteract)
            unityEvent.Invoke();
    }
}
