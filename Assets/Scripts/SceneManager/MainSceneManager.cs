using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using Shiki.Constants;
using Shiki.Quests;
using Shiki.ReaderWriter;
using Shiki.ReaderWriter.TomlImplementation;

/// <summary>
/// Main entry point for the game.
/// </summary>
public class MainSceneManager : MonoBehaviour {
    public QuestEventManager questEventManager;

    void Start() {
        // Load all of the scenes, additively
        AsyncOperation[] sceneLoadOperations = {
            SceneManager.LoadSceneAsync(SceneName.Village, LoadSceneMode.Additive),
            SceneManager.LoadSceneAsync(SceneName.Spring, LoadSceneMode.Additive),
            SceneManager.LoadSceneAsync(SceneName.Summer, LoadSceneMode.Additive),
            SceneManager.LoadSceneAsync(SceneName.Fall, LoadSceneMode.Additive),
            SceneManager.LoadSceneAsync(SceneName.Winter, LoadSceneMode.Additive)
        };
        WaitForAll(sceneLoadOperations);
        var saveDataManager = new SaveDataManager(
                new TomlQuestReader(),
                new TomlQuestStateReader(),
                new TomlQuestStateWriter()
        );
        this.questEventManager = new QuestEventManager(saveDataManager);
        this.questEventManager.ReadSaves();
    }

    /// <summary>
    /// Converts the name of a scene to the name of the corresponding season.
    /// </summary>
    /// <param name="sceneName">The name of the scene to get the season of.</param>
    /// <returns>The name of the corresponding season, or null if it does not exist.</returns>
    public static string SceneNameToSeasonName(string sceneName) {
        switch(sceneName) {
            case SceneName.Fall:
                return SeasonName.Fall;
            case SceneName.Winter:
                return SeasonName.Winter;
            case SceneName.Spring:
                return SeasonName.Spring;
            case SceneName.Summer:
                return SeasonName.Summer;
            default:
                return null;
        }
    }

    private IEnumerator WaitForAllHelper(AsyncOperation[] ops) {
        foreach(var op in ops) {
            yield return op.Await();
        }
    }

    /// <summary>
    /// Block until all of the given AsyncOperations are complete.
    /// </summary>
    /// <param name="ops">The operations to wait for</param>
    private void WaitForAll(AsyncOperation[] ops) {
        var enumerator = WaitForAllHelper(ops);
        while(enumerator.MoveNext()) { }
    }
}

public static class AsyncOperationExtensions {
    // from https://gamedev.stackexchange.com/q/120643
    /// <summary>
    /// Await the completion of an AsyncOperation
    /// </summary>
    /// <param name="operation">The operation to await the completion of</param>
    /// <returns>An enumerator existing only to make this work.</returns>
    public static IEnumerator Await(this AsyncOperation operation) {
        while(!operation.isDone)
            yield return operation;
    }
}
