using UnityEngine;

public class ParallaxEffect : MonoBehaviour
{
    public Transform[] backgrounds; // Array de las tres imágenes
    public float parallaxScale = 0.5f; // Escala del efecto parallax
    public float smoothing = 1f; // Suavizado del movimiento

    private Transform cam; // Referencia a la cámara
    private Vector3 previousCamPos; // Posición anterior de la cámara

    void Start()
    {
        cam = Camera.main.transform;
        previousCamPos = cam.position;
    }

    void Update()
    {
        // Calcula el movimiento de la cámara
        float parallax = (previousCamPos.x - cam.position.x) * parallaxScale;

        // Mueve cada imagen en función del parallax
        for (int i = 0; i < backgrounds.Length; i++)
        {
            float backgroundTargetPosX = backgrounds[i].position.x - parallax;
            Vector3 backgroundTargetPos = new Vector3(backgroundTargetPosX, backgrounds[i].position.y, backgrounds[i].position.z);
            backgrounds[i].position = backgroundTargetPos; //Vector3.Lerp(backgrounds[i].position, backgroundTargetPos, smoothing * Time.deltaTime);
        }

        // Reposiciona las imágenes cuando salen de la pantalla
       // RepositionBackgrounds();

        // Actualiza la posición anterior de la cámara
        previousCamPos = cam.position;
    }

    void RepositionBackgrounds()
    {
        // Obtén el ancho de la pantalla en unidades del mundo
        float cameraWidth = Camera.main.orthographicSize * 2 * Camera.main.aspect;

        // Reposiciona las imágenes si salen de la pantalla
        for (int i = 0; i < backgrounds.Length; i++)
        {
            if (cam.position.x - backgrounds[i].position.x > cameraWidth / 2)
            {
                Vector3 newPos = backgrounds[i].position;
                newPos.x += cameraWidth * backgrounds.Length;
                backgrounds[i].position = newPos;
            }
            else if (backgrounds[i].position.x - cam.position.x > cameraWidth / 2)
            {
                Vector3 newPos = backgrounds[i].position;
                newPos.x -= cameraWidth * backgrounds.Length;
                backgrounds[i].position = newPos;
            }
        }
    }
}