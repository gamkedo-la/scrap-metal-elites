using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class volSmokeAnimator : MonoBehaviour {

    public Texture[] textures;
    public Material smokeMat;
    public int frameFactor = 1;
    int fFCount = 0;
    public int texSwitcher = 0;

   

	void FixedUpdate () {
        fFCount++;
        if (fFCount > frameFactor) {
            fFCount = 0;
        }
        if (fFCount == frameFactor) {
            texSwitcher++;
            if (texSwitcher > (textures.Length - 1)) {
                texSwitcher = 0;
            }
            smokeMat.SetTexture("_MainTex", textures[texSwitcher]);
        }

	}
}
