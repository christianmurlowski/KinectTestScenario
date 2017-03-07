using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasManager : MonoBehaviour
{

    public CanvasGroup[] CanvasGroupArray;

    private CanvasGroup _canvasGroupCurrent;
    private CanvasGroup _canvasGroupNext;

    public GameObject GestureManager;

    // Use this for initialization
	void Start ()
	{
	    _canvasGroupCurrent = CanvasGroupArray[0];
	    _canvasGroupNext = CanvasGroupArray[System.Array.IndexOf(CanvasGroupArray, _canvasGroupCurrent) + 1];
	}
	
    public void NextCanvas()
    {
        _canvasGroupCurrent.alpha = 0.0f;
        _canvasGroupCurrent = CanvasGroupArray[System.Array.IndexOf(CanvasGroupArray, _canvasGroupCurrent) + 1];
        _canvasGroupCurrent.alpha = 1.0f;

        //Debug.Log("Canvasname: " + _canvasGroupCurrent.name);
        // If current canvas is Canvas Exercise --> activate GestureManager, else nextchild is next canvas in array
        if (_canvasGroupCurrent.name == "Canvas Exercise")
        {    Debug.Log("If condition");
            // activate GestureManager
            if (!GestureManager.activeSelf)
            {
                GestureManager.SetActive(true);
            }
        }
        else
        {
//            nextChild = canvasChilds[System.Array.IndexOf(canvasChilds, child)+1];
        }
  //      Debug.Log(currentChild + " " + nextChild);
    }
}
