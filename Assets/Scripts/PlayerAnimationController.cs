using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    // MEMBER VARIABLES

    // dialogBox System
    private DialogBoxSystem dialogBoxSystem;

    // inverted shader
    [Range(1, 6)]
    public int buggyAnimationLevel = 1;
    //private Renderer[] glitchEffectRenderer = new Renderer[0];
    [SerializeField]
    Material invertedMaterial;

    float cooldownCounter = 0;

    // Shader parameters in order in string:
    //      Noise Tiling X
    //      Noise Tiling Y
    //      Noise Offset X
    //      Noise Offset Y
    //      Noise Brightness
    //      Noise Contrast
    //      Noise Move Direction X
    //      Noise Move Direction Y
    //      Noise Move Direction Z
    //      Noise Move Direction W
    //      Enable Noise Scale Growth
    //      Noise Grow Frequency
    //      Noise Grow Amount
    private string[] buggyShaderParameters =
    {
        "0.12 0.12 0 0 255 1000 0.1 0.1 0 0 1 20 1", // Level 1
        "0.12 0.12 0 0 255 254 0.1 0.1 0 0 1 20 1", // Level 2
        "0.12 0.12 0 0 255 232 0.1 0.1 0 0 1 20 1", // Level 3
        "0.12 0.12 0 0 255 213 0.1 0.1 0 0 1 20 1", // Level 4
        "0.12 0.12 0 0 255 189 0.1 0.1 0 0 1 20 1", // Level 5
        "0.12 0.12 0 0 255 170 0.1 0.1 0 0 1 20 1", // Level 6
    };

    // Animation controller
    private Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        dialogBoxSystem = GameObject.FindGameObjectWithTag("GameManager").GetComponent<DialogBoxSystem>();
    }

    void Update()
    {
        cooldownCounter += Time.deltaTime;

        if (Input.GetKeyUp(KeyCode.UpArrow) && cooldownCounter > 0.1)
        {
            GoNextBuggyState();
            cooldownCounter = 0;
        }
        else if (Input.GetKeyUp(KeyCode.DownArrow) && cooldownCounter > 0.1)
        {
            GoPrevBuggyState();
            cooldownCounter = 0;
        }
    }

    public void DialogClosed()
    {
        dialogBoxSystem.getDialogBoxGameObject().SetActive(false);
    }

    public void UpdateBuggyState(int level)
    {
        if (level < 1)
            level = 1;
        else if (level > 6)
            level = 6;

        buggyAnimationLevel = level;
        level -= 1;


        string[] temp = buggyShaderParameters[level].Split(' ');
        float[] buggyParams = new float[temp.Length];

        for (int i = 0; i < buggyParams.Length; i++)
        {
            buggyParams[i] = float.Parse(temp[i]);

            Debug.Log(buggyParams[i]);
        }

        // update material's texture tilling/offset
        invertedMaterial.mainTextureScale = new Vector2(buggyParams[0], buggyParams[1]);
        invertedMaterial.mainTextureOffset = new Vector2(buggyParams[2], buggyParams[3]);
        invertedMaterial.SetFloat("_NoiseBrightness", buggyParams[4]);
        invertedMaterial.SetFloat("_NoiseContrast", buggyParams[5]);
        invertedMaterial.SetFloat("_NoiseMoveDirection.x", buggyParams[6]);
        invertedMaterial.SetFloat("_NoiseMoveDirection.y", buggyParams[7]);
        invertedMaterial.SetFloat("_NoiseMoveDirection.z", buggyParams[8]);
        invertedMaterial.SetFloat("_NoiseMoveDirection.w", buggyParams[9]);
        invertedMaterial.SetFloat("_NoiseGrowingEnabled", buggyParams[10]);
        invertedMaterial.SetFloat("_NoiseGrowFrequency", buggyParams[11]);
        invertedMaterial.SetFloat("_NoiseGrowAmount", buggyParams[12]);

        Debug.Log("Current Level: " + (level + 1));
    }

    public void GoNextBuggyState()
    {
        UpdateBuggyState(buggyAnimationLevel + 1);
    }

    public void GoPrevBuggyState()
    {
        UpdateBuggyState(buggyAnimationLevel - 1);
    }
}
