using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class TreeManager : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> treePrefabs;
    [SerializeField]
    private MapManager mapManager;
    private List<TreeNode> trees;

    void Awake()
    {
        Assert.IsNotNull(treePrefabs);
        Assert.IsNotNull(mapManager);

        trees = new List<TreeNode>();
    }

    public void CreateTree(TreeNode tree)
    {
        trees.Add(tree);
        Vector3 position = mapManager.GetUnityPositionFromCoordinatesAndAltitude(tree.LatLong, tree.Altitude, true);

        System.Random random = new System.Random();
        GameObject treeObject = Instantiate(treePrefabs[random.Next(0, treePrefabs.Count)], position, Quaternion.identity, transform);
        treeObject.transform.localScale *= mapManager.GetWorldRelativeScale();
        tree.Tree = treeObject;
    }

    public void ClearTrees()
    {
        foreach (TreeNode tree in trees) {
            GameObject.Destroy(tree.Tree);
        }
        trees.Clear();
    }

    public void Show(bool show)
    {
        gameObject.SetActive(show);
    }

    public void UpdateTreesPosition()
    {
        foreach (TreeNode tree in trees) {
            tree.Tree.transform.position = mapManager.GetUnityPositionFromCoordinatesAndAltitude(tree.LatLong, tree.Altitude, true);
        }
    }

    public List<Feature> GetFeatures()
    {
        List<Feature> features = new List<Feature>();

        /*
        foreach (TreeNode tree in trees) {
            Feature feature = new Feature();
            feature.Properties.Add("type", "tree");

            feature.Coordinates = new Vector3d(tree.LatLong, tree.Altitude);
            features.Add(feature);
        }
        */

        foreach (Transform tree in transform) {
            Feature feature = new Feature();
            feature.Properties.Add("type", "tree");

            feature.Coordinates = new Vector3d(mapManager.GetCoordinatesFromUnityPosition(tree.position), mapManager.GetAltitudeFromUnityPosition(tree.position));
            features.Add(feature);
        }

        return features;
    }
}
