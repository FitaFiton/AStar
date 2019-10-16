using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Character {
    protected AStarNavigator _navigator;

	protected float _nextPointDistance;
	protected int _nextPoint;
	protected float _lastRepath = float.NegativeInfinity;
    protected List<Vector2> _path;

	public float RepathRate = .1f;

	protected override void Awake () {
		base.Awake ();
        _navigator = GetComponent<AStarNavigator> ();
	}

	protected override Vector2 GetMovement() {
		if (GameManager.Instance.Chests.Count == 0) {
			return Vector2.zero;
		}

		if (Time.time > _lastRepath + RepathRate) {
			// Localizar el cofre más cercano
			float minDist = Mathf.Infinity;
			GameObject closest = null;

			foreach (var chest in GameManager.Instance.Chests.Values) {
				if (chest && Vector3.Distance (transform.position, chest.transform.position) < minDist) {
					minDist = Vector3.Distance (transform.position, chest.transform.position);
					closest = chest;
				}
			}
			if (closest) {
				_lastRepath = Time.time;
				_path = _navigator.GetPath(transform.position, closest.transform.position);
                _nextPoint = 0;

            }
		}

        if (_path == null) {
			return Vector2.zero;
		}

		if (_nextPoint > _path.Count) {
			return Vector2.zero;
		}

		if (_nextPoint == _path.Count) {
			_nextPoint++;
			return Vector2.zero;
		}

		var nextNodePosition = _path [_nextPoint];
		Vector2 direction = new Vector2 (nextNodePosition.x - transform.position.x, nextNodePosition.y - transform.position.y).normalized;
		Direction = direction;

		if (Vector3.Distance (transform.position, _path [_nextPoint]) < .1f) {
			_nextPoint++;
		}

		// Nos movemos a él
		return direction;
	}

	public Vector2 Direction;
}
