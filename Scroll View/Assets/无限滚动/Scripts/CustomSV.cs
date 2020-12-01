using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 该接口  作为格子对象必须继承的类 它用于实现初始化格子的方法
/// </summary>
/// <typeparam name="T"></typeparam>
public interface IItemBase<T>
{
    void InitInfo(T info);
}
/// <summary>
/// 自定义sv类 通过缓存池创建复用对象
/// </summary>
/// <typeparam name="T">代表 数据来源类</typeparam>
/// <typeparam name="M">代表 格子类</typeparam>
public class CustomSV<T,K>where K:IItemBase<T>
{
    private RectTransform content;

    private int viewPortH;
    //当前要显示的物品
    private Dictionary<int, GameObject> nowShowItems = new Dictionary<int, GameObject>();

    //数据来源  外界传入
    // ===========private List<Items> items;
    private List<T> items;
    
    private int oldMinIndex = -1;
    private int oldMaxIndex = -1;

    //资源路径
    private string itemsResName;
    
    //格子的间隔宽高
    private int itemW;
    private int itemH;
    
    //格子的列数
    private int col;
    public void InitContent(RectTransform trans,int h)
    {
        this.content = trans;
        this.viewPortH = h;
    }

    public void InitItemResName(string name)
    {
        this.itemsResName = name;
    }

    /// <summary>
    /// 初始化数据来源
    /// </summary>
    public void InitInfos(List<T> itemes)
    {
        this.items = itemes;
        content.sizeDelta=new Vector2(0,Mathf.CeilToInt(BagMrg.GetInstance().itemList.Count/col)*itemH);
    }

    public void InitItemSizeAndCol(int w, int h, int col)
    {
        this.itemW = w;
        this.itemH = h;
        this.col = col;
    }
    /// <summary>
    /// 更新格子
    /// </summary>
    public void CheckShowOrHide()
    {
        /*
         假设一个各组的大小为80*80
         90/80=1*3=3
         使用可视范围的起始位置Y/一个格子的高=》
         其实现实的是哪一行*哪一行有多少格子=起始位置显示的索引值
         330/80=4*3=12+2=14
         
         使用可视范围的结束位置Y/一个格子的高度=》
         结束位置时哪一行+（一行格子数-1）=结束位置显示的索引值
         */
        //检测哪些格子应该显示出来
        int minIndex=(int)(content.anchoredPosition.y/itemH)*col;
        int maxIndex=(int)((content.anchoredPosition.y+viewPortH)/itemH)*col+col-1;
        
        //最小值判断
        if (minIndex<0)
        {
            minIndex = 0;
        }
        //超出最大数量
        if (maxIndex>=BagMrg.GetInstance().itemList.Count)
        {
            maxIndex = BagMrg.GetInstance().itemList.Count - 1;
        }
        //不等于之后在更新
        if (minIndex!=oldMaxIndex||maxIndex!=oldMaxIndex)
        {
            for (int i = oldMinIndex; i < minIndex; ++i)
            {
                if (nowShowItems.ContainsKey(i))
                {
                    if (nowShowItems[i]!=null)
                    {
                        PoolMgr.GetInstance().PushObj("UI/BagItem",nowShowItems[i]);
                    }
                    nowShowItems.Remove(i);
                }
            }

            for (int i = maxIndex; i <= oldMaxIndex; ++i)
            {
                if (nowShowItems.ContainsKey(i))
                {
                    if (nowShowItems[i]!=null)
                    {
                        PoolMgr.GetInstance().PushObj(itemsResName,nowShowItems[i]);
                    }
                    nowShowItems.Remove(i);
                }
            }

        }

      

      
        //根据上一次索引和这一次新算出来的索引  来判断 哪些该移除
        oldMaxIndex = maxIndex;
        oldMinIndex = minIndex;
        
        //创建指定索引范围值格子
        for (int i = minIndex; i <= maxIndex; ++i)
        {
            if (nowShowItems.ContainsKey(i))
                continue;
            else
            {
                int index = i;
                nowShowItems.Add(index,null);
                PoolMgr.GetInstance().GetObj(itemsResName, (obj) =>
                {
                    //当格子创建出来需要做什么
                    //设置父对象
                    obj.transform.parent = content.transform;
                    //重置相对大小
                    obj.transform.localScale = Vector3.one;
                    //充值位置
                    // obj.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
                    // obj.transform.localPosition=Vector3.zero;
                    // Debug.Log(transform.localPosition.x+"|"+transform.localPosition.y);
                    //Debug.Log((index%3)*240+"|"+-index/3*190);
                    // obj.GetComponent<RectTransform>().anchoredPosition = new Vector3((index%3)*240,-index/3*190,0);
                    obj.transform.localPosition=new Vector3((index%col)*itemW,-index/col*itemH,0);
                    //充值格子信息
                    /////-------obj.GetComponent<BagItem>().InitItemsInfo(BagMrg.GetInstance().itemList[index]);
                    obj.GetComponent<K>().InitInfo(items[index]);
                
                    //先判断有没有这个坑
                    if (nowShowItems.ContainsKey(index))
                    {
                        nowShowItems[index] = obj;
                    }
                    else
                    {
                        PoolMgr.GetInstance().PushObj(itemsResName,obj);
                    }
                });
            } 
          
        }
        
    }
}
