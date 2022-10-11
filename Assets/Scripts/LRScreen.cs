using UnityEngine;

public class LRScreen : MonoBehaviour {
    private Transform mainScreen;
    public int side;
    private void Start() {
        mainScreen = transform.parent.GetChild(0);
        print(mainScreen.name);
        transform.position = new Vector3(this.mainScreen.position.x + Screen.width * side, mainScreen.position.y);
    }
}
