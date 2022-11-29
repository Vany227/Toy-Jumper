using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.VisualScripting;

public class GameController : MonoBehaviour
{
    private Vector3 Startposition;
    private Vector3Int startingPOS = new Vector3Int(0, 0, 0);
    private Grid grid;
    private Mode mode;
    public enum Type { TWOXONE, TWOXTWO, THREEXTHREE };
    public enum Mode { TWOD, THREED};
    public List<Transform> screens = new List<Transform>();
    public Type m_type;
    public Transform Camera;
    // Start is called before the first frame update
    void Start()
    {
        mode = Mode.TWOD;
        grid = this.GetComponent<Grid>();
        

        
        
        switch (m_type){
            case Type.TWOXTWO:
                for (int i = 0; i < screens.Count; i++)
                {
                    Transform currentScreen = screens[i];
                    Startposition = grid.CellToWorld(new Vector3Int(currentScreen.GetComponent<ScreenController>().GridX, currentScreen.GetComponent<ScreenController>().GridY, -30));
                    Startposition.Set(Startposition.x, Startposition.y, currentScreen.position.z);
                    currentScreen.position = Startposition;

                    if (currentScreen.GetComponent<ScreenController>().startingScreen)
                    {
                        Startposition.Set(Startposition.x + 16, Startposition.y + 16, -30);
                        Camera.position = Startposition;
                    }
                }
                break;
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
