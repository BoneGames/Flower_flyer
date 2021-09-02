using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace UnityStandardAssets.Characters.FirstPerson
{
    [Serializable]
    public class BoostCheck
    {
        #region DATA_STRUCTURES
        public enum InteruptionTrigger
        {
            MouseDown, MouseUp, SpaceBar, None
        }

        [Serializable]
        public enum BoostPart
        {
            Up, Mid, Down
        }

        [Serializable]
        public enum BoostState
        {
            Base, Boost, MaxBoost, Burnout, None
        }

        [Serializable]
        public struct SpeedEffectCurve
        {
            public BoostPart thisPart;
            public float duration;
            public InteruptionTrigger interuptTrigger;
            public bool toMinOnInterupt;
            public AnimationCurve speedCurve;
            public EffectController[] effects;

            public void ControlEffects(float _timer)
            {
                foreach (var effect in effects)
                {
                    foreach (var field in effect.effectSettings)
                    {
                        if (!effect.on)
                            continue;
                        // get the value required for the field
                        float curveMulti = field.curve.Evaluate(_timer);
                        float valRange = Mathf.Abs(field.minMax.x - field.minMax.y);
                        float value = valRange * curveMulti + (field.minMax.x > field.minMax.y ? field.minMax.y : field.minMax.x);

                        effect.ControlEffect(field.paramName, value);
                    }
                }
            }

            public void CompleteCurve(bool toMin = false)
            {
                foreach (var effect in effects)
                {
                    if (effect.on)
                        effect.MinMax(toMin);
                }
            }
        }

        [Serializable]
        public struct BoostValues
        {
            public BoostState thisState;
            public float speed;
            public SpeedEffectCurve up, mid, down;
            public Dictionary<BoostPart, SpeedEffectCurve> boostPartsDict;
            public BoostValues(BoostValues bV, Dictionary<BoostPart, SpeedEffectCurve> _dict)
            {
                this.thisState = bV.thisState;
                this.speed = bV.speed;
                this.up = bV.up;
                this.mid = bV.mid;
                this.down = bV.down;
                this.boostPartsDict = _dict;
            }
        }
        #endregion

        #region VARIABLES
        public bool boostActive;
        // REFS
        UI ui;
        flight_controller_V2 coroutineCaller;

        // Boost Data
        [SerializeField] BoostValues normal, boost, burnout, maxBoost;
        public float lerpSpeed;

        // Collections
        Dictionary<BoostState, BoostValues> boostStatesDict = new Dictionary<BoostState, BoostValues>();

        // States
        [SerializeField] BoostState _bs;
        public BoostState BoostState_
        {
            get { return _bs; }
            set
            {
                if (value != _bs)
                {
                    _bs = value;
                    if (ui != null)
                        ui.SetBoostStateLabel(_bs.ToString());
                }
            }
        }
        [SerializeField] BoostPart _ts;
        public BoostPart BoostPart_
        {
            get { return _ts; }
            set
            {
                if (value != _ts)
                {
                    _ts = value;
                    if (ui != null)
                        ui.SetTransitionStateLabel(_ts.ToString());
                }
            }
        }
        #endregion

        #region INIT
        public void OnStart(flight_controller_V2 _monoB, UI _ui)
        {
            if (!boostActive)
                return;

            coroutineCaller = _monoB;
            ui = _ui;

            InitEffectControllersAndDicts();
            InitUI();

            BoostCycle(BoostState.Base);
        }

        void InitUI()
        {
            if (ui)
            {
                ui.SetBoostStateLabel(BoostState_.ToString());
                ui.SetTransitionStateLabel(BoostPart_.ToString());
            }
        }

        void InitEffectControllersAndDicts()
        {
            BoostValues[] collec = new BoostValues[4] { normal, boost, maxBoost, burnout };
            for(int i = 0; i < collec.Length; i++)
            {
                Dictionary<BoostPart, SpeedEffectCurve> d = new Dictionary<BoostPart, SpeedEffectCurve>();

                collec[i] = new BoostValues(collec[i], d);
                BoostValues bV = collec[i];
                boostStatesDict.Add(bV.thisState, bV);

                foreach (var sEC in new SpeedEffectCurve[] { bV.up, bV.mid, bV.down })
                {
                    bV.boostPartsDict.Add(sEC.thisPart, sEC);

                    foreach (var effect in sEC.effects)
                        effect.Init();
                }
            }
        }
        #endregion

        #region STATE_SETTING
        void SetBoostPart(BoostPart newState)
        {
            if (BoostPart_ != newState)
                BoostPart_ = newState;
        }

        void SetBoostState(BoostState newState)
        {
            if (newState != BoostState_)
                BoostState_ = newState;
        }
        #endregion

        #region BOOST_STAGES
        public IEnumerator BoostUp()
        {
            SetBoostPart(BoostPart.Up);
            BoostValues _boost = boostStatesDict[BoostState_];
            SpeedEffectCurve sEC = _boost.boostPartsDict[BoostPart_];

            BoostState nextState = GetNextBoostState();
            float _timer = 0;
            while (_timer <= 1)
            {
                if (sEC.duration == 0)
                    break;

                TryExitStage(1 - _timer, sEC, nextState);
                lerpSpeed = SetBoostSpeed(_boost, _timer);
                SetEffectVals(_boost, _timer);
                _timer += Time.deltaTime / sEC.duration;
                yield return null;
            }

            // reset all max values
            lerpSpeed = _boost.speed;
            // effects
            sEC.CompleteCurve();

            // ### MID BOOST PART ###

            SetBoostPart(BoostPart.Mid);

            sEC = _boost.boostPartsDict[BoostPart_];

            nextState = GetNextBoostState();
            _timer = 0;
            while (_timer <= 1)
            {
                if (sEC.duration == 0)
                {
                    _timer = 1;
                    break;
                }

                TryExitStage(1 - _timer, sEC, nextState);
                SetEffectVals(_boost, _timer);
                _timer += Time.deltaTime / sEC.duration;
                yield return null;
            }
            sEC.CompleteCurve();
            TryExitStage(1 - _timer, sEC, BoostState.None);
        }
       
        public IEnumerator BoostDown(float _timer)
        {
            SetBoostPart(BoostPart.Down);
            BoostValues _boost = boostStatesDict[BoostState_];
            SpeedEffectCurve sEC = _boost.boostPartsDict[BoostPart_];

            while (_timer <= 1)
            {
                if (sEC.duration == 0)
                    break;

                TryExitStage(1 - _timer, sEC, BoostState.MaxBoost);
                lerpSpeed = SetBoostSpeed(_boost, _timer);
                SetEffectVals(_boost, _timer);
                _timer += Time.deltaTime / sEC.duration;
                yield return null;
            }

            // reset all min values
            lerpSpeed = normal.speed;

            if (BoostState_ != BoostState.Base)
                sEC.CompleteCurve();

            // EXIT STUFF
            BoostCycle(BoostState.Base);
        }

        public IEnumerator AwaitInput()
        {
            while (true)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    if (coroutineCaller.boost != null)
                        coroutineCaller.StopCoroutine(coroutineCaller.boost);

                    SetBoostState(BoostState.MaxBoost);

                    coroutineCaller.boost = coroutineCaller.StartCoroutine(BoostUp());
                }
                yield return null;
            }
        }
        #endregion

        #region BOOST_STAGE_HELPERS
        BoostState GetNextBoostState()
        {
            if (BoostState_ == BoostState.Base)
            {
                switch (BoostPart_)
                {
                    case BoostPart.Up:
                        return BoostState.None;
                    case BoostPart.Mid:
                        return BoostState.Boost;
                    case BoostPart.Down:
                        return BoostState.MaxBoost;
                }
            }
            return BoostState.None;
        }
        
        float SetBoostSpeed(BoostValues _boost, float _timer)
        {
            AnimationCurve boostCurve = _boost.boostPartsDict[BoostPart_].speedCurve;
            return normal.speed + ((_boost.speed - normal.speed) * boostCurve.Evaluate(_timer));
        }

        void SetEffectVals(BoostValues _boost, float _timer)
        {
            // control PP effects
            SpeedEffectCurve sEC = _boost.boostPartsDict[BoostPart_];
            sEC.ControlEffects(_timer);
        }
        #endregion

        #region BOOST_SETTING_LOGIC
        void TryExitStage(float _timer, SpeedEffectCurve sEC, BoostState nextBoost)
        {
            if (!CanExit(_timer, sEC.interuptTrigger))
                return;

            if (BoostState_ == BoostState.Boost && BoostPart_ == BoostPart.Mid)
                nextBoost = _timer <= 0 ? BoostState.Burnout : BoostState.None;
            bool interupted = _timer != 0;

            sEC.CompleteCurve(sEC.toMinOnInterupt && interupted);

            BoostCycle(nextBoost, _timer);
        }

        bool CanExit(float _timer, InteruptionTrigger interuptTrigger)
        {
            // if timer is up - can exit
            if (_timer <= 0)
                return true;

            switch (interuptTrigger)
            {
                case InteruptionTrigger.MouseDown:
                    // continue cruising if pre boost stage
                    if (Input.GetMouseButton(0))
                        return true;
                    break;
                case InteruptionTrigger.MouseUp:
                    // continue boosting if on boost
                    if (!Input.GetMouseButton(0))
                        return true;
                    break;
                case InteruptionTrigger.SpaceBar:
                    if (Input.GetKey(KeyCode.Space))
                        return true;
                    break;
            }
            return false;
        }

        void BoostCycle(BoostState nextBoost, float timer = -1)
        {
            if (coroutineCaller.boost != null)
                coroutineCaller.StopCoroutine(coroutineCaller.boost);

            if (nextBoost == BoostState.None)
            {
                coroutineCaller.boost = coroutineCaller.StartCoroutine(BoostDown(timer));
            }
            else if (BoostState_ == nextBoost && BoostPart_ == BoostPart.Down)
            {
                coroutineCaller.boost = coroutineCaller.StartCoroutine(AwaitInput());
            }
            else
            {
                SetBoostState(nextBoost);
                coroutineCaller.boost = coroutineCaller.StartCoroutine(BoostUp());
            }
        }
        #endregion
    }
}