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

        // Crafted One Layer
        Cloth,
        Button,
        
        // Crafted Two Layers
        RedCloth,
        BlueCloth,
        YellowCloth,
        GreyCloth,
        HoodieString,
        LeatherWallet,
        
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

    public static Sprite GetItemIcon(CraftingItem item)
    {
        // get the sprite from the Resources/Icons folder based on the icon passed in
        // the suffix to the icon is "Icon"
        
        switch (item)
        {
            case CraftingItem.Denim:
                return Resources.Load<Sprite>("Icons/Outlines/DenimIcon");
            case CraftingItem.Leather:
                return Resources.Load<Sprite>("Icons/Outlines/LeatherIcon");
            case CraftingItem.Wood:
                return Resources.Load<Sprite>("Icons/Outlines/LogIcon");
            case CraftingItem.Cotton:
                return Resources.Load<Sprite>("Icons/Outlines/CottonIcon");
            case CraftingItem.Cloth:
                return Resources.Load<Sprite>("Icons/Outlines/ClothIcon");
            case CraftingItem.Button:
                return Resources.Load<Sprite>("Icons/Outlines/ButtonIcon");
            case CraftingItem.RedCloth:
                return Resources.Load<Sprite>("Icons/Outlines/RedClothIcon");
            case CraftingItem.BlueCloth:
                return Resources.Load<Sprite>("Icons/Outlines/BlueClothIcon");
            case CraftingItem.YellowCloth:
                return Resources.Load<Sprite>("Icons/Outlines/YellowClothIcon");
            case CraftingItem.GreyCloth:
                return Resources.Load<Sprite>("Icons/Outlines/GreyClothIcon");
            case CraftingItem.HoodieString:
                return Resources.Load<Sprite>("Icons/Outlines/HoodieStrings");
            case CraftingItem.Hoodie:
                return Resources.Load<Sprite>("Icons/Outlines/HoodieIcon");
            case CraftingItem.Sweatpants:
                return Resources.Load<Sprite>("Icons/Outlines/SweatpantsIcon");
            case CraftingItem.Jeans:
                return Resources.Load<Sprite>("Icons/Outlines/JeansIcon");
            case CraftingItem.Jacket:
                return Resources.Load<Sprite>("Icons/Outlines/JacketIcon");
            case CraftingItem.LeatherBag:
                return Resources.Load<Sprite>("Icons/Outlines/LeatherBagIcon");
            case CraftingItem.Pyjamas:
                return Resources.Load<Sprite>("Icons/Outlines/PyjamasIcon");
            case CraftingItem.Shirt:
                return Resources.Load<Sprite>("Icons/Outlines/ShirtIcon");
            case CraftingItem.Shorts:
                return Resources.Load<Sprite>("Icons/Outlines/ShortsIcon");
            case CraftingItem.Skirt:
                return Resources.Load<Sprite>("Icons/Outlines/SkirtIcon");
            case CraftingItem.LeatherWallet:
                return Resources.Load<Sprite>("Icons/Outlines/WalletIcon");
            default:
                return null;
        }
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
            case CraftingItem.GreyCloth:
                return Resources.Load<GameObject>("Items/GreyCloth");
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
            case CraftingItem.LeatherWallet:
                return Resources.Load<GameObject>("Items/Wallet");
            default:
                return null;
        }
    }
}