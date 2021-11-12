using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public abstract class Player : MonoBehaviour {
  public bool isGrounded = false;
  public bool isFreezed = false, freezed = false;

  public int playerNumber;

  public Text playerStatus;

  private void FixedUpdate() {
    Vector3 position = this.GetComponent<Transform>().position;

    // Player ÒÑµøÈëÐé¿Õ
    if (position.y < -3.0) {
      GameManager.Instance.EndGame(playerNumber);
      Destroy(gameObject);
    }
  }

  public bool Operate() {
    if (!isGrounded) {
      // playerStatus.text = "Falling...";
      return false;
    }

    if (isFreezed && freezed) {
      isFreezed = false;
      return true;
    }

    if (Remove() || Move()) {
      freezed = false;
      return true;
    }

    return false;
  }
  public bool isAvailable(Vector3 direction) {
    return true;
    // return Physics.Linecast(transform.position, direction);
  }

  public abstract bool Move();
  public abstract bool Remove();

  private void OnCollisionEnter(Collision collision) {
    isGrounded = true; // Maybe

    int blockStatus = collision.collider.gameObject.GetComponent<Block>().status;
    if (blockStatus == 2) {
      if (!freezed && !isFreezed) {
        isFreezed = true;
        freezed = true;
      }
    }
  }
}
