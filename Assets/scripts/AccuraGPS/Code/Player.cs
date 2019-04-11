using UnityEngine;

public class Player : MonoBehaviour
{
	public Material material;
    public int uvTieX = 1;
    public int uvTieY = 1;
    public int fps = 10;

    private Vector2 size;   
	private float iX = 0;
    private float iY = 1;
	private int lastIndex = -1;

    void Start()
    {
        size = new Vector2(1.0f / uvTieX, 1.0f / uvTieY);      
		material.SetTextureScale("_MainTex", size);
    }
   
    void Update()
    {
        int index = (int)(Time.timeSinceLevelLoad * fps) % (uvTieX * uvTieY);

        if (index != lastIndex)
        {
            Vector2 offset = new Vector2(iX * size.x, 1 - (size.y * iY));
            iX++;
			if (Mathf.Approximately(iX / uvTieX, 1))
            {
                if (uvTieY != 1) iY++;
                iX = 0;
				if (Mathf.Approximately(iY / uvTieY, 1))
                {
                    iY = 1;
                }
            }         
			material.SetTextureOffset("_MainTex", offset);         
            lastIndex = index;
        }
    }
}