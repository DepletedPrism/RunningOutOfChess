using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public abstract class Player : MonoBehaviour {
  public bool isGrounded = false;
  public bool isFreezed = false, freezed = false;

  private void Start() {
    // TODO
  }

  private void FixedUpdate() {
    Vector3 position = this.GetComponent<Transform>().position;

    // Player ÒÑµøÈëÐé¿Õ
    if (position.y < -3.0) {
      Destroy(gameObject);
    }
  }

  public bool Operate() {
    if (!isGrounded)
      return false;

    if (isFreezed && freezed) {
      isFreezed = false;
      return true;
    }

    if (Move()) {
      freezed = false;
      return true;
    }
    if (Remove()) {
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
