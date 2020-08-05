using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerLevel : MonoBehaviour
{
    float t = 0;
    float old;
    bool _active;

    [SerializeField]
    private float newSize = 0.2f;

    [SerializeField]
    private Scene nextScene;

    void Update()
    {
        if(_active)
        {
            Camera.main.orthographicSize =old + (t) * (newSize - old);
            
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
        }
    }
}
