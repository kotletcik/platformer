//Wiktoria Burniak, Paweł Gardzijewski
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

namespace Game023
{
    public class MainMenu : MonoBehaviour
    {
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
        public void OnLevel1ButtonPressed()
        {
            StartCoroutine(StartGame("Level1"));
        }

        public void OnExitToDesktopButtonPressed()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
            Application.Quit();
        }

        private IEnumerator StartGame(string levelName)
        {
            yield return new WaitForSeconds(0.1f);
            SceneManager.LoadScene(levelName);
        }
    }
}
