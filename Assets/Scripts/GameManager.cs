using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager singleton;
    private GroundPiece[] allGroundPieces; 
    public AudioClip completedSound;
    private AudioSource playerAudio;
    public TextMeshProUGUI gameCompletedText;
    public Button restartButton;

    // Start is called before the first frame update
    void Start()
    {
        playerAudio = GetComponent<AudioSource>();

    }

    public void StartGameButton() {
        SetupNewLevel();
        SceneManager.LoadScene(1);
    }


    private void SetupNewLevel() {
        allGroundPieces = FindObjectsOfType<GroundPiece>();
    }

    private void Awake() {
        if(singleton == null) {
            singleton = this;
        } else if(singleton != this) {
            Destroy(gameObject);
            DontDestroyOnLoad(gameObject);
        }
    }

    private void OnEnable() {
        SceneManager.sceneLoaded += OnLevelFinishedLoading;
    }

    private void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode) {
        SetupNewLevel();
    }

    public void CheckComplete() {
    bool isFinished = true;

    for(int i = 0; i < allGroundPieces.Length; i++) {
        if(allGroundPieces[i].isColored == false) {
            isFinished = false;
            break;
        }
    }        

    if(isFinished) {
        playerAudio.PlayOneShot(completedSound, 1.0f);
        StartCoroutine(BeforeNextLevel());
    }
  }

  IEnumerator BeforeNextLevel() {
    yield return new WaitForSeconds(0.5f);
    NextLevel();
    
  }

    private void NextLevel() {
        if(SceneManager.GetActiveScene().buildIndex == 5) {
            gameCompletedText.gameObject.SetActive(true);
            restartButton.gameObject.SetActive(true);

        }
     
        else {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    }

        public void RestartGame() {
        SceneManager.LoadScene(0); 
    }
}
