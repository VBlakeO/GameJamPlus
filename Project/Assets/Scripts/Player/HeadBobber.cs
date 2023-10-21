using UnityEngine;

public class HeadBobber : MonoBehaviour
{
    [SerializeField] private float walkingBobbingSpeed = 14f;
    [SerializeField] private float idleSpeed = 3f;
    [SerializeField] private float bobbingAmount = 0.05f;
    [SerializeField] private PlayerMovement playerMovement;

    private float defaultPosY = 0;
    private float timer = 0;

    void Start()
    {
        defaultPosY = transform.localPosition.y;
    }
    
    void LateUpdate()
    {
        if (playerMovement.cantMove)
            return;

        if (Mathf.Abs(playerMovement.CurrentDir.x) > 0.1f || Mathf.Abs(playerMovement.CurrentDir.y) > 0.1f)
        {
            //Player is moving
            timer += Time.deltaTime * walkingBobbingSpeed;
            transform.localPosition = new Vector3(transform.localPosition.x, defaultPosY + Mathf.Sin(timer) * bobbingAmount, transform.localPosition.z);
        }
        else
        {
            //Idle
            timer = 0;
            transform.localPosition = new Vector3(transform.localPosition.x, Mathf.MoveTowards(transform.localPosition.y, defaultPosY, Time.deltaTime * idleSpeed), transform.localPosition.z);
        }  
    } 
}