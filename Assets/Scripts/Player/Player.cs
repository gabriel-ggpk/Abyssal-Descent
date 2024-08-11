using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private GameInput gameInput;

    private void Update () {

        Vector2 inputVector = gameInput.GetMovementVectorNormalized();

        Vector3 moveDir = new Vector3(inputVector.x, 0f, 0f);
        transform.position += moveDir * moveSpeed * Time.deltaTime;

        //Debug.Log(inputVector);
    }

}
