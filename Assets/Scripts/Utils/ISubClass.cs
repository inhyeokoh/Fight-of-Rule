using System;
public interface ISubClass
{
    void Mount<T>(T board);

    public Action GetAction();
    public void Init();
    public void Excute();
    public void Clear();
}
