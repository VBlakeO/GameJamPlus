using UnityEngine;
using DG.Tweening;

public class ScalingObject : MonoBehaviour
{
    public Vector3 targetScale = new Vector3(1.2f, 1.2f, 1.2f); // Escala final desejada
    public float duration = 1.0f; // Duração da animação


    private void OnEnable() 
    {
        transform.localScale = new Vector3 (0.3f, 0.3f, 0.3f);

        // Escala o objeto alvo para o tamanho desejado
        transform.DOScale(targetScale, duration/3).OnComplete(() =>
            {
                transform.DOScale(new Vector3(1f, 1f, 1f), duration);
            });
    }
}
