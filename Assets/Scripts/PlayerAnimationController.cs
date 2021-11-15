using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    // MEMBER VARIABLES

    // dialogBox System
    private DialogBoxSystem dialogBoxSystem;

    // inverted shader
    [Range(1, 5)]
    public int animationState = 1;
    [SerializeField]
    private Renderer[] glitchEffectRenderer = new Renderer[0];
    Material invertedMaterial;

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
        "0.12 0.12 0 0 255 254 0.1 0.1 0 0 1 20 1", // Level 1
        "0.12 0.12 0 0 255 232 0.1 0.1 0 0 1 20 1", // Level 2
        "0.12 0.12 0 0 255 213 0.1 0.1 0 0 1 20 1", // Level 3
        "0.12 0.12 0 0 255 189 0.1 0.1 0 0 1 20 1", // Level 4
        "0.12 0.12 0 0 255 170 0.1 0.1 0 0 1 20 1", // Level 5
    };

    // Animation controller
    private Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        dialogBoxSystem = GameObject.FindGameObjectWithTag("GameManager").GetComponent<DialogBoxSystem>();
        invertedMaterial = GetComponent<Material>();
    }

    public void DialogClosed()
    {
        dialogBoxSystem.getDialogBoxGameObject().SetActive(false);

    }

    public void UpdateBuggyState(int level)
    {
        if (level < 0)
            level = 0;
        else if (level > 5)
            level = 5;

        string[] temp = buggyShaderParameters[level - 1].Split(' ');
        float[] buggyParams = new float[temp.Length];

        for (int i = 0; i < buggyParams.Length; i++)
        {
            buggyParams[i] = float.Parse(temp[i]);

            Debug.Log(buggyParams[i]);
        }

        // update material's texture tilling/offset
        foreach (Renderer renderer in glitchEffectRenderer)
        {
            renderer.material.mainTextureScale = new Vector2(buggyParams[0], buggyParams[1]);
            renderer.material.mainTextureOffset = new Vector2(buggyParams[2], buggyParams[3]);
            renderer.material.SetFloat("_NoiseBrightness", buggyParams[4]);
            renderer.material.SetFloat("_NoiseContrast", buggyParams[5]);
            renderer.material.SetFloat("_NoiseMoveDirection.x", buggyParams[6]);
            renderer.material.SetFloat("_NoiseMoveDirection.y", buggyParams[7]);
            renderer.material.SetFloat("_NoiseMoveDirection.z", buggyParams[8]);
            renderer.material.SetFloat("_NoiseMoveDirection.w", buggyParams[9]);
            renderer.material.SetFloat("_NoiseGrowingEnabled", buggyParams[10]);
            renderer.material.SetFloat("_NoiseGrowFrequency", buggyParams[11]);
            renderer.material.SetFloat("_NoiseGrowAmount", buggyParams[12]);
        }
    }

    public void GoNextBuggyState(int level)
    {
        UpdateBuggyState(level + 1);
    }

    public void GoPrevBuggyState(int level)
    {
        UpdateBuggyState(level - 1);
    }
}
