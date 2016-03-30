using UnityEngine;
using System.Collections;

public class MainMenuScript : MonoBehaviour 
{
    public GUISkin GameSkin;

    private bool _isFirstMenu = true;
    private bool _isLevelSelectMenu = false;
    private bool _isLoadGameMenu = false;
    private bool _isOptionsMenu = false;
    private bool _isAudioOptions = false;
    private bool _isGraphicsOptions = false;
    private bool _isNewGameMenu = false;

    //private string _playerName = "";
    //private string _playerGender = "";
    private int _currentLevel = 0;

    private float _gameVolume = 0.6f;
    private float _gameFOV = 65.0f;
    
    public Camera gameCamera;

    public Texture2D ChantOfChaos;
    public Texture2D Window;
    public Texture2D WindowWall;
    public Texture2D Wall;
    
	// Use this for initialization
	void Start () 
    {
        //PlayerPrefs.DeleteAll();

        _gameVolume = PlayerPrefs.GetFloat("Game Volume", _gameVolume);
        _gameFOV = PlayerPrefs.GetFloat("Game FOV", _gameFOV);

        if (PlayerPrefs.HasKey("Game Volume"))
        {
            AudioListener.volume = PlayerPrefs.GetFloat("Game Volume");
        }
        else
        {
            PlayerPrefs.SetFloat("Game Volume", _gameVolume);
        }

	    if (PlayerPrefs.HasKey("Game FOV"))
	    {
	        gameCamera.fieldOfView = PlayerPrefs.GetFloat("Game FOV");
	    }
	    else
	    {
	        PlayerPrefs.SetFloat("Game FOV", _gameFOV);
	    }
	}
	
	// Update is called once per frame
	void Update () 
    {
        
    }

    void OnGUI() 
    {
        GUI.skin = GameSkin;

        FirstMenu();
        //LoadGameMenu();
        LevelSelectMenu();
        OptionsMenu();
        NewGameOptions();
        AudioOptionsDisplay();
        GraphicsOptionsDisplay();

        GUI.DrawTexture(new Rect(Screen.width / 2 - 220, Screen.height / 2 -330, 420, 300), ChantOfChaos);



        if (_isLevelSelectMenu == true || _isLoadGameMenu == true || _isOptionsMenu == true || _isNewGameMenu == true) 
        {
            if (GUI.Button(new Rect(10, Screen.height - 35, 140, 30), " Back", "Button Style")) 
            {
                _isLevelSelectMenu = false;
                _isLoadGameMenu = false;
                _isOptionsMenu = false;
                _isNewGameMenu = false;
                _isAudioOptions = false;
                _isGraphicsOptions = false;

                _isFirstMenu = true;
            }
        }
    }
    


    void FirstMenu() 
    {
        if (_isFirstMenu) 
        {
            if (GUI.Button(new Rect(Screen.width / 2 - 100, Screen.height / 2 + 10, 200, 70), " New Game", "Button Style")) 
            {
                Application.LoadLevel(1);
            }

            if (GUI.Button(new Rect(Screen.width / 2 - 100, Screen.height / 2 + 90, 200, 70), " Level Select", "Button Style"))
            {
                _isFirstMenu = false;
                _isLevelSelectMenu = true;
            }

            /*if (GUI.Button(new Rect(Screen.width / 2 - 50, Screen.height / 2 + 55, 160, 50), " Audio", "Button Style"))
            {
                _isFirstMenu = false;
                _isOptionsMenu = true;
            }*/

            if (GUI.Button(new Rect(Screen.width / 2 - 100, Screen.height / 2 + 170, 200, 70), " Quit Game", "Button Style"))
            {
                Application.Quit();
            }

            /*
            if (GUI.Button(new Rect(Screen.width / 2 - 50, Screen.height / 2 - 65, 140, 30), " Load Game", "Button Style"))
            {
                _isFirstMenu = false;
                _isLoadGameMenu = true;
            }*/
        }
    }
    
    void NewGameOptions() 
    {
        /*if (_isNewGameMenu) 
        {
            GUI.Label(new Rect(10, Screen.height / 2 - 200, 200, 50), "New Game", "Sub Menu Title");

            GUI.Label(new Rect(10, Screen.height / 2 - 100, 90, 25), "Player Name:");
            _playerName = GUI.TextField(new Rect(120, Screen.height / 2 - 100, 200, 25), _playerName);

            GUI.Label(new Rect(10, Screen.height / 2 - 65, 90, 25), "Player Gender:");
            _playerGender = GUI.TextField(new Rect(120, Screen.height / 2 - 65, 200, 25), _playerGender);


            if (_playerName != "" && _playerGender != "")
            {
                if (GUI.Button(new Rect(10, Screen.height / 2 - 30, 140, 25), " Create", "Button Style 2"))
                {
                    PlayerPrefs.SetString("Player Name", _playerName);
                    PlayerPrefs.SetString("Player Gender", _playerGender);
                    PlayerPrefs.SetString("Current Level", _currentLevel);

                    //Application.LoadLevel("Level01");
                }
            }

            else
            {
                if (GUI.Button(new Rect(10, Screen.height / 2 - 30, 140, 25), " Generating", "Button Style"))
                {

                }
            }
        }*/


    }

    void LoadGameMenu()
    {
        /*if (_isLoadGameMenu) 
        {
            GUI.Label(new Rect(10, Screen.height / 2 - 200, 200, 50), "Load Game", "Sub Menu Title");
            GUI.Box(new Rect(280, Screen.height / 2 - 100, Screen.width / 2 + 100, Screen.height - 600), "Choose Saved Game", "Box Style");

            if (PlayerPrefs.HasKey("Player Name")) 
            { 
                GUI.Label(new Rect(300, Screen.height / 2 - 65, 200, 25), "Player Name: " + PlayerPrefs.GetString("Player Name"));
                GUI.Label(new Rect(300, Screen.height / 2 - 45, 200, 25), "Player Gender: " + PlayerPrefs.GetString("Player Gender"));

                if (GUI.Button(new Rect(Screen.width / 2 + 150, Screen.height / 2 - 65, 140, 25), " Load", "Button Style"))
                {
                    Application.LoadLevel(PlayerPrefs.GetString("Current Level"));
                }

                if (GUI.Button(new Rect(Screen.width / 2 + 150, Screen.height / 2 - 25, 140, 25), " Delete", "Button Style"))
                {
                    PlayerPrefs.DeleteAll();
                }
            }
        }*/
    }

    void LevelSelectMenu()
    {
        if (_isLevelSelectMenu) 
        {
            // Top Buttons. 1x4
            if (GUI.Button(new Rect(Screen.width / 2 - 100, Screen.height / 2 + 10, 200, 70), " Level 01", "Button Style"))
            {
                Application.LoadLevel(1);
            }

            if (GUI.Button(new Rect(Screen.width / 2 - 100, Screen.height / 2 + 90, 200, 70), " Level 02", "Button Style"))
            {
                Application.LoadLevel(2);
            }

            if (GUI.Button(new Rect(Screen.width / 2 - 100, Screen.height / 2 + 170, 200, 70), " Boss", "Button Style"))
            {
                Application.LoadLevel(3);
            }

            /*if (GUI.Button(new Rect(640, Screen.height / 2 - 100, 200, 150), "Level 04", "Level Select"))
            {

            }

            // Buttom Buttons. 1x4
            if (GUI.Button(new Rect(10, Screen.height / 2 + 60, 200, 150), "Level 05", "Level Select"))
            {

            }

            if (GUI.Button(new Rect(220, Screen.height / 2 + 60, 200, 150), "Level 06", "Level Select"))
            {

            }

            if (GUI.Button(new Rect(430, Screen.height / 2 + 60, 200, 150), "Level 07", "Level Select"))
            {

            }

            if (GUI.Button(new Rect(640, Screen.height / 2 + 60, 200, 150), "Level 08", "Level Select"))
            {

            }*/
        }
    }

    void OptionsMenu()
    {
        /*if (_isOptionsMenu) 
        {
            GUI.Label(new Rect(10, Screen.height / 2 - 200, 200, 50), "Audio Options", "Sub Menu Title");

            if (_isAudioOptions == true || _isGraphicsOptions == true)
            {
                GUI.Box(new Rect(Screen.width / 2, 10, Screen.width / 2.25f, Screen.height / 2 + 340), "", "Box Style");
            }

            if (GUI.Button(new Rect(10, Screen.height / 2 - 30, 140, 25), " Audio", "Button Style 2"))
            {
                _isGraphicsOptions = false;
                _isAudioOptions = true;
            }

            if (GUI.Button(new Rect(10, Screen.height / 2 + 5, 140, 25), " Graphic", "Button Style 2"))
            {
                _isAudioOptions = false;
                _isGraphicsOptions = true;
            }
        }*/

        if (_isOptionsMenu)
        {
            GUI.Label(new Rect(Screen.width / 2 - 400, 300, 200, 50), "Audio", "Sub Menu Title");
            GUI.Label(new Rect(Screen.width / 2 - 400, 335, 200, 25), "Game Volume: ");

            _gameVolume = GUI.HorizontalSlider(new Rect(Screen.width / 2 - 300, Screen.height / 2, 600, 25), _gameVolume, 0.0f, 1.0f);
            GUI.Label(new Rect(Screen.width - 330, 350, 50, 25), "" + (System.Math.Round(_gameVolume, 2)));
            AudioListener.volume = _gameVolume;
            if (GUI.Button(new Rect(180, Screen.height - 35, 140, 30), " Apply", "Button Style"))
            {
                PlayerPrefs.SetFloat("Game Volume", _gameVolume);
            }
        }
    }

    public void AudioOptionsDisplay()
    {
        /*if (_isAudioOptions == true)
        {
            GUI.Label(new Rect(Screen.width / 2 + 10, 20, 200, 50), "Audio", "Sub Menu Title");

            GUI.Label(new Rect(Screen.width / 2 + 10, 60, 200, 25), "Game Volume: ");

            _gameVolume = GUI.HorizontalSlider(new Rect(Screen.width / 2 + 10, 150, Screen.width / 2 - 90, 25), _gameVolume, 0.0f, 1.0f);
            GUI.Label(new Rect(Screen.width - 65, 140, 50, 25), "" + (System.Math.Round(_gameVolume, 2)));
            AudioListener.volume = _gameVolume;

            if (GUI.Button(new Rect(Screen.width / 2 + 10, Screen.height - 50, 140, 25), " Apply", "Button Style"))
            {
                PlayerPrefs.SetFloat("Game Volume", _gameVolume);
            }
        }*/
    }
    
    public void GraphicsOptionsDisplay()
    {
        if (_isGraphicsOptions == true)
        {
            GUI.Label(new Rect(Screen.width / 2 + 10, 20, 200, 50), "Video", "Sub Menu Title");

            GUI.Label(new Rect(Screen.width / 2 + 10, 60, 200, 25), "Game FOV: ");
            _gameFOV = GUI.HorizontalSlider(new Rect(Screen.width / 2 + 10, 150, Screen.width / 2 - 90, 25), _gameFOV, 40.0f, 100.0f);
            GUI.Label(new Rect(Screen.width - 65, 140, 50, 25), "" + (int)_gameFOV);
            gameCamera.fieldOfView = _gameFOV;

            GUILayout.BeginVertical();

            GUI.Label(new Rect(Screen.width / 2 + 10, 200, 200, 25), "Graphics Quality");
            for (int i = 0; i < QualitySettings.names.Length; i++)
            {
                if (GUI.Button(new Rect(Screen.width / 2 + 30, 235 + i * 35, 155, 25), QualitySettings.names[i], "Button Style"))
                {
                    QualitySettings.SetQualityLevel(i, true);
                }
            }

            GUILayout.EndVertical();

            if (GUI.Button(new Rect(Screen.width / 2 + 10, Screen.height - 50, 140, 25), " Apply", "Button Style"))
            {
                PlayerPrefs.SetFloat("Game FOV", _gameFOV);
            }
        }
    }
}