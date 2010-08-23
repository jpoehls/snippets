using System;

namespace Samples.DataImport
{
    public interface ICsvImporter : IDisposable
    {
        void Import(string fileName);
    }
}