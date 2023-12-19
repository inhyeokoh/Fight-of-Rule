using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Threading;
using System.Threading.Tasks;

public class ThreadManager : SubClass<GameManager>
{
    static readonly int THREAD_WORKER = 2;
    static readonly int THREAD_IOCP = 2;

    struct OwnedJob
    {
        public object owner;
        public Action work;
    }

    Queue<OwnedJob> _jobs = new Queue<OwnedJob>();
    object _jobLock = new object();

    protected override void _Clear()
    {
        
    }

    protected override void _Excute()
    {
        
    }

    protected override void _Init()
    {
        //completion port thread는 초기치와 달라짐. thread 부족 시 지 멋대로 만듬. 싫으면 적정치를 찾아서 알아서 넣어야함
        ThreadPool.SetMinThreads(1, 1);
        ThreadPool.SetMaxThreads(THREAD_WORKER, THREAD_IOCP);

        for (int i = 0; i < THREAD_WORKER; i++)
        {
            var task = new Task(() => { _ThreadWork(); });
            task.Start();
        }
    }

    public void EnqueueJob<T>(T owner, Action lambdaEx)
    {
        object capture = owner;
        var job = new OwnedJob { owner = capture, work = lambdaEx };

        lock(_jobLock)
        {
            _jobs.Enqueue(job);
        }
    }

    void _ThreadWork()
    {
        Queue<OwnedJob> threadJob = new Queue<OwnedJob>();

        while(true)
        {
            lock(_jobLock)
            {
                while (_jobs.Count > 0)
                    threadJob.Enqueue(_jobs.Dequeue());
            }

            while(threadJob.Count > 0)
            {
                var item = threadJob.Dequeue();
                item.work?.Invoke();
            }

            _SleepEx();
        }
    }

    void _SleepEx() { Thread.Sleep(1); }
}
