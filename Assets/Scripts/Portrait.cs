using System;
using System.IO;
using UnityEngine;

public class Portrait : MonoBehaviour {
    private GameManager _gm;
    private void Start() {
        _gm = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    public void PickImage() {
        var portrait = transform.parent.GetChild(1).GetComponent<UnityEngine.UI.Image>();
        Debug.Log("working...");
        if (!NativeGallery.IsMediaPickerBusy()) {
            var permission = NativeGallery.GetImageFromGallery(  path  =>
            {
                Debug.Log( "Image path: " + path );
                if( path != null ) {
                    Texture2D texture = new Texture2D( 2, 2 );
                    texture.LoadImage( File.ReadAllBytes( path ) );
                    var smallestDim = texture.width < texture.height ? texture.width : texture.height;
                    portrait.sprite = Sprite.Create(texture, 
                        new Rect(0,texture.height - smallestDim, smallestDim, smallestDim), new Vector2(0.5f, 0.5f));
                    _gm.currentCharacter.portraitPath = path;
                }
            }, "Select Image Viewer");
            
            Debug.Log( "Permission result: " + permission );
        }
    }

}
