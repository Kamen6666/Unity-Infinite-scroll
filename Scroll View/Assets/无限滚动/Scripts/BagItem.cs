using UnityEngine.UI;

public class BagItem : BasePanel,IItemBase<Items>
{
    /// <summary>
    /// 初始化格子信息
    /// </summary>
    /// <param name="item"></param>
    /*public void InitItemsInfo(Items item)
    {
        //先读取物品表
        //根据表中信息  来更新信息
        //更新图标
        //更新名字
        //更新道具信息
        GetControl<Text>("Text").text = item.num.ToString();
    }*/

    public void InitInfo(Items info)
    {
        GetControl<Text>("Text").text = info.num.ToString();
    }
}
