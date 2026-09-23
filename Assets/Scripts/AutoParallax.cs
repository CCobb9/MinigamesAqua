using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class AutoParallax : MonoBehaviour
{
    [Header("Ajustes de Velocidad")]
    [Tooltip("Velocidad base para el Order in Layer 1")]
    [SerializeField] private float baseSpeed = 0.5f;

    [Tooltip("Invertir la dirección del movimiento")]
    [SerializeField] private bool moveLeft = true;

    private SpriteRenderer spriteRenderer;
    private float effectiveSpeed;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        // Calcula la velocidad multiplicando la velocidad base por el "Order in Layer"
        // A mayor número en Order in Layer, mayor será la velocidad.
        int order = spriteRenderer.sortingOrder;
        
        // Si el Order in Layer es 0 o negativo, le asignamos una velocidad mínima
        effectiveSpeed = baseSpeed * Mathf.Max(1, order);
    }

    void Update()
    {
        // Calcula la dirección (-1 para la izquierda, 1 para la derecha)
        float direction = moveLeft ? -1f : 1f;

        // Desplaza el objeto en el eje X
        transform.Translate(Vector3.right * (direction * effectiveSpeed * Time.deltaTime));

        // Si la imagen avanza demasiado, la reseteamos según el ancho del sprite original
        // para lograr un bucle infinito e imperceptible
        float textureWidth = spriteRenderer.sprite.bounds.size.x;
        
        if (moveLeft && transform.position.x <= -textureWidth)
        {
            transform.position += new Vector3(textureWidth, 0, 0);
        }
        else if (!moveLeft && transform.position.x >= textureWidth)
        {
            transform.position -= new Vector3(textureWidth, 0, 0);
        }
    }
}