using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    float speed;
    [SerializeField]
    float rotationSpeed;

    Vector2 screenBounds;


    // Start is called before the first frame update
    void Awake()
    {
        screenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));

    }

    // Update is called once per frame
    void Update()
    {
        //transform.position =  Mathf.Clamp()
        if (Input.GetKey(KeyCode.W))
        {
            transform.Translate(transform.up * speed * Time.deltaTime, Space.World);
        }

        float horizontal;
        horizontal = Input.GetAxis("Horizontal") * rotationSpeed;
        horizontal *= Time.deltaTime;
        transform.Rotate(0, 0, -horizontal, Space.World);
    }

    private void LateUpdate()
    {
        Vector3 view = transform.position;
        view.x = Mathf.Clamp(view.x, -screenBounds.x, screenBounds.x);
        view.y = Mathf.Clamp(view.y, -screenBounds.y, screenBounds.y);
        transform.position = view;
    }
}
