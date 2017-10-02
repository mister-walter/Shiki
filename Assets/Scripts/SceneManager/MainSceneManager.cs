﻿using System;
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

    IEnumerator Start() {
#if UNITY_EDITOR
        // Needed to keep the compiler happy
        yield return null;
#else
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
#endif
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
                return String.Empty;
        }
    }


}