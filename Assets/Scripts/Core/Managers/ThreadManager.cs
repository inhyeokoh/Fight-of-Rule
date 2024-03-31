using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Threading;
using System.Threading.Tasks;

using Cysharp.Threading.Tasks;
using UnityEngine.SceneManagement;

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

    /// <summary>
    /// tick after 만큼 지연 후 실행
    /// </summary>
    /// <param name="lambdaAsync"></param>
    /// <param name="tick_after"></param>
    /// <param name="validator"></param>
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

    /// <summary>
    /// looplambda가 false를 반환할 때 까지 지정한 timing마다 수행
    /// </summary>
    /// <param name="looplambda"></param>
    /// <param name="validator"></param>
    /// <param name="timing"></param>
    public void UniAsyncLoopJob(Func<bool> looplambda, GameObject validator = null, PlayerLoopTiming timing = PlayerLoopTiming.Update)
    {
        if (validator != null)
        {
            var ct = validator.GetCancellationTokenOnDestroy();
            _board.EnqueueAsync(async () => {
                while (looplambda.Invoke())
                    await UniTask.Yield(timing, cancellationToken: ct);
            });

            return;
        }

        //난 책임 안진다 ㅋㅋ
        _board.EnqueueAsync(async () => {
            while (looplambda.Invoke())
                await UniTask.Yield(timing);
        });
    }

    /// <summary>
    /// 지정한 task를 전달만 해줌
    /// </summary>
    /// <param name="asynclambda"></param>
    public void UniAsyncTask(Func<UniTask> asynclambda)
    {
        _board.EnqueueAsync(async () =>
        {
            await asynclambda.Invoke();
        });
    }

    /// <summary>
    /// 지정한 Task를 전달만 해줌 + 인자로 자동으로 cancellationToken을 추출해서 던져줌
    /// </summary>
    /// <param name="asynclambda"></param>
    /// <param name="validator"></param>
    public void UniAsyncTask(Func<CancellationToken, UniTask> asynclambda, GameObject validator)
    {
        var ct = validator.GetCancellationTokenOnDestroy();
        _board.EnqueueAsync(async () =>
        {
            await asynclambda.Invoke(ct);
        });
    }

    private void Example1()
    {
        //GameObject test1 = new GameObject("testObject1");
        var ct = GameManager.Instance.GetCancellationTokenOnDestroy();
        UniAsyncTask(async () =>
        {
            while (true)
            {
                var loadAsync = SceneManager.LoadSceneAsync("로드할 씬");
                if (loadAsync.isDone)
                    return;

                await UniTask.Yield(PlayerLoopTiming.Update, cancellationToken: ct);
            }
        });
    }

    private void Example2()
    {
        //GameObject test2 = new GameObject("testObject2");
        UniAsyncTask(async (ct) =>
        {
            while(true)
            {
                var loadAsync = SceneManager.LoadSceneAsync("로드할 씬");
                if (loadAsync.isDone)
                    return;

                await UniTask.Yield(PlayerLoopTiming.Update, cancellationToken:ct);
            }
        }, GameManager.Instance.gameObject);
    }

    private void Example3()
    {
        UniAsyncJob(() => { Debug.Log("테스트 로그"); }, 1000);

        GameObject test3 = new GameObject("test3");
        UniAsyncJob(() => { Debug.Log("테스트 로그"); }, 1000, test3);
    }

    private void Example4()
    {
        UniAsyncLoopJob(() =>
        {
            var loadAsync = SceneManager.LoadSceneAsync("로드할 씬");
            //false일 경우 return true;
            return loadAsync.isDone == false;
        },GameManager.Instance.gameObject);
    }
}
