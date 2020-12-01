using System.Collections.Generic;
public class Items
{
    public int id;
    public int num;
}
public class BagMrg : BaseManager<BagMrg>
{

    public List<Items> itemList = new List<Items>();

    public void InitItemsInfo()
    {
        for (int i = 0; i < 10000; i++)
        {
            Items item=new Items();
            item.id = i;
            item.num = i;
            
            itemList.Add(item);
        }
    }
}
