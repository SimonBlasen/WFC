using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonListener : MonoBehaviour
{
    public delegate void ButtonClickedEvent(Vector2Int buttonPos);

    public event ButtonClickedEvent ButtonClicked;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SelfClicked()
    {
        ButtonClicked?.Invoke(ButtonPos);
    }

    public Vector2Int ButtonPos
    {
        get;set;
    }
}
