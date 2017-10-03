using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using Shiki.Constants;
using Shiki.Quests;
using Shiki.ReaderWriter;
using Shiki.ReaderWriter.TomlImplementation;
using Shiki.EventSystem;
using System.Collections.Generic;
using Shiki.EventSystem.InternalEvents;
using Shiki.EventSystem.Events;

/// <summary>
/// Main entry point for the game.
/// </summary>
public class MainSceneManager : MonoBehaviour {
    public QuestEventManager questEventManager;

    IEnumerable<Scene> GetAllScenes() {
        for(var i = 0; i < SceneManager.sceneCount; i++) {
            yield return SceneManager.GetSceneAt(i);
        }
    }

    private void Awake() {
        EventManager.AttachDelegate<PrologueDoneEvent>(this.OnPrologueDoneEvent);
        EventManager.AttachDelegate<PrologueStartEvent>(this.OnPrologueStartEvent);
    }

    private void OnDestroy() {
        EventManager.RemoveDelegate<PrologueDoneEvent>(this.OnPrologueDoneEvent);
        EventManager.RemoveDelegate<PrologueStartEvent>(this.OnPrologueStartEvent);
    }

    IEnumerator LoadScenes() {

        // Load all of the scenes, additively
        AsyncOperation[] sceneLoadOperations = {
            SceneManager.LoadSceneAsync(SceneName.Village, LoadSceneMode.Additive),
            SceneManager.LoadSceneAsync(SceneName.Spring, LoadSceneMode.Additive),
            SceneManager.LoadSceneAsync(SceneName.Summer, LoadSceneMode.Additive),
            SceneManager.LoadSceneAsync(SceneName.Fall, LoadSceneMode.Additive),
            SceneManager.LoadSceneAsync(SceneName.Winter, LoadSceneMode.Additive)
        };
        foreach(var op in sceneLoadOperations) {
            yield return op.Await();
        }
        foreach(var scene in this.GetAllScenes()) {
            yield return scene.AwaitLoad();
        }
        EventManager.FireEvent(new AllScenesLoadedEvent());
        var saveDataManager = new SaveDataManager(
                new TomlQuestReader(),
                new TomlQuestStateReader(),
                new TomlQuestStateWriter()
        );
        this.questEventManager = new QuestEventManager(saveDataManager);
        this.questEventManager.Init();
        yield break;
    }

    void OnPrologueStartEvent(PrologueStartEvent evt) {
        StartCoroutine(LoadScenes());
    }

    void OnPrologueDoneEvent(PrologueDoneEvent evt) {
        StartCoroutine(FadeInAndDeleteRoom());
    }

    IEnumerator FadeInAndDeleteRoom() {
        yield return new WaitForSeconds(1);
        SteamVR_Fade.Start(Color.black, 0);
        SteamVR_Fade.Start(Color.clear, 1);
        EventManager.FireEvent(new DeleteObjectEvent("StartRoom"));
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
                return SeasonName.None;
        }
    }

    /// <summary>
    /// Converts the name of a scene to the name of the corresponding season.
    /// </summary>
    /// <param name="seasonName">The name of the scene to get the season of.</param>
    /// <returns>The name of the corresponding season, or null if it does not exist.</returns>
    public static string SeasonNameToSceneName(string seasonName) {
        switch(seasonName) {
            case SeasonName.Fall:
                return SceneName.Fall;
            case SeasonName.Winter:
                return SceneName.Winter;
            case SeasonName.Spring:
                return SceneName.Spring;
            case SeasonName.Summer:
                return SceneName.Summer;
            default:
                return null;
        }
    }
}