using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class PlayerBlack : Player {
  public Vector3 destination = Vector3.zero;

  private void Start() {
    playerNumber = 1;
  }

  override public bool Move() {
    const float moveSpeed = 1.0f;

    destination = transform.position;
    if (transform.position == destination) {
      if (Input.GetKeyDown(KeyCode.W) && isAvailable(Vector3.forward)) {
        destination += Vector3.forward;
      }
      if (Input.GetKeyDown(KeyCode.S) && isAvailable(Vector3.back)) {
        destination += Vector3.back;
      }
      if (Input.GetKeyDown(KeyCode.A) && isAvailable(Vector3.left)) {
        destination += Vector3.left;
      }
      if (Input.GetKeyDown(KeyCode.D) && isAvailable(Vector3.right)) {
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

  public override bool Remove() {
    if (Input.GetKey(KeyCode.Space)) {
      // Vector3 start = transform.position + Vector3.up, end = start;
      Vector3 start = transform.position, end = start;
      if (Input.GetKeyDown(KeyCode.W)) {
        end += Vector3.forward;
      }
      if (Input.GetKeyDown(KeyCode.S)) {
        end += Vector3.back;
      }
      if (Input.GetKeyDown(KeyCode.A)) {
        end += Vector3.left;
      }
      if (Input.GetKeyDown(KeyCode.D)) {
        end += Vector3.right;
      }

      if (start != end) {
        RaycastHit hitObject;
        if (Physics.Linecast(start, end, out hitObject)) {
          int idx = hitObject.collider.gameObject.GetComponent<Block>().idx;
          if (idx == 0)
            return false;
          Destroy(hitObject.collider.gameObject);
          GameManager.Instance.MarkDestory(idx);
          return true;
        }
      }
    }

    return false;
  }
}
