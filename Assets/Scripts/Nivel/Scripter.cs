using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using TMPro;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

public class Scripter : MonoBehaviour
{
    private SoundObject soundObject; // Sound Object
    private AudioSource musicaNivel; // Musica del nivel en juego

    [HideInInspector] public Color playerOneColor; // Color del jugador uno
    private Color playerTwoColor; // Color del jugador dos
    [HideInInspector] public string playerOneName; // Nombre del jugador uno
    private string playerTwoName; // Nombre del jugador dos
    private int highScore; // Puntanción más alta en este nivel

    private bool noOneHasReceivedDamage = true; // Booleano de No Hit

    private Canvas canvas; // The game's canvas
    private InputActions inputActions; // The user's input actions

    private DialogueManager dialogueManager; // The Dialogue Manager
    private BossHealthBarController bossHealthBarController; // The Boss' Health Bar
    private GameObject hud; // Hud del juego
    private GameObject pauseMenu; // menú de pausa del juego
    private GameObject gameOverMenu; // menú de gameover del juego
    private GameObject completedLevelMenu; // menú de nivel completado

    private GameObject highScoreText; // Texto que contiene el highscore (from the Hud)
    private GameObject scoreText; // Texto que contiene el score (from the Hud)
    private GameObject timeText; // Texto que contiene el tiempo (from the Hud)

    [HideInInspector] public bool isMultiplayer; // true = multiplayer (2 jugadores), false = singlepLayer (1 jugador)
    [HideInInspector] public bool isHardMode; // true = hard difficulty, false = normal difficulty
    private Vector3 posicionSpawnSinglePlayer = new(-3, -7, 0); // Posición Spawn Jugador Uno (SinglePlayer)
    private Vector3 posicionSpawnMultiPlayerUno = new(-5, -7, 0); // Posición Spawn Jugador Uno (MultiPlayer)
    private Vector3 posicionSpawnMultiPlayerDos = new(-1, -7, 0); // Posición Spawn Jugador Dos (MultiPlayer)

    private Transform lifeBox; // Contiene los corazones de los jugadores
    private Color lostLifeColor = new(0.7255f, 0.7255f, 0.7255f, 1f); // Color de cuando un corazón está vacío
    private int puntuacion; // Puntuacion actual del juego
    private float tiempoTranscurrido; // Tiempo de Juego
    private bool isPaused = false; // Current's game pause
    private bool isGameOver = false; // Checks the game status
    private bool isLevelCompleted = false; // Checks if the player has won the level

    private List<Transform> posicionJugadores; // Lista de posicion de jugadores
    private PlayerController playerPrefab; // Prefab de jugador
    private PlayerController jugadorUno; // Jugador Uno
    private PlayerController jugadorDos; // Jugador Dos

    ProgressBar barraModoBasadoJugadorUno;
    ProgressBar barraModoBasadoJugadorDos;

    private readonly Dictionary<int, string> colores = new()
    {
        { 1, "#FFFFFF" }, // Blanco
        { 2, "#00FF00" }, // Verde
        { 3, "#FF0000" }, // Rojo
        { 4, "#F3722C" }, // Naranja
        { 94, "#72DDF7" }, // (Special) Pastel Blue Agito
        { 95, "#FFC8DD" }, // (Special) Rosa Pastel Nico
        { 96, "#0AA0FF" }, // (Special) Azul Céxar
        { 97, "#023E8A" }, // (Special) Azul Oscuro Lucas
        { 98, "#FFEE32" }, // (Special) Amarillo Voraz
        { 99, "#0D060F" } // (Special) Negro-Like Based
    };

    void Start()
    {
        Time.timeScale = 1f;

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        soundObject = FindObjectOfType<SoundObject>(); // Obtenemos el Sound Object
        musicaNivel = soundObject.GetAndPlayMusicBackground(SceneManager.GetActiveScene().buildIndex); // Obtiene la música del nivel y la reproduce en loop

        dialogueManager = gameObject.GetComponentInChildren<DialogueManager>(); // Obtenemos el dialogue manager
        bossHealthBarController = gameObject.GetComponentInChildren<BossHealthBarController>(); // Obtenemos el boss health bar
        canvas = gameObject.GetComponentInChildren<Canvas>(); // Obtenemos el canvas (asumiendo que el scripter está en la base de la escena y solo existe un único canvas allí)
        hud = canvas.transform.GetChild(0).gameObject; // Obtenemos el hud
        pauseMenu = canvas.transform.GetChild(1).gameObject; // Obtenemos el menú de pausa
        gameOverMenu = canvas.transform.GetChild(2).gameObject; // Obtenemos el menú de game over
        completedLevelMenu = canvas.transform.GetChild(5).gameObject; // Obtenemos el menú de nivel completado
        playerPrefab = Resources.Load<PlayerController>("Prefabs/Jugadores/Player"); // Obtiene el prefab de jugador

        barraModoBasadoJugadorUno = hud.transform.GetChild(7).GetComponent<ProgressBar>(); // Obtiene barra modo basado jugador uno
        barraModoBasadoJugadorDos = hud.transform.GetChild(8).GetComponent<ProgressBar>(); // Obtiene barra modo basado jugador dos

        lifeBox = hud.transform.GetChild(6); // Obtiene el contenedor de corazones

        highScoreText = hud.transform.GetChild(0).gameObject; // Obtenemos el texto de puntaje
        scoreText = hud.transform.GetChild(1).gameObject; // Obtenemos el texto de vidas
        timeText = hud.transform.GetChild(2).gameObject; // Obtenemos el texto de tiempo

        pauseMenu.SetActive(false); // Desactiva el menú de pausa
        gameOverMenu.SetActive(false); // Desactiva el menú de Game Over

        // Input Actions
        inputActions = new InputActions(); // Obtiene los controles
        inputActions.UI.Enable(); // Desde los controles de UI, los activa
        inputActions.UI.Escape.started += InvokePause; // La función InvokePause es añadido al listener del evento Escape (started)

        LoadFile();
        SpawnHearts();
        SpawnPlayers();
        ActualizarPosicionesJugador();
    }

    private void FixedUpdate()
    {
        tiempoTranscurrido += Time.deltaTime;
        timeText.GetComponent<TextMeshProUGUI>().text = TimeSpan.FromSeconds(tiempoTranscurrido).ToString(@"hh\:mm\:ss");
    }

    /// <summary>
    /// Carga los datos de la partida actual.
    /// </summary>
    private void LoadFile()
    {
        int dataKey = GlobalSettings.activeSlot;

        isMultiplayer = PlayerPrefs.GetInt(dataKey + "isMultiPlayer") == 1;
        isHardMode = PlayerPrefs.GetInt(dataKey + "isHardMode") == 1;
        highScore = PlayerPrefs.GetInt(dataKey + "HiScore" + SceneManager.GetActiveScene().buildIndex);
        highScoreText.GetComponent<TextMeshProUGUI>().text = highScore.ToString("D7");

        string playerOneColorHex = colores[PlayerPrefs.GetInt(dataKey + "PlayerOneColor")];
        playerOneName = PlayerPrefs.GetString(dataKey + "PlayerOneName");
        if (ColorUtility.TryParseHtmlString(playerOneColorHex, out Color playerOneColor)) this.playerOneColor = playerOneColor;

        if (!isMultiplayer) return;

        string playerTwoColorHex = colores[PlayerPrefs.GetInt(dataKey + "PlayerTwoColor")];
        playerTwoName = PlayerPrefs.GetString(dataKey + "PlayerTwoName");
        if (ColorUtility.TryParseHtmlString(playerTwoColorHex, out Color playerTwoColor)) this.playerTwoColor = playerTwoColor;
    }

    /// <summary>
    /// Guarda los datos finales del nivel cuando el jugador completa el nivel.
    /// </summary>
    private void SaveFile()
    {
        int dataKey = GlobalSettings.activeSlot;
        float timePlayed = PlayerPrefs.GetFloat(dataKey + "TimePlayed");
        int fileProgress = PlayerPrefs.GetInt(dataKey + "FileProgress");

        PlayerPrefs.SetInt(dataKey + "HiScore" + SceneManager.GetActiveScene().buildIndex, Mathf.Max(puntuacion, highScore)); // Guarda el highscore        
        PlayerPrefs.SetFloat(dataKey + "TimePlayed", timePlayed + tiempoTranscurrido); // Guarda el tiempo jugado

        if (noOneHasReceivedDamage) PlayerPrefs.SetInt(dataKey + "NoHit" + SceneManager.GetActiveScene().buildIndex, 1); // Guarda el noHit status
        if (fileProgress < SceneManager.GetActiveScene().buildIndex) PlayerPrefs.SetInt(dataKey + "FileProgress", SceneManager.GetActiveScene().buildIndex); // Cambia el progreso del file
    }

    /// <summary>
    /// Muestra los corazones según dificultad y cantidad de jugadores
    /// en el hud de la izquierda dentro de un nivel.
    /// </summary>
    private void SpawnHearts()
    {
        if (!isMultiplayer && !isHardMode) foreach (int i in Enumerable.Range(0, 3)) {
            lifeBox.GetChild(i).gameObject.SetActive(true); // Aparecen los 3 corazones
            lifeBox.GetChild(i).GetComponent<Image>().color = playerOneColor; // Recolorea los corazones del jugador uno
        }
        else if (isMultiplayer && !isHardMode)
        {
            foreach (int i in Enumerable.Range(0, 6)) lifeBox.GetChild(i).gameObject.SetActive(true); // Aparecen los 6 corazones
            foreach (int i in Enumerable.Range(0, 3)) lifeBox.GetChild(i).GetComponent<Image>().color = playerOneColor; // Recolorea los corazones del jugador uno
            foreach (int i in Enumerable.Range(3, 3)) lifeBox.GetChild(i).GetComponent<Image>().color = playerTwoColor; // Recolorea los corazones del jugador dos
        }
        else
        {
            lifeBox.GetChild(1).gameObject.SetActive(true); // Aparece corazón Hardmode
            lifeBox.GetChild(1).GetComponent<Image>().color = Color.black; // Colorea el corazón a negro
        }
    }

    /// <summary>
    /// Inicializa los jugadores según tipo de juego (Single o Multiplayer)
    /// </summary>
    private void SpawnPlayers()
    {
        jugadorUno = Instantiate(playerPrefab, isMultiplayer ? posicionSpawnMultiPlayerUno : posicionSpawnSinglePlayer, Quaternion.identity); // Genera el jugador uno
        jugadorUno.spawnPosition = isMultiplayer ? posicionSpawnMultiPlayerUno : posicionSpawnSinglePlayer;
        jugadorUno.playerId = 1;
        jugadorUno.playerColor = playerOneColor;
        jugadorUno.nombre = playerOneName;

        barraModoBasadoJugadorUno.gameObject.SetActive(true);
        jugadorUno.barraModoBasado = barraModoBasadoJugadorUno;

        if (isMultiplayer)
        {
            jugadorDos = Instantiate(playerPrefab, posicionSpawnMultiPlayerDos, Quaternion.identity);
            jugadorDos.spawnPosition = posicionSpawnMultiPlayerDos;
            jugadorDos.playerId = 2;
            jugadorDos.playerColor = playerTwoColor;
            jugadorDos.nombre = playerTwoName;

            barraModoBasadoJugadorDos.gameObject.SetActive(true);
            jugadorDos.barraModoBasado = barraModoBasadoJugadorDos;
        }
    }

    /// <summary>
    /// Sube el puntaje obtenido en la partida y la muestra en el HUD
    /// </summary>
    /// <param name="score">Cantidad de puntaje obtenido</param>
    public void RaiseScore(int score)
    {
        puntuacion += score;
        scoreText.GetComponent<TextMeshProUGUI>().text = puntuacion.ToString("D7");
    }

    /// <summary>
    /// Retorna el puntaje
    /// </summary>
    public int GetScore()
    {
        return puntuacion;
    }

    /// <summary>
    /// Se dirije al menú principal y termina el nivel
    /// </summary>
    public void MainMenu()
    {
        Time.timeScale = 1f; // Pone el tiempo de vuelta a la normalidad, para evitar errores desde el menú de pausa
        inputActions.Disable(); // Desactiva IO
        soundObject.GetAndPlayMusicBackground(0); // Reproduce la música del main menu
        SceneManager.LoadScene(0); // Se devuelve al Main Menu
    }

    /// <summary>
    /// Se dirije a los créditos y termina el nivel
    /// </summary>
    public void CargarCreditos()
    {
        Time.timeScale = 1f; // Pone el tiempo de vuelta a la normalidad, para evitar errores desde el menú de pausa
        inputActions.Disable(); // Desactiva IO
        SceneManager.LoadScene(6); // Carga secuencia de créditos
    }

    /// <summary>
    /// Evento de cuando se presiona el botón de pausa
    /// Si el juego está en ejecución, instancia la pausa y viceversa.
    /// </summary>
    /// <param name="context">Contexto de cuando se presionó el botón de pausa</param>
    public void InvokePause(InputAction.CallbackContext context)
    {
        if (isGameOver) return; // Si está en game over, no se puede poner pausa
        if (isLevelCompleted) return; // Si está en el menú de nivel completado, no se puede poner pausa
        if (dialogueManager.isDialogueActive) return; // Si está en pleno diálogo, no se puede poner pausa

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        if (!isPaused && !context.control.Equals(inputActions.PlayerOne.BasedMode.controls[0])) PauseGame();
        else if (isPaused && pauseMenu.activeSelf)
        {
            soundObject.PlayUIBack();
            ResumeGame();
        }
    }

    /// <summary>
    /// Pausa el juego y muestra el menú de pausa
    /// </summary>
    public void PauseGame()
    {
        musicaNivel.Pause(); // Para la música del nivel
        pauseMenu.SetActive(true); // Muestra el menú de pausa
        pauseMenu.GetComponentsInChildren<Button>()[0].Select(); // Selecciona el primer botón
        Time.timeScale = 0f; // Para el tiempo en el juego (cero = pause total)
        isPaused = true; // Cambia el estado del juego
    }

    /// <summary>
    /// Quita el menú de pausa y continúa con el juego
    /// </summary>
    public void ResumeGame()
    {
        musicaNivel.UnPause(); // Sigue la música del nivel
        pauseMenu.SetActive(false); // Quita el menú de pausa

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        Time.timeScale = 1f; // Para el tiempo en el juego (uno = tiempo normal)
        isPaused = false; // Cambia el estado del juego
    }

    /// <summary>
    /// Actualiza los corazones del HUD cuando un jugador pierde una vida.
    /// También ejecuta la pantalla de Game Over en caso de perder todas las vidas según las reglas del juego.
    /// </summary>
    /// <param name="playerId">ID del jugador que perdió una vida. Puede ser jugador uno o dos.</param>
    public async void OnLifeLost(int playerId)
    {
        try
        {
            if (isHardMode)
            {
                lifeBox.GetChild(1).GetComponent<Image>().color = lostLifeColor; // Colorea el corazón a gris
                await Task.Delay(1000); // Pausará el thread por 0.9 segundos (para dar tiempo a completar la animación)
                OnGameOver(); // Aparece el game over
                return;
            }

            int idCorazon = (playerId == 1 ? jugadorUno.vidasRestantes : jugadorDos.vidasRestantes + 3) - 1; // Calcula el índice del corazon perdido
            lifeBox.GetChild(idCorazon).GetComponent<Image>().color = lostLifeColor; // Colorea el corazón a gris

            await Task.Delay(1000); // Pausará el thread por 0.9 segundos (para dar tiempo a completar la animación)
            if ((isMultiplayer && (jugadorUno.vidasRestantes + jugadorDos.vidasRestantes == 0)) || (!isMultiplayer && jugadorUno.vidasRestantes == 0))
            { // Si se acaban todas las vidas de los jugadores (Sea en single o multiplayer)
                if (isPaused) ResumeGame(); // Si el jugador pausó al morir, quitará la pausa
                OnGameOver(); // Aparece el game over
            }
        }
        catch { };
    }

    /// <summary>
    /// Actualiza el HUD para cuando se ganó una vida y lo muestra en pantalla.
    /// Solo actualiza lo visual, no se encaraga de modificar los parámetros del jugador.
    /// </summary>
    /// <param name="playerId">ID del jugador que perdió una vida. Puede ser jugador uno o dos.</param>
    public void OnLifeEarnt(int playerId) // Sube una vida en el hud
    {
        int idCorazon = (playerId == 1 ? jugadorUno.vidasRestantes : jugadorDos.vidasRestantes + 3); // Calcula el índice del corazon perdido
        lifeBox.GetChild(idCorazon).GetComponent<Image>().color = playerId == 1 ? playerOneColor : playerTwoColor; // Colorea el corazón a gris
    }

    /// <summary>
    /// Pausa el juego y muestra el menú de Game Over en pantalla.
    /// </summary>
    private void OnGameOver()
    {
        int dataKey = GlobalSettings.activeSlot;
        float timePlayed = PlayerPrefs.GetFloat(dataKey + "TimePlayed");
        isGameOver = true;
        musicaNivel.Pause(); // Para la música del nivel
        PlayerPrefs.SetFloat(dataKey + "TimePlayed", timePlayed + tiempoTranscurrido); // Guarda el tiempo jugado
        Time.timeScale = 0f; // Para el tiempo en el juego (cero = pause total)
        gameOverMenu.SetActive(true);  // Activa el menú de Game Over
        gameOverMenu.GetComponentsInChildren<Button>()[0].Select(); // Selecciona el primer botón
    }

    /// <summary>
    /// Se devuelve al menu principal.
    /// </summary>
    public void OnExit()
    {
        int dataKey = GlobalSettings.activeSlot;
        float timePlayed = PlayerPrefs.GetFloat(dataKey + "TimePlayed");
        PlayerPrefs.SetFloat(dataKey + "TimePlayed", timePlayed + tiempoTranscurrido); // Guarda el tiempo jugado
        MainMenu();
    }
    /// <summary>
    /// Permite acabar y reiniciar el nivel desde cero.
    /// </summary>
    public void RestartGame()
    {
        int dataKey = GlobalSettings.activeSlot;
        float timePlayed = PlayerPrefs.GetFloat(dataKey + "TimePlayed");
        Time.timeScale = 1f; // Para el tiempo en el juego (uno = tiempo normal)
        inputActions.UI.Escape.started -= InvokePause; // La función InvokePause es añadido al listener del evento Escape (started)
        PlayerPrefs.SetFloat(dataKey + "TimePlayed", timePlayed + tiempoTranscurrido); // Guarda el tiempo jugado

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // Recarga esta escena
    }

    /// <summary>
    /// Actualiza la lista de posicion de jugadores
    /// </summary>
    private void ActualizarPosicionesJugador()
    {
        posicionJugadores = new();
        foreach (PlayerController jugador in FindObjectsOfType<PlayerController>().ToList())
        {
            Transform playerTransform = jugador.GetComponent<Transform>();
            posicionJugadores.Add(playerTransform);
        }
    }

    /// <summary>
    /// Devuelve una posición de un jugador aleatorio
    /// </summary>
    /// <remarks>
    /// Si no existen jugadores en juego, devuelve el Vector3 PositiveInfinity.
    /// </remarks>
    /// <returns>Vector3 que representa la posición del jugador.</returns>
    public Vector3 ObtenerPosicionJugadorAleatorio()
    {
        ActualizarPosicionesJugador();

        try { return posicionJugadores[UnityEngine.Random.Range(0, posicionJugadores.Count - 1)].position; }
        catch { return Vector3.positiveInfinity; };
    }

    /// <summary>
    /// Termina el nivel con una victoria.
    /// </summary>
    public void TerminarNivel()
    {
        if (isPaused) ResumeGame(); // Si está pausado (pausaron en última instancia) quitará el pause
        if (isGameOver) return;  // Si el jugador murió milisegundos antes de ganar el nivel, se considera una derrota

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        isLevelCompleted = true; // The player completed the level
        musicaNivel.Pause(); // Para la música del nivel
        soundObject.PlayUISpecial(); // Reproduce musiquita modo basado
        Time.timeScale = 0f; // Para el tiempo en el juego (cero = pause total)
        SaveFile(); // Guarda los datos de la partida
        completedLevelMenu.SetActive(true);  // Activa el menú de victoria
        completedLevelMenu.GetComponent<CompletedLevelMenu>().ShowUp(SceneManager.GetActiveScene().buildIndex, puntuacion, highScore, tiempoTranscurrido, noOneHasReceivedDamage); // Muestra los datos en pantalla
    }

    /// <summary>
    /// Cambia el booleano si es que algún jugador recibe daño
    /// </summary>
    public void DamageReceived()
    {
        noOneHasReceivedDamage = false;
    }

    /// <summary>
    /// Updates the value of the Boss Bar Health
    /// </summary>
    /// <param name="percentageLeft">Life left in scale from 0 to 1</param>
    public void BossBarUpdate(float percentageLeft)
    {
        bossHealthBarController.SetValue(percentageLeft);
    }

    /// <summary>
    /// Enables or disables the Boss Bar Health
    /// </summary>
    /// <param name="enablingMode">status</param>
    public void EnableBossBar(bool enablingMode)
    {
        bossHealthBarController.UpdateEnabled(enablingMode);
    }

    /// <summary>
    /// Get the current status of the BossBarHealth
    /// </summary>
    /// <returns>The Current Status.</returns>
    public bool GetStatusBossBar()
    {
        return bossHealthBarController.GetCurrentStatus();
    }
}