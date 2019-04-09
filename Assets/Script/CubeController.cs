using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CubeController : MonoBehaviour {

	//'Life Control' Class
	public class LifeClass
	{
		public int init;
		public int second;

		public LifeClass(int init, int sec)
		{
			this.init = init;
			second = sec;
		}

		public int GetInit() {
			return init;
		}

		public int GetSec() {
			return second;
		}

		public void SomaInit() {
			this.init++;
		}

		public void SubSec() {
			second--;
		}
	}

	//Moviment Class
	public class MovClass {

		public float mAngle;
		public float Radius ;
		public float mRotateSpeed; //AngularSpeed //2*Pi*f or 2*pi/T// [Diretion]
		public int mRotationZ = 5;
		public Vector2 mVector;
		public Quaternion target = Quaternion.Euler(0, 0, 0);

		public MovClass() { }

		public MovClass(float Angle, float Radius, float RotateSpeed, Vector2 vector) {
			mAngle = Angle;
			this.Radius = Radius;
			this.mRotateSpeed = RotateSpeed;
			mVector = vector;
		}

		public MovClass(float Radius, float RotateSpeed) {
			this.Radius = Radius;
			this.mRotateSpeed = RotateSpeed;
		}
	
		public int MoreFast() {
			mRotationZ += 3;
			return mRotationZ;
		}

		public void Reset() {
			mRotationZ = 0;
		}

		public Quaternion ResetZ() {
			return this.target;
		}

	}

	//Class' Objects
	public LifeClass myLife = new LifeClass(15, 6);
	public MovClass myMov1 = new MovClass(4f, 4f); 
	public MovClass myMov2 = new MovClass(4f, -4f); 

	//3D Objects in scene
	public GameObject myCubeRed, myCube1, myCube2;

	//Text
	public Text myTextInit;
	public Text myTextSec;

	//Buttons
	public Button myBtnInit;
	public Button myBtnSec;
	public Button myBtnMotion;
	public Button myBtnReset;

	//Controllers
	public int movZ = 5;

	//Original positions 3D
	Vector3 originalPosition;

	void Awake()
	{
		myBtnInit = GetComponent<Button>();
		myBtnSec = GetComponent<Button>();
		myBtnMotion = GetComponent<Button>();
		myBtnReset = GetComponent<Button>();

		originalPosition = myCubeRed.transform.localPosition;
	}

	void Start () {

		myBtnInit = GameObject.Find("myBtnInit").GetComponent<Button>();
		myBtnSec = GameObject.Find("myBtnSec").GetComponent<Button>();
		myBtnMotion = GameObject.Find("myBtnMotion").GetComponent<Button>();
		myBtnReset = GameObject.Find("myBtnReset").GetComponent<Button>();

		myBtnInit.onClick.AddListener(InitUpdate);
		myBtnSec.onClick.AddListener(SecUpdate);
		myBtnMotion.onClick.AddListener(MotionUpdate);
		myBtnReset.onClick.AddListener(Reset);

	}

	public void InitUpdate() {
		myLife.SomaInit();
	}

	public void SecUpdate() {
		myLife.SubSec();
	}

	public void MotionUpdate() {
		movZ += 5;  //Z-axis Red cube speed
		myMov1.MoreFast();
		myMov2.MoreFast();
	}

	public void Reset() {
		movZ = 5;
		myMov1.Reset();
		myMov2.Reset();

		///Quarternion access for the Z-axis rotation (inspectior) - black cubes;
		myCube1.transform.localRotation = myMov1.ResetZ();
		myCube2.transform.localRotation = myMov2.ResetZ();

		myCubeRed.transform.localPosition = originalPosition;
	}

	//unity3d.com/pt/learn/tutorials/topics/scripting/update-and-fixedupdate
	void FixedUpdate ()
	{
		Debug.Log("FixedUpdate time :" + Time.deltaTime);

		///Upate text calling the method;
			myTextInit.text = myLife.GetInit().ToString();
		///Calling directed
			myTextSec.text = myLife.second.ToString();
		
		///Red Cube 
			myCubeRed = GameObject.Find("CubeRed"); ///find object in scene - first in hirearchy;

		///rotate own axis;
		myCubeRed.transform.Rotate(0, 0, movZ);

		///Black Cube 1
			myCube1 = GameObject.Find("Cube1"); ///if s not referenced in scene
			myCube1.transform.Rotate(0, 0, -myMov1.mRotationZ); ///mRotationZ transform rotate in Z-axis;

			//Angular movement
			myMov1.mAngle += myMov1.mRotateSpeed * Time.deltaTime;  ///Angle updated by time;
			var offset = new Vector2(Mathf.Sin(myMov1.mAngle), Mathf.Cos(myMov1.mAngle)) * myMov1.Radius; ///MovimentClass set the radius;

			myCube1.transform.position = myMov1.mVector + offset; ///Apply the motion;

		//Black Cube 2
			myCube2 = GameObject.Find("Cube2");
			myCube2.transform.Rotate(0, 0, myMov2.mRotationZ); 
			myMov2.mAngle += myMov2.mRotateSpeed * Time.deltaTime;
			var offset2 = new Vector2(Mathf.Sin(myMov2.mAngle), Mathf.Cos(myMov2.mAngle)) * myMov2.Radius;
			myCube2.transform.position = myMov2.mVector + offset2; 

	}
}
