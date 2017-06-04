using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using System.IO;
public class CustomScenarioCreatorInspector : EditorWindow
{
    // TODO: Add saving this map to a scriptible object or somesort of DB so we can use it later.
    // Possible SQL?


    // a reference to our current window.
    public static CustomScenarioCreatorInspector windowInstance;
    // textures to use.
    public Texture2D map;
    public Texture2D pathImage;
    public Texture2D CTSpawnImage;
    public Texture2D TSpawnImage;
    public Texture2D PlantImage;
    // input element size.
    private float elementSize = 15f;
    
    // offsets for the primary map/
    float xImageSize = 600f;
    float yImageSize = 600f;
    float xImageOffset = 10;
    float yImageOffset = 30;

    // a list to store the positions that we make.
    [SerializeField]
    public List<Vector2> m_pathingPositions = new List<Vector2>();
    [SerializeField]
    // a list to store the positions that we make.
    public List<Vector2> m_CTSpawnPositions = new List<Vector2>();
    // a list to store the positions that we make.
    [SerializeField]
    public List<Vector2> m_TSpawnPositions = new List<Vector2>();
    // a list to store the positions that we make.
    [SerializeField]
    public List<Vector2> m_PlantPositions = new List<Vector2>();
    // experimental area
    Vector2 scrollPos = new Vector2();

    string m_assetName = "ExampleAsset.asset";

    // bools 
    // bool to check if we are currently placing nodes
    bool m_posNodePlace = false;
    bool m_posCTSpawn = false;
    bool m_posTSpawn = false;
    bool m_posSite = false;


    [MenuItem("Window/Custom Windows/ScenarioWindow")]
    public static void OpenWindow()
    {
        
        // assign window instance 
        windowInstance = GetWindow<CustomScenarioCreatorInspector>(false, "Scenario Editor");
        windowInstance.position = new Rect(0, 0, 250, 80);
        //windowInstance. = new Vector2(600 + 300, 600 + 200 + 510);
        // focus on this window
        windowInstance.Focus();
        // show this window
        windowInstance.Show();


    }

    private void OnGUI()
    {
        // NOTE: OnGUI draws from top to bottom, bottom of func draws in front.


        // store the current event for event checking
        Event e = Event.current;
        // store the map with the selected texture 2d.
        map = (Texture2D)EditorGUI.ObjectField(new Rect(0, 0, position.width, elementSize),new GUIContent("Map:","Select Map"), map, typeof(Texture2D),true);
      
        if (map)
        {
            float yOffset = 80f;
            Rect Image = new Rect(xImageSize + xImageOffset, yImageOffset, 300, 200);
            Rect r = new Rect(xImageSize + xImageOffset, yImageOffset + yOffset, 300, 510);
            // a space of 20 pixels to seperate the above field
            GUILayout.Space(20);
            // area for all the images
            GUILayout.BeginArea(Image);
            pathImage = (Texture2D)EditorGUI.ObjectField(new Rect   (0, 0, r.width / 1, elementSize), new GUIContent("Node Image", "Node Image draws on all the positions defined in the m_positions arr"), pathImage, typeof(Texture2D), true);
            CTSpawnImage = (Texture2D)EditorGUI.ObjectField(new Rect(0, 20, r.width / 1, elementSize), new GUIContent("CT Spawn Image", "Node Image draws on all the positions defined in the m_positions arr"), CTSpawnImage, typeof(Texture2D), true);
            TSpawnImage = (Texture2D)EditorGUI.ObjectField(new Rect (0, 40, r.width / 1, elementSize), new GUIContent("T Spawn Image", "Node Image draws on all the positions defined in the m_positions arr"), TSpawnImage, typeof(Texture2D), true);
            PlantImage = (Texture2D)EditorGUI.ObjectField(new Rect  (0, 60, r.width / 1, elementSize), new GUIContent("Plant Site Image", "Node Image draws on all the positions defined in the m_positions arr"), PlantImage, typeof(Texture2D), true);
            GUILayout.EndArea();
            // draw the map texture defined by the values on the top of this class, with the map and the scale to stretch and fill with no transperancy.
            GUI.DrawTexture(new Rect(xImageOffset, yImageOffset, xImageSize, yImageSize), map, ScaleMode.StretchToFill, false, 10f);
            // create a new rect to store the utility buttons, etc.
            // begin an area
            GUILayout.BeginArea(r);
            // assign the path image to whatever happens to be inside the object field.
            // end list display
            // check if the button has been pressed.
            // for visual representation of what is activated
            GUIStyle gstyle  = new GUIStyle(GUI.skin.button);
            GUIStyle gstyle1 = new GUIStyle(GUI.skin.button);
            GUIStyle gstyle2 = new GUIStyle(GUI.skin.button);
            GUIStyle gstyle3 = new GUIStyle(GUI.skin.button);

            // allow the button to look "pressed"
            // otherwise lets reset it if it is not enabled.
            if (m_posNodePlace)
                gstyle.normal.background = gstyle.onActive.background;
            else
                gstyle = GUI.skin.button;
            if (m_posCTSpawn)
                gstyle1.normal.background = gstyle1.onActive.background;
            else
                gstyle1 = GUI.skin.button;
            if (m_posTSpawn)
                gstyle2.normal.background = gstyle2.onActive.background;
            else
                gstyle2 = GUI.skin.button;
            if (m_posSite)
                gstyle3.normal.background = gstyle3.onActive.background;
            else
                gstyle3 = GUI.skin.button;


            if (GUILayout.Button(new GUIContent("AI Path Position Node", "Selects the position node to place on the map."), gstyle))
            {
                m_posNodePlace = !m_posNodePlace; // invert the value of what we currently have.
                m_posCTSpawn = false;
                m_posTSpawn = false;
                m_posSite = false;
            }

            if (GUILayout.Button(new GUIContent("CT Spawn Node", "Selects the CT Spawn node to place on the map."), gstyle1))
            {

                m_posNodePlace =false; // invert the value of what we currently have.
                m_posCTSpawn = !m_posCTSpawn;
                m_posTSpawn = false;
                m_posSite = false;
            }

            if (GUILayout.Button(new GUIContent("T Spawn Node", "Selects the T Spawn node to place on the map."), gstyle2))
            {
                m_posNodePlace = false; // invert the value of what we currently have.
                m_posCTSpawn = false;
                m_posTSpawn = !m_posTSpawn;
                m_posSite = false;
            }

            if (GUILayout.Button(new GUIContent("Site Plant Node", "Selects the Site Plant node to place on the map."), gstyle3))
            {
                m_posNodePlace = false; // invert the value of what we currently have.
                m_posCTSpawn = false;
                m_posTSpawn = false;
                m_posSite = !m_posSite;
            }
            GUILayout.Label(new GUIContent("Save/Load", "Save and Load parts of the tool"));
            m_assetName = EditorGUILayout.TextField(new GUIContent("Asset Name Save/Load", "Name of the asset to save and load from."), m_assetName);
            if (GUILayout.Button(new GUIContent("Save", "Selects the Site Plant node to place on the map.")))
            {
                OnSave();
            }

            if (GUILayout.Button(new GUIContent("Load", "Selects the Site Plant node to place on the map.")))
            {
                OnLoad();
            }

            scrollPos = GUILayout.BeginScrollView(scrollPos, false, true);
            DrawArray("m_pathingPositions");
            DrawArray("m_CTSpawnPositions");
            DrawArray("m_TSpawnPositions");
            DrawArray("m_PlantPositions");
            GUILayout.EndScrollView();

            GUILayout.EndArea();
        }

        // we need to create a rect for the image to detect input that would be under it.
        Rect imageRect = new Rect(xImageOffset, yImageOffset, 600, 600);
        GUILayout.BeginArea(imageRect);

        if (Event.current.type == EventType.mouseDown)
        {
            if (m_posNodePlace)
                ModifyArray(m_pathingPositions, e.mousePosition);
            if (m_posCTSpawn)
                ModifyArray(m_CTSpawnPositions, e.mousePosition);
            if (m_posTSpawn)
                ModifyArray(m_TSpawnPositions, e.mousePosition);
            if (m_posSite)
                ModifyArray(m_PlantPositions, e.mousePosition);
            
        }

        DrawArrayLocations(m_pathingPositions,pathImage);
        DrawArrayLocations(m_CTSpawnPositions, CTSpawnImage);
        DrawArrayLocations(m_TSpawnPositions, TSpawnImage);
        DrawArrayLocations(m_PlantPositions, PlantImage);
        // close area.
        GUILayout.EndArea();
    }

    void DrawArrayLocations(List<Vector2> arr, Texture2D img)
    {
        foreach (Vector2 t in arr)
        {
            // draw a rect for the textures
            Rect texRect = new Rect(t.x, t.y, 10, 10);
            // draw the texture using the rect we defined, with the image reference we are using and strech it to fit within the size specified, using transperancy.
            GUI.DrawTexture(new Rect(texRect), img, ScaleMode.StretchToFill, true, 10f);
            // draw a label at the position of the node position, with the node position
            GUI.Label(new Rect(t.x, t.y, 50, 50), new GUIContent((t.x.ToString() + "," + t.y.ToString())));
        }

    }


    void ModifyArray(List<Vector2> arr, Vector2 mousePosition)
    {
        // loop through and check if there is any position under the mouse when clicked
        bool hasfound = false;
        foreach (Vector2 vec in arr)
        {
            // create a new rect for the size of what the node would be.
            Rect texRect = new Rect(vec.x, vec.y, 10, 10);

            // check if the rect contains the mouse pos
            if (texRect.Contains(mousePosition))
            {
                // remove that index
                m_pathingPositions.Remove(vec);
                // assign bool
                hasfound = true;
                // break loop
                break;
            }
        }

        // if the object was not found to be under the mouse click, add the new position to the map
        if (!hasfound)
        {
            Vector2 t = new Vector2(mousePosition.x, mousePosition.y);
            arr.Add(t);
        }


    }

    void DrawArray(string arrToFind)
    {
        // list display
        ScriptableObject target = this;
        // create a new property
        SerializedObject so = new SerializedObject(target);
        // find the property
        SerializedProperty property = so.FindProperty(arrToFind);
        // draw the property of the positions
        EditorGUILayout.PropertyField(property, true);
        // apply the properties
        so.ApplyModifiedProperties();
        // end of list display
    }

    void OnLoad()
    {
        string t = "Assets/SavedAssets/" + m_assetName;
        MapData obj = (MapData)AssetDatabase.LoadAssetAtPath(t,typeof(MapData));
        map = obj.map;
        m_CTSpawnPositions.AddRange(obj.m_CTSpawnPositions);
        m_pathingPositions.AddRange(obj.m_pathingPositions);
        m_PlantPositions.AddRange(obj.m_PlantPositions);
        m_TSpawnPositions.AddRange(obj.m_TSpawnPositions);

    }

    void OnSave()
    {
        MapData obj = ScriptableObject.CreateInstance<MapData>();
        obj.map = map;
        obj.m_CTSpawnPositions = m_CTSpawnPositions.ToArray();
        obj.m_pathingPositions = m_pathingPositions.ToArray();
        obj.m_PlantPositions = m_PlantPositions.ToArray();
        obj.m_TSpawnPositions = m_TSpawnPositions.ToArray();

        string t = "Assets/SavedAssets/" + m_assetName;
        AssetDatabase.CreateAsset(obj, t);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
 
    }
}

