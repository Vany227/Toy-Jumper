using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class CameraControl : MonoBehaviour
{
    private int gridPosX;
    private int gridPosY;
    private Camera cam;

    public Grid grid;
    public Transform Character;
    public Transform screen;
    // Start is called before the first frame update
    void Start()
    {
        gridPosX = 0;
        gridPosY = 0;
        cam = GetComponent<Camera>();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateScreen()
    {

        Transform currentScreen = Character.GetComponent<Character_Controller>().currentScreen;
        Vector3 newPos = grid.CellToWorld(new Vector3Int(currentScreen.GetComponent<ScreenController>().GridX, currentScreen.GetComponent<ScreenController>().GridY, -30));
        newPos.Set(newPos.x + 16, newPos.y + 16, -30);
        this.transform.position = newPos;
        
    }
}
