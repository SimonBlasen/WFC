using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WFCVis : MonoBehaviour
{
    [SerializeField]
    private bool collapse = false;
    [SerializeField]
    private bool collapseEntropy = false;
    [SerializeField]
    private Vector2Int size = Vector2Int.zero;
    [SerializeField]
    private int elementsAmount = 2;
    [SerializeField]
    private int stepsPerSecond = 100;

    [Space]

    [SerializeField]
    private Image outputImage = null;
    [SerializeField]
    private ConstraintsCreator constraintsCreator = null;

    private Collapser collapser;

    private float fSteps = 0f;
    private int steps = 0;
    private bool running = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (collapse)
        {
            collapse = false;
            steps = 0;
            fSteps = 0f;
            running = true;


            collapser = new Collapser();
            collapser.Coll(size.x, size.y, elementsAmount, constraintsCreator.Kernels.ToArray());
        }


        if (running)
        {
            fSteps += Time.deltaTime * stepsPerSecond;

            int stepsNow = (int)fSteps;

            while (steps < stepsNow)
            {
                collapser.Tick();

                steps++;
            }

            outputImage.sprite = Sprite.Create(collapser.Texture, new Rect(0.0f, 0.0f, collapser.Texture.width, collapser.Texture.height), new Vector2(0.5f, 0.5f), 100.0f);
        }


        if (collapseEntropy)
        {
            collapseEntropy = false;

            collapser.CollapseReduceEntropy();
        }
    }
}
