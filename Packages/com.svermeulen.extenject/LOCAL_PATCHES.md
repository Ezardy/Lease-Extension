# Local Extenject patch

Embedded from Mathijs-Bakker/Extenject commit `ec31a279be809bc98f5d1ed9f2add1ce3ddc77fc`.

`Runtime/Factories/Pooling/MemoryPoolBase.cs`: `GetInternal` performs one allocation
before checking whether to retry. Validation deliberately returns null placeholders;
retrying those indefinitely freezes Unity when validating pooled factories.
Runtime continues to skip null or destroyed objects.