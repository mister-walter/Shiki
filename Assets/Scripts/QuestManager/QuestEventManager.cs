﻿/// @author Larisa Motova
using System.Collections.Generic;
using UnityEngine;
using Shiki.EventSystem;
using Shiki.EventSystem.Events;
using Shiki.ReaderWriter;

namespace Shiki.Quests {

    /// <summary>
    /// Manager for all events that happen related to quests and tasks. 
    /// Handles communication between the UI files where quest-related data is saved.
    /// </summary>
    public class QuestEventManager {

        /// <summary>
        /// The associated save data manager, which handles saving and writing game data to the filesystem.
        /// </summary>
        SaveDataManager saveDataManager;

        public QuestEventManager(SaveDataManager sdm) {
            saveDataManager = sdm;
        }

        public void Init()
        {
            this.ReadSaves();
            this.AttachHandlers();
        }

        /// <summary>
        /// Whenever an Interaction Event is fired, this runs the event through all task trees recursively to see if a task is now completed
        /// </summary>
        /// <param name="evt">The event fired</param>
        /// <param name="doAll">True if tasks should be run when they completed, regardless of previous completion status. False if a task should only ever be completed once</param>
        public void UpdateTasks(InteractionEvent evt, bool doAll) {
            foreach(TaskNode tn in this.saveDataManager.taskTree) {
                if(!tn.AssociatedTask.isComplete || doAll) {
                    UpdateTaskBranch(evt, tn, doAll);
                }
            }
        }

        /// <summary>
        /// For each individual tree, this runs the Interaction Event through all tasks and their children recursively to check whether a task has been completed.
        /// This first checks if a task's child nodes have all been completed, and only then runs the event through the tasks trigger function. 
        /// </summary>
        /// <param name="evt">Evt.</param>
        /// <param name="tn">Current task node</param>
        /// <param name="doAll">True if trigger should be run regardless of whether task has already been completed once or not</param>
        public bool UpdateTaskBranch(InteractionEvent evt, TaskNode tn, bool doAll) {
            bool allChildrenComplete = true;

            foreach(TaskNode tchild in tn.Children) {
                allChildrenComplete &= (tchild.AssociatedTask.isComplete || UpdateTaskBranch(evt, tchild, doAll));
                // if child not complete, and updating child doesn't complete the child, allChildrenComplete = false
            }

            if((doAll || !tn.AssociatedTask.isComplete) &&
               (allChildrenComplete && (tn.AssociatedTask.trigger == null || tn.AssociatedTask.trigger(evt)))) {
                tn.AssociatedTask.isComplete = true;
                tn.AssociatedTask.onComplete();
                return true;
            }
            return false;
        }

        /// <summary>
        /// Writes the task tree to a file using the save data manager
        /// </summary>
        public void SaveQuestStatus() {
            saveDataManager.WriteSaves();
        }

        /// <summary>
        /// Reads task tree from file using save data manager
        /// </summary>
        public void ReadSaves() {
            saveDataManager.ReadSaves();
        }

        #region Event Handlers
        private void OnObjectPlacedInSeasonFinishedEvent(ObjectPlacedInSeasonFinishedEvent evt) {
            InteractionEvent ievt = new InteractionEvent();
            ievt.location = evt.seasonName;
            ievt.sourceObject = evt.placedObject;
            ievt.kind = InteractionKind.Drop;
            this.UpdateTasks(ievt, false);
        }

        private void OnPlayerEnteredAreaEvent(PlayerEnteredAreaEvent evt) {
            InteractionEvent ievt = new InteractionEvent();
            ievt.location = evt.seasonName;
            ievt.kind = InteractionKind.Enter;
            this.UpdateTasks(ievt, false);
        }

        private void OnPlayerOpenedInventoryEvent(PlayerOpenedInventoryEvent evt) {
            InteractionEvent ievt = new InteractionEvent();
            ievt.kind = InteractionKind.Open;
            this.UpdateTasks(ievt, false);
        }

        private void OnObjectStoredEvent(ObjectStoredEvent evt) {
            InteractionEvent ievt = new InteractionEvent();
            ievt.kind = InteractionKind.Store;
            ievt.sourceObject = evt.storedObject;
            this.UpdateTasks(ievt, false);
        }

        private void OnObjectRetrievedEvent(ObjectRetrievedEvent evt) {
            InteractionEvent ievt = new InteractionEvent();
            ievt.kind = InteractionKind.Retrieve;
            ievt.sourceObject = evt.retrievedObject;
            this.UpdateTasks(ievt, false);
        }

        private void OnObjectHitEvent(ObjectHitEvent evt) {
            Debug.Log("Object hit event");
            InteractionEvent ievt = new InteractionEvent();
            ievt.kind = InteractionKind.Hit;
            ievt.targetObject = evt.hitObject;
            this.UpdateTasks(ievt, false);
        }

        private void OnObjectDroppedOntoDropTargetEvent(ObjectDroppedOntoDropTargetEvent evt) {
            InteractionEvent ievt = new InteractionEvent();
            ievt.kind = InteractionKind.Drop;
            ievt.sourceObject = evt.droppedObject;
            ievt.targetObject = evt.dropTarget;
            this.UpdateTasks(ievt, false);
        }

        private void AttachHandlers() {
            EventManager.AttachDelegate<ObjectPlacedInSeasonFinishedEvent>(this.OnObjectPlacedInSeasonFinishedEvent);
            EventManager.AttachDelegate<PlayerEnteredAreaEvent>(this.OnPlayerEnteredAreaEvent);
            //EventManager.AttachDelegate<PlayerOpenedInventoryEvent>(this.OnPlayerOpenedInventoryEvent);
            EventManager.AttachDelegate<ObjectStoredEvent>(this.OnObjectStoredEvent);
            EventManager.AttachDelegate<ObjectRetrievedEvent>(this.OnObjectRetrievedEvent);
            EventManager.AttachDelegate<ObjectHitEvent>(this.OnObjectHitEvent);
            EventManager.AttachDelegate<ObjectDroppedOntoDropTargetEvent>(this.OnObjectDroppedOntoDropTargetEvent);
        }

        private void DetachHandlers() {
            EventManager.RemoveDelegate<ObjectPlacedInSeasonFinishedEvent>(this.OnObjectPlacedInSeasonFinishedEvent);
            EventManager.RemoveDelegate<PlayerEnteredAreaEvent>(this.OnPlayerEnteredAreaEvent);
           // EventManager.RemoveDelegate<PlayerOpenedInventoryEvent>(this.OnPlayerOpenedInventoryEvent);
            EventManager.RemoveDelegate<ObjectStoredEvent>(this.OnObjectStoredEvent);
            EventManager.RemoveDelegate<ObjectRetrievedEvent>(this.OnObjectRetrievedEvent);
            EventManager.RemoveDelegate<ObjectHitEvent>(this.OnObjectHitEvent);
            EventManager.RemoveDelegate<ObjectDroppedOntoDropTargetEvent>(this.OnObjectDroppedOntoDropTargetEvent);
        }
    }
    #endregion

    /// <summary>
    /// Interaction kind.
    /// Tasks contain trigger and oncomplete functions in a language where most keywords are associated with one of the following enum values.
    /// </summary>
    public enum InteractionKind {
        Get, Enter, Drop, Cut, Hit, Weave, Dig, Grind, Merge, Become, Open, Play, Store, Retrieve, None
    };

    /// <summary>
    /// Interaction event. Gets fired when a player interacts with an object, enters a location, interacts two objects with each other, or any other quest-related interaction occurs.
    /// </summary>
    public class InteractionEvent : IGameEvent {
        /// <summary>
        /// The original object involved in the interaction
        /// </summary>
        public GameObject sourceObject { get; set; }

        /// <summary>
        /// The object receiving interaction/being targeted in the interaction
        /// </summary>
        public GameObject targetObject { get; set; }

        /// <summary>
        /// Current location of the player when event occurs
        /// </summary>
        /// <value>The location.</value>
        public string location { get; set; }

        /// <summary>
        /// Type of interaction between objects/player
        /// </summary>
        public InteractionKind kind { get; set; }
    }

    /// <summary>
    /// Parsing result. When a task's trigger or oncomplete functions are read in, they are filled into this Parsing Result class
    /// </summary>
    public class ParsingResult {
        /// <summary>
        /// First object found in parsing.
        /// </summary>
        public string obj1 { get; set; }

        /// <summary>
        /// Second object found in parsing. This is typically the tool the player is using.
        /// </summary>
        public string obj2 { get; set; }

        /// <summary>
        /// Quantity of the first object.
        /// </summary>
        public int obj1Quantity { get; set; }

        /// <summary>
        /// Quantity of the second object
        /// </summary>
        public int obj2Quantity { get; set; }

        /// <summary>
        /// Location listed in parsing
        /// </summary>
        public string location { get; set; }

        /// <summary>
        /// Interaction required from player (enters, hits, etc)
        /// </summary>
        public InteractionKind interactionKind { get; set; }

        /// <summary>
        /// Event the UI should perform
        /// </summary>
        public UIActionKind uiEventKind { get; set; }

        /// <summary>
        /// Describes relationship the 2 objects, if applicable. (With, on, becomes, etc)
        /// </summary>
        public string objToObjInteractionType { get; set; }
    }
}
