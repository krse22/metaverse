using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomManager 
{
    private List<int> uniqueRandoms = new List<int>();
    private int memoryCount = 0;
    private int max = 0;
    private int currentIndex = 0;

    public void Init(int memCount, int maxValue)
    {
        memoryCount = memCount;
        max = maxValue;
        currentIndex = 0;
        uniqueRandoms.Clear();

        for (int i = 0; i < memoryCount; i++)
        {
            uniqueRandoms.Add(GetRandomWithoutRepeating());
        }
    }

    int GetRandomWithoutRepeating()
    {
        if (memoryCount > max)
        {
            return Random.Range(0, max);
        }
        int random = Random.Range(0, max);
        while (uniqueRandoms.Contains(random))
        {
            random = Random.Range(0, max);
        }
        return random;
    }

    public int GetUniqueRandom()
    {
        int random = GetRandomWithoutRepeating();
        uniqueRandoms[currentIndex] = random;
        currentIndex++;
        if (currentIndex == memoryCount - 1)
        {
            currentIndex = 0;
        }
        return random;
    }

}
