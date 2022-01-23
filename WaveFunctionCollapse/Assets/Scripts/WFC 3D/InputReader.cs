using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WFC3D
{
    public enum WrapStrategy
    {
        NO_PADDING, REPEAT, CLAMP,
    }

    [ExecuteInEditMode]
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

        private PatternManager patternManager;

        public Vector3 InputGridSize
        {
            get
            {
                return inputGridSize;
            }
        }

        void Start()
        {

        }

        void Update()
        {
            if (calculateInput)
            {
                calculateInput = false;

                calcInput();
            }
        }


        private void calcInput()
        {
            InputElement[] inputElements = contentTransform.GetComponentsInChildren<InputElement>();

            patternManager = PatternManager.CreateFromInput(inputElements, inputGridSize);
        }
    }

}