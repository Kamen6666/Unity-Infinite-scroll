using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
public class BagPanel : BasePanel
{
    public RectTransform content;

    public int viewPortH;
    //当前要显示的物品
    private Dictionary<int, GameObject> nowShowItems = new Dictionary<int, GameObject>();

    private int oldMinIndex = -1;
    private int oldMaxIndex = -1;

    private CustomSV<Items,BagItem> sv;
    void Start()
    {
        sv = new CustomSV<Items, BagItem>();
        sv.InitItemResName("UI/BagItem");
        sv.InitItemSizeAndCol(200,200,2);
        sv.InitContent(content,viewPortH);
        sv.InitInfos(BagMrg.GetInstance().itemList);
    }
    public void Update()
    {
        sv.CheckShowOrHide();
    }

   
}
