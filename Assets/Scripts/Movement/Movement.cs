using UnityEngine;

 
public class Movement : MonoBehaviour
{   

    [SerializeField]private Rigidbody rb;

    [Header("Movement")]
    [SerializeField] private float movementSpeed=5f;
    [SerializeField] private float sprintSpeed=7f;

    [SerializeField] private float jumpSpeed = 5f;
    
    [Header("Ground check")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundRadius = 0.25f;
    [SerializeField] private LayerMask groundMask;
    private bool onGround;
    
    //Funcion que genera todo el movimiento
    public void MoveEntity(float horizontal, float vertical, bool jumpRequested, bool isSprinting)
    {
        onGround = Physics.CheckSphere(groundCheck.position, groundRadius, groundMask);//si la esfera detecta un collider de la capa permitida el valor es true

        Vector3 direction = (transform.right*horizontal+transform.forward*vertical).normalized;

        float yDirection=rb.linearVelocity.y;
        float Xdirection=direction.x*movementSpeed;
        float Zdirection=direction.z*movementSpeed;
        if(onGround && isSprinting)
        {
            Xdirection=direction.x*sprintSpeed;
            Zdirection=direction.z*sprintSpeed;
        
        }

        if(jumpRequested && onGround && yDirection <=0.1f)
        {
            yDirection=jumpSpeed;
        }
         
        
        rb.linearVelocity= new Vector3(
        Xdirection,
        yDirection,
        Zdirection
        );
          

    }
    
    //Representación visual de la esfera en el editor
    private void OnDrawGizmosSelected()
    {
        if (groundCheck == null) return;
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(groundCheck.position, groundRadius);
    }
}

