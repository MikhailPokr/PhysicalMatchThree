using System;

namespace PMT
{
    internal interface IGemController : IService
    {
        event Action FieldClear;
        event Action<GemType[]> NeedGenerate;

        void Generate();
        void Generate(int count);
    }
}