using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerLevel : MonoBehaviour
{
    float t = 0;
    float old;

    Vector3 oldPos, newPos;
    bool _active;

    bool _first;

    [SerializeField]
    private float newSize = 0.2f;

    [SerializeField]
    private Scene nextScene;

    void Update()
    {
        if(_active)
        {
            if(_first)
            {
                oldPos = Camera.main.transform.position;
                newPos = transform.position;
                newPos.z = Camera.main.transform.position.z;
                
            }
            Camera.main.orthographicSize =old + (t) * (newSize - old);
            Camera.main.transform.position = Vector3.Lerp(oldPos, newPos, t);
            t += Time.deltaTime * 2;

            if(t > 1)
            {
                SceneManager.LoadScene(nextScene.handle, LoadSceneMode.Single);
            }
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.tag == "EndLevel")
        {
            old = Camera.main.orthographicSize;
            _active = true; 
            _first = true;
        }
    }
}
