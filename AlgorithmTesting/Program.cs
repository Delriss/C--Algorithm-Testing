using System;
using System.Globalization;

public abstract class Sorting
{
    //Abstract method Sort() to be implemented by the derived classes
    public abstract void Sort();

    //List of numbers to be sorted
    public List<int> numbers { get; set; } = new List<int>();

    //Constructor to create a new list of random numbers
    public Sorting(int max = 25) => numbers = Enumerable.Range(0, max).Select(x => Random.Shared.Next(0, 100)).ToList();

    //Outputs the numbers in the list
    public void OutputNumbers(List<int> _numbers) => Console.WriteLine(string.Join(" ", _numbers));
}

class BubbleSort : Sorting
{
    private void Swap(int index1, int index2) //If num1 is bigger than num2, swap them
    {
        int temp = numbers[index1]; 
        numbers[index1] = numbers[index2];
        numbers[index2] = temp;
    }

    public override void Sort() //Loops over list and swaps if out of order (0n^2)
    {
        for (int i = 0; i < numbers.Count - 1; i++)
        {
            for (int j = 0; j < numbers.Count - 1 - i; j++)
            {
                if (numbers[j] > numbers[j + 1])
                {
                    Swap(j, j + 1);
                }
            }

            OutputNumbers(numbers);
        }
    }
}

class MergeSorter : Sorting
{
    List<int> MergeSort(List<int> list) //"Conquer and Divide" splits into multiple lists and hierarchly merges them in order. (0n log n)
    {
        if (list.Count > 1) 
        {
            List<int> left = list.GetRange(0, list.Count / 2);
            List<int> right = list.GetRange(list.Count / 2, list.Count - list.Count / 2);

            left = MergeSort(left);
            right = MergeSort(right);
            list = Merge(left, right);
        }

        return list;
    }

    List<int> Merge(List<int> left, List<int> right) //Merge lists in order
    {
        List<int> result = new List<int>();

        while (left.Count > 0 && right.Count > 0)
        {
            if (left[0] <= right[0])
            {
                result.Add(left[0]);
                left.RemoveAt(0);
            }
            else
            {
                result.Add(right[0]);
                right.RemoveAt(0);
            }
        }

        result.AddRange(left);
        result.AddRange(right);
        return result;
    }

    public override void Sort()
    {
        numbers = MergeSort(numbers);
        OutputNumbers(numbers);
    }
}

class InsertionSort : Sorting
{
    private void InsertSort(int index) //Inserts a number into the correct position in the list (0n^2)
    {
        int temp = numbers[index];
        int i = index - 1;

        while (i >= 0 && numbers[i] > temp) //Loops list until the index has found its correct position
        {
            numbers[i + 1] = numbers[i];
            i--;
        }

        numbers[i + 1] = temp;
    }

    public override void Sort()
    {
        for (int i = 1; i < numbers.Count; i++)
        {
            InsertSort(i);
            OutputNumbers(numbers);
        }
    }
}

class SelectionSort : Sorting
{
    private void SelectSort(int index) //(0n^2)
    {
        int min = index; //Sets the minimum value to the current index

        for (int i = index + 1; i < numbers.Count; i++)
        {
            if (numbers[i] < numbers[min]) //If current index is less than the set minimum
            {
                min = i; //Set the minimum to the current index
            }
        }

        if (min != index) //If minimum is not the current index
        {
            int temp = numbers[index]; //Sets current index to a temporary variable
            numbers[index] = numbers[min]; //Sets current index to the minimum index
            numbers[min] = temp; //Sets the minimum index to the temporary variable
        }
    }

    public override void Sort()
    {
        for (int i = 0; i < numbers.Count; i++)
        {
            SelectSort(i);
            OutputNumbers(numbers);
        }
    }
}

class QuickSorter : Sorting
{
    //Sorts the list using Quick Sort (0n log n)
    private void QuickSort(int left, int right)
    {
        if (left < right) 
        {
            int pivotPoint = SplitList(left, right);

            if (pivotPoint > 1)
            {
                QuickSort(left, pivotPoint - 1); 
            }

            if (pivotPoint + 1 < right)
            {
                QuickSort(pivotPoint + 1, right);
            }
        }
    }

    //Partitions the list
    private int SplitList(int left, int right)
    {
        int pivotPoint = numbers[left]; //Selects the first element as the pivot point

        for (int i = left + 1; i <= right; i++) 
        {
            if (numbers[i] < pivotPoint) //If current element is less than pivot point
            {
                numbers[left] = numbers[i]; //Move current element to the left of the pivot point
                numbers[i] = numbers[left + 1]; //Move the element to the right of the pivot point to the current element's position
                numbers[left + 1] = pivotPoint; //Move the pivot point to the right of the current element
                left++; 
            }
            OutputNumbers(numbers);
        }

        return left;
    }

    public override void Sort()
    {
        QuickSort(0, numbers.Count - 1);
    }
}

class RadixSorter : Sorting
{
    public static void radixSort(int[] numberArray, int arrLength, int loopCount) //Sorts by the least significant digit (0n)
    {
        //Create numberArray to store output
        int[] output = new int[arrLength]; // output numberArray  
        int i;

        //Stores numCount of each unique number
        int[] numCount = new int[10];
        for (i = 0; i < 10; i++)
        {
            numCount[i] = 0;
        }
        for (i = 0; i < arrLength; i++)
        {
            numCount[(numberArray[i] / loopCount) % 10]++;
        }

        //Changes numCount[i] so that numCount[i] now contains actual position of character in the output numberArray.
        for (i = 1; i < 10; i++)
        {
            numCount[i] += numCount[i - 1];
        }

        //Builds output numberArray
        for (i = arrLength - 1; i >= 0; i--)
        {
            output[numCount[(numberArray[i] / loopCount) % 10] - 1] = numberArray[i]; //Sets the output numberArray to the correct position
            numCount[(numberArray[i] / loopCount) % 10]--; //Decreases the numCount
        }

        //Copy sorted numberArray back into original numberArray
        for (i = 0; i < arrLength; i++)
        {
            numberArray[i] = output[i];
        }
    }

    public override void Sort()
    {
        int[] numberArray = numbers.ToArray();
        int arrLength = numberArray.Length;

        int arrMax = numberArray.Max();
        for (int loopCount = 1; arrMax / loopCount > 0; loopCount *= 10) //Loops through each digit 
        {
            radixSort(numberArray, arrLength, loopCount);
            Console.WriteLine();
            foreach (var num in numberArray) //Prints numberArray
            {
                Console.Write(num + " ");
            }
        }

        numbers = numberArray.ToList();
    }
}

public class Program
{
    private static void Menu()
    {
        Console.WriteLine("SORTING ALGORITHMS");
        Console.WriteLine("------------------\n");
        Console.WriteLine("1. Bubble Sort");
        Console.WriteLine("2. Insertion Sort");
        Console.WriteLine("3. Selection Sort");
        Console.WriteLine("4. Quick Sort");
        Console.WriteLine("5. Radix Sort");
        Console.WriteLine("6. Exit");
    }

    private static void AlgSelector(string option)
    {
        Sorting sort = null;
        string title = "";

        switch (option)
        {
            case "1":
                title = "Bubble Sort";
                sort = new BubbleSort();
                break;
            case "2":
                title = "Insertion Sort";
                sort = new InsertionSort();
                break;
            case "3":
                title = "Selection Sort";
                sort = new SelectionSort();
                break;
            case "4":
                title = "Quick Sort";
                sort = new QuickSorter();
                break;
            case "5":
                title = "Radix Sort";
                sort = new RadixSorter();
                break;
            case "6":
                Environment.Exit(0);
                break;
            default:
                Console.WriteLine("Invalid option");
                break;
        }

        if (sort != null)
        {
            Console.WriteLine("\n" + title + "\n");
            Console.WriteLine("Unsorted List: ");
            sort.OutputNumbers(sort.numbers);
            Console.WriteLine("\nWorking: ");
            sort.Sort();
            Console.WriteLine("\n\nSorted List: ");
            sort.OutputNumbers(sort.numbers);
        }
    }


    public static void Main(string[] args)
    {
        bool loop = true;

        do
        {
            Console.Clear();
            Menu();
            Console.Write("\nSelect an option: ");
            string option = Console.ReadLine();
            AlgSelector(option);

            Console.WriteLine("\nWould you like to retry? (Y/N)");
            string retry = Console.ReadLine();
            loop = retry.ToUpper() == "Y" ? true : false;

        } while (loop);
    }
}