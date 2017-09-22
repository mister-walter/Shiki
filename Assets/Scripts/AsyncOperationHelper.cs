using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AsyncOperationHelper {
    /// <summary>
    /// Block until all of the given AsyncOperations are complete.
    /// </summary>
    /// <param name="ops">The operations to wait for</param>
    public static void WaitForAll(IEnumerable<AsyncOperation> ops) {
        var enumerator = WaitForAllHelper(ops);
        while(enumerator.MoveNext()) { }
    }

    private static IEnumerator WaitForAllHelper(IEnumerable<AsyncOperation> ops) {
        foreach(var op in ops) {
            yield return op.Await();
        }
    }
}
