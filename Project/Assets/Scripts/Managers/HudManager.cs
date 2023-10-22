using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class HudManager : Singleton<HudManager>
{
    [SerializeField] private Image _aimBase = null;
    [SerializeField] private Image _aimCircle = null;

    public void ActiveAimCircule(bool active)
    {
       _aimCircle.gameObject.SetActive(active);
    }

}
