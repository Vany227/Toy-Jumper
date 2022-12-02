using System;
using UnityEngine;

//singleton class 
public class GameController : MonoBehaviour
{
    private static GameController _instance;
    public static GameController Instance { get { return _instance; } }

    public bool in3dState;

    [SerializeField]
    PlatformPuzzle puzzle;
    [SerializeField]
    CameraController cam;

    Rigidbody2D[] twoDimensionalObjs;
    [SerializeField]
    Character_Controller player;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    private void Start()
    {
        in3dState = true;
        twoDimensionalObjs = FindObjectsOfType<Rigidbody2D>();
        turnOff2dPhysics();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (in3dState && PuzzleCube.selectedCube != null) //if in 3d, switch to 2d if theres a selected  cube
            {
                in3dState = false;
                PuzzleCube.canClick = false;
                puzzle.unhighlightPuzzle();
                cam.switchTo2d();
                puzzle.StartCoroutine(puzzle.rotateIt());
            }
            else //if in 2d, switch back to 3d
            {
                turnOff2dPhysics();
                cam.switchTo3d();    
            }
        }
        if (Input.GetKeyDown(KeyCode.J)) turnOn2dPhysics();
    }

    public void turnOn2dPhysics()
    {
        foreach (Rigidbody2D obj in twoDimensionalObjs)
        {
            obj.bodyType = RigidbodyType2D.Static;
            obj.simulated = true;
        }
        player.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic; //player needs to be dynamic
    }
    
    public void turnOff2dPhysics()
    {
        foreach (Rigidbody2D obj in twoDimensionalObjs)
        {
            obj.simulated = false;
            obj.isKinematic = true;
        }
    }



    /*
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
                        Startposition.Set(Startposition.x, Startposition.y, 0);
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
    */
}
