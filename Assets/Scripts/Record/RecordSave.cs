using System;
using LeaseExtension.Record.Contract;
using LeaseExtension.Save;
using R3;
using UnityEngine;
using Zenject;

namespace LeaseExtension.Record
{
    [CreateAssetMenu(fileName = "RecordSave", menuName = "Scriptable Objects/Saves/Record")]
    internal class RecordSave : ASave<Record>, IInitializable, IDisposable
    {
        private IRecordModel _record;
        private IDisposable _disposable;

        [Inject]
        public void Init(IRecordModel record)
        {
            _record = record;
        }

        public void Initialize()
        {
            Init();
            _disposable = _record.RecordChanged.Subscribe(r =>
            {
                SaveData.Value = r;
                Save();
            });
        }

        public void Dispose()
        {
            _disposable.Dispose();
        }
    }
}
