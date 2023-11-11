using UnityEngine;

public class BoxManager : MonoBehaviour
{
    public BoxSelector currentlySelectedBox;

    public void RegisterSelection(BoxSelector selectedBox)
   {
        if (currentlySelectedBox && currentlySelectedBox != selectedBox)
       {
            currentlySelectedBox.Deselect();
        }
        currentlySelectedBox = selectedBox;
    }
}
