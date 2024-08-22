using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Utils
{
    public static class ListUtil
    {
        public static List<T> GetUniqueElementsList<T>(List<T> list, int count) 
        {
            if (list.Count <= count)
                return list;

            List<T> tempList = new List<T>();
            do
            {
                int index = Random.Range(0, list.Count);
                if (tempList.Contains(list[index]) == false)
                    tempList.Add(list[index]);
            } while (tempList.Count != count);

            return tempList;
        }
    }
}
