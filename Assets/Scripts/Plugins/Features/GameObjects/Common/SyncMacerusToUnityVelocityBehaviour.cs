using System;
using System.Linq;

using Assets.Scripts.Plugins.Features.GameObjects.Common.Api;
using Assets.Scripts.Unity.Threading;

using Macerus.Api.Behaviors;

using NexusLabs.Contracts;

using UnityEngine;

namespace Assets.Scripts.Plugins.Features.GameObjects.Common
{
    /// <summary>
    /// Responsible for synchronizing Macerus-controlled velocity for movement 
    /// to the front-end Unity.
    /// </summary>
    public sealed class SyncMacerusToUnityVelocityBehaviour :
        MonoBehaviour,
        ISyncMacerusToUnityVelocityBehaviour
    {
        public IObservableMovementBehavior ObservableMovementBehavior { get; set; }

        public Rigidbody2D RigidBody2D { get; set; }

        public IDispatcher Dispatcher { get; set; }

        private void Start()
        {
            Contract.RequiresNotNull(
                ObservableMovementBehavior,
                $"{nameof(ObservableMovementBehavior)} was not set on '{gameObject}.{this}'.");
            Contract.RequiresNotNull(
                RigidBody2D,
                $"{nameof(RigidBody2D)} was not set on '{gameObject}.{this}'.");
            ObservableMovementBehavior.VelocityChanged += ObservableMovementBehavior_VelocityChanged;

            // sync macerus (source of truth) to unity
            SyncMacerusToUnityVelocity();
        }

        private void OnDestroy()
        {
            if (ObservableMovementBehavior != null)
            {
                ObservableMovementBehavior.VelocityChanged -= ObservableMovementBehavior_VelocityChanged;
            }
        }

        private void SyncMacerusToUnityVelocity()
        {
            if (RigidBody2D == null)
            {
                return;
            }

            RigidBody2D.velocity = new Vector2(
                (float)ObservableMovementBehavior.VelocityX,
                (float)ObservableMovementBehavior.VelocityY);
        }

        private void ObservableMovementBehavior_VelocityChanged(
            object sender,
            EventArgs e) => Dispatcher.RunOnMainThread(SyncMacerusToUnityVelocity);
    }
}