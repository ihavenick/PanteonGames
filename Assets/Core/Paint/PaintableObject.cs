using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaintableObject : MonoBehaviour
{
    public GameObject Brush;
    public float BrushSize = 0.1f;
    [SerializeField]
    private Camera _camera;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            var ray = _camera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                var go = Instantiate(Brush, hit.point + Vector3.forward * 0.1f, new Quaternion(-1.062f, -13.143f , -0.007f, 0), transform);
                go.transform.localScale = Vector3.one * BrushSize;
            }
        }
    }
}
