using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Threading;
using System.Threading.Tasks;

using Cysharp.Threading.Tasks;

public class ThreadManager : SubClass<GameManager>
{
    static readonly int THREAD_WORKER = 2;
    static readonly int THREAD_IOCP = 2;

    struct OwnedJob
    {
        public object owner;
        public Action work;
    }

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
    }

    public void EnqueueJob(Action lambdaEx, int tick_after = 0, object mem = null)
    {
        ThreadPool.QueueUserWorkItem(async (obj) =>
        {
            object remember = mem;
            if(tick_after > 0)
                await Task.Delay(tick_after);
            lambdaEx?.Invoke();
        });
    }

    public void EnqueueJob<T>(Func<T> lambdaEx, int tick_after, Action<T> callback = null)
    {
        if (lambdaEx == null)
            return;

        ThreadPool.QueueUserWorkItem(async (obj) =>
        {
            await Task.Delay(tick_after);
            var ret = lambdaEx.Invoke();

            callback?.Invoke(ret);
        });
    }

    public void EnqueueJob(Func<object[]> lambdaEx, int tick_after, Action<object[]> callback = null)
    {
        if (lambdaEx == null)
            return;

        ThreadPool.QueueUserWorkItem(async (obj) =>
        {
            await Task.Delay(tick_after);
            var ret = lambdaEx.Invoke();

            callback?.Invoke(ret);
        });
    }

    public void UniAsyncJob(Action lambdaAsync, int tick_after = 0, GameObject validator = null)
    {
        if(validator != null)
        {
            var ct = validator.GetCancellationTokenOnDestroy();
            _board.EnqueueAsync(async () => {
                await UniTask.Delay(tick_after, cancellationToken: ct);
                if (validator != null)
                    lambdaAsync?.Invoke();
            });

            return;
        }

        //난 책임 안진다 ㅋㅋ
        _board.EnqueueAsync(async () => {
            await UniTask.Delay(tick_after);
            lambdaAsync?.Invoke();
        });
    }

    public void UniAsyncJob(Action lambdaAsync, PlayerLoopTiming timing = PlayerLoopTiming.Update, GameObject validator = null, bool forward = false)
    {
        if (validator != null)
        {
            var ct = validator.GetCancellationTokenOnDestroy();
            _board.EnqueueAsync(async () => {
                if (forward == false)
                    await UniTask.Yield(timing, cancellationToken:ct);

                if (validator != null)
                    lambdaAsync?.Invoke();

                if (forward)
                    await UniTask.Yield(timing, cancellationToken: ct);
            });

            return;
        }

        //난 책임 안진다 ㅋㅋ
        _board.EnqueueAsync(async () => {
            if (forward == false)
                await UniTask.Yield(timing);

            lambdaAsync?.Invoke();

            if (forward)
                await UniTask.Yield(timing);
        });
    }
}
