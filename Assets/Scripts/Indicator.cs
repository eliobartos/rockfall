using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Indicator : MonoBehaviour
{
    // The object we are tracking
    public Transform target;

    // Measure the distance from 'target' to this transform
    public Transform showDistanceTo;

    // The label that shows the distance we're measuring
    public Text distanceLabel;

    // How far we should be from the screen edges
    public float margin = 100.0f;

    // Sprite that is drawn when object is not in view
    public Sprite outsideOfViewSprite;

    // Sprite set up by indicator manager when object is in view
    public Sprite insideViewSprite;

    private Image image;

    // Our image's tint color
    public Color color {
        set {
            GetComponent<Image>().color = value;
        }
        get {
            return GetComponent<Image>().color;
        }
    }

    void Start() {

        // Scale the margin according to screen width
        margin *= Screen.width / 1920.0f;

        // Get the image once and for all
        image = GetComponent<Image>();

        // Get currently set inside of view sprite
        insideViewSprite = image.sprite;

        // Hide the label, it will be re-enabled in Update
        // if we have a target
        distanceLabel.enabled = false;

        // On start, wait a frame before appearing to prevent visual glitches
        image.enabled = false;
    }

    void LateUpdate() {

        // Is our target gone? Then we should go too.
        if (target == null) {
            Destroy(gameObject);
            return;
        }

        // If we have a target for calculating distance, then calculate it and display it in the distanceLabel
        if (showDistanceTo != null) {
            
            // Show the label
            distanceLabel.enabled = true;

            // Calculate the distance
            var distance = (int)Vector3.Magnitude(showDistanceTo.position - target.position);

            // Show distance in the label
            distanceLabel.text = distance.ToString() + "m";
        } else {
            
            // Don't show the label
            distanceLabel.enabled = false;
        }

        GetComponent<Image>().enabled = true;

        // Work out where in screen space the object is
        var viewportPoint = Camera.main.WorldToViewportPoint(target.position);

        //Debug.Log("Target World position: " + target.position);
        //Debug.Log("View Port Point: " + viewportPoint);

        // Is the point behind us?
        if( viewportPoint.z < 0) {
            // Push it to the edges of the screen
            viewportPoint.z = 0;
            viewportPoint.y = 1 - viewportPoint.y;
            
            // Handle x coordinate if behind us
            viewportPoint.x = 1 - viewportPoint.x;
            if(viewportPoint.x > 0.5) {
                viewportPoint.x = Mathf.Infinity;
            } else {
                viewportPoint.x = -Mathf.Infinity;
            }

        }

        // Work out where in view-space we shoud be
        var screenPoint = Camera.main.ViewportToScreenPoint(viewportPoint);
        Vector2 newScreenPoint = new Vector2();

        //Debug.Log("Screen Point: " + screenPoint);
        
        newScreenPoint.x = Mathf.Clamp(screenPoint.x, 2*margin, Screen.width - margin * 2);
        newScreenPoint.y = Mathf.Clamp(screenPoint.y, 2*margin, Screen.height - margin * 2);
        
        // Change the sprite if the indicator is at the edge of the screen (object is not visible)
        if(newScreenPoint.x != screenPoint.x || newScreenPoint.y != screenPoint.y) {

            if(image.sprite != outsideOfViewSprite) {
                image.sprite = outsideOfViewSprite;
                image.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
            }
            
        } else {
            if(image.sprite != insideViewSprite) {
                image.sprite = insideViewSprite;
                image.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
            }
            
        }


        //Debug.Log("Screen Point Clamped: " + newScreenPoint);      
        
        // Work out wher ein the canvas-space the view-space coordinate is
        var localPosition = new Vector2();
        
        // RectTransformUtility.ScreenPointToLocalPointInRectangle(
        //     transform.parent.GetComponent<RectTransform>(),
        //     screenPoint,
        //     Camera.main,
        //     out localPosition);

        localPosition.x = -512.0f + (newScreenPoint.x - margin) * (512.0f * 2) / (Screen.width - 2 * margin);
        localPosition.y = -288.0f + (newScreenPoint.y - margin) * (288.0f * 2) / (Screen.height - 2 * margin);

        //Debug.Log("Local Position" + localPosition);

        // Update our position
        var rectTransform = GetComponent<RectTransform>();
        rectTransform.localPosition = localPosition;
    }

}
