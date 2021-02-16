using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConstraintsCreator : MonoBehaviour
{
    [SerializeField]
    private GameObject prefabCellButton = null;
    [SerializeField]
    private int spacing = 5;
    [SerializeField]
    private Transform parentTrans = null;

    [Space]


    [SerializeField]
    private Vector2Int size = Vector2Int.zero;
    [SerializeField]
    private int amountStates = 5;
    [SerializeField]
    private int kernelSize = 3;

    [Space]

    [SerializeField]
    private bool generateKernels = false;

    private Image[,] images = null;
    private int[,] states = null;

    // Start is called before the first frame update
    void Start()
    {
        images = new Image[size.x, size.y];
        states = new int[size.x, size.y];

        for (int x = 0; x < size.x; x++)
        {
            for (int y = 0; y < size.y; y++)
            {
                GameObject instObj = Instantiate(prefabCellButton, parentTrans);
                instObj.transform.localPosition = new Vector3(x * spacing, y * spacing, 0f);
                instObj.GetComponentInChildren<ButtonListener>().ButtonPos = new Vector2Int(x, y);
                instObj.GetComponentInChildren<ButtonListener>().ButtonClicked += ButtonClicked;
                //instObj.GetComponentInChildren<Button>().onClick.AddListener(new UnityEngine.Events.UnityAction(() => ButtonClicked(x + y * size.x)));
                

                images[x, y] = instObj.GetComponentInChildren<Image>();
                states[x, y] = 0;

                ButtonClicked(new Vector2Int(x, y));
            }
        }
    }

    public void ButtonClicked(Vector2Int pos)
    {
        states[pos.x, pos.y]++;
        states[pos.x, pos.y] = states[pos.x, pos.y] % amountStates;

        images[pos.x, pos.y].color = GlobalColors.Colors[states[pos.x, pos.y]];
    }

    // Update is called once per frame
    void Update()
    {
        if (generateKernels)
        {
            generateKernels = false;

            List<OverlapKernel> kernels = new List<OverlapKernel>();

            for (int x = 0; x < size.x; x++)
            {
                for (int y = 0; y < size.y; y++)
                {
                    OverlapKernel ok = new OverlapKernel();
                    ok.states = new int[kernelSize, kernelSize];

                    for (int kX = x - (kernelSize / 2); kX <= x + (kernelSize / 2); kX++)
                    {
                        for (int kY = y - (kernelSize / 2); kY <= y + (kernelSize / 2); kY++)
                        {
                            int clampedX = Mathf.Clamp(kX, 0, size.x - 1);
                            int clampedY = Mathf.Clamp(kY, 0, size.y - 1);

                            ok.states[kX - (x - (kernelSize / 2)), kY - (y - (kernelSize / 2))] = states[clampedX, clampedY];
                        }
                    }

                    kernels.Add(ok);
                }
            }

            Kernels = kernels;

            Debug.Log("Generated " + kernels.Count.ToString() + " kernels");
        }
    }


    public List<OverlapKernel> Kernels
    {
        get; protected set;
    } = new List<OverlapKernel>();
}
