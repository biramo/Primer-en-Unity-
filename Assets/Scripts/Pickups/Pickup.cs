using UnityEngine;

public abstract class Pickup : MonoBehaviour
{

    private bool isActive;

    //----------------[Respawn del pickup]----------------

    [SerializeField] private bool canRespawn;

    [SerializeField]private float respawnTime=5f;//Tiempo que tarda en reaparecer
    private float respawnTimer;//El timer que controla el respawn

    //----------------[Posicion y Rotacion]----------------

    [SerializeField]private float rotationSpeed=100f;
    private float startPositionY;
    [SerializeField]private float floatAmplitude = 0.3f; // Altura del movimiento
    [SerializeField]private float floatFrequency = 2f;   // Velocidad de subida y bajada


    //----------------[Componentes a desabilitar]----------------
    private Collider pickupCollider;

    private Renderer pickupRenderer;
    
    [SerializeField] private Light pickupLight;


    void Awake()
    {
        startPositionY=transform.position.y;
        isActive=true;
        pickupCollider=GetComponentInChildren<Collider>();
        pickupRenderer=GetComponentInChildren<Renderer>();

    }

    private void RespawnPickup()
    {   
        if(isActive) return;

        if (Time.time > respawnTimer)  
        {
            pickupRenderer.enabled=true;
            pickupCollider.enabled=true;
            pickupLight.enabled=true;
            isActive=true; 
              
        }
    }

    protected virtual void collectPickup(Player player)
    {
        if (!isActive) return;
        isActive=false;

        pickupRenderer.enabled=false;

        pickupCollider.enabled=false;

        pickupLight.enabled=false;

        respawnTimer=Time.time+respawnTime;
    }
    void Update()
    {   
        if (isActive)
            Spin();
        else if(canRespawn)
            RespawnPickup();
    }
    protected virtual void Spin()
    {
      transform.Rotate(new Vector3(0, rotationSpeed*Time.deltaTime, 0),Space.World);//Rotamos solo la y
      
      //Le añadimos la posicion inicial al valor dado
      float newY = startPositionY + Mathf.Sin(Time.time * floatFrequency)//Multiplicamos el tiempo pasado por la frequencia(velocidad de subida)
                     * floatAmplitude;//Multiplicamos por la altura maxima que llegara
    
        transform.position = new Vector3(
            transform.position.x,
            newY,
            transform.position.z
        );
    }

    void OnTriggerEnter(Collider other)
    {
        Player player = other.GetComponentInParent<Player>();
        if (player == null|| !isActive) return;

         collectPickup(player);             
            
    }
}
