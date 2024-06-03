using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator anim;
    private Vector2 direccion;

    [Header("Estadísticas")]
    public float velocidadDeMovimiento = 10;
    public float fuerzaDeSalto = 5;

    [Header("Colisiones")]
    public LayerMask layerPiso;
    public float radioDeColision;
    public Vector2 abajo;

    [Header("Booleanos")]
    public bool puedeMover = true;
    public bool enSuelo = true;



    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Movimiento();
        Agarres();
    }

    private void Movimiento()
    {
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");

        direccion = new Vector2(x, y);
        Caminar();

        MejorarSalto();
        if (Input.GetKeyDown(KeyCode.Space)) {
            if (enSuelo)
            {
                anim.SetBool("saltar", true);
                Saltar();
            }
        }


        float velocidad;
        if (rb.velocity.y > 0)
        {
            velocidad = 1;
        }
        else
        {
            velocidad = -1;
        }





        if (!enSuelo) {

            anim.SetFloat("velocidadVertical", velocidad);
        } else
        {
            if(velocidad == -1)
            {
                FinalizarSalto();
            }
        }
    }

    public void FinalizarSalto()
    {
        anim.SetBool("saltar", false);
    }


    private void MejorarSalto()
    {
        if(rb.velocity.y < 0)
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (2.5f - 1) * Time.deltaTime;
        }
        else if (rb.velocity.y > 0 && !Input.GetKey(KeyCode.Space))
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (2.0f - 1) * Time.deltaTime;
        }
    }

    private void Agarres()
    {
        enSuelo = Physics2D.OverlapCircle((Vector2)transform.position + abajo, radioDeColision, layerPiso);
    }


    private void Saltar()
    {
        rb.velocity = new Vector2(rb.velocity.x, 0);
        rb.velocity += Vector2.up * fuerzaDeSalto;
    }

    private void Caminar()
    {
        if (puedeMover)
        {
            rb.velocity = new Vector2(direccion.x * velocidadDeMovimiento, rb.velocity.y);
            
            if(direccion != Vector2.zero)
            {
                if (!enSuelo)
                {
                    anim.SetBool("saltar", true);
                } else
                {
                    anim.SetBool("caminar", true);
                }

                if(direccion.x < 0 && transform.localScale.x > 0)
                {
                    transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
                } else if (direccion.x > 0 && transform.localScale.x < 0)
                {
                    transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
                }
            }
            else
            {
                anim.SetBool("caminar", false);
            }
        }
    }



}
