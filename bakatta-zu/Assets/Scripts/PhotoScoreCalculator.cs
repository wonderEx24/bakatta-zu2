using UnityEngine;

public class PhotoScoreCalculator : MonoBehaviour
{
    [SerializeField] Camera photoCamera;

    public int CalculateScore()
    {
        int score = 0;
        Plane[] planes = GeometryUtility.CalculateFrustumPlanes(photoCamera);

        // 枯れ木：3000000点
        foreach (GameObject tree in GameObject.FindGameObjectsWithTag("DeadTree"))
        {
            if (IsVisible(tree, planes))
            {
                score += 3_000_000;
            }
        }

        // 黄色い水：600000点
        foreach (GameObject water in GameObject.FindGameObjectsWithTag("YellowWater"))
        {
            if (IsVisible(water, planes))
            {
                score += 600_000;
            }
        }

        return score;
    }

    bool IsVisible(GameObject obj, Plane[] planes)
    {
        Renderer r = obj.GetComponent<Renderer>();
        if (r == null) return false;

        return GeometryUtility.TestPlanesAABB(planes, r.bounds);
    }
}
