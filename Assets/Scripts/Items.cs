using UnityEngine;

public class Items
{
    public enum CraftingItem
    {
        // Raw Materials
        Denim,
        Leather,
        Wood,
        Cotton,
        RedDye,
        BlueDye,
        YellowDye,
        
        // Crafted One Layer
        Cloth,
        Button,
        
        // Crafted Two Layers
        RedCloth,
        BlueCloth,
        YellowCloth,
        HoodieString,
        
        // Crafted Three Layers
        Hoodie,
        Sweatpants,
        Jeans,
        Jacket,
        LeatherBag,
        Pyjamas,
        Shirt,
        Shorts,
        Skirt,
    }
    public static GameObject GetItemPrefab(CraftingItem item)
    {
        switch (item)
        {
            case CraftingItem.Denim:
                return Resources.Load<GameObject>("Items/Denim");
            case CraftingItem.Leather:
                return Resources.Load<GameObject>("Items/Leather");
            case CraftingItem.Wood:
                return Resources.Load<GameObject>("Items/Wood");
            case CraftingItem.Cotton:
                return Resources.Load<GameObject>("Items/Cotton");
            case CraftingItem.RedDye:
                return Resources.Load<GameObject>("Items/RedDye");
            case CraftingItem.BlueDye:
                return Resources.Load<GameObject>("Items/BlueDye");
            case CraftingItem.YellowDye:
                return Resources.Load<GameObject>("Items/YellowDye");
            case CraftingItem.Cloth:
                return Resources.Load<GameObject>("Items/Cloth");
            case CraftingItem.Button:
                return Resources.Load<GameObject>("Items/Button");
            case CraftingItem.RedCloth:
                return Resources.Load<GameObject>("Items/RedCloth");
            case CraftingItem.BlueCloth:
                return Resources.Load<GameObject>("Items/BlueCloth");
            case CraftingItem.YellowCloth:
                return Resources.Load<GameObject>("Items/YellowCloth");
            case CraftingItem.HoodieString:
                return Resources.Load<GameObject>("Items/HoodieStrings");
            case CraftingItem.Hoodie:
                return Resources.Load<GameObject>("Items/Hoodie");
            case CraftingItem.Sweatpants:
                return Resources.Load<GameObject>("Items/Sweatpants");
            case CraftingItem.Jeans:
                return Resources.Load<GameObject>("Items/Jeans");
            case CraftingItem.Jacket:
                return Resources.Load<GameObject>("Items/Jacket");
            case CraftingItem.LeatherBag:
                return Resources.Load<GameObject>("Items/LeatherBag");
            case CraftingItem.Pyjamas:
                return Resources.Load<GameObject>("Items/Pyjamas");
            case CraftingItem.Shirt:
                return Resources.Load<GameObject>("Items/Shirt");
            case CraftingItem.Shorts:
                return Resources.Load<GameObject>("Items/Shorts");
            case CraftingItem.Skirt:
                return Resources.Load<GameObject>("Items/Skirt");
            default:
                return null;
        }
    }
}