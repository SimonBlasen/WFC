using OverlapWFC;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutputGenerator : MonoBehaviour
{
    [SerializeField]
    private bool solve = false;
    [SerializeField]
    private bool solveStep = false;
    [SerializeField]
    private bool solveComplete = false;
    [SerializeField]
    private Vector2Int outputSize;

    [Space]

    [Header("References")]
    [SerializeField]
    private InputReader inputReader = null;
    [SerializeField]
    private Transform outputContentTransform = null;


    private GameObject[,] outputGOs = new GameObject[0, 0];


    private Solver solver;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (solve)
        {
            solve = false;

            for (int x = 0; x < outputGOs.GetLength(0); x++)
            {
                for (int y = 0; y < outputGOs.GetLength(1); y++)
                {
                    if (outputGOs[x, y] != null)
                    {
                        Destroy(outputGOs[x, y]);
                    }
                }
            }
            outputGOs = new GameObject[outputSize.x, outputSize.y];

            solveWFC();
        }

        if (solveStep)
        {
            solveStep = false;

            solver.Iterate();

            renderSolution();
        }

        if (solveComplete)
        {
            bool done = solver.Iterate();

            if (done)
            {
                solveComplete = false;
            }

            renderSolution();
        }
    }


    private void renderSolution()
    {
        for (int x = 0; x < outputGOs.GetLength(0); x++)
        {
            for (int y = 0; y < outputGOs.GetLength(1); y++)
            {
                if (outputGOs[x, y] == null
                    && solver.Slots[x, y].DomainSize == 1)
                {
                    Pattern determinedPattern = inputReader.PatternManager.GetPattern(solver.Slots[x, y].GetDomainAt(0));


                    GameObject instOutput = Instantiate(inputReader.GetMesh(determinedPattern[0, 0]), outputContentTransform);
                    instOutput.transform.localPosition = new Vector3(x, y, 0f);

                    outputGOs[x, y] = instOutput;
                    instOutput.transform.Rotate(0f, 180f, 0f);
                }
            }
        }
    }



    private void solveWFC()
    {
        solver = new Solver(outputSize, inputReader.PatternManager);
    }
}
