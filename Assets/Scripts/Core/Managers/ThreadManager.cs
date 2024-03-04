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
        //completion port thread�� �ʱ�ġ�� �޶���. thread ���� �� �� �ڴ�� ����. ������ ����ġ�� ã�Ƽ� �˾Ƽ� �־����
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
    /// tick after ��ŭ ���� �� ����
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

        //�� å�� ������ ����
        _board.EnqueueAsync(async () => {
            await UniTask.Delay(tick_after);
            lambdaAsync?.Invoke();
        });
    }

    /// <summary>
    /// looplambda�� false�� ��ȯ�� �� ���� ������ timing���� ����
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

        //�� å�� ������ ����
        _board.EnqueueAsync(async () => {
            while (looplambda.Invoke())
                await UniTask.Yield(timing);
        });
    }

    /// <summary>
    /// ������ task�� ���޸� ����
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
    /// ������ Task�� ���޸� ���� + ���ڷ� �ڵ����� cancellationToken�� �����ؼ� ������
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
                var loadAsync = SceneManager.LoadSceneAsync("�ε��� ��");
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
                var loadAsync = SceneManager.LoadSceneAsync("�ε��� ��");
                if (loadAsync.isDone)
                    return;

                await UniTask.Yield(PlayerLoopTiming.Update, cancellationToken:ct);
            }
        }, GameManager.Instance.gameObject);
    }

    private void Example3()
    {
        UniAsyncJob(() => { Debug.Log("�׽�Ʈ �α�"); }, 1000);

        GameObject test3 = new GameObject("test3");
        UniAsyncJob(() => { Debug.Log("�׽�Ʈ �α�"); }, 1000, test3);
    }

    private void Example4()
    {
        UniAsyncLoopJob(() =>
        {
            var loadAsync = SceneManager.LoadSceneAsync("�ε��� ��");
            //false�� ��� return true;
            return loadAsync.isDone == false;
        },GameManager.Instance.gameObject);
    }
}
