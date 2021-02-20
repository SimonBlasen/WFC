using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WFC3D
{
    public enum WrapStrategy
    {
        NO_PADDING, REPEAT, CLAMP,
    }

    public class InputReader : MonoBehaviour
    {
        [SerializeField]
        private bool calculateInput = false;

        [Space]

        [SerializeField]
        private int patternSize = 2;
        [SerializeField]
        private Vector3 inputGridSize = new Vector3(3f, 1.1f, 3f);
        [SerializeField]
        private bool mirror = false;
        [SerializeField]
        private bool rotate = false;
        [SerializeField]
        private WrapStrategy wrapStrategy = WrapStrategy.NO_PADDING;

        [Space]

        [Header("References")]
        [SerializeField]
        private Transform contentTransform = null;

        private int[,,] inputValues = null;

        void Start()
        {

        }

        void Update()
        {

        }


        private void calcInput()
        {

        }
    }

}