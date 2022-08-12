using System;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Threading.Tasks;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    // Variables de identificación de jugadores
    public Color playerColor;
    public Vector3 spawnPosition;
    public int playerId;
    public string nombre;
    public ProgressBar barraModoBasado;

    // Objetos Necesarios
    private Scripter scripter; // Scripter
    private SoundObject soundObject; // Sound Object
    private GlobalSettings globalSettings; // global settings object
    private InputActionMap accionEntrada; // Obtiene los controles
    private GameObject sombraJugador; // Sombra del jugador
    private GameObject spriteDañoRecibido; // Sobreposición sprite blanca al recibir Daño
    private GameObject hitBox; // Hitbox de la nave
    private readonly string[] tagsColisiones = { "Enemy", "BasedEnemyProjectile", "EnemyProjectile" }; // Tags de elementos que afectan al jugador

    // Movimiento del jugador
    private Vector2 vectorMovimientoJugador; // Vector del movimiento del jugador
    private float amplificadorVelocidad = 1f; // Contiene el multiplicador de la velocidad (para el modo lento o muerte)
    private readonly int velocidad = 900; // Velocidad del jugador
    private bool isSlowingDown = false;

    // Proyectiles del jugador
    private Transform armaModoBasadoDerecha; // Posición de cañón del modo basado (derecha)
    private Transform armaModoBasadoIzquierda;  // Posición de cañón del modo basado (izquierda)
    private Transform armaPrincipal;  // Posición de cañón del principal
    private GameObject misil; // Munición (Modo Basado)
    private GameObject bala; // Munición (Cañón Principal)
    private GameObject bomba; // Bomba
    private readonly float cooldownBalas = 0.1f; // Tiempo de cooldown entre bala y bala
    private readonly float cooldownProyectiles = 0.1f;  // Tiempo de cooldown entre proyectil y proyectil
    private float tiempoUltimoBala = 0; // Guarda el tiempo en que se disparó la última bala
    private float tiempoUltimoProyectil = 0; // Guarda el tiempo en que se disparó el último proyectil
    private float tiempoUltimaBomba = -15; // Guarda el tiempo en que se disparó la ultima bomba
    private bool multiplesDisparos = false; // Indica si el jugador puede disparar de multiples cañones
    private int coinsCollected = 0; // Numero de monedas recogidas
    private bool basedCoreRecogido = false; // Indica si el jugador ha recogido un based core

    // Estado y vida del jugador
    public int vidasRestantes; // Contiene las vidas restantes del jugador
    private bool isIntangible = false; // Estado de intangibilidad del jugador

    // Modo Basado
    private bool canBased = false; // Indica si el jugador tiene el PowerUp a dispoisición de ser usado
    private bool isBased = false; // Indica si el jugador está en el modo basado

    /// <summary>
    /// Aqui determinamos que pasara al iniciarse el script
    /// </summary>
    void Start()
    {
        GetComponent<SpriteRenderer>().material.color = playerColor; // Recoloriza la nave
        scripter = FindObjectOfType<Scripter>(); // Obtiene el Scripter
        soundObject = FindObjectOfType<SoundObject>(); // Obtenemos el Sound Object
        globalSettings = FindObjectOfType<GlobalSettings>(); // Obtenemos el global settings object

        barraModoBasado.SetTitle(nombre);
        barraModoBasado.SetColor(playerColor);

        armaModoBasadoDerecha = transform.GetChild(0); // Obtenemos posición de arma del modo basado (derecha)
        armaModoBasadoIzquierda = transform.GetChild(1); // Obtenemos posición de arma del modo basado (izquierda)
        armaPrincipal = transform.GetChild(2); // Obtenemos posición de arma principal
        sombraJugador = transform.GetChild(3).gameObject; // Obtenemos la sombra
        spriteDañoRecibido = transform.GetChild(4).gameObject; // Obtenemos la sombre del daño
        hitBox = transform.GetChild(5).gameObject; // Obtenemos la hitbox del modo lento

        //Guardamos el total de vidas en otra variable para usar una como contador y otra como punto de guardado
        vidasRestantes = scripter.isHardMode ? 1 : 3;

        //Obtiene los prefabs de bala y proyectil
        misil = Resources.Load<GameObject>("Prefabs/Proyectiles/Misil");
        bala = Resources.Load<GameObject>("Prefabs/Proyectiles/Bala");
        bomba = Resources.Load<GameObject>("Prefabs/Proyectiles/Bomb");

        InputStart();
    }

    /// <summary>
    /// Carga los controles del jugador asignado.
    /// </summary>
    private void InputStart()
    {
        // Inicialización de Controles
        accionEntrada = globalSettings.ObtenerMapaJugador(playerId);
        accionEntrada.Enable(); // Desde los controles de UI, los activa
        accionEntrada.actions[3].started += SlowDown;
        accionEntrada.actions[3].canceled += StopSlowDown;
        accionEntrada.actions[2].canceled += ModoBasado;
    }

    private void OnDestroy()
    {
        // Finalización de Controles
        accionEntrada.Disable(); // Desde los controles de UI, los activa
    }

    /// <summary>
    /// Aquí determinamos que pasara en cada frame
    /// </summary>
    void FixedUpdate()
    {
        if (SceneManager.GetActiveScene().name == "Level 1") sombraJugador.transform.localPosition = new Vector3(2, -6, 1);
        if (SceneManager.GetActiveScene().name == "Level 2") sombraJugador.transform.localPosition = new Vector3(0, -6, 1);
        if (SceneManager.GetActiveScene().name == "Level 3") sombraJugador.transform.localPosition = new Vector3(-2, -6, 1);
        if (SceneManager.GetActiveScene().name == "Level 4") sombraJugador.transform.localPosition = new Vector3(0, 6, 1);
        if (SceneManager.GetActiveScene().name == "Level 5") sombraJugador.transform.localPosition = new Vector3(2, -6, 1);

        if (gameObject.GetComponent<Animator>().GetBool("Die") == true) return; // Si la nave está en proceso de muerte, retorna
        if (accionEntrada.actions[1].IsPressed()) FireBullet(); // Dispara si se presionó la tecla de disparo
        if (isBased) FireBomb(); // Si el jugador esta basado, libera una bomba

        if (!isBased) barraModoBasado.AddValue((scripter.isHardMode ? 0.033333f : 0.066666f) * Time.deltaTime); // 0.066666f es 15 segundos, 0.033333f es 30 segundos
        if (barraModoBasado.GetValue() == 1f) canBased = true;
        else canBased = false;

        if (coinsCollected >= 40 || basedCoreRecogido) multiplesDisparos = true; // Si el numero de monedas es igual a la cantidad ingresada o se ha recogido un based core, la nave puede disparar multiples veces
        else multiplesDisparos = false;

        scripter.SetCoins(coinsCollected);
        if (multiplesDisparos) FireProyectile(); // Si la nave puede realizar multiples disparos, dispara xd

        if(isSlowingDown)hitBox.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, Mathf.PingPong(Time.time*3, 1f)); // La opacidad de la hitbox varia entre 0 y 1 con respecto al tiempo

        vectorMovimientoJugador = accionEntrada.actions[0].ReadValue<Vector2>().normalized; // Obtengo Vector2 de Movimiento (Normalizado)
        gameObject.GetComponent<Rigidbody2D>().AddForce(amplificadorVelocidad * velocidad * vectorMovimientoJugador); // Se aplica Vector Movimiento al Jugador

        gameObject.GetComponent<Animator>().SetBool("Moving", Convert.ToBoolean(vectorMovimientoJugador.x)); // Activa la animación de movimiento (jugador) horizontal si es necesitado
        gameObject.GetComponent<SpriteRenderer>().flipX = vectorMovimientoJugador.x < 0; // Efecto espejo en animación (jugador) si es movimiento a la izquierda

        if (isSlowingDown) hitBox.transform.localPosition = new Vector3(vectorMovimientoJugador.x, 0, -1); // Mueve la hitbox del modo slowdown en el movimiento horizontal
        else hitBox.transform.localPosition = Vector3.zero; // Mueve la hitbox del modo slowdown en el movimiento horizontal

        sombraJugador.GetComponent<Animator>().SetBool("Moving", Convert.ToBoolean(vectorMovimientoJugador.x)); // Activa la animación del movimiento horizontal (sombra) si es necesitado
        sombraJugador.GetComponent<SpriteRenderer>().flipX = vectorMovimientoJugador.x < 0; // Efecto espejo en animación (sombra) si es movimiento a la izquierda
        sombraJugador.GetComponent<Animator>().SetBool("Slow", accionEntrada.actions[3].IsPressed()); // Limita la animación de la sombra si está en modo lento

        spriteDañoRecibido.GetComponent<Animator>().SetBool("Moving", Convert.ToBoolean(vectorMovimientoJugador.x)); // Activa la animación del movimiento horizontal (sombra) si es necesitado
        spriteDañoRecibido.GetComponent<SpriteRenderer>().flipX = vectorMovimientoJugador.x < 0; // Efecto espejo en animación (sombra) si es movimiento a la izquierda
        spriteDañoRecibido.GetComponent<Animator>().SetBool("Slow", accionEntrada.actions[3].IsPressed()); // Limita la animación de la sombra si está en modo lento
    }
    
    /// <summary>
    /// Dispara una bala si el cooldown lo permite.
    /// </summary>
    private void FireBullet()
    {
        if (tiempoUltimoBala + cooldownBalas <= Time.time)  // Si ha pasado el suficiente tiempo para disparar
        {
            tiempoUltimoBala = Time.time; // Actualiza último tiempo disparo de bala
            Instantiate(bala, armaPrincipal.position, Quaternion.identity); // Instancia una bala en el arma principal
            if (!multiplesDisparos) soundObject.PlayEffectFireShooting(); // Reproduce música de bala si no se disparan mas balas a la vez
        }
    }

    /// <summary>
    /// Permite aumentar la cantidad de disparos.
    /// </summary>
    private void FireProyectile()
    {
        if (tiempoUltimoProyectil + cooldownProyectiles <= Time.time) // Si ha pasado el suficiente tiempo para disparar
        {
            tiempoUltimoProyectil = Time.time;  // Actualiza último tiempo disparo de proyectil
            Instantiate(misil, armaModoBasadoDerecha.position, Quaternion.identity); // Instancia una bala en el arma del modo basado (derecha)
            Instantiate(misil, armaModoBasadoIzquierda.position, Quaternion.identity); // Instancia una bala en el arma del modo basado (izquierda)
            soundObject.PlayEffectProyectileShooting(); // Reproduce música de proyectil
        }
    }

    /// <summary>
    /// Permite liberar una bomba (modo basado) si el cooldown lo permite.
    /// </summary>
    private void FireBomb()
    {
        if (tiempoUltimaBomba + 16 <= Time.time) // Si ha pasado el suficiente tiempo para disparar
        {
            tiempoUltimaBomba = Time.time;  // Actualiza tiempo de disparo de la bomba
            Instantiate(bomba, armaPrincipal.position, Quaternion.identity); // Instancia una bomba en el arma principal
            soundObject.PlayEffectBasedMode(); // Reproduce música de bomba
        }
    }

    /// <summary>
    /// Activa el modo lento de movimiento en la nave.
    /// </summary>
    /// <param name="context">Contexto del botón de modo lento</param>
    void SlowDown(InputAction.CallbackContext context)
    {
        isSlowingDown = true;
        amplificadorVelocidad = 0.5f; // Activa el multiplicador del modo lento de velocidad
        gameObject.GetComponent<Animator>().SetBool("Slow", true); // Activa animación modo lento (ladeo ligero)
        hitBox.transform.localScale = new(1f, 1f, 1f); // Disminuye la escala de la hitbox
    }

    /// <summary>
    /// Desactiva el modo lento de movimiento en la nave.
    /// </summary>
    /// <param name="context">Contexto del botón de modo lento</param>
    void StopSlowDown(InputAction.CallbackContext context)
    {
        isSlowingDown = false;
        amplificadorVelocidad = 1f; // Devuelve el amplificador al normal
        gameObject.GetComponent<Animator>().SetBool("Slow", false); // Desactiva la animación del modo lento (ahora se ladea completamente)
        hitBox.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0); // La opacidad de la hitbox se reduce a 0
        hitBox.transform.localScale = new(1.5f, 1.5f, 1f); // Aumenta la escala de la hitbox
    }

    /// <summary>
    /// Activa el modo basado si las condiciones lo permiten.
    /// </summary>
    /// <param name="context">Contexto del botón de modo basado</param>
    public async void ModoBasado(InputAction.CallbackContext context)
    {
        try
        {
            if (!canBased) return; // Si no se puede basar, retorna
            barraModoBasado.SetValue(0f);
            barraModoBasado.SetCoolDown(true);
            canBased = false; // Desactiva el poder basarse otra vez
            isBased = true; // Activa el modo basado
            await Task.Delay(15000); // Pausará el thread por 15 segundos (lo que dure el modo basado);
            isBased = false; // Desactiva el modo basado
            barraModoBasado.SetCoolDown(false);
        }
        catch { };
    }

    /// <summary>
    /// Evento de cuando se toma una moneda del modo basado.
    /// </summary>
    public void OnBasedCoinCollected()
    {
        basedCoreRecogido = true;
    }

    /// <summary>
    /// Evento de cuando se toma power up de vida extra
    /// </summary>
    public void OnLifeCoinCollected()
    {
        if (scripter.isHardMode || vidasRestantes >= 3) return; // Si es hardmode o el jugador está máximo de vida, return
        scripter.OnLifeEarnt(playerId); // Sube una vida en el Hud
        vidasRestantes++; // Sube la vida del jugador
    }

    /// <summary>
    /// Evento de cuando se toma una moneda
    /// </summary>
    public void OnCoinCollected()
    {
        coinsCollected += 1;
    }

    /// <summary>
    /// Aqui determinamos que pasara con el personaje cuando colisione con algo en especifico
    /// </summary>
    /// <param name="collision">Elemento con que colisionó la nave</param>
    private async void OnTriggerEnter2D(Collider2D collision)
    {
        try
        {
            if (isIntangible) return; // Si la nave es intangible, retorna
            if (!(tagsColisiones).Any(collision.tag.Contains)) return; // Si no es un tag necesitado, retorna

            scripter.DamageReceived(); // Cambia el booleano de no hit
            PararBarraModoBasado(scripter.isHardMode ? 3500 : 2000); // Cooldown de la barra de modo basado por 1 segundo

            if (barraModoBasado.GetValue() > 0.5f) barraModoBasado.SetValue(0.5f);
            else if (barraModoBasado.GetValue() <= 0.5f && barraModoBasado.GetValue() != 0f) barraModoBasado.SetValue(0f);
            else Muerte();

            soundObject.PlayEffectOnHit(); // Reproduce Música de Daño
            spriteDañoRecibido.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1); // Aparece Sprite de Daño (Sombra Blanca)
            await Task.Delay(150); // Pausará el thread por la cantidad de ms señalado
            spriteDañoRecibido.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0); // Desaparece Sprite de Daño (Sombra Blanca)
        }
        catch { };
    }

    /// <summary>
    /// Otorga cooldown a la barra de modo basado de la nave.
    /// </summary>
    /// <param name="cantidadTiempoMS">Cantidad de milisegundos de cooldown</param>
    private async void PararBarraModoBasado(int cantidadTiempoMS)
    {
        try
        {
            barraModoBasado.SetCoolDown(true);
            await Task.Delay(cantidadTiempoMS); // Pausará el thread por la cantidad de ms señalado
            barraModoBasado.SetCoolDown(false);
        }
        catch { };
    }

    /// <summary>
    /// Evento de muerte de la nave.
    /// </summary>
    private async void Muerte()
    {
        try
        {
            PararBarraModoBasado(1900);
            basedCoreRecogido = false; // En caso de que se haya recogido un nucleo basado antes de morir
            gameObject.GetComponent<Animator>().SetBool("Die", true); // Se activa flag de muerte (animación de explosión)
            isIntangible = true; // La nave ahora es intangible
            if (isBased == true) isBased = canBased = false; // Deshabilita el modo basado y no podrá basarse otra vez al reaparecer
            scripter.OnLifeLost(playerId); // Actualiza corazones en pantalla
            vidasRestantes--; // Pierdes una vida
            await Task.Delay(900); // Pausará el thread por 0.9 segundos (para dar tiempo a completar la animación)

            if (vidasRestantes == 0) // Si el jugador ya no tiene vidas
            {
                Destroy(gameObject); // Eliminar jugador
                return; // Retornar
            }

            gameObject.GetComponent<Animator>().SetBool("Die", false); // Se desactiva flag de muerte
            transform.position = spawnPosition; // Mueve la nave al punto de Respawn
            coinsCollected = 0; // Se eliminan las monedas guardadas
            barraModoBasado.SetValue(scripter.isHardMode ? 0f : 0.5f); // Rellena media barra si es modo normal, la reinicia en modo difícil
            gameObject.GetComponent<Animator>().SetBool("Spawning", true); // Se activa flag de spawning
            await Task.Delay(1000); // Pausará el thread por 1 segundo (para dar tiempo de invulnerabilidad)
            gameObject.GetComponent<Animator>().SetBool("Spawning", false); // Se desactiva flag de spawning
            isIntangible = false; // La nave deja de ser intangible
        }
        catch { };
    }
}