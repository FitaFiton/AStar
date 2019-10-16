using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GUIManager : Singleton<GUIManager> {
	public Text TimerText, TopScoreText;
	public Button MainMenuButton;

    /// <summary>
    /// Acción que se ejecuta al pulsar el botón de "Volver al menú"
    /// </summary>
    public void GoToMenu() {
		SceneManager.LoadScene(0);
    }

}
