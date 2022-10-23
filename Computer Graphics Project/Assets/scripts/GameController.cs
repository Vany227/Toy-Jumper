using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    
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
        Vector3 Startposition = grid.CellToWorld(startingPOS);
        Startposition.Set(Startposition.x + 16, Startposition.y + 16, -30);
        Debug.Log(Startposition);
        Camera.position = Startposition;

        Transform currentScreen = screens[0];
        Startposition.Set(Startposition.x - 16, Startposition.y - 16, currentScreen.position.z);
        currentScreen.position = Startposition;

        
        /*
        switch (m_type){
            case Type.TWOXTWO:

                break;
        }
        */
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
