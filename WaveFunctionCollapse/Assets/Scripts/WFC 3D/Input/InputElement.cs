using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace WFC3D
{
    [ExecuteInEditMode]
    public class InputElement : MonoBehaviour
    {
        private const float drawLinesDistance = 10f;

        [SerializeField]
        private string conn_down = "";
        [SerializeField]
        private string conn_up = "";
        [SerializeField]
        private string conn_x_Neg = "";
        [SerializeField]
        private string conn_x_Pos = "";
        [SerializeField]
        private string conn_z_Neg = "";
        [SerializeField]
        private string conn_z_Pos = "";

        private Vector3 oldPos = Vector3.zero;

        List<GameObject> toDrawTo = new List<GameObject>();

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            /*if (oldPos != transform.position)
            {
                InputElement[] allEls = GameObject.FindObjectsOfType<InputElement>();

                toDrawTo = new List<GameObject>();

                for (int i = 0; i < allEls.Length; i++)
                {
                    if (allEls[i].transform.GetInstanceID() != transform.GetInstanceID() && Vector3.Distance(allEls[i].transform.position, transform.position) <= drawLinesDistance)
                    {
                        toDrawTo.Add(allEls[i].gameObject);
                    }
                }
            }


            if (Selection.activeGameObject != null && Selection.activeGameObject.transform.GetInstanceID() == transform.GetInstanceID())
            {
                for (int i = 0; i < toDrawTo.Count; i++)
                {
                    Debug.DrawLine(transform.position, toDrawTo[i].transform.position);
                }
            }
            else
            {
                toDrawTo = new List<GameObject>();
            }*/
        }
    }

}