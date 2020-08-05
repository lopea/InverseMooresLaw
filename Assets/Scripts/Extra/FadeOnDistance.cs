using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeOnDistance : MonoBehaviour
{
    bool _gone;

    float t = 1;

    [SerializeField]
    float speed = 1;

    [SerializeField]
    bool deleteOnZero;

    [SerializeField]
    float radius = 1;

    
    public float Value{private set; get;}

    public bool DeleteOnZero => deleteOnZero;
    // Start is called before the first frame update
    void Start()
    {
        Value = 1;
    }

    // Update is called once per frame
    void Update()
    {
        if((transform.position - Player.playerTransform.position).sqrMagnitude > radius * radius)
        {
            _gone = true;   
        }

        if(_gone)
        {
            t -= Time.deltaTime * speed;
            Value = 1- Mathf.Pow(1-(t), 3);
        }
        if(t < 0 && deleteOnZero)
        {
            Destroy(gameObject);
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
