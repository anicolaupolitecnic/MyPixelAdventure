using System;
using System.Threading.Tasks;
using UnityEngine;

public class ParallaxEffect : MonoBehaviour
{
    [SerializeField] private bool clouds = false;
    public Transform[] backgrounds; // Array de las tres imágenes
    public float parallaxScale = 0.5f; // Escala del efecto parallax
    public float smoothing = 1f; // Suavizado del movimiento

    private Transform cam; // Referencia a la cámara
    private Vector3 previousCamPos; // Posición anterior de la cámara

    private PlayerManagerProves2D playerManager;

    void Start()
    {
        cam = Camera.main.transform;
        previousCamPos = cam.position;
        playerManager = GameObject.Find("Player").GetComponent<PlayerManagerProves2D>();

        SetBackgroundPosition();
    }

    private void SetBackgroundPosition()
    {
        backgrounds[0].position = new Vector3(cam.transform.position.x - backgrounds[0].GetComponent<SpriteRenderer>().bounds.size.x, backgrounds[0].position.y, backgrounds[0].position.z);
        backgrounds[1].position = new Vector3(cam.transform.position.x, backgrounds[1].position.y, backgrounds[1].position.z);
        backgrounds[2].position = new Vector3(cam.transform.position.x + backgrounds[2].GetComponent<SpriteRenderer>().bounds.size.x, backgrounds[2].position.y, backgrounds[1].position.z);
    }

    void Update()
    {
        // Calcula el movimiento de la cámara
        float parallax = (previousCamPos.x - cam.position.x) * parallaxScale;

        // Mueve cada imagen en función del parallax
        for (int i = 0; i < backgrounds.Length; i++)
        {
            float backgroundTargetPosX = backgrounds[i].position.x + parallax;
            Vector3 backgroundTargetPos = new Vector3(backgroundTargetPosX, backgrounds[i].position.y, backgrounds[i].position.z);
            backgrounds[i].position = backgroundTargetPos; //Vector3.Lerp(backgrounds[i].position, backgroundTargetPos, smoothing * Time.deltaTime);
        }

        if (clouds)
        {
            MoveSpritesBySpeed();
        }
        else
        {
            // Reposiciona las imágenes cuando salen de la pantalla
            RepositionBackgrounds();
        }

        // Actualiza la posición anterior de la cámara
        previousCamPos = cam.position;
    }

    void MoveSpritesBySpeed()
    {
        for (int i = 0; i < backgrounds.Length; i++)
        {
            float backgroundTargetPosX = backgrounds[i].position.x + 0.0005f;
            Vector3 backgroundTargetPos = new Vector3(backgroundTargetPosX, backgrounds[i].position.y, backgrounds[i].position.z);
            backgrounds[i].position = backgroundTargetPos;
            RepositionBackgrounds();
        }
    }

    void RepositionBackgrounds()
    {
        // Obtén el ancho de la pantalla en unidades del mundo
        float cameraWidth = Camera.main.orthographicSize * 2 * Camera.main.aspect;
        Debug.Log("cameraWidth: " + cameraWidth);
        if (playerManager.facingDirection > 0)
        {
            for (int i = 0; i < backgrounds.Length; i++)
            {
                if ((backgrounds[i].position.x + backgrounds[i].GetComponent<SpriteRenderer>().bounds.size.x) < cam.transform.position.x)
                {
                    backgrounds[i].position = new Vector3(cam.transform.position.x + backgrounds[i].GetComponent<SpriteRenderer>().bounds.size.x, backgrounds[i].position.y, backgrounds[i].position.z);
                }
            }
        }
        else 
        {
            for (int i = 0; i < backgrounds.Length; i++)
            {
                if (backgrounds[i].position.x > cam.transform.position.x + backgrounds[i].GetComponent<SpriteRenderer>().bounds.size.x)
                {
                    backgrounds[i].position = new Vector3(cam.transform.position.x - backgrounds[i].GetComponent<SpriteRenderer>().bounds.size.x, backgrounds[i].position.y, backgrounds[i].position.z);
                }
            }
        }
    }
}