﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MotionData {

	private int listSize;
	private LinkedList<float> motionDataList = new LinkedList<float>();

	public MotionData(int listSize) {
		this.listSize = listSize;
	}

	// Add a new value in the list, pushes out old values if full
	public void addMotionData(float data) {

		if (motionDataList.Count >= listSize) {
			motionDataList.RemoveLast();
		}

		motionDataList.AddFirst (data);
	}

	// Get the average of all values in the list
	public float getAverage() {

		float average = 0f;

		foreach (float motionData in motionDataList) {
			average += motionData;
		}

		average = average / motionDataList.Count;
	}
}
