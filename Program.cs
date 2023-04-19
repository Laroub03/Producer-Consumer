using System;
using System.Collections.Generic;
using System.Threading;

public class Program
{
    // Create a queue to hold items produced by the Producer
    Queue<int> _itemBox = new Queue<int>();

    public static void Main(string[] args)
    {
        Program program = new Program();

        // Create a producer and a consumer thread
        Thread producerThread = new Thread(new ThreadStart(program.Producer));
        Thread consumerThread = new Thread(new ThreadStart(program.Consumer));

        producerThread.Start();
        consumerThread.Start();

        producerThread.Join();
        consumerThread.Join();
    }

    // A method to produce items
    public void Producer()
    {
        while (true)
        {
            Monitor.Enter(_itemBox);
            try
            {
                // If the _itemBox is empty, produce 10 items and add them to the queue
                if (_itemBox.Count == 0)
                {
                    for (int itemP = 0; itemP < 10; itemP++)
                    {
                        _itemBox.Enqueue(itemP);
                        Console.WriteLine($"Producer har produceret: {itemP}");

                        // Random delay 
                        Random rnd = new Random();
                        int number = rnd.Next(1, 2000);
                        Thread.Sleep(number);
                    }

                    // Signal the waiting threads that there are items in the queue
                    Console.WriteLine("Producer waits ");
                    Monitor.PulseAll(_itemBox);
                }
            }
            finally
            {
                Monitor.Exit(_itemBox);
            }
        }
    }

    // A method to consume items
    public void Consumer()
    {
        while (true)
        {
            Monitor.Enter(_itemBox);
            try
            {
                // If the _itemBox is empty, wait until there are items in the queue
                while (_itemBox.Count == 0)
                {
                    Monitor.Wait(_itemBox);
                }

                // Remove an item from the queue and consume it
                int item = _itemBox.Dequeue();
                Console.WriteLine($"Consumer har consumeret: {item}");

                // If the _itemBox is empty, signal the waiting threads
                if (_itemBox.Count == 0)
                {
                    Console.WriteLine("Consumer waits ");
                }

                // Random delay
                Random rnd = new Random();
                int number = rnd.Next(1, 2000);
                Thread.Sleep(number);
            }
            finally
            {
                Monitor.Exit(_itemBox);
            }
        }
    }
}
