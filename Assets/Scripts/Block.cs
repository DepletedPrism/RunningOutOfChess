using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class Block : MonoBehaviour {
  // status 0: block, 1: warning, 2: trap
  public int status = 0;

  public int disappearStart = -1;
  public const int disappearRound = 1;

  public void SetStatus(int code) {
    status = code;
    if (code == 0) {
      Material blockMaterial = Resources.Load("Materials/block_top") as Material;
      GetComponent<MeshRenderer>().material = blockMaterial;
    }
    if (code == 1) {
      Material warnningMaterial = Resources.Load("Materials/warning_top") as Material;
      GetComponent<MeshRenderer>().material = warnningMaterial;
    }
    if (code == 2) {
      Material trapMaterial = Resources.Load("Materials/trap_top") as Material;
      GetComponent<MeshRenderer>().material = trapMaterial;
    }
  }

  void FixedUpdate() {
    int roundCount = GameObject.Find("GameManager").GetComponent<GameManager>().roundCount;
    // warning
    if (status == 1) {
      if (disappearStart < 0) {
        disappearStart = roundCount;
      } else {
        if (roundCount - disappearStart > disappearRound) {
          Destroy(gameObject);
        }
      }
    }
  }
}
