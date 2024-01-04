using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public int gold;
    public Player player;
    public Monster monster;

    [Header(" [ Game Var Header ]")]
    public int inStageCount;
    public bool[] isClear;
    public bool[] isFirstClear;
    public int clearStageIndex = 0;

    public bool isStop = false;
    private Scene scene;

    private void Awake()
    {
        if (instance == null) instance = this;
        else if (instance != null) Destroy(gameObject);

        DontDestroyOnLoad(gameObject);

        isClear = new bool[CubeManager.instance.cube.Length];
        isFirstClear = new bool[CubeManager.instance.cube.Length];
    }

    private void Start()
    {
        for (int i = 0; i < isClear.Length; i++)
        {
            int clear = PlayerPrefs.GetInt("isClear" + i);
            isClear[i] = clear == 1 ? true : false;
        }
        if(!PlayerPrefs.HasKey("Gold"))
        {
            PlayerPrefs.SetInt("Gold", gold);
        }

        clearStageIndex = PlayerPrefs.GetInt("ClearIndex");
        gold = PlayerPrefs.GetInt("Gold");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha0)) SceneManager.LoadScene("MainCube");
    }

    public void GameClear()
    {
        InGameUiManager.instance.panel[0].SetActive(true);
    }

    public void GameOver()
    {
        InGameUiManager.instance.panel[1].SetActive(true);
    }
}
