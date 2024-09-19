using System;
using UnityEngine.UIElements;

namespace BloodClockTower.UI
{
    public class PointerManipulator : Manipulator
    {
        private readonly Action<PointerDownEvent> _onDown;
        private readonly Action<PointerMoveEvent> _onMove;
        private readonly Action<PointerUpEvent> _onUp;

        public PointerManipulator(
            Action<PointerDownEvent>? onDown = null,
            Action<PointerMoveEvent>? onMove = null,
            Action<PointerUpEvent>? onUp = null
        )
        {
            _onDown = onDown ?? (_ => { });
            _onMove = onMove ?? (_ => { });
            _onUp = onUp ?? (_ => { });
        }

        protected override void RegisterCallbacksOnTarget()
        {
            target.RegisterCallback<PointerDownEvent>(PointerDownHandler);
            target.RegisterCallback<PointerMoveEvent>(PointerMoveHandler);
            target.RegisterCallback<PointerUpEvent>(PointerUpHandler);
        }

        protected override void UnregisterCallbacksFromTarget()
        {
            target.UnregisterCallback<PointerDownEvent>(PointerDownHandler);
            target.UnregisterCallback<PointerMoveEvent>(PointerMoveHandler);
            target.UnregisterCallback<PointerUpEvent>(PointerUpHandler);
        }

        private void PointerDownHandler(PointerDownEvent evt)
        {
            target.CapturePointer(evt.pointerId);
            evt.StopPropagation();
            _onDown.Invoke(evt);
        }

        private void PointerMoveHandler(PointerMoveEvent evt)
        {
            if (!target.HasPointerCapture(evt.pointerId))
                return;
            evt.StopImmediatePropagation();
            _onMove.Invoke(evt);
        }

        private void PointerUpHandler(PointerUpEvent evt)
        {
            if (target.HasPointerCapture(evt.pointerId))
            {
                target.ReleasePointer(evt.pointerId);
                _onUp.Invoke(evt);
            }

            evt.StopPropagation();
        }
    }
}
