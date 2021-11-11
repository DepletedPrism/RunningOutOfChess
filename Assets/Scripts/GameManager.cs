using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class GameManager : MonoBehaviour {
  // 当前地图大小 MAXN * MAXK * MAXN
  public const int MAXK = 2, MAXN = 5;
  public const int triggerRound = 1;

  public int roundCount = 0;
  private int[,] indices = new int[2, MAXN * MAXN];

  public GameObject Terrain;
  public GameObject PlayerWhite;
  public GameObject PlayerBlack;
  private GameObject[,,] blocks = new GameObject[MAXK, MAXN, MAXN];

  private void Awake() {
    Application.targetFrameRate = 60;
  }

  private int currentPlayer = 0;
  private int levelCount = 0, warningCount = 0;

  private void Start() {
    currentPlayer = 0;
    levelCount = warningCount = 0;

    System.Random rnd = new System.Random();

    for (int k = 0; k < MAXK; ++k) {
      for (int i = 0; i < MAXN; ++i) {
        for (int j = 0; j < MAXN; ++j) {
          Transform tmp = Terrain.transform;
          Vector3 position = new Vector3(i, k, j);
          blocks[k, i, j] = (GameObject)Instantiate(Resources.Load("Perfabs/Block"), position, tmp.rotation, tmp);
        }
      }

      for (int i = 0; i < MAXN * MAXN; ++i)
        indices[k, i] = i;
      for (int i = MAXN * MAXN - 1; i > 0; --i) {
        int idx = rnd.Next(0, i);
        int tmp = indices[k, idx];
        indices[k, idx] = indices[k, i];
        indices[k, i] = tmp;
      }

      for (int i = MAXN + rnd.Next(0, MAXN); i < MAXN * MAXN; i += MAXN) {
        int idx = indices[k, i];
        blocks[k, idx / MAXN, idx % MAXN].GetComponent<Block>().SetStatus(2);
      }
    }
  }

  private void FixedUpdate() {
    const int LEVEL_TOTAL = MAXN * MAXN;

    if (levelCount < MAXK) {
      int warningTotal = warningCount + LEVEL_TOTAL * levelCount;
      if (warningCount < LEVEL_TOTAL && roundCount / triggerRound > warningTotal + 1) {
        int idx = indices[levelCount, warningCount++];
        blocks[MAXK - levelCount - 1, idx / MAXN, idx % MAXN].GetComponent<Block>().SetStatus(1);
      }
      if (warningCount == LEVEL_TOTAL) {
        ++levelCount;
        warningCount = 0;
      }
    }

    if (isOperation()) {
      ++roundCount;
    }
  }

  private bool isOperation() {
    if (currentPlayer == 0) { // White
      if (PlayerWhite.GetComponent<PlayerWhite>().Operate()) {
        currentPlayer ^= 1;
        return true;
      }
    }
    if (currentPlayer == 1) { // Black
      if (PlayerBlack.GetComponent<PlayerBlack>().Operate()) {
        currentPlayer ^= 1;
        return true;
      }
    }

    return false;
  }
}
