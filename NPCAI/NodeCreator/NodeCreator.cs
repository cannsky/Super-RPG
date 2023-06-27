using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;

[ExecuteInEditMode]
public class NodeCreator : MonoBehaviour
{
    public bool openPathMode = false;
    public GameObject pathToEdit;
    
    private void OnEnable(){
        if(!Application.isEditor){
            Destroy(this);
        }
        SceneView.onSceneGUIDelegate += OnScene;
    }
    
    void OnScene(SceneView scene){
        if(!openPathMode) return;
        Event e = Event.current;
        if(e.type == EventType.MouseDown && e.button == 2){
            Vector3 mousePosition = e.mousePosition;
            float pixelsPerPoint = EditorGUIUtility.pixelsPerPoint;
            mousePosition.y = scene.camera.pixelHeight - mousePosition.y * pixelsPerPoint;
            mousePosition.x *= pixelsPerPoint;
            Ray ray = scene.camera.ScreenPointToRay(mousePosition);
            RaycastHit hit;
            if(Physics.Raycast(ray, out hit)){
                if(pathToEdit == null) CreateNewPath(hit);
                CreateNewNode(hit);
            }
            e.Use();
        }
    }
    
    private void CreateNewPath(RaycastHit hit){
        GameObject path = new GameObject();
        path.AddComponent<Path>();
        path.name = "New Path";
        path.transform.position = hit.point;
        this.pathToEdit = path;
    }
    
    private void CreateNewNode(RaycastHit hit){
        GameObject obj = new GameObject();
        obj.transform.parent = pathToEdit.transform;
        obj.name = "Node (" + (pathToEdit.GetComponentsInChildren<Transform>().Length - 1).ToString() + ")";
        obj.transform.position = hit.point;
    }
}
#endif