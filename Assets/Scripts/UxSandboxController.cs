using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UxSandboxController : MonoBehaviour {
    public RectTransform externalPanel;

    public MaterialTag botMaterial;
    public GameObject cameraBoom;
    public Camera sandboxCamera;
    public float rotateSpeed = 8f;

    private GameObject spawnedBot;
    private bool runCameraRotation = false;

    public void Start() {
        // link external panel to camera through the use of a rendertexture
        if (externalPanel != null) {
            // destroy any current image linked to the panel
            var image = externalPanel.GetComponent<Image>();
            if (image != null) {
                DestroyImmediate(image);
            }

            // create rendertexture, matching size of panel
            var renderTexture = new RenderTexture((int) externalPanel.rect.width, (int) externalPanel.rect.height, 24);

            // add new raw image
            var rawImage = externalPanel.gameObject.AddComponent<RawImage>();
            rawImage.texture = renderTexture;

            // set camera output texture
            sandboxCamera.targetTexture = renderTexture;
        }

        // start camera rotation
        runCameraRotation = true;
        StartCoroutine(CameraRotate());
    }

    IEnumerator CameraRotate() {
        while (runCameraRotation) {
            // rotate camera via boom
            float h = rotateSpeed * Time.deltaTime;
            cameraBoom.transform.eulerAngles = new Vector3(cameraBoom.transform.eulerAngles.x, cameraBoom.transform.eulerAngles.y + h, cameraBoom.transform.eulerAngles.z);
            yield return null;
        }
    }

    public void Clear() {
        if (spawnedBot != null) {
            // clean up child parts
            var childLink = spawnedBot.GetComponent<ChildLink>();
            if (childLink != null && childLink.childGo) {
                Object.Destroy(childLink.childGo);
            }
            Object.Destroy(spawnedBot);
        }
    }

    public void ShowBot(
        GameObject botPrefab
    ) {
        // clear any previous state
        Clear();

        // spawn the bot (at current sandbox location)
        spawnedBot = Object.Instantiate(botPrefab, transform.position, transform.rotation);

        // set layer for all child parts
        var layer = LayerMask.NameToLayer("sandbox");
        var transforms = PartUtil.GetComponentsInChildren<Transform>(spawnedBot);
        for (var i=0; i<transforms.Length; i++) {
            transforms[i].gameObject.layer = layer;
        }

        var materialDistributor = spawnedBot.GetComponent<MaterialDistributor>();
        if (materialDistributor != null) {
            materialDistributor.materialTag = botMaterial;
        }

        // attach the AI controller
        var ai = spawnedBot.AddComponent<OnOffAIController>();
        ai.randomize = true;
        ai.delay = 2f;

    }

}
