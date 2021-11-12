using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
  private static GameManager _instance;
  public static GameManager Instance {
    get {
      return _instance;
    }
  }

  // 当前地图大小 MAXN * MAXK * MAXN
  public const int MAXK = 2, MAXN = 4, TOTAL = MAXN * MAXN;
  public const int triggerRound = 1;

  public int roundCount = 0;
  private int[,] indices = new int[MAXK, TOTAL];
  private bool[,] isDestoried = new bool[MAXK, TOTAL];

  public GameObject Terrain;
  public GameObject GameOver;

  public GameObject PlayerBlack;
  public GameObject PlayerWhite;
  public GameObject PlayerBlackArrow;
  public GameObject PlayerWhiteArrow;

  public Text PlayerBlackStatus;
  public Text PlayerWhiteStatus;

  private GameObject[,,] blocks = new GameObject[MAXK, MAXN, MAXN];

  private void Awake() {
    _instance = this;
    Application.targetFrameRate = 60;
  }

  private bool currentPlayer = true;
  private int levelCount = 0, warningCount = 0;

  private void SwitchPlayer() {
    if (currentPlayer) {
      PlayerBlackStatus.text = "Waiting...";
      PlayerWhiteStatus.text = "Ready.";
    } else {
      PlayerBlackStatus.text = "Ready.";
      PlayerWhiteStatus.text = "Waiting...";
    }

    currentPlayer = !currentPlayer;
    PlayerWhiteArrow.SetActive(!currentPlayer);
    PlayerBlackArrow.SetActive(currentPlayer);
  }

  private void Start() {
    levelCount = warningCount = 0;
    for (int k = 0; k < MAXK; ++k)
      for (int i = 0; i < TOTAL; ++i)
        isDestoried[k, i] = false;

    System.Random rnd = new System.Random();

    for (int k = 0; k < MAXK; ++k) {
      for (int i = 0; i < MAXN; ++i) {
        for (int j = 0; j < MAXN; ++j) {
          Transform tmp = Terrain.transform;
          Vector3 position = new Vector3(i, k, j);
          blocks[k, i, j] = (GameObject)Instantiate(Resources.Load("Perfabs/Block"), position, tmp.rotation, tmp);
          blocks[k, i, j].GetComponent<Block>().idx = k * MAXN * MAXN + i * MAXN + j;
        }
      }

      for (int i = 0; i < TOTAL; ++i)
        indices[k, i] = i;
      for (int i = MAXN * MAXN - 1; i > 0; --i) {
        int idx = rnd.Next(0, i);
        int tmp = indices[k, idx];
        indices[k, idx] = indices[k, i];
        indices[k, i] = tmp;
      }

      for (int i = MAXN + rnd.Next(0, MAXN); i < TOTAL; i += MAXN) {
        int idx = indices[k, i];
        blocks[k, idx / MAXN, idx % MAXN].GetComponent<Block>().SetStatus(2);
      }
    }

    currentPlayer = false;
    SwitchPlayer();
  }

  public void MarkDestory(int idx) {
    isDestoried[idx / TOTAL, idx % TOTAL] = true;
  }
  public void MarkDestory(int idxK, int idx) {
    isDestoried[idxK, idx] = true;
  }

  private bool gameRunning = true;

  private void FixedUpdate() {
    if (!gameRunning) {
      if (Input.GetKeyDown(KeyCode.R)) {
        gameRunning = true;
        SceneManager.LoadScene(0);
      }
      return;
    }

    if (levelCount < MAXK) {
      int warningTotal = warningCount + TOTAL * levelCount;
      if (warningCount < TOTAL && roundCount / triggerRound > warningTotal + 1) {
        int idxK = MAXK - levelCount - 1, idx = indices[levelCount, warningCount++];
        while (warningCount < TOTAL && isDestoried[idxK, idx])
          idx = indices[levelCount, warningCount++];
        if (warningCount != TOTAL) {
          MarkDestory(idxK, idx);
          blocks[MAXK - levelCount - 1, idx / MAXN, idx % MAXN].GetComponent<Block>().SetStatus(1);
        }
      }
      if (warningCount == TOTAL) {
        ++levelCount;
        warningCount = 0;
      }
    }

    if (isOperation()) {
      ++roundCount;
    }
  }

  public void EndGame(int playerNumber) {
    if (playerNumber == 0) {
      GameOver.GetComponentInChildren<Text>().text = "Black Player Wins.";
    }
    if (playerNumber == 1) {
      GameOver.GetComponentInChildren<Text>().text = "White Player Wins.";
    }
    gameRunning = false;
    GameOver.SetActive(true);
  }

  private bool isOperation() {
    if (currentPlayer) { // Black
      if (PlayerBlack.GetComponent<PlayerBlack>().Operate()) {
        SwitchPlayer();
        return true;
      }
    } else { // White
      if (PlayerWhite.GetComponent<PlayerWhite>().Operate()) {
        SwitchPlayer();
        return true;
      }
    }
    return false;
  }
}
