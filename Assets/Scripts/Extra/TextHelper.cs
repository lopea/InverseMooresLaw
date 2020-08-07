using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextHelper : MonoBehaviour
{
    Text _text;

    [SerializeField]
    Player _player;
    
    int lastCount =0;

    int bulletKills = 0;

    void OnAfterKill(KillType type, int amount)
    {
        if(type == KillType.Bullet)
        {
            bulletKills ++;
            print(bulletKills);
        }
    }

    
    void Awake()
    {
        _player.AfterRewindEvent += OnAfterKill;
        _text = GetComponent<Text>();
    }

    void Update()
    {
        switch(bulletKills)
        {
            case 0:
            _text.text = "Good Luck.";
            break;
            case 1:
            _text.text = "Press Q to rewind your PC to an older time.";
            break;
            case 2:
            _text.text = "Press Q";
            break;
            case 3:
            _text.text = "Q";
            break;
            case 4:
            _text.text = "The third button on the third row on the keyboard.";
            break;
            case 5:
            _text.text = "Its right next to the a button and i know you're using that shit.";
            break;
            case 6:
            _text.text = "Really?";
            break;
            case 7:
            _text.text = "Alright, whatever. you're on your own.";
            break;
            default:
            _text.text = "";
            break;
        }
    }

}
