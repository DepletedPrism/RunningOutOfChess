using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class PlayerWhite : Player {
  public Vector3 destination = Vector3.zero;

  override public bool Move() {
    const float moveSpeed = 1.0f;

    destination = transform.position;
    if (transform.position == destination) {
      if (Input.GetKeyDown(KeyCode.UpArrow) && isAvailable(Vector3.forward)) {
        destination += Vector3.forward;
      }
      if (Input.GetKeyDown(KeyCode.DownArrow) && isAvailable(Vector3.back)) {
        destination += Vector3.back;
      }
      if (Input.GetKeyDown(KeyCode.LeftArrow) && isAvailable(Vector3.left)) {
        destination += Vector3.left;
      }
      if (Input.GetKeyDown(KeyCode.RightArrow) && isAvailable(Vector3.right)) {
        destination += Vector3.right;
      }
    }
    if (transform.position != destination) {
      transform.position = Vector3.MoveTowards(transform.position, transform.position + Vector3.up, moveSpeed);
      transform.position = Vector3.MoveTowards(transform.position, destination + Vector3.up, moveSpeed);
      return true;
    }

    return false;
  }

  override public bool Remove() {
    if (Input.GetKeyDown(KeyCode.RightControl)) {
      // TODO
    }

    return false;
  }
}
