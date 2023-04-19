using System;
using System.Collections.Generic;
using System.Threading;

public class Program
{
    Queue<int> _itemBox = new Queue<int>();

    public static void Main(string[] args)
    {
        Program program = new Program();

        Thread producerThread = new Thread(new ThreadStart(program.Producer));
        Thread consumerThread = new Thread(new ThreadStart(program.Consumer));

        producerThread.Start();
        consumerThread.Start();

        producerThread.Join();
        consumerThread.Join();
    }

    public void Producer()
    {
        while(true)
        {
            Monitor.Enter(_itemBox);
            try
            {

                if (_itemBox.Count == 0)
                {
                    for(int itemP = 0; itemP < 10; itemP++)
                    {
                        _itemBox.Enqueue(itemP);
                        Console.WriteLine($"Producer har produceret: {itemP}");

                        Random rnd = new Random();
                        int number = rnd.Next(1, 2000);
                        Thread.Sleep(number);
                    }
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

    public void Consumer()
    {
        while (true)
        {
            Monitor.Enter(_itemBox);
            
                try
                {
                while (_itemBox.Count == 0)
                {
                    Monitor.Wait(_itemBox);
                }
                int item = _itemBox.Dequeue();
                Console.WriteLine($"Consumer har consumeret: {item}");
                if(_itemBox.Count == 0)
                {
                    Console.WriteLine("Consumer waits ");
                }
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