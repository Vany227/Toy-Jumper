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
            }
            else //if in 2d, switch back to 3d
            {
                cam.switchTo3d();
            }

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
