using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Physics
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class Rigidbody2DManager : MonoBehaviour
    {
        private Rigidbody2D _rigidbody2D;
        public List<Force> Forces = new List<Force>();
        private Vector2 _minVelocity => _terminalVelocity * -1f;
        private Vector2 _maxVelocity => _terminalVelocity;
        [SerializeField] private Vector2 _terminalVelocity = new Vector2(10, 10);
        
        public Rigidbody2D GetRigidbody2D() { return _rigidbody2D; }
        
        public Force GetForce(string id)
        {
            Force f = Forces.Find(x => x.ID == id);
            if (f != null)
            {
                return f;
            }
            else
            {
                Debug.LogWarning("Force with ID " + id + " not found.");
            }

            return null;
        }

        public bool HasForce(string id)
        {
            return Forces.Find(x => x.ID == id) != null;
        }
        
        public void SetForce(string id, Vector2 applyVelocity, float angular)
        {
            Force f = Forces.Find(x => x.ID == id);
            if (f != null)
            {
                f.ApplyVelocity = applyVelocity;
                f.AngularVelocity = angular;
            }
            else
            {
                Forces.Add(new Force(id, applyVelocity, angular, 0));
            }
        }
        
        private void Awake()
        {
            _rigidbody2D = GetComponent<Rigidbody2D>();
        }

        private void FixedUpdate()
        {
            ApplyForces();
            //UpdateForces();
        }

        private void ApplyForces()
        {
            _rigidbody2D.linearVelocity = Vector2.zero;
            _rigidbody2D.angularVelocity = 0f;
            foreach (Force f in Forces)
            {
                _rigidbody2D.linearVelocity += f.ApplyVelocity;
                _rigidbody2D.angularVelocity += f.AngularVelocity;
            }
            _rigidbody2D.linearVelocity = new Vector2(
                Mathf.Clamp(_rigidbody2D.linearVelocity.x, _minVelocity.x, _maxVelocity.x),
                Mathf.Clamp(_rigidbody2D.linearVelocity.y, _minVelocity.y, _maxVelocity.y));
        }

        private void UpdateForces()
        {
            foreach (Force f in Forces)
            {
                if (f.DecayRate == 0) continue;
                f.ApplyVelocity -= f.DecayRate * Time.fixedTime * Vector2.one;
                if (f.ApplyVelocity.magnitude < .5f)
                {
                    RemoveForce(f);
                }
            }
        }

        public void RemoveForce(Force f)
        {
            if (!Forces.Contains(f))
            {
                return;
            }

            int index = Forces.IndexOf(f);
            Forces.RemoveAt(index);
            Forces.RemoveAll(item => item == null);
        }

        public void AddForce(string id, Vector2 force, float angularForce, float decay)
        {
            Forces.Add(new Force(id, force, angularForce, decay));
        }
    }

    public class Force
    {
        public Vector2 ApplyVelocity;
        public float AngularVelocity;
        public float DecayRate;
        public string ID;

        public Force(string id, Vector2 applyVelocity, float angularVelocity, float decayRate)
        {
            ID = id;
            ApplyVelocity = applyVelocity;
            AngularVelocity = angularVelocity;
            DecayRate = decayRate;
        }
    }
}