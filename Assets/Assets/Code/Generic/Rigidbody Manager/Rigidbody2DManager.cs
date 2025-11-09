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
                Forces.Add(new Force(id, applyVelocity, angular, 1));
            }
        }

        public void AdjustForce(string id, Vector2 applyVelocity, float angular)
        {
            Force f = Forces.Find(x => x.ID == id);
            if (f != null)
            {
                f.ApplyVelocity += applyVelocity;
                f.AngularVelocity += angular;
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
            UpdateForces();
        }

        private void ApplyForces()
        {
            Vector2 totalVelocity = Vector2.zero;
            float totalAngular = 0f;

            //_rigidbody2D.linearVelocity = Vector2.zero;
            //_rigidbody2D.angularVelocity = 0f;

            foreach (Force f in Forces)
            {
                totalVelocity += f.ApplyVelocity;
                totalAngular += f.AngularVelocity;

                //_rigidbody2D.linearVelocity += f.ApplyVelocity;
                //_rigidbody2D.angularVelocity += f.AngularVelocity;
                //Debug.Log("Force" + f);
                //Debug.Log("LinVel Apply" + -f.ApplyVelocity);
                //Debug.Log("LinVel" + _rigidbody2D.linearVelocity);
                //Debug.Log("AngVel Apply" + f.AngularVelocity);
                //Debug.Log("AngVel" + _rigidbody2D.angularVelocity);
            }
            _rigidbody2D.linearVelocity = new Vector2(
                Mathf.Clamp(totalVelocity.x, _minVelocity.x, _maxVelocity.x),
                Mathf.Clamp(totalVelocity.y, _minVelocity.y, _maxVelocity.y)
            );
            _rigidbody2D.angularVelocity = totalAngular;
            //_rigidbody2D.linearVelocity = new Vector2(
            //Mathf.Clamp(_rigidbody2D.linearVelocity.x, _minVelocity.x, _maxVelocity.x),
            //Mathf.Clamp(_rigidbody2D.linearVelocity.y, _minVelocity.y, _maxVelocity.y));
        }

        private void UpdateForces()
        {
            // Reverse iteration
            for (int i = Forces.Count - 1; i >= 0; i--)
            {
                Force f = Forces[i]; // current force

                if (f.DecayRate == 0) continue; // skip without decay

                // Approach 0
                Vector2 decayAmount = f.ApplyVelocity.normalized * f.DecayRate * Time.fixedDeltaTime;

                // Clip before 0
                if (decayAmount.magnitude >= f.ApplyVelocity.magnitude)
                {
                    RemoveForce(f);
                }
                else
                {
                    f.ApplyVelocity -= decayAmount;
                }

                // Decay angular velocity 
                float angularDecay = Mathf.Sign(f.AngularVelocity) * f.DecayRate * Time.fixedDeltaTime;
                if (Mathf.Abs(angularDecay) >= Mathf.Abs(f.AngularVelocity))
                {
                    f.AngularVelocity = 0f;
                }
                else
                {
                    f.AngularVelocity -= angularDecay;
                }

                // Remove both if needed
                if (f.ApplyVelocity.magnitude < 0.01f && Mathf.Abs(f.AngularVelocity) < 0.01f)
                {
                    RemoveForce(f);
                }
            }

            //foreach (Force f in Forces)
            //{
            //    if (f.DecayRate == 0) continue;
            //    f.ApplyVelocity -= f.DecayRate * Time.fixedTime * Vector2.one;
            //    if (f.ApplyVelocity.magnitude < .5f)
            //    {
            //        RemoveForce(f);
            //    }
            //}
        }

        public void RemoveForce(Force f)
        {
            Forces.Remove(f);
            //if (!Forces.Contains(f))
            //{
            //    return;
            //}

            //int index = Forces.IndexOf(f);
            //Forces.RemoveAt(index);
            //Forces.RemoveAll(item => item == null);
        }

        public void RemoveForce(string id)
        {
            Force f = Forces.Find(x => x.ID == id);
            if (f != null)
            {
                Forces.Remove(f);
            }
        }

        public void AddForce(string id, Vector2 force, float angularForce, float decay)
        {
            Forces.Add(new Force(id, force, angularForce, decay));
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
}